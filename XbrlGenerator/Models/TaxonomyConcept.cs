namespace XbrlGenerator.Models;

public class TaxonomyConcept
{
    public string Name { get; set; } = "";
    public string DataType { get; set; } = "";
    public bool IsRequired { get; set; }
    public string? Validation { get; set; }
    public string? Calculation { get; set; }

    public string XbrlName { get; set; } = "";
    //public string DataType { get; set; } = "";
    public string ContextRef { get; set; } = "";
    public string UnitRef { get; set; } = "";
    public int Decimals { get; set; }
}