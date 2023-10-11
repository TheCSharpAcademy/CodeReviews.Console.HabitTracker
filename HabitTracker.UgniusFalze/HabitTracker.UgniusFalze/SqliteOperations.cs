using Microsoft.Data.Sqlite;

namespace HabitTracker.UgniusFalze
{
    internal class SqliteOperations
    {
        private SqliteConnection connection;
        public SqliteOperations(string dbFile = "habbitLog.db")
        {
            connection = new SqliteConnection($"Data Source={dbFile}");
            connection.Open();
            CreateHabbitTable();
        }

        public void CheckIfRecordWithIdExsists(long id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT 1 
                FROM CupsOfCoffee 
                WHERE Id = $id";
            command.Parameters.AddWithValue("id", id);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read() == false)
                {
                    throw new ArgumentException();
                }
            }
        }

        private void CreateHabbitTable()
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS CupsOfCoffee(
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Quantity INTEGER NOT NULL,
                    Date TEXT NOT NULL UNIQUE
                  );
            ";
            command.ExecuteNonQuery();
        }

        public void InsertHabbit(long quantity, DateOnly date)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO CupsOfCoffee(quantity, Date) VALUES($quantity, $date);
            ";

            command.Parameters.AddWithValue("$quantity", quantity);
            command.Parameters.AddWithValue("$date", date.ToString(GetDateFormat()));
            command.ExecuteNonQuery();
        }

        public void UpdateQuantity(long id, long quantity)
        {
            CheckIfRecordWithIdExsists(id);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE CupsOfCoffee
                SET quantity = $quantity
                WHERE Id = $id
                ;
            ";
            command.Parameters.AddWithValue("$quantity", quantity);
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }

        public void UpdateDate(long id, DateOnly date)
        {
            CheckIfRecordWithIdExsists(id);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE CupsOfCoffee
                SET date = $date
                WHERE Id = $id
                ;
            ";
            command.Parameters.AddWithValue("$date", date.ToString(GetDateFormat()));
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }
        public void DeleteHabbit(long id)
        {
            CheckIfRecordWithIdExsists(id);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                DELETE FROM CupsOfCoffee
                WHERE Id = $id
                ;
            ";
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }

        public List<Habbit> GetHabbitHistory()
        {
            List<Habbit> habbits = new List<Habbit>();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT Id, Quantity, Date
                FROM CupsOfCoffee
                ;
            ";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    long id = reader.GetInt64(0);
                    long quantity = reader.GetInt64(1);
                    DateOnly date = DateOnly.ParseExact(reader.GetString(2), GetDateFormat());
                    Habbit habbit = new(id, quantity, date);
                    habbits.Add(habbit);
                }
            }

            return habbits;
        }

        public static string GetDateFormat()
        {
            return "dd/MM/yyyy";
        }
    }
}
