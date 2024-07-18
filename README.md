# HabitTracker
This Console App provides CRUD operations to the Habits Database. The directive used for creating and interacting with SQLite database is <b>System.Data.SQLite</b>.   
- Two classes are created <b>Program and HabitTrackingDatabase</b>. The Program class references the HabitTrackingDatabase class.
- The HabitTrackingDatabase class contains methods which performs the database operations
- The Program class accepts user input and drives the main program.

The basic workflow is as follows:
1. When the program starts it creates a table (habits) if it does not exist, with column names <b>" Id Habit_name Quantity"</b> using IntitalizeDatabase() method
2. If the table is created for the first time then 100 rows are inserted with pre defined habits and their random duration. It is done using SeedHabits() method
3. The user is presented with Main Menu to choose the CRUD operation to be performed and on the basis of user choice the following methods are called InsertHabit(), DeleteHabit(), Updatehabit()
4. A report functionality is provided which provides 3 reports from the <b>"habits table"</b> based on the user's choice.
