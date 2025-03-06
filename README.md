# Habit Tracker

This application allows you to create and log habits, such as how many glasses of water you drink per day.  Its a console based CRUD application, developed with C# and SQLite.

## Given Requirements

+ This is an application where you’ll log occurrences of a habit, that should be tracked by quantity.
+ Users need to be able to input the date of the occurrence of the habit
+ The application should store and retrieve data from a real database
+ When the application starts, it should create a sqlite database, if one isn’t present.
+ It should also create a table in the database, where the habit will be logged.
+ The users should be able to insert, delete, update and view their logged habit.
+ You should handle all possible errors so that the application never crashes.
+ You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
+ Follow the DRY Principle, and avoid code repetition.

## Additional Features

+ For date inputs, the user can type 'today' and the app will enter today's date.
+ I used parameterised queries.
+ The user can create their own habits to track, including choosing a unit of measure.
+ The app has started data seeded when the database gets created for the first time.
+ The app utilises Spectre.Console to manage the menus.

## How it works

+ SQLite database connection.
  - Two tables are created on start, if they dont exist, and populated with some seed data.
  - The tables are linked with a foreign key on Habit Id.
 + The console based UI has menus that can be manipualted with the arrow keys.
   
![image](https://github.com/user-attachments/assets/603ce1bd-7956-4315-b901-4de45ccd4db1)

- Selecting Manage your habits takes you to the habit menu, where you can View/Insert/Delete or Update your habits.
- Selecting Manage Habit Logs takes you to the Habit Logs Menu, where you can View/Insert/Delete or Update your logs for a chosen habit.

+ Data entry is guided by relevant questions, and the user inputs are validated.
 
+ Data is displayed in tables.  The Habit Log table is displayed next to the Habit table, and the habit that it applies to is rendered in green, to the user can visualise the connection..
  
![image](https://github.com/user-attachments/assets/2bc00b03-9766-4457-b18e-dbdee01315cf)

## Challenges

+ This was my first real try at OOP, and also at CRUD.  It was a challenge trying to incorporate both at the same time.  I feel that I have grasped general concept of both OOP and CRUD.
+ Although I tried to refactor my code, to incorporate DRY principles, I found that I struggled trying to reuse the SQLite code, although I could clearly see that I was reusing code.

## Known Issues

+ When closing the application manually by closing the console, the database gets locked, and crashes on next run.  I feel that the data integrity could be made more secure, but I lack the skill to fix this at present.

## Tools and Technologies Used

+ Visual Studio
+ C#
+ SQLite
+ DB Browser
+ Spectre.Console
+ ADO.Net
