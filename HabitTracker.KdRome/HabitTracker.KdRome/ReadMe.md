Console Habit Tracker

This was my first C# application using an SQL database.
Console based CRUD application to track number of time a task is being done per day.
In this case the user will be tracking how many cups of water they drank each day.

Given Requirements:

* The application should store and retrieve data from a real database.
* When the application starts a database will be created if it doesnt exists yet.
* You need to be able to insert, delete, update and view your habits.
* The applicaiton should only be terminated when the user inserts 0, validations on user's inputs.
* You can only interact with the database using raw SQL. You can't use mappers such as Entity Framework.

Features:

* CRUD DB function
- The user and create, read, update, and delete habits, and the number of times they practiced the habit.
* SQLite database connection
- The program uses a SQLite db connection to store and read information
- If no database exits, the program will create one on startup with a first habit.

Challenges:

* This project was my first using both C# and SQLite.
* While i had knowledge of SQL, being able to apply it correctly took a few tries to get it right.

Lessons Learned:
* How to open a connection with a database.
* Better understanding of using SQL within C#
* How to query the database using SQLite to get proper data.
* How to use and install packages using NuGet package manager.

Areas to imporove:
* While the application funtions, there are areas for improvements especially with redundent code which may be present.
* The applicaiton is coded within a single file, seperating some of the functions would allow for a cleaner and neater project.

Resources Used:
* StackOverflow
* CodeAcademy C# course
* geeksforgeeks.org