# Habit Logger

An application in which you can register one habit and track by day the number of approaches per day to complete it.

Used libs: ConsoleTables, Dapper instead ADO.NET, Microsoft.Data.Sqlite

# Fulfillment of requirements: 
- [x] This is an application where you’ll register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The app should show the user a menu of options.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] Handle all possible errors so that the application never crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] Can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
- [x] Project needs to contain a Read Me file.

# Fulfillment of challenges:
- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [ ] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
- [ ] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

# Description of how the app works:

- Select default habit or register new habit...
  - ![image](https://github.com/echoblade100/HabitTracker/assets/149004548/d7fe0953-cbe3-4cdc-8c39-3d3a2374df80)


# Resources Used
- Docs for libs:
  - [ConsoleTables](https://github.com/khalidabuhakmeh/ConsoleTables)
  - Dapper
  - Microsoft.Data.Sqlite
- [SQL As Understood By SQLite](https://www.sqlite.org/lang.html)
- Various StackOverflow articles
