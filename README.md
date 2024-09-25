# Console.HabitTracker

## Description
This is a simple habit tracker console application. It allows users to create, read, update, and delete habits. The application saves the habits to a sqlite databased stored on the local client so that they can be accessed later.

## Given Requirements
- This is an application where you’ll log occurrences of a habit.
- This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- Users need to be able to input the date of the occurrence of the habit
- The application should store and retrieve data from a real database
- When the application starts, it should create a sqlite database, if one isn’t present.
- It should also create a table in the database, where the habit will be logged.
- The users should be able to insert, delete, update and view their logged habit.
- You should handle all possible errors so that the application never crashes.
- You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:

## How it works
- When the application starts, it checks if the database exists. If it doesn't, it creates a new one.
- The user is then presented with a menu of options to choose from.
- The user can choose to:
  - Add a new habit
  - View all habits
  - Update a habit
  - Delete a habit
  - Exit the application
- When adding a new habit, the user is prompted to enter the habit name and the date of the occurrence.
- When viewing all habits, the user is presented with a list of all habits in the database.
- When updating a habit, the user is prompted to enter all new habit information.

## Resources Used
- [SQLite](https://www.sqlite.org/index.html)