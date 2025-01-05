# Habit Tracker Console Application
My first ever experience with SQL and SQLite.

This is a console application to log and keep 
track of habits using a SQLite DB table, created 
using C# and Visual Studio Code.

# Requirements
- [x] This is an application where you'll log occurences of a habit
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

# Challenges
- This was my first ever experience using SQL/SQLite. The most challenging part was learning how it all worked. I learned how to create connections and commands, how to interact with the tables using those commands, how to execute the different types of commands(ExecuteNonQuery, ExecuteReader, and ExecuteScalar), etc.
- I think trying to learn how the SqliteDataReader works was pretty tough, but I was able to get it working enough to suit my needs.
- I managed to add another column to the table called 'Name'. When the user is prompted for Habit info, it now asks for the name of the Habit as well as the date performed and the quantity.