using HabitTrackerMain;
using Microsoft.Data.Sqlite;
using StartUp;

string dataBaseID = "habitlogger.db";

string databaseSchema =
    @"
        CREATE TABLE habit (
            id INTEGER NOT NULL PRIMARY KEY  AUTOINCREMENT,
            habitname TEXT NOT NULL,
            quantity INTEGER,
            quantityunit text,
            date TEXT NOT NULL,
            UNIQUE(habitname, date)
        );
    ";

InitializeProgram.StartDatabase(dataBaseID, databaseSchema,true);

UserInterface userinteface = new UserInterface();
userinteface.ShowMainMenu(dataBaseID);
