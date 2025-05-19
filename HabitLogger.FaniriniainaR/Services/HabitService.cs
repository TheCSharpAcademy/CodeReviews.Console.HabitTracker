using HabitLogger.Data;
using System.Data.SQLite;

namespace HabitLogger.Services
{
    public static class HabitService
    {
        public static void AddHabit(string date, int quantity, int typeId)
        {
            try
            {
                using var connection = Database.GetConnection();
                var insert = new SQLiteCommand("INSERT INTO Habits (Date, Quantity, HabitTypeId) VALUES (@date, @quantity, @typeId);", connection);
                insert.Parameters.AddWithValue("@date", date);
                insert.Parameters.AddWithValue("@quantity", quantity);
                insert.Parameters.AddWithValue("@typeId", typeId);
                insert.ExecuteNonQuery();

                Console.WriteLine("Habitude ajoutée avec succès !");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        public static List<(int Id, string Name, string Unit)> GetHabitTypes()
        {
            var types = new List<(int Id, string Name, string Unit)>();

            try
            {
                using var connection = Database.GetConnection();
                var cmd = new SQLiteCommand("SELECT Id, Name, Unit FROM HabitTypes;", connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    types.Add((reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }

            return types;
        }

        public static void AddHabitType(string name, string unit)
        {
            try
            {
                using var connection = Database.GetConnection();
                var insert = new SQLiteCommand("INSERT INTO HabitTypes (Name, Unit) VALUES (@name, @unit);", connection);
                insert.Parameters.AddWithValue("@name", name);
                insert.Parameters.AddWithValue("@unit", unit);
                insert.ExecuteNonQuery();

                Console.WriteLine("Type d’habitude ajouté avec succès.");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        public static void ShowHabitTypes()
        {
            try
            {
                using var connection = Database.GetConnection();
                var cmd = new SQLiteCommand("SELECT Id, Name, Unit FROM HabitTypes;", connection);
                using var reader = cmd.ExecuteReader();

                Console.WriteLine("\nTypes d’habitudes disponibles :");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetInt32(0)} - {reader.GetString(1)} ({reader.GetString(2)})");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        public static void ShowAllHabits()
        {
            using var connection = Database.GetConnection();

            var query = @"
                SELECT H.Date, HT.Name, H.Quantity, HT.Unit
                FROM Habits H
                JOIN HabitTypes HT ON H.HabitTypeId = HT.Id
                ORDER BY H.Date DESC;
            ";

            using var command = new SQLiteCommand(query, connection);
            using var reader = command.ExecuteReader();

            Console.WriteLine("\n Liste des habitudes enregistrées :\n");

            while (reader.Read())
            {
                string date = reader.GetString(0);
                string typeName = reader.GetString(1);
                int quantity = reader.GetInt32(2);
                string unit = reader.GetString(3);

                Console.WriteLine($"{date} | {typeName} | {quantity} {unit}");
            }

            Console.WriteLine();
        }

        public static void ShowHabitsByType(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                Console.WriteLine("Type d’habitude invalide.");
                return;
            }

            try
            {
                using var connection = Database.GetConnection();
                string query = @"
                    SELECT H.Date, HT.Name, H.Quantity, HT.Unit
                    FROM Habits H
                    JOIN HabitTypes HT ON H.HabitTypeId = HT.Id
                    WHERE HT.Name = @TypeName
                    ORDER BY H.Date DESC;
                ";

                using var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@TypeName", typeName);

                using var reader = command.ExecuteReader();

                bool hasResults = false;
                Console.WriteLine($"\nHabitudes pour le type '{typeName}' :\n");

                while (reader.Read())
                {
                    hasResults = true;
                    string date = reader.GetString(0);
                    string name = reader.GetString(1);
                    int quantity = reader.GetInt32(2);
                    string unit = reader.GetString(3);

                    Console.WriteLine($" {date} | {name} | {quantity} {unit}");
                }

                if (!hasResults)
                    Console.WriteLine("Aucune habitude trouvée pour ce type.");

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        public static void ShowHabitsByDateRange(string startDate, string endDate)
        {
            if (!DateTime.TryParse(startDate, out _) || !DateTime.TryParse(endDate, out _))
            {
                Console.WriteLine("Format de date invalide. Utilisez AAAA-MM-JJ.");
                return;
            }

            try
            {
                using var connection = Database.GetConnection();
                string query = @"
                    SELECT H.Date, HT.Name, H.Quantity, HT.Unit
                    FROM Habits H
                    JOIN HabitTypes HT ON H.HabitTypeId = HT.Id
                    WHERE Date BETWEEN @Start AND @End
                    ORDER BY H.Date;
                ";

                using var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@Start", startDate);
                command.Parameters.AddWithValue("@End", endDate);

                using var reader = command.ExecuteReader();

                bool hasResults = false;
                Console.WriteLine($"\n Habitudes entre {startDate} et {endDate} :\n");

                while (reader.Read())
                {
                    hasResults = true;
                    string date = reader.GetString(0);
                    string name = reader.GetString(1);
                    int quantity = reader.GetInt32(2);
                    string unit = reader.GetString(3);

                    Console.WriteLine($" {date} |  {name} | {quantity} {unit}");
                }

                if (!hasResults)
                    Console.WriteLine("Aucune habitude trouvée dans cette période.");

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        public static void ShowStatistics()
        {
            try
            {
                using var connection = Database.GetConnection();
                string query = @"
                    SELECT HT.Name, SUM(H.Quantity), AVG(H.Quantity), COUNT(*)
                    FROM Habits H
                    JOIN HabitTypes HT ON H.HabitTypeId = HT.Id
                    GROUP BY HT.Name;
                ";

                using var command = new SQLiteCommand(query, connection);
                using var reader = command.ExecuteReader();

                Console.WriteLine("\n Statistiques par type d’habitude :\n");
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    int total = reader.GetInt32(1);
                    double average = reader.GetDouble(2);
                    int count = reader.GetInt32(3);

                    Console.WriteLine($" {name} | Total: {total}, Moyenne: {average:F2}, Fréquence: {count}");
                }

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private static void ShowError(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Une erreur est survenue : " + ex.Message);
            Console.ResetColor();
        }
    }
}