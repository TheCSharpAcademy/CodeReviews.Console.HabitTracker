using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    public class DatabaseHandler
    {
        readonly private string dbSource;

        public DatabaseHandler()
        {
            this.dbSource = @"Data Source=habit_tracker.db";
            this.HabitTable();
        }

        void HabitTable()
        {
            using var connection = new SqliteConnection(this.dbSource);
            using var command = connection.CreateCommand();

            connection.Open();
            command.CommandText =
                $@"CREATE TABLE IF NOT EXISTS habits(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                habit_name TEXT,
                date TEXT,
                quantity INTEGER
                )";

            command.ExecuteNonQuery();
        }

        public void Insert(string name, string dateToday, int quantity = 0)
        {
            if (CheckIfNameExists(name))
            {
                throw new IOException("Habit already exists! Try again:");
            }

            using var connection = new SqliteConnection(this.dbSource);
            using var command = connection.CreateCommand();

            connection.Open();
            command.CommandText =
                $@"INSERT INTO habits(habit_name, date, quantity)
                  VALUES(@name, @date, @quantity)";

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@date", dateToday);
            command.Parameters.AddWithValue("@quantity", quantity);

            command.ExecuteNonQuery();
        }


        bool CheckIfNameExists(string name)
        {
            using var connection = new SqliteConnection(this.dbSource);
            using var command = connection.CreateCommand();
            connection.Open();

            command.CommandText =
                "SELECT * FROM habits WHERE habit_name=@name";

            command.Parameters.AddWithValue("@name", name);

            using var reader = command.ExecuteReader();
            return reader.Read();
        }

        public void View()
        {
            using var connection = new SqliteConnection(this.dbSource);
            using var command = connection.CreateCommand();
            connection.Open();

            command.CommandText =
                "SELECT * from habits";

            using var reader = command.ExecuteReader();

            Console.WriteLine("ID\tHabit\t\t\tDate\t\tQuantity");

            while (reader.Read())
            {
                int Id = reader.GetInt32(0);
                string HabitName = FormatName(reader.GetString(1));
                string Date = reader.GetString(2);
                int Quantity = reader.GetInt32(3);

                string output = $"{Id}\t{HabitName}\t\t{Date}\t{Quantity}";
                Console.WriteLine(output);
            }
        }

        static string FormatName(string habitName)
        {
            habitName = char.ToUpper(habitName[0]) + habitName[1..];
            habitName = habitName.Replace("_", " ");

            int lengthDiff = 14 - habitName.Length;
            if (lengthDiff != 0)
            {
                habitName += string.Concat(Enumerable.Repeat(" ", lengthDiff));
            }

            return habitName;
        }

        public void Update(int id)
        {
            using var connection = new SqliteConnection(this.dbSource);
            using var command = connection.CreateCommand();
            connection.Open();

            command.CommandText =
                @"UPDATE habits 
                  SET quantity = quantity + 1 
                  WHERE id=@id";

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new SqliteConnection(this.dbSource);
            using var command = connection.CreateCommand();
            connection.Open();

            command.CommandText =
                @"DELETE FROM habits
                  WHERE id=@id";

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }

    }
}