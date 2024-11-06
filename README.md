# Habit Logger on Console Application
## Introduction
On the learning path with **The C# Academy** the 3rd challenge was to program the third application, the habit logger.

During the development stage, the main aim was to build bug-free software with various functions. Importantly, the app is built around the KISS and DRY principles. That means that the code was reviewed regularly to detect repeating blocks of code and unnecessary lines. A considerable amount of knowledge and features that were included in the previous 2 challenges- The Math Game and The Calculator Program, were attempted in this program too.

This program uses **C#** for selection menu functionality, calculations and data display. **SQLite** //is added// to deal with database features, such as retrieving the correct data or saving it in the desired format.

## Requirements:

- [x] Logging occurrences of the habits
- [x] The occurrences are only tracked in quantity measurements, although multiple options are available. This means the user can track their habit in a chosen measurement type, such as kilograms, or meters. Those options do not include time units, such as seconds or days.
- [x] User is always asked to insert the time of their record.
- [x] The data can be easily read from or saved to the database without issues.
- [x] If there is no database present upon running the program, 3 will be created automatically. Additionally, it will also create their test tables accordingly, with some records. Thanks to this feature, the user will be able to test the functions of the program, such as logging in their first record, straight away! The name of the test habit table will always start with *test* keyword.
> [!NOTE]
> This feature can be switched off in the *Program* Class:
``` C#
static bool runDeveloperSeedTest = true;
```
> [!NOTE]
> The number of test records can be alternated in the *DataSeed* Class:
``` C#
int numberOfTestTables = 3;
```
- [x] This program provides functionality for creating, reading, updating and deleting both the tables and their records.
- [x] The application was checked from multiple angles for possible crashes or incompatibilities with SQLite and C# syntax.
- [x] The program focuses on applying ADO.NET database. No mappers such as Entity Framework or Dapper were utilised.
- [x] DRY principles were followed, this means that any repeating code blocks were stored in the functions to be used when needed.

## Challenge features:

- [x] The user can create and track their habits. The name, measurement type and measurement name will be set up when the user creates a new habit to track.
- [x] Data seed class automatically creates several test habits with up to 2,000 records included.
- [x] In the habit menu, the user can choose to generate a report of their chosen habit.

## Features:
- Screens functionality on the console:
  - The user can choose a numeric value in a given range to navigate between different menus and choose from program features.
  - The console will always indicate the instructions or the menu location with text highlighted in red.
  - Wrong inputs will be automatically flagged to the user, asking for a correct format of their choice.
- SQLite connection:
    - Ability to read, create, remove and update data. The user can alternate the data tables and their records.
    - The program will automatically create 3 habits if they do not exist yet. They will be also populated with records ranging from 1 to 2,000.
    - SQLite commands are used within the report class, to automatically calculate some of the figures (e.g. max, sum, min) or to read data from the databases efficiently, e.g.
``` C#
tblCmd.CommandText = $"SELECT COUNT(*) FROM (SELECT DISTINCT SUBSTR(Date, LENGTH(Date) - 3, 4) FROM '{habitName}')";
```
or:
``` C#
tblCmd.CommandText = $"SELECT * FROM '{habitName}' WHERE Date LIKE '%{year}'{yearlyCalculator}";
```
- Record class:
    - The user can navigate to the report creation menu. From there they can choose the figures that they want to be included in the report.
  ![1](https://github.com/user-attachments/assets/d123d731-9eb9-4504-ad3e-8b7e5f1ade56)

    - Data is always presented in table format by year vertically and by month horizontally.
  ![2](https://github.com/user-attachments/assets/2aace4e1-94d4-4109-904a-e11e82ff818b)

## Challenges:
- 
