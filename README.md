# HabitLoggerCLI

Console baased CRUD application to track habits. Developed using C# and SQLite.

# Running the Application
Ensure you have .NET 8 SDK installed on your machine. 

Move into the console application using `cd HabitLogger.Console` (or similar on your operating system). Run the application using `dotnet run`. 

# Requirements

- This is an application where you’ll register one habit.
- This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- The application should store and retrieve data from a real database
- When the application starts, it should create a sqlite database, if one isn’t present.
- It should also create a table in the database, where the habit will be logged.
- The app should show the user a menu of options.
- The users should be able to insert, delete, update and view their logged habit.
- You should handle all possible errors so that the application never crashes.
- The application should only be terminated when the user inserts 0.
- You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.

# Features

- A console based UI for navigating the app.

- Connects to a SQLite database in order to create, read, update and delete habits.

  - Initializes a database with some dummy data if no database is found.

- Allows various views of habits
  - View all entries for a single habit
  - View all entries for every habit found in the database
  - View all habit entries for a particular date

# Resources Used

- [Stack Overflow "What is the best way to connect and use a sqlite database from C#"](https://stackoverflow.com/questions/26020/what-is-the-best-way-to-connect-and-use-a-sqlite-database-from-c-sharp)
