# Habit Tracker Application

This is a C# console application for tracking and managing habits. The application allows users to track their habits, record data related to those habits, and perform various actions such as viewing, adding, updating, and deleting habit data. Below, I will provide an overview of the key components of this application and how to use it.

## Table of Contents
- [Overview](#overview)
- [Usage](#usage)
- [Components](#components)
  - [UserInterface Class](#userinterface-class)
  - [InputValidation Class](#inputvalidation-class)
  - [DatabaseAccess Class](#databaseaccess-class)
  - [HabitDataHandler Class](#habitdatahandler-class)
  - [Helpers Class](#helpers-class)
  - [HabitManagementHandler Class](#habitmanagementhandler-class)
- [Getting Started](#getting-started)
- [Contributing](#contributing)
- [License](#license)

## Overview

This C# console application provides a user-friendly interface for managing habits. Users can interact with the application through a menu-driven interface to perform various tasks related to habit tracking. The application stores habit data in a SQLite database and allows users to create, update, delete, and switch between different habits.

## Usage

1. **Main Menu**: When you run the application, it presents a main menu with the following options:
   - View all habit data
   - Add new habit entry
   - Update an existing entry
   - Delete an entry
   - Create a new habit
   - Switch habit
   - Delete a habit
   - Exit Habit Tracker

2. **View Habit Data**: Choose option 1 to view all habit data for the current habit. It displays a table with records showing the date, quantity, and unit of measure for the selected habit.

3. **Add New Habit Entry**: Select option 2 to add a new entry for the current habit. You will be prompted to enter the date and quantity for the entry.

4. **Update or Delete Entry**: Options 3 and 4 allow you to update or delete an existing entry. You'll be prompted to enter the ID of the record you want to modify or delete.

5. **Create a New Habit**: Option 5 allows you to create a new habit. You'll need to specify the habit's name and the unit of measure.

6. **Switch Habit**: Option 6 lets you switch between different habits that you have created.

7. **Delete a Habit**: Option 7 allows you to delete a habit. Note that the "drinking_water" habit cannot be deleted.

8. **Exit**: Select option 0 to exit the Habit Tracker application.

## Components

### UserInterface Class

- Manages the main menu and user interactions.
- Provides options to perform various tasks related to habit tracking.

### InputValidation Class

- Validates user inputs for quantity and date.
- Ensures inputs meet certain criteria and allows returning to the main menu.

### DatabaseAccess Class

- Handles interactions with the SQLite database.
- Provides methods for creating tables, querying habit data, and deleting habits.

### HabitDataHandler Class

- Contains methods for viewing, inserting, and modifying habit data.
- Allows users to switch between different habits.

### Helpers Class

- Provides a helper method for displaying headers in a consistent format.

### HabitManagementHandler Class

- Manages the creation and deletion of habits.
- Validates habit names and handles user confirmation.

## Getting Started

1. **Prerequisites**: Ensure you have the .NET runtime installed on your system.

2. **Clone the Repository**: Clone this repository to your local machine.

3. **Build and Run**: Open the project in your preferred C# IDE (e.g., Visual Studio) and build/run the application.

4. **Follow the Console Prompts**: Use the application by following the prompts in the console.

## Contributing

Contributions are welcome! If you'd like to improve this application or add new features, please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
