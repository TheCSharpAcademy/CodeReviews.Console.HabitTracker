using System.Data.SQLite;

namespace HabitTrackingDatabase
{
    public class HabitDatabase
    {
        public void InitializeDatabse()
        {
            if (File.Exists("habits.db"))
            {
                return;
            }

            using (var connection = new SQLiteConnection("Data Source= habits.db"))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS habits(
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       habit_name TEXT NOT NULL,
                       quantity INTEGER NOT NULL,
                       date TEXT NOT NULL
                    );";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                SeedHabits();
            }
        }

        void SeedHabits()
        {
            int count = 0;
            List<string> habitList = new List<string> { "Walking", "Cycling", "Meditating", "Drinkling water", "Reading", "Playing Video Games"};
            Random random = new Random();
            using (var connection = new SQLiteConnection("Data Source= habits.db"))
            {
                connection.Open();
                while(count < 100)
                {
                    int index = random.Next(habitList.Count);
                    string habitName = habitList[index];
                    int quantity = random.Next(1, 6);
                    string insertQuery = "INSERT INTO habits (habit_name, quantity, date) VALUES (@habit_name, @quantity, @date)";
                    using(var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@habit_name", habitName);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.ExecuteNonQuery();
                    }

                    count++;
                }
            }
        }

        public void ViewHabits()
        {
            Console.WriteLine("YOUR HABITS");
            Console.WriteLine("-----------------------------------------------------------------------------------------------");
            using (var connection = new SQLiteConnection("Data Source= habits.db"))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM habits";
                using(var command = new SQLiteCommand(selectQuery, connection))
                {
                    using(var reader = command.ExecuteReader())
                    { 
                        Console.WriteLine("ID\tHabit\t\t\tQuantity\t\t\tDate");
                        Console.WriteLine("-----------------------------------------------------------------------------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["id"]}\t{reader["habit_name"]}\t\t\t{reader["quantity"]}\t\t\t{reader["date"]}");
                        }
                    } 

                }
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------\n");
        }

        public void InsertHabits()
        {
            Console.Write("Enter habit name: ");
            string? habitName = Console.ReadLine();
            int habitQuantity;
            while (true)
            {
                Console.Write("Enter quantity: ");
                if (int.TryParse(Console.ReadLine(), out habitQuantity))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter valid integer");
                }
            }

            using(var connection = new SQLiteConnection("Data Source= habits.db"))
            {
                connection.Open();

                string insertQuery = "INSERT INTO habits (habit_name, quantity, date) VALUES(@habit_name, @quantity, @date)";
                
                using(var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@habit_name", habitName);
                    command.Parameters.AddWithValue("@quantity", habitQuantity);
                    command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Habit inserted succesfully.\n\n");
        }

        public void DeleteHabits()
        {
           
            int id = 0;
            while (true)
            {
                Console.Write("Enter habit id that is to be deleted: ");
                if (int.TryParse(Console.ReadLine(), out id))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter valid integer");
                }
            }

            using (var connection = new SQLiteConnection("Data Source= habits.db"))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM habits WHERE id = @id";
                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("\nHabit deleted successfully.\n\n");
        }


        public void UpdateHabits()
        {
            int id = 0;
            while (true)
            {
                Console.Write("Enter habit id to be updated: ");
                if(int.TryParse(Console.ReadLine(), out id))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter valid integer");
                }
            }

            int newQuantity;
            while (true)
            {
                Console.Write("Enter new quantity: ");
                if (int.TryParse(Console.ReadLine(), out newQuantity))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter valid integer");
                }
            }

            using (var connection = new SQLiteConnection("Data Source = habits.db"))
            {
                connection.Open();
                string updateQuery = "UPDATE habits SET quantity = @quantity WHERE id = @id";
                using(var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@quantity", newQuantity);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("\nHabit updated succesfully.\n\n");
        }

        public void ReportHabit()
        {
            Console.WriteLine("\nChoose the option for which report is to be generated: ");
            Console.WriteLine("1. View a specific habit");
            Console.WriteLine("2. How much time/quantity you spend on a specific habit?");
            Console.WriteLine("3. Habits with quantities greater than a specific value?");
            Console.WriteLine("4. Which habit you spend maximum time on and how much?");
            Console.WriteLine("5. Which day you were most active?");
            Console.Write("\nEnter choice: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    int id;
                    while (true)
                    {
                        Console.Write("Enter id: ");
                        if (int.TryParse(Console.ReadLine(), out id))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Enter valid integer");
                        }
                    }
                    ViewSpecificRow(id);
                    break;

                case "2":
                    Console.Write("Enter habit: ");
                    string? habit_name = Console.ReadLine();
                    TimeSpentOnHabitQuery(habit_name);
                    break;

                case "3":
                    int quantity;
                    while (true)
                    {
                        Console.Write("Enter quantity: ");
                        if (int.TryParse(Console.ReadLine(), out quantity))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Enter valid integer");
                        }
                    }
                    GreaterThanValueQuery(quantity);
                    break;

                case "4":
                    MaximumTimeQuery();
                    break;

                case "5":
                    MostActiveDay();
                    break;

                default:
                    Console.WriteLine("Invalid choice\n");
                    break;
            }
        }

        void MaximumTimeQuery()
        {
            try
            {

                using (var connection = new SQLiteConnection("Data Source= habits.db"))
                {
                    connection.Open();
                    string query = @"
                    SELECT habit_name, total_quantity
                    FROM (SELECT habit_name,  SUM(quantity) AS total_quantity
                            FROM habits
                            GROUP BY habit_name
                    )as quantity
                    ORDER BY total_quantity DESC
                    LIMIT 1;";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            Console.WriteLine("\n-----------------------");
                            Console.WriteLine("Habit\tQuantity");
                            Console.WriteLine("-----------------------");
                            while(reader.Read())
                                Console.WriteLine($"{reader[0]}\t{reader[1]}");
                             
                        }
                        Console.WriteLine("-----------------------\n");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Opps error occured: " + ex.Message + "\n");
                return;
            }
        }
           
        void TimeSpentOnHabitQuery(string habit_name)
        {
            using (var connection = new SQLiteConnection("Data Source= habits.db"))
            {
                connection.Open();
                string query = $"SELECT SUM(quantity) FROM habits WHERE habit_name = '{habit_name}'";
                using(var command = new SQLiteCommand(query, connection))
                {
                    using(var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("\n-----------------------");
                        Console.WriteLine("Time/Quantity");
                        Console.WriteLine("-----------------------");
                        while(reader.Read())
                            Console.WriteLine($"{reader[0]}");
                    }
                }
                Console.WriteLine("-----------------------\n");
            }
        }

        void GreaterThanValueQuery(int quantity)
        {
            try
            {
                using (var connection = new SQLiteConnection("Data Source= habits.db"))
                {
                    connection.Open();
                    string query = @$"
                    SELECT habit_name, total_quantity
                    FROM (SELECT habit_name, SUM(quantity) AS total_quantity
                          FROM habits
                          GROUP BY habit_name
                           )
                    WHERE total_quantity > {quantity}
                    GROUP BY  habit_name;";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            Console.WriteLine("\n-----------------------");
                            Console.WriteLine("Habit\tQuantity");
                            Console.WriteLine("-----------------------");
                            while (reader.Read())
                                Console.WriteLine($"{reader[0]}\t{reader[1]}");
                        }
                    }
                    Console.WriteLine("-----------------------\n");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Opps error occured: " + ex.Message + "\n");
            }
        }

        void ViewSpecificRow(int id)
        {
            using (var connection = new SQLiteConnection("Data Source= habits.db"))
            {
                connection.Open();
                string viewQuery = "SELECT * FROM habits WHERE id = @id";
                using (var command = new SQLiteCommand(viewQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using(var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("-----------------------------------------------------------------------------------------------");
                        Console.WriteLine("ID\tHabit\t\t\tQuantity\t\t\tDate");
                        Console.WriteLine("-----------------------------------------------------------------------------------------------");

                        if(reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader[0]}\t{reader[1]}\t\t\t{reader[2]}\t\t\t{reader[3]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Habits Found");
                        }
                        Console.WriteLine("-----------------------------------------------------------------------------------------------\n");
                    }
                }
            }

        }
        void MostActiveDay()
        {
            using(var connnection = new SQLiteConnection("Data Source = habits.db"))
            {
                connnection.Open();
                string query = @"
                    SELECT SUBSTR(date, 1, 10), COUNT(date) AS frequency
                    FROM habits 
                    GROUP BY SUBSTR(date, 1, 10)
                    ORDER BY frequency DESC
                    LIMIT 1;
                    ";
                using(var command = new SQLiteCommand(query, connnection))
                {
                    using(var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("-------------------------------------------");
                        Console.WriteLine("Date\t\t# times logged in");
                        Console.WriteLine("-------------------------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]}\t{reader[1]}");

                        }
                        Console.WriteLine("-------------------------------------------\n");
                    }
                }
            }
        }
    }

}