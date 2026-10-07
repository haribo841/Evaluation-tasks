# Evaluation task 3

## Jak to rozumiem

<b>Zadanie polega na stworzeniu generatora raportów dla działu HR w pewnej firmie.
Generator ma tworzyć raporty na dowolny okres czasu, podsumowywać czasy pracy pracownika w określonym przedziale czasowym, obsługiwać sytuacje, w których pracownik ma wiele kontraktów w danym okresie, a także okresy bez zatrudnienia.
Kontrakty mogą być na czas nieokreślony, a czasy pracy powinny być sumowane osobno dla odpowiednich kontraktów lub okresów bez zatrudnienia.</b>
niestety treść zadania jest sformułowana w sposób bardzo nie jasny

##Pobranie danych z plików CSV:

Kod wczytuje dane z dwóch plików CSV: "interval.csv" i "test.csv", które zawierają interwały czasowe oraz informacje o umowach z pracownikami.
Dane z tych plików są wczytywane za pomocą biblioteki CsvHelper. W przypadku interwałów czasowych, dane są wczytywane jako listę dat (typ DateTime), a w przypadku umów pracowniczych, dane są wczytywane do obiektów klasy Contract.

##Określenie daty początkowej i końcowej raportu:

Na podstawie wczytanych interwałów czasowych obliczane są daty początkowe i końcowe raportu. Jeśli brak interwałów czasowych, używana jest aktualna data.
Jeśli nie uda się określić dat początkowej i końcowej, program wyrzuca wyjątek.

##Generowanie raportu:

Następnie program generuje raport w postaci listy ReportRecord, która zawiera posortowane przedziały czasowe, w których będą sumowane czasy pracy pracowników.
Iteruje przez umowy pracownicze (kontrakty) i tworzy przedziały czasowe, w których czasy pracy powinny być sumowane. Jeśli umowa ma datę rozpoczęcia późniejszą niż początek aktualnego przedziału, tworzony jest nowy przedział od początku aktualnego przedziału do daty rozpoczęcia umowy.
Jeśli umowa ma datę zakończenia, aktualny przedział kończy się w tej dacie.
Na koniec, jeśli aktualny przedział nie osiągnął daty końcowej raportu, tworzony jest przedział od aktualnego końca do daty końcowej raportu.

##Zapis raportu do pliku CSV:

Wygenerowany raport w postaci listy ReportRecord jest zapisywany do pliku "output.csv" za pomocą CsvHelper.
Kod zawiera także konwersję dat z formatu tekstowego w plikach CSV za pomocą klasy CustomDateTimeConverter oraz obsługę daty niesprecyzowanej (null) za pomocą klasy NullableDateTimeConverter.