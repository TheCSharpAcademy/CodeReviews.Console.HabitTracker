# ConsoleTimeLogger
Project by The C# Academy

Console based CRUD application to track habits.
Developed using C# and SQLite.


# Given Requirements:
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habits will be logged.
- [x] You need to be able to insert, delete, update and view your logged habits. 
- [x] You should handle all possible errors so that the application never crashes 
- [x] The application should only be terminated when the user inserts 0. 
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework

# Features

* SQLite database connection

	- The program uses a SQLite db connection to store and read information. 
	- If no database exists, or the correct table does not exist they will be created on program start.
