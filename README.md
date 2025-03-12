# Console Habit Logger
Console based CRUD application to track habits measured in Times/Time.

# Project Requirements
## Mandatoy:
- [x] This is an application where you’ll log occurrences of a habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] Users need to be able to input the date of the occurrence of the habit
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [ ] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- [ ] Follow the DRY Principle, and avoid code repetition.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works.

## Optional:
- [x] If you haven't, try using parameterized queries to make your application more secure.
- [ ] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
- [ ] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

# Resources
- [The C# academy project page](https://thecsharpacademy.com/project/12/habit-logger)
- [SQLite documentation](https://www.sqlite.org/)
- [MS docs for setting up SQLite with C#](https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
- [Spectre Console Docs](https://spectreconsole.net/)

# Screenshots
- Main Menu
![image](https://github.com/user-attachments/assets/6015e79b-b22c-4baa-8538-93cbf03501ee)

- Report page
![image](https://github.com/user-attachments/assets/d1f6d680-5807-4022-bc13-76948a6cf890)
