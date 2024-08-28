using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite; 


namespace habitTracker{
    class Program{
        
        static string connectionString = @"Data Source=habit_tracker.db"; 
        
        static void Main(string[] args)
        {
            InitializeDataBase(); 
            HabitSelection(); 
        }

        private static void InitializeDataBase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT, 
                        Quantity INTEGER 
                        )";

                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS running_count (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT, 
                        Distance INTEGER 
                        )";

                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS programming_hours (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT, 
                        Hours INTEGER 
                        )";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        static void HabitSelection(){
            Console.Clear(); 
            Console.WriteLine("Welcome to the Habit Tracker. Please, select which habit you want to track"); 
            Console.WriteLine(@"
    Type 0 - to Close Application.
    Type 1 - to Track Drinking water.
    Type 2 - to Track Running counting.
    Type 3 - to Track Programming learning hours.
    Type 4 - To Create a new Custom tracker.
    Type 5 - To Track the Custom tracker.
            ");
            
            string? readResult = Console.ReadLine(); 

            switch(readResult){
                case "0":
                    Environment.Exit(0); 
                    break; 
                case "1":
                    GetUserInput(new DrinkingWater()); 
                    break; 
                case "2": 
                    GetUserInput(new RunningCount());
                    break;
                case "3":
                    GetUserInput(new ProgrammingHours());
                    break; 
                case "4":
                    CreateCustomHabit(); 
                    break; 
                case "5":
                    TrackCustomHabit(); 
                    break; 
                default:
                    Console.WriteLine("Invalid key, please try again"); 
                    HabitSelection(); 
                    break; 
            }
        }
        
        static void CreateCustomHabit(){
            Console.Clear(); 
            
            Console.WriteLine("Which habit you want to track?"); 
            string? habitName = Console.ReadLine();

            while(String.IsNullOrEmpty(habitName)){
                Console.WriteLine("Please enter a valid input"); 
                Console.ReadLine(); 
            }

            Console.WriteLine("Which unit you want to use?"); 
            string? measureUnit = Console.ReadLine(); 
            while(String.IsNullOrEmpty(measureUnit)){
                Console.WriteLine("Please enter a valid input"); 
                Console.ReadLine(); 
            }

            SaveCustomHabit(habitName, measureUnit);

            Console.WriteLine($"Custom habit '{habitName}' with measure unit '{measureUnit}' has been created.");
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
            HabitSelection(); 
        }

        static void SaveCustomHabit(string habitName, string measureUnit){
            string tableName = $"habit_{habitName.ToLower().Replace(" ", "_")}";

            using (var connection = new SqliteConnection(connectionString)){
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"CREATE TABLE IF NOT EXISTS custom_habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT, 
                        MeasureUnit TEXT, 
                        TableName TEXT
                    )";

                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText = 
                    @$"CREATE TABLE IF NOT EXISTS {tableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                        Date TEXT, 
                        Value INTEGER
                    )";
                tableCmd.ExecuteNonQuery(); 

                tableCmd.CommandText = 
                    "INSERT INTO custom_habits (Name, MeasureUnit, TableName) VALUES (@name, @measureUnit, @tableName)"; 
                tableCmd.Parameters.AddWithValue("@name", habitName); 
                tableCmd.Parameters.AddWithValue("@measureUnit", measureUnit); 
                tableCmd.Parameters.AddWithValue("@tableName", tableName); 
                tableCmd.ExecuteNonQuery();
            }
        }

        static List<CustomHabit> ListCustomHabits(){
            List<CustomHabit> customHabits = new List<CustomHabit>();
            using(var connection = new SqliteConnection(connectionString)){
                connection.Open();
                var tableCmd = connection.CreateCommand(); 
                tableCmd.CommandText = "SELECT Id, Name, MeasureUnit FROM custom_habits"; 

                using(var reader = tableCmd.ExecuteReader()){
                    while(reader.Read()){
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1); 
                        string measure = reader.GetString(2); 
                        string tableName = $"habit_{name.ToLower().Replace(" ", "_")}"; 
                        
                        customHabits.Add(new CustomHabit(name, measure){
                            Id = id, 
                            TableName = tableName
                        }); 
                    }
                }
            }

            return customHabits; 
        }

        static void TrackCustomHabit(){
            Console.Clear(); 
            var customHabits = ListCustomHabits();

            if(customHabits.Count == 0){
                Console.WriteLine("No Custom Habits found. Press any key to go to menu");
                Console.ReadKey(); 
                HabitSelection(); 
            }

            Console.WriteLine("Select a custom habit to track"); 
            
            for(int i = 0; i < customHabits.Count; i++){
                Console.WriteLine($"{i + 1}. {customHabits[i].Name}"); 
            }

            int selection; 

            while(!int.TryParse(Console.ReadLine(), out selection) || selection < 1 || selection > customHabits.Count){
                Console.WriteLine("Invalid selection. Please Try again"); 
            }

            var selectedHabit = customHabits[selection - 1]; 
            GetUserInput(selectedHabit); 
        }

        static void GetUserInput(Habit habit){
            bool closeApp = false; 

            while(closeApp == false){
                Console.Clear();

                Console.WriteLine(@$"What would you like to do?
                
    Type 0 - to Return to the Habit Selection Menu.
    Type 1 - to View All Records.
    Type 2 - to Insert Records.
    Type 3 - to Delete Records. 
    Type 4 - to Update Records.
                
                "); 

                string? readResult = Console.ReadLine(); 

                switch(readResult){
                    case "0":
                        HabitSelection(); 
                        break; 

                    case "1":
                        ViewRecords(habit); 
                        break; 

                    case "2":
                        InsertRecords(habit); 
                        break; 

                    case "3":
                        DeleteRecords(habit); 
                        break; 

                    case "4":
                        UpdateRecords(habit); 
                        break; 
                    
                    default:
                        Console.WriteLine("Invalid Command. Please type a valid number"); 
                        break; 
                }
            }
        }
            
        static void ViewRecords(Habit habit){
            Console.Clear(); 

            string tableName = GetHabitName(habit); 

            using (var connection = new SqliteConnection(connectionString)){
                connection.Open(); 

                var tableCmd = connection.CreateCommand(); 
                tableCmd.CommandText = 
                    $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
                var result = tableCmd.ExecuteScalar();

                if(result == null){
                    Console.WriteLine($"No records found for {habit.GetType().Name}");
                    Console.WriteLine("Press any key to return to the main menu");
                    Console.ReadKey();
                }
                
                tableCmd.CommandText = 
                    $"SELECT * FROM {GetHabitName(habit)}"; 
                List<Habit> tableData = new List<Habit>();

                var reader = tableCmd.ExecuteReader(); 

                if(reader.HasRows){
                    while(reader.Read()){
                        if(habit is DrinkingWater){
                            tableData.Add(new DrinkingWater{
                                Id = reader.GetInt32(0), 
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")), 
                                Quantity = reader.GetInt32(2) 
                            }); 
                        } else if(habit is RunningCount){
                            tableData.Add(new RunningCount{
                                Id = reader.GetInt32(0), 
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                                Distance = reader.GetInt32(2)
                            });
                        } else if(habit is ProgrammingHours){
                            tableData.Add(new ProgrammingHours{
                                Id = reader.GetInt32(0), 
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                                Hours = reader.GetInt32(2)
                            });
                        } else if(habit is CustomHabit customHabit){
                            tableData.Add(new CustomHabit(customHabit.Name, customHabit.Measure){
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")), 
                                MeasureValue = reader.GetInt32(2),
                                TableName = customHabit.TableName

                            }); 
                        } else{
                            Console.WriteLine("Unknown habit type"); 
                        }
                    }

                } else{
                    Console.WriteLine("No rows found");  
                }

                connection.Close(); 

                Console.WriteLine("-----------------------------------------------"); 
                foreach (var item in tableData){
                    if(item is DrinkingWater){
                        var drinkingWater = (DrinkingWater)item;
                        Console.WriteLine($"{drinkingWater.Id} - {drinkingWater.Date.ToString("dd-MM-yyyy")} - Quantity: {drinkingWater.Quantity}"); 
                    }
                    else if(item is RunningCount){
                        var runningCount = (RunningCount)item;
                        Console.WriteLine($"{runningCount.Id} - {runningCount.Date.ToString("dd-MM-yyyy")} - Distance: {runningCount.Distance}"); 
                    }
                    else if(item is ProgrammingHours){
                        var programmingHours = (ProgrammingHours)item;
                        Console.WriteLine($"{programmingHours.Id} - {programmingHours.Date.ToString("dd-MM-yyyy")} - Hours: {programmingHours.Hours}"); 
                    }
                    else if(item is CustomHabit){
                        var customHabit = (CustomHabit)item;
                        Console.WriteLine($"{customHabit.Id} - {customHabit.Date.ToString("dd-MM-yyyy")} - {customHabit.Measure}: {customHabit.MeasureValue}"); 
                    }
                    else{
                        Console.WriteLine("Uknown habit type"); 
                    }
                         
                }
                Console.WriteLine("-----------------------------------------------"); 
                Console.WriteLine("Press any key to proceed"); 
                Console.ReadKey(); 
            }
        }
    
        static void InsertRecords(Habit habit){
            Console.Clear(); 
            string date = GetDateInput(); 
            int flexibleNumber = 0; 

            if(habit is DrinkingWater){
                flexibleNumber = GetNumberInput("Please, insert the number of glasses or other measures of your choice (no decimals allowed). Press 0 to return to main menu");
            } 
            else if(habit is RunningCount){
                flexibleNumber = GetNumberInput("Please, insert the distance of your choice (no decimals allowed). Press 0 to return to main menu"); 
            }
            else if(habit is ProgrammingHours){
                flexibleNumber = GetNumberInput("Please, insert the hours of your choice (no decimals allowed). Press 0 to return to main menu"); 
            }
            else if(habit is CustomHabit customHabit){
                flexibleNumber = GetNumberInput($"Please, insert the {customHabit.Measure} of your choice (no decimals allowed). Press 0 to return to main menu"); 
            }

            using (var connection = new SqliteConnection(connectionString)){
                connection.Open(); 
                var tableCmd = connection.CreateCommand(); 
                if(habit is DrinkingWater){
                    tableCmd.CommandText = 
                    $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', '{flexibleNumber}')";
                }
                else if(habit is RunningCount){
                    tableCmd.CommandText =
                    $"INSERT INTO running_count(date, distance) VALUES('{date}', '{flexibleNumber}')";
                }
                else if(habit is ProgrammingHours){
                    tableCmd.CommandText =
                    $"INSERT INTO programming_hours(date, hours) VALUES('{date}', '{flexibleNumber}')";
                }
                else if(habit is CustomHabit customHabit){
                    tableCmd.CommandText =
                    $"INSERT INTO {customHabit.TableName}(date, Value) VALUES ('{date}', '{flexibleNumber}')";
                }
               
                tableCmd.ExecuteNonQuery();
                connection.Close();   
            }
        }

        static void DeleteRecords(Habit habit){
            Console.Clear();
            ViewRecords(habit); 

            var recordId = GetNumberInput("Please, enter the ID of the record you want to delete"); 

            using (var connection = new SqliteConnection(connectionString)){
                connection.Open(); 
                var tableCmd = connection.CreateCommand(); 

                if (habit is DrinkingWater){
                    tableCmd.CommandText = 
                    $"DELETE from drinking_water WHERE Id = '{recordId}'"; 
                }
                else if (habit is RunningCount){
                    tableCmd.CommandText = 
                    $"DELETE from running_count WHERE Id = '{recordId}'"; 
                }
                else if (habit is ProgrammingHours){
                    tableCmd.CommandText = 
                    $"DELETE from programming_hours WHERE Id = '{recordId}'"; 
                }
                else if(habit is CustomHabit customHabit){
                    tableCmd.CommandText =
                    $"DELETE from {customHabit.TableName} WHERE Id = '{recordId}'"; 
                }

                int rowCount = tableCmd.ExecuteNonQuery(); 

                if(rowCount == 0){
                    Console.WriteLine($"Record with nº id: {recordId} doesn't exist"); 
                    DeleteRecords(habit); 
                }
            }

            Console.WriteLine($"Record with nº id: {recordId} has been deleted"); 
    
            HabitSelection();

        }
        
        static void UpdateRecords(Habit habit){
            Console.Clear();
            ViewRecords(habit); 

            var recordId = GetNumberInput($"Please, type the nº id of the record you want to update"); 
            using (var connection = new SqliteConnection(connectionString)){
                connection.Open(); 

                var checkCmd = connection.CreateCommand(); 
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {GetHabitName(habit)} WHERE Id = {recordId})"; 
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar()); 

                while(checkQuery == 0){
                    Console.WriteLine($"Record with nº id: {recordId} doesn't exist"); 
                    Console.ReadKey(); 
                    connection.Close(); 
                }

                string date = GetDateInput();
                int flexibleNumber = 0; 

                if(habit is DrinkingWater){
                    flexibleNumber = GetNumberInput("Please, insert the number of glasses or other measures of your choice (no decimals allowed). Press 0 to return to main menu");
                } 
                else if(habit is RunningCount){
                    flexibleNumber = GetNumberInput("Please, insert the distance of your choice (no decimals allowed). Press 0 to return to main menu"); 
                }
                else if(habit is ProgrammingHours){
                    flexibleNumber = GetNumberInput("Please, insert the hours of your choice (no decimals allowed). Press 0 to return to main menu");
                }
                else if(habit is CustomHabit customHabit){
                    flexibleNumber = GetNumberInput($"Please, insert the {customHabit.Measure} of your choice (no decimals allowed). Press 0 to return to main menu"); 
                }
                var tableCmd = connection.CreateCommand(); 

                if(habit is DrinkingWater){
                    tableCmd.CommandText =
                    $"UPDATE drinking_water SET date = '{date}', quantity = '{flexibleNumber}' WHERE Id = '{recordId}'";
                }
                else if(habit is RunningCount){
                    tableCmd.CommandText =
                    $"UPDATE running_count SET date = '{date}', distance = '{flexibleNumber}' WHERE Id = '{recordId}'";
                }
                else if(habit is ProgrammingHours){
                    tableCmd.CommandText =
                    $"UPDATE programming_hours SET date = '{date}', hours = '{flexibleNumber}' WHERE Id = '{recordId}'";
                }
                else if(habit is CustomHabit customHabit){
                    tableCmd.CommandText =
                    $"UPDATE {customHabit.TableName} SET date = '{date}', Value = '{flexibleNumber}' WHERE Id = '{recordId}'"; 
                }
                 
                tableCmd.ExecuteNonQuery(); 
                connection.Close();
            }
        }

        static string GetDateInput(){ 
            Console.WriteLine("Please, insert a date (Format: dd-mm-yyyy). Press 0 to return to main menu"); 
            string? dateInput = Console.ReadLine(); 

            if(dateInput == "0"){
                HabitSelection(); 
            }
            
            while(!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _)){
                Console.WriteLine("Invalid date format (Format: dd-mm-yyyy). Press 0 to return to main menu"); 
                dateInput = Console.ReadLine(); 
            }

            return dateInput; 
        }

        static int GetNumberInput(string message){
            Console.WriteLine(message); 
            string numberInput = Console.ReadLine();  

            if(numberInput == "0"){
                HabitSelection(); 
            }
           
            while(String.IsNullOrEmpty(numberInput)){
                Console.WriteLine("Invalid input, Please insert the nº of id of the record you want to update"); 
                numberInput = Console.ReadLine();
            }

            while(!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0){
                Console.WriteLine("Invalid number. Try again"); 
                numberInput = Console.ReadLine(); 
            }

            int finalInput = Convert.ToInt32(numberInput); 
            return finalInput; 
        }

        static string GetHabitName(Habit habit){
            if(habit is DrinkingWater){
                return "drinking_water"; 
            }
            else if(habit is RunningCount){
                return "running_count"; 
            }
            else if(habit is ProgrammingHours){
                return "programming_hours"; 
            }
            else if(habit is CustomHabit customHabit){
                return customHabit.TableName; 
            }
            else{
                Console.WriteLine("Habit not recognized");
                return string.Empty; 
            }
        }
    }
}

public class Habit{
    public int Id { get; set; }
    public DateTime Date { get; set; }
}

public class DrinkingWater: Habit{
    public int Quantity { get; set; } 
}

public class RunningCount: Habit{
    public int Distance { get; set; } 
}

public class ProgrammingHours: Habit{
    public int Hours { get; set; } 
}

public class CustomHabit: Habit{

    public string Name { get; set; }
    public string Measure { get; set; }
    public int MeasureValue { get; set; }
    public string TableName { get; set; }

    public CustomHabit(string name, string measure){
        Name = name; 
        Measure = measure;
        TableName = $"habit_{name.ToLower().Replace(" ", "_")}"; 
    }

}
