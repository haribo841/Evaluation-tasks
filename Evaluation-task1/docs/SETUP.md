# Setup guide

The project is now in `Evaluation-task1`. Run the commands below from that folder (`cd Evaluation-task1` from the repository root). The shared solution and active GitHub Actions workflow live at the repository root.

## Prerequisites

- .NET 7 SDK, matching the target framework in the project file.
- A local SQL Server or SQL Server Express instance.
- The Entity Framework command-line tool when database migrations are needed.

This is a historical .NET 7 project. Use an isolated development environment and assess the framework and package versions before any real deployment.

## Configure the database

Set a dedicated local connection string through user secrets or an environment variable. Do not place passwords or production endpoints in appsettings.json or source control.

~~~powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=WebApplication1;Trusted_Connection=True;TrustServerCertificate=True"
~~~

An environment-variable alternative is ConnectionStrings__DefaultConnection.

When the matching Entity Framework CLI is available, apply the included migration:

~~~powershell
dotnet ef database update
~~~

Then start the application with:

~~~powershell
dotnet run
~~~

## Data behavior

Identity data uses the configured SQL Server database. The UserXmlService additionally writes user records to users.xml in the process working directory when a record is created, updated, or deleted.

The XML file can contain personally identifiable information. Keep it local, restrict access, and do not add it to source control or issue reports.

## Authentication behavior

The current Identity configuration requires confirmed accounts to sign in. This repository does not provide a production email-confirmation setup, so configure and test that flow only in a safe development environment.

Authentication scaffolding does not mean every data route is protected. The `HomeController` XML actions do not have an authorization attribute, and authorization coverage differs between controllers. Use loopback-only access and fictional records. Do not deploy this historical exercise as an authenticated personnel system.

## Isolated UI preview

The README screenshot was captured from the actual MVC application using the [fictional XML fixture](examples/users.sample.xml). The list and its `Home` routes do not query the Identity database. Login and registration still require a configured SQL Server instance and are not part of this preview.

Run the following in PowerShell from the repository root. It builds the existing project, creates a new temporary data directory, and uses a deliberately unreachable SQL endpoint so no existing database is contacted. The .NET 7 ASP.NET Core runtime must be installed, even when building with a newer SDK.

```powershell
$projectRoot = (Get-Location).Path
dotnet build .\WebApplication1.csproj -c Release
if ($LASTEXITCODE -ne 0) { throw 'Build failed' }
$previewDir = Join-Path ([IO.Path]::GetTempPath()) ('xml-crud-preview-' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $previewDir | Out-Null
Copy-Item .\docs\examples\users.sample.xml (Join-Path $previewDir 'users.xml')
$previousConnection = $env:ConnectionStrings__DefaultConnection
$previousEnvironment = $env:ASPNETCORE_ENVIRONMENT
Push-Location $previewDir
try {
    $env:ASPNETCORE_ENVIRONMENT = 'Production'
    $env:ConnectionStrings__DefaultConnection = 'Server=127.0.0.1,1;Database=PortfolioPreview;Integrated Security=True;Connect Timeout=1;TrustServerCertificate=True'
    dotnet "$projectRoot\bin\Release\net7.0\WebApplication1.dll" --urls http://127.0.0.1:54127 --contentRoot "$projectRoot"
}
finally {
    Pop-Location
    $env:ConnectionStrings__DefaultConnection = $previousConnection
    $env:ASPNETCORE_ENVIRONMENT = $previousEnvironment
}
```

Open `http://127.0.0.1:54127` and stop the preview with Ctrl+C. The temporary XML file is kept for inspection; your original working-directory data is not overwritten. `Production` here selects the normal error page and avoids loading personal development secrets; it does not make this application production-ready.

Validated on Windows on 2026-10-02: Release build, XML-backed list rendering, create/edit form loading, and Bootstrap stylesheet delivery. The sample XML and original local data remained unchanged. Existing compiler warnings and the unsupported framework warning remain. SQL migrations, account confirmation, and a full CRUD regression suite have not been validated in this presentation pass.
