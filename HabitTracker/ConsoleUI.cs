using HabitTrackerLibrary.DataAccess;
using HabitTrackerLibrary.Models;

namespace HabitTracker
{
    public class ConsoleUI
    {
        private readonly SqlData sqlData;
        private readonly Menu mainMenu = new Menu(
            "Main Menu",
            "Select an option below by entering the menu item number:",
            new List<string>()
            {
                "Select Habit",
                "Add New Habit",
                "Delete Habit",
                "Exit Application"
            }
        );
        private readonly Menu habitMenu = new Menu(
            "View Habit",
            "Select an option below by entering the menu item number:",
            new List<string>()
            {
                "Add Record",
                "Delete Record",
                "Change Record",
                "Generate Report",
                "Return to Main Menu"
            }
        );
        private readonly Menu reportMenu = new Menu(
            "Select Report",
            "Select an option below by entering the menu item number:",
            new List<string>()
            {
                "Past Year",
                "Year to Date",
                "All Records",
                "Custom Range",
                "Return to Main Menu"
            }
        );

        public ConsoleUI(SqlData sqlData)
        {
            this.sqlData = sqlData;
            MenuHandler_MainMenu();
        }

        private void MenuHandler_MainMenu()
        {
            bool closeApp = false;

            while (closeApp == false)
            {
                PrintTitleBar();
                PrintHabitsList();
                mainMenu.PrintMenu();

                int userSelection = (int)UserInput.GetNumberInput("Enter menu selection: ", 1, mainMenu.Options.Count);

                switch (userSelection)
                {
                    case 1:
                        ProcessManager_SelectHabit();
                        break;
                    case 2:
                        ProcessManager_CreateHabit();
                        break;
                    case 3:
                        ProcessManager_DeleteHabit();
                        break;
                    case 4:
                        PrintGoodbyeMessage();
                        closeApp = true;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("ERROR - Invalid selection detected");
                        break;
                }
            }
        }
        private void MenuHandler_SelectHabit(HabitModel habit)
        {
            bool returnToMainMenu = false;
            bool validInput = true;

            while (returnToMainMenu == false)
            {
                var sortedRecords = GetSortedRecordsList(habit);
                PrintTitleBar();
                PrintRecordsList(sortedRecords, habit);
                habitMenu.PrintMenu();

                do
                {
                    int userSelection = (int)UserInput.GetNumberInput("Enter menu selection: ", 1, habitMenu.Options.Count);

                    switch (userSelection)
                    {
                        case 1:
                            ProcessManager_AddRecord(sortedRecords, habit);
                            break;
                        case 2:
                            ProcessManager_DeleteRecord(sortedRecords, habit);
                            break;
                        case 3:
                            ProcessManager_UpdateRecord(sortedRecords, habit);
                            break;
                        case 4:
                            MenuHandler_SelectReport(habit);
                            break;
                        case 5:
                            returnToMainMenu = true;
                            break;
                        default:
                            validInput = false;
                            Console.WriteLine();
                            Console.WriteLine("ERROR - Invalid selection detected");
                            break;
                    }
                } while (validInput == false); 
            }
        }
        private void MenuHandler_SelectReport(HabitModel habit)
        {
            bool returnToPreviousMenu = false;
            bool validInput = true;

            while (returnToPreviousMenu == false)
            {
                var startDate = new DateTime();
                var endDate = new DateTime();

                PrintTitleBar();
                reportMenu.PrintMenu();

                do
                {
                    int userSelection = (int)UserInput.GetNumberInput("Enter menu selection: ", 1, reportMenu.Options.Count);

                    switch (userSelection)
                    {
                        case 1:
                            (startDate, endDate) = GetDatesForPastYear();
                            break;
                        case 2:
                            (startDate, endDate) = GetDatesForYearToDate();
                            break;
                        case 3:
                            startDate = DateTime.MinValue.AddDays(1);
                            endDate = DateTime.MaxValue;
                            break;
                        case 4:
                            (startDate, endDate) = GetDateForCustomRange();
                            break;
                        case 5:
                            returnToPreviousMenu = true;
                            break;
                        default:
                            validInput = false;
                            Console.WriteLine();
                            Console.WriteLine("ERROR - Invalid selection detected");
                            break;
                    }
                } while (validInput == false);

                if (returnToPreviousMenu == false)
                {
                    ProcessManager_SelectReport(habit, startDate, endDate);
                }
            }
        }



        private void ProcessManager_SelectReport(HabitModel habit, DateTime startDate, DateTime endDate)
        {
            var sortedRecords = GetSortedRecordsList(habit);

            PrintTitleBar();

            var report = new ReportModel(sortedRecords, startDate, endDate);

            PrintReport(habit, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), report);
        }
        private void ProcessManager_SelectHabit()
        {
            PrintTitleBar();
            PrintHabitsList();
            var habit = GetHabitFromList("view");

            MenuHandler_SelectHabit(habit);
        }
        private void ProcessManager_CreateHabit()
        {
            PrintTitleBar();
            PrintHabitsList();
            var habitName = GetNewHabitName();

            PrintTitleBar();
            PrintUnitsList();
            var unitName = GetUnitNameFromList();

            PrintHabitData(habitName, unitName);
            ConfirmAddHabit(habitName, unitName);
        }
        private void ProcessManager_DeleteHabit()
        {
            PrintTitleBar();
            PrintHabitsList();
            var habit = GetHabitFromList("delete");

            var recordCount = GetSortedRecordsList(habit).Count();

            PrintHabitDeleteWarning(habit.HabitName, recordCount);
            ConfirmHabitDelete(habit);
        }
        private void ProcessManager_AddRecord(List<RecordModel> sortedRecords, HabitModel habit)
        {
            PrintTitleBar();
            PrintRecordsList( sortedRecords, habit);
            PrintAddRecordInstruction(habit.HabitName);
            (DateTime date, double quantity) = GetRecordData(habit);

            PrintRecordData("Adding", quantity, habit.UnitName, habit.HabitName, date.ToString("yyyy-MM-dd"));
            UserInput.PressAnyKeyToContinue();

            sqlData.InsertRecord(habit.HabitId, date.ToString("yyyy-MM-dd"), quantity);
        }
        private void ProcessManager_UpdateRecord(List<RecordModel> sortedRecords, HabitModel habit)
        {
            var record = GetRecordFromList(sortedRecords, "change", habit);
            PrintRecordData("Changing", record.Quantity, habit.UnitName, habit.HabitName, record.Date.ToString("yyyy-MM-dd"));

            (bool dateChanged, var newDate) = GetUpdatedDateFromUser(record.Date);
            (bool quantityChanged, var newQuantity) = GetUpdatedQuantityFromUser(record.Quantity);

            PrintRecordUpdateMessage(dateChanged, quantityChanged, record.Date.ToString("yyyy-MM-dd"), newDate.ToString("yyyy-MM-dd"), record.Quantity, newQuantity, habit.UnitName);
            ConfirmRecordUpdate(record.Id, newDate.ToString("yyyy-MM-dd"), newQuantity);
        }
        private void ProcessManager_DeleteRecord(List<RecordModel> sortedRecords, HabitModel habit)
        {
            var record = GetRecordFromList(sortedRecords, "delete", habit);

            PrintRecordData("Preparing to delete", record.Quantity, habit.UnitName, habit.HabitName, record.Date.ToString("yyyy-MM-dd"));

            ConfirmRecordDelete(record.Id);
        }



        private void ConfirmHabitDelete(HabitModel habit)
        {
            bool habitDeleted = UserInput.GetUserConfirmation("delete");
            string message = $"User cancelled the deletion of {habit.HabitName}.";

            if (habitDeleted)
            {
                sqlData.DeleteAllRecordsForAHabit(habit.HabitId);
                sqlData.DeleteHabit(habit.HabitId);

                message = $"{habit.HabitName} was successfully deleted!";
            }

            Console.WriteLine(message);
            UserInput.PressAnyKeyToContinue();
        }
        private void ConfirmRecordDelete(int recordId)
        {
            bool recordDeleted = UserInput.GetUserConfirmation("delete");
            string message = "Record delete cancelled!";

            if (recordDeleted)
            {
                message = "Record successfuly deleted!";
                sqlData.DeleteRecord("Records", recordId);
            }

            Console.WriteLine(message);
            UserInput.PressAnyKeyToContinue();
        }
        private void ConfirmRecordUpdate(int recordId, string newDate, double newQuantity)
        {
            bool recordUpdated = UserInput.GetUserConfirmation("update");
            string message = "Record update cancelled!";

            if (recordUpdated)
            {
                message = "Record successfully updated!";
                sqlData.UpdateRecord("Records", recordId, newDate, newQuantity);
            }

            Console.WriteLine(message);
            UserInput.PressAnyKeyToContinue();
        }
        private void ConfirmAddHabit(string habitName, string unitName)
        {
            bool habitAdded = UserInput.GetUserConfirmation("delete");
            string message = "Habit delete cancelled!";

            if (habitAdded)
            {
                message = "New habit successfully added!";
                sqlData.InsertUnit(unitName);
                sqlData.InsertHabit(habitName, unitName);
            }

            Console.WriteLine(message);
            UserInput.PressAnyKeyToContinue();
        }



        private void RefreshConsoleWindow()
        {
            Console.Clear();
            Console.Write("\x1b[3J");
        }
        private void DrawHorizontalLine(int size, bool spaceBefore, bool spaceAfter)
        {
            if (size > Console.WindowWidth) size = Console.WindowWidth;

            if (spaceBefore) Console.WriteLine();

            for (int i = 1; i < size; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine();
            if (spaceAfter) Console.WriteLine();
        }
        private void PrintHabitsList()
        {
            var habits = sqlData.GetAllHabits();

            Console.WriteLine("Habits List");
            DrawHorizontalLine(65, false, true);

            foreach (var habit in habits.Select((value, i) => (value, i)))
            {
                Console.WriteLine($"\t{habit.i + 1,4}: {habit.value.HabitName}");
            }

            DrawHorizontalLine(65, true, true);
        }
        private void PrintUnitsList()
        {
            var units = sqlData.GetAllUnits();

            Console.WriteLine("Units List");
            DrawHorizontalLine(65, false, true);

            foreach (var unit in units.Select((value, i) => (value, i)))
            {
                Console.WriteLine($"\t{unit.i + 1}: {unit.value.UnitName}");
            }

            DrawHorizontalLine(65, true, true);
        }
        private void PrintRecordsList(List<RecordModel> sortedRecords, HabitModel habit)
        {
            Console.WriteLine($"Records List for {habit.HabitName}");
            DrawHorizontalLine(50, false, true);

            Console.WriteLine($"\t No.    Date          {habit.UnitName}");
            foreach (var record in sortedRecords.Select((value, i) => (value, i)))
            {
                Console.WriteLine($"\t{record.i + 1,4}:   {record.value.Date.ToString("yyyy-MM-dd"),10}    {record.value.Quantity,5:0.##}");
            }

            DrawHorizontalLine(40, true, true);
        }
        private void PrintTitleBar()
        {
            RefreshConsoleWindow();
            Console.WriteLine("Habit Tracker Application: Version 1.0");
            DrawHorizontalLine(Console.WindowWidth, false, true);
        }
        private void PrintGoodbyeMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Goodbye!");
            Console.WriteLine();
        }
        private void PrintHabitDeleteWarning(string habitName, int recordCount)
        {
            Console.WriteLine();
            Console.WriteLine($"WARNING: Deleting {habitName} will also delete the {recordCount} records associated with it. This cannot be undone.");
            Console.WriteLine();
        }
        private void PrintRecordUpdateMessage(bool dateChanged, bool quantityChanged, string originalDate, string newDate, double originalQuantity, double newQuantity, string unit)
        {
            string output = string.Empty;

            if (dateChanged && !quantityChanged)
            {
                output = $"Preparing to change date from {originalDate} to {newDate}.";
            }
            else if (!dateChanged && quantityChanged)
            {
                output = $"Preparing to change quantity from {originalQuantity:0.#########} {unit} to {newQuantity:0.#########} {unit}.";
            }
            else if (dateChanged && quantityChanged)
            {
                output = $"Preparing to change date from {originalDate} to {newDate} and changed quantity from {originalQuantity:0.#########} to {newQuantity:0.#########}.";
            }
            else
            {
                output = "No changes were made to the data!";
            }

            Console.WriteLine(output);
        }
        private void PrintRecordData(string action, double quantity, string unitName, string habitName, string date)
        {
            Console.WriteLine();
            Console.WriteLine($"{action} record for {quantity:0.#########} {unitName} of {habitName} on {date}");

        }
        private void PrintHabitData(string habitName, string unitName)
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine($"Preparing to add new habit, {habitName}, measured with units, {unitName}.");
        }
        private void PrintAddRecordInstruction(string habitName)
        {
            Console.WriteLine();
            Console.WriteLine($"Enter data for new record of {habitName}");
            Console.WriteLine();
        }
        private void PrintReport(HabitModel habit, string startDate, string endDate, ReportModel report)
        {
            Console.WriteLine($"Generating report for {habit.HabitName}");
            Console.WriteLine($"Unit: {habit.UnitName}");
            Console.WriteLine($"Start date: {startDate}");
            Console.WriteLine($"End Date: {endDate}");
            DrawHorizontalLine(65, false, true);
            Console.WriteLine($"Total record count: {report.RecordCount}");
            Console.WriteLine($"Number of days with record: {report.DayCount}");
            Console.WriteLine($"Sum of records: {report.Sum:0.###}");
            Console.WriteLine($"Daily Average: {report.DailyAverage:0.###}");
            Console.WriteLine($"Longest Streak: {report.StreakQuantity} {habit.UnitName} over {report.StreakDuration} days starting on {report.StreakStartDate.ToString("yyyy-MM-dd")}");

            UserInput.PressAnyKeyToContinue();
        }



        private List<RecordModel> GetSortedRecordsList(HabitModel habit)
        {
            var records = sqlData.GetAllRecords(habit.HabitId);

            return records.OrderBy(o => o.Date).ToList();
        }
        private HabitModel GetHabitFromList(string action)
        {
            var habitList = sqlData.GetAllHabits();

            int userSelection = (int)UserInput.GetNumberInput($"Enter ID of the Habit you wish to {action}: ", 1, habitList.Count());
            
            return habitList[userSelection - 1];
        }
        private RecordModel GetRecordFromList(List<RecordModel> sortedRecords, string action, HabitModel habit)
        {
            int userSelection = (int)UserInput.GetNumberInput($"Enter ID of the record you wish to {action}: ", 1, sortedRecords.Count());
            
            return sortedRecords[userSelection - 1];
        }



        private (bool dateChanged, DateTime newDate) GetUpdatedDateFromUser(DateTime originalDate)
        {
            var dateChanged = true;
            var newDate = UserInput.GetDateInput($"Enter the new date (or press enter to keep original date): ", "original");

            if (newDate == DateTime.MinValue)
            {
                newDate = originalDate;
                dateChanged = false;
            }

            return (dateChanged, newDate);
        }
        private (bool quantityChanged, double newQuantity) GetUpdatedQuantityFromUser(double originalQuantity)
        {
            var quantityChanged = true;
            var newQuantity = UserInput.GetNumberInput($"Enter the new quantity for the record (or press enter to keep original quantity): ", 1, Int32.MaxValue, true);

            if (newQuantity == Int32.MinValue)
            {
                newQuantity = originalQuantity;
                quantityChanged = false;
            }

            return (quantityChanged, newQuantity);
        }
        private (DateTime date, double quantity) GetRecordData(HabitModel habit)
        {
            var date = UserInput.GetDateInput($"Enter the date when {habit.HabitName} occurred (leave blank to add today's date): ", "today");
            var quantity = UserInput.GetNumberInput($"Enter the quantity to record (Unit = {habit.UnitName}): ", 1, Int32.MaxValue);

            return (date, quantity);
        }
        private string GetUnitNameFromList()
        {
            var unitName = string.Empty;
            var unitList = sqlData.GetAllUnits();

            int userSelection = (int)UserInput.GetNumberInput($"Enter ID of the unit you wish to use (or enter blank to create a new unit): ", 1, unitList.Count(), true);

            if (userSelection == Int32.MinValue)
            {
                unitName = GetNewUnitName();
            }
            else if (userSelection <= unitList.Count())
            {
                unitName = unitList[userSelection - 1].UnitName;
            }

            return unitName;
        }
        private string GetNewUnitName()
        {
            var unitName = string.Empty;
            var status = string.Empty;
            bool unitExists = true;

            while (unitExists == true)
            {
                unitName = GetNameFromUser("unit");
                unitExists = sqlData.CheckIfUnitExists(unitName);

                if (unitExists == true)
                {
                    Console.WriteLine("ERROR! Unit already exists");
                }
            }

            return unitName;
        }
        private string GetNewHabitName()
        {
            var habitName = string.Empty;
            var status = string.Empty;
            bool habitExists = true;

            while (habitExists == true)
            {
                habitName = GetNameFromUser("habit");
                habitExists = sqlData.CheckIfHabitExists(habitName);

                if (habitExists == true)
                {
                    Console.WriteLine("ERROR! Habit already exists");
                }
            }

            return habitName;
        }
        private string GetNameFromUser(string itemType)
        {
            string name = string.Empty;
            bool validName = false;

            while (validName == false)
            {
                name = UserInput.GetUserInput($"Enter the name of the new {itemType}: ");

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine($"INVALID {itemType.ToUpper()} NAME!");
                }
                else
                {
                    validName = true;
                }
            }

            return name;
        }
        private (DateTime, DateTime) GetDatesForPastYear()
        {
            var startDate = DateTime.Now.AddYears(-1);
            var endDate = DateTime.Now;

            return (startDate, endDate);
        }
        private (DateTime, DateTime) GetDatesForYearToDate()
        {
            var startDate = DateTime.Parse(DateTime.Now.Year.ToString() + "-01-01");
            var endDate = DateTime.Now;

            return (startDate, endDate);
        }
        private (DateTime, DateTime) GetDateForCustomRange()
        {
            var startDate = UserInput.GetDateInput("Enter start date: ");
            var endDate = new DateTime();
            bool firstTimeFlag = true;

            do
            {
                if (firstTimeFlag == false)
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR: End date must be on or after the startDate!");
                    Console.WriteLine();
                }
                endDate = UserInput.GetDateInput("Enter end date (leave blank for todays date): ", "today");
                firstTimeFlag = false;
            } while (endDate < startDate);

            return (startDate, endDate);
        }
    }
}
