# HabitTracker
Console based CRUD application to track habit repetition across dates, C# with SQLite database.


# Given Requirements:

- [x] This is an application where you’ll register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The app should show the user a menu of options.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.


# Optional Features:

- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?).


# Features

* SQLite database connection

	- The program uses a SQLite db connection to store and read information. 
	- If no database exists, or the correct table does not exist they will be created on program start.

* A console based UI where users can navigate by key presses

   <img src="https://github.com/MartinL-no/MartinL-no/blob/images/HabitTracker1.png" width="300px" />

* CRUD DB functions

	- From the main menu users can Create, Read, Update or Delete entries for any date, entered in yyyy-MM-dd format.
	- Dates are checked to make sure they are in the correct format and valid. 

* View reports based on date:

	<img src="https://github.com/MartinL-no/MartinL-no/blob/images/HabitTracker2.png" width="300px" />

* Reports are formatted using the ConsoleTableExt library:

	<img src="https://github.com/MartinL-no/MartinL-no/blob/images/HabitTracker3.png" width="300px" />

# Challenges

- This was my first time working directly with ADO.NET in .NET/C#, using it was a little less user-friendly than than the higher level approaches such as Dapper and Entity framework I have used in the past but it's given me a better understanding of how interaction with databases works in .NET Core.
- I used the DateOnly struct in the C# Model to record the date, which required reformatting it's string format to work with SQLite.
- I tried to be consistent with my use of exceptions, which overall improved the structure of the code but required a little bit of different thinking.
	
# Lessons Learned
- Having good separation of concerns is important not matter the size of the project
- We're lucky to have tools like Dapper and Entity Framework

# Areas to Improve
- There is quite a lot of repetition in the repository class, part of that is due to the extra verbosity of using ADO.NET but I think that code repetition could be further reduced.
- Coupling beween the different layers of the application, it would be better if the dependencies were abstractions rather than concrete classes.
- Testing, testing, testing.

# Resources Used
- [Project guidelines from C# Academy](https://www.thecsharpacademy.com/)
- [MS docs for setting up SQLite with C#](https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
- [ADO.NET code examples on MS docs](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-code-examples)
- StackOverflow posts
