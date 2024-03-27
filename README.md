# Console Habit Tracker
A habit tracker console application is designed to help users track and manage their habits through a text-based interface in the console window
Developed using C# and SQLite.

## Requirements
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

## Challenges
- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
- [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

## Features
* A console based UI where the user can navigate by key presses

![VsDebugConsole_BKYEGDRrVz](https://github.com/GetTeched/CodeReviews.Console.HabitTracker/assets/1556111/4d344bd8-9617-4d59-a77e-3982d580510c)

* SQLite Database
     - Using SQLite to store and read information
     - If no database or table exists it will automatically create one and sample table _drinking_water_

* CRUD Functions
   - Directly from the Main Menu the user can perform all CRUD functions (Create, Read, Update and Delete)
   - View all available tables in the database
   - Switch between habit trackers with ease from the Main Menu
   - Date validation to ensure that dates are correctly formatted
   - Drop table functionality - delete all data and associated table *(User is warned multiple times before action is taken.)*

* Reporting

![image](https://github.com/GetTeched/CodeReviews.Console.HabitTracker/assets/1556111/a1820b6b-01d8-4b43-8c7c-0be2f7b736c0)

## Installation
When launching your habit tracker please ensure to disable the following in the Program.cs
Currently set to testingMode as true, this is just to test the habit trackers ability with multiple tables and data.

With testing enabled it will generate the following:
* 5 Habit tracker tables
* Each with their own unit of measurement
* Random amount of entries between 100 and 365
* Random entries will be generated per table with dates and quantities

```
static void Main(string[] args)
{
    //Set testingMode to false
    bool testingMode = true; 

    if (testingMode)
    {
        randomData.GenerateRandomData();
    }
    else
    {
        sqlCommands.SqlInitialize("drinking_water", "Glasses");
    }

    GetUserInput();
}
```

## Lessons Learned

- Working with SQL CRUD functions
- Drawing a menu flow helped me keep track of the flow the user will experience and ensure that it is easy to navigate
- Should use a console table library to format instead of wasting time getting everything aligned manually. (This has not been implemented in this project.)

## Resources
- Help and advice from [Richwestcoast](https://github.com/richwestcoast)
- [MS SQLite Documentation](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
- [SQLite Documentation](https://www.sqlite.org/docs.html)
- StackOverflow Articles
