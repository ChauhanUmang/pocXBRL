using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XbrlGenerator.Services
{
    public class ReportRenderer
    {
        public string Render(string templatePath, Dictionary<string, decimal> financialData)
        {
            var templateContent = File.ReadAllText(templatePath);

            foreach (var entry in financialData)
            {
                // Format the value as a currency string
                string formattedValue = entry.Value.ToString("C"); // "C" for currency
                templateContent = templateContent.Replace($"{{{{{entry.Key}}}}}", formattedValue);
            }

            return templateContent;
        }
    }
}