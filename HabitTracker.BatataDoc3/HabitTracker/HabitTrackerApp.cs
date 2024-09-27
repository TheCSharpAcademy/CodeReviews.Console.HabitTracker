using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabitTracker.BatataDoc3.db;
using Microsoft.Data.Sqlite;

namespace HabitTracker.BatataDoc3.HabitTracker
{
    
    internal class HabitTrackerApp
    {
        private List<String> habits = new List<String>(); 
        private CRUD crud;

        public HabitTrackerApp(CRUD crud) {
            this.crud = crud;

            String line;
            StreamReader sr = new StreamReader(@"HabitTracker\habits.txt");
            line = sr.ReadLine();
            while (line != null) 
            { 
                habits.Add(line);
                line = sr.ReadLine();
            }
        }

        public void MainMenu()
        {
            while (true)
            {
                String start = "0) Exit\n1) View All Records\n2) Insert Record\n3) Delete Record\n4) Update Record";
                Console.WriteLine("=================\nMAIN MENU\n=================");
                Console.WriteLine("Please choose an option");
                Console.WriteLine(start);
                Console.WriteLine("---------------------------------------------------------");
                String input = Console.ReadLine();
                bool success = int.TryParse(input, out int option);
                if (!success )
                {
                    Console.WriteLine("Please choose a valid option\n\n");
                    continue;
                }

                switch(option)
                {
                    case 0:
                        return;
                    case 1:
                        ViewAllRecords();
                        break;
                    case 2:
                        InsertRecord();
                        break; 
                    case 3:
                        DeleteRecord();
                        break;
                    case 4:
                        Console.WriteLine("4");
                        break;
                    default:
                        Console.WriteLine("Please choose a valid option\n\n");
                        continue;
                }
            }
        }

        private void ViewAllRecords()
        {
            String start = "=================\nVIEW ALL RECORDS\n=================\n";
            Console.WriteLine(start);
            List<Habit> habits = crud.GetAllRecords();
            if (habits.Count == 0)
            {
                Console.WriteLine("There is no data in the database");
                MainMenu();
            }
            PrintResults(habits);
        }

        private void PrintResults(List<Habit> habits)
        {
            int linespace = 20;
            string head = String.Format("||{0,-3}||{1,-25}||{2,-10}||", "Id", "Habit", "Date");
            string top = new('=', head.Length);
            Console.WriteLine(top);
            Console.WriteLine(head);
            Console.WriteLine(top);
            foreach(Habit habit in habits)
            {
                Console.WriteLine("||{0,-3}||{1,-25}||{2,-10}||", habit.Id, habit.Name, habit.Date.ToShortDateString());

            }
            Console.WriteLine(top + "\n");
            Console.ReadLine();
        }



        private void DeleteRecord()
        {
            String start = "=================\nDELETE A RECORD\n=================\n";
            Console.WriteLine(start);
            while (true)
            {
                Console.WriteLine("Which id would you want to remove?");
                string response = Console.ReadLine();
                bool parse = int.TryParse(response, out int id);
                if (!parse)
                {
                    Console.WriteLine("Please insert a valid id");
                    Console.ReadLine();
                    continue;
                }
                bool success = crud.DeleteRecord(id);
                if (!success)
                {
                    Console.WriteLine("The provided id does not exist");
                    Console.ReadLine();
                    continue;
                }
                Console.WriteLine("\nRecord deleted with success!\n");
                Console.ReadLine();
                MainMenu();
            }
        }


        private void InsertRecord()
        {
            String start = "=================\nINSERT A NEW RECORD\n=================\n";
            Console.WriteLine(start);
            while (true) { 

                String habitsString = "\nWhat habit do you want to add? Write 's' for a list of available habits and 'b' to go back to the Main Menu\n---------------------------------------------------------";
                Console.WriteLine(habitsString);
                String input = Console.ReadLine();
                if (input.Equals("s"))
                {
                    ShowHabits();
                    Console.WriteLine("Press any key to go back");
                    Console.ReadLine();
                }
                else if (input.Equals("b"))
                {
                    MainMenu();
                }
                else if (habits.Contains(input))
                {
                    DateTime parsedTime = GetDate();
                    crud.InsertRecord(input, parsedTime);
                    Console.WriteLine("\nRecord added successfully!\nPress any key to continue");
                    Console.ReadLine();
                    MainMenu();
                    
                }
                else
                {
                    Console.WriteLine("Invalid Habit");
                    Console.ReadLine();
                }
            }
        }

        private DateTime GetDate()
        {
            while (true)
            { 
                Console.WriteLine("Input the Date of the Habit (YYYY-MM-DD)");
                string input = Console.ReadLine();

                DateTime parsedDate;
                if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    return parsedDate;
                }
                else
                {
                    Console.WriteLine("invalid format");
                }
            }
        }


        private void ShowHabits()
        {
            Console.WriteLine("---------------------------------------------------------");
            foreach(string habit in habits)
            {
                Console.WriteLine(habit);
                
            }
            Console.WriteLine("---------------------------------------------------------");
        }
    }
}
