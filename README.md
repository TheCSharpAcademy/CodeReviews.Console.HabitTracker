# HabitTracker
HabitTracker is a simple C# console app that interacts with a SQlite Database to store your habbits. This has been my first time performing CRUD operations against a databse.

## Requirments
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] Users need to be able to input the date of the occurrence of the habit
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.

## Features
* SQlite Database
  - Creates a Databse if one is not presents.
  - Allows users to perform CRUD operations against the databse.
* User Interface
  - Neat Console user Interface.
  - Allows users to navigate through various operations through keyboard.
* Simple Report function
  - Number of entries.
  - Sum of entries.
  - Average value of entries.
  - List of all entriess of specified habit
