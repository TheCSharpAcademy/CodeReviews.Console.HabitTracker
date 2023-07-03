# Console Habit Tracker
My second C# application.


A console-based CRUD application to track habits.
Developed using C# and SQLite.

# Given Requirements:
- [x] Upon application start, it should create a SQLite database if one doesn't already exist.
- [x] The application should create a table in the database to store habit records.
- [x] Users should be able to insert, delete, update, and view their habits.
- [x] All possible errors should be handled to prevent application crashes.
- [x] The application should only terminate when the user enters 0.
- [x] Raw SQL should be used for interacting with the database; mappers like Entity Framework are not allowed.
- [x] Reporting capabilities are required.

# Features

* SQLite database connection

	- The program establishes a connection to a SQLite database for storing and retrieving information.
	- If no database exists or the required table is missing, they will be created at program startup.

* Console-based UI with navigation using key presses
 
 	 ![image](https://github.com/alvaromosconi/CodeReviews.Console.HabitTracker/assets/77434507/c0dbeda7-30bf-42cd-a86f-f04beef9f6e3)

* CRUD database functions

	- Users can create, read, update, and delete habit entries from the main menu. Dates must be entered in the format dd-mm-yy.
	- Input validation ensures that time and dates are in the correct and realistic format.

* Basic reports of records grouped by habit name

	 ![image](https://github.com/alvaromosconi/CodeReviews.Console.HabitTracker/assets/77434507/f834daec-be8f-4cf2-b3f4-7db35c714699)

* New Habit or Record menu:

   ![image](https://github.com/alvaromosconi/CodeReviews.Console.HabitTracker/assets/77434507/ab9b73f7-e2ed-4979-8192-9a0dcda3c97e)

# Challenges
	
- Handling users input and manage program flow effectively.
- Working with SQLite for the first time, requiring learning the technology from scratch to complete the project.

# Areas to Improve
- Implement more input controls and provide more detailed and descriptive messages.
- Enhance functionality for tracking and adding habits (possibly by introducing additional database tables and more classes and logic in the program).

# Resources Used
- The CSharpAcademy guide.
- Various StackOverflow articles for C# syntax and resolving SQLite doubts.
