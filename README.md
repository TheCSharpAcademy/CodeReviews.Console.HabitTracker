# Habit Tracker

Console-based CRUD app to track learning JavaScript. Developed with C# and SQLite (using ADO.NET).

## Given Requirements:
* When the app starts, it create a SQLite DB, if one isn’t present.
* It further creates a table in said DB, where hours will be logged.
* You need to be able to insert, delete, update and view your logged hours.

* You should handle all possible errors so that the application never crashes.

* The application should only be terminated when the user inserts 0.

* You can only interact with the database using raw SQL.
* You can’t use mappers such as Entity Framework/Dapper
* Reporting Capabilities

## Features

* SQLite DB connection

 * The program uses a SQLite db connection to store and read information.
 * If no database exists, or the correct table does not exist they will be created on program start.

* Console-based UI navigable with key presses

* CRUD DB Functions
  * From the main menu users can Create Entries, View Stored Entries, Update or Delete entries by ID.
  * Time and Dates inputted are checked to make sure they are in the correct and realistic format.

## Challenges
* First time ever using SQLite, and any DB for that matter, so had to learn some basic SQL queries on account of the prohibition of ORMs.
* DateTime was tricky to implement correctly and make the SQLite DB work since it stores the dates as simple strings.
* Structuring the Main Menu and the workflow of the whole app proved a little more daunting than I had expected.

## Lessons Learned
* Treat DateTime conversions and parsing with great care.
* Create a basic plan of the app before building.
* Test each unit separately and as soon as creating to avoid later complications.

## Areas to Improve
* SQL queries
* Mapping out project structure

## Resources
* CSharpAcademy YT video on the topic
* DateTime docs to better understand conversion/parsing
* SQLite docs to get a basic understanding of queries






  
