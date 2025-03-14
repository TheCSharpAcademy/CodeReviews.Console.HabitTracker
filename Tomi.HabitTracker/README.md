# Habit Logger

## Overview

Habit Logger is a simple console-based application that helps users log their habits and track them over time. This project is designed to teach the basics of performing CRUD (Create, Read, Update, Delete) operations against a real database.

The app interacts with an SQLite database using C# and ADO.NET.

## Features

- View all habit records
- Insert new habit records
- Delete existing habit records
- Update habit records

## Technologies Used

- **C#** (Main programming language)
- **.NET** (Framework)
- **SQLite** (Database)
- **ADO.NET** (For database operations)

## Getting Started

### Prerequisites

Ensure you have the following installed on your system:

- .NET SDK
- SQLite
- A code editor (e.g., Visual Studio Code, Rider, or Visual Studio)

### Installation

1. Clone this repository:
   ```sh
   git clone https://github.com/remioluwatomi/CodeReviews.Console.HabitTracker.git
   cd CodeReviews.Console.HabitTracker/Tomi.HabitTracker
   ```
2. Open the project in your preferred C# development environment.
3. Build and run the project:
   ```sh
   dotnet run
   ```

## Usage

Once you run the application, you'll be presented with a menu to choose from:

```
Main Menu

What would you like to do?
Type 0 to Close Application
Type 1 to View All Records
Type 2 to Insert Records
Type 3 to Delete Records
Type 4 to Update Records
Your option: _
```

- **View All Records:** Lists all logged habits.
- **Insert Records:** Allows adding a new habit.
- **Delete Records:** Removes a habit entry.
- **Update Records:** Modifies an existing habit.

## Project Structure

```
# Project Structure: CODEREVIEWS.CONSOLE.HABITTRACKER

Tomi.HabitTracker
├── Data/
│   ├── Data.csproj
│   ├── DBHelper.cs
│   ├── HabitCompactGist.cs
├── Prompts/
│   ├── HabitPrompts.cs
│   ├── Prompts.csproj
├── habit-logger.db
├── Program.cs
├── README.md
├── Tomi.HabitTracker.csproj
├── .codacy.yml
├── .gitignore
└── CodeReviews.Console.HabitTracker.sln

```

## Database Schema

The SQLite database consists of a single table:

```sql
CREATE TABLE IF NOT EXISTS habit_logger (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    habit TEXT NOT NULL,
    quantity REAL NOT NULL,
    date TEXT NOT NULL
);
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
