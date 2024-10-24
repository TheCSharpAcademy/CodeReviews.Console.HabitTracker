
using CrudLibrary;
using Microsoft.Data.Sqlite;

namespace ReportLibrary
{

    public class Reports
    {
        private SqliteCrudController SqliteController { get; }

        public Reports(SqliteCrudController controller) => SqliteController = controller;

        public void View()
        {
            using (var connection = new SqliteConnection(SqliteController.ConnectionString))
            {
                connection.Open();

                string[] queries =
                {
                    $"SELECT COUNT(*) FROM {SqliteController.Habit}",
                    $"SELECT SUM(Quantity) FROM {SqliteController.Habit}",
                    $"SELECT AVG(Quantity) FROM {SqliteController.Habit}"
                };

                int loggedAmount = ExecuteScalarQuery(connection, queries[0]);
                int unitOfMeasureAmount = ExecuteScalarQuery(connection, queries[1]);
                double avgQuantity = ExecuteScalarQueryDouble(connection, queries[2]);

                
                connection.Close();
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine($"Logged Habit instances: {loggedAmount}");
                Console.WriteLine($"Total {SqliteController.UnitOfMeasure}: {unitOfMeasureAmount}");
                Console.WriteLine($"Average {SqliteController.UnitOfMeasure}: {avgQuantity}");
                Console.WriteLine("---------------------------------------------------------------");
            }
        }

        private int ExecuteScalarQuery(SqliteConnection connection, string query) 
        {
            using (var command = new SqliteCommand(query, connection))
            {
                return Convert.ToInt32(command.ExecuteScalar() ?? 0);
            }
        }

        private double ExecuteScalarQueryDouble(SqliteConnection connection, string query)
        {
            using (var command = new SqliteCommand(query, connection))
            {
                return Convert.ToDouble(command.ExecuteScalar() ?? 0.0);
            }
        }
    }
}
