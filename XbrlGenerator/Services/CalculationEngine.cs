using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XbrlGenerator.Models;

namespace XbrlGenerator.Services
{
    public class CalculationEngine
    {
        public void ProcessCalculations(List<TaxonomyConcept> concepts, Dictionary<string, decimal> financialData)
        {
            var conceptsWithCalculations = concepts.Where(c => !string.IsNullOrEmpty(c.Calculation)).ToList();

            foreach (var concept in conceptsWithCalculations)
            {
                if (concept.Calculation != null)
                {
                    // Assuming the calculation is in the format "operandA operator operandB"
                    // e.g., "Revenue - Expenses"
                    var parts = concept.Calculation.Split(' ').Select(p => p.Trim()).ToArray();
                    if (parts.Length == 3)
                    {
                        var operandA_Name = parts[0];
                        var operatorSymbol = parts[1];
                        var operandB_Name = parts[2];

                        if (financialData.TryGetValue(operandA_Name, out var valA) &&
                            financialData.TryGetValue(operandB_Name, out var valB))
                        {
                            decimal result = 0;
                            if (operatorSymbol == "-")
                            {
                                result = valA - valB;
                            }
                            // Add more operators like +, *, / as needed.

                            financialData[concept.Name] = result;
                            Console.WriteLine($"Calculated {concept.Name}: {result}");
                        }
                    }
                }
            }
        }
    }
}