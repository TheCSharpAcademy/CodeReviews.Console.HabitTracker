# Habit Logger on Console Application
## Introduction
On the learning path with **The C# Academy** the next challenge was to program the third application, the habit logger.

During the development stage, the main aim was to build bug-free software with a variety of functions. Importantly, the app is built around the KISS and DRY principles. That means that the code was reviewed regularly to detect repeating blocks of code and unnecessary lines. A considerable amount of knowledge and features that were included in the previous 2 challenges- The Math Game and The Calculator Program, were attempted in this program too.

This program uses **C#** for menu navigation, calculations and data display. **SQLite** was necessary to deal with database features, such as retrieving the correct data or saving it in the desired format.

## Requirements:

- [x] Logging occurrences of the habits
- [x] The occurrences are only tracked in quantity measurements, although multiple options are available. This means the user can track their habit in a chosen measurement type, such as kilograms, or meters. Those options do not include time units, such as seconds or days.
- [x] User is always asked to insert the time of their record.
- [x] The data can be easily read from or saved to the database without issues.
- [x] If there is no database present upon running the program, 3 will be created automatically. Additionally, it will also create their test tables accordingly, with some records already in. Thanks to this feature, the user will be able to test the functions of the program, such as logging in their first record, straight away! The name of the test habit table will always start with *test* keyword.
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

- [x] The user can create and track their habits. The name, measurement type and measurement name will need to be set up manually by the user when they create a new habit to track.
- [x] Data seed class automatically creates several test habits with up to 2,000 records included.
- [x] In the habit menu, the user can choose to generate a report of their chosen habit.

## Features:
- Screens functionality on the console:
  - The user can choose a numeric value in a given range to navigate between different menus and choose from program features.
  - The console will always indicate the instructions or the current menu name with text highlighted in red.
  - Wrong inputs will be automatically flagged to the user, asking for a correct format of their choice.
- SQLite connection:
    - Ability to read, create, remove and update data. The user can alternate *the data tables* and *their records*.
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

    - Data is always presented in table format and sorted vertically by year and horizontally by month.
  ![2](https://github.com/user-attachments/assets/2aace4e1-94d4-4109-904a-e11e82ff818b)

## Challenges:
- This was the first time I had to use a database functionality in the actual software. Initially, there was a lot of confusion with how the connection opening works. This feature had to be used in several places in the program, so with time I got a grip of it.
- Same as with *REGEX*, I had to dive into the SQLite command language to build some additional features that were not explained in the tutorial video. Those include reading data with multiple conditions, or performing simple calculations while creating a query, e.g. sum or max of the records.
- I deemed that it would be a beneficial idea to repeat some of the features that I built in the previous two projects, so I can remember them better through the repetition, e.g. *REGEX* search or *Tuple* variable application.
- While I was adding more and more elements to the program, I realised soon enough that navigation between the screens could cause some issues. The number of different screens, or menus, was getting bigger and bigger, to the point that it was challenging to know immediately what exact function the user was using at the given time. I amended this slightly by adding a red background colour to the principal description or purpose of the current menu.
- Report creation functionality- this was the most consuming part of the software, especially the table creation part and populating its data. I am aware that there are add-ons that can deal with this much more rapidly while giving the user more options to alter the way their data is presented. Nevertheless, as I was reading the project requirements, I realised that using third-party additions would be considered cheating. I consider this a very good exercise to catch the repeating code and nest it in the for loop to make the classes more readable.

## Lessons learnt:
- Basic SQLite data reading, saving, updating and removing functionality.
- The structure of SQLite Text Command language and its components. This was expanded by adding features that search data with multiple specialised conditions and perform basic calculations.
- Some more console commands were learnt that include text and background colour optimization and size of the buffer and console change.
- Basic knowledge of date data validation, that was worked around in this particular project.

## Things to improve:
- Menu readability suffered significantly in this project. I need to think of a better way to deal with switching menus and their console interface. The user should always be able to know or read what functionality they are using and how to navigate to the next desired feature.
- User Input code started to get a bit messy, especially judging from the data validation angle. Further experimentation is needed to improve the habits I continued from my second software.
- I could possibly break down some of the classes into even smaller ones, just to improve the readability of the code. For example, the report class could have its *table writing function* in a separate class, as this is quite a long algorithm that works independently from the data calculator function.
- Consistency. Yes, I am lacking this a lot. And as much as I love programming, I sometimes keep my breaks for too long. Maybe I should program less on my programming days, but then plan them more often?

### *References:*
- Google search that directed me to several useful [Stack Overflow](https://stackoverflow.com/) and [Reddit](https://www.reddit.com/) conversations that talked over extensively.
- [Video tutorial](https://www.youtube.com/watch?v=d1JIJdDVFjs&t=1s) of this project
