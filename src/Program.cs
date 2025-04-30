
using Microsoft.Data.Sqlite;
using Spectre.Console;
using Golvi1124.HabitLogger.src.Database;
using Golvi1124.HabitLogger.src.Menus;

public static class Program
{

    public static void Main(string[] args)
    {
        CreateDatabase();
        MainMenu();
    }

    private static void CreateDatabase()
    {
        try
        {
            using (SqliteConnection connection = new(DatabaseConfig.ConnectionString))
            using (SqliteCommand tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS records (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT, 
                    Quantity INTEGER,
                    HabitId INTEGER,
                    FOREIGN KEY (HabitId) REFERENCES habits(Id) ON DELETE CASCADE
                    )";
                /*
                    * HabitId property associate all rows with a row in the Habits table. The association happens due to the FOREIGN KEY constraint.
                Script's structure : FOREIGN KEY(<column that will be linked to the external table>) REFERENCES habits(<column we will link to, in the external table>)
                    * ON DELETE CASCADE means if we delete a record in the habits table, all records that have it as a foreign key will be deleted in the records table. 
                It's a very important script for data integrity.
                 */
                tableCmd.ExecuteNonQuery(); // call the ExecuteNonQuery method when we don't want any data to be returned

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT, 
                    MeasurementUnit TEXT
                    )";
                tableCmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating database: {ex.Message}");
        }
    }

    public static void MainMenu()
    {
        var menuOptions = new MenuOptions();
        var databaseOperations = new DatabaseOperations();
    var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[bold cyan]What do you want to do?[/]")
                    .AddChoices(
                        "Habit Options",
                        "Record Options",
                        "Specific Search",
                        "Wipe All Data",
                        "Add Random Data",
                        "Quit"
                        )
            );

            switch (usersChoice)
            {
                case "Habit Options":
                    menuOptions.HabitMenu();
                    break;
                case "Record Options":
                    menuOptions.RecordMenu();
                    break;
                case "Specific Search":
                    menuOptions.SearchMenu();
                    break;
                case "Wipe All Data":
                    if (AnsiConsole.Confirm("Are you sure you want to delete ALL data?"))
                        databaseOperations.WipeData();
                    break;
                case "Add Random Data":
                    databaseOperations.AddRandomData();
                    break;
                case "Quit":
                    Console.WriteLine("Goodbye!");
                    isMenuRunning = false;
                    break;
            }
        }
    }















}





