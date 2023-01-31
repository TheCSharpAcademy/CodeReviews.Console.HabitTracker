namespace yashsachdev.HabitTracker;

public class UserRepo
{
 
    public User Retrieve(int userId)
    {
        User user = null;
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT * FROM User WHERE User_Id = @Id";
                command.Parameters.AddWithValue("@Id", userId);
                command.ExecuteNonQuery();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            User_Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Password = reader.GetString(2),
                            Email = reader.GetString(3)


                        };
                    }
                }
            }

        }
        return user;
    }

    public void Save(User user)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "INSERT INTO Users(Name,Email,Password) VALUES(@Name,@Email,@Password)";
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.ExecuteNonQuery();
            }
        }
    }
}