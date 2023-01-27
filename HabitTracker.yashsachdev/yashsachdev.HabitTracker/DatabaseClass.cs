
namespace yashsachdev.HabitTracker;
public class DatabaseClass
{
    public DatabaseClass()
    {

    }
    private readonly string _connectionString;
    public DatabaseClass(string connectionString):this()
    {
        _connectionString = connectionString;
    }
    public void CreateDatabase(string databaseName)
    {
        using (SqliteConnection connection = new SqliteConnection(_connectionString))
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
    public void CreateTable()
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {

                connection.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = connection;
                    StringBuilder sb = new StringBuilder();

                    sb.Append("CREATE TABLE IF NOT EXISTS User (User_Id INTEGER PRIMARY KEY AUTOINCREMENT,Name VARCHAR(45) NOT NULL,Email VARCHAR(45) NOT NULL,Password VARCHAR(8) NOT NULL);");
                    sb.Append("CREATE TABLE IF NOT EXISTS Habit(Habit_Id INTEGER PRIMARY KEY AUTOINCREMENT,Habit_Name VARCHAR(45) NOT NULL,unit VARCHAR(45) NOT NULL);");
                    sb.Append("CREATE TABLE IF NOT EXISTS Habit_Enroll( Users_Id INTEGER NOT NULL, Habit_Id INTEGER NOT NULL,date DATETIME NULL, PRIMARY KEY (Users_Id, Habit_Id), CONSTRAINT fk_Habit_Enroll_Users FOREIGN KEY (Users_Id) REFERENCES Users (User_Id) ON DELETE NO ACTION ON UPDATE CASCADE, CONSTRAINT fk_Habit_Enroll_Habit FOREIGN KEY (Habit_Id) REFERENCES Habit (Habit_Id) ON DELETE NO ACTION ON UPDATE CASCADE);");
                    sb.Append("CREATE TABLE IF NOT EXISTS Progess(Progess_Id INTEGER  NOT NULL PRIMARY KEY  AUTOINCREMENT, Habit_Id INT NOT NULL, User_Id INT NOT NULL, Progess_Percentage DECIMAL(5,2) NULL, Progess_Status VARCHAR(45) NULL,Habit_Habit_Id INT NOT NULL, CONSTRAINT fk_Progress_Habit FOREIGN KEY (Habit_Habit_Id) REFERENCES Habit(Habit_Id) ON DELETE NO ACTION ON UPDATE CASCADE);");
                    sb.Append("CREATE TABLE IF NOT EXISTS Goal(Goal_Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Target_Date DATETIME NOT NULL, Habit_Habit_Id INT NOT NULL, Goal_Achievement TINYINT NULL, CONSTRAINT fk_Goal_Habit FOREIGN KEY (Habit_Habit_Id) REFERENCES Habit(Habit_Id) ON DELETE NO ACTION ON UPDATE CASCADE);");
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
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
                connection.Close();
        }
    }
   
    public void NewRegistration()
    {
        using (SqliteConnection cnn = new SqliteConnection(_connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                Console.WriteLine("Enter your Name");
                string Name = Console.ReadLine();
                Console.WriteLine("enter your email Id");
                string Email = Console.ReadLine();
                Console.WriteLine("Enter ur password");
                string Password = Console.ReadLine();  
                command.CommandText = "INSERT INTO User(Name,Email,Password) VALUES(@Name,@Email,@Password)";
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@Password", Password);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                { 
                    Console.WriteLine("User " + Name + " created successfully");
                }
                else
                {

                    Console.WriteLine("Failed to create user " + Name);
                }

            }

        }
    }
    public void ExistingUserLogin()
    {
        Console.WriteLine("Enter Email:");
        string email = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();
        using (SqliteConnection cnn = new SqliteConnection(_connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT COUNT(*) FROM User WHERE Email = @Email AND Password = @Password";
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                Int64 result = (Int64)command.ExecuteScalar();
                if (result > 0)
                {
                    Console.WriteLine("Welcome!!!");
                }
                else
                {
                    Console.WriteLine("Invalid User");
                }
                cnn.Close();
            }

        }


    }
}
