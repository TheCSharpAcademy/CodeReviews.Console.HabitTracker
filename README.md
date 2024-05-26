# ConsoleHabitTracker
C# Console Application to Log Habits.
Console based CRUD application to log Habits.
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
- [x] Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example: 

# Features

* SQLite database connection

	- The program uses a SQLite db connection to store and read information. 
	- If no database exists, or the correct table does not exist they will be created on program start.

* A console based UI where users can navigate by key presses
 
 	- ![image](https://github.com/javedkhan2k2/Csharpacademy/assets/48986371/c107b2bd-9f7a-4d07-905d-f6987275f480)

* CRUD DB functions

	- From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in yyyy-MM-dd format. 
	- Time and Dates inputted are checked to make sure they are in the correct and realistic format. 

* View All Habit Logs by pressing 1
	- ![image] (https://github.com/javedkhan2k2/Csharpacademy/assets/48986371/2fa6d659-6537-434a-a11f-79bcff90ae6e)
* To log new Habit press 2
	- First all the Habits are displayed so the users can enter a valid HabitId from the list
		- ![image] (https://github.com/javedkhan2k2/Csharpacademy/assets/48986371/6a71eede-e1a2-4c76-b468-a4d011153892)
	- Next User will enter Unit Quantity and a Valid date in yyyy-MM-dd format.
		- ![image] (https://github.com/javedkhan2k2/Csharpacademy/assets/48986371/16a8dbf1-ccb2-409f-8e03-d73d92c099b6)
* To delete a Record press 3
	- Enter the Id from the list to delete the Habitlog.
		- ![image] (https://github.com/javedkhan2k2/Csharpacademy/assets/48986371/487490c4-02c1-4452-91bf-67eb4efd3729)
* To update a Record press 4
	- Enter the Id from the list to update the Habitlog, next enter new quantity and Date to update it.
		- ![image] (https://github.com/javedkhan2k2/Csharpacademy/assets/48986371/1fe98179-7d55-45d7-9524-58dc1fb0aef7)
* To view all Habit press 5
	- [image] (https://github.com/javedkhan2k2/Csharpacademy/assets/48986371/8047004c-5b88-4f37-81f0-88c9bd1451ef)
* To Enter a New Habit press 6
	- User will entered Habit Description and Unit
		- [image] (https://github.com/javedkhan2k2/Csharpacademy/assets/48986371/0e966580-7ac8-402d-a89f-c4052806c80e)
* Basic Report for current Year
	- ![image](https://github.com/javedkhan2k2/Csharpacademy/assets/48986371/b623a3e5-74b9-422e-8f5c-c47abd34d283)



# Challenges
	
- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
- [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.
	
# Areas to Improve
 - Need to move back to main menu
 - Still need to refactor the code to catch exceptions
 - Using Async to interact with database
