# Console.HabitTracker by fdv

## My Project for creating a habit tracker in C#.  When you start the program, you can add, delete, update, or view habits and there quantities.

### Given Requirements:
1. This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
2. Users need to be able to input the date of the occurrence of the habit
3. The application should store and retrieve data from a real database
4. When the application starts, it should create a sqlite database, if one isn’t present.
5. It should also create a table in the database, where the habit will be logged.
6. The users should be able to insert, delete, update and view their logged habit.
7. You should handle all possible errors so that the application never crashes.
8. You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
9. Follow the DRY Principle, and avoid code repetition.
10. Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:

#### How to use
* There are 50 values randomly added to the database when it is created.
* You can track multiple habits, I'm trying to decide if it should be limited or add an option to Add a habit and unit of measurement.
* You can add a new habit, update an existing habit, or delete one.
