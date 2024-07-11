# HabitTracker
C# and Spectre.Console Application that tracks habits and goals.
CRUD operations through ADO.NET to a SQLite database.

## Requirements
- User should be able to create, read, update, and delete habits.

![image](https://github.com/thags/ConsoleTimeLogger/assets/106484883/5109b797-51a7-44ac-b16c-5301920a577e)

- When the application starts it should create a new database file and table if it doesn't exist.
- Should give the user a menu of options.

![image](https://github.com/thags/ConsoleTimeLogger/assets/106484883/689ca115-9160-4265-a96f-8ad7551c2df3)

## Optional Challenges
✅ Let users create their own habit, where they choose the unit of measurement (e.g. pushups, hours of sleep, etc.)

✅ Seed mock data into the database when the program starts.

![image](https://github.com/thags/ConsoleTimeLogger/assets/106484883/a057751a-6061-48b0-a011-7c77054edf27)

✅ Create report functionality to show the user their progress.

![image](https://github.com/thags/ConsoleTimeLogger/assets/106484883/b242a47f-8b5e-40ce-86e5-05dd4322b337)

## Challenges
- Managing dependencies without a DI Container.
- Altering table to add IsMock flag and then deleting existing mocks on start. 