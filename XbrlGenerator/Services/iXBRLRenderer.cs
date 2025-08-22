using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using XbrlGenerator.Models;

namespace XbrlGenerator.Services
{
    public class iXBRLRenderer
    {
        public string Render(string templatePath, Dictionary<string, decimal> financialData, List<TaxonomyConcept> concepts, ReportInfo reportInfo)
        {
            var templateContent = File.ReadAllText(templatePath);

            // Extract style from template to preserve it
            var styleContent = "";
            var match = Regex.Match(templateContent, @"<style>([\s\S]*?)</style>");
            if (match.Success)
            {
                styleContent = match.Groups[1].Value;
            }

            // 1. Add required xmlns attributes to the <html> tag
            templateContent = templateContent.Replace(
                "<html lang=\"en\">",
                $"<html lang=\"en\" xmlns:ix=\"http://www.xbrl.org/2013/inlineXBRL\" xmlns:xbrli=\"http://www.xbrl.org/2003/instance\" xmlns:ixt=\"http://www.xbrl.org/2013/inlineXBRL-transformation\" xmlns:{reportInfo.Namespace.Prefix}=\"{reportInfo.Namespace.Uri}\">"
            );

            // 2. Create the new iXBRL head element
            var ixNS = XNamespace.Get("http://www.xbrl.org/2013/inlineXBRL");
            var xbrliNS = XNamespace.Get("http://www.xbrl.org/2003/instance");

            var newHead = new XElement("head",
                new XElement("meta", new XAttribute("charset", "UTF-8")),
                new XElement("title", "iXBRL Report"),
                new XElement("style", new XText(styleContent)), // Add preserved styles
                new XElement(ixNS + "header",
                    new XElement(ixNS + "resources",
                        // Add contexts from ReportInfo
                        reportInfo.Contexts.Select(c =>
                            new XElement(xbrliNS + "context",
                                new XAttribute("id", c.Id),
                                new XElement(xbrliNS + "entity",
                                    new XElement(xbrliNS + "identifier",
                                        new XAttribute("scheme", c.EntityScheme),
                                        c.EntityIdentifier
                                    )
                                ),
                                new XElement(xbrliNS + "period",
                                    new XElement(xbrliNS + "instant", c.Instant.ToString("yyyy-MM-dd"))
                                )
                            )
                        ),
                        // Add units from ReportInfo
                        reportInfo.Units.Select(u =>
                            new XElement(xbrliNS + "unit",
                                new XAttribute("id", u.Id),
                                new XElement(xbrliNS + "measure", u.Measure)
                            )
                        )
                    )
                )
            );

            // 3. Replace the old head with the new one
            var headRegex = new Regex(@"<head>[\s\S]*?</head>");
            templateContent = headRegex.Replace(templateContent, newHead.ToString(SaveOptions.DisableFormatting), 1);


            // 4. Replace placeholders in the body with iXBRL tags
            foreach (var concept in concepts.Where(c => financialData.ContainsKey(c.Name)))
            {
                var value = financialData[concept.Name];

                var ixbrlTag = new XElement(ixNS + "nonFraction",
                    new XAttribute("name", concept.XbrlName),
                    new XAttribute("contextRef", concept.ContextRef),
                    new XAttribute("unitRef", concept.UnitRef),
                    new XAttribute("decimals", concept.Decimals),
                    new XAttribute("format", "ixt:num-dot-decimal"),
                    value.ToString("F" + concept.Decimals) // Format number to required decimal places
                );

                templateContent = templateContent.Replace($"{{{{{concept.Name}}}}}", ixbrlTag.ToString(SaveOptions.DisableFormatting));
            }

            return templateContent;
        }
    }
}