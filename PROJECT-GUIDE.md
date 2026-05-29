# Internship Project — Folder Guide

**Project name:** Apprenticeship Management System  
**Technology:** ASP.NET Core MVC + MySQL  

---

## Main folders (what is inside)

| Folder | What you coded here | Main files |
|--------|---------------------|------------|
| **Controllers/** | Backend logic — runs when user clicks buttons / opens pages | `HomeController`, `AccountController`, `AdminController`, `ApprenticeController` |
| **Models/** | Database table structure (C# classes) | `Admin.cs`, `Apprentice.cs`, `UserRole.cs` |
| **ViewModels/** | Form data & page data (not stored in DB directly) | `AdminHomeModel`, `StudentFormModel`, etc. |
| **Views/** | Frontend HTML pages (Razor) | `.cshtml` files |
| **Data/** | Database connection | `InternshipDb.cs` |
| **Helpers/** | Extra utility code | `AuthHelper.cs` (password hash) |
| **wwwroot/** | CSS, JavaScript, images | `css/site.css`, `js/site.js` |
| **Program.cs** | App startup — DB + login setup | Root file |
| **appsettings.json** | MySQL connection settings | Root file |

---

## Controllers (brain of project)

| File | Job |
|------|-----|
| `HomeController.cs` | Home page, privacy page, error page |
| `AccountController.cs` | Admin/Apprentice **login**, **register**, **logout** |
| `AdminController.cs` | Admin panel — list students, search, add, edit, activate/deactivate |
| `ApprenticeController.cs` | Student dashboard & edit own profile |

---

## Models (database tables)

| Class | MySQL table | Purpose |
|-------|-------------|---------|
| `Admin` | `admins` | Admin users |
| `Apprentice` | `apprentices` | Students / apprentices |
| `UserRole` | (enum) | Admin or Apprentice role for login |

---

## ViewModels (forms & lists)

| Class | Used on page |
|-------|----------------|
| `AdminHomeModel` | Admin panel dashboard |
| `StudentRowModel` | One row in admin student table |
| `StudentFormModel` | Add student form |
| `EditStudentModel` | Admin edit student |
| `AdminRegModel` | Admin registration |
| `AdminLoginModel` | Admin login |
| `StudentRegModel` | Student registration |
| `StudentLoginModel` | Student login |
| `StudentHomeModel` | Student dashboard |
| `MyProfileModel` | Student edit profile |

---

## Views (folders = website pages)

| Folder | Pages |
|--------|--------|
| `Views/Home/` | Landing page |
| `Views/Account/` | Login & register screens |
| `Views/Admin/` | Admin panel — `Index`, `Add`, `Edit`, `Details` |
| `Views/Apprentice/` | Student dashboard & profile |
| `Views/Shared/` | Layout (navbar), error page |

---

## How request flows (tell your guide)

```
Browser → Controller → InternshipDb (MySQL) → Controller → View (.cshtml) → Browser
```

---

## Run project

```powershell
cd Internship
dotnet run
```

Open: **http://localhost:5000**

---

## Class diagram (simple)

```
Admin ──────────────┐
Apprentice ─────────┼──► InternshipDb ──► MySQL
                    │
Controllers ────────┘
     │
     └──► Views (HTML)
```

---

*Written for internship report / viva explanation.*
