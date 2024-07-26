# Console Habit Logger

Simple Console CRUD application to log habits. Part of '[The C# Academy](www.thecsharpacademy.com)' console projects.

## Usage

- SQLite database
  - The app uses [ADO.NET](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/) to connect to an [SQLite](https://www.sqlite.org/index.html) database to store Habits.
  - On first run a database along with a default habit "drinking_water", is created if there isn't one present. The database is then automatically seeded with 100 entries.
- Main menu
  - CRUD-operations. Create, Read, Update and Delete habit records.
  - Delete ALL habit records.
  - Change active habit / create new habit. - New habits are automatically seeded with 100 entries upon creation.

## Ref

Project page at The C# Academy: [Habit Logger](https://thecsharpacademy.com/project/12/habit-logger)
