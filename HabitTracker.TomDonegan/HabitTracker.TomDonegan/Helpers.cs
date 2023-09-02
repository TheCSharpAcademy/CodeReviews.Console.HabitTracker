namespace HabitTracker.TomDonegan
{
    internal class Helpers
    {
        internal static int NumberValidation(string requiresValidation)
        {
            int checkedNumber;

            while (!int.TryParse(requiresValidation, out checkedNumber)) 
            {
                Console.WriteLine("Please enter a valid number.");
                Console.ReadLine();
            }

            return checkedNumber;
        }
    }
}
