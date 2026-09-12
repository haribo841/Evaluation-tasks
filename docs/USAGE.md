# CSV input and output reference

## Input CSV

The processor expects a comma-separated CSV file with this exact, case-sensitive header row:

```csv
TIMESTAMP,FLOUR,GROAT,MILK,EGG
2026-01-01T08:15:00,50,125,250,2
```

All five fields are required. `TIMESTAMP` must be parseable as a .NET `DateTime`; `FLOUR` and `EGG` are integers, while `GROAT` and `MILK` accept decimal values. Values are read using `InvariantCulture`, so use a period (`.`) as the decimal separator.

The conversion rules are:

| Input column | Interpreted unit | Output representation |
| --- | --- | --- |
| `FLOUR` | decagrams | kilograms; no explicit rounding in the source |
| `GROAT` | grams | kilograms, rounded to two decimal places |
| `MILK` | millilitres | litres, rounded to two decimal places |
| `EGG` | count | count |

Records that fall in the same hour are summed. Their timestamp is rounded down to the beginning of that hour, and the final rows are sorted by `TIMESTAMP`.

## Output CSV

The program writes a CSV file with the same five headers, now holding the hourly aggregate in the output units:

```csv
TIMESTAMP,FLOUR,GROAT,MILK,EGG
01/01/2026 08:00:00,1,1,1,3
```

The example represents two records from 08:00–08:59 with a combined 100 decagrams of flour, 1,000 grams of groat, 1,000 millilitres of milk, and three eggs. CsvHelper serializes the timestamp with `InvariantCulture`; the value itself is always the start of the aggregated hour.

## Current source configuration

`Program.cs` currently contains absolute Windows paths for the input and output files. Update `filePath` and `outputFilePath` before running the application outside the original development machine.

The repository also contains an independent task in `Evaluation task 3/`. Build the primary project with the command in the README so that the SDK does not include that nested source tree in this application's compilation.

When the input cannot be processed, the application writes an error message to the console. Correct the source data and rerun it rather than treating an empty output file as a successful aggregate.

## Project layout

- `CsvDataReader.cs` reads the input data with CsvHelper.
- `DataService.cs` orchestrates loading, grouping, and conversion.
- `PancakeProcessor.cs` performs unit conversion and aggregation.
- `SumResults.cs` applies the final hourly sum and ordering.
- `DataAnalyzer.cs` prints the result to the console.
