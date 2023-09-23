# Habit Tracker

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

# Features

* SQLite database connection

	- The program uses a SQLite db connection to store and read information. 
	- If no database exists, or the correct table does not exist they will be created on program start.

* CRUD DB functions

	- From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in dd-MM-yy format.
	- Time and Dates inputted are checked to make sure they are in the correct and realistic format. 

* Basic report of coffee cups consumed

# Challenges
	
- Connecting the database was somewhat challenging and took mutiple attempts 
- I could not manage to view the dtabase within the editor	

# Areas to Improve
- Add the challenges, such as tracking multiple habits
- Don't have all the classes inside the main, rather have calls to classes within main

# Resources Used
- [Habit Tracker App. C# Beginner Project. CRUD Console, Sqlite, VSCode]([https://www.youtube.com/watch?v=gfkTfcpWqAY&list=PLTjRvDozrdlz3_FPXwb6lX_HoGXa09Yef](https://youtu.be/d1JIJdDVFjs)https://youtu.be/d1JIJdDVFjs)
- ChatGPT
