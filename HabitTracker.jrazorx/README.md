# Habit Tracker Console Application

## Description

This is a simple console application to track the habit of drinking water by quantity. The application allows you to insert, view, update, and delete records of water glasses consumed. The data is stored in a SQLite database.

## Requirements

- .NET 8.0
- Microsoft.Data.Sqlite 8.0.6

## How to Run

1. Clone the repository or download the source code.
2. Open the solution.
3. Restore the NuGet packages.
4. Build and run the application.

## Usage

1. When the application starts, it will create a SQLite database if one isn’t present.
2. The main menu will be displayed with the following options:
   - Create Habit Type
   - View Habit Types
   - Update Habit Type
   - Delete Habit Type
   - Create Habit
   - View Habits
   - Update Habit
   - Delete Habit
   - Exit
3. Follow the prompts to interact with the application.

## Error Handling

The application handles all possible errors to ensure it never crashes. Any errors will be displayed to the user with appropriate messages.
