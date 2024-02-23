# Habit Tracker
Console Based CRUD application to track the ammount of times a user does a habit.

# Given Requirements
 - This is an application where you’ll register one habit.
 - This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
 - The application should store and retrieve data from a real database
 - When the application starts, it should create a sqlite database, if one isn’t present.
 - It should also create a table in the database, where the habit will be logged.
 - The app should show the user a menu of options.
 - The users should be able to insert, delete, update and view their logged habit.
 - You should handle all possible errors so that the application never crashes.
 - The application should only be terminated when the user inserts 0.
 - You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
 - Your project needs to contain a Read Me file where you'll explain how your app works.

# Features
 - SQLite database
   - Habits stored in a database
   - if no database exists one will be created.
 - Console based user interface
 - CRUD Functionality
 - Viewing data is presented by the
 - Logs Database operations into a JSON File
 - Seperate Log files, each time the Application is run with DD/MM/YYYY HH-MM datetime format
   
