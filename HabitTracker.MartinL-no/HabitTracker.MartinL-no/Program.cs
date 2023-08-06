namespace HabitTracker.MartinL_no;

internal class Program
{
    static void Main(string[] args)
    {
        var repo = new HabitRepository();
        var service = new HabitService(repo);
        var application = new HabitTrackerApplication(service);
        application.Execute();
    }
}
        
/*
            Console.Write("Name: ");
            var name = Console.ReadLine();

            #region snippet_Parameter
            command.CommandText =
            @"
                INSERT INTO user (name)
                VALUES ($name)
            ";
            command.Parameters.AddWithValue("$name", name);
            #endregion
            command.ExecuteNonQuery();

            command.CommandText =
            @"
                SELECT last_insert_rowid()
            ";
            var newId = (long)command.ExecuteScalar();

            Console.WriteLine($"Your new user ID is {newId}.");
        }

        Console.Write("User ID: ");
        var id = int.Parse(Console.ReadLine());

        #region snippet_HelloWorld
        using (var connection = new SqliteConnection("Data Source=hello.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT name
                FROM user
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", id);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var name = reader.GetString(0);

                    Console.WriteLine($"Hello, {name}!");
                }
            }
        }
        #endregion

        // Clean up
        //File.Delete("hello.db");
    }
}
}
*/