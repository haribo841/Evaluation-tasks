# Evaluation task 3

Historyczny, nieukończony prototyp raportu HR. Projekt używa .NET 9 oraz CsvHelper 30.0.1. Budowanie sprawdza spójność źródeł po migracji; nie potwierdza poprawności raportów.

Z głównego katalogu repozytorium:

```powershell
dotnet build "Evaluation-task3/Evaluation task 3.csproj" -c Release
```

Program.cs nadal zawiera ścieżki do interval.csv, test.csv i output.csv na Pulpicie autora. Przed uruchomieniem trzeba dostosować je do osobnego katalogu z fikcyjnymi danymi; zapis wyniku nadpisuje wskazany plik. Nie uruchamiaj prototypu na istniejącym zbiorze danych.

Obecny algorytm tworzy przedziały na podstawie dat kontraktów. Nie realizuje pełnego sumowania czasu pracy opisywanego w pierwotnym zadaniu. Oryginalny opis jest zachowany w [archiwum README](docs/archive/README-before-migration-2026-10-06.md).
