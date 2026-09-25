# CSV Ingredient Aggregator

A compact C# console case study that reads ingredient-use records from CSV, normalizes units, aggregates values by hour, and writes a CSV result. It is a learning project, not a production food-inventory system.

[Source code](https://github.com/haribo841/Evaluation-tasks) | [Input and output reference](docs/USAGE.md) | [Report an issue](https://github.com/haribo841/Evaluation-tasks/issues)

## What it does

- Reads timestamped flour, groat, milk, and egg records with CsvHelper.
- Groups records by hour.
- Converts decagrams to kilograms, grams to kilograms, and millilitres to litres.
- Prints the aggregate to the console and exports an ordered CSV file.
- Takes input and output paths as command-line arguments and refuses to overwrite an existing output file.

## Quick start

There is no binary release. Install a .NET SDK that can build `net7.0` and the .NET 7 runtime to execute this historical project. Its target framework is out of support; a framework upgrade is separate from this presentation update.

1. Clone the repository.

   ```powershell
   git clone https://github.com/haribo841/Evaluation-tasks.git
   cd Evaluation-tasks
   ```

2. Run the included, fictional [three-record example](docs/examples/ingredients.csv):

   ```powershell
   dotnet run --project "Evaluation task 2.csproj" -- "docs/examples/ingredients.csv" "hourly-result.csv"
   ```

3. Open `hourly-result.csv` and compare it with the [expected output](docs/examples/expected-hourly.csv). To run it again, choose a different output name. No source editing is required.

The primary project now excludes `Evaluation task 3/` automatically. That folder remains an independent exercise.

## Example result

The input deliberately contains a 09:05 record before two records from 08:00-08:59. The output combines the earlier hour and sorts both rows:

```csv
TIMESTAMP,FLOUR,GROAT,MILK,EGG
01/01/2026 08:00:00,1,1,1,3
01/01/2026 09:00:00,0.25,0.12,0.25,1
```

`FLOUR` and `GROAT` are now in kilograms, `MILK` in litres, and `EGG` is a count. The 125 g groat value becomes `0.12` kg because the existing algorithm uses .NET's default midpoint-to-even rounding to two decimal places. This behavior is documented and covered by the example comparison.

## Supported environment

| Environment | Support |
| --- | --- |
| Windows | Verified with .NET SDK 10.0.302 and .NET runtime 7.0.20. |
| Linux and macOS | Paths are supplied at runtime; execution on these platforms has not been verified. |
| Technology | C#, .NET 7 console application, CsvHelper 30.0.1. |

## Checks

With PowerShell 7 available, run:

```powershell
pwsh -File tests/smoke.ps1
```

The script builds Release and runs 11 CLI checks, including the example result, invalid input, missing arguments, paths containing spaces, refusal to overwrite files, and header-only input. It uses fictional data in a new temporary directory and keeps that directory for inspection. These are focused regression checks, not a full test suite for every input value.

## Documentation

See the [input and output reference](docs/USAGE.md) for the CSV schema, processing diagram, unit conversions, exit codes, current limitations, and a source map.

## License and issues

No license file is currently published. For questions or reproducible defects, use [GitHub Issues](https://github.com/haribo841/Evaluation-tasks/issues).

The previous class-by-class description is preserved in [the README archive](docs/archive/README-2026-09-06.md).
