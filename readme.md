## Description

This app is my first project that connects CMD App with SQL database file.
It helps to track any habits you wish to, choosing your habit name, unit name.

## Requirments

`Basic` Requirments:

    - This is an application where you’ll log occurrences of a habit.
    - This habit can't be tracked by time, only by quantity.
    - Users need to be able to input the date of the occurrence of the habit
    - The application should store and retrieve data from a real database
    - When app starts, it should create a sqlite database, if one isn’t present.
    - It should also create a table in the database, where the habit will be logged.
    - The users should be able to insert, delete, update and view their logged habit.
    - You should handle all possible errors so that the application never crashes.
    - You can only interact with the DB using ADO.NET. You can’t use mappers.
    - Project needs to contain a ReadMe file where you'll explain how app works.

`Advanced` Requirments:

    - Try using parameterized queries to make your application more secure
    - Let the users create their own habits (with their own unit) to track.
    - Seed data when DB gets created 1st time, inserting a 100 records.
    - Create a report functionality where the users can view specific information.

> [!NOTE]
> I changed a bit advanced requirments to challenge myself harder. :frog:

## Features

![UI Menu Screenshot](/assets/UI.png)

+ Full SQLite Database connection
 	- [x] Create, read, update, delete records
    - [x] Create, select, view habits
 	- [x] Generate random records with user criterias:
        * Amount of records
        * Amount of units
        * Year of records from 2020 to 2024

+ Console Interface (Navigation with numbers)  
+ Validation for:
 	- Dates
 	- Names for habits[^1]
 	- Numbers
    - Auto Counter of Records number and total amount

![UI Menu Screenshot](/assets/AutoCounter.png)

[^1]Every habit is created in new table, so any symbols that might be wrong for
SQL name are checked.

## Resources Used

+ GitHub
+ Stack Overflow
+ sqlite.org
+ [GitHub Emoji Cheat Sheet](github.com/ikatyang/emoji-cheat-sheet/tree/master)
+ ChatGPT (couple of minor issues)