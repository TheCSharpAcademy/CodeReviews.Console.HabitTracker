# Habit Tracker
A very simple Habit Tracking console app. Exercise by [The C# Academy](https://www.thecsharpacademy.com)

## Requirements
see https://www.thecsharpacademy.com/project/12
* register one habit
  * only by quantity (ex. number of water glasses a date)
* store and retrieve data from a real database
  * ADO.NET + raw SQL + SQLite
  * create new database on startup if not present
  * create a table in the database, where the habit will be logged
* show the user a menu of options
  * insert, delete, update and view logged habit
* handle all possible errors so that the application never crashes
* terminate the application when the user inserts 0
* Read Me file explaining how the app works

## Optional Requirements
* Let the users create their own habits to track
  * let them choose the unit of measurement of each habit
* report functionality
  * i.e. how many times the user ran in a year? how many kms?

## Notes
* https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli
* https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings
* https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/types
* https://www.sqlite.org/autoinc.html
* https://learn.microsoft.com/en-us/dotnet/standard/datetime/how-to-use-dateonly-timeonly
* https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.find?view=net-7.0
* https://learn.microsoft.com/en-us/dotnet/api/system.icloneable?view=net-7.0