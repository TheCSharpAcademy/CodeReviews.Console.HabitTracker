using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabbitLogger;

class HabbitLogger
{
    static string connectionString = @"Data Source=HabbitLogger.db";

    static void Main(string[] arg)
    {
        bool runHabbitLogger = true;
        string selectedHabbit = "Coffee_Cups";
        string? mainMenuSelection;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS Coffee_Cups (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Date STRING,
                Measure INTEGER
            )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        Console.WriteLine("Welcome to the Habbit Logger\n");

        while(runHabbitLogger)
        {
            Console.WriteLine($"Currently you have selected the {selectedHabbit} habbit\n");

            Console.WriteLine("Please select one of the following options:\n" +
            "1) Insert a new log\n"+
            "2) Delete a previous log\n"+
            "3) Update a previous log\n"+
            "4) View the recorded logs\n"+
            "5) Switch to another habbit\n"+
            "6) Create a new habbit log\n"+
            "7) Show habbit statistics\n"+
            "0) Exit the application\n");

            mainMenuSelection = Console.ReadLine();

            switch(mainMenuSelection)
            {
                case "0":
                    runHabbitLogger = false;
                    break;

                case "1":
                    InsertHabbit(selectedHabbit);
                    break;

                case "2":
                    if (!IsDBEmpty(selectedHabbit))
                    {
                        DeleteHabbitRecord(selectedHabbit);
                    }
                    else 
                    {
                        Console.Clear();
                        Console.WriteLine("The selected habbit does not have any records.\n");
                    }
                    break;

                case "3":
                    if (!IsDBEmpty(selectedHabbit))
                    {
                        UpdateHabbit(selectedHabbit);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("The selected habbit does not have any records.\n");
                    }
                    break;

                case "4":
                    if (!IsDBEmpty(selectedHabbit))
                    {
                        ViewLogs(selectedHabbit);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("The selected habbit does not have any records.\n");
                    }
                    break;

                case "5":
                    selectedHabbit = GetHabbits();
                    break;

                case "6":
                    CreateTable();
                    break;

                case "7":
                    if (!IsDBEmpty(selectedHabbit))
                    {
                        DisplayStatistics(selectedHabbit);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("The selected habbit does not have any records.\n");
                    }
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Please select a valid option\n");
                    break;                
            }
        }
    }

    static string GetHabbits()
    {
        Console.Clear();
        List<String> tableNames = new();
        string selection;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"SELECT name
                FROM sqlite_schema
                WHERE type = 'table' AND
                name NOT LIKE 'sqlite_%'";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            while(reader.Read())
            {
                tableNames.Add(reader.GetString(0));
            }

            connection.Close();
        }

        Console.WriteLine("Please write the habbit name you want to select. The following habbits are stored in the system:\n");
        foreach (String tableName in tableNames)
        {
            Console.WriteLine(tableName);
        }

        selection = Console.ReadLine();

        while (!tableNames.Contains(selection))
        {
            Console.WriteLine("The habbit name you have written is not stored in the system");
            selection = Console.ReadLine();
        }
        return selection;
    }

    static void ViewLogs(string tableName)
    {
        Console.Clear();
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"SELECT * FROM {tableName}
                ORDER BY ID ASC";
            
            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while(reader.Read())
                {   
                    Console.WriteLine($"ID: {reader.GetInt32(0) }  " +
                    $"Date: {reader.GetString(1) }  " +
                    $"Quantity: {reader.GetInt32(2)}\n");
                }
            }
            connection.Close();
        }
    }

    static void CreateTable()
    {   
        bool invalidName = true;
        string? habbitMeasure = "";
        string? habbitCategory = "";
        while(invalidName)
        {
            Console.Clear();
            Console.WriteLine("Please write the unit of measure for the new habbit (e.g., cups, steps, pages)");
            habbitMeasure = Console.ReadLine();

            Console.WriteLine("Please write the name of the habbit you want to measure (e.g., coffee, water):");
            habbitCategory = Console.ReadLine();

            Console.WriteLine($"The name of the new habbit will be: {habbitCategory}_{habbitMeasure}");
            Console.WriteLine("Is the name okay y/n?");
            if(Console.ReadLine() == "y")
            {
                invalidName=false;
            }
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"CREATE TABLE IF NOT EXISTS {habbitCategory}_{habbitMeasure} (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Date STRING,
                Measure INTEGER
            )";
            
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }   
    }

    static void InsertHabbit(string selectedHabbit)
    {
        string[] data = new string[2];
        data = DataValidation();
        int dataInt;
        dataInt = Convert.ToInt32(data[1]);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"INSERT INTO {selectedHabbit} (Date, Measure)
                VALUES ('{data[0]}',{dataInt});";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    static string[] DataValidation()
    {
        string[] data = new string[2];
        bool insertedHabbitNotValid = true;
        string? habbitDate = "";
        string? habbitMeasure = "";
        DateOnly habbitDateValidated = new();
        Console.Clear();
        while (insertedHabbitNotValid)
        {

            Console.WriteLine("Please write the date of the habbit in the format yyyy/MM/dd:");
            habbitDate = Console.ReadLine();
            Console.WriteLine("Please write an integer quantity of the measure:");
            habbitMeasure = Console.ReadLine();
            if (DateOnly.TryParseExact(habbitDate,"yyyy/MM/dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out habbitDateValidated)
            && int.TryParse(habbitMeasure,out int habbitMeasureInt) && habbitMeasureInt > 0)
            {
                data[0] = habbitDate;
                data[1] = habbitMeasure;
                insertedHabbitNotValid = false;
                Console.Clear();
                Console.WriteLine("Record added succesfully!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("The data you entered is incorrect. Please try again.\n");
            }
        }
        return data;
    }

    static void UpdateHabbit(string selectedHabbit)
    {
        string[] data;
        int dataInt;
        int rowsUpdated;

        int IDInt = SelectHabbitID(selectedHabbit,"update");
        data = DataValidation();
        dataInt = Convert.ToInt32(data[1]);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"UPDATE {selectedHabbit} 
                SET
                Date = '{data[0]}',
                Measure = {dataInt}
                WHERE
                ID = {IDInt}";

            rowsUpdated = tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        if (rowsUpdated == 0)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"INSERT INTO {selectedHabbit} (ID, Date, Measure)
                VALUES ({IDInt}, '{data[0]}',{dataInt});";

            tableCmd.ExecuteNonQuery();
            connection.Close();
            }
        }
    }

    static void DeleteHabbitRecord(string selectedHabbit)
    {
        Console.Clear();    
    
        int IDInt = SelectHabbitID(selectedHabbit,"delete");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @$"DELETE FROM {selectedHabbit}
                WHERE ID = {IDInt}";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }    
        Console.Clear();
        Console.WriteLine("Record succesfully deleted!\n");        
    }

    static int SelectHabbitID(string selectedHabbit, string command)
    {
        bool IDNotValid = true;
        string? ID;
        int IDInt = 0;
        int IDRows;

        Console.Clear();
        ViewLogs(selectedHabbit);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"SELECT seq
                FROM sqlite_sequence
                WHERE name = '{selectedHabbit}'";
            
            SqliteDataReader reader = tableCmd.ExecuteReader();
            reader.Read();
            IDRows = reader.GetInt32(0);
            connection.Close();
        }    

        Console.WriteLine($"Please write the record ID you want to {command}:");

        while(IDNotValid)
        {
            ID = Console.ReadLine();
            int.TryParse(ID,out IDInt);
            if(0<IDInt && IDInt<=IDRows)
            {
                IDNotValid = false;
            }
            else
            {
                Console.WriteLine("Please write a valid ID record");
            }
        }
        return IDInt;
    }

    static void DisplayStatistics(string selectedHabbit)
    {
        Console.Clear();
        string[] maxMeasure = GetMinMaxMeasure(selectedHabbit,"MAX");
        string[] minMeasure = GetMinMaxMeasure(selectedHabbit,"MIN"); 
        string dailyAverage = DailyAverage(selectedHabbit);   
        Console.WriteLine($"Your maximum value is {maxMeasure[1]} on {maxMeasure[0]}");
        Console.WriteLine($"Your minimum value is {minMeasure[1]} on {minMeasure[0]}");
        Console.WriteLine($"Your daily average is {dailyAverage}\n");
    }

    static string[] GetMinMaxMeasure(string selectedHabbit,string Option)
    {
        string[] Measure = new string[2];
       
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"SELECT Date, Measure
                FROM {selectedHabbit}
                WHERE Measure = (SELECT {Option}(Measure) FROM {selectedHabbit})";
            
            SqliteDataReader reader = tableCmd.ExecuteReader();
            
            reader.Read();
            Measure[0] = reader.GetString(0);
            Measure[1] = Convert.ToString(reader.GetInt32(1));
            connection.Close();
        }
        return Measure;
    }

    static string DailyAverage(string selectedHabbit)
    {
        string average;
       
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"SELECT AVG(Total_Measures)
                FROM (SELECT Date, SUM (Measure) as Total_Measures
                FROM {selectedHabbit}
                GROUP BY Date)";
            
            SqliteDataReader reader = tableCmd.ExecuteReader();
            reader.Read();   
            average = Convert.ToString(reader.GetFloat(0));
            connection.Close();
            
        }
        return average;
    }

    static bool IsDBEmpty(string selectedHabbit)
    {
        bool isDBEmpty = true;
        int DBRows;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"SELECT EXISTS (SELECT 1 FROM {selectedHabbit})";
            
            SqliteDataReader reader = tableCmd.ExecuteReader();

            reader.Read();
            DBRows = reader.GetInt32(0);
            if (DBRows != 0)
            {
                isDBEmpty = false;
            }
            connection.Close();
        }
        return isDBEmpty;
    }
}