# Habit Tracker
[The Habit Tracker project](https://thecsharpacademy.com/project/12/habit-logger) is a @TheCSharpAcademy roadmap project that introduces you to CRUD operations against a database.

Developed with C# and SQLite.
![Screenshot of the Habit Tracker Main Menu](/assets/habit_tracker_main.png)

# Requirements:

 - :white_check_mark: Log occurences of a habit measured by quantity (not time). 
 - :white_check_mark: Users should be able to input a date of the habit occurence. 
 - :white_check_mark: The application should store and retrieve data from a real database.
 - :white_check_mark: When the application starts it should create a SQLite database (if one does not already exist). It should also create a table where the habit will be logged. 
 - :white_check_mark: The users should be able to insert, delete, update and view their logged habits. 
 - :white_check_mark: All possible errors should be handled to prevent application crashes.
 - :white_check_mark: Can only interact with the database using ADO NET (no mappers such as Entity Framework or Dapper). 
 - :white_check_mark: Follow the DRY Principle, and avoid code repetition. 
 - :white_check_mark: Project needs to contain a readme to explain how the application works.
 - :white_check_mark: **[Challenge]** Use parameterized queries to run the application more securely.
 - :white_check_mark: **[Challenge]** Let the users create their own habits to track, this includes allowing them to define the unit of measurement.
 - :white_check_mark: **[Challenge]** Seed data into the database automatically when the database gets created for the first time and inserting a hundred records with randomly generated values.
	 - 💬 **Note:** For this challenge the database creates 3 default habits on initialization. Additional habits and randomized habit log data can be seeded using the `debug` launch argument (see the features section).

# Dependencies
- Microsoft.Data.Sqlite (9.0.7)
- Spectre.Console (0.50.0)
# Features

### :floppy_disk: Database Initialization and Persistance
Upon first starting the application, if a local database does not already exist the application initializes the creation of a new database with some pre-defined habits:
- Water Intake (ml)
### :books: Data Seeding with debug launch argument
To seed the database with test data, launch the application with `debug` as  a launch argument.

Executable:
```
start HabitTracker.TruthfulUK.exe debug
```
Project:
```
dotnet run debug
```
### :pencil2: Manage Habit Logs
Full ability to manage habit logs. You will first be prompted to select a habit for all habit management options.
- Add a New Habit Log
	- Ability to add a new log by first selecting the habit from the menu, then the amount of the measurement you want to log followed finally by the date of the log (YYYY-MM-DD) or simply leave blank to default to the current day.
- View Habit Logs
	- Ability to view all logs for a chosen habit by selecting the habit from the menu.
- Delete a Habit Log
	- Ability to delete a specific habit log by first selecting the habit from the menu and then entering the respective ID # to delete from the database.
- Update a Habit Log
	- Ability to update a specific habit log by first selecting the habit from the menu and then entering the respective ID # to update the record. You will then be prompted to provide an updated measurement amount and date.

### :pencil2: Add a New Habit
Create a new habit by providing the habit name and unit of measurement. Once created you will be able to immediately start adding new habit logs from the Manage Habit Logs menu.

### :page_with_curl: Habit Reports
Ability to generate reports of your logged habits. There's currently 2 supported reports:
- Day Report
	- Generate a report for all habits logged on a specific date. Enter the requested date in the YYYY-MM-DD format or leave blank to default to the current day.
-  Total Logged by Habit
	- Generate a report providing the total amount logged (in their respective measurements) for each habit.

# Challenges
- Using SQL tested in DB Browser and converting to ADO NET commands with parameterised queries initially proved to be quite confusing.
- I initially over scoped to include full management of Habits which would of required additional logic to handle deleting and updating habit logs for deleted or updated habits. I removed these options after re-reviewing the project requirements and challenges.
-  Difficulty understanding / finding the Microsoft.Data.Sqlite documentation which replaced System.Data.Sqlite 
	
# Lessons Learned
- Splitting up functionality into UI, DB and manager classes helped to greatly keep logic contained and my code more readable.
- ChatGPT and Gemini can help greatly to parse documentation and translate it into examples or step-by-step breakdowns.
- Spectre.Console handles a lot of possible exceptions by ensuring the inputs are validated and type checked.



# Resources Used
- [Microsoft Learn - Microsoft.Data.Sqlite overview](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/)
- [Microsoft Learn - Use a SQLite database in a Windows app](https://learn.microsoft.com/en-us/windows/apps/develop/data-access/sqlite-data-access)
- [Microsoft Learn - Executing a Command](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/executing-a-command)
- [GitHub dotnet docs - SQLite Sample](https://github.com/dotnet/docs/blob/main/samples/snippets/standard/data/sqlite/HelloWorldSample/Program.cs)
- [SQLBolt - Interactive SQL Excercises](https://sqlbolt.com)
- [W3Schools SQL Data Types](https://www.w3schools.com/sql/sql_datatypes.asp)
- [DB Browser for SQLite](https://sqlitebrowser.org/)
- ChatGPT and Gemini to help with explaining sample code.