# Habit Tracker
A console based CRUD app to track your bike riding habits.

## Given Requirements
- [x] This is an application where you’ll log occurrences of a habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] Users need to be able to input the date of the occurrence of the habit
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- [x] Follow the DRY Principle, and avoid code repetition.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works.

## Prerequisites

None!

## Features

	* SQLite Database Connection
		- The program uses a SQLite database connection to store and read information
		- The program will create both the expected database and the tables if either does not exist

	* CRUD Operations
		- The user has the ability to create, read, update, and delete data via the application

	* Console based UI
		- The user selects menu options by entering numbers

	* Input Validation
		- Validation on menu choices to ensure the selected option is an actual menu choice
		- Validation on date entries to be the expected dd-MM-yy input format, and input will continue until the date entry is the correct format

## Acknowledgments

* The C# Academy for guidelines and tutorials on completing this project
* Github user [thags for this README outline](https://github.com/thags/ConsoleTimeLogger/blob/master/README.md?plain=1)
