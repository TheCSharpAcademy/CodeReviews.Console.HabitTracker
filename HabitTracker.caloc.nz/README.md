Followed the C# Academy YouTube tutorial and applied my learnings from the  
previous projects to this project. Needed to go back to previous projects and  
research some of the changes I made to to understand the changes I made.

## Project Requirements

- [x] This is an application where you’ll register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by  
quantity (ex. number of water glasses a day)
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one  
isn’t present.
- [x] It should also create a table in the database, where the habit will be  
logged.
- [x] The app should show the user a menu of options.
- [x] The users should be able to insert, delete, update and view their  
logged habit.
- [x] You should handle all possible errors so that the application never  
crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using raw SQL. You can’t use  
mappers such as Entity Framework.
- [x] Your project needs to contain a Read Me file where you'll explain how  
your app works.

## Program Overview

- When the program loads for the first time, it will create a SQLite DB.
- The SQLite DB will store the information entered by the users.
- Console based UI where user will select options and input using key presses.
- Displays a report, sorted by oldest input at the top of the list.
- Input accepts multiple date formtats (dd-MM-yy, dd-MM-yyyy, dd/MM/yy,  
dd/MM/yyyy).

## Code Overview

- Code is separated into multiple classes.
  - Menu.cs: Containts the menu items.
  - Helpers.cs: Contains the helper items, Get, Update, Delete.
  - Program.cs: Contains the remaining program items
  