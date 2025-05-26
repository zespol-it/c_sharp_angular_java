# FamilyBudget & BudgetManager

## Instrukcja uruchomienia

1. **Backend (C# - BudgetManager.Api)**
   - Przejdź do katalogu `BudgetManager.Api`
   - Uruchom: `dotnet run`
   - Backend domyślnie nasłuchuje na porcie 5000

2. **Frontend (Angular)**
   - Przejdź do katalogu `family-budget`
   - Uruchom: `npm install` (jeśli pierwszy raz)
   - Uruchom: `ng serve`
   - Frontend domyślnie dostępny na `http://localhost:4200`

---

## Dane do logowania (przykładowy użytkownik)

- **Email:** test@example.com
- **Hasło:** string123

Użytkownik ten jest automatycznie tworzony przy pierwszym uruchomieniu backendu (seedowanie bazy).

---

## Endpoint logowania (backend)

- `POST http://localhost:5000/api/auth/login`
- Body:
  ```json
  {
    "email": "test@example.com",
    "password": "string123"
  }
  ```

---

## Kontakt
W razie problemów lub pytań napisz do autora projektu. 