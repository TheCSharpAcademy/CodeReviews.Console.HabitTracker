# Habit Tracker
Console based CRUD application to track how many leetcode problems you've done. Developed using C# and SQLite.

# Given Requirements:
- [X] When the application starts, it should create a sqlite database, if one isn’t present.
- [X] It should also create a table in the database, where the number of problems will be logged.
- [X] You need to be able to insert, delete, update and view your logged number of problems.
- [X] You should handle all possible errors so that the application never crashes
- [X] The application should only be terminated when the user inserts 0.
- [X] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework
- [X] Reporting Capabilities
# Features
- SQLite database connection
  - The program uses a SQLite db connection to store and read information.
  - If no database exists, or the correct table does not exist they will be created on program start.
- A console based UI where users can navigate by key presses
  - ![image](/images/screenshot_1.png)
- CRUD DB functions
  - Number of Leetcode Problems and Dates inputted are checked to make sure they are in the correct and realistic format.
    
# Challenges
- This was my first time using SQLite.
# Lessons Learned
- Code is easier to understand/read when you have functions that do one thing (Single Responsibility Principle) and code is repeated (DRY Principle).