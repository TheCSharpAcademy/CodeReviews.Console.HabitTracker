# Habit Logger

Habit Logger is a C# console application designed for users to log the occurrences of their habits. It allows users to record various details about their habits, including the type of habit, the date of occurrence, the quantity, and the unit of measure. The application stores this data in an SQLite database and provides functionality to insert, delete, update, and view logged habits.

## Requirements

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
- [x] Your project needs to contain a Read Me file where you'll explain how your app works.

## Technologies Used

- **.NET**: Framework version 8.0
- **.NET Core**
- **C#**: Version 12.0
- SQLite

## Features
- Log Habits: Users can log occurrences of habits, specifying the habit type, date, quantity, and unit of measure.
- Database: Data is stored in an SQLite database. The application ensures the database and necessary tables are created on startup if they do not exist.
- CRUD Operations: Provides functionality to create, read, update, and delete habit records.
- Error Handling: Comprehensive error handling to ensure the application does not crash.
- Console Interface: The application interacts with the user via the console for all input and output operations.

## Project Structure

```plaintext
HabitLogger
├── Program.cs                  # Entry point of the application
├── HabitLoggerDatabase.cs      # Manages SQLite database connections and CRUD operations
├── Habit.cs                    # Represents the Habit entity
└── README.md                   # Project documentation
```

## Usage

<img width="273" alt="A console interface displaying the welcome message for Habit Logger and a menu with the following options: 1. Insert a habit 2. Get a habit 3. Get all habits 4. Update a habit 5. Delete a habit 6. View report on habit by type and date 7. View habits by date 0. Exit" src="https://github.com/user-attachments/assets/39ab3f98-51da-4cc5-a1f6-5f8a0cf7d286">

### Logging a Habit

1. Select the option to log a new habit.
1. Enter the habit type, date, quantity, and measure when prompted.

### Viewing Logged Habits
1. Select the option to view all logged habits to see a list of all your logged data.

### Updating a Habit
1. Select the option to update a habit.
1. Enter the ID of the habit you wish to update.
1. Follow the prompts to update the habit details.

### Deleting a Habit
1. Select the option to delete a habit.
1. Enter the ID of the habit you wish to delete.

### Database
The application uses SQLite for data storage. On application start, it checks for the existence of the database and the habit logger table, creating them if needed.