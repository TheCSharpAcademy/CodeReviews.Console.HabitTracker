using Spectre.Console;
using System;

namespace Lonchanick.HabbitLogger;

internal class CaseSolutions
{
    public static void ReadAllRecords()
    {
        var aux = Db.Select();

        if (aux is not null)
            PrintHabitTable(aux);
        else
            Console.WriteLine("There is no records yet!");
    }

    public static void AddNewRecord()
    {
        var date = GetValidDateTime("Date (yyyy-MM-dddd)");
        var quantity = GetValidInteger("Quantity");

        bool response = Db.Insert(new Habit {DateField=date, Quantity=quantity });
        
        if(response)
        {
            Console.WriteLine("Succefull Insert.");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("There was a problem with the request.");
            Console.ReadLine();
        }
    }

    public static void Update()
    {
        var id = GetValidInteger("Type object \"Id\" to Update");

        var date = GetValidDateTime("Date (yyyy-MM-dddd)");

        var quantity = GetValidInteger("Quantity");

        bool response = Db.Update(new Habit { Id= id, DateField = date, Quantity = quantity });

        if (response)
        {
            Console.WriteLine("Succefull Update.");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("There was a problem with the request.");
            Console.ReadLine();

        }
    }

    public static void Delete()
    {
        var id = GetValidInteger("Type object \"Id\" to Delete");

        bool response = Db.Delete(id);

        if (response)
        {
            Console.WriteLine("Object Succefull Deleted.");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("There was a problem with the request.");
            Console.ReadLine();

        }
    }

    static void PrintHabitTable(IEnumerable<Habit> habit)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Date");
        table.AddColumn("Quantity");

        foreach (var h in habit)
        {
            table.AddRow(
                h.Id.ToString(),
                h.DateField.ToString("yyyy-MM-dd"),
                h.Quantity.ToString()
                );
        }

        AnsiConsole.Write(table);
        Console.Write("Press any Key to continue");
        Console.ReadLine();
    }

    static int GetValidInteger(string param)
    {
        string aux = string.Empty;
        int result;

        while (!int.TryParse(aux, out result))
        {
            Console.Write($"{param}: ");
            aux = Console.ReadLine();
        }

        return result;
    }

    static DateTime GetValidDateTime(string param)
    {
        string userInput = string.Empty;
        DateTime result;

        do
        {
            Console.Write($"{param}: ");
            userInput = Console.ReadLine();
        } while (!DateTime.TryParse(userInput, out result));

        return result;
    }
}


