using System.Text.RegularExpressions;
using HabitTracker;
using Microsoft.Data.Sqlite;
using SQLitePCL;

var crud = new CRUD();

raw.SetProvider(new SQLite3Provider_e_sqlite3());
var dbString = "habit-Tracker.db";
var ex = crud.DbExistence(dbString);
var connectionString = @$"Data Source={dbString}";
using var connection = new SqliteConnection(connectionString);
connection.Open();
var end = true;

if (!ex)
{
    // seed db data if it was created for the first time
    var test = ("test1", "test2", "test3");
    crud.Create(connection, test.Item1);
    crud.Create(connection, test.Item2);
    crud.Create(connection, test.Item3);
    var random = new Random();
    for (var i = 0; i <= 100; i++)
    {
        crud.Update(connection, test.Item1, RngRegex(), random.Next(0, 100), true);
        crud.Update(connection, test.Item2, RngRegex(), random.Next(0, 100), true);
        crud.Update(connection, test.Item3, RngRegex(), random.Next(0, 100), true);
    }
}


while (end)
{
    // user menu
    Console.Write(@"Welcome to the Habbit Tracker application.
    Available options:
    1. Add new habit.
    2. View your habit progress.
    3. Delete habit.
    4. Add or Delete data to/from your habit.
    5. Habit report.
    6. Exit.
    
    Choose your option: ");
    
    var option = Console.ReadLine();
    int optionInt;
    while (!int.TryParse(option, out optionInt))
    {
        Console.Write("Incorrect option, try again: ");
        option = Console.ReadLine();
    }
    switch (optionInt)
    {
        case 1:
            Console.Write("Write name of your habit: ");
            var nameCreate = Console.ReadLine();
            if (crud.Create(connection, nameCreate)) Console.WriteLine("Created successfully");
            break;
        case 2:
            Console.Write("Write name of your current habit: ");
            var nameRead = Console.ReadLine();
            try
            {
                var x = crud.Read(connection, nameRead);
                if (!x) Console.WriteLine("No data yet.");
            }
            catch (SqliteException)
            {
                Console.WriteLine("Name was not found");
            }

            break;
        case 3:
            Console.Write("Write name of your habit you want to delete: ");
            var nameDelete = Console.ReadLine();
            try
            {
                if (crud.Delete(connection, nameDelete)) Console.WriteLine("Deleted successfully");
            }
            catch (SqliteException)
            {
                Console.WriteLine("Name was not found");
            }

            break;
        case 4:
            Console.WriteLine("Add or Delete? Type a - to add, d - to delete: ");
            var check = Console.ReadLine();
            while (!Regex.IsMatch(check, "^a$|^d$"))
            {
                Console.WriteLine("Wrong data format, Type a - to add, d - to delete:");
                check = Console.ReadLine();
            }

            Console.Write("Write name of your habit: ");
            var nameUpdate = Console.ReadLine();
            Console.Write("Write date of your habit OR type T for today's date: ");
            // check if user inputs correct date
            var regex = new Regex(@"^([0-2][0-9]|3[01])\.(0[1-9]|1[0-2])\.(\d{4})$|^T$");
            var dateUpdate = Console.ReadLine();
            while (!regex.IsMatch(dateUpdate))
            {
                Console.WriteLine("Wrong data format, try again. Example: 01.01.2001 or T for today's date.");
                Console.Write("Write date of your habit: ");
                dateUpdate = Console.ReadLine();
            }
            if (dateUpdate == "T") dateUpdate = DateTime.Today.ToString("d");
            if (check == "d")
            {
                crud.Update(connection, nameUpdate, dateUpdate, null, false);
                break;
            }

            Console.Write("Write repetition of your habit that day: ");
            var repetitionUpdateReadLine = Console.ReadLine();
            int repetitionUpdate;
            while (!int.TryParse(repetitionUpdateReadLine, out repetitionUpdate))
            {
                Console.WriteLine("Wrong repetition number, try again.");
                Console.Write("Write repetition of your habit that day: ");
                repetitionUpdateReadLine = Console.ReadLine();
            }


            if (crud.Update(connection, nameUpdate, dateUpdate, repetitionUpdate, true))
                Console.WriteLine("Created successfully");
            break;
        case 5:
            Console.Write("Write name of your habit: ");
            var nameReport = Console.ReadLine();
            
            Console.Write("Write date of interested data: ");
            var year = Console.ReadLine();
            var regexReport = new Regex(@"^(\d{4})$|^T$");
            while (!regexReport.IsMatch(year))
            {
                Console.WriteLine("Wrong data format, try again. Example: 2001 or T for today's date.");
                Console.Write("Write date of your habit: ");
                year = Console.ReadLine();
            }
            if (year == "T") year = DateTime.Today.ToString("yyyy");
            try
            {
                crud.Report(connection, nameReport, year);
            }
            catch (SqliteException)
            {
                Console.WriteLine("Name was not found");
            }
            break;
        case 6:
            end = false;
            break;
        default:
            Console.WriteLine("Wrong choice selection, try again: \n");
            break;
    }

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
    Console.Clear();
}

connection.Close();

string RngRegex()
{
    // method to create data for seed
    var rng = new Random();
    var date = $"{rng.Next(0, 3)}{rng.Next(0, 10)}.0{rng.Next(0, 10)}.{rng.Next(2, 3)}{0}{rng.Next(0, 3)}{rng.Next(0, 5)}";
    return date;
}