# ConsoleTimeLogger
My first C# application, and first time using Visual Studio. 

Console based CRUD application to track time spent coding.
Developed using C# and SQLite.


# Given Requirements:
- [-] Log occurrences of a habit
- [x] Track only quantity
- [x] Input date of occurrence
- [x] Store and retrieve data from a database (sqlite)
- [x] Upon start of application, automatically creates database if not present
- [x] Creates table in DB to log habit
- [x] Ability for users to insert, delete, update and view logs
- [-] Handles errors to prevent crashes
- [-] Only uses ADO.NET, don't use EF or Dapper yet
- [x] Implement DRY principle
- [x] Create ReadMe file containing documentation for application

# Features

* SQLite database connection

	- The program uses a SQLite db connection to store and read information. 
	- If no database exists, or the correct table does not exist they will be created on program start.

* A console based UI where users can navigate by key presses
 
 	<!-- - ![image](<image>.png) -->

* CRUD DB functions

	- From the main menu users can Create, Read, Update or Delete entries for whichever record Id they want, entered in integer format. 
	- Dates inputted are checked to make sure they are in the correct and realistic format. 

# Challenges
	
- It was my first time using SQLite and implementing secure paramterized queries. I had to learn how the flow of the using statements worked and how to execute the queries effectively
- I had to think about how to implement a effective loops to ensure user has a chance to enter correct inputs when inputs were invalid, and also allow users to cancel operations depending on their input
- SQLite. Even though I mentioned it above, it is worth mentioning again by itself. I had not used SQLite before and had never used C# to interact with it. I had to learn how to create commands to do all the CRUD operations and how to execute those commands. I also had to learn some SQL to use the proper SQL commands to SELECT, UPDATE, INSERT or DELETE along with modifiers to SELECT or DELETE only the desired rows. The reports also heavily relied on correct SQL statements. 
	
# Lessons Learned
- Determine SQL table fields to satisfy requirements of this project. 
- Do it the right way the first time. There were several instances where I would try implementing code and then run it to find it did not work. It may have slowed me down and doing some research and reading documentation was challenging but ultimately got me to achieve my goals
- Finding the corred extensions and packages was a hurdle, but once accomplishing it once, I was able to retrieve them immediately from scratch. Experience in that regard, helps ten-fold in understanding where to look and how to find them
- Implementing checks and validations in methods helps clean up the flow and process, especially for user inputs. 

# Areas to Improve
- I hope to improve on KISS principle, as some of the logic may be over-complicated and may not be necessary to check for certain conditions. However, I also want to balance the ability to account for edge cases and handle them accordingly.


# Resources Used
-