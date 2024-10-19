# ConsoleHabitTracker

Console based CRUD application to track people habits. Developed using C# and SQLite with ADO.NET.

## Given Requirements:
- [x] This is an application where you log occurrences of a habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day).
- [x] Users need to be able to input the date of the occurrence of the habit.
- [x] The application should store and retrieve data from a real database.
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] Should be handled all possible errors so that the application never crashes.
- [x] Iteraction can only be with the database using ADO.NET. Can’t use mappers such as Entity Framework or Dapper.

## Features
* SQLite database connection with ADO.NET

* The program uses a SQLite db connection to store and read information.
  - If no database exists, or the correct table does not exist they will be created on program start.

* A console based UI where users can navigate by user input.

* CRUD DB functions
  - From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in dd.mm.yyyy format. Duplicate days will sum quantity of habit occurances.

* Reports of days and occurances that happened during one year. User can choose which year to check.

* Measurement of habit, which is chosen by user in creation process of a new habit and can't be changed later for that specific habit.

* Seeded sample of 3 habit tables with 100 different occurances each.

## Challenges
- It wasn't my first or even second time of SQL like db usage, but definitely first one when I was using SQLite with ADO.NET. It was a little challenged to write the right query, especially with parametrized queries, which not always works with SQLite.
- Other little problem was to handle all the input's validation and parsing different types of user's inputs.
- One specific task was a bit harder then other - to add a measurement feature to habits, where I had to design a very specific solution to create an empty row in every table, but I think that it could be handled in much better way.
## Lessons Learned
- Debugging is very useful tool, which was not very often used by me previously.
- SQL commands can be tricky but you can get almost everything with a right specific query.
- Handling user's input with Regex is a very handy way to do it.
Areas to Improve
- Better code writing with DRY and KISS principles, better error and inputs handling, more different libraries and frameworks to acknowledge that I can try more different features.
## Resources Used
- C# Academy guidelines and roadmap.
- ChatGPT for very inconvenient bags and new information like DateTimes and SQLite parametrized queries.
- W3School and CodeAcademy for SQL queries.
- Various StackOverflow articles.
