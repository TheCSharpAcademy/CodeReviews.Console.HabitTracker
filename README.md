# ConsoleHabitLogger Project by C# Academy

Project Link: https://www.thecsharpacademy.com/project/12/habit-logger

This was my first project working with databases and SQL.

## Project Requirements:

1.	The application allows users to log occurrences of a habit.
2.	Habits are tracked by quantity (e.g., number of glasses of water) rather than time (e.g., hours of sleep).
3.	Users can input the date of each habit occurrence.
4.	Data must be stored and retrieved from a real database.
5.	On startup, the application should create an SQLite database if one does not already exist.
6.	A table must be created in the database to store habit logs.
7.	Users should be able to insert, delete, update, and view their logged habits.
8.	The application should handle all possible errors to prevent crashes.
9.	Database interactions must be done using ADO.NET—ORMs like Entity Framework or Dapper are not allowed.
10. The DRY (Don't Repeat Yourself) principle should be followed to minimize code duplication.
11. The project must include a README file explaining how the application works.

## Additional Challenges

1.	Use parameterized queries to improve security
2.	Allow users to create their own habits, including specifying a unit of measurement.
3.	Seed the database with sample data when it is first created, including a few predefined habits and 100 randomly generated habit records.
4.	Implement a reporting feature to provide specific insights into logged habits.

## Challenges Faced

* This was my first experience working with SQLite and databases in general. I had to learn basic SQL syntax and how to interact with SQLite databases using C#.

* To start, I experimented with the SQLite command-line interface to learn and test SQL queries. Once I understood the basics, I built a simple C# application to read and write data to a database. However, I frequently encountered SQL logic errors and parsing issues.

* During development, I learned about the using statement, which defines the scope of SqlConnection and other disposable objects. While I never personally encountered database locking issues due to improperly disposed objects, I followed best practices by using using statements throughout my application.

* Another major challenge was managing messy, unstructured code. As the project grew, I ended up with spaghetti code and many unused methods. Eventually, I had to start over, drawing a diagram on paper to plan the structure of the code. Unfortunately, I threw it away before realizing I should have scanned it for future reference.


## Lessons Learned

* Plan ahead – Creating a flowchart or notes on program structure saves a lot of headaches and leads to cleaner code. Even a rough plan is better than none.

* SQL confidence – SQL was intimidating at first and I was postponing it for longer than necessary, but breaking it down into small, working examples helped me overcome that fear. This approach can be applied to learning other complex topics in programming.

* Using statements – I gained a better understanding of the using statement, which ensures objects implementing IDisposable are properly released, especially when working with unmanaged resources like database connections or files.

* Dependency Injection (DI) – Instead of initializing dependencies like database access and input handling directly within application class, I learned to pass them via the constructor. This makes the code more flexible by allowing dependencies to be changed without modifying the class itself. There is still much more to learn about DI, but this project was a good introduction.

## Areas for Improvement

* Handling External Database Changes – The application does not account for external modifications to the database (e.g., adding records outside the program or deleting the database during runtime), which could cause crashes. Implementing better error handling and validation checks could improve stability.

* Habit Unit Storage – I'm still unsure about the best approach for storing units of measurement for habits. My current solution uses a separate table, but I haven't found a cleaner alternative, aside from adding a redundant column to store the same information in each record.

## Main Resources Used

SQLite FAQ: https://www.sqlite.org/faq.html

ZetCode: C# SQLite Guide: https://zetcode.com/csharp/sqlite/

Reintech Blog Post on Parameterized Queries in ADO.NET: https://reintech.io/blog/mastering-parameterized-queries-ado-net

Various Stack Overflow Discussions
