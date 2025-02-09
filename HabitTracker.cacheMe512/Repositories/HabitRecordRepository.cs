using System.Collections.Generic;
using habit_logger.Data;
using habit_logger.Models;
using Microsoft.Data.Sqlite;

namespace habit_logger.Repositories
{
    public class HabitRecordRepository
    {

        public IEnumerable<HabitRecord> GetRecordsForHabit(int id)
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, HabitId, Date, Quantity FROM habit_records WHERE HabitId = @Id";
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            var records = new List<HabitRecord>();

            while (reader.Read())
            {
                records.Add(new HabitRecord
                {
                    Id = reader.GetInt32(0),
                    HabitId = reader.GetInt32(1),
                    Date = reader.GetDateTime(2),
                    Quantity = reader.GetInt32(3)
                });
            }

            return records;            
        }

        public IEnumerable<HabitRecord> GetAllRecords()
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, HabitId, Date, Quantity FROM habit_records";

            using var reader = command.ExecuteReader();
            var records = new List<HabitRecord>();

            while (reader.Read())
            {
                records.Add(new HabitRecord
                {
                    Id = reader.GetInt32(0),
                    HabitId = reader.GetInt32(1),
                    Date = reader.GetDateTime(2),
                    Quantity = reader.GetInt32(3)
                });
            }

            return records;
        }

        public void InsertHabitRecord(HabitRecord record)
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = 
                "INSERT INTO habit_records (HabitId, Date, Quantity) VALUES (@HabitId, @Date, @Quantity)";
            command.Parameters.AddWithValue("@HabitId", record.HabitId);
            command.Parameters.AddWithValue("@Date", record.Date);
            command.Parameters.AddWithValue("@Quantity", record.Quantity);
            command.ExecuteNonQuery();
        }

        public void UpdateHabitRecord(HabitRecord record)
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE habit_records SET Date = @Date, Quantity = @Quantity WHERE Id = @Id";
            command.Parameters.AddWithValue("@Date", record.Date);
            command.Parameters.AddWithValue("@Quantity", record.Quantity);
            command.Parameters.AddWithValue("@Id", record.Id);
            command.ExecuteNonQuery();
        }

        public void DeleteHabitRecord(int id)
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM habit_records WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }
}
