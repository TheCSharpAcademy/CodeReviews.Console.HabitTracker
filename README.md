# Habit Logger

Habit Logger is a console crud application written in C# that will allow you to keep track of your individual habits.

# Given Requirements:
1. This is an application where you’ll log occurrences of a habit.
2. This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
3. Users need to be able to input the date of the occurrence of the habit
4. The application should store and retrieve data from a real database
5. When the application starts, it should create a sqlite database, if one isn’t present.
6. It should also create a table in the database, where the habit will be logged.
7. The users should be able to insert, delete, update and view their logged habit.
8. You should handle all possible errors so that the application never crashes.
9. You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
10. Follow the DRY Principle, and avoid code repetition.
11. Your project needs to contain a Read Me file where you'll explain how your app works.

# Features
- SQLite Database Connection
   - When first run will create a database to be used for habit storage as well as storage of a record of each habit.
- Console based application using Spectre Console allowing easy navigation between options.
- Allows users to create, retrieve, update and delete information in regard to individual habits.

# External libraries used:
[Spectre Console](https://github.com/spectreconsole/spectre.console)

[Microsoft.Data.Sqlite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=net-cli)
   
