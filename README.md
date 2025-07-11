## Requirements

- This is an application where you’ll log occurrences of a habit.
- This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- Users need to be able to input the date of the occurrence of the habit
- The application should store and retrieve data from a real database
- When the application starts, it should create a sqlite database, if one isn’t present.
- It should also create a table in the database, where the habit will be logged.
- The users should be able to insert, delete, update and view their logged habit.
- You should handle all possible errors so that the application never crashes.
- You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- Follow the DRY Principle, and avoid code repetition.
- Your project needs to contain a Read Me file where you'll explain how your app works. 

## References
https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=net-cli

## Sample user case

- User starts app
- User provides name
- Options:
  - retrieve stored habits 
  - insert habit
  - update previous habits: count or date
  - delete single habit
  - delete all instances of a habit

## Still to do
- Make user dynamic -> add new prompt at start to provide a username
- Split up methods to separate files
- Fix printing to console
- Add some colour to messages.


