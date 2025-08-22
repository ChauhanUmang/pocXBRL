using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XbrlGenerator.Models;

namespace XbrlGenerator.Services
{
    public class ValidationEngine
    {
        public List<string> Validate(List<TaxonomyConcept> concepts, Dictionary<string, decimal> financialData)
        {
            var errors = new List<string>();

            foreach (var concept in concepts)
            {
                // Rule 1: Check for required concepts
                if (concept.IsRequired && !financialData.ContainsKey(concept.Name))
                {
                    errors.Add($"Validation Error: Required concept '{concept.Name}' is missing.");
                    continue; // No further validation needed if it's missing
                }

                // Rule 2: Perform validation checks
                if (!string.IsNullOrEmpty(concept.Validation) && financialData.TryGetValue(concept.Name, out var value))
                {
                    switch (concept.Validation)
                    {
                        case "must_be_positive":
                            if (value < 0)
                            {
                                errors.Add($"Validation Error: Concept '{concept.Name}' must be positive, but was {value}.");
                            }
                            break;
                            // Add other validation cases here as needed
                    }
                }
            }

            return errors;
        }
    }
}