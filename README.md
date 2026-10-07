# Evaluation tasks

Trzy historyczne zadania ewaluacyjne w C#. Każde ma osobny katalog, projekt i instrukcję. Zadanie ASP.NET Core XML CRUD zostało przeniesione z repozytorium `aspnetcore-xml-crud` jako zadanie 1. Repozytorium źródłowe zostanie wykorzystane dla aplikacji Kolejka transkrypcji.

| Zadanie | Zawartość | Framework | Instrukcja |
| --- | --- | --- | --- |
| [Evaluation-task1](Evaluation-task1) | ASP.NET Core MVC, Identity, SQL Server i CRUD rekordów przechowywanych w XML | .NET 7 | [README](Evaluation-task1/README.md), [konfiguracja i lokalny podgląd](Evaluation-task1/docs/SETUP.md) |
| [Evaluation-task2](Evaluation-task2) | Wczytywanie CSV, normalizacja jednostek składników i agregacja godzinowa | .NET 7 | [README](Evaluation-task2/README.md), [format danych](Evaluation-task2/docs/USAGE.md) |
| [Evaluation-task3](Evaluation-task3) | Nieukończony prototyp przedziałów raportu HR na podstawie kontraktów | .NET 9 | [README](Evaluation-task3/README.md) |

## Budowanie

Z katalogu głównego repozytorium:

```powershell
dotnet restore Evaluation-tasks.sln
dotnet build Evaluation-tasks.sln -c Release --no-restore
```

Solution obejmuje trzy niezależne projekty. Do uruchamiania zadań 1 i 2 potrzebny jest runtime .NET 7, a zadania 3 runtime .NET 9. Budowanie sprawdzono lokalnie z SDK 10.0.302. Zadania zachowują swoje frameworki i istniejące ograniczenia; ta migracja nie stanowi modernizacji aplikacji.

## Uruchamianie i sprawdzanie

Zadanie 1 uruchamiaj z jego własnego katalogu, ponieważ XML i raporty zapisuje względem katalogu roboczego:

```powershell
Set-Location Evaluation-task1
dotnet run --project WebApplication1.csproj
```

Najpierw wykonaj [konfigurację SQL Server i Identity](Evaluation-task1/docs/SETUP.md). Dostępny jest także odseparowany podgląd na fikcyjnych danych, bez połączenia z rzeczywistą bazą. Historyczny projekt wymaga przeglądu bezpieczeństwa przed udostępnianiem poza lokalnym środowiskiem.

Zadanie 2 zawiera przykładowe dane i testy uruchomienia. Z katalogu głównego:

```powershell
pwsh -File Evaluation-task2/tests/smoke.ps1
```

Test buduje projekt Release i sprawdza 11 scenariuszy CLI, w tym poprawność przykładu, zachowanie wejścia i odmowę nadpisania wyniku.

Zadanie 3 jest zachowanym prototypem. Dodano brakującą zależność CsvHelper, aby można było zbudować projekt. Nadal ma ścieżki wejścia i wyjścia wpisane w `Program.cs`; jego działanie nie zostało potwierdzone jako kompletnego generatora raportów HR. Nie używaj rzeczywistych danych do prób.

## Historia i licencje

Połączenie dołącza historię CRUD-a jako drugi rodzic commita migracji, bez przepisywania wcześniejszych commitów. Szczegóły źródeł i zakres zmian opisuje [dokument migracji](docs/MIGRATION.md). Dawne opisy pozostają w archiwach poszczególnych zadań.

[Licencja MIT](Evaluation-task1/LICENSE) i [licencje bibliotek frontendowych](Evaluation-task1/docs/THIRD-PARTY.md) dotyczą zadania 1. Zadania 2 i 3 nie mają osobno opublikowanych licencji. [Zgłoszenia](https://github.com/haribo841/Evaluation-tasks/issues) dotyczą całego zbioru zadań.
