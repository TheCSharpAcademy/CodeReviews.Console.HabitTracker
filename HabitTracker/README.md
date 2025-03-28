# Enhanced Habit Logger

## Description

Habit Logger is a console application built with C# and .NET that allows users to define and track multiple habits based on quantity (e.g., glasses of water per day, km ran). It stores the data persistently in a local SQLite database using raw ADO.NET commands, ensuring security through parameterized queries.

## Features

*   **Custom Habits:** Define your own habits with specific names and units of measurement (e.g., "Running" with unit "km", "Water Intake" with unit "glasses").
*   **Track Habit by Quantity:** Log how many units of a specific habit you performed on a given date.
*   **SQLite Database:** Uses a local SQLite database file (`habit_logger.db`). The database and necessary tables (`Habits`, `HabitLog`) are created automatically on first run.
*   **Data Seeding:** Automatically populates the database with sample habits and ~100 random log entries on the very first run (or if the database is empty) to facilitate testing and development.
*   **Log Management (CRUD):**
    *   **View:** See all logged entries, grouped by habit and sorted by date.
    *   **Insert:** Add a new log entry for a chosen habit, specifying date and quantity.
    *   **Delete:** Remove an existing log entry by its unique Log ID.
    *   **Update:** Modify the date and/or quantity of an existing log entry by its unique Log ID.
*   **Habit Management:**
    *   **View:** List all defined habits and their units.
    *   **Create:** Add new habits to track.
*   **Reporting:**
    *   **Yearly Summary:** View the total count and total quantity recorded for a specific habit within a chosen year.
*   **Error Handling:** Includes handling for invalid input, database constraints (e.g., unique habit names), and potential database connection issues.
*   **Secure Database Interaction:** Uses parameterized queries via ADO.NET (`Microsoft.Data.Sqlite`) exclusively to prevent SQL injection vulnerabilities. No ORMs (Entity Framework, Dapper) are used.

## Technology Stack

*   **Language:** C#
*   **Framework:** .NET (e.g., .NET 8 or later)
*   **Database:** SQLite
*   **Database Access:** ADO.NET (`Microsoft.Data.Sqlite`)

## Setup and Running the Application

1.  **Prerequisites:**
    *   .NET SDK (version compatible with the project's target framework, e.g., .NET 8.0 SDK) installed. Download from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).
2.  **Clone or Download:** Get the project files.
3.  **Navigate to Project Directory:** Open a terminal/command prompt and `cd` into the `HabitLogger` folder (containing `HabitLogger.csproj`).
4.  **Restore Dependencies:** Run:
    ```bash
    dotnet restore
    ```
5.  **Run the Application:** Execute:
    ```bash
    dotnet run
    ```
6.  **Database File:** A file named `habit_logger.db` will be created in the run directory. It contains sample data on first launch. **Do not delete this file unless you want to reset all data.**

## How to Use

The application presents a main menu: