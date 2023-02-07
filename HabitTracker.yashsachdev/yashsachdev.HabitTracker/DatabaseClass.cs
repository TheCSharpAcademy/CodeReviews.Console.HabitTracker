public static class DatabaseClass
{
    public static readonly string connectionString = "Data Source=Habit-Tracker.db";
    public static void CreateDatabase()
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                Console.WriteLine(connection.State.ToString());
                SqliteCommand command = new SqliteCommand($"ATTACH DATABASE 'Habit-tracker.db' AS Habit;", connection);
                command.ExecuteNonQuery();
                Console.WriteLine("Database Created");
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("error:" + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
    public static void CreateTable()
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = connection;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("CREATE TABLE IF NOT EXISTS User (User_Id INTEGER PRIMARY KEY AUTOINCREMENT,Name VARCHAR(45) NOT NULL,Email VARCHAR(45) NOT NULL,Password VARCHAR(8) NOT NULL);");
                    sb.Append("CREATE TABLE IF NOT EXISTS Habit(Habit_Id INTEGER PRIMARY KEY AUTOINCREMENT,Habit_Name VARCHAR(45) NOT NULL,Unit VARCHAR(45) NOT NULL);");
                    sb.Append("CREATE TABLE IF NOT EXISTS Habit_Enroll( User_Id INTEGER NOT NULL, Habit_Id INTEGER NOT NULL,Date DATETIME NULL, PRIMARY KEY (User_Id, Habit_Id), CONSTRAINT fk_Habit_Enroll_Users FOREIGN KEY (User_Id) REFERENCES User (User_Id) ON DELETE NO ACTION ON UPDATE CASCADE, CONSTRAINT fk_Habit_Enroll_Habit FOREIGN KEY (Habit_Id) REFERENCES Habit (Habit_Id) ON DELETE NO ACTION ON UPDATE CASCADE);");
                    command.CommandText = sb.ToString();
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            connection.Close();
        }
    }
}
