# Habit Logger - Water Consumption Tracker

## Overview

This is a simple command-line application for tracking and logging your daily water consumption. It allows you to register the quantity of water you drink each day and store this data in a SQLite database. You can perform various operations such as inserting new records, updating existing records, deleting records, viewing your logged water consumption, and generating a report.

## Features

- **User-friendly Menu:** The application provides a user-friendly menu with options to perform various actions.

- **SQLite Database:** The application uses a SQLite database to store and retrieve your water consumption data. If the database doesn't exist when the application starts, it creates one along with a table to store the habit data.

- **Data Logging:** You can log the quantity of water you drink each day. Unlike some habits that can be tracked by time, this application tracks the habit solely by quantity (e.g., number of glasses of water).

- **Error Handling:** The application is designed to handle all possible errors gracefully, ensuring that it never crashes and provides meaningful error messages to the user.

- **Graceful Termination:** The application can be terminated at any time by inserting `0` as your input.

## Usage

1. **Running the Application:**

   When you start the application, it will check if the SQLite database exists. If not, it will create one along with the necessary table.

2. **Main Menu:**

   The main menu will display the following options:

   - **0. Close Application:** Terminates the application.

   - **1. View All Records:** Displays a list of all your logged water consumption records.

   - **2. Insert Record:** Allows you to log the quantity of water you drink on a specific day.

   - **3. Delete Record:** Allows you to delete a record of your water consumption for a specific day.

   - **4. Update Record:** Lets you update the quantity of water you drank on a specific day.

   - **5. View Report:** Generates a report of your water consumption habits.

3. **Viewing All Records:**

   Selecting option 1 from the menu will display a list of all your logged water consumption records.

4. **Logging Habit Data:**

   To log your water consumption, select option 2 from the menu. You will be prompted to enter the date and the quantity of water you drank on that day. The data will be saved in the database.

5. **Deleting Habit Data:**

   To delete a previously logged record, select option 3 from the menu. You will be prompted to enter the date of the record you want to delete.

6. **Updating Habit Data:**

   To update a previously logged record, select option 4 from the menu. You will be prompted to enter the date of the record you want to update and the new quantity.

7. **Viewing Report:**

   Selecting option 5 from the menu will generate a report of your water consumption habits, providing insights into your daily water intake.

8. **Exiting the Application:**

   To exit the application, simply select option 0 from the menu.

## Database Structure

The application uses a SQLite database with a single table named `drinking_water` with the following structure:

- `Id` (INTEGER): A unique identifier for each habit log entry.
- `Date` (TEXT): The date when the habit was logged (in the format 'YYYY-MM-DD').
- `Quantity` (INTEGER): The quantity of water consumed on that date (e.g., number of glasses).
