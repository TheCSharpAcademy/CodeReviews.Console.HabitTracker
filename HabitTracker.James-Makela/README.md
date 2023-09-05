# Habit Tracker

---

Console based CRUD application to track various habits.
Developed using C# and SQLite.


# Given Requirements

---

- [x] This is an application where you'll register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] The application should store and retrieve data from a real database.
- [x] When the application  starts, it should create a sqlite database, if one isn't present. 
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The app should show the user a menu of options.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works.

## Extra Challenges

- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.


# Features

---
This program is designed to track one or more habits of the users choice. On opening the program, if a habit table does not exist, the user must choose a name for their habit and the units to measure. This program has a main menu that allows users to insert, delete, update, and view records. The user can also run a few queries on a table(min, max, average), and the user can add more habits to track.

# Challenges

---
- Working with SQLite in C# was a challenge, particularly learning the syntax to populate a list with data to then print out.
- Implementing further habits was very challenging as it required modifying nearly every function I had written up to that point.
- Working out the last few bugs at the end, ensuring the program doesn't crash when there is no data.
# Areas to Improve

- The program file haas become quite long, it would be good to split the functions into other files for better organisation.
- There is no end to different queries that could be implemented.
- Storing the date in format YYYY-MM-DD would make it easier to have WHERE statements to select date ranges.