This CONSOLE Application has 5 Options:
Option 0: Close Application
Option 1: Insert New Habit.
		App lets user to insert any Habit;
			Only alphabetic characters allowed.
			Minimum 8 characters.
		App lets user to choose its own Units of Measurement;
			Minimum 2 alphabetic characters.
		App lets user to insert its own Units.
			Non Negative numeric input only
Option 2: Update Existing Habit.
		App lets user to Delete Habit.
		App lets user to update Units of Habit.
			Non Negative numeric input only
Option 3: View all Habits in database
Option 4: Delete All Records


This is a simple App, I divided it into 4 files:

UserInterface.cs - For Interface and user interaction;
DatabaseHelper.cs - Database Interaction;
helpers.cs - few helper functions(IsNumeric, IsString, IsNotNull)
Program.cs - Main file with creation of database