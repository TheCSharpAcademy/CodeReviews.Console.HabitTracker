Usage
When you run the application, you'll be greeted with a menu that allows you to perform various actions. Here's an overview of the available commands:

Main Menu Options:
0 - Exit: Exit the application.
1 - View All Records: Lists all habit records.
2 - Insert Record: Adds a new habit record by asking for the habit type, date, and quantity.
3 - Delete Record: Deletes a specific habit record by entering its ID.
4 - Update Record: Updates an existing record by entering the record ID.
5 - Clear All Records: Deletes all habit records from the database.
6 - View Records by Date Range: Shows habits between two dates.
7 - View Records for a Specific Habit: Displays all records for a specific habit type.
Example Usage:
Adding a Habit Record:

Select option 2 to insert a record.
You’ll be prompted to enter the habit type (e.g., "exercise").
Then, you’ll enter the date in the format dd-MM-yyyy.
Finally, you’ll provide the quantity for the habit (e.g., number of hours).
Viewing Habit Records:

Choose option 1 to see all habit records.
Option 6 allows you to filter habits by a specific date range.
Option 7 will let you view records for a specific habit type.
Updating a Record:

To update a habit, choose option 4, provide the record ID, and then enter the new date and quantity.
Deleting Records:

Choose option 3 to delete a specific record by its ID.
Select option 5 to clear all records from the database.
Database
This application uses SQLite to store habit records. The database is created automatically in the same directory as the executable with the filename habit-Tracker.db.
