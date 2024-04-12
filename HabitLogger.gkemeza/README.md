# Habit Logger 📅

- This is my first C# database console application.
- The app tracks habits using basic CRUD operations.
- Developed using ADO.NET, SQLite and Visual Studio.

## Requirements:

1) The habits can't be tracked by time (ex. hours of sleep),  
only by quantity (ex. number of water glasses a day).
2) When the application starts, it should create an SQLite database,  
if one isn’t present.
4) It should also create a table in the database, where the habit will be logged.
5) Show the user a menu of options.
6) The users should be able to insert, delete, update and view their logged habits.
7) Handle all possible errors so that the application never crashes.
8) The application should only be terminated when the user inserts 0.
9) Only interact with the database using raw SQL.  
Don't use mappers such as Entity Framework.
11) Let the users create their habits to track.  
That will require them to choose the unit of measurement for each habit.
13) Seed Data into the database automatically when the database gets created  
for the first time, generating a few habits  
and inserting a hundred records with randomly generated values.
15) Create a report functionality where the users can view specific information.

## Features:

- SQLite database connection
  - The program uses an SQLite db connection to store and read information.
  - If no database exists, it will be created on start.
  - One table is for habits names, another for records storage.
    
- A console-based UI where users can navigate by key presses
- ![image](https://github.com/gkemeza/HabitLogger/assets/148207780/ebe4f9bf-4816-4d1f-8b33-3dae8442ec94)
- ![image](https://github.com/gkemeza/HabitLogger/assets/148207780/269ecfeb-cdda-4944-804b-f5e4c0fc852f)
    
- Menu functions
  - The first menu asks to choose a habit or create a new one.
  - In the second menu users can Create, Read, Update or Delete  
  entries for specific dates, entered in their local format.  
  As well as view reports.
  - Wrong inputs will output a helpful message for the user:
    - ![image](https://github.com/gkemeza/HabitLogger/assets/148207780/c27e0e5d-d2d6-4d18-b00b-b5226a352b33)

 
- Basic reports of the chosen habit data:
  - ![image](https://github.com/gkemeza/HabitLogger/assets/148207780/5547f4ba-878e-4ecd-817d-2aeb6d3ccabd)

## Things learned:

- Create/connect to a database.
- Basic SQLite queries.
- Structure the program into different classes and many methods.
- Get the user's local date format.
- Generate random dates.
- Thoroughly check for invalid user input.
- That Console.Clear() only clears visible output in the console.

## Resources used:
- Many articles and videos from:
  - Microsoft documentation, Google, AI, Youtube.
