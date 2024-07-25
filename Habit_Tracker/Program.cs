
using Habit_Logger_Application;

UserHabit userHabit = new();
DatabaseServices databaseServices = new();
databaseServices.CreateDatabaseAndTable();
GetUserInput();


string userSelectionString;
void GetUserInput()
{
    Console.WriteLine("MAIN MENU\n\nWhat would you like to do?\n");
    Console.WriteLine(
        "Type 0 to Close the application.\n" +
        "Type 1 to View your Habit.\n" +
        "Type 2 to Create your Habit.\n" +
        "Type 3 to Delete your Habit.\n" +
        "Type 4 to Update your Habit.\n" +
        "------------------------------------------------\n\n"
        );
    userSelectionString = Console.ReadLine();
    ValidateUserInput(userSelectionString);
}


void ValidateUserInput(string userSelectionString)
{
    if (!int.TryParse(userSelectionString, out int userSelectionInt))
    {
        Console.Clear();
        Console.WriteLine("\n- Please enter a number between 0 and 4 -\n\n\n");
        GetUserInput();
    }
    else if (userSelectionInt > 0 || userSelectionInt < 5)
    {
        switch (userSelectionInt)
        {
            case 0: Console.Clear(); Console.WriteLine("\n\n\n ---- Goodbye! ---- \n\n\n"); break;
            case 1: ViewRecord(); break;
            case 2: InsertRecord(); break;
            case 3: DeleteRecord(); break;
            case 4: UpdateRecord(); break;
        }
    }
    else
    {
        GetUserInput();
    }
}


//when user selects 1
void ViewRecord()
{
    databaseServices.GetFromDatabase();
}


//when user selects 2
void InsertRecord()
{
    string habitNameInput;
    do
    {
        Console.WriteLine("Please enter a Name for your habit");
        habitNameInput = Console.ReadLine();

    } while (string.IsNullOrWhiteSpace(habitNameInput));

    userHabit.HabitName = habitNameInput;
    userHabit.HabitCounter = 0;

    databaseServices.PostToDatabase(userHabit);
    Console.WriteLine("\n\nYour habit has been recorded\n\n\n");

    GetUserInput();
}


//when user selects 3
void DeleteRecord()
{
    Console.WriteLine("Are you sure you want to delete your habit? Y/N");
    string deleteConfirmation = Console.ReadLine();

    if (deleteConfirmation.ToUpper() == "Y")
    {
        databaseServices.DeleteFromDatabase();
        Console.WriteLine("\n\n ---- Your habit has been deleted ---- \n\n\n");
    }

    GetUserInput();
}


//when user selects 4
void UpdateRecord()
{
    Console.WriteLine("What is your new Habit Count?");
    string userHabitCountInput = Console.ReadLine();

    if (int.TryParse(userHabitCountInput, out int result))
    {
        databaseServices.UpdateToDatabase(result);
        Console.WriteLine($"\n\nYour habit count has been updated to {result}");
        GetUserInput();
    }
    else
    {
        UpdateRecord();
    }
}