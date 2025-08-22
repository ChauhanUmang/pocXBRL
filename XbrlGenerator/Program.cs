using XbrlGenerator.Models;
using XbrlGenerator.Services;

Console.WriteLine("Starting XBRL Report Generation...");

string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
string inputDirectory = Path.Combine(baseDirectory, "InputFiles");

string taxonomyPath = Path.Combine(inputDirectory, "Taxonomy.xml");
string dataPath = Path.Combine(inputDirectory, "data.csv");
string templatePath = Path.Combine(inputDirectory, "template.html");

string standaloneReportPath = Path.Combine(baseDirectory, "GeneratedReport.html");
string outputPath = Path.Combine(baseDirectory, "ReportPackage.zip");

var taxonomyLoader = new TaxonomyLoader();
var csvReader = new CsvDataReader();
var calculationEngine = new CalculationEngine();
var validationEngine = new ValidationEngine();
var reportRenderer = new ReportRenderer();
var packageGenerator = new PackageGenerator();

try
{
    // --- Execute the Workflow ---

    // Load data
    Console.WriteLine("Loading taxonomy and data...");
    List<TaxonomyConcept> concepts = taxonomyLoader.Load(taxonomyPath);
    List<FinancialData> rawData = csvReader.Read(dataPath);

    // Convert list to a dictionary for easier lookup
    var financialDataDict = rawData.ToDictionary(d => d.Concept, d => d.Value);

    // Process calculations
    Console.WriteLine("Processing calculations...");
    calculationEngine.ProcessCalculations(concepts, financialDataDict);

    // Validate data
    Console.WriteLine("Validating data...");
    List<string> validationErrors = validationEngine.Validate(concepts, financialDataDict);

    if (validationErrors.Any())
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Validation Failed:");
        validationErrors.ForEach(Console.WriteLine);
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Validation Successful.");
        Console.ResetColor();
    }

    // Render report
    Console.WriteLine("Rendering HTML report...");
    string htmlReport = reportRenderer.Render(templatePath, financialDataDict);

    File.WriteAllText(standaloneReportPath, htmlReport);
    Console.WriteLine($"Standalone HTML report saved to: {standaloneReportPath}");

    // Create final package
    Console.WriteLine("Creating report package...");
    packageGenerator.CreatePackage(htmlReport, validationErrors, dataPath, taxonomyPath, outputPath);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
    Console.ResetColor();
}

Console.WriteLine("Generation process complete.");

