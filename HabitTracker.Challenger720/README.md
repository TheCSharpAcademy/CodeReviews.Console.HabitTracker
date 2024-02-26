# Console App: Habit Logger

[
    "# Head",
    "## Item",
    "### Detail"
]

Console based CRUD application to track time studying each day,
using C# and SQLite.

This is my 3rd beginner project at C# Academy.

Guidance by Pable from C# Academy is provided in his Youtube Tutorial,
but I try my best to build this console app by myself through
reading Microsoft documentations.

My aim is to keep my codes simple,
so I can follow the logic when I make reference of my own code later.

## Features

- ADO.NET, because it’s the closest to raw SQL (Microsoft.Data.SQLite NuGet Package)
- This app stores and retrive study hours by date from a SQL database
- User navigates the console-based UI through keyboard
- User can insert, delete, update and view the logged study hours
- User input is checked for validity (e.g. date format, hours are whole numbers only)

## Usage/Examples

```javascript
static void Main(string[] args)
{
    bool end = false;
    using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();

        command.CommandText =
            @"CREATE TABLE IF NOT EXISTS StudyHours 
                (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
                ,Date DATE
                ,Quantity INTEGER                        
                )";

        command.ExecuteNonQuery();

        connection.Close();
    }

    userInput(end);
}
```

- When the application starts, a SQLite database is created if one isn’t present.
- It creates a table in the database to log the study hours
- It brings the user into main menu where data manipulation happens.

```javascript
static void userInput(bool end)
{
    Console.Clear();
    string? input;
    while (!end)
    {
        Console.WriteLine("MAIN MENU\n");
        Console.WriteLine("What would you like to do?\n");
        Console.WriteLine("Type 0 to Close Application");
        Console.WriteLine("Type 1 to View All Records");
        Console.WriteLine("Type 2 to Insert Record");
        Console.WriteLine("Type 3 to Delete Record");
        Console.WriteLine("Type 4 to Update Record");
        Console.WriteLine("------------------------------------------\n");

        input = Console.ReadLine();

        switch (input)
        {
            case "0":
                end = true;
                Environment.Exit(0);
                break;
            case "1":
                viewRecord();
                break;
            case "2":
                insert();
                break;
            case "3":
                delete();
                break;
            case "4":
                update();
                break;
            default:
                Console.WriteLine("\nInvalid command. Please type number 0 to 4.\n");
                break;
        }
    }

}
```

- User is presented a main menu and asked to type in a number to do a list of actions.
- User can choose to insert, delete, update study hour, view record or quit app.
- If user enters an invalid input, they are ask to enter a valid number again.

```javascript
static void viewRecord()
{
    Console.Clear();

    using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();

        command.CommandText =
            "SELECT * FROM StudyHours";

        List<string> table = new List<string>();

        SqliteDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                table.Add($"{reader.GetString(0)} - 
                Date: {reader.GetString(1)} - 
                Hours: {reader.GetString(2)}");
            }
        }
        else
        {
            Console.WriteLine("No rows found.");
        }

        connection.Close();

        foreach (var row in table)
        {
            Console.WriteLine(row);
        }

        Console.WriteLine();

    }
}
```

- When user requests to view the record, all the data in SQL database are
'read' and transferred into a List in C#.

- If no rows are 'read', the user is notified no rows found
and brings user back to main menu.
- The List is then printed onto the console by iteratiing through all its rows.


```javascript
static void insert()
{
    Console.Clear();
    string date = getDate();
    int quantity = getNumber("Please insert number of hours in whole number, 
    or type 0 to go back to main menu.");

    using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();

        command.CommandText = $"INSERT INTO StudyHours (Date, Quantity) Values('{date}','{quantity}')";

        command.ExecuteNonQuery();

        connection.Close();
    }
}

static string getDate()
{
    string? date;
    bool result;
    do
    {
        Console.WriteLine("Please insert the date in format of YYYY-MM-DD. 
        Type 0 to return to main menu.");
        date = Console.ReadLine();
        if (date == "0") userInput(false);
        result = DateTime.TryParseExact(date, "yyyy-MM-dd", 
        new CultureInfo("en-US"), DateTimeStyles.None, out DateTime product);

    } while ((date == null) || (!result));

    return date;
}

static int getNumber(string message)
{
    int num;
    string? numberInput;

    do
    {
        Console.WriteLine(message);
        numberInput = Console.ReadLine();
        Console.WriteLine();

    } while (!int.TryParse(numberInput, out num));

    if (numberInput == "0") userInput(false);

    return num;
}
```

- To insert the study hours onto the table in SQL database,
user is required to input a valid date and study hour in whole number.

- getData method checks if the user input a valid date by DateTime's
TryParseExact method (refer to Microsoft Documentation for usage).
If the date is invalid or empty, user is prompt to enter again.

- getNumber method checks if the user input a valid number for
study hour, it should be a whole number.

- The user can return to the main menu by entering "0" at any point.

```javascript
static void update()
{
    Console.Clear();
    viewRecord();

    var recordID = getNumber("Please type the ID of the record 
    you want to update, or type 0 to go back to main menu.");

    using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();

        command.CommandText = $"SELECT * FROM StudyHours WHERE ID = '{recordID}'";

        var rows = command.ExecuteScalar();

        if (rows == null)
        {
            Console.WriteLine($"\nRecord ID {recordID} 
            doesn't exist. Please any key to try again! \n");
            Console.ReadLine();
            update();
        }
        else
        {
            string date = getDate();
            int quantity = getNumber("Please insert number of hours 
            in whole number, or type 0 to go back to main menu.");

            command.CommandText = $"UPDATE StudyHours 
            SET Date='{date}',Quantity='{quantity}' WHERE ID = '{recordID}'";

            command.ExecuteNonQuery();

            Console.WriteLine($"\nRecord ID {recordID} is updated. \n");

            connection.Close();
        }
    }
}
```

- User can update the existing record by entering the ID of record
as shown on the screen (presented through viewRecord method).

- If user input a invalid ID, the user is prompted to try again
or go back to main menu.

- If the ID exists, the user will be asked to enter date and study hour,
which are vetted by the same validity method as mentioned above in "insert" menu.

```javascript
static void delete()
{
    Console.Clear();
    viewRecord();

    var recordID = getNumber("Please type the ID of the 
    record you want to delete, or type 0 to go back to main menu.");

    using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = $"DELETE FROM StudyHours where ID = '{recordID}'";

        int rowCount = command.ExecuteNonQuery();

        if (rowCount == 0)
        {
            Console.WriteLine($"\nRecord ID {recordID} doesn't exist. 
            Please any key to try again! \n");
            Console.ReadLine();
            delete();
        }
        else
        {
            Console.WriteLine($"\nRecord ID {recordID} is deleted. \n");
        }

        connection.Close();
    }
}
```

- User can delete the existing record by entering the ID of record
as shown on the screen (presented through viewRecord method).

- If user input a invalid ID, the user is prompted to try again
or go back to main menu.

- If the ID is valid, the record will be deleted and the user is
notified the record is deleted.

## Documentation

[Pablo's Youtube Tutorial](https://www.youtube.com/watch?v=d1JIJdDVFjs)

[Microsoft Documentation: Creating SQLite Connection](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)

[Microsoft Documentation: SQL Query to Console](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand?view=dotnet-plat-ext-8.0)

[C# and SQLite – Updating Data](https://stuartsplace.com/information-technology/programming/c-sharp/c-sharp-and-sqlite-updating-data)

[Microsoft Documentation: DateTime Format](https://learn.microsoft.com/en-us/dotnet/api/system.datetime?view=net-8.0)

## Lessons Learned

- How to use SQLite to create a simple CRUD app in C#
- Read Microsoft Documentation when in doubt. I already know SQL
before this project, but the learning curve is still steep,
because I do not understand how to incorporate it
 into C# and I was panicking when I first watch the youtube tutorial.
 Fortunately, I found the Microsoft Documentation helps a lot

- Make use of debugging with breakpoints
- Refactor the codes and keep it clean
- Take a rest when I can't process. Programming requires a very active brain
