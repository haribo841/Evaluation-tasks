# Setup guide

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
