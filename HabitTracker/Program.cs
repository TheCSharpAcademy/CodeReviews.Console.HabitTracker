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

         } while (exitApp);

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

     static void closeApplication()
     {
         Environment.Exit(0);
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
             // TODO: get user input for which ID/record he wants to delete.
         }

         public static void getMenuInput(string input)
         {
             switch (input)
             {
                 case "0":
                     closeApplication();
                     break;
                 case "1":
                     insertRecord();
                     break;
                 case "2":
                     deleteRecord();
                     break;
                 case "3":
                     //Update();
                     break;
                 case "4":
                     //ViewAll();
                     break;
             }
         }
 }



