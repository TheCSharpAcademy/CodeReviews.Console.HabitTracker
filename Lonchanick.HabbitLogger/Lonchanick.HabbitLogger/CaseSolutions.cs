using Spectre.Console;
using System;

namespace Lonchanick.HabbitLogger;

internal class CaseSolutions
{
    public static void ReadAllRecords()
    {
        var aux = db.select();

        if (aux is not null)
        {
            printHabitTable(aux);
            //foreach (var i in aux)
            //    Console.WriteLine($"{i.Id} - {i.DateField.ToString("yyyy-MM-dd")} - {i.Quantity}");
        }
        else
            Console.WriteLine("There is no records yet!");
    }

    public static void AddNewRecord()
    {
        var date = GetValidDateTime("Date (yyyy-MM-dddd)");

        var quantity = GetValidInteger("Quantity");


        bool response = db.Insert(new Habit {DateField=date, Quantity=quantity });
        
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

        bool response = db.Update(new Habit { Id= id, DateField = date, Quantity = quantity });

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

        bool response = db.Delete(id);

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

    static void printHabitTable(IEnumerable<Habit> h)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Date");
        table.AddColumn("Quantity");

        foreach (var p in h)
        {
            table.AddRow(
                p.Id.ToString(),
                p.DateField.ToString("yyyy-MM-dd"),
                p.Quantity.ToString()
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
        string aux = string.Empty;
        DateTime result;

        do
        {
            Console.Write($"{param}: ");
            aux = Console.ReadLine();
        } while (!DateTime.TryParse(aux, out result));

        return result;
    }
}


