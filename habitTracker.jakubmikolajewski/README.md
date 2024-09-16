
# Habit Tracker

My first app connected to a SQLite database. 
Console based CRUD app to track habits.

## Requirements
- This is an application where you’ll log occurrences of a habit.
- This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- Users need to be able to input the date of the occurrence of the habit
- The application should store and retrieve data from a real database
- When the application starts, it should create a sqlite database, if one isn’t present.
- It should also create a table in the database, where the habit will be logged.
- The users should be able to insert, delete, update and view their logged habit. You should handle all possible errors so that the application never crashes.
- You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- If you haven't, try using parameterized queries to make your application more secure.
- Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
- Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.


## Features

- SQLite database connection. The program uses a SQLite db connection to store and read information. If no database exists, a randomized database with 4 defaults habits will be generated.
- A console based UI where users can navigate by key presses
- CRUD database functions. All inputs are validated. DateTimes are checked to make sure they are in the correct format. Exceptions are handled.
- Report creation functionality.
## Challenges
- Parsing DateTimes correctly and storing them in the database in the same format to avoid exceptions when querying. (CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal)
- Untangling the code. Still feel like it could be refactored.

## Lessons Learned
- Refreshed my SQL skills and learned how to communicate with a SQLite database through C#.
- Stumbled onto https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/collection-expressions.
- Learned of the Random.Shared class.
- Did a better job of validating user input in a succinct way (although I'm sure it can be further improved, still).
- Improved notetaking process for future reference.
## Areas to improve
- Further clean up my notes and take more of them.
- Learn how to implement lambda expressions whenever possible.
- Keep function lenght to a minimum.

