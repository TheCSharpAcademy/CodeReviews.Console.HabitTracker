# Habit Tracker
A very simple Habit Tracking console app. Exercise by [The C# Academy](https://www.thecsharpacademy.com)

## Usage
### Execute the App
```
HabitTracker.pcjb$ dotnet run
```
If the app executes for the first time, it creates an SQLite database file 'HabitTracker.db' in the same directory where the app was started.
After the app has started, you will see the main menu:

### Main Menu
```
1 - Add new habit
2 - Select habit
3 - Log habit
4 - View habit log
5 - Report: Frequency and total per month
0 - Exit
Enter one of the numbers above to select a menu option.
```

### Add a New Habit like e.g. 'Drink Water' 
Press (1) on the main menu screen to add a new habit and enter the name and unit of measure of the habit:
```
New habit
Enter '0' as name or uom to return to main menu without adding a new log entry.
Name: Drink Water
Unit of measure: Glasses
```
### Select the Active Habit
Press (2) on the main menu screen to select the habit you want to add or edit log entries or generate reports for:
```
Habits
   ID                 Name        UOM
    1          Drink Water    Glasses
    2              Walking         km
Enter ID and press enter to select a habit.
1
```

### Add Log Entries to Track Your Progress
Press (3) on the main menu screen to record a log entry for the active habit:
```
New log entry for habit 'Drink Water'
Enter '0' as date or quantity to return to main menu without adding a new log entry.
Date: 2023-09-10
Quantity [Glasses]: 5
```

### View Log Entries for the Active Habit
Press (4) on the main menu screen to view all log entries you have entered for the active habit:
```
Log entries for habit 'Drink Water'
   ID       Date    Glasses
    1  9/10/2023          5
    2   9/9/2023          3
    3   9/8/2023          6
    4   9/2/2023          8
   14  8/15/2023          7
   15  8/18/2023          4
   16  8/27/2023          1
   17   8/9/2023          3
   18  8/19/2023          7
   19   7/1/2023          4
   20   7/2/2023          5
Enter ID and press enter to edit/delete a log entry or press enter alone to return to main menu.
```

### Edit or Delete a Log Entry
On the screen with the list of all log entries you can select an entry to edit the date and/or quantity if you made an error or to completely delete the entry:
```
Log entries for habit 'Drink Water'
   ID       Date    Glasses
    1  9/10/2023          5
    2   9/9/2023          3
    3   9/8/2023          6
    4   9/2/2023          8
   14  8/15/2023          7
   15  8/18/2023          4
   16  8/27/2023          1
   17   8/9/2023          3
   18  8/19/2023          7
   19   7/1/2023          4
   20   7/2/2023          5
Enter ID and press enter to edit/delete a log entry or press enter alone to return to main menu.
3
```

On the following screen you see the details of the selected log entry and can select if you would like to edit or delete the entry:
```
Log entry for habit 'Drink Water':
ID      : 3
Date    : 9/8/2023
Quantity: 6 Glasses
Enter 'e' to edit or 'd' to delete this log entry and press enter or press enter alone to cancel.
e
```

If you select 'e' for edit, you can modify the date and the quantity on the next screen:
```
Edit log entry for habit 'Drink Water':
ID      : 3
Date    : 9/8/2023
Quantity: 6 Glasses
New Date (leave empty to keep old date): 
New quantity [Glasses] (leave empty to keep old quantity): 7
```

Afterwards you are back on the screen with the list of all log entries where you see the modified quantity:

```
Log entries for habit 'Drink Water'
   ID       Date    Glasses
    1  9/10/2023          5
    2   9/9/2023          3
    3   9/8/2023          7
    4   9/2/2023          8
   14  8/15/2023          7
   15  8/18/2023          4
   16  8/27/2023          1
   17   8/9/2023          3
   18  8/19/2023          7
   19   7/1/2023          4
   20   7/2/2023          5
Enter ID and press enter to edit/delete a log entry or press enter alone to return to main menu.
```

## Generate a Report
Press (5) on the main menu screen to generate a report calculating the monthly frequency and total for the active habit:
```
Report 'Frequency and total per month'
Habit 'Drink Water' measured in 'Glasses'
 Year Month  Frequency      Total
 2023     7          2          9
 2023     8          5         22
 2023     9          4         23
Press enter to proceed.
```

## Areas for Improvement
* Sort the list of log entries by date and not by id. Currently it is sorted by id to make it easy to see the last entered log entry at the end of the list.
* The screen showing the list of log entries as this will become too long very soon. Add pagination and a filter on user defined date range.
* Do not require the user to select the active habit if there is only one habit in the database yet.
* Add multiple log entries in a row without having to repeatedly select '3 - Log habit' on the main menu screen inbetween.
* Currently it is possible to log multiple entries for the same date. The user could inadvertently log duplicate entries without noticing. To mitigate this error, the app could ask if the new entry (a) should replace the existing entry for that date or (b) if the quantity of the new entry should be added to the existing entry or (c) if the new entry simply is a duplicate and should be ignored.

## Notes / References
* https://www.thecsharpacademy.com/project/12
* https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli
* https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings
* https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/types
* https://www.sqlite.org/autoinc.html
* https://learn.microsoft.com/en-us/dotnet/standard/datetime/how-to-use-dateonly-timeonly
* https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.find?view=net-7.0
* https://learn.microsoft.com/en-us/dotnet/api/system.icloneable?view=net-7.0