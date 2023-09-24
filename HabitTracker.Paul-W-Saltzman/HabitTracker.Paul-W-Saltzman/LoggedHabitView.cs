using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Paul_W_Saltzman
{
    internal class LoggedHabitView
    {
        internal int LoggedId { get; set; }
        internal string HabitName { get; set; }
        internal int HabitId { get; set; }
        internal int Quantity { get; set; }
        internal string UnitName { get; set; }
        internal int UnitId { get; set; }
        internal DateOnly Date { get; set; }


        public static void RadomLogHabit()
        {
            bool valid = false;
            int numberOfRandom = 0;
            while (!valid)
            {
                Console.WriteLine("How many random habits would you like to load? choose a number between 1 and 50");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out numberOfRandom))
                {
                    if (numberOfRandom > 0 && numberOfRandom < 51)
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine($@" ENTERED: {numberOfRandom} - Number is either to large or too small. Press ENTER to continue.");
                        Console.ReadLine();

                    }
                }
                else { }
            }
            int numberOfHabits = Helpers.NumberOfHabits();
            int numberOfUnits = Helpers.NumberOfUnits();
            int randoms = numberOfRandom;
            while (randoms > 0)
            {
                Random random = new Random();
                Habits randomHabit = null;
                while (randomHabit == null)//some habit Id's can be null due to deletions so I need to check and make sure the value is not null
                {
                    int randomHabitID = random.Next(1, numberOfHabits + 1);
                    List<Habits> habitList = Data.LoadHabits();
                    randomHabit = habitList.FirstOrDefault(habit => habit.HabitId == randomHabitID);
                }
                int currentYear = DateTime.Now.Year;      //this code block chooses a random date
                int year = random.Next(2010, currentYear + 1);
                int month = random.Next(1, 13);
                int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
                DateOnly randomDateInput = new DateOnly(year, month, day);

                int randomNumberInput = random.Next(1, 21);

                Data.LogHabbit(randomHabit.HabitId, randomHabit.UnitId, randomDateInput, randomNumberInput);
                randoms--;

            }

            Console.WriteLine($@"{numberOfRandom} random habits have been entered. Press ENTER to continue");
            Console.ReadLine();



            //

        }
        internal static void CLoggedHabit()
        {
            DateOnly dateInput = Helpers.GetDate();
            string userInstruction = "Please choose a habit type to log or press A to add a new habit.";
            Habits habit = Habits.ChooseHabit(userInstruction, 1);
            //userInstruction = "Please choose a habit to log or press A to add a new habit.";
            //int unitID = UnitType.ChooseUnit(userInstruction);
            int numberInput = Helpers.GetNumber(habit.UnitId);
            Data.LogHabbit(habit.HabitId, habit.UnitId, dateInput, numberInput);
            Console.WriteLine("Press Enter to Continue");
            Console.ReadLine();

        }
        public static void RUDLoggedHabits()
        {
            int habitID = -1;

            bool stayOnPage = true;
            while (stayOnPage)
            {
                Console.Clear();
                List<LoggedHabitView> loggedHabitList = Data.LoadLoggedHabits();
                Console.WriteLine("                          Logged Habits                                 ");
                Console.WriteLine($@"________________________________________________________________________");
                Console.WriteLine("|{0,5}|{1,20}|{2,10}|{3,20}|{4,11}|", "Log#", "Habit Name", "Number", "Unit Type", "Date");
                Console.WriteLine($@"________________________________________________________________________");
                int total = 0;
                String habitName = "";
                String unitName = "";
                foreach (var LoggedHabit in loggedHabitList)
                {

                    switch (habitID)
                    {
                        case -1:
                            Console.WriteLine("|{0,5}|{1,20}|{2,10}|{3,20}|{4,11}|", LoggedHabit.LoggedId, LoggedHabit.HabitName, LoggedHabit.Quantity, LoggedHabit.UnitName, LoggedHabit.Date);

                            break;
                        default:
                            if (LoggedHabit.HabitId == habitID)
                            {
                                Console.WriteLine("|{0,5}|{1,20}|{2,10}|{3,20}|{4,11}|", LoggedHabit.LoggedId, LoggedHabit.HabitName, LoggedHabit.Quantity, LoggedHabit.UnitName, LoggedHabit.Date);
                                total = total+LoggedHabit.Quantity; 
                                habitName = LoggedHabit.HabitName;
                                unitName = LoggedHabit.UnitName;

                            }
                            else { }
                            break;
                    }
                }
                if (total == 0) {}
                else
                {
                    Console.WriteLine($@"________________________________________________________________________");
                    Console.WriteLine("|{0,20} {1,16} {2,5} {3,5}{4,20}|", habitName,"","Total:", total, unitName);
                    Console.WriteLine($@"________________________________________________________________________");
                }
                    
                Console.WriteLine($@"__________________________________________________________________________________________");
                Console.WriteLine($@"Options : X - Exit : A - All Habits : R - Reporting : D - Delete record : U - Update record:");
                string userInput = Console.ReadLine();
                userInput = userInput.ToLower().Trim();
                switch (userInput)
                {
                    case "x":
                        stayOnPage = false;
                        break;
                    case "a":
                        habitID = -1;
                        break;
                    case "r":
                        string userInstruction = "Please Choose a Habit to report on.";
                        Habits habit = Habits.ChooseHabit(userInstruction, 3);
                        habitID = habit.HabitId;
                        //userInstruction = "Please Chose the Unit to report on.";
                        //unitID = UnitType.ChooseUnit(userInstruction,habit.HabitId);
                        break;
                    case "d":
                        DeleteLoggedHabit();
                        break;
                    case "u":
                        UpdateLoggedHabit();
                        break;
                    default: break;

                }
            }

        }
        internal static void DeleteLoggedHabit()
        {
            int logedHabitId = 0;
            bool valid = false;

            while (!valid)
            {
                Console.WriteLine("Please enter the number corresponding to the logged habit you wish to delete.");
                string numberInputString = Console.ReadLine();
                if (int.TryParse(numberInputString, out int number))
                {
                    logedHabitId = number;
                    valid = LoggedHabitView.ValidateNumber(logedHabitId);
                    if (!valid)
                    {
                        Console.WriteLine("Invalid Input Press Enter to Continue");
                        Console.ReadLine();
                    }

                }
                else
                {
                    Console.WriteLine("Invalid Input Press Enter to Continue");
                    Console.ReadLine();
                }

            }

            Data.DeleteLoggedHabit(logedHabitId);

            Console.WriteLine("Press ENTER to Continue.");
            Console.ReadLine();

        }
        internal static void UpdateLoggedHabit()
        {
            int logedHabitId = 0;
            bool valid = false;
            Console.WriteLine("Please enter the number corresponding to the logged habit you wish to update.");

            while (!valid)
            {

                string numberInputString = Console.ReadLine();
                if (int.TryParse(numberInputString, out int number))
                {
                    logedHabitId = number;
                    valid = LoggedHabitView.ValidateNumber(logedHabitId);
                    if (!valid)
                    {
                        Console.WriteLine("Invalid Input Press Enter to Continue");
                        Console.ReadLine();
                    }

                }
                else
                {
                    Console.WriteLine("Invalid Input Press Enter to Continue");
                    Console.ReadLine();
                }

            }
            DateOnly newDate = Helpers.GetDate();
            string userInstruction = "Please choose a habit type to log or press A to add a new habit.";
            Habits newHabit = Habits.ChooseHabit(userInstruction, 1);
            userInstruction = "Please choose a habit to log or press A to add a new habit.";
            //int newUnit = UnitType.ChooseUnit(userInstruction);
            int newNumberInput = Helpers.GetNumber(newHabit.UnitId);

            Data.UpdateLoggedHabit(logedHabitId, newHabit.HabitId, newHabit.UnitId, newDate, newNumberInput);

            Console.WriteLine("Press ENTER to Continue.");
            Console.ReadLine();

        }
        internal static bool ValidateNumber(int number)
        {
            bool valid = false;
            List<LoggedHabitView> loggedHabitList = Data.LoadLoggedHabits();

            foreach (var logedHabit in loggedHabitList)
            {
                if (number == logedHabit.LoggedId)
                {
                    valid = true;
                    break;
                }
            }
            return valid;

        }
        internal static bool BeingUsed(int number, int option)//option 1 to look for Unit option 2 to look for Habit.
        {
            bool beingUsed = false;
            List<LoggedHabitView> loggedHabitList = Data.LoadLoggedHabits();

            foreach (var logedHabit in loggedHabitList)
            {
                switch (option)
                {
                    case 1:
                    if (number == logedHabit.UnitId)
                    {
                        beingUsed = true;
                        break;
                    }
                        break;
                    case 2:
                    if (number == logedHabit.HabitId)
                    {
                        beingUsed = true;
                        break;
                    }
                        break;
                }
            }
            return beingUsed;

        }
        

    }

    
}
