# Console Habit Logger  
This is the Console Habit Logger app, created for [The C# Academy](https://www.thecsharpacademy.com/#), based on the requirements listed in the project.
The purpose of this app, is to learn and implement CRUD operations with SQLite database inside a Console application.

## Requirements  
- [x] This application should register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day).
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The app should show the user a menu of options.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using **raw SQL**. You can’t use mappers such as Entity Framework.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works. 

- [x] CHALLENGE: Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.

 ## App Overview  
 
 - The App comes with a self-contain database, which is created on first startup. It is a single file, named "HabitTrackerDB" and can be copied and shared among users. If the database will become corrupted (or deleted) a new one will be created when the app is launched.
 - The app allows for creation of countless habits to track and viewed via a Console UI 
 
    ![An image of the Main Menu](/assets/images/consoleUI.png "Main Menu")
    
- The app always assumes the data enter into it is from the present day (because you know... it's suppose to be a habit).
- The app allows for viewing, updating and deleting present day information, or all information. Such operation is performed for each habit separately.
- List view is provided for both current day progress and entire progress so far

    ![Habit Progress view](/assets/images/habitList.png "Selected Habit progress as a list")
    
- Habit Logger also allows for deletion of either present day information, or entire progress.
- Additional confirmation is required when deleting records, to prevent unfortunate accidents :)

    ![Habit Delete confirmation screen](/assets/images/deleteProgress.png "Delete confirmation screen")


## Technology Used  

- Habit Logger is powered by .NET 7
- SQlite Database is handled by System.Data.Sqlite.Core libraries. No Object Relation Mappers (ORM) were used.



