# Habit Tracker

## Description
The Habit Tracker application is designed to help users log and track their habits efficiently. It allows users to manage habits by adding new ones, deleting existing ones, and inserting data associated with each habit into an SQLite database. The user interface is built using WinForms.

## Features
- **Add Habit:** Create and add new habits specifying their name and metric type.
- **Delete Habit:** Remove existing habits from the database.
- **Insert Habit Data:** Insert data related to habits, specifying the habit type and metric value.

## Screenshots

![HabitMenu](https://github.com/GitNach/CodeReviews.Console.HabitTracker/assets/137569683/b4613813-e47b-4362-8aa2-0ab3fdd38713)

*Figure 1: Main Menu - Here you can access buttons for creating, inserting, and deleting habits.*

![image](https://github.com/GitNach/CodeReviews.Console.HabitTracker/assets/137569683/194486e1-f39c-49f9-b45f-6c27d46f0bb6)

*Figure 2: Create Habit - Specify the name and metric type (e.g., daily, weekly) for a new habit.*

![image](https://github.com/GitNach/CodeReviews.Console.HabitTracker/assets/137569683/734e9374-eb34-4f58-9cd2-9b2118a75716)

*Figure 3: Insert Habit Data - Insert data for an existing habit, specifying the type and metric value.*

## Usage
1. **Creating a Habit:**
   - Navigate to the "Create Habit" section from the main menu (Figure 1).
   - Enter the name and select the metric type (Figure 2).
   - Click "Create" to add the habit to the database.

2. **Inserting Habit Data:**
   - Go to the "Insert Data" section from the main menu (Figure 1).
   - Choose the habit type from the dropdown menu and enter the metric value (Figure 3).
   - Click "Insert" to store the data in the database.

3. **Deleting a Habit:**
   - In the main menu (Figure 1), select the habit you want to delete.
   - Click on the "Delete" button to remove the selected habit from the database.

## Technologies Used
- C# for application logic
- SQLite for database management
- WinForms for user interface development


