# Habit Tracker
C# Console Application built as part of **The C# Academy's** roadmap.

Developed using C# and SQLite. Allows the user to track premade or custom habits.

# Given Requirements:

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

# Features

* SQLite database connection

	- This application uses a SQLite database connection to store and read information. 
	- If no database is found, a new database will be created and multiple sample habits and records will be inserted.

* A console based UI made with Spectre.Console for easier user navigation.
 
 	- image

* CRUD DB functions

	- From the main menu users can view, insert and delete records of their existing habits.
	- Users can also create their own custom habits with their own unit of measurement.

* Basic Reports of Records

	- Users can generate reports to see how many times they practised a habit in a specific year.
	- image

# Challenges
	
- It was my first time using SQLite. I had previous basic knowledge on how to query a database but this served as a great way to take said knowledge to the practice.
- Had a couple interface bugs that made the console ask for the same menu option multiple times. It was caused by menu screens getting unintentionally nested.

# Lessons Learned
- I've improved my skills on how to query and manage a database through code.
- Improved C# logical thinking and syntax.
- Learned techniques to format strings.

# Resources Used
- The C# Academy foundamental C# course.
- Various StackOverflow and Reddit articles.
- AI to generate the sample records (they were a lot and I'm to lazy to make 100 records manually).