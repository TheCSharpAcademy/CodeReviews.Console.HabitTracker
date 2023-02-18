using System.Xml.Linq;

namespace yashsachdev.HabitTracker;
public class UserRepo
{
    public void Save(User user)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "INSERT INTO User(Name,Email,Password) VALUES(@Name,@Email,@Password);";
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.ExecuteNonQuery();
            }
        }
    }
    public int GetIdFromEmail(string email)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT User_Id FROM User WHERE Email = @email";
                command.Parameters.AddWithValue("@email", email);
                var result = command.ExecuteScalar();
                if (result == null)
                {
                    Console.WriteLine("No Data returned");
                    return 0;
                }
                int userid = Convert.ToInt32(result);
                return userid;
            }
        }
    }
    public int GetIdFromEmail(string email,SqliteConnection cnn)
    {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT User_Id FROM User WHERE Email = @email";
                command.Parameters.AddWithValue("@email", email);
                var result = command.ExecuteScalar();
                if (result == null)
                {
                    Console.WriteLine("No Data returned");
                    return 0;
                }
                int userid = Convert.ToInt32(result);
                return userid;
            }
    }
    public string GetNameFromEmail(string email)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            SqliteCommand cmd = new SqliteCommand("SELECT Name FROM User WHERE Email = @email", cnn);
            cmd.Parameters.AddWithValue("@email", email);
            string name = (string)cmd.ExecuteScalar();
            return name;
        }
    }
    public int CountofUser(string Email)
    {
        try
        {
            using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
            {
                cnn.Open();
                SqliteCommand cmd = new SqliteCommand("SELECT COUNT(*) FROM User WHERE Email = @email", cnn);
                cmd.Parameters.AddWithValue("@email", Email);
                var emailCount = Convert.ToInt32(cmd.ExecuteScalar());
                return emailCount;
            }
        }
        catch (Exception ex) { Console.WriteLine("An error occurred"+ex.Message); }
        return int.MinValue;
    }
    public bool CheckPassword(string email, string password)
    {
            using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
            {
                cnn.Open();
                SqliteCommand cmd = new SqliteCommand("SELECT Password FROM User WHERE Email = @email", cnn);
                cmd.Parameters.AddWithValue("@email", email);
                string correctPassword = (string)cmd.ExecuteScalar();
            return correctPassword == password; 
            }
    }
}