using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Paul_W_Saltzman
{
    internal class UnitType
    {
        internal int UnitTypeId;
        internal String UnitName;

        internal static List<UnitType> ListUnitTypes()
        {
            Console.Clear();
            List<UnitType> unitList = Data.LoadUnits();
            Console.WriteLine("                     Units                     ");
            Console.WriteLine("_______________________________________________");
            foreach (var habit in unitList)
            {
                Console.WriteLine($@"{habit.UnitTypeId} - {habit.UnitName}");
            }
            Console.WriteLine("_______________________________________________");
            return unitList;

        }
        public static int ChooseUnit(string userInstruction)
        {
            int unitId = 0;
            bool unitFound = false;
            while (!unitFound)
            {
                Console.Clear();
                List<UnitType> unitList = UnitType.ListUnitTypes();
                Console.WriteLine($@"{userInstruction}");



                string input = Console.ReadLine();
                input = input.Trim().ToLower();


                if (input == "a")
                {
                    AddUnit();
                    Console.ReadLine();
                }
                else
                {
                    int selectedHabitID;
                    if (int.TryParse(input, out selectedHabitID))
                    {
                        foreach (var habit in unitList)
                        {
                            if (selectedHabitID == habit.UnitTypeId)
                            {
                                Console.Clear();
                                Console.WriteLine($@"{habit.UnitTypeId} - {habit.UnitName}");
                                unitFound = true;
                                unitId = habit.UnitTypeId;
                                break;

                            }
                        }
                    }
                }

                if (!unitFound)
                {
                }

            }

            return unitId;

        }
        public static int ChooseUnit(string userInstruction, int habitType)
        {
            {
                int unitId = 0;
                bool unitFound = false;
                while (!unitFound)
                {
                    Console.Clear();
                    List<UnitType> unitList = UnitType.ListUnitTypes();
                    Console.WriteLine($@"{userInstruction}");



                    string input = Console.ReadLine();
                    input = input.Trim().ToLower();


                    if (input == "a")
                    {
                        AddUnit();
                        Console.ReadLine();
                    }
                    else
                    {
                        int selectedHabitID;
                        if (int.TryParse(input, out selectedHabitID))
                        {
                            foreach (var habit in unitList)
                            {
                                if (selectedHabitID == habit.UnitTypeId)
                                {
                                    Console.Clear();
                                    Console.WriteLine($@"{habit.UnitTypeId} - {habit.UnitName}");
                                    unitFound = true;
                                    unitId = habit.UnitTypeId;
                                    break;

                                }
                            }
                        }
                    }

                    if (!unitFound)
                    {
                    }

                }

                return unitId;
            }
        }


        public static void CRUDUnitTypes()
        {
            bool valid = false;
            while (!valid)
            {
                UnitType.ListUnitTypes();
                Console.WriteLine("Options: X to Exit : A to Add a record: D to Delete a record: U to Update a record");

                string userInput = Console.ReadLine();
                userInput = userInput.ToLower().Trim();
                switch (userInput)
                {
                    case "x":
                        valid = true;
                        break;
                    case "a":
                        AddUnit();
                        break;
                    case "d":
                        DeleteUnit();
                        break;
                    case "u":
                        UpdateUnitType();
                        break;
                    default:
                        break;

                }
            }
        }
        public static void AddUnit()
        {
            Console.Clear();
            Console.WriteLine("Please enter a new Unit.");
            string newUnit = Console.ReadLine();
            newUnit = Helpers.Sanitize(newUnit);
            newUnit = Helpers.EnforceFormatting(newUnit);
            newUnit = Helpers.CheckSize(newUnit);
            bool dupsFound = Helpers.CheckForDuplicateUnits(newUnit);
            if (!dupsFound)
            {
                Data.CreateUnitType(newUnit);
            }
            else
            {
                Console.WriteLine($@"There is already a habit called {newUnit}.");
            }

        }
        public static void DeleteUnit()
        {
            bool beingUsed = false;
            string userInstruction = "Please choose the unit type you wish to delete.";
            int unitTypeID = ChooseUnit(userInstruction);
            if (LoggedHabitView.BeingUsed(unitTypeID, 1))
            { 
                beingUsed = true;
                Console.WriteLine($@"Habit ID {unitTypeID} is being used.  Please Delete any Logged Habits using unit {unitTypeID} first.");
            }
            if (Habits.UnitBeingUsed(unitTypeID))
            {
                beingUsed = true;
                Console.WriteLine($@"Habit ID {unitTypeID} is being used.  Please Delete any Habits using unit {unitTypeID} first.)");
            }
            if (!beingUsed)
            {
                    Data.DeleteUnitType(unitTypeID);
            }
            Console.WriteLine("Press ENTER to Continue");
            Console.ReadLine();
        }
        public static void UpdateUnitType()
        {
            string userInstruction = "Please choose the unit type you wish to update.";
            int habitID = ChooseUnit(userInstruction);
            Console.WriteLine("Please enter the updated unit type");
            string updatedUnit = Console.ReadLine();
            updatedUnit = Helpers.Sanitize(updatedUnit);
            updatedUnit = Helpers.EnforceFormatting(updatedUnit);
            updatedUnit = Helpers.CheckSize(updatedUnit);
            bool dupsFound = Helpers.CheckForDuplicateHabits(updatedUnit);
            if (!dupsFound)
            {
                Data.UpdateUnitType(habitID, updatedUnit);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($@"There is already a habit called {updatedUnit}.");
            }
        }
    }
}
