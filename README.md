# Habit Tracker Application

Console based CRUD application that allows users to track sessions of custom hobbies
Developed using C# and SQLite in Visual Studio.


# Given Requirements:
- [x] The user should be able to log occurences of a habit and input the date of its occurence.
- [x] The application should store and retrieve data from a real database.
- [x] When the application starts, it should create a sqlite database, if one is not present
- [x] It should also create a table in the database, if it doesn't exist, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] The application should handle all possible errors so that it never crashes.
- [x] Interaction with the database can only be done using ADO.NET, no mappers.
- [x] Code follows the DRY Principple

# Features

* SQLite database connection

	- The program uses a SQLite db connection to store and read information. 
	- If no database exists, or the correct table does not exist they will be created on program start.

* A console based UI where users can navigate by key presses
 
 	- ![Image](https://github.com/user-attachments/assets/b80c1658-e6d5-4d76-9250-840c76104739)

* CRUD DB functions

	- From the main menu users can Create, Read, Update or Delete entries of habit sessions for a specific date including the quantity of said habit.
	- Dates inputted are checked to make sure they are in the correct format (dd-mm-yy). 

* Number of times habit was practiced report

	- ![Image](https://github.com/user-attachments/assets/4ae637f2-aac7-4a92-b2ee-393106e899be)

* Total amount of measurement unit of a hobby

	- ![Image](https://github.com/user-attachments/assets/641a3039-a92e-4c45-97c7-ba83647406d3)

# Challenges
	
- DateTime can be very tricky to work with! There is not a single "correct" way to parse DateTime values and practice is required in order to feel comfortable working with them.
- Trying to enforce DRY Principle to its maximum potential. After initial development, I spent some time refactoring, trying to reduce the total code as much as possible.

# Lessons Learned
- Design is important! If you design your app correctly before you even write a single piece of code, either by pseudo-code or UMLs, you get a considerate advantage later on during development.
- Focus at the task of hand. Whenever I found myself stuck during this project, it was almost exclusively when I tried doing many things at once, e.g. creating the user input methods while also thinkin of how to INSERT records.

# Areas to Improve
- Enforcing DRY Principle, by using generic types, reflection, and other advanced techniques

# Resources Used
- https://www.thecsharpacademy.com/project/12/habit-logger
- https://reintech.io/blog/mastering-parameterized-queries-ado-net
- https://www.youtube.com/watch?v=HQKwgk6XkIA&ab_channel=Avery