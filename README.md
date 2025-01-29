# Habit Logger

**Habit Logger** is a C# console application designed to help users track their daily water intake by logging the number of glasses of water they drink each day. The application uses SQLite as its database to store and manage the logged data.

---

## Features

- **Log Daily Water Intake**: Users can log the number of glasses of water they drink each day.
- **View Previous Entries**: Users can view all previously logged entries.
- **Edit Entries**: Users can update existing entries to correct mistakes or update information.
- **Delete Entries**: Users can delete unwanted entries from the log.

---

## What Does This Code Do?

This project demonstrates how to:
1. **Create and Manage a SQLite Database**:
   - The application creates a SQLite database file (`DBSQLite.db`) if it doesn't already exist.
   - It establishes a connection to the database and handles potential connection errors.

2. **Create a Table**:
   - The application creates a table named `habitable` with the following columns:
     - `entrynumber` (Primary Key, Auto-incremented)
     - `date` (Text, Format: `YYYY-MM-DD`)
     - `glnumber` (Integer, Number of glasses)

3. **CRUD Operations**:
   - **Create**: Users can insert new records into the table.
   - **Read**: Users can view all records in the table.
   - **Update**: Users can edit existing records.
   - **Delete**: Users can delete records.

4. **User Interaction**:
   - The application provides a menu-driven interface for users to interact with the database.
   - It includes error handling to ensure valid user input (e.g., valid dates, integers).

---

## Main Takeaways for Learners

1. **SQLite in C#**:
   - This project introduces learners to SQLite, a lightweight, file-based database system.
   - It demonstrates how to use the `System.Data.SQLite` library to interact with a SQLite database in C#.

2. **Database Operations**:
   - Learners will understand how to perform basic CRUD operations using SQL commands executed from C#.

3. **Error Handling**:
   - The project includes robust error handling to manage database connection issues, invalid user input, and other exceptions.

4. **User Interface Design**:
   - The application uses a simple console-based menu system to guide users through the available options.

5. **Code Organization**:
   - The code is organized into regions and methods, making it easy to read and maintain.

---
