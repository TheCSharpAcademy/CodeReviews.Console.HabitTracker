using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Text.RegularExpressions;

string connectionString = @"Data Source=.\MyDatabase.db;Version=3;";
string mainMenu = "Type 0 to close application\n" +
                  "Type 1 to view all habits\n" +
                  "Type 2 to add a habit\n" +
                  "Type 3 to edit a habit\n" +
                  "Type 4 to delete a habit\n" +
                  "------------------------------";

string createTable = "CREATE TABLE IF NOT EXISTS \"Habits\" (\"id\" INTEGER NOT NULL, \"habit\" TEXT, \"quantity\" TEXT, \"date\" TEXT, PRIMARY KEY(\"id\" AUTOINCREMENT));";
string createHabit = "INSERT INTO Habits (\"habit\", \"quantity\", \"date\") VALUES (@habit, @quantity, @date);";
string readHabits = "SELECT * FROM Habits";
string updateHabit = "UPDATE Habits SET \"habit\" = @habit, \"quantity\" = @quantity, \"date\"=@date WHERE id = @id;";
string deleteHabit = "DELETE FROM Habits WHERE id = @id;";
string checkIdQuery = "SELECT COUNT(1) FROM Habits WHERE id = @id";

string dateRegex = "^(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/\\d{4}$";
Console.WriteLine("Welcome to the habit logger");
createTableFunct();
bool flag = true;
while (flag)
{
    Console.WriteLine(mainMenu);
    string? input = Console.ReadLine();
    switch (input)
    {
        case "0":
            flag = false;
            break;
        case "1":
            readHabitsFunct();
            break;
        case "2":
            createHabitFunct();
            break;
        case "3":
            updateHabitFunct();
            break;
        case "4":
            deleteHabitFunct();
            break;
        default:
            Console.WriteLine("Invalid input");
            break;
    }
}

void createTableFunct()
{
    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
    {
        try
        {
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand(createTable, connection))
            {
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Table created");
            connection.Close();
        }
        catch (Exception ex)
        { 
            Console.WriteLine("An error occurred creating the table: " + ex.ToString());
        }
        
    }
}

void createHabitFunct()
{
    string habit = getHabit();
    string quantity = getQuantity();
    string date = getDate();

    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
    {
        try
        {
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand(createHabit, connection))
            {
                command.Parameters.AddWithValue("@habit", habit);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@date", date);

                command.ExecuteNonQuery();
            }
            Console.WriteLine("Habit created");
            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred creating the habit: " + ex.ToString());
        }
    }
}

void readHabitsFunct()
{
    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
    {
        try
        {
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand(readHabits, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Habit ID: {reader["id"]} - {reader["habit"]}, {reader["quantity"]}, {reader["date"]}");
                    }
                }
            }
            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred reading the habits table: " + ex.ToString());
        }
    }
}

void updateHabitFunct()
{
    string idToUpdate = getId();

    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
    {
        connection.Open();

        // Check if the ID exists
        using (SQLiteCommand checkCommand = new SQLiteCommand(checkIdQuery, connection))
        {
            checkCommand.Parameters.AddWithValue("@id", idToUpdate);

            int count = Convert.ToInt32(checkCommand.ExecuteScalar());  // Returns the count of rows

            if (count > 0) // If count > 0, the ID exists
            {
                Console.WriteLine($"Record with ID {idToUpdate} exists. Proceeding to edit...");
                string newHabit = getHabit();
                string newQuantity = getQuantity();
                string newDate = getDate();
                using (SQLiteCommand updateCommand = new SQLiteCommand(updateHabit, connection))
                {
                    updateCommand.Parameters.AddWithValue("@id", idToUpdate);
                    updateCommand.Parameters.AddWithValue("@habit", newHabit);
                    updateCommand.Parameters.AddWithValue("@date", newDate);
                    updateCommand.Parameters.AddWithValue("@quantity", newQuantity);

                    updateCommand.ExecuteNonQuery();
                }
                Console.WriteLine($"Habit with ID {idToUpdate} is edited.");
            }
            else
            {
                Console.WriteLine($"Habit with ID {idToUpdate} does not exist.");
            }
        }
    }

}

void deleteHabitFunct()
{
    string idToDelete = getId();
    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
    {
        connection.Open();

        // Check if the ID exists
        using (SQLiteCommand checkCommand = new SQLiteCommand(checkIdQuery, connection))
        {
            checkCommand.Parameters.AddWithValue("@id", idToDelete);

            int count = Convert.ToInt32(checkCommand.ExecuteScalar());  // Returns the count of rows

            if (count > 0) // If count > 0, the ID exists
            {
                Console.WriteLine($"Record with ID {idToDelete} exists. Proceeding to delete...");
                // Proceed with the delete operation
                using (SQLiteCommand deleteCommand = new SQLiteCommand(deleteHabit, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@id", idToDelete);

                    int rowsAffected = deleteCommand.ExecuteNonQuery(); // Execute delete command

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Record with ID {idToDelete} deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Delete operation failed.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Habit with ID {idToDelete} does not exist.");
            }
        }
    }
}

string getId()
{
    string id = "";
    bool isNumber;

    do
    {
        Console.WriteLine("Enter an ID: ");
        id = Console.ReadLine();
        isNumber = int.TryParse(id, out _) || double.TryParse(id, out _);
        if (!isNumber)
        {
            Console.WriteLine("Please enter a number for ID");
        }
    } while (!isNumber);

    return id;
}

string getHabit()
{
    string habit = "";
    bool isNumber;

    do
    {
        Console.WriteLine("Please enter a habit (ex. drink more water): ");
        habit = Console.ReadLine();
        isNumber = int.TryParse(habit, out _) || double.TryParse(habit, out _);
        if (isNumber)
        {
            Console.WriteLine("Please enter a string for a habit");
        }
    } while (isNumber);

    return habit;
}

string getQuantity()
{
    string quantity = "";
    bool isNumber;

    do
    {
        Console.WriteLine("How would you quantify this habit? (ex. number of water glasses a day): ");
        quantity = Console.ReadLine();
        isNumber = int.TryParse(quantity, out _) || double.TryParse(quantity, out _);
        if (isNumber)
        {
            Console.WriteLine("Please enter a string for a quantity");
        }
    } while (isNumber);

    return quantity;
}

string getDate()
{
    string date = "";
    bool invalidDate;

    do
    {
        Console.WriteLine("Please enter the date of occurance for this habit (mm/dd/yyyy). Enter 'x' to to use today's date.");
        date = Console.ReadLine();
        if (date == "x")
        {
            date = DateTime.Now.ToString("d");
            invalidDate = false;
        }
        else
        {
            invalidDate = !Regex.IsMatch(date, dateRegex);
            if (invalidDate)
            {
                Console.WriteLine("Invalid date");
            }

        }
    } while (invalidDate);

    return date;
}