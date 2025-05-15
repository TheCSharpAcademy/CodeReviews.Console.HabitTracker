# Habit Tracker
Console based CRUD application to habit occurrences.
Developed using C#, SQLite and ADO.NET.


# Given Requirements:
- [x] Habits tracked by quantity.
- [x] Users can input the date of the occurrence of the habit
- [x] Store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] Users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] Only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- [x] Follow the DRY Principle, and avoid code repetition.

# Features
* SQLite database connection
	- The program uses a SQLite db connection to store and read information. 
	- If no database exists, or the correct table does not exist they will be created on program start.

* A console based UI where users can navigate by key presses.

* CRUD DB functions
	- From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in mm-DD-yyyy format. Duplicate days will not be inputted. 
	- Time and Dates inputted are checked to make sure they are in the correct and realistic format. 

	
# Areas to Improve
- I felt the codebase became a bit hectic. I would like to improve on code organization and creating better abstraction so it is easier for future devs to read.


# Resources Used
- [ADO.NET DOCS](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/)
- Everything else, either I had known how to do from other programming languages & projects, or I used google to figure out (no specific resource).
