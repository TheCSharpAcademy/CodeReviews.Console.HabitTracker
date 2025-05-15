using Microsoft.Data.Sqlite;
using Spectre.Console;
using System;
using static System.Formats.Asn1.AsnWriter;
using System.Security.AccessControl;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.PortableExecutable;

namespace HabitTracker;

class Program
{
    static string connection_string = @"Data source=habit-tracker.db";
    static void Main()
    {
        DataInserter.InsertHabits();
        DataInserter.InsertData();
        GetUserInput();
    }

    static void GetUserInput()
    {
        bool close_app = false;

        while (!close_app)
        {
            Console.Clear();

            var selected_menu_option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .AddChoices("Close Application", "View Records", "Insert Record", "Delete Record", "Update Record", "Create New Habit", "View Report"));

            switch (selected_menu_option)
            {
                case "Close Application":
                    close_app = true;
                    break;
                case "View Records":
                    ViewRecords();
                    Console.ReadKey();
                    break;
                case "Insert Record":
                    InsertRecord();
                    break;
                case "Delete Record":
                    DeleteRecord();
                    break;
                case "Update Record":
                    UpdateRecord();
                    break;
                case "Create New Habit":
                    CreateHabit();
                    break;
                case "View Report":
                    ViewReport();
                    break;
            }
        }
    }

    private static void InsertRecord()
    {
        using (var connection = new SqliteConnection(connection_string))
        {
            connection.Open();
            string habit_name = AnsiConsole.Ask<string>("Please insert the name of the habit you want to insert a record into: ");
            string formatted_habit_name = habit_name.ToLower().Trim().Replace(" ", "_");

            var check_cmd = connection.CreateCommand();
            check_cmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name=@habitName";
            check_cmd.Parameters.AddWithValue("@habitName", formatted_habit_name);
            var result = check_cmd.ExecuteScalar();

            if (result == null)
            {
                AnsiConsole.MarkupLine($"Habit called {formatted_habit_name} does not exist.");
                connection.Close();
                Console.ReadKey();
            }
            else
            {
                var column_cmd = connection.CreateCommand();
                column_cmd.CommandText = $"PRAGMA table_info({formatted_habit_name})";
                var reader = column_cmd.ExecuteReader();
                string measurement = null;

                while (reader.Read())
                {
                    string column_name = reader.GetString(1);
                    if (column_name != "Id" && column_name != "Date")
                    {
                        measurement = column_name;
                        break;
                    }
                }
                reader.Close();

                if (measurement == null)
                {
                    AnsiConsole.MarkupLine($"No measurement column found for habit {formatted_habit_name}.");
                    connection.Close();
                    Console.ReadKey();
                    return;
                }

                string date = GetDateInput();
                if (date == null) return;
                int? quantity = GetQuantityInput(FirstLetterToUpper(measurement.Replace("_", " ")));
                if (quantity == null) return;

                var table_cmd = connection.CreateCommand();
                table_cmd.CommandText =
                    $"INSERT INTO {formatted_habit_name}(Date, \"{measurement}\") VALUES('{date}', {quantity})";
                table_cmd.Parameters.AddWithValue("@date", date);
                table_cmd.Parameters.AddWithValue("@quantity", quantity);
                table_cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    private static void ViewRecords(string habit_name = "ALL")
    {
        using (var connection = new SqliteConnection(connection_string))
        {
            connection.Open();
            if(habit_name == "ALL")
            {
                var table_cmd = connection.CreateCommand();
                table_cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
                var reader = table_cmd.ExecuteReader();

                var table_names = new List<string>();
                while (reader.Read())
                {
                    if(reader.GetString(0) != "sqlite_sequence")
                    table_names.Add(FirstLetterToUpper(reader.GetString(0)));
                }
                reader.Close();

                foreach (var table_name in table_names)
                {
                    AnsiConsole.MarkupLine($"[bold][green]{table_name}[/][/]\n");

                    table_cmd.CommandText = $"SELECT * FROM '{table_name}' ORDER BY date(substr(Date, 7, 4) || '-' || substr(Date, 4, 2) || '-' || substr(Date, 1, 2))";
                    reader = table_cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var panelContent = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                panelContent.Add($"[bold][blue]{reader.GetName(i)}:[/][/]{reader.GetValue(i)}");
                            }
                            var panel = new Panel(new Markup(string.Join(" - ", panelContent)))
                            {
                                Border = BoxBorder.Rounded
                            };
                            AnsiConsole.Write(panel);
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("No records found.\n");
                    }
                    reader.Close();
                }
                connection.Close();
            }
            else
            {
                var habit_cmd = connection.CreateCommand();
                habit_cmd.CommandText = $"SELECT * FROM {habit_name} ORDER BY date(substr(Date, 7, 4) || '-' || substr(Date, 4, 2) || '-' || substr(Date, 1, 2))";
                var reader = habit_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var panelContent = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            panelContent.Add($"[bold][blue]{reader.GetName(i)}:[/][/]{reader.GetValue(i)}");
                        }
                        var panel = new Panel(new Markup(string.Join(" - ", panelContent)))
                        {
                            Border = BoxBorder.Rounded
                        };
                        AnsiConsole.Write(panel);
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("No records found.\n");
                }
                reader.Close();
            }
        }
    }

    private static void DeleteRecord()
    {
        AnsiConsole.MarkupLine("Insert '0' to return to main menu:\n");
        ViewRecords();
        int selected_id = AnsiConsole.Ask<int>("Please insert the id of the item you want to delete:");

        using (var connection = new SqliteConnection(connection_string))
        {
            connection.Open();
            var table_cmd = connection.CreateCommand();
            table_cmd.CommandText = $"DELETE FROM drinking_water WHERE Id = @id";
            table_cmd.Parameters.AddWithValue("@id", selected_id);
            int row_count = table_cmd.ExecuteNonQuery();

            if (row_count == 0)
            {
                AnsiConsole.MarkupLine($"Record with Id {selected_id} doesn't exist.");
                DeleteRecord();
            }
            connection.Close();
        }
    }

    private static void UpdateRecord()
    {
        AnsiConsole.MarkupLine("Insert '0' to return to main menu:\n");

        using (var connection = new SqliteConnection(connection_string))
        {
            connection.Open();
            string habit_name = AnsiConsole.Ask<string>("Please insert the name of the habit you want to update a record in: ");
            string formatted_habit_name = habit_name.ToLower().Trim().Replace(" ", "_");

            var check_cmd = connection.CreateCommand();
            check_cmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name=@habitName";
            check_cmd.Parameters.AddWithValue("@habitName", formatted_habit_name);
            var result = check_cmd.ExecuteScalar();

            if (result == null)
            {
                AnsiConsole.MarkupLine($"Habit called {formatted_habit_name} does not exist.");
                connection.Close();
                Console.ReadKey();
                return;
            }

            ViewRecords(formatted_habit_name);
            int selected_id = AnsiConsole.Ask<int>("Please insert the id of the item you want to update:");

            var check_id_cmd = connection.CreateCommand();
            check_id_cmd.CommandText = $"SELECT * FROM {formatted_habit_name} WHERE Id = @id";
            check_id_cmd.Parameters.AddWithValue("@id", selected_id);
            var id_result = check_id_cmd.ExecuteScalar();

            if (id_result == null)
            {
                AnsiConsole.MarkupLine($"Record with ID {selected_id} does not exist.");
                connection.Close();
                Console.ReadKey();
                return;
            }

            var column_cmd = connection.CreateCommand();
            column_cmd.CommandText = $"PRAGMA table_info({formatted_habit_name})";
            var reader = column_cmd.ExecuteReader();
            string measurement = null;

            while (reader.Read())
            {
                string column_name = reader.GetString(1);
                if (column_name != "Id" && column_name != "Date")
                {
                    measurement = column_name;
                    break;
                }
            }
            reader.Close();

            string date = GetDateInput();
            if (date == null) return;
            int? quantity = GetQuantityInput(FirstLetterToUpper(measurement.Replace("_", " ")));
            if (quantity == null) return;

            var table_cmd = connection.CreateCommand();
            table_cmd.CommandText = $"UPDATE {formatted_habit_name} SET Date = @date, '{measurement}' = @quantity WHERE Id = @id";
            table_cmd.Parameters.AddWithValue("@date", date);
            table_cmd.Parameters.AddWithValue("@quantity", quantity);
            table_cmd.Parameters.AddWithValue("@id", selected_id);
            table_cmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    private static void CreateHabit()
    {
        using (var connection = new SqliteConnection(connection_string))
        {
            string formatted_habit_name;
            while (true)
            {
                connection.Open();

                AnsiConsole.MarkupLine("Insert '0' to return to main menu:\n");
                string habit_name = AnsiConsole.Ask<string>("Please insert the name of your new habit: ");
                formatted_habit_name = habit_name.ToLower().Trim().Replace(" ", "_");

                var check_cmd = connection.CreateCommand();
                check_cmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name=@habitName";
                check_cmd.Parameters.AddWithValue("@habitName", formatted_habit_name);
                var result = check_cmd.ExecuteScalar();

                if (result != null)
                {
                    AnsiConsole.MarkupLine($"A habit with the name {habit_name} already exists.");
                    connection.Close();
                }
                else
                {
                    break;
                }
            }

            string measurement = GetMeasurementInput();
            if (measurement == null) return;

            var habit_cmd = connection.CreateCommand();
            habit_cmd.CommandText = $"CREATE TABLE {formatted_habit_name} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, '{measurement}' TEXT)";
            habit_cmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    private static void ViewReport()
    {
        using (var connection = new SqliteConnection(connection_string))
        {
            connection.Open();

            string habit_name = AnsiConsole.Ask<string>("Please insert the name of the habit you want to generate a report on: ");
            string formatted_habit_name = habit_name.ToLower().Trim().Replace(" ", "_");

            var check_cmd = connection.CreateCommand();
            check_cmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name=@habitName";
            check_cmd.Parameters.AddWithValue("@habitName", formatted_habit_name);
            var result = check_cmd.ExecuteScalar();

            if (result == null)
            {
                AnsiConsole.MarkupLine($"Habit called {formatted_habit_name} does not exist.");
                connection.Close();
                Console.ReadKey();
                return;
            }

            int year = AnsiConsole.Ask<int>("Please insert the year you want to check the habit for: ");

            var measurement_cmd = connection.CreateCommand();
            measurement_cmd.CommandText = $"PRAGMA table_info({formatted_habit_name})";
            var measurement_reader = measurement_cmd.ExecuteReader();

            string measurement = null;
            while (measurement_reader.Read())
            {
                string column_name = measurement_reader.GetString(1);
                if (column_name != "Id" && column_name != "Date")
                {
                    measurement = column_name;
                    break;
                }
            }
            measurement_reader.Close();

            var report_cmd = connection.CreateCommand();
            report_cmd.CommandText = $@"
                SELECT COUNT(*), SUM(""{measurement}"") 
                FROM {formatted_habit_name} 
                WHERE strftime('%Y', substr(Date, 7, 4) || '-' || substr(Date, 4, 2) || '-' || substr(Date, 1, 2)) = @year";
            report_cmd.Parameters.AddWithValue("@year", year.ToString());
            var report_reader = report_cmd.ExecuteReader();

            if (report_reader.Read())
            {
                int counter = report_reader.GetInt32(0);
                int sum = report_reader.IsDBNull(1) ? 0 : report_reader.GetInt32(1);

                AnsiConsole.MarkupLine($"You did the habit '{FirstLetterToUpper(habit_name.ToLower().Trim())}' {counter} times in the year {year}, with a total of {sum} {measurement}.");
            }

            Console.ReadKey();

            report_reader.Close();
            connection.Close();
        }
    }

    internal static string GetDateInput()
    {
        AnsiConsole.MarkupLine("Insert '0' to return to main menu:");
        string date_input;
        DateTime temp_date;

        while (true)
        {
            date_input = AnsiConsole.Ask<string>("Please insert date (format: dd-mm-yyyy):");

            if (date_input == "0") return null;

            if (DateTime.TryParseExact(date_input, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out temp_date))
            {
                break;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid date format. Please try again.[/]");
            }
        }
        return date_input;
    }

    internal static string GetMeasurementInput()
    {
        AnsiConsole.MarkupLine("Insert '0' to return to main menu:");
        string measurement_input = AnsiConsole.Ask<string>("Please insert unit of measurement of your habit (e.g. Glasses, Pushups, Kilometers): ");

        if (measurement_input == "0") return null;

        return measurement_input;
    }

    internal static int? GetQuantityInput(string measurement)
    {
        AnsiConsole.MarkupLine("Insert '0' to return to main menu:");
        string quantity_input = AnsiConsole.Ask<string>($"Please insert {measurement} (without decimals):");

        if (quantity_input == "0") return null;

        int final_quantity_input = Convert.ToInt32(quantity_input);
        return final_quantity_input;
    }

    internal static string FirstLetterToUpper(string str)
    {
        if (str == null)
            return null;

        if (str.Length > 1)
            return char.ToUpper(str[0]) + str.Substring(1);

        return str.ToUpper();
    }
}