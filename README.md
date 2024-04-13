# Habit Tracker Console Application

My third C# console application.

Console based CRUD application to track habits.
Developed using C# and SQLite.

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
- [x] Your project needs to contain a Read Me file where you'll explain how your app works.

# Features

- SQLite database connection

  - The program uses a SQLite db connection to store and read information.
  - If no database exists, or the correct table does not exist they will be created when program starts.

- A console based UI where users can navigate by entering the number of the desired function

  - ![image](https://imgur.com/a/dhqi1v6)

- CRUD DB functions

  - From the main menu, users can Create, Read, Update or Delete habits.

# Challenges

- I am using VS Code in Linux. I had to deal with using VS Code as my editor. Visual Studio is not available in Linux.
- It was my first time using C# with SQLite. I had to learn how to connect SQLite to my C# project using NuGet in VS Code.
- Main method vs  Top-level statements. I had to learn how to implement the Main method. I had been doing my past projects using top-level statements. Time for a change.
- NAMESPACES. I had to learn what namespaces are and what they do.
- Relearning methodology and technology. I had to relearn both on how SQLite works and how to implement OOP to my project.

# Lessons Learned

- Main method is the way. Top-level statements are good for beginners or for those who are just learning the language. The Main method is definitely what I will be using now. I can organized my code the way I want it to be without confusion.
- Namespaces. I learned how it works but still have some confusions left.
- OOP is the best! I can't believe I didn't start using it before I got to the third project. I will be implementing it now on future projects.

# Areas to Improve

- Testing my application. There's too many things to test that I get bored on doing it. Maybe I'll let my friend test it. Wahhhhhhh.
- Namespaces. I guess I just have to create more projects dealing with it so that I can understand namespaces further.


# Resources Used

- Main Method in C# [Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/main-command-line)
- Add package [Microsoft Learn](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-dotnet-cli)
- [SQLite Tutorial](https://www.sqlitetutorial.net/)
- [Dani Krossing - C# Tutorials](https://youtube.com/playlist?list=PL0eyrZgxdwhxD9HhtpuZV22KxEJAZ55X-&si=GFW5CUyCbClBbZgZ)
- ChatGPT for clarifications on some ideas
- Various Reddit posts