# Habit Tracker for The C# Academy

This was my first C# application using the Microsoft.data.sqlite library to store data permanently in an sqlite database along side the application.
The application is intended to track the frequency of a given habit, in this case "Drinking Water".

## Requirements

The application must have the following requirements:

- [x] This is an application where you’ll register one habit.
- [x]  This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x]  The application should store and retrieve data from a real database
- [x]  When the application starts, it should create a sqlite database, if one isn’t present.
- [x]  It should also create a table in the database, where the habit will be logged.
- [x]  The app should show the user a menu of options.
- [x]  The users should be able to insert, delete, update and view their logged habit.
- [x]  You should handle all possible errors so that the application never crashes.
- [x]  The application should only be terminated when the user inserts 0.
- [x]  You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
- [x]  Your project needs to contain a Read Me file where you'll explain how your app works.

# Features

- SQLite database connection
	- Creates an sqlite database file and table to store data in, if it doesn't exist
- Console Base UI
	- number entry based options (1 thru 4) for different actions
	- exit program by entering 0
- DB Functions
	- Ability to add, read, update and delete records through console entry
	- Dates entered are checked before being stored
	- Numbers are integer checked before being stored
	- Attempts to update or delete non-existing entries are rejected

# Further Learning

- Read into the usage of the DateTime class and how it can be used to validate
- Read into the proper usage of TryParse when out is `_`

# Resources Used

- [The C# Acadamy Project](https://thecsharpacademy.com/project/12)
- [Youtube tutorial](https://www.youtube.com/watch?v=d1JIJdDVFjs)