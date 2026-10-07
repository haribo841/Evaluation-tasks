# Migracja zadań, 06.10.2026

Źródła przed reorganizacją:

| Repozytorium | Gałąź | Commit |
| --- | --- | --- |
| `haribo841/Evaluation-tasks` | `master` | `2037f4e60d8f177ecc54510868a5d133d643a3de` |
| `haribo841/aspnetcore-xml-crud` | `master` | `0b078d310db7922928e4ffe67f9c6121dd16570b` |

Historia obu źródeł jest łączona poprzez commit z dwoma rodzicami. Stare identyfikatory commitów pozostają dostępne w tym repozytorium. Nie użyto force push ani zmiany autorów czy dat historii.

Nowy układ:

- Cały ASP.NET Core XML CRUD trafia do `Evaluation-task1`, razem z widokami, migracjami, screenshotem, przykładami XML, licencją i bibliotekami frontendowymi.
- Dawne pliki z głównego katalogu Evaluation-tasks oraz ich `docs` i `tests` trafiają do `Evaluation-task2`.
- Folder `Evaluation task 3` staje się `Evaluation-task3`.
- W katalogu głównym powstaje indeks README, wspólna solution i workflow budowania.

Zmiany wymagane przez nowe ścieżki:

- Zaktualizowano instrukcje i linki aktywnych README. Poprzednie wersje README są zachowane w archiwach.
- Naprawiono odwołanie do zadania 3 w solution zadania 2 i usunięto niepotrzebne wykluczenie źródeł sąsiedniego projektu.
- W zadaniu 3 zadeklarowano brakujący `CsvHelper` 30.0.1 i usunięto nieużywany import `System.ComponentModel`, który powodował niejednoznaczność atrybutu `TypeConverter`. Algorytm prototypu pozostaje zachowany.
- Dawny workflow .NET 6 z CRUD-a zachowano w `Evaluation-task1/docs/archive/dotnet-workflow-2023.yml`. Aktywny workflow jest w głównym `.github/workflows/dotnet.yml` i wskazuje wspólną solution oraz testy zadania 2.
- Usunięto końcowe spacje i pustą końcową linię w pięciu importowanych plikach, bez zmiany ich działania.

Nie przenoszono lokalnych plików `users.xml`, wyników `bin`/`obj`, konfiguracji user-secrets ani danych kolejki transkrypcji. Wygenerowane wcześniej raporty CSV z `wwwroot` są pominięte w nowym drzewie źródeł i pozostają dostępne wyłącznie we wcześniejszych commitach. Przykładowe dane dokumentacji oraz kod pozostają zachowane.

Weryfikacja lokalna obejmuje budowanie trzech projektów, 11 testów uruchomienia zadania 2 i izolowany podgląd HTTP zadania 1. Szczegóły są w [raporcie walidacji](VALIDATION-2026-10-06.md). Test budowania zadania 3 nie jest potwierdzeniem poprawności jego raportów. Stan GitHub Actions należy sprawdzić po publikacji.
