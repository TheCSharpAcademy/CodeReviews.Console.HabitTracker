using AnaClos.HabitTracker;
using Microsoft.Data.Sqlite;
using System.Globalization;


string choice = "q";
string connectionString = @"Data Source=habit_Tracker.db";

CreateBaseAndTable(connectionString);

choice =Menu();

while (choice != "q")
{
    switch (choice)
    {
        case "i":
            Insert(connectionString);
            break;
        case "v":
            View(connectionString);
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            break;
        case "d":
            Delete(connectionString);
            break;
        case "u":
            Update(connectionString);
            break;
        case "q":
            Console.WriteLine("By!!!");
            break;
    }
    choice = Menu();
}

void CreateBaseAndTable(string connectionString)
{
    using (var connection = new SqliteConnection(connectionString))
    {

        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = 
            @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT, 
            Date TEXT,
            Quantity INTEGER
            )";

        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
}

string Menu()
{
    string choice = string.Empty;

    Console.Clear();
    Console.WriteLine("Select your choice\n");
    Console.WriteLine("(I) Insert Habit");
    Console.WriteLine("(D) Delete Habit");
    Console.WriteLine("(U) Update Habit");
    Console.WriteLine("(V) View All Habits");
    Console.WriteLine("(Q) Quit\n");
    Console.WriteLine("------------------------------------------\n");

    choice = Console.ReadLine().ToLower().Trim();

    return choice;
}

void View(string connectionString)
{
    using (var connection = new SqliteConnection(connectionString))
    {
        List<Habit> list = new List<Habit>();

        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            "SELECT * FROM drinking_water";

        SqliteDataReader reader = tableCmd.ExecuteReader();
        if (reader.HasRows)
        {
            while(reader.Read())
            {
                list.Add(new Habit { Id = reader.GetInt32(0),Date=reader.GetString(1),Quantity=reader.GetInt32(2)});
            }
        }        

        connection.Close();
        Console.WriteLine();
        foreach (var item in list)
        {
            Console.WriteLine(item.ToString());
        }        
    }
}

void Insert(string connectionString)
{
    string date = string.Empty;
    string response = string.Empty;
    int quantity = 0;
    bool ok = false;
    while( !ok )
    {
        date = GetDateInput();
        ok = ValidateDate(date);
    }
    if (date == "r")
    {
        return;
    }
    ok = false;
    while( !ok ) 
    {
        response = GetInput("Enter the Quantity of glasses you drink");
        ok = ValidateInt(response);
    }
    if (response == "r")
    {
        return;
    }
    quantity = Int32.Parse(response);

    var myHabit = new Habit { Date = date, Quantity = quantity };

    using (var connection = new SqliteConnection(connectionString))
    {

        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
        @$"INSERT INTO drinking_water
        (Date,Quantity)
        VALUES (
        '{myHabit.Date}',
        {myHabit.Quantity}
        )";

        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
}

void Update(string connectionString)
{
    string date = string.Empty;
    string response = string.Empty;
    int quantity = 0;
    int id = 0;
    bool ok = false;

    View(connectionString);

    Console.WriteLine();

    while (!ok)
    {
        response = GetInput("Enter the Id to Update.");
        ok = ValidateInt(response);
    }
    if (response == "r")
    {
        return;
    }
    id = Int32.Parse(response);
    ok = false;

    while (!ok)
    {
        date = GetDateInput();
        ok = ValidateDate(date);
    }
    if (date == "r")
    {
        return;
    }
    ok = false;
    while (!ok)
    {
        response = GetInput("Enter the Quantity of glasses you drink");
        ok = ValidateInt(response);
    }
    if (response == "r")
    {
        return;
    }
    quantity = Int32.Parse(response);

    var myHabit = new Habit { Id = id, Date = date, Quantity = quantity };

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
        @$"UPDATE drinking_water
        SET Date = '{myHabit.Date}', Quantity = {myHabit.Quantity}
        WHERE Id = {myHabit.Id}";

        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
}


void Delete(string connectionString)
{
    string response=string.Empty;
    int id = 0;
    bool ok = false;

    View(connectionString);

    Console.WriteLine();

    while (!ok)
    {
        response = GetInput("Enter the Id to delete.");
        ok = ValidateInt(response);
    }
    if (response == "r")
    {
        return;
    }

    id = Int32.Parse(response);
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
        @$"DELETE FROM drinking_water
                WHERE Id = {id}";

        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
}

bool ValidateDate(string date)
{
    bool ok =false;
    DateTime dateTime = DateTime.MinValue;

    if (date == "r") 
    { 
        ok = true;
    }
    else
    {
        try
        {
            dateTime = DateTime.ParseExact(date, "dd-MM-yy", new CultureInfo("en-US"));
            ok = true;
        }
        catch(Exception ex)
        {
            ok = false;
        }        
    }
    
    return ok;
}

bool ValidateInt(string response)
{
    bool ok = false;
    int number = 0;

    if (response == "r")
    {
        ok = true;
    }
    else
    {
        try
        {
            number = Int32.Parse(response);
            ok= true;
        }
        catch (Exception ex)
        {
            ok = false;
        }
    }
    return ok;
}



string GetDateInput()
{
    string date=string.Empty;

    Console.WriteLine();
    Console.WriteLine(@"Enter the Date in format dd-MM-yy. Type 'R' to return to main menu.");

    date = Console.ReadLine().ToLower().Trim();
    return date;
}


string GetInput(string message)
{
    string response=string.Empty;

    Console.WriteLine();
    Console.WriteLine($@"{message} Type 'R' to return to main menu.");
    response= Console.ReadLine().ToLower().Trim();
    
    return response;
}

string GetIdInput()
{
    string id = string.Empty;

    Console.WriteLine();
    Console.WriteLine(@"Enter the Id. Type 'R' to return to main menu.");

    id = Console.ReadLine().ToLower().Trim();

    return id;
}