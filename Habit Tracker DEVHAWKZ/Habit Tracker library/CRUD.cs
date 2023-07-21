using static Habit_Tracker_library.Helpers;
using static Habit_Tracker_library.Queries;

namespace Habit_Tracker_library
{
    internal static class Crud
    {
        private static string _connectionString = @"Data Source=habit-tracker.db";
        //private static string _habit;
        private static string _measure;

        internal static string ConnectionString
        {
            get { return _connectionString; }
            private set { _connectionString = value; }
        }

       /* internal static string Habit {
            get { return _habit; }
            set { _habit = value; }
        }
       */
        internal static string Measure
        {
            get { return _measure; }
            set { _measure = value; }
        }

        internal static void CreateTable() => CreateTableQuery();

        internal static int ViewAllRecords()
        {
            Console.Clear();
            Console.WriteLine("View All Records\n");

            int rows = ViewAllRecordsQuery();

            return rows;
        }

        internal static void InsertRecord()
        {
            Console.Clear();
            Console.WriteLine("Insert Record\n");

            string date = GetDateInput("Type 0 to return back to Main Menu");
            int quantity = GetNumberInput($"\nPlease enter quantity in {Measure} (no decimals allowed)\nType 0 to return to Main Menu");

            if (date != "0" && quantity != 0) InsertRecordQuery(date, quantity);
            
            else Menu.MainMenu();    
        }

        internal static void DeleteRecord()
        {
            Console.Clear();
            Console.WriteLine("Delete Record");
            
            int rows =  ViewAllRecords();

            if(rows == 0)
            {
                Console.WriteLine("\n\nPress any key to insert records...");
                Console.ReadKey();
                InsertRecord();
            }
    
            else 
            {
                var recordID = GetNumberInput("\nPlease enter id of the record you want to delete\nType 0 to get back to Main Menu");

                if(recordID > 0) DeleteRecordQuery(recordID);
                
                else Menu.MainMenu();
            }
        }

        internal static void UpdateRecord()
        {
            Console.Clear();
            Console.WriteLine("Update Record");

            int rows = ViewAllRecords();

           if(rows == 0)
            {
                Console.WriteLine("\n\nPress any key to insert records...");
                Console.ReadKey();
                InsertRecord();
            }

           else
            {
                var recordId = GetNumberInput("\nPlease enter id of the record you want to update\nType 0 to get back to Main Menu");

                if (recordId > 0) UpdateRecordQuery(recordId);
             
                else Menu.MainMenu();
            }
        }
    }
}
