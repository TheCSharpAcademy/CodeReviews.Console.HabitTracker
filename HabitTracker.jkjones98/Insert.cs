using Microsoft.Data.Sqlite;
using UserInput;
using NumberInput;
using DateInput;

namespace Insert;
public class InsertRecord
{
    UserDateInput dateInput = new UserDateInput();
    GetNumberInput getNum = new GetNumberInput();
    ClassUserInput rtnToMainMenu = new ClassUserInput();
    public void InsertMethod()
    {
        string connectionString = @"Data Source=habit-Tracker2.db";

        string date = dateInput.GetDate("\nEnter the date using the following format DD-MM-YY. Enter 0 to return to the main menu.\n");
        int quantity = getNum.GetNumInput("\nEnter the number of litres you have drank today, no decimals. Enter 0 to return to the main menu\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var insertCommand = connection.CreateCommand();

            insertCommand.CommandText = $"INSERT INTO drinking_water(Date, Quantity) VALUES({date}, {quantity})";

            // Don't return any values, not querying any values
            insertCommand.ExecuteNonQuery();

            connection.Close();
        }
    }
}