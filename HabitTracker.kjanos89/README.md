# ConsoleHabitTracker
C# Console Application to Log Coding Habits.
Developed using C# and SQLite.


# Given Requirements:
- [x] This is an application where you’ll register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The app should show the user a menu of options. 
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example: 

# Features

* SQLite db with CRUD operations

	- The application creates a database and table if there is none.
	- If the database exists the program connects to it with every operation and closes the connection when the method is complete.

* CRUD db functions

	- Create a record by following instructions appearing on the screen and entering the information asked for. The record is saved automatically in the database.
    - Update records in the db (number of issues solved).
    - View records currently stored in the database.
    - Delete records by ID.
