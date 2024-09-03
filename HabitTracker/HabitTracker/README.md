## Console Habit Tracker

Console based CRUD application to track habits. Developed using C# SQLite.

## Given Requirements:

 **This is an application where you’ll log occurrences of a habit.
 **This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
 **Users need to be able to input the date of the occurrence of the habit
 **The application should store and retrieve data from a real database
 **When the application starts, it should create a sqlite database, if one isn’t present.
 **It should also create a table in the database, where the habit will be logged.
 **The users should be able to insert, delete, update and view their logged habit.
 **You should handle all possible errors so that the application never crashes.
 **You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
 **Your project needs to contain a Read Me file where you'll explain how your app works.

 ## Features:

 -Program uses a SQLite db connection to read and store information
 -There is menu where you can navigate through options to delete, insert, view or update habits
 -Every input is checked that it could be correct.
 -You can add Habits and habit's records to track.

 ## Challenges

 -It was hard to understand how database works with my code, but i got used to it.
 -It was hard to organise my code correctly.
 -Ensuring all inputs, such as dates and quantities, were validated before being added to the database helped to avoid corrupt data.