# ConsoleHabitTracker
My first C# application with CRUD operations.

Console based CRUD application to track habits.
Developed using C# and SQLite.

# Given Requirements
- [x] This is an application where you'll log occurances of a hbit
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] Users need to be able to input the date of the occurrence of the habit
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- [x] Your project needs to contain a Read Me file

# Features
* SQLite database connection

	- The program uses a SQLite database connection to store and read information
	- If no database exists, or the current table doesn't exist, they will be created on the program start.

* A console based UI where users can naviagted by pressing keys
	
* CRUD DB functions

* Basic Reports of Cumulative Hours

# Challenges

- This was my first time using CRUD with C# so I had a lot to learn about the syntax and features
- Datetime brought sligh trouble since I had to learn how to parse it from a text to a date and grab only the month
- Figuring out how to organize my code and trying to follow MVC architecture was difficult
- Figuring out the ins and outs of SQLite on parsing, accessing data and general usage

# Lessons Learned

- I learnt about dependecy injection and why it's so important
- I learnt more about SQLite and casting with it
- Got a better understandng of C#'s syntax and features
- Better understood the introduction to Object Orienated Programming 

# Areas to Improve

- - I want to learn more of the shortcuts and keybinds of Visual Studio Code as I want to be able to traverse through files without clicking with my mouse or moving throughout the code more efficiently
- Single responibility, I'm not too sure I followed it properly as I don't have enough experience but think I could have made my programs more seperated
- I could have better utilized OOP to store the values and more seperate the habits 

# Resources used

- [Habit Tracker App. C# Beginner Project. CRUD Console, Sqlite, VSCode](https://www.youtube.com/watch?v=d1JIJdDVFjs)
- [Get datetime value from X days go](https://stackoverflow.com/questions/14008778/get-datetime-value-from-x-days-go)
- [SQLite date() Function](https://www.sqlitetutorial.net/sqlite-date-functions/sqlite-date-function/)
- [C# KISS Principle (Keep It Simple, Stupid!)](https://www.bytehide.com/blog/kiss-principle-csharp)
- [Datatypes In SQLite](https://www.sqlite.org/datatype3.html)
- [Difference Between "Text" and "String" datatype in SQLite](https://stackoverflow.com/questions/11938401/difference-between-text-and-string-datatype-in-sqlite)