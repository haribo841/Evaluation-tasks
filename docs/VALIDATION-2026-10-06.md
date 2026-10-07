# Weryfikacja migracji, 06.10.2026

Na Windows z SDK .NET 10.0.302 wykonano:

- `dotnet restore Evaluation-tasks.sln`: sukces.
- `dotnet build Evaluation-tasks.sln -c Release --no-restore`: zbudowane wszystkie trzy projekty, zero błędów. Pozostają istniejące ostrzeżenia o .NET 7, nullable i nieużywanych zmiennych.
- `pwsh -File Evaluation-task2/tests/smoke.ps1`: 11 testów CLI zakończonych sukcesem. Przykłady i testy używają fikcyjnych danych w osobnym katalogu tymczasowym.
- Izolowany podgląd zadania 1 na `127.0.0.1`, z fixture `docs/examples/users.sample.xml`, środowiskiem Production i nieosiągalnym adresem SQL: odpowiedzi HTTP 200 dla listy z dwoma rekordami, formularza dodawania z tokenem anty-CSRF, formularza edycji z danymi przykładowymi oraz Bootstrap CSS.
- Suma SHA-256 źródłowego i tymczasowego XML pozostała identyczna: `aee228f889a98393b4ed3e4e4f017b4b19c3f8b9795d22b9c6be098364b96d8b`. Serwer podglądu zakończono po próbie.

Nie wykonywano zapisu przez HTTP, logowania, migracji SQL ani testów rzeczywistej bazy. Zadanie 3 sprawdzono wyłącznie przez kompilację; nie uruchamiano jego Main ze ścieżkami na Pulpit.

Weryfikacja lokalna nie stanowi wyniku GitHub Actions. Workflow jest uruchamiany oddzielnie po publikacji.
