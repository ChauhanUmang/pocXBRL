namespace XbrlGenerator.Models;

public class TaxonomyConcept
{
    public string Name { get; set; } = "";
    public string DataType { get; set; } = "";
    public bool IsRequired { get; set; }
    public string? Validation { get; set; }
    public string? Calculation { get; set; }
}