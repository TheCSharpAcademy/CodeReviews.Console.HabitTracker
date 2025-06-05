namespace Main.Data
{
    using System.Collections.Generic;
    using Main.Models;
    using Microsoft.Data.Sqlite;

    internal class CategoryDatabase : Database
    {
        public static void Insert(Category category)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "INSERT INTO habit_category(name, unit) VALUES(@Name, @Unit)";
                tableCmd.Parameters.Add("@Name", SqliteType.Text).Value = category.Name;
                tableCmd.Parameters.Add("@Unit", SqliteType.Text).Value = category.Unit;
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static List<Category> GetAll()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM habit_category";
                List<Category> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    tableData.Add(new Category(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                }

                connection.Close();
                return tableData;
            }
        }

        public static int Update(Category category)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "UPDATE habit_category SET name=@Name, unit=@Unit WHERE id=@Id";
                tableCmd.Parameters.Add("@Name", SqliteType.Text).Value = category.Name;
                tableCmd.Parameters.Add("@Unit", SqliteType.Text).Value = category.Unit;
                tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = category.Id;
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
                tableCmd.CommandText = "DELETE from habit_category WHERE id=@Id";
                tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = id;
                int affectedRows = tableCmd.ExecuteNonQuery();
                connection.Close();
                return affectedRows;
            }
        }
    }
}
