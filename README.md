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
- Seed data with a few habits & a hundred random records for testing
- Create a report functionality to view information from *SQL* (times ran in a year, how far)
- Present a menu with *CRUD* operations (insert, delete, update, retrieve)
- Ability to let users create own habits & the unit of measurement for each
- Use parameterized queries for security
- Close when specified
- Use *ADO.Net* to interact with DB
- Have a *README.md* explaining how it works

# Features
* SQLite database connection
   - The program uses a SQLite database connection to store and read information
   - The program makes a database on program start if none exists

* CRUB database functions
   - The user can use CRUB functions from the main menu.
   - Uses parameterized queries for security 

* Create and delete own habits
   - The user can create their own habits with their own unit of measurement
   - Can also delete habits
   - The program first searches for the existence of the habit

* Easy to look at tables
   - Look 
* A console based UI controlled with key presses
 -![image](/images/HabitTracker-MainMenu.PNG)

* Easy to look at tables using *Spectre Library*
-![image](../images/HabitTracker-ViewRecords.png)

* Statistics for current year, month, and all time
-![image](../images/HabitTracker-StatisticsMenu.png)
-![image](../images/HabitTracker-Statistics.png)

# Challenges
   - This my first time user SQLite so I had a hard time with the syntax
   - The first iteration of this app had more than 10 classes and I couldn't make it work
   - I had a lot of trouble with *DateTime* taking a lot of time to understand the documentation
   - It is also my first time using *Spectre*, it was difficult to understand at first, but in the end helped me lower my code lines.
   - This was the hardest project I've ever done in my life. It took me one day and a half to finish and most of the time was spend pondering on how to connect parts of my project.

# Lessons Learned
   - After struggling to do anything, I tried making a flowchard using pen and paper. It helped me think of the structure of my program and I couldn't have finished the project without it.
   - ChatGPT is great at teaching, but I told him to never teach me the answer in code and just give me "obscure hints" so I can solve it myself.
   - Learning shortcuts for VS helped in my efficiency. To name a few, "F2" changes all instances of anything in the scope it is in. "F5" to start debugging. "Alt + Shift + C" to add a new item(e.g. classes).
   - Breakpoints are a godsend. A lot of bugs I couldn't figure out by reading code got solved just by running the code one statement at a time.