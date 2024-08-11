# HabitLogger
A console-based application to track your progress in habits per day. Developed using C# and SQLite.
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
- [x] Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:
# Features
- SQLite database connection
  - The data is stored in a db file which I connect to perform CRUD operations.
  - If database doesn't exist, it creates one.
  - In DEBUG mode, it creates an additional habit with 100 random generated records.
- Console based UI to navigate via keypress
   - ![image](https://github.com/user-attachments/assets/66f57057-f657-4f26-bac7-17382c6b9f7e)
   - ![image](https://github.com/user-attachments/assets/5b91c9b2-b386-4132-83ab-a6158b73f1b2)
- CRUD operations
  - From the first menu you can create, show or delete habits.
  - From the second menu you can create, update, show or delete. To choose an option you enter the number, for the day you enter the date in format YYYY-MM-DD.
  - Inputs are validated to be the requested type.
- Basic report functionality
   - ![image](https://github.com/user-attachments/assets/beb5f72b-3c3c-44b3-ad07-5d04e1c7e785)
# Challenges
 - I hadn't worked with an embedded database before, so I learned SQLite while working on the project. Also, the package from Microsoft to interact with SQLite was a challenge.
 - Generating seed data for dates but thanks to the DateTime class.
 - Code duplication for inputs and menu was solved using interfaces and generics.
 - For every habit, I have a table to list the tables and get an imaginary ID.
# Lessons Learned
- Reducing code duplication thanks to interfaces, inheritance and polymorphism.
- Reducing code duplication for different types thanks to generics with constraints.
- Creating new Git branches for features, allowing me to focus on one thing at a time.
- Using the Abstract Factory pattern to get the right input validator.
# Areas to Improve
- I want to keep learning about design patterns and improve how to organize my code.
- I was able to write a query using `ROW_NUMBER()`. Window functions seem really powerful and a nice area to explore.
- Code reusability is new to with both generics and interfaces, and I'm going to need them for high quality code.
 #  Resources used
 - StackOverflow posts
 - [Microsoft Docs Sqlite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=net-cli)
 - [Sqlite Window Functions](https://sqlite.org/windowfunctions.html)
