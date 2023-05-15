using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    internal class DbManager
    {

        public static void Insert()
        {

            string date = Helpers.GetDateInput();

            string activity = Helpers.GetActivityInput();

            string unit = Helpers.GetUnitInput();

            int amount = Helpers.GetNumberInput("Enter the amount as an integer: (1, 2, 3)." +
                                        "\nType 0 to return to the main menu");

            using (var connection = new SqliteConnection(Program.ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO habits(Date, Activity, Unit, Amount) VALUES ('{date}', '{activity}', '{unit}', {amount})";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            Console.Clear();
        }

        public static void Update()
        {
            Console.Clear();

            DbViewer.ViewRecords();

            var recordId = Helpers.GetNumberInput("\n\nPlease type the Id of the record you'd like to update " +
                                                "or 0 to return to the Main Menu");
            using (var connection = new SqliteConnection(Program.ConnectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Id = {recordId})";
                var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());//returns 0 for false 1 for true

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nThe following record Id doesnt exist: {recordId}\n\n");
                    connection.Close();
                    Update();
                }

                string date = Helpers.GetDateInput();

                string activity = Helpers.GetActivityInput();

                string unit = Helpers.GetUnitInput();

                var amount = Helpers.GetNumberInput("Enter the amount as an integer: (1, 2, 3)." +
                                        "\nType 0 to return to the main menu");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE habits SET date = '{date}', Activity = '{activity}', Unit = '{unit}', Amount = {amount}" +
                                      $" WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void Delete()
        {
            Console.Clear();
            DbViewer.ViewRecords();

            var recordId = Helpers.GetNumberInput("\n\nPlease type the Id of the record you'd like to delete " +
                                                "or 0 to return to the Main Menu");

            using (var connection = new SqliteConnection(Program.ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE from habits WHERE Id = '{recordId}'";

                var rowCount = tableCmd.ExecuteNonQuery();//returns the amount of rows affected by command

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nThe record you entered does not exist: {recordId}. Press Enter to continue.\n\n");
                    Console.ReadLine();

                    Delete();
                }
            }

            Console.WriteLine($"The following record Id was deleted: {recordId}. Press Enter to return to main menu.\n\n");
            Console.ReadLine();

            MainMenu.GetUserInput();
        }
    }
}