using Microsoft.Data.Sqlite;

namespace HabitTracker.Business
{
    public class HabitService
    {
        List<Habit> habits;
        public HabitReader habitReader = new HabitReader();


        string connectionString = @"Data Source=habit-Tracker.db";

        public void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habit_logger (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date Text,
                    Name Text,
                    Quantity INTEGER,
                    Unit Text
                    )";

                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText = $"SELECT * FROM habit_logger";

                habits = new List<Habit>();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Habit temp = new Habit();
                    for (int i = 1; i <= 100; i++)
                    {
                        temp.Date = habitReader.RandomizeDate();
                        habitReader.RandomizeHabit(ref temp);

                        AddEntry(temp);
                    }
                }

                connection.Close();
            }
        }
        public void ShowDatabase()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();

                tablecmd.CommandText = $"SELECT * FROM habit_logger";

                habits = new List<Habit>();

                SqliteDataReader reader = tablecmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        habits.Add(
                            new Habit
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.Parse(reader.GetString(1)),
                                Name = reader.GetString(2),
                                Quantity = reader.GetInt32(3),
                                Unit = reader.GetString(4),
                            });
                    }
                    foreach (Habit habit in habits)
                    {
                        Console.WriteLine($"ID {habit.Id} - {habit.Name}, {habit.Quantity} {habit.Unit} in {habit.Date.ToString("dd-MMM-yyyy")}");
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();
            }
            Console.WriteLine("-----------------------------");
        }
        public void AddEntry(Habit newHabit)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();

                tablecmd.CommandText = $"INSERT INTO habit_logger(Date, Name, Quantity, Unit) VALUES('{newHabit.Date}', '{newHabit.Name}', '{newHabit.Quantity}', '{newHabit.Unit}')";
                tablecmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        public void DeleteEntry()
        {
            Console.Clear();
            ShowDatabase();

            Console.WriteLine("\nIngress the ID of the entry you want to delete or ingress 0 to go back to the main menu");
            int recordId = Convert.ToInt32(Console.ReadLine());

            if (recordId == 0)
            {
                return;
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();

                tablecmd.CommandText = $"DELETE from habit_logger WHERE Id={recordId}";
                int rowCount = tablecmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"Record with ID {recordId} doesn't exist.");
                }
                else
                {
                    Console.WriteLine($"Record with ID {recordId} deleted succesfully.");
                }

                connection.Close();
            }


        }
        public void UpdateEntry()
        {
            Console.Clear();
            ShowDatabase();

            Console.WriteLine("\nIngress the ID of the entry you want to update");
            int recordId = Convert.ToInt32(Console.ReadLine());

            DateTime date = habitReader.IngressDate();
            string name = habitReader.IngressName();
            int quantity = habitReader.IngressQuantity();
            string unit = habitReader.IngressUnit();


            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM habit_logger Where Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"Record with ID {recordId} doesn't exist.");
                    connection.Close();
                }
                else
                {
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText = $"UPDATE habit_logger SET date = '{date}', name = '{name}', quantity = '{quantity}', unit = '{unit}' WHERE Id = {recordId}";
                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                }

            }
        }



        public void ShowReports()
        {

            string column = "Name";

            Console.Clear();
            Console.WriteLine("Select the habit you want to display:");
            Console.WriteLine("1 - Reading");
            Console.WriteLine("2 - Walking");
            Console.WriteLine("3 - Drinking water");
            Console.WriteLine("4 - Yoga");
            Console.WriteLine("5 - Eat a Fruit");
            Console.WriteLine("6 - Other\n");

            int number = Convert.ToInt32(Console.ReadLine());
            string name;
            switch (number)
            {
                case 1:
                    name = "Reading";
                    ShowReports(column, name);
                    break;
                case 2:
                    name = "Walking";
                    ShowReports(column, name);
                    break;
                case 3:
                    name = "Drinking water";
                    ShowReports(column, name);
                    break;
                case 4:
                    name = "Yoga";
                    ShowReports(column, name);
                    break;
                case 5:
                    name = "Eat a Fruit";
                    ShowReports(column, name);
                    break;
                case 6:
                    Console.WriteLine("Ingress the name of the habit you want to show, first character must be uppercase");
                    name = Console.ReadLine().Trim();
                    ShowReports(column, name);
                    break;
            }

        }

        public void ShowReports(string column, string name = "")
        {
            Console.Clear();


            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();

                tablecmd.CommandText = $"SELECT * FROM habit_logger WHERE {column} = '{name}'";

                habits = new List<Habit>();

                SqliteDataReader reader = tablecmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        habits.Add(
                            new Habit
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.Parse(reader.GetString(1)),
                                Name = reader.GetString(2),
                                Quantity = reader.GetInt32(3),
                                Unit = reader.GetString(4),
                            });
                    }
                    foreach (Habit habit in habits)
                    {
                        Console.WriteLine($"ID {habit.Id} - {habit.Name}, {habit.Quantity} {habit.Unit} in {habit.Date.ToString("dd-MMM-yyyy")}");
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                    Console.ReadLine();
                }
                Console.WriteLine("-----------------------------");

                connection.Close();
            }

            Console.ReadLine();
        }

    }
}
