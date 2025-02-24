using Microsoft.Data.Sqlite;

ConsoleKeyInfo keyPressed;
bool endApp = false;

string connectionString = @"Data Source=HabitTracker.db";

using (SqliteConnection connection = new SqliteConnection(connectionString))
{
    connection.Open();

    SqliteCommand tableCommand = connection.CreateCommand();

    tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water ( 
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT,
                                Quantity INTEGER
                                )";

    tableCommand.ExecuteNonQuery();
}

while (!endApp)

{
    Console.Clear();
    Console.WriteLine("Habit Tracker");
    Console.WriteLine("-------------");
    Console.WriteLine(@"Press 0 to close the application.
Press 1 to view all records.
Press 2 to insert record.
Press 3 to update record.
Press 4 to delete record.");
    Console.WriteLine("-------------");

    keyPressed = Console.ReadKey();

    switch (keyPressed.Key)
    {
        case ConsoleKey.D0:
        case ConsoleKey.NumPad0:
            endApp = true;
            break;

        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
            View();
            break;

        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:
            Insert();
            break;

        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:
            Update();
            break;

        case ConsoleKey.D4:
        case ConsoleKey.NumPad4:
            Delete();
            break;
    }
}
void Insert()
{
    string date = "";
    int quantity = 0;

    Console.Clear();
    Console.WriteLine("Insert date in dd/mm/yyyy format.");
    date = Console.ReadLine();
    Console.WriteLine("Insert quantity.");
    quantity = int.Parse(Console.ReadLine());


    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        SqliteCommand insertCommand = connection.CreateCommand();

        insertCommand.CommandText = $@"INSERT INTO drinking_water (Date, Quantity)
                                       VALUES ('{date}', {quantity}
                                       )";

        insertCommand.ExecuteNonQuery();
    }
}
void Update()
{
    string date = "";
    int quantity = 0;
    int index = 0;

    Console.Clear();
    Console.WriteLine("Choose which row you want to update.");
    index = int.Parse(Console.ReadLine());
    Console.WriteLine("Insert date in dd/mm/yyyy format.");
    date = Console.ReadLine();
    Console.WriteLine("Insert quantity.");
    quantity = int.Parse(Console.ReadLine());


    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        SqliteCommand updateCommand = connection.CreateCommand();

        updateCommand.CommandText = $@"UPDATE drinking_water
                                       SET Date = '{date}', Quantity = {quantity}
                                       WHERE ID = {index}
                                       ";

        updateCommand.ExecuteNonQuery();
    }
}
void Delete()
{
    int index = 0;

    Console.Clear();

    Console.WriteLine("------------------------------------");
    Console.WriteLine("Choose which row you want to delete.");
    index = int.Parse(Console.ReadLine());


    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        SqliteCommand deleteCommand = connection.CreateCommand();

        deleteCommand.CommandText = $@"DELETE FROM drinking_water
                                       WHERE ID = {index}
                                       ";

        deleteCommand.ExecuteNonQuery();
    }
}
void View()
{
    Console.Clear();

    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        SqliteCommand selectCommand = connection.CreateCommand();

        selectCommand.CommandText = $@"SELECT * FROM drinking_water
                                       ";

        using (SqliteDataReader reader = selectCommand.ExecuteReader())
        {
            
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    var columnValue = reader.GetValue(i);

                    Console.Write($@"{columnName}: {columnValue} ");
                }
                Console.WriteLine();
            }
        }
    }
    Console.ReadKey();
}