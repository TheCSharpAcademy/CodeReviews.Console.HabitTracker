# Habit Tracker

## Description
A habit tracker used to track users' habits, allowing them to create habits and
enter input for them daily.

## Requirement

### Given Requirements
-[x] This is an application where you’ll register one habit.
-[x] This habit can't be tracked by time (e.g., hours of sleep) only by quantity 
  (e.g., number of water glasses a day).
-[x] The application should store and retrieve data from a real database.
-[x] When the application starts, it should create a SQLite database if one isn’t 
  present.
-[x] It should also create a table in the database, where the habit will be logged.
-[x] The app should show the user a menu of options.
-[x] The users should be able to insert, delete, update, and view their logged 
  habit.
-[x] You should handle all possible errors so that the application never crashes.
-[x] The application should only be terminated when the user inserts 0.
-[x] You can only interact with the database using raw SQL. You can’t use mappers 
  such as Entity Framework.
-[x] Your project needs to contain a ReadMe file where you'll explain how your app 
  works.

## Features

### SQLITE Database Connection
- The program uses a SQLITE db connection to store and read information.
- If no database exists, the user will be prompted to create a table.
- Once the user creates the table, they will be prompted to select a table for 
  the remaining operations.
- A console-based UI, prompting the user to create a table.

### CRUD DB Functions
- A console-based UI, displaying CRUD operations.
     ![Create Habit](link_to_image_for_create_habit)

  1. **Create a habit**: The user can create a habit with option "1".
     ![Create Habit](link_to_image_for_create_habit)
     ![Create Habit](link_to_image_for_create_habit)
  2. **Insert into a habit**: The user can insert into a habit with option "2" 
     from the table selected at startup.
  3. **View tracked habits**: The user can see all tracked habits with option 
     "3".
  4. **Update a habit record**: The user can update a habit record with option 
     "4".
  5. **Delete a habit record**: The user can delete a habit record with option 
     "5".
  6. **List all habits**: The user can list all habits created with option "6".
  7. **Report on a habit**: The user can see a report on a habit for each month 
     with option "7".
     - A console-based UI, displaying the report.
     ![Create Habit](link_to_image_for_create_habit)
  8. **Change the habit table**: The user can change the habit table with option 
     "8".
  9. **Exit the application**: The user can exit the application with option 
     "0".

## Challenges
- Refactoring the code and putting related files into class libraries.

## Areas to Improve
- Adding an option for going back to the menu.
- Adding exceptions.
