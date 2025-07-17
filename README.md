# Habit Tracker
This is a simple C# console application used to track occurrences of habits that are tracked by quantity (e.g. number of water glasses a day). The data is stored locally in a SQLite database and interactions with the database are made using ADO.NET. During the aplication start, it will create the database, if one isn't present. Libraries like Entity Framework or Dapper are intentionally avoided to help you learn the underlying mechanics. Users can insert, delete, update, and view their logged habits.

## Features
* Create a SQLite database on the first run
* Automatically create the required table for habit tracking
* Add, view, update, and delete habit entries
* Input validation for dates, numeric values, and menu options
* Graceful error handling to prevent application crashes
* Follows DRY and KISS principles for maintainable code

## Getting Started
1. Clone the repository
2. Build and run the project
3. Follow the on-screen prompts to log and manage your habits
