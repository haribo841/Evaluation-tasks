# WebApplication1 — ASP.NET Core recruitment exercise

An ASP.NET Core MVC recruitment exercise with authentication scaffolding and simple user-record CRUD flows. Application records are serialized to a local XML file; this is a learning project, not a production-ready identity or personnel-management system.

## What it demonstrates

- ASP.NET Core MVC controllers, Razor views, and routing.
- ASP.NET Core Identity backed by Entity Framework Core and SQL Server.
- Model validation for user data.
- XML-based read, create, update, and delete operations through UserXmlService.

## Quick start

Requirements: .NET 7 SDK and a local SQL Server instance accessible to the development account.

~~~powershell
dotnet restore
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=WebApplication1;Trusted_Connection=True;TrustServerCertificate=True"
dotnet ef database update
dotnet run
~~~

The migration command requires the matching Entity Framework CLI to be available. See [the setup guide](docs/SETUP.md) for configuration details and data-handling notes.

## Supported platform

- ASP.NET Core targeting .NET 7.
- SQL Server through Entity Framework Core.
- A modern desktop browser for the MVC interface.

## Important limitations

The application requires confirmed accounts through its current Identity configuration. It also writes application user records to users.xml in the working directory once data is created. Use only non-sensitive development data, protect the database connection string with user secrets or environment variables, and do not commit generated XML data.

The checked-in development configuration should be overridden locally with a dedicated development database; do not use a system database for application data.

## Documentation, license, and support

- [Setup guide](docs/SETUP.md)
- [Previous README archive](docs/archive/README-2026-09-16.md)
- No license file is currently included. Ask the author before reuse.
- Report a reproducible issue without including database strings, accounts, or user data.
