HABIT TRACKER 
------------------------------------------------------------------------------------------------------
C# Console Application using Visual Studio 2022
Console based CRUD application to track a habit with a unit of measurement. Developed using C# and SQLite.

REQUIREMENTS
------------------------------------------------------------------------------------------------------
This is an application where you’ll log occurrences of a habit.
This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
Users need to be able to input the date of the occurrence of the habit
The application should store and retrieve data from a real database
When the application starts, it should create a sqlite database, if one isn’t present.
It should also create a table in the database, where the habit will be logged.
The users should be able to insert, delete, update and view their logged habit.
You should handle all possible errors so that the application never crashes.
You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
Follow the DRY Principle, and avoid code repetition.

FEATURES
-------------------------------------------------------------------------------------------------------
- SQLite database connection
    -The program uses a SQLite db connection to store and read information.
    - If no database exists, or the correct table does not exist they will be created on program start.

-  A console based UI where users can navigate by key presses

CRUD DB functions
From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in mm-DD-yyyy format. 
Dates inputted are checked to make sure they are in the correct and realistic format.

Basic Reports of
- Total logged habit instances
- Total unit of measurement for habit i.e kms for running
- Average unit of measure entered
