# HabitTracker
A console based CRUD application to track a habit (distance walked).
Made as a project for [*The C# Academy*](https://thecsharpacademy.com/project/12/habit-logger)
Developed using C# and SQLite.

# Requirements
- Register records related to a habit
- Habits should only be tracked by quantity(e.g. number of water glasses in a day)
- Store and retrieve data from an external DB
- Handle errors to never crash
- If one doesn't exist, create an *SQLite* DB at launch
- Create a database table for habit logging 
- Present a menu with *CRUD* operations (insert, delete, update, retrieve)
- Close when specified
- Use *ADO.Net* to interact with DB
- Have a *README.md* explaining how it works

# Optional Requirements (To be implemented)
- Ability to let users create own habits & the unit of measurement for each
- Use parameterized queries for security
- Seed data with a few habits & a hundred random records for testing
- Create a report functionality to view information from *SQL* (times ran in a year, how far)
- Add voice input using *Azure AI Speech Recognition*

# Features
* SQLite database connection
   - The program uses a SQLite database connection to store and read information
   - The program makes a database on program start if none exists

* CRUB database functions
   - The user can use CRUB functions from the main menu.

* A console based UI controlled with key presses
 -![image](/images/HabitTracker-MainMenu.PNG)

* Easy to look at tables using *Spectre Library*
-![image](/images/HabitTracker-ViewRecords.PNG)
