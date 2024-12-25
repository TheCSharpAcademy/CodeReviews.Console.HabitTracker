# General features:
- The application stores and reads data from a real SQLite database.
- When the application starts it creates a SQLite database if one isn’t present.
- It also creates two tables in the database where the habit will be logged and populates another table with default habit names.
- The user can insert, delete, update and view their logged habit.
- The app interacts with the database using ADO.NET.

# Additional features
- Spectre.Console based UI where user can navigate by key presses.
- All SQL queries are parameterized.
- Users can create their own habits to track while creating a new record.
- Users can delete habit names with an option to delete all records with that habit in the database.
- Users can seed data into the database with available habit names and choose number of records to generate.
- Users can delete all records from the database.
- The app has a report functionality where the user can view summarized report for habit and time interval.