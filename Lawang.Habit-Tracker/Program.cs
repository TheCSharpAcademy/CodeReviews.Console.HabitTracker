using System.Text.RegularExpressions;
using Lawang.Habit_Tracker;
using Spectre.Console;

class Program
{
    static HabitTracker habitTracker = new HabitTracker();
    public static void Main(string[] args)
    {
        bool exitApp = false;
        do
        {
            ShowMenuOptions();
            string userSelection = SelectMenuOption();

            habitTracker.CreateTable();

            switch (userSelection)
            {
                case "0":
                    Console.Clear();
                    Console.WriteLine("Have a Nice Day!!");
                    exitApp = true;
                    break;
                case "1":
                    Console.Clear();
                    habitTracker.ViewAll();

                    Console.Write("\nPress Enter to Exit!!");
                    Console.ReadLine();

                    break;
                case "2":
                    var habitRecord = UserInputRecord();
                    if (habitRecord == null)
                    {
                        break;
                    }
                    int rowsInserted = habitTracker.Insert(habitRecord);

                    Console.WriteLine($"\nNumber of Row Inserted: {rowsInserted}");
                    Console.ReadLine();
                    break;

                case "3":
                    Console.Clear();
                    habitTracker.ViewAll();

                    int recordId = RecordId();

                    if (recordId == 0)
                    {
                        break;
                    }
                    //RowCount() => returns 1 if there is a row with given Id, O if there isn't 
                    int noOfRows = habitTracker.RowCount(recordId);

                    while (noOfRows == 0)
                    {
                        Console.WriteLine($"No record of the given Id {recordId} present");
                        recordId = RecordId();
                        if (recordId == 0)
                        {
                            break;
                        }
                        noOfRows = habitTracker.RowCount(recordId);
                    }

                    int rowsAffected = habitTracker.Delete(recordId);

                    Console.Clear();
                    Console.WriteLine($"{rowsAffected} Row With Id {recordId}: Deleted!");
                    Console.ReadLine();
                    break;

                case "4":
                    Console.Clear();
                    habitTracker.ViewAll();

                    int updateRecordId = RecordId();

                    if (updateRecordId == 0)
                    {
                        break;
                    }

                    int noOfRowsFound = habitTracker.RowCount(updateRecordId);

                    while (noOfRowsFound == 0)
                    {
                        Console.WriteLine($"No record of the given Id {updateRecordId} present");
                        recordId = RecordId();
                        if (recordId == 0)
                        {
                            break;
                        }
                        noOfRowsFound = habitTracker.RowCount(updateRecordId);
                    }


                    habitRecord = UpdateRecord();
                    int rowsUpdated = 0;
                    if (habitRecord is not null)
                        rowsUpdated = habitTracker.Update(updateRecordId, habitRecord);

                    if (rowsUpdated == 1)
                    {
                        Console.Clear();
                        Console.WriteLine($"{rowsUpdated} Row with Record Id {updateRecordId} Update");
                        Console.ReadLine();
                    }

                    break;

                case "5":
                    List<HabitRecord> habitRecords = habitTracker.GetHabitRecords();
                    GetReport(habitRecords);
                    break;

                default:
                    break;
            }
        } while (!exitApp);


    }

    static void ShowMenuOptions()
    {
        Console.Clear();
        Console.WriteLine("\n  -- MENU OPTIONS --");
        Console.WriteLine("\n  What would Like to do ?");
        Console.WriteLine("\n  Press '1' to View all Records.\n  Press '2' to Insert Record.\n  Press '3' to Delete Record.\n  Press '4' to Update Record.\n  Press '5' to show the Report.");
        Console.WriteLine("  Press '0' to exit the App.");
        Console.WriteLine("\n------------------------------------\n");


    }

    static string SelectMenuOption()
    {
        string? userResponse = null;
        do
        {
            userResponse = Console.ReadLine();

            if (userResponse != null && !Regex.IsMatch(userResponse, "^[0-5]$"))
            {
                Console.WriteLine("Please enter the valid value ranging from [0 - 5");
                userResponse = null;
            }
        } while (userResponse == null);

        return userResponse;
    }

    static DateOnly? AskDate()
    {
        Console.Write("Press '0' to exit OR Enter the Date in the Format 'dd-mm-yy': ");
        DateOnly date = new();
        string? userInput = Console.ReadLine();

        if (userInput == "0")
        {
            return null;
        }

        while (userInput != null && !DateOnly.TryParseExact(userInput, "dd-MM-yy", out date))
        {
            Console.Write("Please Input the Date in the Correct Format of 'dd-mm-yy': ");
            userInput = Console.ReadLine();
        }

        return date;
    }

    static double? AskDistance()
    {
        Console.Write("Enter the Distane You Ran: ");
        double distance = 0;
        string? userInput = Console.ReadLine();


        while (!double.TryParse(userInput, out distance))
        {
            Console.Write("Please Input the Distance in number: ");
            userInput = Console.ReadLine();
        }

        return distance;
    }
    static HabitRecord? UserInputRecord()
    {
        Console.Clear();

        DateOnly? date = AskDate();
        if (date is null)
        {
            return null;
        }

        double? distance = AskDistance();

        return new HabitRecord() { Date = date, Distance = distance };
    }

    static int RecordId()
    {
        int Id = 0;

        Console.WriteLine("\nEnter the 'Id' of the Record OR Press '0' to exit ");
        string? inputId = Console.ReadLine();


        while (!string.IsNullOrEmpty(inputId) && !int.TryParse(inputId, out Id))
        {
            Console.Write("\nPlease Give the valid Number: ");
            inputId = Console.ReadLine();
        }

        return Id;
    }

    static HabitRecord? UpdateRecord()
    {
        Console.WriteLine("\nPress '1' to Update Only Date Field.");
        Console.WriteLine("Press '2' to Update Only Distance Field");
        Console.WriteLine("Press '3' to Update All the Field of Record");

        string? userInput = Console.ReadLine();

        while (!string.IsNullOrEmpty(userInput) && !Regex.IsMatch(userInput, "^[1-3]$"))
        {
            Console.Write("Please Enter the Valid value from the range of 1 - 3: ");
            userInput = Console.ReadLine();
        }

        switch (userInput)
        {
            case "1":
                DateOnly? date = AskDate();
                return new HabitRecord() { Date = date };

            case "2":

                double? distance = AskDistance();
                return new HabitRecord() { Distance = distance };

            case "3":
                return UserInputRecord();
        }

        return null;
    }

    static void GetReport(List<HabitRecord> habitRecords)
    {
        Console.Clear();
        Console.WriteLine("Press '1' to get report on how many times a user ran in a given Year");
        Console.WriteLine("Press '2' to get total Distance Ran.");
        Console.WriteLine("Press '3' to get total Distance Ran From start date to end date");
        Console.WriteLine("Press '4' to get the month in which the user ran the most in the given Year");
        Console.WriteLine("Press '0' to Return to Main menu.");

        string? userInput = Console.ReadLine();

        while (!string.IsNullOrEmpty(userInput) && !Regex.IsMatch(userInput, @"^[0-4]$"))
        {
            Console.WriteLine("\nPlease Enter the valid value from [0 - 4]!!");
            userInput = Console.ReadLine();
        }

        switch (userInput)
        {
            case "1":
                Console.Clear();
                userInput = GetYear();


                double totalDistance = 0;
                int yearPresent = 0;
                //To make Table
                var table = new Table();
                table.Border = TableBorder.Ascii2;
                table.ShowRowSeparators = true;
                table.AddColumns(new string[] { "S.No", "Year", "Distance in Km" });

                foreach (var habit in habitRecords)
                {
                    if (habit.Date.HasValue && habit.Date.Value.Year == Convert.ToInt32(userInput))
                    {
                        yearPresent += 1;
                        totalDistance += habit.Distance.GetValueOrDefault();
                        table.AddRow(yearPresent.ToString(), habit.Date.Value.ToShortDateString(), habit.Distance.GetValueOrDefault().ToString());
                    }
                }
                Console.Clear();
                AnsiConsole.Write(table);
                if (yearPresent == 0)
                {
                    Console.WriteLine($"\nUser Didn't Ran in {userInput}");
                }
                else
                {
                    Console.WriteLine($"\n  User Ran {totalDistance} Km in Year {userInput}");
                }

                Console.ReadLine();


                break;
            case "2":
                Console.Clear();
                habitTracker.ViewAll();

                double totalDistanceRan = 0;
                foreach (var habit in habitRecords)
                {
                    totalDistanceRan += habit.Distance.GetValueOrDefault();
                }

                if (totalDistanceRan > 0)
                {
                    Console.WriteLine($"\nTotal Distance Ran: {totalDistanceRan} km");
                }
                else
                {
                    Console.WriteLine($"User hasn't started to Run");
                }

                Console.ReadLine();
                break;
            case "3":
                Console.Clear();
                DateOnly? startDate = AskDate();
                DateOnly? EndDate = AskDate();

                table = new Table();
                table.ShowRowSeparators = true;
                table.Border = TableBorder.Ascii2;

                table.AddColumns(new String[] { "Start Date", "End Date", "Total Distance Ran" });
                totalDistanceRan = 0;
                foreach (var habit in habitRecords)
                {
                    if (habit.Date.GetValueOrDefault() >= startDate.GetValueOrDefault() && habit.Date.GetValueOrDefault() <= EndDate.GetValueOrDefault())
                    {
                        totalDistanceRan += habit.Distance.GetValueOrDefault();
                    }
                }
                if (totalDistanceRan > 0)
                {
                    table.AddRow(startDate.GetValueOrDefault().ToString(), EndDate.GetValueOrDefault().ToString(), totalDistanceRan.ToString() + " km");
                    AnsiConsole.Write(table);
                }
                else
                {
                    Console.WriteLine($"No Distance Ran from {startDate.ToString()} to {EndDate.ToString()}");
                }
                Console.ReadLine();
                break;
            case "4":
                Console.Clear();
                userInput = GetYear();
                double[] monthTotal = new double[12];


                foreach (var habit in habitRecords)
                {
                    if (habit.Date.GetValueOrDefault().Year == Convert.ToInt32(userInput))
                    {
                        monthTotal[habit.Date.GetValueOrDefault().Month - 1] += habit.Distance.GetValueOrDefault();
                    }
                }

                int mostRanMonth = 0;
                double mostRanDistance = 0;

                for (int i = 0; i < monthTotal.Length; i++)
                {
                    if (mostRanDistance < monthTotal[i])
                    {
                        mostRanDistance = monthTotal[i];
                        mostRanMonth = i;
                    }
                }

                if (mostRanMonth == 0 && mostRanDistance == 0)
                {
                    Console.Clear(); 
                    Console.WriteLine($"User Didn't ran in Year {userInput}");
                }
                else
                {

                    table = new Table();
                    table.Border = TableBorder.Ascii2;
                    table.AddColumns(new string[] {"Year", "Most Ran Month", "Total Distance Coverd"});
                    table.AddRow(userInput?? "", DateOnly.MinValue.AddMonths(mostRanMonth).ToString("m").Split(' ')[0], mostRanDistance.ToString());

                    AnsiConsole.Write(table);
                }
                Console.ReadLine();


                break;
            case "0":
                break;
        }

    }

    static string? GetYear()
    {
        Console.WriteLine("Enter the Year Ranging From (1990 - 2099)");
        string? userInput = Console.ReadLine();

        while (!string.IsNullOrEmpty(userInput) && !Regex.IsMatch(userInput, @"^(19|20)\d{2}$"))
        {
            Console.WriteLine("Please enter the year in the forma YYYY");
            userInput = Console.ReadLine();
        }

        return userInput;
    }

}