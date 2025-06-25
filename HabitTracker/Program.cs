using Microsoft.Data.Sqlite;
 
 internal class Program
 {
     static string _connectionString = "Data Source=HabitTracker.db";
     static bool _exitApp;

     public static void Main(string[] args)
     {
         using (var connection = new SqliteConnection(_connectionString))
         {
             connection.Open();
             var command = connection.CreateCommand();
             command.CommandText = @"CREATE TABLE IF NOT EXISTS habit_tracker (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT,
    Quantity INTEGER
)";

             command.ExecuteNonQuery();
             connection.Close();
         }

         do
         {
             DrawMenu();
             var input = Console.ReadLine();
             GetMenuInput(input);

         } while (!_exitApp);
     }
         static void DrawMenu()
     {
         Console.Clear();
         Console.WriteLine("MAIN MENU");
         Console.WriteLine("----------------------------------");
         Console.WriteLine("Select an option:");
         Console.WriteLine("Type 0 - Exit");
         Console.WriteLine("Type 1 - Insert Record");
         Console.WriteLine("Type 2 - Delete Record");
         Console.WriteLine("Type 3 - Update Record");
         Console.WriteLine("Type 4 - View All Records");
         Console.WriteLine("----------------------------------");
     }
         static void InsertRecord()
     {
         var date = GetDateInput();
         var quantity = GetQuantityInput();
         

         using (var connection = new SqliteConnection(_connectionString))
         {
             connection.Open();
             var command = connection.CreateCommand();
             command.CommandText = @"INSERT INTO habit_tracker (Date, Quantity) VALUES (@date, @quantity)";
             command.Parameters.AddWithValue("@date", date);
             command.Parameters.AddWithValue("@quantity", quantity);
             command.ExecuteNonQuery();
             connection.Close();
         }
         
         Console.WriteLine("Record inserted");
         Console.WriteLine("Press any key to continue...");
         Console.ReadLine();
     }
         static bool ContainsOnlyNumbersAndDash(string text)
     {
         if (string.IsNullOrEmpty(text))
         {
             return false; 
         }

         foreach (char c in text)
         {
             if (!char.IsDigit(c) && c != '-')
             {
                 return false;
             }
         }
         return true;
     }
         static bool CheckSeperator(string text)
     {
         int[] dashPos = { 2, 5 };
         
         foreach (int index in dashPos)
         {
             if (index >= 0 && index < text.Length)
             {
                 if (text[index] != '-')
                 {
                     return false;
                 }
             }
             else
             {
                 return false;
             }
         }
         return true;
     }
         static string GetDateInput()
         {

             bool validDate = false;
             string? date;
             do
             {
                 Console.WriteLine("Please enter the date in this format: dd-mm-yyyy");
                 date = Console.ReadLine();
                 

                 if (CheckSeperator(date) && ContainsOnlyNumbersAndDash(date))
                 {
                     validDate = true;
                 }
                 else
                 {
                     Console.WriteLine("ERROR: Invalid date format, please try again (dd-mm-yyyy)");
                 }
             } while (!validDate);
             return date;
         }
         static int GetQuantityInput()
         {
             bool validInput = false;
             string input;
             int quantity = 0;
             do
             {
                 Console.WriteLine("Please enter the quantity:");
                 input = Console.ReadLine();

                 if (string.IsNullOrEmpty(input)) Console.WriteLine("ERROR: Quantity cannot be empty");
                 else if (!int.TryParse(input, out quantity)) Console.WriteLine("ERROR: Quantity must be a number");
                 else validInput = true;
                 
             } while (!validInput);
                
             return quantity;
         }
         static void DeleteRecord()
         {
             var id = GetIdInput();
             using (var connection = new SqliteConnection(_connectionString))
             {
                 connection.Open();
                 var command = connection.CreateCommand();
                 command.CommandText = @"DELETE FROM habit_tracker WHERE id = @id";
                 command.Parameters.AddWithValue("@id", id);
                 
                 int rowCount = command.ExecuteNonQuery();

                 if (rowCount == 0)
                 {
                     Console.WriteLine($"Record with ID {id} was not found.");
                     DeleteRecord();
                 }
                 else
                 {
                     Console.WriteLine($"{id}: Record deleted.");
                 }
                 
                 connection.Close();
             }
             
             Console.WriteLine("Press any key to continue...");
             Console.ReadLine();
         }
         static int GetIdInput()
         {
             bool validInput = false;
             string input;
             int id = 0;
             do
             {
                 Console.WriteLine("Please enter the ID:");
                 input = Console.ReadLine();

                 if (string.IsNullOrEmpty(input)) Console.WriteLine("ERROR: ID cannot be empty");
                 else if (!int.TryParse(input, out id)) Console.WriteLine("ERROR: ID must be a number");
                 else validInput = true;
                 
             } while (!validInput);
                
             return id;
         }
         static void UpdateRecord()
         {
             bool idFound = false;
             var id = 0;
             using (var connection = new SqliteConnection(_connectionString))
             {
                 connection.Open();
                 
                 while (!idFound)
                 {
                     id = GetIdInput();
                     
                     var checkCmd = connection.CreateCommand();
                     checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_tracker WHERE id = @id)";
                     checkCmd.Parameters.AddWithValue("@id", id);
                     int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                     if (checkQuery == 0)
                     {
                         Console.WriteLine($"Record with ID {id} was not found.");
                     }
                     else
                     {
                         idFound = true;
                     }
                 }
                 
                 var command = connection.CreateCommand();
                 var date = GetDateInput();
                 var quantity = GetQuantityInput();
                 command.CommandText = @"UPDATE habit_tracker SET Date = @date, Quantity = @quantity  WHERE id = @id";
                 command.Parameters.AddWithValue("@id", id);
                 command.Parameters.AddWithValue("@date", date);
                 command.Parameters.AddWithValue("@quantity", quantity);
                 command.ExecuteNonQuery();
                 Console.WriteLine("Record updated");

                 connection.Close();
             }
             
             Console.WriteLine("Press any key to continue...");
             Console.ReadLine();
         }
         static void ViewAllRecord()
         {
             Console.Clear();
             
             using (var connection = new SqliteConnection(_connectionString))
             {
                 connection.Open();
                 var command = connection.CreateCommand();
                 command.CommandText = @"SELECT * FROM habit_tracker";

                 List<Habit> tableData = new();
                 
                 SqliteDataReader reader = command.ExecuteReader();

                 if (reader.HasRows)
                 {
                     while (reader.Read())
                     {
                         tableData.Add(new Habit
                         {
                           Id = reader.GetInt32(0),
                           Date = reader.GetString(1), 
                           Quantity = reader.GetInt32(2)
                         });
                     }
                 }
                 else
                 {
                     Console.WriteLine("No records found.");
                 }
                 connection.Close();
                 
                 Console.WriteLine("-------------------");
                 Console.WriteLine("ID | Date | Quantity");
                 foreach (var dw in tableData)
                 {
                     Console.WriteLine($"{dw.Id} - {dw.Date} - {dw.Quantity}");
                 }
                 Console.WriteLine("-------------------\n");
             }

             Console.WriteLine("Press any key to continue...");
             Console.ReadLine();
         }

         public static void GetMenuInput(string input)
         {
             switch (input)
             {
                 case "0":
                     Console.WriteLine("Bye!");
                     _exitApp = true;
                     break;
                 case "1":
                     InsertRecord();
                     break;
                 case "2":
                     DeleteRecord();
                     break;
                 case "3":
                     UpdateRecord();
                     break;
                 case "4":
                     ViewAllRecord();
                     break;
             }
         }

         internal class Habit
         {
             public int Id { get; set; }
             public string Date { get; set; }
             public int Quantity { get; set; }
         }
 }



