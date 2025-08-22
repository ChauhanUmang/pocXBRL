using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace XbrlGenerator.Services
{
    public class PackageGenerator
    {
        public void CreatePackage(
        string renderedHtml,
        List<string> validationLog,
        string dataCsvPath,
        string taxonomyXmlPath,
        string outputZipPath)
        {
            // 1. Create a temporary staging directory
            string stagingDir = Path.Combine(Path.GetTempPath(), "xbrl_package_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(stagingDir);

            try
            {
                // 2. Write generated files to staging
                File.WriteAllText(Path.Combine(stagingDir, "report.html"), renderedHtml);

                var logContent = validationLog.Count == 0 ? "Validation Successful." : string.Join("\n", validationLog);
                File.WriteAllText(Path.Combine(stagingDir, "validation_log.txt"), logContent);

                // 3. Copy source files to staging
                File.Copy(dataCsvPath, Path.Combine(stagingDir, Path.GetFileName(dataCsvPath)));
                File.Copy(taxonomyXmlPath, Path.Combine(stagingDir, Path.GetFileName(taxonomyXmlPath)));

                // 4. Create the zip file
                if (File.Exists(outputZipPath))
                {
                    File.Delete(outputZipPath);
                }
                ZipFile.CreateFromDirectory(stagingDir, outputZipPath);

                Console.WriteLine($"Successfully created report package at: {outputZipPath}");
            }
            finally
            {
                // 5. Clean up the temporary directory
                if (Directory.Exists(stagingDir))
                {
                    Directory.Delete(stagingDir, true);
                }
            }
        }
    }
}