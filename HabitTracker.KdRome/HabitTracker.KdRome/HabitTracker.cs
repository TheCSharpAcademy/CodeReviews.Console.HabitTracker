using Microsoft.Data.Sqlite;

class HabitTracker {
     private static void Main(string[] args) {

          HabitTracker DrinkingWater = new HabitTracker();

          DrinkingWater.InitiateDataBase();
          DrinkingWater.MainMenu();

     }
     
     void InitiateDataBase() {

          string connectionString = @"Data Source=HabitTracker.db";

          using (var connection = new SqliteConnection(connectionString)) {

               connection.Open();

               var tableCmd = connection.CreateCommand();

               tableCmd.CommandText =
                         @"CREATE TABLE IF NOT EXISTS drinking_water (
                         Id INTEGER PRIMARY KEY AUTOINCREMENT,
                         Date TEXT,
                         Quantity INTEGER
                         )";

               tableCmd.ExecuteNonQuery(); //means that the data base will not return any values

               connection.Close();
          }
     }

     void MainMenu() {

          Console.Clear();

          Console.WriteLine("Type 0 to Close Application.");
          Console.WriteLine("Type 1 to View All Records.");
          Console.WriteLine("Type 2 to Insert Record.");
          Console.WriteLine("Type 3 to Delete Record.");
          Console.WriteLine("Type 4 to Update Record.");
          
          String UserOption = Console.ReadLine();

          InputCheck(Convert.ToInt32(UserOption));
     }

     void InputCheck(int UserOption) {

          if(UserOption < 0 || UserOption > 4) {
               Console.WriteLine("Please select one of the options");
               MainMenu();
          }
          
          switch(UserOption) {

               case 0://close application
                    Console.WriteLine("Good Bye");
                    System.Environment.Exit(0);
                    break;
               case 1://view all records
                    Console.WriteLine("Gathering ALL Records");
                    ViewRecords();
                    break;
               case 2://insert records
                    Console.WriteLine("Inserting Racord");
                    InsertRecord();
                    break;
               case 3://delete records
                    Console.WriteLine("Deleting Record");
                    DeleteRecord();
                    break;
               case 4://update records
                    Console.WriteLine("Updating Record");
                    UpdateRecord();
                    break;
               default:
                    Console.WriteLine("Please select one of the options");
                    MainMenu();
                    break;
          }
     }

     
     void ViewRecords() {

          Console.Clear();
          string connectionString = @"Data Source=HabitTracker.db";

          using (var connection = new SqliteConnection(connectionString)) {
               connection.Open();
               using (SqliteCommand command = new SqliteCommand($"SELECT * FROM drinking_water", connection)) {
                    // Execute the command and read til end of file
                    using (SqliteDataReader reader = command.ExecuteReader()) {
                         while (reader.Read()) {
                              // Access the columns by index or name
                              //Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["Name"]}");
                              Console.WriteLine($"Id: {reader["Id"]}, Date: {reader["Date"]}, Quantity: {reader["Quantity"]}");
                         }
                    }
               }
          }
     }
     
     void InsertRecord() {
          Console.Clear();

          Console.WriteLine("Please Enter Todays Date (MM/DD/YYYY)");
          string Date = Console.ReadLine();
          Console.WriteLine("Please Enter how many cups you drank today");
          int Num_Cups = Convert.ToInt32(Console.ReadLine());

          string connectionString = @"Data Source=HabitTracker.db";

          using (var connection = new SqliteConnection(connectionString)) {
               connection.Open();

               string query = "INSERT INTO drinking_water (Date, Quantity) VALUES (@Date, @Quantity)";
               using (SqliteCommand command = new SqliteCommand(query, connection)) {
                    // Adding parameters for the date and quantity
                    command.Parameters.AddWithValue("@Date", Date);
                    command.Parameters.AddWithValue("@Quantity", Num_Cups);

                    command.ExecuteNonQuery();
               }
               
               Console.WriteLine("Data Successfully Inserted");
               connection.Close();
          }
     }
     
     void DeleteRecord() { 
          

     }
     
     void UpdateRecord() {
     

     }
}