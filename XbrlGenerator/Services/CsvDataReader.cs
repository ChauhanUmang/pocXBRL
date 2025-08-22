using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XbrlGenerator.Models;

namespace XbrlGenerator.Services
{
    public class CsvDataReader
    {
        public List<FinancialData> Read(string filePath)
        {
            var data = new List<FinancialData>();
            var lines = File.ReadAllLines(filePath).Skip(1);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2)
                {
                    var financialData = new FinancialData
                    {
                        Concept = parts[0].Trim(),
                        Value = decimal.Parse(parts[1].Trim())
                    };
                    data.Add(financialData);
                }
            }

            return data;
        }
    }
}