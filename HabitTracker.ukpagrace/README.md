# Project Title
Habit Tracker

## Description
A habit tracker used to track uses habits, allowing them create habits and enter input for them daily.

## Requirement
## Given Requirements:
- [x] This is an application where you’ll register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] The application should store and retrieve data from a real database.
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The app should show the user a menu of options.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works.

## Features
- **SQLITE database connection**
	-The Program uses a SQLITE db connection to store and read information
	-If No Database exist the uses will be prompted to create a table
-	-Once the User creates the Table, the will be prompted to select a table for the remaining operations
	-A console based UI, prompting the user to create a table
	- ![Console UI](images/images1.png)
	- ![Console UI](images/images2.png)
- ** CRUD DB functions**
	-A console based UI, Displaying crud operations
	- ![Console UI](images/images1.png)
	- The user can create Habit with option "1"
	- The user can insert into a habit with option "2" from the table selected at start up
	- The user can see all tracked habit with option "3"
	- The user can update a habit record with option "4"
	- The user can delete a habit record with option "5"
	- The user can list all habits created with option "6"
	- The user can see report on habit for each month with option "7"
	-A console based UI, displaying report
	- ![Console UI](images/images1.png)
	- The user can change habit table with option "8"
	- The user can exit the application with option "0"
Instructions and examples for use.

## Challenges
- Refactoring the code, and putting related files into class libraries

## Areas to Improve
- [] Adding option for going back to menu
- [] Adding Exceptions