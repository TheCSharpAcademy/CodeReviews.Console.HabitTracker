namespace HabitTracker
{
    public class Habit
    {
        enum Frequency
        {
            Daily,
            Weekly,
            Monthly
        }

        private static string GetInput (string logName, string dataType = "string")
        {
            Console.WriteLine($"Enter the {logName}:");
            string input = Console.ReadLine ();

            switch (dataType)
            {
                case "frequency":
                    Frequency frequencyValue;

                    while (!Enum.TryParse(input, ignoreCase: true, out frequencyValue) || !Enum.IsDefined(typeof(Frequency), frequencyValue))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid frequency (daily, weekly, monthly): ");
                        input = Console.ReadLine();
                    }

                    input = frequencyValue.ToString();
                    break;
                case "string":
                    while (input == null)
                    {
                        Console.WriteLine($"Please, make sure you enter a correct {logName}: ");
                        input = Console.ReadLine();
                    }
                    break;
            }

            return input.Trim();
        }

        private static int GetInputInt (string logName, string dataType = "def")
        {
            Console.WriteLine($"Enter the {logName}:");
            string input = Console.ReadLine();
            int inputInt = 0;

            switch (dataType)
            {
                case "def":
                    while (input == null || !int.TryParse(input, out inputInt))
                    {
                        Console.WriteLine($"Please, make sure you enter the {logName}:");
                        input = Console.ReadLine();
                    }
                    break;

            }

            return inputInt;
        }

        private static DateTime GetInputDate(string logName, string dataType = "startDate")
        {
            Console.WriteLine($"Enter the {logName}:");
            string input = Console.ReadLine();
            DateTime startDateValue = new DateTime(2000, 01, 01);


            switch (dataType)
            {
                case "startDate":
                    while (input == null || !DateTime.TryParse(input, out startDateValue))
                    {
                        Console.WriteLine($"Please, make sure you enter the {logName}:");
                        input = Console.ReadLine();
                    }
                    break;

            }

            return startDateValue;
        }

        private static string GetUpdateCommand(string table, string var1, string var2)
        {
            return $@"UPDATE {table} SET {var1} = @{var1} WHERE {var2} = @{var2}";
        }
        private static string GetAllFrom(string table, string var1 = null)
        {
                return $"SELECT * FROM {table}{(var1 != null ? $" WHERE {var1} = @{var1}" : "")}";
        }
        public static void CreateHabit()
        {
            string name = Habit.GetInput("title of your new habit");
            string frequency = Habit.GetInput("frequency to track it by (weekly, daily, monthly)", "frequency");
            int timesInt = Habit.GetInputInt("times per period you would like to perform the habit");
            DateTime startDateValue = Habit.GetInputDate("desired start date of your new habit (format YYYY-MM-DD)", "startDate");

            try
            {
                var connection = Data.DbConnection.GetConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @$"
                    INSERT INTO habits (Name, Frequency, TimesPerPeriod, StartDate)
                    VALUES (@name, @frequency, @timesInt, @startDate)
                ";

                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@frequency", frequency);
                command.Parameters.AddWithValue("@timesInt", timesInt);
                command.Parameters.AddWithValue("@startDate", startDateValue);

                command.ExecuteNonQuery();

                Console.WriteLine("The habit has been added.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public static void UpdateHabit()
        {
            ShowAll("Habits");
            int idInt = Habit.GetInputInt("ID of the habit you would like to edit");

            try
            {
                var connection = Data.DbConnection.GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = Habit.GetAllFrom("habits", "id");

                command.Parameters.AddWithValue("@id", idInt);

                int result = command.ExecuteNonQuery();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"Row with Id {idInt} was found successfully.");
                        int fieldToEditInt = Habit.GetInputInt("number of the field to edit\t1 - Name\t2 - Frequency\t3 - Times Per Period\t4 - Start Date");
                        var updateCommand = connection.CreateCommand();
                        switch (fieldToEditInt)
                        {
                            case 1:
                                string name = Habit.GetInput("a new title of your habit");
                                updateCommand.CommandText = Habit.GetUpdateCommand("habits", "name", "idInt");
                                updateCommand.Parameters.AddWithValue("@name", name);
                                updateCommand.Parameters.AddWithValue("@id", idInt);
                                break;
                            case 2:
                                string frequency = Habit.GetInput("a new frequency to track the habit (weekly, daily, monthly)", "frequency");
                                updateCommand.CommandText = Habit.GetUpdateCommand("habits", "frequency", "id");
                                updateCommand.Parameters.AddWithValue("@frequency", frequency);
                                updateCommand.Parameters.AddWithValue("@id", idInt);
                                break;
                            case 3:
                                int timesInt = Habit.GetInputInt("a new times per period you would like to perform the habit");
                                updateCommand.CommandText = Habit.GetUpdateCommand("habits", "name", "id");
                                updateCommand.Parameters.AddWithValue("@name", timesInt);
                                updateCommand.Parameters.AddWithValue("@id", idInt);
                                break;
                            case 4:
                                DateTime startDateValue = Habit.GetInputDate("a new start date of your new habit (format YYYY-MM-DD)", "startDate");
                                updateCommand.CommandText = Habit.GetUpdateCommand("habits", "startDate", "id");
                                updateCommand.Parameters.AddWithValue("@startDate", startDateValue);
                                updateCommand.Parameters.AddWithValue("@id", idInt);
                                break;
                            default:
                                break;
                        }


                    }
                    else
                    {
                        Console.WriteLine($"No row found with Id {idInt}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void ShowAll(string db)
        {
            try
            {
                var connection = Data.DbConnection.GetConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = Habit.GetAllFrom(db);
                if (db == "Habits") Console.WriteLine("Id\tName\t\tFrequency\tTimes/Period\tStart Date");
                else Console.WriteLine("Id\tName\t\t\tDate");
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (db == "Habits")
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string frequency = reader.GetString(2);
                            int timesPerPeriod = reader.GetInt32(3);
                            string startDate = reader.GetString(4);

                            Console.WriteLine($"{id}\t{name}\t\t{frequency}\t\t{timesPerPeriod}\t\t{startDate}");
                        } else
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string date = reader.GetString(2);

                            Console.WriteLine($"{id}\t{name}\t\t{date}");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static void RemoveHabit()
        {
            ShowAll("Habits");
            int idInt = Habit.GetInputInt("ID of the habit you would like to remove");

            try
            {
                using (var connection = Data.DbConnection.GetConnection()) {
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @$"
                        DELETE FROM habits
                        WHERE id = @id
                     ";

                    command.Parameters.AddWithValue("@id", idInt);

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine($"The habit with Id {idInt} was deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"No row found with Id {idInt}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void AddHabitRecord()
        {
            int idInt = Habit.GetInputInt("ID of the habit you would like to add a record for:");

            try
            {
                using (var connection = Data.DbConnection.GetConnection())
                {
                    string name = "null";
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                        @$"
                            SELECT * FROM habits
                            WHERE id = @id
                        ";

                        command.Parameters.AddWithValue("@id", idInt);

                        var result = command.ExecuteScalar();
                        if (result == null)
                        {
                            Console.WriteLine("The habit with such an id could not be found");
                            return;
                        }
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                name = reader.GetString(1);
                            }
                        }
                    }
                    using (var insertCommand = connection.CreateCommand())
                    {
                        insertCommand.CommandText =
                        @$"
                            INSERT INTO records (Name, HabitDate, HabitId)
                            VALUES (@name, @date, @idInt)
                        ";

                        insertCommand.Parameters.AddWithValue("@name", name);
                        insertCommand.Parameters.AddWithValue("@date", DateTime.Now);
                        insertCommand.Parameters.AddWithValue("@idInt", idInt);

                        insertCommand.ExecuteNonQuery();

                        Console.WriteLine("The record has been added.");
                    }
                }

                  


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
