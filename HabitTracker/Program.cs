 using System.Globalization;
 using Microsoft.Data.Sqlite;
 
 internal class Program
 {
     static string connectionString = "Data Source=HabitTracker.db";

     public static void Main(string[] args)
     {
         using (var connection = new SqliteConnection(connectionString))
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

         bool exitApp = false;

         do
         {
             DrawMenu();
             var input = Console.ReadLine();
             getMenuInput(input);
             
             

         } while (!exitApp);

         Console.ReadLine();
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
        static void insertRecord()
     {
         var date = getDateInput();
         var quantity = getQuantityInput();

         using (var connection = new SqliteConnection(connectionString))
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
     }
        static string getDateInput()
         {
             int[] dashPos = { 2, 5 };
             bool validDate = false;
             string? date = "";
             
             do
             {
                 Console.WriteLine("Please enter the date in this format: dd-mm-yyyy");
                 date = Console.ReadLine();
                 
                 if (string.IsNullOrEmpty(date))
                 {
                     Console.WriteLine("Date cannot be empty");
                     continue;
                 }

                 if (date.Length != 10)
                 {
                     Console.WriteLine("ERROR: Invalid date format (Length is not good), please try again (dd-mm-yyyy) ");
                     continue;
                 }

                 bool validFormat = true;

                 foreach (int index in dashPos)
                 {
                     if (index >= 0 && index < date.Length)
                     {
                         if (date[index] != '-')
                         {
                             validFormat = false;
                             break;
                         }
                     }
                     else
                     {
                         validFormat = false;
                         break;
                     }
                 }

                 if (validFormat)
                 {
                     validDate = true;
                 }
                 else
                 {
                     Console.WriteLine("ERROR: Invalid date format (Wrong separator), please try again (dd-mm-yyyy)");
                 }
             } while (!validDate);
             return date;
         }
         
         static int getQuantityInput()
         {
             bool validInput = false;
             string input = "";
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

         static void deleteRecord()
         {
             var id = getIdInput();
             using (var connection = new SqliteConnection(connectionString))
             {
                 connection.Open();
                 var command = connection.CreateCommand();
                 // Delete from habit_tracker where id = 1
                 command.CommandText = @"DELETE FROM habit_tracker WHERE id = @id";
                 command.Parameters.AddWithValue("@id", id);
                 command.ExecuteNonQuery();
                 connection.Close();
             }

             Console.WriteLine($"{id}: Record deleted.");
         }

         static int getIdInput()
         {
             bool validInput = false;
             string input = "";
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

         static void updateRecord()
         {
             var id = getIdInput();
             var date = getDateInput();
             var quantity = getQuantityInput();

             using (var connection = new SqliteConnection(connectionString))
             {
                 connection.Open();
                 var command = connection.CreateCommand();
                 command.CommandText = @"UPDATE habit_tracker SET Date = @date, Quantity = @quantity  WHERE id = @id";
                 command.Parameters.AddWithValue("@id", id);
                 command.Parameters.AddWithValue("@date", date);
                 command.Parameters.AddWithValue("@quantity", quantity);
                 command.ExecuteNonQuery();
                 connection.Close();
             }
             
             Console.WriteLine("Record updated.");
         }

         static void viewAllRecord()
         {
             Console.Clear();
             
             using (var connection = new SqliteConnection(connectionString))
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
                           Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
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
                     Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - {dw.Quantity}");
                 }
                 Console.WriteLine("-------------------\n");
             }
         }

         public static void getMenuInput(string input)
         {
             switch (input)
             {
                 case "0":
                     //
                     break;
                 case "1":
                     insertRecord();
                     break;
                 case "2":
                     deleteRecord();
                     break;
                 case "3":
                     updateRecord();
                     break;
                 case "4":
                     viewAllRecord();
                     break;
             }
         }

         internal class Habit
         {
             public int Id { get; set; }
             public DateTime Date { get; set; }
             public int Quantity { get; set; }
         }
 }



