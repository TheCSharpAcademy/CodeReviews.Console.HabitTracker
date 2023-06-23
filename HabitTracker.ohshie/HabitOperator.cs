using Microsoft.Data.Sqlite;

namespace ohshie.HabitTracker;

public class HabitOperator
{
    public static List<TrackedHabits> AllHabitsList = new List<TrackedHabits>();
    
    public void CreateHabit()
    {
        DbOperations dbOperations = new DbOperations();
        
        string userHabit = Program.GetStringFromUser("habit name");
        if (userHabit == "#") return;
        
        string habitQuantifier = Program.GetStringFromUser("how to quantify that habit? E.g. Liters, Pages, Km etc.");
        
        dbOperations.CreateDb(userHabit,habitQuantifier);
    }

    public void DeleteHabit()
    {
        Console.Clear();

        DbOperations dbOperations = new DbOperations();

        using (var connection = new SqliteConnection(dbOperations.Dbconnection))
        {
            dbOperations.PrintAllTables();
            
            connection.Open();
            var tableCommand = connection.CreateCommand();
            int invalidRow = 0;
            int entryId = 0;
            string habitName = "";
            
            while (invalidRow == 0)
            {
                if (AllHabitsList.Count == 0)
                {
                    Console.WriteLine("There are no habits to delete. Create one first.\n" +
                                      "any key to return");
                    Console.ReadKey(true);
                    return;
                }
                
                entryId = Program.GetNumberFromUser("Id of habit you want to delete");
                if (entryId == -1) return;

                foreach (var habit in AllHabitsList)
                {
                    if (habit.Id == entryId)
                    {
                        habitName = habit.HabitName;
                    }
                }
                
                if (habitName != null)
                {
                    tableCommand.CommandText = $"DROP TABLE IF EXISTS {habitName}"; 
                    tableCommand.ExecuteNonQuery();

                    invalidRow = 1;
                }
                else
                {
                    Console.WriteLine("Looks like there you've chosen entry that does not exist, or there are no entries left");
                }
            }
            
            Console.WriteLine($"Entry with Id {entryId} was deleted");
            connection.Close();
        }
    }

    public TrackedHabits ChooseHabit()
    {
        DbOperations dbOperations = new DbOperations();
        
        bool habitindexExist = false;

        while (habitindexExist == false)
        {
            if (dbOperations.DbExistCheck() == 0)
            {
                Console.WriteLine("No habit exist yet, create one first.\n" +
                                  "press any key to continue");
                Console.ReadKey(true);
                return null;
            }
            
            int userChoice = Program.GetNumberFromUser("index of a habit");
            if (userChoice == -1) return null;
            
            foreach (var habit in AllHabitsList)
            {
                if (habit.Id == userChoice)
                {
                    return habit;
                }
            }

            Console.WriteLine("Looks like index you've entered is incorrect, press any key to try again");
            Console.ReadKey(true);
        }
        
        return null; // not sure how to get rid of it.
    }
}

public class TrackedHabits
{
    public int Id { get; set; }
    public string HabitName { get; set; }
    public string Date { get; set; }
    public string AmountType { get; set; }
    public int Quantity { get; set; }
}