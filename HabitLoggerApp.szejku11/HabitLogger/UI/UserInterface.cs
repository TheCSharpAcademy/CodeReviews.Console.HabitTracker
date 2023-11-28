using HabitLogger.Database;
using HabitLogger.Models;
using System.Globalization;

namespace HabitLogger.UI
{
    public static class UserInterface
    {
        public static void Start()
        {
            bool stop = false;
            while (!stop)
            {
                Menu();
                Console.Write("Option: ");
                string decision = Console.ReadLine();

                switch (decision)
                {
                    case "0":
                        stop = true;
                        break;
                    case "1":
                        DisplayData();
                        Console.WriteLine();
                        Console.WriteLine("Click enter to go back");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case "2":
                        DatabaseHelper.InsertRow();
                        break;
                    case "3":
                        DatabaseHelper.UpdateRow();
                        break;
                    case "4":
                        DatabaseHelper.DeleteRow();
                        break;
                    default:
                        Console.Write("Unkown key pressed. Please click enter to try again.");
                        Console.ReadLine();
                        break;
                }

            }
        }
        public static (string?, int) GetInsertInfo()
        {
            string date = GetInputDate();
            if (string.IsNullOrEmpty(date))
            {
                return (null, 0);
            }

            int quantity = GetInputNumber("Please insert number of glasses (0 to go back): ", true);
            if (quantity == 0)
            {
                return (null, 0);
            }

            return (date, quantity);
        }
        public static (int, string?, int) GetUpdateInfo()
        {
            Console.Clear();
            DisplayData();

            List<DrinkingWaterModel> data = DatabaseHelper.AllData;
            List<int> ids = data.Select(x => x.Id).ToList();

            if (ids.Count == 0)
            {
                Console.Clear();
                Console.Write("No rows to update, click enter to go back");
                Console.ReadLine();
                return (0, null, 0);
            }

            int id = GetInputNumber("Please insert row ID you want to update (0 to go back): ", false);

            while (!ids.Contains(id))
            {
                if (id == 0)
                {
                    return (0, null, 0);
                }
                Console.WriteLine("Row ID is not in the database, please click enter to try again.");
                Console.ReadLine();
                Console.Clear();
                DisplayData();
                id = GetInputNumber("Please insert row ID you want to update (0 to go back): ", false);
            }

            (string date, int quantity) = GetInsertInfo();
            return (id, date, quantity);
        }
        public static int GetDeleteInfo()
        {
            Console.Clear();
            DisplayData();

            List<DrinkingWaterModel> data = DatabaseHelper.AllData;
            List<int> ids = data.Select(x => x.Id).ToList();

            if (ids.Count == 0)
            {
                Console.Clear();
                Console.Write("No rows to delete, click enter to go back");
                Console.ReadLine();
                return 0;
            }

            int id = GetInputNumber("Please insert row ID you want to delete (0 to go back): ", false);

            while (!ids.Contains(id))
            {
                if (id == 0)
                {
                    return 0;
                }
                Console.WriteLine("Row ID is not in the database, please click enter to try again.");
                Console.ReadLine();
                Console.Clear();
                DisplayData();
                id = GetInputNumber("Please insert row ID you want to delete (0 to go back): ", false);
            }

            return id;
        }

        private static void Menu()
        {
            Console.Clear();
            Console.WriteLine("Habit Logger Application");
            Console.WriteLine("\nMain menu");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("1 - View all records");
            Console.WriteLine("2 - Add new record");
            Console.WriteLine("3 - Update record");
            Console.WriteLine("4 - Delete record");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("-------------------------------------------");
        }
        private static void DisplayData()
        {
            Console.Clear();
            List<DrinkingWaterModel> data = DatabaseHelper.AllData;
            Console.WriteLine("-------------------------------------------");

            if (data.Count == 0)
            {
                Console.WriteLine("No rows found");
            }
            else
            {
                foreach (var row in data)
                {
                    Console.WriteLine($"Row {row.Id}  |  {row.Date.ToString("dd-MM-yyyy")}  |  Quantity: {row.Quantity}");
                }
            }
            Console.WriteLine("-------------------------------------------");
        }
        private static int GetInputNumber(string text, bool shouldClearConsole)
        {
            if (shouldClearConsole)
            {
                Console.Clear();
            }

            Console.Write(text);
            string numberText = Console.ReadLine();
            int number;
            int.TryParse(numberText, out number);

            while (number <= 0)
            {
                if (number == 0)
                {
                    return 0;
                }
                Console.Clear();
                Console.Write("Invalid input, " + text.ToLower());
                numberText = Console.ReadLine();
                int.TryParse(numberText, out number);
            }

            return number;
        }
        private static string GetInputDate()
        {
            Console.Clear();
            Console.Write("Please insert date in format dd-mm-yyyy (0 to go back): ");
            string date = Console.ReadLine();

            if (date == "0")
            {
                return null;
            }

            while (!DateTime.TryParseExact(date, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))

            {
                Console.Clear();
                Console.Write("Invalid input, please insert date in format dd-mm-yyyy: ");
                date = Console.ReadLine();
            }

            return date;
        }

    }
}
