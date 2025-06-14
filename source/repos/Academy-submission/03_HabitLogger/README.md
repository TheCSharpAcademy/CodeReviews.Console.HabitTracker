# 📋 Habit Logger

A console app that helps the user to track and log habits that can be measured.

Inspired by the [C# Academy Habit Logger Project]([https://www.thecsharpacademy.com/project/11/calculator](https://www.thecsharpacademy.com/project/12/habit-logger)).

# 🧠 Key concepts

- Modular structure following **clean architecture** and **composition laws**
- Separation of concerns: UI (`App`), logic (`Services`), data (`Data`)
- Input validation using helper methods
- ADO.NET database interactions with **parameterized queries** (no Entity Framework)
- Auto-seeded database


# 🖼 Screenshots

## Main Menu
![Main Menu]())

---

## Habit Report
![Habit Report]()

## Logging an entry
![Logging an entry]()

# 🧭 Architecture Overview

This diagram shows how the program is structured:

![UML Diagram]())



# ▶️ How to Run

To run this project, you’ll need the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed locally.

# Prerequisites
- .NET SDL 8.0 or later
- (Optional) Visual Studio 2022+ for IDE support

  Run the Project
  ```bash
  # Clone the full portfolio repo
  git clone https://github.com/Ana-Anna/CSharp-Academy-Portfolio.git

  # Navigate to the project folder
  cd CSharp-Academy-Portfolio/03_HabitLogger

  # Restore dependencies
  dotnet restore

  # Build the project
  dotnet build

  # Run the game
  dotnet run

