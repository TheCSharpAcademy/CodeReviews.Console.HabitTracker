This is a .Net project which demonstrates uses of CRUD operations against a real database, using very simple SQL commands. It's function is to track Habits with an Insert, Update and Delete feature. 


## Requirements

-  Log occurrences of a habit.
- Can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
-  Users need to be able to input the date of the occurrence of the habit
-  The application should store and retrieve data from a real database
- When the application starts, it should create a sqlite database, if one isn’t present.
- It should also create a table in the database, where the habit will be logged.
- The users should be able to insert, delete, update and view their logged habit.
- You should handle all possible errors so that the application never crashes.
- You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- Follow the DRY Principle, and avoid code repetition.
- Your project needs to contain a Read Me file

## Challenges
- Using parameterized queries to make your application more secure.
- Let the users create their own habits to track.
- Seed Data into the database.

## Usage

- The console app connects to the database
- A user menu is created which allows the user to make a choice
- Each menu option has a method that performs the function selected, by using the menu you can perform operations against the database to add, remove and edit entries.
- The data is stored in two tables: Habits and Records. They are linked using HabitID as the Foreign Key.
- The app stores and retrieves data from these tables.
- The user can perform CRUD operations on the Habit and Records.
- The app is exited through the menu.
- 
## Lessons Learned
- Infinite tables impacts the database so having one table that tracks habits and the other that tracks the records.
- Started learning more about seperation of concerns by having one class for my database, and another for the program.
