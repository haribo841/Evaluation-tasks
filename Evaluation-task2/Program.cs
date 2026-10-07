using System;
using CsvHelper;
using System.Globalization;
using System.Collections.Generic;
using CsvHelper.Configuration;

namespace Evaluation_task_2
{
    class Program
    {
        static int Main(string[] args)
        {
            const string usage = "Usage: dotnet \"Evaluation task 2.dll\" <input.csv> <output.csv>\n"
                + "The output file must not exist. Use --help to show this message.";

            if (args.Length == 1 && (args[0] == "--help" || args[0] == "-h"))
            {
                Console.WriteLine(usage);
                return 0;
            }

            if (args.Length != 2 || args.Any(string.IsNullOrWhiteSpace))
            {
                Console.Error.WriteLine(usage);
                return 2;
            }

            try
            {
                string filePath = Path.GetFullPath(args[0]);
                string outputFilePath = Path.GetFullPath(args[1]);
                StringComparison pathComparison = OperatingSystem.IsWindows()
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal;

                if (string.Equals(filePath, outputFilePath, pathComparison))
                {
                    Console.Error.WriteLine("Input and output paths must be different.");
                    return 2;
                }

                if (File.Exists(outputFilePath))
                {
                    Console.Error.WriteLine("The output file already exists. Choose a new path.");
                    return 1;
                }

                ICsvReader csvReader = new CsvDataReader();
                DataService dataService = new(csvReader, new PancakeProcessor(), new DataAnalyzer());
                List<IngredientUsage>? result = dataService.ProcessCsvFile(filePath, csvReader);
                if (result is null)
                {
                    return 1;
                }

                using (FileStream output = new(outputFilePath, FileMode.CreateNew, FileAccess.Write))
                using (StreamWriter writer = new(output))
                using (CsvWriter csv = new(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    SumResult sumResults = new();
                    IEnumerable<IngredientUsage> summedResults =
                        (IEnumerable<IngredientUsage>)sumResults.SumResults(result);
                    csv.WriteRecords(summedResults);
                }

                Console.WriteLine($"Wyniki zostały zapisane do pliku: {outputFilePath}");
                return 0;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException
                or ArgumentException or NotSupportedException)
            {
                Console.Error.WriteLine($"Cannot read or write the requested files ({ex.GetType().Name}).");
                return 1;
            }
        }
    }
}
