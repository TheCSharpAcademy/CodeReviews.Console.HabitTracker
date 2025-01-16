# HabitTracker

HabitTracker is a simple CLI-based app that can be used to log occurences of a habit and store them in a database. In this case the logged habit is reading and the unit is a page.

# Tech stack
- C#
- ADO.NET
- SQLite

# Features
- The app uses SQLite database to store data.
- The user can interact with the database through command line interface.
- Available operations include inserting, updating, deleting and viewing the data.
- The user can also generate a report, that displays units and occurences of the habit in the selected year.

# Project structure

- Program.cs:
    - Entry point of the application
    - Manages the main loop
    - Interacts with other classes
- Interface.cs 
    - Contains UserInterface class which provides methods for interaction with the user, such as displaying the menu and getting user's input.
    - Contains Report class responsible for generating reports (currently only yearly reports).
- Database.cs
    - Contains DatabaseHandler class that creates the database and implements database operations.
    - Contains DatabaseRecord class that represents a database record with its values.

# Database schema
The application creates an SQLite database (database.db) with the following schema:
| Column  | Type    | Description                      |
|---------|---------|----------------------------------|
| id      | INTEGER | Primary key, auto-incremented.   |
| date    | TEXT    | Date of the habit (dd-MM-yyyy).  |
| amount  | INT     | Number of pages read.            |

# Running the application
To run the application you need to have .NET installed on your machine.

1. Clone the repository:
    ```
    git clone https://github.com/afilipkowski/CodeReviews.Console.Calculator
    cd HabitTracker.afilipkowski/HabitTracker.afilipkowski
    ```
2. Build the project:
    ```
    dotnet build
    ```
3. Run the application:
    ```
    dotnet run
    ```


