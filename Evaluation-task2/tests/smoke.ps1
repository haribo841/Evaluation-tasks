#Requires -Version 7.0
[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$projectFile = Join-Path $projectRoot 'Evaluation task 2.csproj'
dotnet build $projectFile -c $Configuration --nologo
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }

$application = Join-Path $projectRoot "bin/$Configuration/net7.0/Evaluation task 2.dll"
$testDirectory = Join-Path ([IO.Path]::GetTempPath()) ('csv aggregator smoke ' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $testDirectory | Out-Null
$inputPath = Join-Path $testDirectory 'sample input.csv'
Copy-Item -LiteralPath (Join-Path $projectRoot 'docs/examples/ingredients.csv') -Destination $inputPath
$inputHash = (Get-FileHash -LiteralPath $inputPath -Algorithm SHA256).Hash
$checks = 0

function Invoke-Aggregator {
    param([string[]]$Arguments = @())

    $startInfo = [Diagnostics.ProcessStartInfo]::new('dotnet')
    $startInfo.UseShellExecute = $false
    $startInfo.CreateNoWindow = $true
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true
    $startInfo.WorkingDirectory = $testDirectory
    $startInfo.ArgumentList.Add($application)
    foreach ($argument in $Arguments) { $startInfo.ArgumentList.Add($argument) }

    $process = [Diagnostics.Process]::Start($startInfo)
    try {
        $stdout = $process.StandardOutput.ReadToEndAsync()
        $stderr = $process.StandardError.ReadToEndAsync()
        if (-not $process.WaitForExit(10000)) {
            $process.Kill($true)
            throw 'The application exceeded the 10-second test timeout.'
        }
        [pscustomobject]@{
            ExitCode = $process.ExitCode
            Stdout = $stdout.GetAwaiter().GetResult()
            Stderr = $stderr.GetAwaiter().GetResult()
        }
    }
    finally { $process.Dispose() }
}

function Assert-Condition {
    param([bool]$Condition, [string]$Message)
    if (-not $Condition) { throw $Message }
}

$result = Invoke-Aggregator -Arguments @('--help')
Assert-Condition ($result.ExitCode -eq 0 -and $result.Stdout.Contains('Usage:')) 'Help must succeed and print usage.'
$checks++

$result = Invoke-Aggregator
Assert-Condition ($result.ExitCode -eq 2 -and $result.Stderr.Contains('Usage:')) 'Missing arguments must fail with usage.'
$checks++

$result = Invoke-Aggregator -Arguments @($inputPath)
Assert-Condition ($result.ExitCode -eq 2) 'A missing output argument must fail.'
$checks++

$result = Invoke-Aggregator -Arguments @($inputPath, $inputPath)
Assert-Condition ($result.ExitCode -eq 2) 'Input and output must not be the same path.'
Assert-Condition ((Get-FileHash -LiteralPath $inputPath -Algorithm SHA256).Hash -eq $inputHash) 'Input was modified.'
$checks++

$existingOutput = Join-Path $testDirectory 'existing.csv'
Copy-Item -LiteralPath $inputPath -Destination $existingOutput
$result = Invoke-Aggregator -Arguments @($inputPath, $existingOutput)
Assert-Condition ($result.ExitCode -eq 1) 'An existing output must be rejected.'
Assert-Condition ((Get-FileHash -LiteralPath $existingOutput -Algorithm SHA256).Hash -eq $inputHash) 'An existing output was overwritten.'
$checks++

$missingOutput = Join-Path $testDirectory 'missing-input-result.csv'
$result = Invoke-Aggregator -Arguments @((Join-Path $testDirectory 'missing-input.csv'), $missingOutput)
Assert-Condition ($result.ExitCode -eq 1 -and -not (Test-Path -LiteralPath $missingOutput)) 'A missing input must not create an output.'
$checks++

foreach ($invalidName in @('invalid-header', 'invalid-value')) {
    $invalidPath = Join-Path $PSScriptRoot "fixtures/$invalidName.csv"
    $invalidOutput = Join-Path $testDirectory "$invalidName-result.csv"
    $result = Invoke-Aggregator -Arguments @($invalidPath, $invalidOutput)
    Assert-Condition ($result.ExitCode -eq 1 -and -not (Test-Path -LiteralPath $invalidOutput)) "$invalidName must fail without creating output."
    Assert-Condition ($result.Stderr.Contains('Cannot process the CSV')) "$invalidName must report the failure on stderr."
    Assert-Condition (-not $result.Stderr.Contains('not-a-number')) 'Diagnostics must not echo raw CSV records.'
    $checks++
}

$result = Invoke-Aggregator -Arguments @($inputPath, (Join-Path $testDirectory 'missing-directory/output.csv'))
Assert-Condition ($result.ExitCode -eq 1) 'An unavailable output directory must return an error.'
$checks++

$outputPath = Join-Path $testDirectory 'hourly result.csv'
$result = Invoke-Aggregator -Arguments @($inputPath, $outputPath)
Assert-Condition ($result.ExitCode -eq 0 -and (Test-Path -LiteralPath $outputPath)) 'Valid input must produce output, including paths with spaces.'
Assert-Condition ($result.Stdout.Contains("2026-01-01 09:00:00`t0.25`t0.12`t0.25`t1")) 'Console output must preserve CSV precision and invariant decimal separators.'
$expectedRows = @(Import-Csv -LiteralPath (Join-Path $projectRoot 'docs/examples/expected-hourly.csv'))
$actualRows = @(Import-Csv -LiteralPath $outputPath)
Assert-Condition ($actualRows.Count -eq $expectedRows.Count) 'Unexpected output row count.'
for ($row = 0; $row -lt $expectedRows.Count; $row++) {
    foreach ($column in @('TIMESTAMP', 'FLOUR', 'GROAT', 'MILK', 'EGG')) {
        Assert-Condition ($actualRows[$row].$column -ceq $expectedRows[$row].$column) "Row $row, $column differs from the verified example."
    }
}
Assert-Condition ((Get-FileHash -LiteralPath $inputPath -Algorithm SHA256).Hash -eq $inputHash) 'Valid processing must not modify input.'
$checks++

$emptyOutput = Join-Path $testDirectory 'header-only-result.csv'
$result = Invoke-Aggregator -Arguments @((Join-Path $PSScriptRoot 'fixtures/header-only.csv'), $emptyOutput)
Assert-Condition ($result.ExitCode -eq 0) 'A header-only input must succeed.'
Assert-Condition ((Get-Content -LiteralPath $emptyOutput -Raw).Trim() -ceq 'TIMESTAMP,FLOUR,GROAT,MILK,EGG') 'A header-only input must produce only the output header.'
$checks++

Write-Output "PASS: $checks CLI smoke checks."
Write-Output "Synthetic test files retained for inspection: $testDirectory"
