# Habit Tracker

A simple console application for tracking daily push-ups using SQLite for data storage.

## Features

- **View All Recorded Data:** Display all push-up records.
- **Add a New Entry:** Insert a new record with the date and quantity of push-ups.
- **Delete an Entry:** Remove an existing record.
- **Update an Entry:** Modify the date and quantity of an existing record.

## Requirements

- [.NET SDK]
- SQLite (included via Microsoft.Data.Sqlite - NuGet)

## Getting Started

1. **Clone the Repository:**

    ```bash/Windows Terminal
    git clone https://github.com/jakized/csharp-habit-tracker.git
    cd csharp-habit-tracker
    ```

2. **Build the Application:**

    ```
    dotnet build
    ```

3. **Run the Application:**

    ```
    dotnet run
    ```

4. **Usage:**

    - **View all recorded data:** Select option `1`.
    - **Add a new entry:** Select option `2`, then enter the date and number of push-ups.
    - **Delete an entry:** Select option `3`, then enter the ID of the record you want to delete.
    - **Update an entry:** Select option `4`, then enter the ID of the record and provide new data.

5. **Exit the Application:**

    Type `x` at any prompt to exit the application.

## Code Overview

- `Program.cs` - Contains the main logic for the application, including CRUD operations and user interactions.
- `PushUp.cs` - Represents a record of push-ups with properties for ID, date, and quantity.
