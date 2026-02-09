# Weather Web App 🌦️

A Blazor Server-based weather web application that mimics basic features of the Windows Weather app. Users can search for cities, view forecasts, and manage favorite cities based on their roles.

---
## Terms
- **Frontend:** Solution Project Name `Weather.Web`
- **Backend:** Solution Project Name `Weather.Api`

## Table of Contents

- [Features](#features)  
- [Tech Stack](#tech-stack)  
- [Solution Structure](#solution-structure)  
- [Setup & Running the Solution](#setup--running-the-solution)  
- [Assumptions & Known Limitations](#assumptions--known-limitations)  

---

## Features
1. **Stand Alone Apps**  
   - **Frontend:** Weather.Api (also refered as FE App), runs individually
   - **Backend:** Weather.Web (also refered as BE App), runs individually, Blazeor-Server-based
   - Only shared in Class modules.

2. **User Authentication**  
   - Registration and login flows using JWT authentication  
   - Role-based access: `SimpleUser` and `SuperUser`  
   - Administrator user (through db seeding) only for app startup purpose  
   - Session persistence via JWT  

3. **City Search & Weather Forecast**  
   - Search for cities using an external geocoding API  
   - Display predefined number of days (e.g. 5) of weather forecast  
   - Maps API responses to internal DTOs  

4. **Favorite Cities**  
   - `SuperUser` can save up to predfined number of favorite cities (e.g., 5)  
   - Add, remove, and view favorite cities  
   - Favorite cities are persisted in the database to avoid repeatence  
   - Not accisible to `SimpleUser` and Guest user even at API level.  

5. **User Experience**  
   - Simple and responsive UI, including Mobile UI layout  
   - Smooth flow from login → city search → add/remove favorites  
   - `SimpleUser` and Gues user (not authenticated) have the same functionalties (Search and view forcast)  

---

## Tech Stack

- **Frontend:** Blazor Server, C#  
- **Backend:** .NET 8, Entity Framework Core, C#  
- **Database:** MYSQL, EF Core migrations (Code First), Auto Seeding for Roles and Admin (owner) account
- **Styling & UI:** Tailwind CSS  
- **External API:** Open Meteo API for weather and geolocation

---

## Solution Structure (Quick Overview)

```
Weather (Solution)
│
├─ Weather.Api (Backend)
│  ├─ Controllers
│  │  ├─ AuthController.cs
│  │  ├─ FavoritesController.cs
│  │  ├─ ISOCountrycodeController.cs
│  │  └─ WeatherController.cs
│  ├─ Data
│  │  └─ AppDbContext.cs
│  ├─ Helpers
│  │  ├─ StringExtensions.cs
│  │  └─ UnixTimeExtensions.cs
│  ├─ Interfaces
│  └─ Services
│
├─ Weather.Models
│  ├─ Configuration
│  ├─ DTOs
│  ├─ Entities
│  └─ Enums
│
└─ Weather.Web (Frontend)
   ├─ Components
   ├─ Helpers
   ├─ Interfaces
   │  ├─ Favorite
   │  ├─ Identity
   │  ├─ IForcastService.cs
   │  └─ ISearchCityService.cs
   └─ Services
```

---

## Setup & Running the Solution

### Prerequisites

- Visual Studio 2022
- .NET 8 SDK  
- MySql or SQL Server Engine  
- Tailwind, you can install it using: `install tailwindcss @tailwindcss/cli`

### Backend Setup (Weather.Api)

1. Default DB framework is MySql, If you are going to user SQL Server, you need to modify add the package first and do the injection at `Program.cs`  
      ```
      -- builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
      ++ builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
      ```
2. Create an empty Databse (say `Weather`) using the engine/manger you set in point 1, 
3. Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Data Source=BRK;Initial Catalog=Weather;Integrated Security=True;Pooling=False;Encrypt=False;Trust Server Certificate=True"
}
```
4. Optionaly, adjust the other configuration in the `appsettings.json`, example:
```json

"AdminUser": {
  "Email": "admin@weather.local",
  "Password": "Admin@321"
},
"WeatherOptions": {
  "FavoriteMaxLimit": "5",
  "ForcastMaxLimit": "5"
}

```
4. Run the API project (or Solution):
```bash
dotnet run --project Weather.Api
```
 If you set pont 1-5, right the Databes should be auto seeded with user roles and based AdminUser account, defined in point No.4:

- The API will start at `https://localhost:44334/`


### Frontend Setup (Weather.Web)

1. Update the API base address in `appsettings.json`:

```json
"WeatherApi": {
    "BaseAddress": "https://localhost:44334/api/"
}
```

2. Run the Blazor Server project (or Solution):

```bash
dotnet run --project Weather.Web
```

3. Open your browser at `https://localhost:5001` (or the port displayed in terminal).

---

## Assumptions & Known Limitations

- Only `SuperUser` roles can manage favorites; `SimpleUser` cannot.  
- `SimpleUser` and guest user are assumed to utlized similar features (Search cities, and Forcast Weather at Home page).  
- External APIs are assumed to be stable; temporary failures return a 503 status.  
- Mobile responsiveness is basic; no dedicated mobile layout implemented.  
- Some edge cases (e.g., duplicate city names, or no search result return (e.g., `tokyo`)) rely on external API accuracy.  

---
