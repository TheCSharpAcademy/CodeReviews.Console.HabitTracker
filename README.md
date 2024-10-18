# HabitTracker App
My third project for the C# Academy and first using an SQLite database

Console based CRUD application to track instances of a habit, in this case drinking glasses of water. Developed using C# and SQLite.

## Given Requirements:

- This is an application where you’ll log occurrences of a habit.

- This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)

- Users need to be able to input the date of the occurrence of the habit

- The application should store and retrieve data from a real database

- When the application starts, it should create a sqlite database, if one isn’t present.

- It should also create a table in the database, where the habit will be logged.

- The users should be able to insert, delete, update and view their logged habit.

- You should handle all possible errors so that the application never crashes.

- You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.

- Follow the DRY Principle, and avoid code repetition.

- Your project needs to contain a Read Me file where you'll explain how your app works.

## Features
- SQLite Database Connection
- This program uses an SQLite database to store and access information.
- If no database/table exists when starting the program, it will create one automatically.

## Console Based UI
- This application featues a text based menu and navigation system to access its functions
- For most options, entering '0' will halt the operation and go back to the main menu. If entered from the main menu, it will exit the application.
- ![Screenshot of the main menu of the application.](https://rvnprojectstorage.blob.core.windows.net/images/habittracker_menu.png)

## CRUD DB Functions
The application offers database CRUD operations (Create, Read, Update, Delete) for any date entered.
The date needs to be in yyyy-MM-dd format and offers the ability to enter '1' to select today's date.
The quanity needs to be a positive integer greater than 0

## View Current Entries
- The main menu also offers the option to view all current entries in the database
- ![Screenshot of the the view operation](https://rvnprojectstorage.blob.core.windows.net/images/habittracker_viewrecords.png)


