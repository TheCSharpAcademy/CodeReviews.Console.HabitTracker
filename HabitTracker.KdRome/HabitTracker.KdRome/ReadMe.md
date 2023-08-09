# <span style="font-size:larger;">Console Habit Tracker</span>

This was my first C# application using an SQL database.
Console-based CRUD application to track the number of times a task is being done per day.
In this case, the user will be tracking how many cups of water they drank each day.

# <span style="font-size:larger;">Given Requirements:</span>

* The application should store and retrieve data from a real database.
* When the application starts a database will be created if it doesn't exist yet.
* You need to be able to insert, delete, update, and view your habits.
* The application should only be terminated when the user inserts 0, validations on the user's inputs.
* You can only interact with the database using raw SQL. You can't use mappers such as Entity Framework.

# <span style="font-size:larger;">Features:</span>
* CRUD DB function
- ◦ The user creates, reads, updates, and deletes habits, and the number of times they practiced the habit.
* SQLite database connection
- ◦ The program uses an SQLite db connection to store and read information
- ◦ If no database exists, the program will create one on startup with a first habit.

# <span style="font-size:larger;">Challenges:</span>
* This project was my first using both C# and SQLite.
* While I had some knowledge of SQL, being able to apply it correctly took a few tries to get it right.

# <span style="font-size:larger;">Lessons Learned:</span>
* How to open a connection with a database.
* Better understanding of using SQL within C#
* How to query the database using SQLite to get proper data.
* How to use and install packages using NuGet package manager.

# <span style="font-size:larger;">Areas to improve:</span>
* While the application functions, there are areas for improvements, especially with redundant code which may be present.
* The application is coded within a single file, separating some of the functions would allow for a cleaner and neater project.

# <span style="font-size:larger;">Resources Used:</span>
* StackOverflow
* CodeAcademy C# course
* geeksforgeeks.org
