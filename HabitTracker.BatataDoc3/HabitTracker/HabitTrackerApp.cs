using HabitTracker.BatataDoc3.db;

namespace HabitTracker.BatataDoc3.HabitTracker
{
    
    internal class HabitTrackerApp
    {
        private Dictionary<string, string> habits = new Dictionary<string, string>(); 
        private Crud crud;

        public HabitTrackerApp(Crud crud) {
            this.crud = crud;

            string line;
            using (StreamReader sr = new StreamReader(@"HabitTracker\habits.txt")) { 
            
                line = sr.ReadLine();
                while (line != null)
                {
                    string[] words = line.Split(",");
                    habits.Add(words[0], words[1]);
                    line = sr.ReadLine();
                }
            }
        }

        public void MainMenu()
        {
            while (true)
            {
                String start = "0) Exit\n1) View All Records\n2) Insert Record\n3) Delete Record\n4) Update Record\n5) Insert new Habit";
                Console.WriteLine("=================\nMAIN MENU\n=================");
                Console.WriteLine("Please choose an option");
                Console.WriteLine(start);
                Console.WriteLine("---------------------------------------------------------");
                string input = Console.ReadLine();
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
                        string records = "=================\nVIEW ALL RECORDS\n=================\n";
                        Console.WriteLine(records);
                        ViewAllRecords();
                        break;
                    case 2:
                        InsertRecord();
                        break; 
                    case 3:
                        DeleteRecord();
                        break;
                    case 4:
                        UpdateRecord();
                        break;
                    case 5:
                        AddHabit();
                        break;
                    default:
                        Console.WriteLine("Please choose a valid option\n\n");
                        continue;
                }
            }
        }

        private void AddHabit()
        {
            Console.WriteLine("=================\nADD HABIT\n=================\n");

            while (true)
            {
                Console.WriteLine("\nPlease insert the new habit");
                Console.WriteLine("---------------------------------------------------------");
                string? habit = Console.ReadLine();
                if (habit.Length > 20 || habits.Keys.Contains(habit))
                {
                    Console.WriteLine("The habit should not be over 20 characters nor should it exist already");
                    Console.ReadLine();
                    continue;
                }
                Console.WriteLine("\nPlease insert the measurement");
                Console.WriteLine("---------------------------------------------------------");
                string? measure = Console.ReadLine();
                if (measure.Length > 10)
                {
                    Console.WriteLine("The measure should not be over 10 characters");
                    Console.ReadLine();
                    continue;
                }
                habits.Add(habit, measure);
                using (StreamWriter sw = File.AppendText(@"HabitTracker\habits.txt"))
                {
                    sw.WriteLine(habit + "," + measure);
                }
                Console.WriteLine("\nHabit added with success!");
                Console.ReadLine();
                return;

            }
        }

        private void ViewAllRecords()
        {
            
            List<Habit> habits = crud.GetAllRecords();
            if (habits.Count == 0)
            {
                Console.WriteLine("There is no data in the database");
                Console.ReadLine();
                return;
            }
            PrintResults(habits);
        }


        private void UpdateRecord()
        {
            string start = "=================\nUPDATE RECORD\n=================\n";
            Console.WriteLine(start);
            while (true) 
            { 
                Console.WriteLine("Please insert the ID you want to update (press 's' to see all records and 'b' to go back)");
                Console.WriteLine("---------------------------------------------------------");
                string input = Console.ReadLine();
                bool isInt = int.TryParse(input, out int option);
                if (input.Equals("s")) ViewAllRecords();
                if (input.Equals("b")) return;
                else if (!isInt)
                {
                    Console.WriteLine("Please insert a valid value");
                    continue;
                }
                else if (!crud.CheckIfTheIdExists(option))
                {
                    Console.WriteLine("Id not present in the database! Please insert a valid value");
                    Console.ReadLine();
                    continue;
                }
                else
                {
                    AlterInfo(option);
                    return;
                }
            }
        }

        private void AlterInfo(int id)
        {
            while (true)
            {

                Console.WriteLine("\nWhat would you like to alter?\n1)Habit\n2)Date\n3)Quantity");
                Console.WriteLine("---------------------------------------------------------");
                string input = Console.ReadLine();
                bool isInt = int.TryParse(input, out int option);
                if (!isInt || (!input.Equals("1") && !input.Equals("2") && !input.Equals("3")))
                {
                    Console.WriteLine("Please insert a valid value");
                    continue;
                }
                else if (option == 1)
                {
                    ChangeHabit(id);
                    return;
                }
                else if (option == 2) 
                {
                    ChangeDate(id);
                    return;
                }
                else
                {
                    ChangeQuantity(id);
                    return;
                }
                
            }
        }


        private void ChangeQuantity(int id)
        {
            while (true)
            {
                Console.WriteLine("What is the quantity you want to change to?");
                Console.WriteLine("---------------------------------------------------------");
                string input = Console.ReadLine();
                bool tryInt = int.TryParse(input, out int option);
                if (!tryInt) Console.WriteLine("\nPlease insert a valid value");
                else if (option < 1 || option > 10000) Console.WriteLine("\nPlease insert a valid value");
                else
                {
                    bool update = crud.UpdateRecord(id, option);
                    if (update)
                    {
                        Console.WriteLine("\nRecord updated with success! Press any key to continue");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Error updating db. Press any key to continue");
                        Console.ReadLine();
                    }
                }
            }
        }

        public void PopulateDb()
        {
            Random rand = new Random();
            int length = habits.Count();
            for (int i = 0; i < 100; i++)
            {
                int r = rand.Next(0, length);
                string habit = habits.Keys.ElementAt(r);
                string measure = habits[habit];
                int quantity = rand.Next(0, 10000);
                DateTime dt = DateTime.Now;
                crud.InsertRecord(habit, measure, quantity, dt);
            }
        }

        private void ChangeHabit(int id)
        {
            while (true)
            {
                Console.WriteLine("\nWhat is the new habit you want to change to? (press 's' for a list of available habits)");
                Console.WriteLine("---------------------------------------------------------");
                string input = Console.ReadLine();
                if (input.Equals("s"))
                {
                    ShowHabits();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }
                else if (!habits.ContainsKey(input))
                {
                    Console.WriteLine("Invalid Habit");
                    Console.ReadLine();
                }
                else
                {
                    int quantity = GetQuantity(input);
                    bool update = crud.UpdateRecord(id, input, habits[input], quantity);
                    if (update)
                    {
                        Console.WriteLine("\nRecord updated with success! Press any key to continue");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Error updating db. Press any key to continue");
                        Console.ReadLine();
                    }
                }
            }
        }

        private void ChangeDate(int id)
        {
            while (true)
            {
                Console.WriteLine("\nWhat is the new date you want to change to (YYYY-MM-DD)?");
                Console.WriteLine("---------------------------------------------------------");
                string input = Console.ReadLine();

                DateTime parsedDate;
                if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    bool update = crud.UpdateRecord(id, parsedDate);
                    if (update)
                    {
                        Console.WriteLine("\nRecord updated with success! Press any key to continue");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Error updating db. Press any key to continue");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Format. Press any key to continue");
                    Console.ReadLine();
                }
            }
        }

        private void PrintResults(List<Habit> habits)
        {
            string head = String.Format("||{0,-3}||{1,-25}||{2,-15}||{3,-10}|||", "Id", "Habit", "Quantity", "Date");
            string top = new('=', head.Length);
            Console.WriteLine(top);
            Console.WriteLine(head);
            Console.WriteLine(top);
            foreach(Habit habit in habits)
            {
                Console.WriteLine("||{0,-3}||{1,-25}||{2,-15}||{3,-10}||", habit.Id, habit.Name, habit.Quantity + " " + habit.Measure, habit.Date.ToShortDateString());

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
                Console.WriteLine("Which id would you want to remove? (press 's' to see records and 'b' to go back)");
                Console.WriteLine("---------------------------------------------------------");
                string response = Console.ReadLine();
                if (response.Equals("s")) ViewAllRecords();
                else if (response.Equals("b")) return;
                else { 

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
                    return;
                }
            }
        }


        private void InsertRecord()
        {
            string start = "=================\nINSERT A NEW RECORD\n=================\n";
            Console.WriteLine(start);
            while (true) { 

                string habitsString = "\nWhat habit do you want to add? Write 's' for a list of available habits and 'b' to go back to the Main Menu\n---------------------------------------------------------";
                Console.WriteLine(habitsString);
                string input = Console.ReadLine();
                if (input.Equals("s"))
                {
                    ShowHabits();
                    Console.WriteLine("Press any key to go back");
                    Console.ReadLine();
                }
                else if (input.Equals("b"))
                {
                    return;
                }
                else if (habits.ContainsKey(input))
                {
                    DateTime parsedTime = GetDate();
                    int quantity = GetQuantity(input);
                    crud.InsertRecord(input, habits[input], quantity, parsedTime);
                    Console.WriteLine("\nRecord added successfully!\nPress any key to continue");
                    Console.ReadLine();
                    return;
                    
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
                Console.WriteLine("\nInput the Date of the Habit (YYYY-MM-DD)");
                Console.WriteLine("---------------------------------------------------------");
                string input = Console.ReadLine();

                DateTime parsedDate;
                if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    return parsedDate;
                }
                else
                {
                    Console.WriteLine("invalid format");
                    Console.ReadLine();
                }
            }
        }

        private int GetQuantity(string habit)
        {
            while(true)
            {
                Console.WriteLine($"\nPlease insert the number of {habits[habit]}");
                Console.WriteLine("---------------------------------------------------------");
                string? input = Console.ReadLine();
                bool tryInt = int.TryParse(input, out int value);
                if (!tryInt)
                {
                    Console.WriteLine("The input must be an integer");
                    Console.ReadLine();
                }
                else if (value < 1 || value > 10000)
                {
                    Console.WriteLine("Please insert a valid value");
                    Console.ReadLine();
                }
                else return value;
            }
        }


        private void ShowHabits()
        {
            Console.WriteLine("---------------------------------------------------------");
            foreach(string habit in habits.Keys)
            {
                Console.WriteLine(habit);
                
            }
            Console.WriteLine("---------------------------------------------------------");
        }
    }
}
