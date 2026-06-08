# Dokumentacja techniczna — GameStore

## 1. Opis ogólny

GameStore to aplikacja webowa typu sklep z grami, zbudowana w architekturze **ASP.NET Core MVC (.NET 8)** z wykorzystaniem **Entity Framework Core** (podejście Code First) i bazy **MS SQL Server (LocalDB)**. Warstwa prezentacji opiera się na silniku widoków Razor oraz Bootstrap 5 z autorskim, ciemnym motywem inspirowanym Steam. Uwierzytelnianie i autoryzację zapewnia **ASP.NET Core Identity** z podziałem na role `Admin` i `User`.

## 2. Architektura rozwiązania

Aplikacja realizuje wzorzec **Model–View–Controller**:

- **Model** — encje domenowe (`Models/DbModels`) oraz modele widoków (`Models/ViewModels`). Dostęp do danych przez `ApplicationDbContext` (EF Core).
- **View** — widoki Razor (`.cshtml`) renderowane po stronie serwera; współdzielony layout, partiale (`_GameCard`, `_LoginPartial`).
- **Controller** — logika obsługi żądań: kontrolery publiczne (`Home`, `Games`, `Library`) oraz kontrolery panelu administratora w obszarze (Area) `Admin`.

### Podział na warstwy

| Warstwa            | Element                                                   |
|--------------------|-----------------------------------------------------------|
| Prezentacja        | Widoki Razor, Bootstrap 5, `site.css`, `site.js`          |
| Logika aplikacji   | Kontrolery MVC, atrybuty autoryzacji                      |
| Dostęp do danych   | `ApplicationDbContext`, EF Core, migracje                 |
| Baza danych        | MS SQL Server (LocalDB)                                   |
| Bezpieczeństwo     | ASP.NET Core Identity, role, `ValidateAntiForgeryToken`   |

### Mechanizm Areas

Panel administratora wydzielono do obszaru `Areas/Admin`. Routing obsługuje trasa `{area:exists}/{controller}/{action}/{id?}`. Wszystkie kontrolery panelu są oznaczone atrybutem `[Authorize(Roles = "Admin")]`, dzięki czemu dostęp mają wyłącznie administratorzy.

## 3. Struktura bazy danych

Baza powstaje automatycznie z modelu (Code First) i migracji. Oprócz tabel ASP.NET Core Identity (`AspNetUsers`, `AspNetRoles`, `AspNetUserRoles` itd.) aplikacja definiuje pięć własnych tabel.

### Encje i relacje

- **Genre** (`GenreId`, `Name`) — gatunek gry.
- **Publisher** (`PublisherId`, `Name`) — wydawca / studio.
- **Game** (`GameId`, `Title`, `Description`, `Price`, `ReleaseDate`, `CoverImagePath`, `GenreId`, `PublisherId`) — główna encja.
- **Review** (`ReviewId`, `GameId`, `UserId`, `UserName`, `Rating`, `Comment`, `CreatedAt`) — recenzja użytkownika.
- **LibraryEntry** (`LibraryEntryId`, `GameId`, `UserId`, `AddedAt`) — wpis w bibliotece użytkownika.

### Diagram relacji (uproszczony)

```
Genre 1 ────< N Game N >──── 1 Publisher
                  │ 1
                  ├──────< N Review
                  └──────< N LibraryEntry
```

- `Game → Genre` oraz `Game → Publisher`: relacja N:1, **DeleteBehavior.Restrict** (nie można usunąć gatunku/wydawcy mającego przypisane gry; zapobiega to także błędowi SQL Server „multiple cascade paths").
- `Review → Game` oraz `LibraryEntry → Game`: relacja N:1, **DeleteBehavior.Cascade** (usunięcie gry kasuje powiązane recenzje i wpisy bibliotek).
- `LibraryEntry`: unikalny indeks złożony `(UserId, GameId)` — jedna gra może trafić do biblioteki danego użytkownika tylko raz.
- `Game.Price`: typ `decimal(8,2)`.

Powiązanie recenzji i biblioteki z użytkownikiem realizowane jest przez pole `UserId` (identyfikator z Identity) bez twardego klucza obcego — świadome uproszczenie zmniejszające liczbę ścieżek kaskadowych.

## 4. Najważniejsze zaimplementowane funkcjonalności

### Katalog (GamesController.Index)
Zapytanie LINQ z `Include` ładuje gatunek, wydawcę i recenzje. Obsługuje parametry `searchTerm` (filtr po tytule), `genreId` (filtr gatunku) i `sortOrder` (sortowanie). Dodatkowo skrypt `site.js` filtruje karty po stronie klienta w czasie rzeczywistym.

### Strona gry i recenzje (GamesController.Details / AddReview)
Widok łączy dane gry, listę recenzji i formularz dodania recenzji (model `GameDetailsViewModel`). Dodawanie recenzji wymaga zalogowania (`[Authorize]`), waliduje ocenę (1–5) i blokuje wielokrotne recenzowanie tej samej gry przez jednego użytkownika.

### Biblioteka użytkownika (LibraryController)
Akcje `Add` / `Remove` operują na tabeli `LibraryEntry` w kontekście aktualnie zalogowanego użytkownika (`ClaimTypes.NameIdentifier`). Widok `Index` prezentuje kolekcję jako siatkę kart.

### Panel administratora (Areas/Admin)
Pełny CRUD dla gier, gatunków i wydawców. Formularz gry obsługuje **upload pliku okładki**: `IFormFile` zapisywany jest do `wwwroot/uploads` pod nazwą GUID, a względna ścieżka trafia do pola `CoverImagePath`. Edycja umożliwia podmianę okładki. Usuwanie gatunku/wydawcy jest blokowane, gdy istnieją powiązane gry.

### Uwierzytelnianie i dane startowe
`Program.cs` rejestruje Identity z rolami. Klasa `SeedData` przy starcie stosuje migracje, tworzy role `Admin`/`User`, konto administratora oraz przykładowe gatunki, wydawców i gry.

## 5. Bezpieczeństwo

- Hasła i konta zarządzane przez ASP.NET Core Identity (hashowanie haseł, polityka złożoności).
- Autoryzacja oparta na rolach — panel administratora dostępny tylko dla roli `Admin`.
- Ochrona przed CSRF — formularze modyfikujące dane używają `@Html.AntiForgeryToken()` i atrybutu `[ValidateAntiForgeryToken]`.
- Walidacja modeli po stronie serwera (atrybuty `DataAnnotations`) oraz klienta (jQuery Validation Unobtrusive).

## 6. Technologie i wersje

| Komponent                                  | Wersja  |
|--------------------------------------------|---------|
| .NET                                       | 8.0     |
| Microsoft.EntityFrameworkCore.SqlServer    | 8.0.26  |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.0.26 |
| Microsoft.AspNetCore.Identity.UI           | 8.0.26  |
| Bootstrap                                  | 5.x     |
| jQuery / jQuery Validation                 | 3.x     |
