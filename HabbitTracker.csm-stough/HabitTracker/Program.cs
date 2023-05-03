using HabitTracker;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Net.NetworkInformation;

namespace habit_tracker {

    class Program
    {

        static void Main(string[] args)
        {
            RecordService.Init();
            MainMenu();
        }

        static void MainMenu()
        {
            ConsoleUtilities.Menu mainMenu = new ConsoleUtilities.Menu("Main Menu, please select an opion");

            //Display Record Menu
            mainMenu.AddOption("A", "View All Records...", () => { RecordsMenu(); });

            //Insert new record
            mainMenu.AddOption("I", "Insert New Record", () => {
                ConsoleUtilities.Form insertForm = new ConsoleUtilities.Form((values) =>
                {
                    Console.Clear();

                    RecordService.Insert((DateTime)values[0], int.Parse(values[1].ToString()));

                });

                insertForm.AddQuery<DateTime>("Enter the date (dd/mm/yyyy)");
                insertForm.AddQuery<int>("Enter the quantity");

                insertForm.Start();
            });

            //Quit program
            mainMenu.AddOption("Q", "Quite Habit Tracker", () => { Environment.Exit(0); });

            mainMenu.SelectOption();
        }

        static void RecordsMenu()
        {
            ConsoleUtilities.Menu recordsMenu = new ConsoleUtilities.Menu("Avalable records, please select by #");

            List<WaterRecord> records = RecordService.GetAll();

            //This is just to label the search result number --> instead of using entry ID
            int num = 1;

            records.ForEach((record) =>
            {
                recordsMenu.AddOption(num.ToString(), record.Date.Date.ToShortDateString(), () => { RecordMenu(record); });
                num++;
            });

            recordsMenu.AddOption("M", "Go Back To Main Menu", () => { MainMenu(); });

            recordsMenu.SelectOption();
        }

        static void RecordMenu(WaterRecord record)
        {
            ConsoleUtilities.Menu recordMenu = new ConsoleUtilities.Menu($"Record Submitted On {record.Date.Date.ToShortDateString()}, with Quantity: {record.Quantity}. Please choose an option below");

            recordMenu.AddOption("U", "Update Record Information", () => {
                ConsoleUtilities.Form updateForm = new ConsoleUtilities.Form((values) =>
                {
                    if (values[0].ToString().ToLower() == "y")
                    {
                        record.Date = (DateTime)values[1];
                        record.Quantity = int.Parse(values[2].ToString());
                        RecordService.Update(record);
                        RecordMenu(record);
                    }
                });

                updateForm.AddQuery<string>("Are you sure you want to update this record?");
                updateForm.AddQuery<DateTime>("Please enter a new date (dd/mm/yyyy)");
                updateForm.AddQuery<int>("Please enter a new quantity");

                updateForm.Start();
            });
            recordMenu.AddOption("D", "Delete Record", () => {
                ConsoleUtilities.Form confirmationForm = new ConsoleUtilities.Form((values) =>
                {
                    if (values[0].ToString().ToLower() == "y")
                    {
                        RecordService.Delete(record);
                        RecordsMenu();
                    }
                });

                confirmationForm.AddQuery<string>("Are you sure you want to delete this record? (y/n)");

                confirmationForm.Start();
            });
            recordMenu.AddOption("A", "Go Back To All Records", () => { RecordsMenu(); });

            recordMenu.SelectOption();
        }
    }

}