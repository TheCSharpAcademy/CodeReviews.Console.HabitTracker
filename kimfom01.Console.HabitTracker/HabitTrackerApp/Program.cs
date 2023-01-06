using Microsoft.Data.Sqlite;
Console.Clear();

string connectionString = @"Data Source=habit-Tracker.db";

CreateDatabase();

void CreateDatabase()
{
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var tableCmd = connection.CreateCommand())
        {
            connection.Open();
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS walkingHabit (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TXT,
                    Quantity INTEGER
                    )";
            tableCmd.ExecuteNonQuery();
        }
    }

    GetUserInput();
}

void GetUserInput()
{
    string choice;
    bool is_menu_running;

    is_menu_running = true;
    while (is_menu_running == true)
    {
        Console.WriteLine("MAIN MENU\n");
        Console.WriteLine("What would you like to do?\n");
        Console.WriteLine("Type 0 to Close Application");
        Console.WriteLine("Type 1 to View All Records");
        Console.WriteLine("Type 2 to Insert Record");
        Console.WriteLine("Type 3 to Delete Record");
        Console.WriteLine("Type 4 to Update Record");

        choice = Console.ReadLine();

        switch (choice)
        {
            case "0":
                is_menu_running = false;
                return;
            case "1":
                ViewAllRecords();
                break;
            case "2":
                InsertRecord();
                break;
            case "3":
                ChooseRecordFilterToDelete();
                break;
            case "4":
                ChooseFieldToUpdate();
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }
}

void ViewAllRecords()
{
    Console.Clear();
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var command = connection.CreateCommand())
        {
            connection.Open();
            command.CommandText =
                @"SELECT * FROM walkingHabit";

            var reader = command.ExecuteReader();

            if (reader.HasRows == true)
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetInt32(0)}: {reader.GetString(1)} - {reader.GetDouble(2)} time(s)");
                }
            }
            else
            {
                Console.WriteLine("\nNo habit has been logged");
            }
            Console.WriteLine();
        }
    }
}

void InsertRecord()
{
    double quantity;
    string date;

    date = GetDate();
    quantity = GetQuantity();

    using (var connection = new SqliteConnection(connectionString))
    {
        using (var command = connection.CreateCommand())
        {
            connection.Open();
            command.CommandText =
                @$"INSERT INTO walkingHabit (Date, Quantity)
                    VALUES ('{date}', '{quantity}')";

            command.ExecuteNonQuery();
        }
    }
}

void UpdateRecord(int id, string? date, double? quantity)
{
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var command = connection.CreateCommand())
        {
            connection.Open();
            if (date is null)
            {
                command.CommandText =
                    @$"UPDATE walkingHabit
                        SET Quantity = {quantity}
                        WHERE Id = {id}";
            }
            else if (quantity is null)
            {
                command.CommandText =
                    @$"UPDATE walkingHabit
                        SET Date = '{date}'
                        WHERE Id = {id}";
            }
            else
            {
                command.CommandText =
                    @$"UPDATE walkingHabit
                        SET Date = '{date}', Quantity = {quantity}
                        WHERE Id = {id}";
            }

            command.ExecuteNonQuery();
        }
    }
}

void DeleteRecord(string? date, double? quantity)
{
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var command = connection.CreateCommand())
        {
            connection.Open();
            if (date is null)
            {
                command.CommandText =
                    @$"DELETE FROM walkingHabit WHERE Quantity = {quantity}";
            }
            else if (quantity is null)
            {
                command.CommandText =
                    $@"DELETE FROM walkingHabit WHERE Date = '{date}'";
            }
            if (date is null && quantity is null)
            {
                command.CommandText =
                    @"DELETE FROM walkingHabit";
            }
            command.ExecuteNonQuery();
        }
    }
}

void ChooseFieldToUpdate()
{
    ViewAllRecords();

    int id = GetId();

    bool is_menu_running = true;
    while (is_menu_running == true)
    {
        Console.WriteLine("Choose field to update:");
        Console.WriteLine("0. Go back");
        Console.WriteLine("1. Date");
        Console.WriteLine("2. Quantity");
        Console.WriteLine("3. All");
        string field_choice = Console.ReadLine();

        double quantity;
        string date;
        switch (field_choice)
        {
            case "0":
                is_menu_running = false;
                break;
            case "1":
                date = GetDate();
                UpdateRecord(id, date, null);
                is_menu_running = false;
                break;
            case "2":
                quantity = GetQuantity();
                UpdateRecord(id, null, quantity);
                is_menu_running = false;
                break;
            case "3":
                date = GetDate();
                quantity = GetQuantity();
                UpdateRecord(id, date, quantity);
                is_menu_running = false;
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }
}

void ChooseRecordFilterToDelete()
{
    string choice, date;
    bool is_menu_running;

    is_menu_running = true;
    while (is_menu_running == true)
    {
        ViewAllRecords();

        Console.WriteLine("Choose filter to delete from:");
        Console.WriteLine("0. Go back");
        Console.WriteLine("1. Date");
        Console.WriteLine("2. Quantity");
        Console.WriteLine("3. Delete All Records");

        choice = Console.ReadLine();

        switch (choice)
        {
            case "0":
                is_menu_running = false;
                break;
            case "1":
                date = GetDate();
                DeleteRecord(date, null);
                is_menu_running = false;
                break;
            case "2":
                double quantity = GetQuantity();
                DeleteRecord(null, quantity);
                is_menu_running = false;
                break;
            case "3":
                DeleteRecord(null, null);
                is_menu_running = false;
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }
}

string GetDate()
{
    Console.WriteLine("Enter the date (format - dd-mm-yyyy): ");
    string date = Console.ReadLine();
    while (!IsValidDate(date))
    {
        Console.WriteLine("Invalid date given!");
        Console.WriteLine("Enter the date (format - dd-mm-yyyy): ");
        date = Console.ReadLine();
    }

    return date;
}

bool IsValidDate(string date)
{
    var successDay = int.TryParse(date.Substring(0, 2), out int day);
    var successMonth = int.TryParse(date.Substring(3, 2), out int month);
    var successYear = int.TryParse(date.Substring(6), out int year);

    if (!successDay || !successMonth || !successYear && date.Substring(6).Length != 4)
    {
        return false;
    }

    return IsValidDay(day, month, year);
}

double GetQuantity()
{
    double quantity;
    bool success;

    Console.Write("Enter the quantity: ");
    success = Double.TryParse(Console.ReadLine(), out quantity);
    while (success == false)
    {
        Console.Write("Invalid value! Enter the quantity: ");
        success = Double.TryParse(Console.ReadLine(), out quantity);
    }

    return quantity;
}

int GetId()
{
    int id;
    bool success;

    Console.Write("Enter the Id to update: ");
    success = Int32.TryParse(Console.ReadLine(), out id);
    while (success == false)
    {
        Console.Write("Invaid value! Enter the Id to update: ");
        success = Int32.TryParse(Console.ReadLine(), out id);
    }

    return id;
}

bool IsValidDay(int day, int month, int year)
{
    switch (month)
    {
        case 2:
            if (IsLeapYear(year) && day > 29 || !IsLeapYear(year) && day > 28)
                return false;
            break;
        case 4:
        case 6:
        case 9:
        case 11:
            if (day > 30)
            {
                return false;
            }
            break;
        case 1:
        case 3:
        case 5:
        case 7:
        case 8:
        case 10:
        case 12:
            if (day > 31)
            {
                return false;
            }
            break;
        default:
            return false;
    }

    return true;
}

bool IsLeapYear(int year)
{
    return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
}