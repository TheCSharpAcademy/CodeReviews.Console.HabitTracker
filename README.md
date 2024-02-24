# HabitTracker
Console based CRUD application to track water intake
Developed using C# and SQLite

# Given Requirements:
- [x] When application starts, it creates a SQLite database if one is not present
- [x] Creates a database where water intake is logged
- [x] Shows a menu of options
- [x] Allows user to insert, delete, update, and view their water intake
- [x] Handles all errors so application doesn't crash
- [x] Only exits when user enters 0

# Features

* SQLite database connection
		
		- The program uses a SQLite db conneciton to store and read information.
		- If no database exists, or the correct table does not exist, they will be created when the program starts.

* A console based UI where users can navigate with commands
				
	![image](https://github.com/Fennikko/Images/blob/main/Screenshot%202024-02-23%20212510.png)


* CRUD DB functions

		- From the main menu users can close the application, view all records, insert a record, delete a record, or update a record.
		- Date and number of bottles are checked to make sure they are in the correct format.