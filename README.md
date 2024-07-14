# Console HabitTracker
  
  Basic CRUD Application using C# and SQLite3
  Made using Visual Studio and VScode
  
# Given Requirments:
  [x] This is an application where you’ll register one habit.
  [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
  [x] The application should store and retrieve data from a real database
  [x] When the application starts, it should create a sqlite database, if one isn’t present.
  [x] It should also create a table in the database, where the habit will be logged.
  [x] The app should show the user a menu of options.
  [x] The users should be able to insert, delete, update and view their logged habit.
  [x] You should handle all possible errors so that the application never crashes.
  [x] The application should only be terminated when the user inserts 0.
  [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
  [x] Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:

# Features
  * SQLite Database
    * Connects to SQLite3 Database
    * Creates a new database if one doesnt exist
    * Creates and modifies new databases for each habit
  * Console UI that allows users to interact with the program by pressing keys
      * (image)
  * CRUD Database functionality
      * Create, Modify, or Delete Habits and their SQL Tables
      * Create, Modify, or Delete Records from a Habits Table

# Challenges
  * First time using Visual Studio. Had a few issues setting it up and many more with viewing the database. Ended up switching to VScode to debug issues with the database
  * SQLite with C# was more challenging to work with than I thought. I had worked with it in PHP before and had a much harder time getting it to work with C#

# Lessions Learned
  * Use the debugger. Had a lot of different issues that stumped me until I remembered to use it. At one point I had a weird glitch with a while loop that turned out to be a Console.Clear() failing silently the first two times a function was run. Wasnt able to figure out the reason for that but was able to remove the problematic code

# Areas To Improve
  * Reuseability. I dont know any specific points of my program that can be made more reuseable and efficient but feels like it can be much more efficient
  * I went into the project with no plan and while it didnt go that badly it was not the best way and would have been faster to take a moment to plan it out

# Resources Used
  * (MS docs for SQLite and C#) [https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=net-cli]
  * Multiple Stack Overflow articles for different issues I ran into and SQLite
  * SQLite and C# documentation
