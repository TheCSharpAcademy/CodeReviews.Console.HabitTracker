# HabitTrackerApp

Console based CRUD application to track habits. Developed using C# and SQLite.

# Given Requirements
- [x] This is an application where you’ll register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] The application should store and retrieve data from a real database.
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The app should show the user a menu of options.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.

# Features
* SQLite database connection
    - The program uses a SQLite db connection to store and read information.
    - If no database exists, or the correct table does not exist they will be created.

* A console based UI where users can navigate by key presses
    - ![image](https://github.com/kimfom01/HabitTrackerApp/blob/master/HabitTrackerApp/main%20menu.png)

* CRUD DB functions
    - From the main menu users can Create, Read, Update or Delete entries.

# Challenges
* It was my first time using SQLite. I had to learn this techology from the beginning in order to complete this project. I had to learn how to create commands to do all the CRUD operations and how to execute those commands. I also had to learn some SQL to use proper SQL commands to SELECT, UPDATE, INSERT or DELETE along with modifiers to SELECT or DELETE only the desired rows.

# Resources Used
* [W3Schools SQL Tutorial](https://www.w3schools.com/sql/default.asp)
* [Zetcode C# SQLite](https://zetcode.com/csharp/sqlite)
* [MS Docs (using statement)](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement)

