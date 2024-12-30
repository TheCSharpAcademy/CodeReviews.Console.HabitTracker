# Console HabitTracker

Console based CRUD application to track habits. Developed using C# and SQLite.

# Given Requirements:
- [x] Create an application where you’ll log occurrences of a habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] Users need to be able to input the date of the occurrence of the habit
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- [x] Follow the DRY Principle, and avoid code repetition.

# Features
* A console based UI where users can navigate by key presses
* SQLite database connection
	- The program uses a SQLite db connection to store and read information. 
	- If database does not already exist, or the correct table is nout found in the database, the program will create the database and table upon start.
* CRUD DB functions
	- Using the menu, users can create, read, update or delete database entries.
	- Date and number inputs are validated upon user entry, before they can be stored in the database.

# Observations / Lessons Learned
- I am relatively new to C#, and have never used SQLite. Learning new technologies is always fun but can be a bit challenging when getting started.
- Implementing classes in individual files, although a bit of work at first, improved organization and made the code much cleaner.
- Using the debugger tools in Visual Studio makes life much easier, and can save tons of debugging time!
