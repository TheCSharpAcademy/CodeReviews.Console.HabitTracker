# Coding Tracker Application

This is a console-based C# application that allows you to track coding sessions. You can insert, view, update, and delete coding records from a SQLite database.

## Features

- **View All Data**: Retrieve and display all the coding records from the database.
- **Insert New Record**: Add a new coding session with the date and duration.
- **Update Record**: Update the date and/or duration of an existing record by specifying the ID.
- **Delete Record**: Remove a record by specifying the ID.

## Prerequisites

- **.NET 6.0 SDK**: Make sure to install the .NET SDK from [here](https://dotnet.microsoft.com/download).
- **SQLite**: The application uses SQLite as the database. No external setup is required as it uses `Microsoft.Data.Sqlite`.

## Getting Started
Once the application is running, you will see the main menu with the following options:

------------------------------------------------
MAIN MENU
What would you like to do:
0 - Close application
1 - View all data
2 - Insert new record
3 - Update record
4 - Delete record
------------------------------------------------
Enter your option:
0: Close the application.
1: View all coding records in the database.
2: Insert a new coding session by entering the date and duration.
3: Update an existing record by providing the ID along with the new date and duration.
4: Delete a record by providing the ID.