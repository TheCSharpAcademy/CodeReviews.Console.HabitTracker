using HabitLoggerConsole.Models;
using Microsoft.Data.Sqlite;

namespace HabitLoggerConsole.HabitUpdates;

internal class HabitUpdate
{
    internal static bool RunHabitUpdateMenu(string habit, SqliteConnection connection, char exitChar)
    {
        string habitTrakced = habit;
        bool runUpdateLoop = true;
        while (runUpdateLoop)
        {
            Console.Clear();
            Console.WriteLine($"You are currently updating {HabitCommands.TableNameToDisplayableFormat(habitTrakced).ToLower()} habit.");
            Console.WriteLine("Please choose one of the options listed below. \n");
            Console.WriteLine($"{new string('-', Console.BufferWidth)}");
            Console.WriteLine("\n0 - Chose another habit to update");
            Console.WriteLine("1 - Rename the habit");
            Console.WriteLine("2 - Select another measurement type");
            Console.WriteLine("3 - Rename what you are tracking\n");
            Console.WriteLine($"{new string('-', Console.BufferWidth)}");
            Console.WriteLine();
            Program.InsertExitPrompt(exitChar);

            int selectionInput = 0;
            bool shouldExit = Program.AssignSelectionInput(ref selectionInput, 0, 3, skipSelection: exitChar);
            if (shouldExit)
            {
                connection.Close();
                return true;
            }

            switch (selectionInput)
            {
                case 0:
                    runUpdateLoop = false;
                    break;
                case 1:
                    Console.Clear();
                    habitTrakced = UpdateTableName(habitTrakced, connection, exitChar);
                    break;
                case 2:
                    Console.Clear();
                    UpdateMeasurementType(habitTrakced, connection, exitChar);
                    break;
                case 3:
                    Console.Clear();
                    UpdateColumnName(habitTrakced, connection, exitChar);
                    break;
            }
        }
        return false;
    }

    private static string UpdateTableName(string habit, SqliteConnection connection, char exitChar)
    {
        string? name = null;

        Console.WriteLine($"Please chose a new name for the {HabitCommands.TableNameToDisplayableFormat(habit).ToLower()} habit.\n");
        Program.InsertExitPrompt(exitChar, backMenuAlteration: true);
        while (true)
        {
            bool exitFunction = Program.AssingNameInput(ref name, "Your name must not be empty. Please, try inserting the habit's name again: ", exitChar: exitChar, excludeSymbols: true);
            if (exitFunction)
            {
                return habit;
            }
            if (HabitCommands.IsTableNameDuplicate(name))
            {
                continue;
            }
            break;
        }

        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"ALTER TABLE '{habit}' RENAME TO '{name}'";

        tableCmd.ExecuteNonQuery();

        return name;
    }

    private static void UpdateMeasurementType(string habit, SqliteConnection connection, char exitChar)
    {
        var tblCommand = connection.CreateCommand();

        tblCommand.CommandText = $"SELECT * FROM '{habit}'";

        var reader = tblCommand.ExecuteReader();

        string columnName = reader.GetName(3);

        reader.Close();

        string measurementFullName = MeasurementUnits.MeasurementFullName[(MeasurementType)Enum.Parse(typeof(MeasurementType), columnName)];

        Console.WriteLine($"Currently the measurement type for the {HabitCommands.TableNameToDisplayableFormat(habit).ToLower()} is {measurementFullName}. Please choose a new one from those that are listed below:");
        var listOfMeasurements = MeasurementUnits.DisplayMeasurements();
        Console.WriteLine();
        Console.WriteLine($"{new string('-', Console.BufferWidth)}");
        Console.WriteLine();
        Program.InsertExitPrompt(exitChar, backMenuAlteration: true);

        while (true)
        {
            int userInput = 0;
            bool shouldExit = Program.AssignSelectionInput(ref userInput, 1, listOfMeasurements.Length, exitChar);
            if (shouldExit)
            {
                return;
            }

            MeasurementType newMeasurement = listOfMeasurements[userInput - 1];
            string stringMeasurementType = Enum.GetName(typeof(MeasurementType), newMeasurement);

            if (columnName == stringMeasurementType)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write("This measurement type has already been selected for this habit. Please choose another one: ");
                continue;
            }
            
            tblCommand.CommandText = $"ALTER TABLE '{habit}' RENAME COLUMN '{columnName}' TO '{stringMeasurementType}'";

            tblCommand.ExecuteNonQuery();

            break;
        }
    }

    private static void UpdateColumnName(string habit, SqliteConnection connection, char exitChar)
    {
        var tblCommand = connection.CreateCommand();

        tblCommand.CommandText = $"SELECT * FROM '{habit}'";

        SqliteDataReader reader = tblCommand.ExecuteReader();

        string columnName = reader.GetName(2);

        reader.Close();

        Console.WriteLine($"On your {HabitCommands.TableNameToDisplayableFormat(habit).ToLower()} habit your are currently tracking {columnName}. If you want, you can rename the name of this value.");
        Program.InsertExitPrompt(exitChar);

        while (true)
        {
            string? name = null;
            bool shouldExit = Program.AssingNameInput(ref name, "Your new value tracking name cannot be empty. Please retry: ", exitChar, excludeSymbols: true);
            if (shouldExit)
            {
                return;
            }

            if (columnName == name)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write("Please select a different tracking value name for your habit to that what was chosen before: ");
                continue;
            }

            tblCommand.CommandText = $"ALTER TABLE '{habit}' RENAME COLUMN '{columnName}' TO '{name}'";

            tblCommand.ExecuteNonQuery();

            break;
        }
    }
}

