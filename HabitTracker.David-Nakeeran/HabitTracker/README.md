# Habit Logger

Console based CRUD application to track number of drinks consumed in a day.
Developed using C# and SqLite.

## Requirements

1. **Habit Tracking**:

   - Log occurrences of a habit.
   - Habits are tracked by Quantity.

2. **User Input**:

   - Users need to be able to input the date of the occurrence of the habit.

3. **Database Management**:

   - Application should store and retrieve data from a real database.
   - Upon application start, it should create a sqlite database, if one isn’t present.
   - Application will create a table in the database, where the habit will be logged.

4. **CRUD**:

   - Users should be able to insert, delete, update and view their logged habit.

5. **Error Handling**:

   - Application should handle all possible errors so that the application never crashes.

6. **Database**:

   - Can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.

7. **Documentation**:
   - Project needs to contain a Read Me file where you'll explain how your app works.

## Features

- View all records: List all stored entries, including dates and quantities.
- Add a new record: Insert a new habit entry with the date and quantity.
- Update existing record: Update a specific record by its ID.
- Delete a record: Remove a habit entry by its ID.
