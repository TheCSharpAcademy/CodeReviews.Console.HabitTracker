# Console HabitTracker

Console Application using CRUD with SQLite Database

# Given Requirements:

- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the hours will be logged.
- [x] You need to be able to insert, delete, update and view your logged hours.
- [x] You should handle all possible errors so that the application never crashes
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework Reporting Capabilities
- [x]This is an application where you’ll register one habit.
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

# Challenge Requirements

- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
- [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

# Features

- SQLite database connection

  - The program uses a SQLite db connection to store and read information.
  - If no database exists, or the correct table does not exist they will be created on program start.
  - If tables are empty, 100 random records are generated for testing

- A console based UI where users can navigate by use of Spectre Console library
- CRUD DB functions
  - From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in mm-DD-yyyy format. Duplicate days will not be inputted.
  - Time and Dates inputted are checked to make sure they are in the correct and realistic format.
  - Users can also Create, Read, Update or Delete user defined habits
- Two basic reports are implemented (total count and quantity by habit)

# Challenges

- Being primarily familiar with JavaScript interaction with Databases (PostgreSQL), it is challenging to learn the library to interact with SQLite DB in C#.
- Transitioning back to object oriented programming led to interesting challenges while refactoring the code to "seperate concerns".
- Learning a new database interface (SQLite vs Postico)

# Lessons Learned

- Seperate concerns early into logical classes and files to avoid confusion trying to refactor later.
- It will take a bit, but focus on learning the libraries for DB interaction.

# Resources Used

- The C Sharp Academy [Habit Logger](https://thecsharpschool.getlearnworlds.com/course/habit-logger)
- MS .NET Documentation [.NET API](https://learn.microsoft.com/en-us/dotnet/api/?view=net-8.0)
