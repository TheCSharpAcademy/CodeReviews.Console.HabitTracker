# ConsoleRunLogger
My second c# application.

Console based CRUD application to track running miles.
Developed using C# and SQLite.


# Given Requirements:
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the miles will be logged.
- [x] You need to be able to insert, delete, update and view your logged miles. 
- [x] You should handle all possible errors so that the application never crashes 
- [x] The application should only be terminated when the user inserts 0. 
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework
- [x] Reporting Capabilities

# Features

* SQLite database connection

	- The program uses a SQLite db connection to store and read information. 
	- If no database exists, or the correct table does not exist they will be created on program start.

* A console based UI where users can navigate by key presses
 

* CRUD DB functions

	- From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in yyyy-mm-dd format. 
	- Time and Dates inputted are checked to make sure they are in the correct and realistic format. 


* Reporting and other data output uses ConsoleTableExt library to output in a more pleasant way


# Challenges
	
- It was my first time using SQLite and second c# project. I had to read documentation and articles each of these technologies from the beginning in order to complete this project. 
- DateTime was a hurdle to get over. I had to learn how to parse into and from DateTime into either more storable or human readable formats.
- The main class got cluttered, and not using classes as I am used to required me to be intentional with the layout of my code.
- SQLite. Even though I mentioned it above, it is worth mentioning again by itself. I had not used SQLite before and had never used C# to interact with it. I had to learn how to create commands to do all the CRUD operations and how to execute those commands. I also had to learn some SQL to use the proper SQL commands to SELECT, UPDATE, INSERT or DELETE along with modifiers to SELECT or DELETE only the desired rows. 
	
# Lessons Learned
- Create a basic map of what some of the objects in the program should be and some of their basic methods before starting to code. I think this would help in not making spaghetti code from the beginning.
- Create classes in my code to reflect DB objects if needing to pass them along functions or methods.
- SQL quirks. While setting up the DB, I ran into many bugs that come from DB settings being set on creation that would not reflect my updated commands. For example, making a column UNIQUE after forgetting to do so initially.

# Areas to Improve
- Learn more about code snippets. I currently only know the code snipping for Console.WriteLine();. I should learn more, as this did spead up my coding. I couldn't find an included one for Console.ReadLine(), however I may be able to add more through my IDE.
- The menu case statements feel hacky, as I do more console projects I may iterate on my menu implementation.
- Although I made sure my data was flowed clean, it would be nice if I placed error handling wrappers around my DB functions to prepare for possibly corrupted data.

# Resources Used
- [MS docs for setting up SQLite with C#](https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
- [MS docs for DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime?view=net-5.0)
- Various StackOverflow articles
- "Sams Teach Yourself SQL in 10 Minutes a Day" by Ben Forta
