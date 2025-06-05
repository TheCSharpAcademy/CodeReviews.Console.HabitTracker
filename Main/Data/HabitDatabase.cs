using Main.Models;
using Microsoft.Data.Sqlite;

namespace Main.Data
{
    internal class HabitDatabase: Database
    {
        public static void Insert(Habit habit)
        {
            using (var connection = GetConnection())
            {
                if (habit.Category?.Id == null)
                {
                    throw new Exception("Habit must have a category defined");
                }
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "INSERT INTO habit(date, quantity, category_id) VALUES(@Date, @Quantity, @CategoryId)";
                tableCmd.Parameters.Add("@Date", SqliteType.Text).Value = habit.Date;
                tableCmd.Parameters.Add("@Quantity", SqliteType.Integer).Value = habit.Quantity;
                tableCmd.Parameters.Add("@CategoryId", SqliteType.Integer).Value = habit.Category.Id;
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static List<Habit> GetHabitsForCategory(int categoryId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"
                    SELECT
                        h.id AS habitId,
                        h.date,
                        h.quantity,
                        hc.id AS categoryId,
                        hc.name AS categoryName,
                        hc.unit
                    FROM habit h
                    INNER JOIN habit_category hc ON h.category_id = hc.id
                    WHERE categoryId=@CategoryId;
                ";
                tableCmd.Parameters.Add("@CategoryId", SqliteType.Integer).Value = categoryId;
                List<Habit> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    tableData.Add(new Habit
                    {
                        Id = reader.GetInt32(0),
                        Date = reader.GetString(1),
                        Quantity = reader.GetInt32(2),
                        Category = new Category(reader.GetInt32(3), reader.GetString(4), reader.GetString(5))
                    });
                }

                connection.Close();
                return tableData;
            }
        }

        public static int Update(Habit habit)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "UPDATE habit SET date=@Date, quantity=@Quantity WHERE id=@Id";
                tableCmd.Parameters.Add("@Date", SqliteType.Text).Value = habit.Date;
                tableCmd.Parameters.Add("@Quantity", SqliteType.Integer).Value = habit.Quantity;
                tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = habit.Id;
                int rowsAffected = tableCmd.ExecuteNonQuery();
                connection.Close();
                return rowsAffected;
            }
        }

        public static int Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "DELETE from habit WHERE id=@Id";
                tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = id;
                int affectedRows = tableCmd.ExecuteNonQuery();
                connection.Close();
                return affectedRows;
            }
        }
    }
}
