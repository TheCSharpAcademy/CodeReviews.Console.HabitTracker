# HABITLOGGER CONSOLE APP

### Console based CRUD application to track time spent viewing media content. Developed using C# and SQLite.

#### Given Requirements:
 1. When the application starts, it should create a sqlite database, if one isn’t present.
 2. It should also create a table in the database, where the hours will be logged.
 3. You need to be able to insert, delete, update and view your logged hours.
 4. You should handle all possible errors so that the application never crashes
 5. The application should only be terminated when the user inserts 0.
 6. You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework Reporting Capabilities

    
#### Features
_SQLite database connection_

1. The program uses a SQLite db connection to store and read information.
2. If no database exists, or the correct table does not exist they will be created on program start.
3. A console based UI where users can navigate by key presses

MAIN MENU

    What do you want to do?
    Type 0 to exit app
    Type 1 to view the data
    Type 2 to insert data
    Type 3 to delete data
    Type 4 to update data
    Type 5 to retrieve specific data

From the main menu users can Create, Read, Update or Delete entries. Input format is validated.

    What do you want to do?
    Type 0 to exit app
    Type 1 to view the data from a particular month
    Type 2 to view data in between specific dates
    Type 3 to view data for the last 10 days
    Type 4 to view data sorted by media view time
    Type 5 to view data for when media view time is more than an hour
    Type 6 to return to the main menu 

A menu is offered to retrieve specific data. SQLite 'WHERE' command and c# 'ParseExact' is mainly used to attain this.

#### Learnings and Challenges
1. The usage of SQLite package was learnt.
2. Learnt to implement and view the table in visual studio ny downloading the SQLite and SQL Server Compact Toolbox.
3. Learnt to implement CRUD operations.
4. It was challenging to navigate through specific SQLite commands, especially the ones regarding date and time.
5. Improved my knowledge data validation; found it challenging to keep the app running.

