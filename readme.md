Konfiguracja:

- żeby wysyłac pliki, trzeba ustawić folder akcji dla aplikacji, to jest ścieżkę do plików. Można mieć kilka folderów.
- ścieżka do folderu może mieć podfoldery ale powinna zaczynać się od nazwy i kończyć nią. Pomiędzy można używać ukośnika i ukośnika wstecznege, np: Faktury/2024, bądź Faktury\2024


![image](https://github.com/Maniek13/EmailWebService/assets/47826375/1e08ad28-97e2-4cca-9aa1-a945ae7e2d4d)

Domena:
- do pobrania pliku potrzebna jest znajomość akcji i nazwy pliku. Nazwą pliku jest nazwa razem z rozszerzeniem.
- można pobrać wszytstkie pliki akcji w formie zip
- pobieranie plików po id, ogólnie jest taka opcja ale trzeba znać id pliku w bazie danych. W przyszłosci może zostanie to jakoś rozwiązane.
- po usunięciu wszystkich plików, wpis w bazie danych z folderem do akcji zostaje. Trzeba go usunąć poprzez konfigurację