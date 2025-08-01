using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabitLoggerApp.Data;
using HabitLoggerApp.Models;

namespace HabitLoggerApp.Services
{
    public class ReportService(DatabaseManager dbManager)
    {
        private readonly DatabaseManager _dbManager = dbManager;

        public HabitReport? GenerateHabitReport(int habitId, int year)
        {
            var habits = _dbManager.GetHabits();
            var habit = habits.Find(h => h.Id == habitId);
            if (habit == null)
                return null;

            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);

            int totalQuantity = 0;
            int entryCount = 0;

            using var connection = new SQLiteConnection(_dbManager._connectionString);
            connection.Open();

            string query = @"
                SELECT COUNT(*) AS EntryCount, SUM(Quantity) AS TotalQuantity
                FROM HabitEntries
                WHERE HabitId = @HabitId
                AND Date BETWEEN @StartDate AND @EndDate";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@HabitId", habitId);
            command.Parameters.AddWithValue("@StartDate", start.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@EndDate", end.ToString("yyyy-MM-dd"));

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                entryCount = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                totalQuantity = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
            }

            return new HabitReport
            {
                HabitId = habit.Id,
                HabitName = habit.Name,
                Year = year,
                EntryCount = entryCount,
                TotalQuantity = totalQuantity
            };
        }
    }
    }

