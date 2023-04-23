using HabitTracker.barakisbrown;
using System.Collections.Generic;
using System.Globalization;

/*
 * Check to see if DB Exist. 
 * Exist => True then Display the Menu
 * Exist => False -> Create the DB and Initial Table
 */
Helpers help = new();
DataLayer data = new();
/*
Habit habit = new();
habit.Amount = help.GetAmount();
habit.Date = help.GetDate();
data.Insert(habit);
*/


List<Habit> habits = data.SelectAll();
foreach (var hab in habits)
{
    Console.WriteLine(hab);
}
