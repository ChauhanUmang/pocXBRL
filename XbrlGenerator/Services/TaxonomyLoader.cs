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
        public List<TaxonomyConcept> Load(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var concepts = new List<TaxonomyConcept>();

            foreach (var conceptElement in doc.Descendants("concept"))
            {
                var concept = new TaxonomyConcept
                {
                    Name = conceptElement.Attribute("name")?.Value ?? "",
                    DataType = conceptElement.Element("data_type")?.Value ?? "",
                    IsRequired = bool.Parse(conceptElement.Element("required")?.Value ?? "false"),
                    Validation = conceptElement.Element("validation")?.Value,
                    Calculation = conceptElement.Element("calculation")?.Value
                };
                concepts.Add(concept);
            }
            return concepts;
        }
    }
}