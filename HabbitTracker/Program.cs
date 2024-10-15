// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using HabitTracker;
using Microsoft.Data.Sqlite;
using SQLitePCL;

CRUD crud = new CRUD();

raw.SetProvider(new SQLite3Provider_e_sqlite3());
string connectionString = @"Data Source=new-habit-Tracker.db";
using var connection = new SqliteConnection(connectionString);
connection.Open();
bool end = true;

while(end){
Console.Write(@"Welcome to the Habbit Tracker application.
    Available options:
    1. Add new habit.
    2. View your habit progress.
    3. Delete habit.
    4. Add new data to your habit
    5. Exit
    
    Choose your option: ");
var option = Convert.ToInt32(Console.ReadLine());
switch (option)
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
           if(!x) Console.WriteLine("No data yet.");
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
        Console.Write("Write name of your habit: ");
        var nameUpdate = Console.ReadLine();
        Console.Write("Write date of your habit OR type T for today's date: ");
        Regex regex = new Regex(@"^([0-2][0-9]|3[01])\.(0[1-9]|1[0-2])\.(\d{4})$|^T$");
        var dateUpdate = Console.ReadLine();
        
            while (!regex.IsMatch(dateUpdate))
        {
            Console.WriteLine("Wrong data format, try again. Example: 01.01.2001");
            Console.Write("Write date of your habit: "); dateUpdate = Console.ReadLine();
        }
        if(dateUpdate == "T"){
            DateTime thisTime = DateTime.Today;
            dateUpdate = thisTime.ToString("d");
        }
        Console.Write("Write repetition of your habit that day: "); 

        var repetitionUpdateReadLine = Console.ReadLine();
        int repetitionUpdate;
        while (!int.TryParse(repetitionUpdateReadLine, out repetitionUpdate))
        {
            Console.WriteLine("Wrong repetition number, try again.");
            Console.Write("Write date of your habit: "); repetitionUpdateReadLine = Console.ReadLine();
        }
        if(crud.Update(connection, nameUpdate, dateUpdate, repetitionUpdate)) Console.WriteLine("Created successfully");
        break;
    case 5:
        end = false;
        break;
    default:
        Console.WriteLine("Wrong choice selection, try again: \n");
        break;
} 
}
//crud.Create(connection);
//crud.Read(connection);
connection.Close();