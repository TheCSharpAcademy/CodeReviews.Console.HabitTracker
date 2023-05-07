namespace ohshie.HabitTracker;
using Microsoft.Data.Sqlite;

public class DbOperations
{
    public readonly string Dbconnection = @"Data Source=habit_tracker.db";

    public void CreateDb(string habitName, string quantityName)
    {
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            
            tableCommand.CommandText = 
                $@"CREATE TABLE IF NOT EXISTS {habitName} (Id INTEGER PRIMARY KEY AUTOINCREMENT,Date TEXT, '{quantityName}' INTEGER)";

            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    public void PrintAllTables()
    {
        HabitOperator.AllHabitsList.Clear();

        Console.Clear();
        
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText =
                @"SELECT NAME FROM sqlite_master WHERE TYPE = 'table' AND NAME NOT LIKE 'sqlite_%';";

            SqliteDataReader reader = tableCommand.ExecuteReader();
            
            int habitId = 1;
            Console.WriteLine("Current Habits:");
            while (reader.Read())
            {
                HabitOperator.AllHabitsList.Add(new TrackedHabits()
                {
                    HabitName = reader.GetString(0),
                    Id = habitId++
                });
            }

            foreach (var habit in HabitOperator.AllHabitsList)
            {
                Console.WriteLine($"{habit.Id}. {habit.HabitName}");
            }
            
            connection.Close();
        }
    }

    public int PrintTable(string habitName)
    {
        Console.Clear();
        
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $"SELECT * FROM {habitName}";

            List<TrackedHabits> habitData = new List<TrackedHabits>();

            SqliteDataReader reader = tableCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    habitData.Add(
                        new TrackedHabits()
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetString(1),
                            Quantity = reader.GetInt32(2),
                            AmountType = GetHabitQuantifier(habitName)
                        });
                }
            }
            else
            {
                Console.WriteLine("Database empty!\n" +
                                  "Press any key to go back");
                Console.ReadKey(true);
                return -1;
            }
            
            connection.Close();

            Console.WriteLine("######################");
            foreach (var entry in habitData)
            {
                Console.WriteLine($"{entry.Id}. On {entry.Date}. You've read {entry.Quantity} {entry.AmountType}");
            }
            Console.WriteLine("######################");
        }

        return 1;
    }

    public void DeleteEntry(string dbName)
    {
        Console.Clear();
        
        PrintTable(dbName);
        
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            
            var tableCommand = connection.CreateCommand();
            int invalidRow = 0;
            int entryId = 0;
            
            while (invalidRow == 0)
            {
                entryId = Program.GetNumberFromUser("Id of entry you want to delete");
                if (entryId == -1) return;

                tableCommand.CommandText = $"DELETE FROM {dbName} WHERE id = '{entryId}'";

                invalidRow = tableCommand.ExecuteNonQuery();
                if (invalidRow == 0) Console.WriteLine("Looks like there you've chosen entry that does not exist, or there are no entries left");
            }
            
            connection.Close();
            
            Console.WriteLine($"Entry with Id {entryId} was deleted");
        }
    }

    public void UpdateEntry(string dbName)
    {
        Console.Clear();
        
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            int entryId = 0;
            int checkEntryExist = 0;
            
            while (checkEntryExist == 0)
            {
                PrintTable(dbName);
                entryId = Program.GetNumberFromUser("Id of entry you want to edit");
                if (entryId == -1) return;
                
                tableCommand.CommandText = $"SELECT exists(SELECT 1 FROM {dbName} WHERE Id = {entryId})";
                
                checkEntryExist = Convert.ToInt32(tableCommand.ExecuteScalar());

                if (checkEntryExist == 0)
                {
                    Console.WriteLine("Looks like that entry does not exist!\n" +
                                      "Press enter to try again");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            
            string newDate = Program.DateChooser();
            if (newDate == "X") return;
   
            int newAmount = Program.GetNumberFromUser("new amount");
            if (newAmount == -1) return;

            string habitQuantifier = GetHabitQuantifier(dbName);
            tableCommand.CommandText = $"UPDATE {dbName} SET date = '{newDate}', {habitQuantifier} = {newAmount} WHERE Id = {entryId}";

            tableCommand.ExecuteNonQuery();
                
            connection.Close();
        }
        Console.Clear();
    }
    
    public void AddEntry()
    {
        Console.Clear();

        PrintAllTables();
        
        HabitOperator habitOperator = new();
        
        TrackedHabits habit = habitOperator.ChooseHabit();
        if (habit == null) return;
        
        habit.Date = Program.DateChooser();
        if (habit.Date == "X") return;

        habit.Quantity = Program.GetNumberFromUser("amount you've done");
        if (habit.Quantity == -1) return;

        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            string habitQuantifier = GetHabitQuantifier(habit.HabitName);
            
            tableCommand.CommandText = $"INSERT INTO {habit.HabitName} (date, {habitQuantifier}) " +
                                       $"VALUES ('{habit.Date}', {habit.Quantity})";

            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
        
        Console.Clear();
    }

    private string GetHabitQuantifier(string habitName)
    {
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $@"PRAGMA table_info ({habitName});";

            var reader = tableCommand.ExecuteReader();

            List<string> collumnNames = new List<string>();
            while (reader.Read())
            {
                collumnNames.Add(reader.GetString(1));
            }
            
            connection.Close();
            
            return collumnNames.Last();
        }
    }

    public int DbExistCheck()
    {
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @"SELECT count(*)
                                            FROM sqlite_master
                                            WHERE TYPE = 'table'
                                            AND NAME NOT LIKE 'sqlite_%'";

            int habitsAmount = Convert.ToInt32(tableCommand.ExecuteScalar());
            
            connection.Close();
            
            return habitsAmount;
        }
    }
}