# Instrukcja użytkownika — GameStore

Prosta, „łopatologiczna" instrukcja obsługi sklepu z grami GameStore. Pokazuje krok po kroku, jak poruszać się po stronie, założyć konto, zalogować się do panelu administratora i zarządzać treścią.

> **Jak dodać zrzuty ekranu:** w każdym miejscu oznaczonym `[ZRZUT EKRANU: ...]` wstaw obrazek ze swojej uruchomionej aplikacji. W pliku Markdown robisz to tak:
> `![Opis](sciezka/do/obrazka.png)`. Najłatwiej wrzucić zrzuty do folderu `docs/img/` i podmienić placeholdery.

---

## 1. Strona główna

Po wejściu na stronę widzisz ekran powitalny z wyróżnionymi grami oraz górne menu: **Start**, **Sklep**, **Zaloguj**, **Załóż konto**.

`[ZRZUT EKRANU: strona główna z kafelkami gier]`

---

## 2. Przeglądanie sklepu

1. Kliknij **Sklep** w górnym menu.
2. Zobaczysz wszystkie dostępne gry w formie kafelków (okładka, tytuł, gatunek, ocena, cena).
3. Możesz:
   - **wyszukać** grę po nazwie w polu „Szukaj tytułu" (lista filtruje się na bieżąco),
   - wybrać **gatunek** z listy,
   - **posortować** gry po cenie, tytule lub dacie.

`[ZRZUT EKRANU: katalog sklepu z paskiem filtrów]`

---

## 3. Strona gry

Kliknij dowolny kafelek lub przycisk **Zobacz**, aby otworzyć stronę gry. Znajdziesz tam opis, okładkę, średnią ocenę oraz recenzje innych użytkowników.

`[ZRZUT EKRANU: strona szczegółów gry]`

---

## 4. Zakładanie konta i logowanie

1. Kliknij **Załóż konto** w prawym górnym rogu.
2. Podaj adres e-mail i hasło, a następnie potwierdź rejestrację.
3. Aby zalogować się ponownie, użyj przycisku **Zaloguj**.

`[ZRZUT EKRANU: formularz rejestracji / logowania]`

Po zalogowaniu w menu pojawią się: **Moja biblioteka** oraz Twój login.

---

## 5. Dodawanie recenzji i oceny

1. Będąc zalogowanym, otwórz stronę wybranej gry.
2. W sekcji **Recenzje** kliknij gwiazdki, aby wystawić ocenę (1–5).
3. Wpisz treść recenzji i kliknij **Opublikuj recenzję**.

> Jedną grę możesz zrecenzować tylko raz.

`[ZRZUT EKRANU: formularz dodawania recenzji z gwiazdkami]`

---

## 6. Biblioteka „moje gry"

1. Na stronie gry kliknij **+ Dodaj do biblioteki**.
2. Wejdź w **Moja biblioteka** (górne menu), aby zobaczyć swoją kolekcję.
3. Grę możesz w każdej chwili **usunąć z biblioteki**.

`[ZRZUT EKRANU: widok biblioteki użytkownika]`

---

## 7. Panel administratora (zarządzanie treścią)

Panel służy do **customizacji treści sklepu** — to tutaj zmieniasz teksty, ceny i okładki gier bez dotykania kodu.

### Logowanie do panelu

1. Zaloguj się na konto administratora:
   - **Login:** `admin@gamestore.local`
   - **Hasło:** `Admin123!`
2. W menu pojawi się zielony odnośnik **Panel admina** — kliknij go.

`[ZRZUT EKRANU: menu z widocznym odnośnikiem „Panel admina"]`

### Zarządzanie grami (CRUD + okładki)

- **Dodanie gry:** kliknij **+ Dodaj grę**, uzupełnij tytuł, opis, cenę, datę, wybierz gatunek i wydawcę. W polu **Okładka** wybierz plik graficzny — od razu zobaczysz jego podgląd. Kliknij **Zapisz**.

  `[ZRZUT EKRANU: formularz dodawania gry z podglądem okładki]`

- **Edycja / podmiana okładki:** w tabeli gier kliknij **Edytuj**, zmień dowolne pole lub wgraj nową okładkę, a następnie **Zapisz zmiany**.
- **Usunięcie:** kliknij **Usuń** i potwierdź.

  `[ZRZUT EKRANU: tabela gier w panelu administratora]`

### Zarządzanie gatunkami i wydawcami

W panelu, w górnym pasku, znajdziesz przyciski **Gatunki** i **Wydawcy**. Działają tak samo jak gry: dodawanie, edycja i usuwanie. Gatunku lub wydawcy nie można usunąć, jeśli są przypisane do jakiejś gry.

`[ZRZUT EKRANU: lista gatunków w panelu administratora]`

---

## 8. Wylogowanie

Kliknij **Wyloguj** w prawym górnym rogu, aby bezpiecznie zakończyć sesję.

---

### Najczęstsze pytania

**Nie widzę „Panelu admina".** Upewnij się, że jesteś zalogowany kontem `admin@gamestore.local` — panel jest dostępny tylko dla administratora.

**Okładka się nie wyświetla.** Sprawdź, czy plik został wgrany (pojawia się w `wwwroot/uploads`) i czy to obraz (np. `.jpg`, `.png`).
