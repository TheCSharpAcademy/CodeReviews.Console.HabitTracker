# The C# Academy - Habit Logger 

## Introduction
This is a very simple app that will teach you how to perform CRUD operations against a real database. 
These operations are the base of web-development and you’ll be using them throughout your career in any most applications. 
We think it’s very important to do it from the start of your journey, since everything that will happen from here is just adding complexity to CRUD operations.
No matter how complex and fancy the app you’re building is, in the end it all comes down to executing CRUD calls to a database.

For that you’ll have to learn very simple SQL commands.
I know it sounds scary, but you’ll be amazed about how little SQL knowledge you need to build a full-stack app.
Don’t worry, we will take you by the hand and by the end you’ll have completed your first fully functioning CRUD app.
The most common ways of calling a SQL database with C# are through ADO.NET, Dapper and Entity Framework.
We will start by using ADO.NET, because it’s the closest to raw SQL.

If you think this project is too hard for you and you have no idea where to even start, you’re probably right. 
You might need an extra hand to build a real application on your own. 
If that’s the case, watch the video tutorial for this project and then come back and try it again on your own.
 It’s perfectly ok to feel lost, since most beginner courses don’t actually teach you how to build something.
 
## Requirements
 -  [x] This is an application where you’ll register one habit.
 -  [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
 -  [x] The application should store and retrieve data from a real database
 -  [x] When the application starts, it should create a sqlite database, if one isn’t present.
 -  [x] It should also create a table in the database, where the habit will be logged.
 -  [x] The app should show the user a menu of options.
 -  [x] The users should be able to insert, delete, update and view their logged habit.
 -  [x] You should handle all possible errors so that the application never crashes.
 -  [x] The application should only be terminated when the user inserts 0.
 -  [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
 -  [x] Your project needs to contain a Read Me file where you'll explain how your app works.
 
## Challenges
 -  [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
 -  [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

# Features

* SQLite database connection

	- The program uses a SQLite db connection to store and read information. 
	- If no database exists, or the correct table does not exist they will be created on program start.

* A console based UI where users can navigate by key presses
 
 	 ![1](https://github.com/Mo3ses/CodeReviews.Console.HabitTracker/assets/70375664/ab8b8497-59e4-4059-b21d-8dbcccc74fa4)


* CRUD DB functions

	- From the main menu users can Create, Read, Update or Delete.
	- Values, Time and Dates inputted are checked to make sure they are in the correct and realistic format. 

* Report Menu

  ![2](https://github.com/Mo3ses/CodeReviews.Console.HabitTracker/assets/70375664/592f8d2b-1666-4e50-8305-297dc788fd6b)
	 
* Reporting and other data output uses ConsoleTableExt library to output in a more pleasant way (Use a NerdFont to get the same visual)

	 ![3](https://github.com/Mo3ses/CodeReviews.Console.HabitTracker/assets/70375664/d28c8a04-3bd6-4814-a3a3-bb58068dc012)
	 - [GitHub for ConsoleTableExt Library](https://github.com/minhhungit/ConsoleTableExt)
  - [GitHub for FiraCode Nerd Font](https://github.com/tonsky/FiraCode)

* ConsoleTableExt Used on all my code to have a beautiful output in my program
	- Thanks to minhungit
	- [GitHub for ConsoleTableExt Library](https://github.com/minhhungit/ConsoleTableExt)
# Challenges
	
- It was my first time using SQLite. I had to learn from the beginning in order to complete this project. 
- DateTime was a hurdle to get over. I had to learn how to parse into and from DateTime into either more storable or human readable formats.
- There was also a issue with DateTime.I had an issue inputting Dates in the format my program wanted because SQLite dont accept string as Date.I was able to resolve this adding a try catch to force the user input the format that the system expects.
- SQLite. I already used normal SQL but SQLite is different I had a difficulty related to data types and conversions that I could do. 
	
# Lessons Learned
- How to read a Documentation, never used ConsoleTableExt Library and loved it for console projects, I'm thinking of re-doing the previous ones with this Library.
- The difficulty of making the system work without returning any problems and informing the user where the error is and what the application expects.

# Areas to Improve
- I want to learn more about github branch, start using github conventional commits and some other things on branch control.
- Still have issues with spaghetti-code. 

# Resources Used
- [ConsoleTableExt Library](https://github.com/minhhungit/ConsoleTableExt)
- [MS docs for setting up SQLite with C#](https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
- [MS docs for DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime?view=net-5.0)
- CodeCademy C# course to get some basic practice with C# variables, methods and classes.
