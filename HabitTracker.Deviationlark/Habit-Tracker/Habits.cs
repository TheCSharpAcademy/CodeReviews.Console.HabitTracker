using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    public class Habits
    {
        public static int count = 0;
        public static string GetInputHabit(List<string> habits)
        {
            // Gets the table names from the database and 
            // gives the user an option to choose which one he would like to edit
            string? inputHabit = "";
            bool habitConvert = false;
            int habit = 0;
            count = 0;
            habits.Clear();
            Console.Clear();

            using (var connection = new SqliteConnection(CRUD.connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%';";

                using (SqliteDataReader reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            count++;
                            // The "name" column contains the table name
                            string tableName = reader.GetString(0);
                            Console.WriteLine($"{count}. {tableName}");
                            habits.Add(tableName);
                        }
                    }
                }
                if (habits.Count > 0)
                {
                    do
                    {
                        Console.WriteLine("Choose which habit you would like to view: ");
                        inputHabit = Console.ReadLine();
                        habitConvert = int.TryParse(inputHabit, out habit);
                    } while(inputHabit == "" || habit < 1 || habit > habits.Count || habitConvert == false);
                }
                else
                {
                    Console.WriteLine("No habits available. Press enter to go to main menu and create a new habit.");
                    Console.ReadLine();
                    Menu.SwitchMenu();
                }
                connection.Close();
            }
            return habits[habit - 1];
        }

        public static void CreateHabit(List<string> habits)
        {
            string?[] habit = new string[2];
            Console.Clear();
            do
            {
                Console.WriteLine("Enter the habit you would like to track (Format: word, word_word): ");
                habit[0] = Console.ReadLine();
            } while (int.TryParse(habit[0], out _) || string.IsNullOrEmpty(habit[0]));

            do
            {
                Console.WriteLine("Enter the unit measurement you would like to track the habit with: ");
                habit[1] = Console.ReadLine();
            } while (int.TryParse(habit[1], out _) || string.IsNullOrEmpty(habit[1]));


            using (var connection = new SqliteConnection(CRUD.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"CREATE TABLE IF NOT EXISTS {habit[0]} (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            {habit[1]} INTEGER
        )";
                count++;
                habits.Add($"{habit[0]}");
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            Menu.GetUserInput();
        }

        public static void DeleteHabit(List<string> habits)
        {
            count = 0;
            string? input = "";
            using (var connection = new SqliteConnection(CRUD.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%';";

                using (SqliteDataReader reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            count++;
                            // The "name" column contains the table name
                            string tableName = reader.GetString(0);
                            habits.Add(tableName);
                        }
                    }
                }
            }
            if (Menu.habits.Capacity > 0)
            {
                using (var connection = new SqliteConnection(CRUD.connectionString))
                {
                    connection.Open();

                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%';";
                    using (SqliteDataReader reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                count++;
                                // The "name" column contains the table name
                                string tableName = reader.GetString(0);
                                Console.WriteLine($"{count}. {tableName}");

                            }
                        }
                    }
                    while (string.IsNullOrEmpty(input) || int.TryParse(input, out _))
                    {
                        Console.WriteLine("TYPE THE EXACT NAME OF THE TABLE YOU WANT TO DELETE");
                        input = Console.ReadLine();
                    }
                    try
                    {
                        tableCmd.CommandText = $"DROP TABLE {input}";
                        tableCmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        DeleteHabit(habits);
                    }
                    
                    connection.Close();
                }
            }
            else
            {
                Console.WriteLine("No habits. Press enter to go back to main menu.");
                Console.ReadLine();
                Menu.SwitchMenu();
            }

            Menu.HabitMenu();
        }
    }

}