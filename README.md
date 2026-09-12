# CSV Ingredient Aggregator

A compact C# console case study that reads ingredient-use records from CSV, normalizes units, aggregates values by hour, and writes a clean CSV result.

[Source code](https://github.com/haribo841/Evaluation-task-2) | [Input and output reference](docs/USAGE.md) | [Report an issue](https://github.com/haribo841/Evaluation-task-2/issues)

## What it does

- Reads timestamped flour, groat, milk, and egg records with CsvHelper.
- Groups records by hour.
- Converts decagrams to kilograms, grams to kilograms, and millilitres to litres.
- Prints the aggregate to the console and exports an ordered CSV file.

## Quick start

There is no binary release. The primary project targets .NET 7.

1. Clone the repository.

   ```powershell
   git clone https://github.com/haribo841/Evaluation-task-2.git
   cd Evaluation-task-2
   ```

2. Create an input CSV described in the [input and output reference](docs/USAGE.md).
3. Update the `filePath` and `outputFilePath` values in `Program.cs` to point to your local files.
4. Build the primary task. The command excludes the separate nested `Evaluation task 3` source tree, which the current project otherwise picks up automatically.

   ```powershell
   dotnet build "Evaluation task 2.csproj" -p:DefaultExcludesInProjectFolder="Evaluation task 3/**"
   ```

5. Run the built processor.

   ```powershell
   dotnet ".\bin\Debug\net7.0\Evaluation task 2.dll"
   ```

## Supported environment

| Environment | Support |
| --- | --- |
| Windows | Supported in the current configuration because the program uses absolute Windows file paths. |
| Linux and macOS | Supported by .NET after replacing the Windows-specific paths in `Program.cs`. |
| .NET 7 SDK | Required by the primary project; use the documented build command while the nested task remains in this source tree. |

## Documentation

See the [input and output reference](docs/USAGE.md) for the CSV schema, unit conversions, current limitations, and a source map.

## License and issues

No license file is currently published. For questions or reproducible defects, use [GitHub Issues](https://github.com/haribo841/Evaluation-task-2/issues).

The previous class-by-class description is preserved in [the README archive](docs/archive/README-2026-09-06.md).
