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

* A console based UI where users can navigate by key presses

* CRUD DB functions

	- From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in dd-MM-yyyy format.
	- Time and Dates inputted are checked to make sure they are in the correct and realistic format. 

* Basic Reports of total measure quantity registered.

* Reporting  of total measure quantity registered by month.

# Challenges
	
- It was my first time using SQLite. I had to learn each of these technologies from the beginning in order to complete this project. 
- DateTime was a hurdle to get over. I had to learn how to parse into and from DateTime into either more storable or human readable formats.
- SQLite. Even though I mentioned it above, it is worth mentioning again by itself. I had not used SQLite before and had never used C# to interact with it. I had to learn how to create commands to do all the CRUD operations and how to execute those commands. I also had to learn some SQL to use the proper SQL commands to SELECT, UPDATE, INSERT or DELETE along with modifiers to SELECT or DELETE only the desired rows. The reports also heavily relied on correct SQL statements. 
	
# Lessons Learned
- Create a basic map of what some of the objects in the program should be and some of their basic methods before starting to code. I think this would help in not making spaghetti code from the beginning.
- Have a testing grounds readily set up. This way small things can be tested seperately from the within the app. This can speed up creation. For example, I was learning how to parse and use DateTime. A testing ground just to check parseing from and to DateTime could help speed up coding, since I would not need to compile this application each change/test and go through the UI. 

# Areas to Improve
- SQL skills to create better queries that allow to create better reports.

# Resources Used
- The help and advice of my mentor [Cappuccinocodes](https://github.com/cappuccinocodes)
- [Habit Tracker App. C# Beginner Project. CRUD Console, Sqlite, VSCode]([https://www.youtube.com/watch?v=gfkTfcpWqAY&list=PLTjRvDozrdlz3_FPXwb6lX_HoGXa09Yef](https://youtu.be/d1JIJdDVFjs)https://youtu.be/d1JIJdDVFjs)
- ChatGPT
