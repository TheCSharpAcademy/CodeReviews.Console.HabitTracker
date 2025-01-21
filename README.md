# Habit Logger Application

## Overview
The Habit Logger is a console application designed to help users track their daily habits. Users can add new habits, log progress, view records, and delete specific entries. This application is perfect for those looking to monitor and manage their habits effectively.

---

## How to Run the Application
1. Ensure `.NET` is installed on your system.
2. Open a terminal or command prompt in the project directory.
3. Run the application using:
   ```bash
   dotnet run

## Main Menu Options

### 1. View All Habits
- **Description:** Displays a list of all stored habits with their IDs and units of measurement.
- **Usage:** Select option `1` from the main menu.

### 2. Add New Habit
- **Description:** Allows you to add a new habit with a name and unit of measurement.
- **Inputs:**
  - Habit name (e.g., "Drink Water").
  - Unit of measurement (e.g., "ml", "minutes").
- **Usage:** Select option `2` from the main menu. Follow the prompts to add your habit.
- **Note:** The habit will not be added if it already exists.

### 3. Add Record for Habit
- **Description:** Logs progress for a specific habit by recording the date and quantity.
- **Inputs:**
  - Habit name (if it doesn’t exist, you will be prompted to create it).
  - Date for the record.
  - Quantity to log.
- **Usage:** Select option `3` from the main menu. Follow the prompts to log a record.

### 4. View Habit Records
- **Description:** Displays all logged records for a specified habit, including dates and quantities.
- **Inputs:**
  - Habit name to view associated records.
- **Usage:** Select option `4` from the main menu. Enter the habit name when prompted.

### 5. Delete Habit Record
- **Description:** Deletes a specific record associated with a habit.
- **Inputs:**
  - Habit name.
  - Record ID to delete.
- **Usage:** Select option `5` from the main menu. Enter the habit name and the record ID to delete the entry.

### 0. Exit
- **Description:** Ends the application.
- **Usage:** Select option `0` from the main menu to terminate the program.

