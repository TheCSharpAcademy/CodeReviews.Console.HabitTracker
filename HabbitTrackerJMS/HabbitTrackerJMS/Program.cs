using HabbitTrackerJMS;
using System.Globalization;

SQLitePCL.Batteries.Init();
DataHelpers dataHelpers = new DataHelpers();
string id;
string quantity;
int idInteger = 0;
int quantityInteger = 0;
DateTime parsedDate;
bool closeApp = false;
List<string> tableNames = dataHelpers.GetTableNames();

while (closeApp == false)
{
    Console.WriteLine(@$"Welcome to the Habits Tracker App!

Current habit selected {dataHelpers.TableName}!

    Menu:
    c - reate new habit
    o - change to another habbit
    i - insert habit log
    u - update habit log
    t - total of actions perfomed
    d - delete habit log
    v - view habits
    0 - close the app");

    string userInput = Console.ReadLine();

    switch (userInput)
    {
        case "c":
            Console.WriteLine("Enter the name of the new habit: ");
            dataHelpers.TableName = Console.ReadLine();
            dataHelpers.CreateTable(dataHelpers.TableName);
            Console.WriteLine($"Table '{dataHelpers.TableName}' created!");
            Console.ReadLine();
            break;

        case "o":
            string userINput;
            Console.WriteLine("Habits list:");
            foreach (string tableName in tableNames)
            {
                Console.WriteLine(tableName);
            }
            Console.WriteLine("Type exactly the name of the habit you would like to modify");
            userInput = Console.ReadLine();
            while(!tableNames.Contains(userInput))
            {
                Console.WriteLine("The input should be EXACTLY the same as an existing habit (pay attention to lower and upper case)! Write another input!");
                userInput = Console.ReadLine();
            }
            dataHelpers.TableName = userInput;
            break;

        case "i":
            parsedDate = GetValidDate();
            quantityInteger = GetQuantity();
            dataHelpers.InsertInfo(parsedDate, quantityInteger);
            break;

        case "u":
            int rows = ViewInfo();
            if (rows > 0)
            {
                bool idExists = false;
                while(idExists == false)
                {
                    idInteger = GetValidID();
                    idExists = dataHelpers.CheckIDExists(idInteger);
                }
                parsedDate = GetValidDate();
                quantityInteger = GetQuantity();
                dataHelpers.UpdateInfo(idInteger, parsedDate, quantityInteger);
            }
            else { Console.WriteLine("No records exist!"); }
            Console.ReadLine();
            break;

        case "t":
            int total = dataHelpers.GetTotal();
            Console.WriteLine($"The total of 'actions' performed under the habit: {dataHelpers.TableName} was {total}!");
            Console.ReadLine();
            break;

        case "d":
            rows = ViewInfo();
            if(rows > 0)
            {
                idInteger = GetValidID();   
                dataHelpers.DeleteInfo(idInteger);
            }
            else { Console.WriteLine("No Records exist!"); }
            Console.ReadLine();
            break;

        case "v":
            rows = ViewInfo();
            if (rows == 0) { Console.WriteLine("No Records exist!"); }
            Console.ReadLine();
            break;

        case "0":
            closeApp = true;
            break;

        default:
            Console.WriteLine("Invalid input!");
            Console.ReadLine();
            break;
    }
    Console.Clear();
}

int ViewInfo()
{
    int countRows = 0;
    var habits = dataHelpers.ViewInfo();
    foreach (var habit in habits)
    {
        Console.WriteLine($"Id: {habit.Id}, Date: {habit.Date}, Quantity: {habit.Quantity}");
        countRows++;
    }
    return countRows;
}

int GetValidID()
{
    bool isValid = false;
    while (isValid == false)
    {
        Console.WriteLine("Enter Id of the record to update: ");
        id = Console.ReadLine();
        if (string.IsNullOrEmpty(id))
        {
            Console.WriteLine("Input is null!");
        }
        else
        {
            if (int.TryParse(id, out idInteger))
            {
                isValid = true;
            }
            else
            {
                Console.WriteLine("Invalid input! The ID must be a valid integer.");
            }
        }
    }
        return idInteger;
}

DateTime GetValidDate()
{
    bool isValid = false;
    string format = "yyyy-MM-dd";
    DateTime parsedDate = DateTime.MinValue;

    while (!isValid)
    {
        Console.WriteLine("Enter date (yyyy-mm-dd): ");
        string input = Console.ReadLine();

        if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            isValid = true;
        }
        else
        {
            Console.WriteLine("Invalid date format.");
        }
    }

    return parsedDate;
}

int GetQuantity()
{
    bool isValid = false;
    while (isValid == false)
    {
        Console.WriteLine("Enter Quantity of the record to update: ");
        quantity = Console.ReadLine();

        if (string.IsNullOrEmpty(quantity))
        {
            Console.WriteLine("Input is null!");
        }
        else
        {
            if (int.TryParse(quantity, out quantityInteger))
            {
                isValid = true;
            }
            else
            {
                Console.WriteLine("Invalid input! The ID must be a valid integer.");
            }
        }
    }
    return quantityInteger;
}





