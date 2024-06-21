# Habit Tracker

A console-based C# application for tracking habits using SQLite database.

## Description

This Habit Tracker allows users to create, view, update, and delete habits. It uses a SQLite database to store habit data and provides a simple console interface for interaction.

## Features

- Create, view, update, and delete habit types
- Create, view, update, and delete habit entries
- SQLite database integration
- Console-based user interface
- Error handling to prevent application crashes
- Input validation for user entries
- Automatic database and table creation on first run
- Seed data generation for initial setup
- Yearly report generation

## Requirements

- .NET 8.0
- SQLite

## Project Structure

The project consists of the following main components:

1. `Program.cs`: The entry point of the application.
2. `HabitTracker.cs`: Contains the main logic for the user interface and interaction.
3. `DatabaseManager.cs`: Handles all database operations.
4. `ReportManager.cs`: Manages report generation.
5. `Habit.cs`: Defines the Habit model.

## Setup and Running

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio Community 2022.
3. Ensure you have .NET 8.0 installed.
4. Build the solution to restore NuGet packages.
5. Run the application.

On first run, the application will create a SQLite database file named `habit_tracker.db` in the project directory and seed it with initial data.

## Usage

Upon running the application, you'll be presented with a menu of options:

1. Create Habit Type
2. View Habit Types
3. Update Habit Type
4. Delete Habit Type
5. Create Habit
6. View Habits
7. Update Habit
8. Delete Habit
9. Generate Yearly Report
0. Exit

Select an option by entering the corresponding number and follow the prompts.

## Database Schema

The application uses two main tables:

1. `HabitTypes`: Stores different types of habits
   - Id (INTEGER, Primary Key)
   - Name (TEXT)
   - Unit (TEXT)

2. `Habits`: Stores individual habit entries
   - Id (INTEGER, Primary Key)
   - Quantity (INTEGER)
   - Date (TEXT)
   - HabitTypeId (INTEGER, Foreign Key to HabitTypes)

## Error Handling

The application includes error handling to prevent crashes. In case of invalid input or database errors, appropriate error messages will be displayed to the user.

## Given Requirements

- [x] This is an application where you'll register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn't present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The app should show the user a menu of options.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using raw SQL. You can't use mappers such as Entity Framework.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works.

## Challenges

- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values.
- [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?)
