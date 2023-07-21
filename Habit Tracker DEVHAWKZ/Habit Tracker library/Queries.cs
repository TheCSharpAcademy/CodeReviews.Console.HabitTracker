using Habit_Tracker_library.Model;
using Microsoft.Data.Sqlite;
using System.Globalization;
using static Habit_Tracker_library.Crud;

namespace Habit_Tracker_library;

internal class Queries
{
    internal static void CreateTableQuery()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open(); // povezivanje sa bazom
            var tableCmd = connection.CreateCommand(); // kreira komandu

            tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS habits 
                                    (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Quantity INTEGER
                                    )"; // tekst komande (Select, Create ...)

            tableCmd.ExecuteNonQuery(); // izvrsavanje te komande (ExecuteNonQuery() znaci da ne zelimo da baza podataka vrati bilo koju vrednost, koristimo je jer zelimo samo da kreira tabelu za nas ili da doda ili obrise red ili nesto drugo iz nje

            connection.Close(); // zatvaranje konekcije sa bazom
        }
    }

    internal static void InsertRecordQuery(string date, int quantity)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO habits(date,quantity) VALUES ('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        Console.WriteLine("Record inserted successfully!\n\nPress any key to get back to Main Menu");
        Console.ReadKey();
    }

    internal static int ViewAllRecordsQuery()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @$"SELECT * FROM 'habits'";

            List<HabitInfo> tableData = new(); // lista u kojoj ce da se nalazi svi objekti (redovi) iz tabele 
            SqliteDataReader reader = tableCmd.ExecuteReader(); // cita podatke iz baze i vraca datareader objekat

            if (reader.HasRows) // proverava da nije tabela prazna
            {
                while (reader.Read()) // pristupa jednom po jednom redu iz tabele
                {
                    tableData.Add
                    (
                        new HabitInfo
                        {
                            ID = reader.GetInt32(0), // 0 kolona u datom redu
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")), // datum u formatu dd-MM-yyyy i americki nacin prikaza (1. idu meseci) 
                            Quantity = reader.GetInt32(2) // 2. kolona u datom redu
                        }
                    );
                }

                Console.WriteLine("-----------------------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.ID} - {dw.Date} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("-----------------------------------------\n");

            }

            else
            {
                Console.WriteLine("-----------------------------------------\n");
                Console.WriteLine("No records found!");
                Console.WriteLine("-----------------------------------------\n");
            }

            connection.Close();



            return tableData.Count;
        }
    }

    internal static void DeleteRecordQuery(int recordID)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM 'habits' WHERE Id = {recordID}";

            int rowCount = tableCmd.ExecuteNonQuery(); // vraca broj kreiranih, updejtovanih ili obrisanih redova

            if (rowCount == 0) // ako je jednak nuli znaci da nijedan red iz baze nije obrisan
            {
                Console.WriteLine($"\nRecord with ID {recordID} doesn't exist.\\n\\nPress any key to continue...");
                Console.ReadKey();
                connection.Close();
                DeleteRecord();
            }

            connection.Close();
        }

        Console.WriteLine("Record deleted successfully!\n\nPress any key to get back to Main Menu");
        Console.ReadKey();
    }

    internal static void UpdateRecordQuery(int recordId)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            
            int checkQuery = Helpers.PossibleUpdate(recordId); // proverava da li postoji red sa tim id-em

            // provera da li postoji red sa datim ID-em
            if (checkQuery == 0)
            {
                Console.WriteLine($"\nRecord with ID {recordId} doesn't exists.\n\nPress any key to continue.");
                Console.ReadKey();
                connection.Close();
                UpdateRecord();
            }

            string date = Helpers.GetDateInput();
            int quantity = Helpers.GetNumberInput($"\nPlease enter quantity in {Measure} (no decimals allowed)");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE habits SET Date = '{date}', Quantity = {quantity} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        Console.WriteLine("Record updated successfully!\n\nPress any key to get back to Main Menu");
        Console.ReadKey();
    }

    internal static void YearlyReportQuery(string year)
    {
        using (var connection = new SqliteConnection(Crud.ConnectionString))
        {
            connection.Open();
            var tableCheck = connection.CreateCommand();


            //proverava da li u tabeli postoji bar 1 red sa datom godinom
            tableCheck.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Date Like '%{year}')";

            // vraca 1. kolonu 1. reda dobijenog iz prethodnog upita, ako takav red ne postoji vraca null
            int checkQuery = Convert.ToInt32(tableCheck.ExecuteScalar());

            // provera da li postoji red sa datom godinom
            if (checkQuery == 0)
            {
                Console.WriteLine($"\nNo record with year {year} exists.\n\nPress any key to continue.");
                Console.ReadKey();
                connection.Close();
                RecordReport.YearlyReport();
            }

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT SUM(Quantity) FROM habits WHERE Date Like '%{year}' ";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.Read())
                
                Console.WriteLine($"\nYou have done habits in {year} for {reader.GetInt32(0)} {Crud.Measure}");

            connection.Close();
        }
        Console.WriteLine("\n\nPress any key to get back to main menu...");
        Console.ReadKey();
    }

    internal static void MonthlyReportQuery(string month)
    {
        using (var connection = new SqliteConnection(Crud.ConnectionString))
        {
            connection.Open();
            var tableCheck = connection.CreateCommand();


            //proverava da li u tabeli postoji bar 1 red sa datom godinom
            tableCheck.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Date Like '%{month}%')";

            // vraca 1. kolonu 1. reda dobijenog iz prethodnog upita, ako takav red ne postoji vraca null
            int checkQuery = Convert.ToInt32(tableCheck.ExecuteScalar());

            // provera da li postoji red sa datom godinom
            if (checkQuery == 0)
            {
                Console.WriteLine($"\nNo record with month {month} exists.\n\nPress any key to continue.");
                Console.ReadKey();
                connection.Close();
                RecordReport.MonthlyReport();
            }

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT SUM(Quantity) FROM habits WHERE Date Like '%{month}%' ";

            SqliteDataReader reader = tableCmd.ExecuteReader();


            if (reader.Read())
            {
                int sum = reader.GetInt32(0);
                string mesec = Helpers.GetMonthName(month);
                
                Console.WriteLine($"\nIn {mesec} you have done total of {sum} {Crud.Measure} {Crud.Habit}");
            }

            else Console.WriteLine("\nNo rows with specified year has been found!");

            connection.Close();
        }
        Console.WriteLine("\n\nPress any key to get back to main menu...");
        Console.ReadKey();
    }
}
 

