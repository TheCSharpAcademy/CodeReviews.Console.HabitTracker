# Habit Tracker

## Summary

An application that helps users develop a habit by allowing them to
easily log and track their behaviour.

## Features

- Create habit log
  - Users can create logs by entering quantity of habit (numerical value)
    and the time at which they did the activity aka log time.
- Display habit logs
- Update logs
  - Users can update a habit log by habit ID (can be found when viewing logs)
- Delete logs
  - Users can delete a habit log by habit ID (can be found when viewing logs)

## Run locally (development)

To run this locally,he application can be run locally via command line:

1. Clone this repository
2. `cd HabitTrackerProgram`
3. `dotnet run`

## Stack/Database details

- This C# console application uses ADO.NET to connect to an SQLite database.
- When the application starts, it should create a sqlite database,
  if one isn’t present.
- It should also create a table in the database, where the habit
  will be logged.

## Code organization

The source code has been organized into various modules/namespaces to
maintain separation of concerns:

- `HabitTrackerProgram/Program.cs`
  - Entrypoint
- `HabitTrackerProgram/Database`
  - Contains a `Connection` class to manage SQLite connection.
  - `HabitLogRepository` to query the database for habit logs.
- `HabitTrackerProgram/Model`
  - Contains the `HabitLog` model which encapsulates the data and
    business logic related to a habit log.
- `HabitTrackerProgram/Util`
  - Contains convenient utilities to allow code re-use across the application.
