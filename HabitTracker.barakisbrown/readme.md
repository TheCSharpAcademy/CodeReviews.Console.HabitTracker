# HABIT TRACKER

Console based CRUD application to track when I entered my blood sugar readings.  
Developed using C#/Sqlite

# GIVEN REQUIREMENTS:
    1. When the application starts, it should create a sqlite database, if one isn’t present.
    2. It should also create a table in the database, where the hours will be logged.
    3. You need to be able to insert, delete, update and view your logged hours.
    4. You should handle all possible errors so that the application never crashes
    5. The application should only be terminated when the user inserts 0.
    6. You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework
    7. Reporting Capabilities

### Features:
    SQLITE DATABASE CONNECTION
        Using a SQlite Connection to process all transactions with the database
        If the DB is never present, the program will create the db and initial table.
    Conosle Based UI:
        Basic UI that I built plus the help from another package called ConsoleTable
        for generating any tables I need to show the user.
    CRUD
        Ability to ADD Readings by Value and Date
        View/Roport Readings to the screen
        Update by Value or Date
        Delete a readings

### Challenges:
    Learn how to use RAW SQLITE TO DO ALL MY TASKS ESPECIALLY since I was so lazy using MAPPERS like EntityFramework
    because this way you know what is happening at all times where sometimes when you EF, if you need to fix then you
    can actually fix it and not completely break your application.

### Lessons Learned:
    I know this app was a little verbose and I probably could have shrunked it and I hope I can improve on that.
    Learn how to use SQLite with raw connection compared to MAPPERS and even LING  
    Learn how to use basic Markdown!

### RESOURCES:
    Stack Overflow
    Microsoft Documentation
    Sqlite.org
