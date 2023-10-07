# Habbit Tracker

CRUD application for tracking a habbit. In this case, habbit being tracked are ciggaretes smoked per day. 

# Disclamer

I do not advertise smoking in any way, just found it practical as an example for my first CRUD application

# Requrements

 ☑This is an application where you’ll register one habit.

 ☑This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)

 ☑The application should store and retrieve data from a real database

 ☑When the application starts, it should create a sqlite database, if one isn’t present.

 ☑It should also create a table in the database, where the habit will be logged.

 ☑The app should show the user a menu of options.

 ☑The users should be able to insert, delete, update and view their logged habit.

 ☑You should handle all possible errors so that the application never crashes.

 ☑The application should only be terminated when the user inserts 0.

 ☑You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.

 ☑Your project needs to contain a Read Me file where you'll explain how your app works.

 # Technologies used

 C#

SQLite

 Written in Visual Studio Community 2022

 # Features

 - This application uses SQLite db connection to read and write information
 - If no database exists, one will be created
 - Main menu has options to View, Insert, Delete and Update records
 
 # Personal notes

 My first CRUD application. Had some issues with parsing date and time in correct format. Making this application made me interested in learning more about databases.
