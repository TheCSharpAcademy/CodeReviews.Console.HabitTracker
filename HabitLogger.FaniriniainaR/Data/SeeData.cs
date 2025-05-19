using System.Data.SQLite;

namespace HabitLogger.Data
{
    public static class SeedData
    {
        public static void Initialize()
        {
            using var connection = Database.GetConnection();

            var checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM HabitTypes;", connection);
            long count = (long)checkCmd.ExecuteScalar();

            if (count == 0)
            {
                Console.WriteLine("Aucun HabitType trouvé. Ajout des données de base...");

                var insertCmd = new SQLiteCommand(connection);
                insertCmd.CommandText = @"
                    INSERT INTO HabitTypes (Name, Unit) VALUES
                    ('Verres d''eau', 'verres'),
                    ('Minutes de sport', 'minutes'),
                    ('Pages lues', 'pages');
                ";
                insertCmd.ExecuteNonQuery();

                Console.WriteLine("Données de base insérées dans HabitTypes.");
            }
            else
            {
                Console.WriteLine("Données déjà présentes. Pas besoin d’insérer.");
            }
        }
    }
}
