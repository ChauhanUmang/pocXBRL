using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using XbrlGenerator.Models;

namespace XbrlGenerator.Services
{
    public class TaxonomyLoader
    {
        public (ReportInfo, List<TaxonomyConcept>) Load(string filePath)
        {
            var doc = XDocument.Load(filePath);

            var reportElement = doc.Descendants("report").First();
            var reportInfo = new ReportInfo
            {
                Namespace = new ReportNamespace
                {
                    Prefix = reportElement.Element("namespace")?.Attribute("prefix")?.Value ?? "",
                    Uri = reportElement.Element("namespace")?.Value ?? ""
                },
                Units = reportElement.Descendants("unit").Select(u => new ReportUnit
                {
                    Id = u.Attribute("id")?.Value ?? "",
                    Measure = u.Value
                }).ToList(),
                Contexts = reportElement.Descendants("context").Select(c => new ReportContext
                {
                    Id = c.Attribute("id")?.Value ?? "",
                    EntityScheme = c.Element("entity")?.Element("identifier")?.Attribute("scheme")?.Value ?? "",
                    EntityIdentifier = c.Element("entity")?.Element("identifier")?.Value ?? "",
                    Instant = DateTime.Parse(c.Element("period")?.Element("instant")?.Value ?? "")
                }).ToList()
            };

            // Load Concepts
            var concepts = doc.Descendants("concept").Select(c => new TaxonomyConcept
            {
                Name = c.Attribute("name")?.Value ?? "",
                XbrlName = c.Attribute("xbrlName")?.Value ?? "",
                DataType = c.Attribute("dataType")?.Value ?? "",
                ContextRef = c.Attribute("contextRef")?.Value ?? "",
                UnitRef = c.Attribute("unitRef")?.Value ?? "",
                Decimals = int.Parse(c.Attribute("decimals")?.Value ?? "0"),
                Validation = c.Element("validation")?.Value,
                Calculation = c.Element("calculation")?.Value
            }).ToList();

            return (reportInfo, concepts);
        }
    }
}