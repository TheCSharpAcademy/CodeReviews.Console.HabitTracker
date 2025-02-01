# Habit_Logger
 This is a very simple app that will teach you how to perform CRUD operations against a real database. The first application where I connect to the database myself.
 
# Requirements
- [x] This is an application where you’ll log occurrences of a habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] Users need to be able to input the date of the occurrence of the habit
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- [x] Follow the DRY Principle, and avoid code repetition.
---------
# Features
- SQLite database connection.
    - The program uses a SQLite db connection to store and read information.
    - If no database exists, or the correct table does not exist they will be created on program start.
- A console based UI where users can navigate by key presses
    - ![image](https://github.com/user-attachments/assets/72cd7ebf-fd60-4b5b-9842-a030719b42c4)
- CRUD DB functions
    - From the main menu users can Create, Read, Update or Delete records for whichever date they want, entered in dd-MM-yy format. 
    - Input validation is implemented: a warning message will appear if an incorrect date or unavailable menu option is entered.
    - 
 --------------
 # Challenges
- This is my first time connect a database with the console application, it took me some times to figure out how to do it.
- The swicth between difference menu was a bit of mess when I tried to implement the input validation.
 ---------------
 # Lessons Learned
- Make a clear plan for the application, write down the fuction you need to implement and the thing you need to check.
- Keep the structure clearn and easy to read, remember the Single Responsibility prinsiable.
 ---------------
 # Areas to Improve
- [ ] Using parameterized queries to make your application more secure.
- [ ] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [ ] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
- [ ] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.
