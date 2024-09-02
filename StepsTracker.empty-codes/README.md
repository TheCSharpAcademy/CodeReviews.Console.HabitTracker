# StepsLogger

A simple console application demonstrating how to perform CRUD operations against a real database; Users can log steps walked daily

## Requirements

1. **Habit Tracking**:
   - The application logs occurrences of a habit.
   - Habits are tracked by quantity (e.g., number of water glasses consumed per day) rather than time-based measurements (e.g., hours of sleep).

2. **User Input**:
   - Users must be able to input the date of each habit occurrence.

3. **Database Management**:
   - The application must store and retrieve data from a SQLite database.
   - On startup, the application should check for the presence of the SQLite database. If it does not exist, it should create one.
   - The application must create a table in the database specifically for logging the habit.

4. **CRUD Operations**:
   - Users should be able to:
     - Insert new habit entries.
     - Delete existing habit entries.
     - Update existing habit entries.
     - View logged habit entries.

5. **Error Handling**:
   - The application should handle all potential errors gracefully, ensuring that it never crashes unexpectedly.

6. **Database Interaction**:
   - All interactions with the database must be performed using ADO.NET.
   - Usage of mappers such as Entity Framework or Dapper is not permitted.

7. **Documentation**:
   - The project must include a `README.md` file.
   - The `README.md` file should explain how the application works, including any setup and usage instructions.

## Features

- [List any features here]

## Challenges Faced and Lessons Learned

- **How to Start**: I was overwhelmed by the many requirements at first, but I calmed down and used ChatGPT to break the requirements down and provide a sequential checklist.
- **Understanding Requirements**: The initial requirement stated that the application should log occurrences of a single habit, but I was initially coding with multiple habits in mind. I decided to restart and ensure I fully understood the instructions before proceeding.

## Areas for Improvement

- [List any potential improvements here]

## Resources Used

- [RIP Tutorial on creating simple CRUD using SQLite in C#](https://riptutorial.com/csharp/example/17513/creating-simple-crud-using-sqlite-in-csharp)
- [CodeProject: Using SQLite - Example of CRUD Operations in C#](https://www.codeproject.com/Tips/1057992/Using-SQLite-An-Example-of-CRUD-Operations-in-Csha)
- [YouTube Video on SQLite CRUD operations](https://www.youtube.com/watch?v=h9c7TZb2QuU)

