class EntryHelper
{
    HabitEntry Delimiter(string userEntry)
    {
        var isValidEntry = false;
        while (!isValidEntry)
            try
            {
                var splitEntry = userEntry.Split(";");
                var habitName = splitEntry[0];
                var date = splitEntry[1];
                HabitEntry entry = new();
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Your entry was not split correctly with the delimiter. Try again.");
            }
            catch (System.Exception)
            {
                isValidEntry = false;
                Console.WriteLine("Invalid entry. Try again.");
            }
    }
}
