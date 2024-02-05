# Run Distance Habit Tracker
This is a simple CRUD console application using ADO.NET and Sqlite to store a habit; in my case, I am storing a run distance log entry which is an id, date, and distance in whole number format.

## How to Run
Clone project or download as zip.
Open solution in Visual Studio or another IDE.
Run project in Visual Studio.

## Requirements:
- This is an application where you’ll register one habit.
-This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of miles run in a day)
- The application should store and retrieve data from a real database
- When the application starts, it should create a sqlite database, if one isn’t present.
- It should also create a table in the database, where the habit will be logged.
- The app should show the user a menu of options.
- The users should be able to insert, delete, update and view their logged habit.
- You should handle all possible errors so that the application never crashes.
- The application should only be terminated when the user inserts 0.
- You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
- Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:

## What I Learned / Future Improvements:
I learned how to interact with SQLite in the context of a C# console app and run basic SQL commands.  I was able to use C# syntax and programming fundamentals such as switch logic and methods and classes.  Future improvements include seeding data upon app run for users, adding a report functionality to get insight on one's habit in the past year for example.