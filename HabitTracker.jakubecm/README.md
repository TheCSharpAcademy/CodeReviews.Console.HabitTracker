# HabitTracker
The third project of the [C# Academy](https://thecsharpacademy.com/).

Console based CRUD application to log habits.

# Building and Running the Project

If you want to try this project out, here is how you can build it and run it on your machine.
Ensure you have .NET SDK installed on your machine. You can download it from [here](https://dotnet.microsoft.com/download).

### Using Visual Studio

1. Open the solution file (`.sln`) in Visual Studio.
2. Select the desired build configuration (Debug or Release) from the dropdown menu.
3. Press the green "Start" button (or F5) to build and run the project.

### Using the Command Line

1. Open a terminal or command prompt.
2. Navigate to the project directory.
3. Run the following command to build the project:
   ```sh
   dotnet build
   ```
4. Run the following command to run the project:
   ```sh
   dotnet run
   ```

# Given Requirements:
- [x] This is an application where you’ll register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present. It should also create a table in the database, where the habit will be logged.
- [x] The app should show the user a menu of options. 
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.

## Challenges
The following challenges have been completed for this project:
- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values.


# Habit Tracker Features

* SQLite database connection

	- The program uses a simple SQLite db connection to store and read information. 
	- If no database exists, it is created and one of the tables is seeded upon startup.

* A console based UI, navigated by key presses

* CRUD DB functions

	- The user can Create, Read, Update, and Delete habits from the database.
	- He can also create new habits and choose the unit of measurement for each habit.

# Functionality description

Upon starting the applicaton, the database either gets created and seeded with some habits and records, or it is connected to if it already exists. 
A major challenge with this was to ensure that if the database already does exist, the existing information is pulled out of the DB and displayed to the user.
After loading the database, the main menu is displayed and the user can choose whether he wants to log a new record, view all records, update a record, delete a record, or create a new habit.
The user can navigate the menu by pressing the corresponding key on the keyboard.
After finishing a desired operation, the applications continues running until the user inputs 0 to exit the application.