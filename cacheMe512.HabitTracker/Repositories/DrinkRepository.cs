using System.Collections.Generic;
using habit_logger.Data;
using habit_logger.Models;
using Microsoft.Data.Sqlite;

namespace habit_logger.Repositories
{
    public class DrinkRepository
    {
        public IEnumerable<DrinkingWater> GetAllRecords()
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Date, Quantity FROM drinking_water";

            using var reader = command.ExecuteReader();
            var records = new List<DrinkingWater>();

            while (reader.Read())
            {
                records.Add(new DrinkingWater
                {
                    Id = reader.GetInt32(0),
                    Date = reader.GetDateTime(1),
                    Quantity = reader.GetInt32(2)
                });
            }

            return records;
        }

        public void InsertDrinkRecord(DrinkingWater drink)
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO drinking_water (Date, Quantity) VALUES (@Date, @Quantity)";
            command.Parameters.AddWithValue("@Date", drink.Date);
            command.Parameters.AddWithValue("@Quantity", drink.Quantity);
            command.ExecuteNonQuery();
        }

        public void UpdateDrinkRecord(DrinkingWater drink)
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE drinking_water SET Date = @Date, Quantity = @Quantity WHERE Id = @Id";
            command.Parameters.AddWithValue("@Date", drink.Date);
            command.Parameters.AddWithValue("@Quantity", drink.Quantity);
            command.Parameters.AddWithValue("@Id", drink.Id);
            command.ExecuteNonQuery();
        }

        public void DeleteDrinkRecord(int id)
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM drinking_water WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }
}
