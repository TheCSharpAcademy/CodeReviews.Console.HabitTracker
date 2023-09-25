using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Paul_W_Saltzman
{
    internal class Habits
    {
        internal int HabitId { get; set; }
        internal string HabitName { get; set; }
        internal int UnitId { get; set; }


        public static void CRUDHabit()
        {
            bool valid = false;
            while (!valid)
            {
                Habits.ListHabits();
                Console.WriteLine("Options: X to Exit : A to Add a record: D to Delete a record: U to Update a record");

                string userInput = Console.ReadLine();
                userInput = userInput.ToLower().Trim();
                switch (userInput)
                {
                    case "x":
                        valid = true;
                        break;
                    case "a":
                        AddHabit();
                        break;
                    case "d":
                        DeleteHabit();
                        break;
                    case "u":
                        UpdateHabit();
                        break;
                    default:
                        break;

                }
            }

        }

        public static void AddHabit()
        {
            Console.Clear();
            Console.WriteLine("Please enter a new Habit.");
            string newHabit = Console.ReadLine();
            newHabit = Helpers.Sanitize(newHabit);
            newHabit = Helpers.EnforceFormatting(newHabit);
            newHabit = Helpers.CheckSize(newHabit);
            bool dupsFound = Helpers.CheckForDuplicateHabits(newHabit);
            if (dupsFound)
            {
                Console.WriteLine($@"There is already a habit called {newHabit}.");
            }
            else
            {
                string userInstruction = "Assign a Unit Type to your new Habit or Type A to Add a new Unit Type.";
                int unitTypeId = UnitType.ChooseUnit(userInstruction);
                Data.CreateHabit(newHabit, unitTypeId);
            }


        }

        public static void DeleteHabit()
        {
            string userInstruction = "Please choose the habit you wish to delete.";
            Habits habit = ChooseHabit(userInstruction, 3);
            bool beingUsed = LoggedHabitView.BeingUsed(habit.HabitId, 2);
            if (beingUsed)
            {
                Console.WriteLine($@"Habit ID {habit.HabitName} is being used.  Please Delete any Logged Habits using Habit {habit.HabitName} first.");
            }
            else
            {
                Data.DeleteHabit(habit.HabitId);
            }
            Console.WriteLine("Press ENTER to Continue");
            Console.ReadLine();

        }

        public static void UpdateHabit()
        {
            string userInstruction = "Please choose the habit you wish to update.";
            Habits habit = ChooseHabit(userInstruction, 4);
            Console.WriteLine("Please enter the updated habit name");
            string updatedHabit = Console.ReadLine();
            updatedHabit = Helpers.Sanitize(updatedHabit);
            updatedHabit = Helpers.EnforceFormatting(updatedHabit);
            updatedHabit = Helpers.CheckSize(updatedHabit);
            bool dupsFound = Helpers.CheckForDuplicateHabits(updatedHabit);
            if (!dupsFound)
            {
                Data.UpdateHabit(habit.HabitId, updatedHabit);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($@"There is already a habit called {updatedHabit}.");
            }

        }
        internal static List<Habits> ListHabits()
        {
            Console.Clear();
            List<Habits> habitList = Data.LoadHabits();
            Console.WriteLine("                 Habits                        ");
            Console.WriteLine("_______________________________________________");
            foreach (var habit in habitList)
            {
                Console.WriteLine($@"{habit.HabitId} - {habit.HabitName}");
            }
            Console.WriteLine("_______________________________________________");
            return habitList;
        }

        internal static Habits ChooseHabit(string userInstruction, int option)
        {
            int habitId = 0;
            bool habitFound = false;
            Habits chosenHabit = null;



            while (!habitFound)
            {

                List<Habits> habitList = Habits.ListHabits();
                Console.WriteLine(userInstruction);



                string input = Console.ReadLine();
                input = input.Trim().ToLower();


                if (input == "a")
                {
                    switch (option)
                    {
                        case 1:
                            AddHabit();
                            Console.ReadLine();
                            break;
                        case 2:
                            habitId = -1;
                            habitFound = true;
                            break;
                    }

                }
                else
                {
                    int selectedHabitID;
                    if (int.TryParse(input, out selectedHabitID))
                    {
                        foreach (var habit in habitList)
                        {
                            if (selectedHabitID == habit.HabitId)
                            {
                                Console.Clear();
                                Console.WriteLine($@"{habit.HabitId} - {habit.HabitName}");
                                habitFound = true;
                                chosenHabit = habit;
                                break;

                            }
                        }
                    }
                }

                if (!habitFound)
                {
                }

            }

            return chosenHabit;

        }

        internal static bool UnitBeingUsed(int unitTypeID)
        {
            bool beingUsed = false;
            List<Habits> habitList = Data.LoadHabits();
            Habits habit = habitList.FirstOrDefault(Habits => Habits.UnitId == unitTypeID);
            if (habit != null)
            {
                beingUsed = true;
            }
            else { }

            return beingUsed;
        }

}
}
