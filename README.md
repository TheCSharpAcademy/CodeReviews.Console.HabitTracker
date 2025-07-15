# 🧠 Habit Tracker Console App

A simple, extensible **.NET console application** that helps users manage and track habits and their progress over time. Built with C#, SQLite, and Spectre.Console, this project serves as a practical learning tool for:

- Database design using **SQLite**
- Console UI with **Spectre.Console**
- CRUD operations
- Clean architecture and separation of concerns
- Manual dependency injection

---

## 📁 Features

- ✅ Add, update, and delete habits
- 📊 Record daily habit entries with date and quantity
- 📆 Generate and seed database with random historical data
- 📋 View and manage records in a styled console UI
- 🧪 Modular code structure to encourage learning and extension

---

## 📸 Sample UI

The app uses `Spectre.Console` to render beautiful tables in the terminal:


| Id | Date        | Amount | Habit          |
|----|-------------|--------|----------------|
| 1  | Jan 5, 2024 | 1500g  | Drinking Water |
| 2  | Feb 7, 2024 | 12pgs  | Reading        |

---

## 🚀 Getting Started

### 🧰 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio or VS Code
- SQLite (optional, DB is created automatically)
- `appsettings.json` with a connection string

### 🔧 appsettings.json

Create an `appsettings.json` file in your project root:


---

## 🚀 Getting Started

### 🧰 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio or VS Code
- SQLite (optional, DB is created automatically)
- `appsettings.json` with a connection string

### 🔧 appsettings.json

Create an `appsettings.json` file in your project root:

{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=habit-Tracker.db"
  }
}



##🏗️ How It Works
Program.cs

    Reads appsettings.json

    Initializes and shares the connection string

    Calls:

        Data.CreateDatabase() → Creates and seeds tables

        HabitFunctions.MainMenu() → Launches interactive console menu

Core Classes
Class	Responsibility
Data	Manages database setup and initial seeding
HabitFunctions	Manages habits and records (CRUD + console UI)
Habit, RecordWithHabit	Data models using C# records
📚 Commands Available

When the app runs, you’ll be prompted with options:

    Add Habit

    Delete Habit

    Update Habit

    Add Record

    Delete Record

    View Records

    Update Record

    Quit

🧪 Sample Seed Data

If no habits or records exist, the app will auto-populate the DB with:

    Habits like: Reading, Running, Drinking Water

    Units like: Pages, Meters, Milliliters

    100 randomly generated records

🧠 Learning Objectives

This app was created as part of a C# learning journey using:

    Manual Dependency Injection

    Microsoft.Data.Sqlite for raw SQL interaction

    Spectre.Console for enhanced CLI interfaces

    Data modeling with record types

    Hands-on ADO.NET experience

📁 Folder Structure
HabitTracker/
├── Program.cs
├── Data.cs
├── HabitFunctions.cs
├── appsettings.json
├── bin/
├── obj/

🙌 Acknowledgements

    Spectre.Console for awesome CLI UI

    The C# Academy for project inspiration

    SQLite for lightweight, embedded data persistence

🔄 Future Improvements

    Add filtering and sorting to records view

    Add export to CSV/JSON

    Migrate to Entity Framework for scalability

    Add unit tests and validation layers

    Optional GUI with MAUI or WinForms

🧑‍💻 Author

Oche "TheNigerianNerd" Edache
💻 GitHub
🎓 1st Class BSc in Computing, University of Bolton
