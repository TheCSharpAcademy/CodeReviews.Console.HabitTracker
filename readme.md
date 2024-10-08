# Habit Tracker

This app is my first project that connects CMD App with SQL database file.
It helps to track any habits you wish to, choosing your habit name, unit name.

## Requirments

`Basic` Requirments:

- [x] This is an application where you’ll log occurrences of a habit.
- [x] This habit can't be tracked by time, only by quantity.
- [x] Users need to be able to input the date of the occurrence of the habit.
- [x] The application should store and retrieve data from a real database.
- [x] When app starts, it should create sqlite database, if one isnt present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] User should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the DB using ADO.NET. You can’t use mappers.
- [x] Project needs to contain a ReadMe file where you'll explain how app works.

`Advanced` Requirments:

- [x] Try using parameterized queries to make your application more secure
- [x] Let the users create their own habits (with their own unit) to track.
- [x] Seed data when DB gets created 1st time, inserting a 100 records.
- [x] Create a report functionality where the users can view specific information.

> [!NOTE]
> I changed advanced requirments to challenge myself a bit harder. :frog:

Features

- ![UI Menu Screenshot](/assets/UI.png)

- Full SQLite database connection

- Create, read, update, delete records.
- Create, select, view habits.
- Generate random records with user criterias.
- Console Interface (Navigation with numbers)
- User input gets validated

- Validation for

  - Dates.
  - Names for habits[^1].
  - Numbers.
  - Auto counter of records number and total amount.
  - ![UI Menu Screenshot](/assets/AutoCounter.png)
[^1]Every habit is created in new table, so any symbols that might be wrong for
SQL name are checked.

Resources Used

- GitHub
- Stack Overflow
- [MS docs for DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime?view=net-5.0)
- [GitHub Emoji Cheat Sheet](github.com/ikatyang/emoji-cheat-sheet/tree/master)
- ChatGPT (couple of minor issues).
