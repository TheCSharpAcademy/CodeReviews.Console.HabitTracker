using System.Globalization;
namespace HKHemanthsharma.HabitLogger
{
    public class UserInputValidation
    {
        public static int EnterQuantity()
        {
            Console.WriteLine("Enter a numeric integer to describe the quantity of the habit");

            bool res = int.TryParse(Console.ReadLine(), out int quantity);
            while (!res)
            {
                Console.WriteLine("Please enter a valid integer!");
                res = int.TryParse(Console.ReadLine(), out quantity);
            }
            return quantity;
        }
        public static string EnterDescription()
        {
            Console.WriteLine("Enter the description for your habit: ");
            string description = Console.ReadLine();
            return description;

        }
        public static string EnterStatus()
        {
            Console.WriteLine("Enter The Current Status of your Habit Note: the allowed statuses are 'completed','inprogress','onhold'");
            List<string> validStatus = new List<string>() { "completed", "inprogress", "onhold" };
            string status = Console.ReadLine().ToLower();
            while (!(validStatus.Contains(status)))
            {
                Console.WriteLine("The only Valid states for a Habit are 'completed','inprogress','onhold'");
                status = Console.ReadLine().ToLower();
            }
            return status;

        }
        public static DateTime EnterDateofCreation()
        {
            DateTime entryDate;
            Console.WriteLine("Please enter the DateofCreation in 'MM/dd/yyyy' format or enter 't' for Today's Date");
            string userInput = Console.ReadLine();
            if (userInput.ToLower() == "t")
            {

                string formattedDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
                entryDate = Convert.ToDateTime(formattedDate);
            }
            else
            {
                bool res = DateTime.TryParseExact(userInput, "MM/dd/yyyy", null, DateTimeStyles.None, out entryDate);
                while (!res)
                {
                    Console.WriteLine("Please enter the Date in valid 'MM/dd/yyyy' format only");
                    res = DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", null, DateTimeStyles.None, out entryDate);
                }
               
            }
            return entryDate;

        }
    }
}
