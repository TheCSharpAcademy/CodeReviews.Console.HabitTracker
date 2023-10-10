namespace HabitTracker.UgniusFalze
{
    internal class Display
    {
        public static void DisplayIntroMessage()
        {
            Console.WriteLine("Hello, welcome to coffee adiction tracking application, where you can track how many cups of coffee have you drank per day.");
        }

        public static void DisplayMenu()
        {
            Console.WriteLine("Please choose what kind of operation you would like to do:");
            Console.WriteLine("1. View all the current logged habbits.");
            Console.WriteLine("2. Insert a new daily habbit.");
            Console.WriteLine("3. Delete a single logged habbit.");
            Console.WriteLine("4. Update a selected logged habbit.");
            Console.WriteLine("0. Exit the application");
        }

        public static void DisplaySeperator(){
            Console.WriteLine("----------------------------------");
        }

        public static void DisplayAllHabbits(List<Habbit> habbits)
        {
            string quantityText = "Cups of coffee";
            int idPadding = habbits.Max(h => h.Id.ToString().Length);
            int quantityPadding = habbits.Max(h => h.Quantity.ToString().Length);
            idPadding = idPadding > 2 ? idPadding : 2;
            quantityPadding = quantityPadding > quantityText.Length ? quantityPadding : quantityText.Length;
            Console.WriteLine("Here are the current logged habbits:");
            Console.WriteLine("Id".PadRight(idPadding) + " " + quantityText.PadRight(quantityPadding) + " Date");
            habbits.ForEach(h => Console.WriteLine(h.Id.ToString().PadRight(idPadding) + " " + h.Quantity.ToString().PadRight(quantityPadding) + " " + h.Date)); //Maybe override ToString method on Habbit class
        }

        public static void DisplayIncorrectFormat()
        {
            Console.WriteLine("Incorrect date format, please enter the correct date format: " + SQLLiteOperations.GetDateFormat() + " .");
        }

        public static void DisplayIncorrectNumber()
        {
            Console.WriteLine("Please enter non negative and correct number.");
        }

        public static void DisplayIncorrectMenuOption()
        {
            Console.WriteLine("Please enter correct menu option.");
        }

        public static void DisplayUpdateMenu()
        {
            Console.WriteLine("Please choose which field you would like to update:");
            Console.WriteLine("1. Quantity");
            Console.WriteLine("2. Date");
            Console.WriteLine("0. Go back to main menu.");
        }

        public static void DisplayQuantityInput()
        {
            Console.WriteLine("Please enter how many cups of coffee you drank on selected day:");
        }

        public static void DisplayDateInput()
        {
            Console.WriteLine("Please enter a correct date using format: dd/MM/yyyy.");
        }
        
        public static void DisplayDeleteHabbit()
        {
            Console.WriteLine("Please choose which habbit you want to delete");
        }

        public static void IOException()
        {
            Console.WriteLine("Console input error, try enter your input again.");
        }

        public static void SQLLiteException()
        {
            Console.WriteLine("SQLlite error, please try again.");
        }

        public static void IncorrectId()
        {
            Console.WriteLine("Check if your id is correct and try again.");
        }

        public static void DisplayWhichRecordToUpdate()
        {
            Console.WriteLine("Please choose which record you want to update.");
        }
    }
}
