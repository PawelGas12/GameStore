# GameStore

Prosty sklep z grami w stylu Steam, robiony jako projekt zaliczeniowy. Można przeglądać katalog gier, założyć konto, oceniać i recenzować tytuły, dodawać je do własnej biblioteki, a z poziomu panelu administratora zarządzać całą zawartością sklepu.

## O co chodzi

Aplikacja ma dwa widoki. Dla zwykłego użytkownika to sklep: lista gier z wyszukiwarką i filtrami, strona każdej gry z opisem, okładką i recenzjami, oraz zakładka "moja biblioteka". Dla administratora to panel, w którym dodaje się i edytuje gry (razem z wgrywaniem okładek), zarządza gatunkami i wydawcami oraz usuwa nieodpowiednie recenzje.

Drobny szczegół: recenzję można wystawić tylko do gry, którą ma się w bibliotece. Chodziło o to, żeby oceny pochodziły od osób, które dany tytuł faktycznie mają, a nie od przypadkowych kont.

## Technologie

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core 8 (Code First + migracje)
- MS SQL Server (LocalDB)
- ASP.NET Core Identity do logowania i ról (Admin / User)
- Bootstrap 5 + własny arkusz CSS (ciemny motyw, RWD)
- trochę czystego JavaScriptu (filtrowanie katalogu na żywo, podgląd okładki przy uploadzie)

Architektura jest standardowa dla MVC. Modele i dostęp do danych siedzą w `Models` i `Data` (klasa `ApplicationDbContext`), logika w kontrolerach, a panel admina jest wydzielony do osobnego obszaru `Areas/Admin` i zabezpieczony rolą Admin.

## Jak to uruchomić

Potrzebne: Visual Studio 2022 z obsługą ASP.NET, .NET 8 SDK oraz LocalDB (instaluje się razem z VS).

1. Otwórz `GameStore.sln` w Visual Studio.
2. Zerknij na connection string w `appsettings.json`. Domyślnie wskazuje na LocalDB, więc zwykle nie trzeba nic ruszać.
3. **Najważniejszy krok, łatwo o nim zapomnieć** - przed pierwszym uruchomieniem trzeba zbudować bazę. Wejdź w `Narzędzia -> Menedżer pakietów NuGet -> Konsola menedżera pakietów` i wpisz po kolei:

   ```
   Add-Migration InitialCreate
   Update-Database
   ```

   Jeśli się to pominie, aplikacja przy starcie wyłoży się błędem `Invalid object name 'AspNetRoles'` - to po prostu znaczy, że w bazie nie ma jeszcze tabel.
4. Uruchom (F5).

Przy pierwszym starcie aplikacja sama zakłada konto administratora oraz wrzuca do bazy kilkanaście przykładowych gier z okładkami, więc od razu jest co klikać.

## Konto administratora

- login: `admin@gamestore.local`
- hasło: `Admin123!`

Po zalogowaniu na to konto w menu u góry pojawia się odnośnik "Panel admina".

## Jakby coś się posypało

Gdy baza była już raz tworzona i chcesz ją wyczyścić od zera (np. żeby wgrały się nowe przykładowe gry), w tej samej konsoli:

```
Drop-Database
Update-Database
```

Okładki dodawane przez admina trafiają do `wwwroot/uploads` i nie są wrzucane do repozytorium.
