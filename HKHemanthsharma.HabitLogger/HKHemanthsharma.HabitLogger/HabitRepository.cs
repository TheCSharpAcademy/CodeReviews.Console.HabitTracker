using System.Data;
using System.Data.SqlClient;
namespace HKHemanthsharma.HabitLogger
{
    public class HabitRepository
    {
        public static string dbconnection = "";
        public static string DbTable = "";
       
        public HabitRepository(DatabaseManager dbmanager, string Dbtable)
        {
            dbconnection=dbmanager.DbTableexists();
            DbTable = Dbtable;
        }

        public void FetchAllRecords()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(dbconnection))
                {
                    string query = $"select * from {DbTable}";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        DataSet records = new DataSet();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(records, "habitstable");
                        Console.WriteLine("The fetched Records: ");                    
                       if(records.Tables[0].Rows.Count ==0)
                        {
                            Console.WriteLine("currently the habits record is empty!");
                            Console.ReadLine();
                            return;
                        }
                        foreach (DataRow row in records.Tables[0].Rows)
                        {
                            foreach (DataColumn col in records.Tables[0].Columns)
                            {
                                Console.Write(row[col.ColumnName] + "  ");
                            }
                            Console.WriteLine();
                        }
                    }
                    Console.WriteLine("Enter any key to Continue");
                    Console.ReadLine();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
        public void InsertRecord()
        {
            Console.Clear();      
            Console.WriteLine("Enter the Habit: ");
            string Habitname = Console.ReadLine();
            Console.WriteLine("Checking if the Habit already exists in habitDetails...");
            try
            {
                using (SqlConnection conn = new SqlConnection(dbconnection))
                {
                    conn.Open();
                    string query = "SELECT * from habitDetails WHERE HabitName=@name";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", Habitname);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Console.WriteLine("The Habit already entered please enter the quantity to add to habit");
                            int quantity = UserInputValidation.EnterQuantity();                        
                            int existingquantity = reader.GetInt32("quantity") + quantity;
                            cmd.Dispose();
                            reader.Close();
                            query = "UPDATE habitDetails SET quantity=@existingquant WHERE HabitName=@habitname";
                            using (SqlCommand cmd1 = new SqlCommand(query, conn))
                            {
                                cmd1.Parameters.AddWithValue("@existingquant", existingquantity);
                                cmd1.Parameters.AddWithValue("@habitname", Habitname);
                                cmd1.ExecuteScalar();
                                Console.WriteLine("Habit has been updated!");
                                Console.WriteLine("Please enter any key to go back to main menu");
                                Console.ReadLine();

                            }
                        }
                        else
                        {
                            reader.Close();                         
                            string description = UserInputValidation.EnterDescription();                         
                            string status = UserInputValidation.EnterStatus();
                            int quant = UserInputValidation.EnterQuantity();
                            DateTime habitdate= UserInputValidation.EnterDateofCreation();                  
                            query = $"INSERT INTO habitDetails(HabitName,Description,Status,quantity,DateofCreation) VALUES(@habitName,@Description,@Status,@quantity,@date);";
                            cmd.Dispose();
                            using (SqlCommand cmd1 = new SqlCommand(query, conn))
                            {
                                cmd1.Parameters.AddWithValue("@habitName", Habitname);
                                cmd1.Parameters.AddWithValue("@Description", description);
                                cmd1.Parameters.AddWithValue("@Status", status);
                                cmd1.Parameters.AddWithValue("@quantity", quant);
                                cmd1.Parameters.AddWithValue("@date", habitdate);
                                int rowsaffected = cmd1.ExecuteNonQuery();
                                if (rowsaffected > 0)
                                {
                                    Console.WriteLine("habit is successfully inserted!");
                                    Console.WriteLine("Enter any key to go back to main menu");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    Console.WriteLine("Error in inserting! please check and try again");
                                    Console.ReadLine();
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Enter any key to go back to main menu and try again");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Enter any key to go back to main menu and try again");
                Console.ReadLine();
            }
        }
        public void DeleteRecord()
        {
            Console.Clear();
            Console.WriteLine("Here are the available records in the Table:");
            this.FetchAllRecords();
            Console.WriteLine("Please enter the habit name to delete the record: ");
            string name = Console.ReadLine();
            using (SqlConnection conn = new SqlConnection(dbconnection))
            {
                conn.Open();
                string query = "DELETE FROM habitDetails WHERE HabitName =@habitname";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@habitname", SqlDbType.NVarChar)).Value = name;
                    int deletecount = cmd.ExecuteNonQuery();
                    if (deletecount > 0)
                    {
                        Console.WriteLine($"{deletecount} habit has been deleted");
                    }
                    else
                    {
                        Console.WriteLine("there is some issue occured while deleting, could not delete");
                    }
                }
            }
            Console.WriteLine("Press any key to go to main menu!");
            Console.ReadLine();
        }
        public void UpdateRecord()
        {
            try
            {
                Console.Clear();
                Console.Clear();
                Console.WriteLine("Here are the available records in the Table:");
                this.FetchAllRecords();
                Console.WriteLine("Enter the name of the habit to be updated: ");
                string name = Console.ReadLine();


                using (SqlConnection conn = new SqlConnection(dbconnection))
                {
                    conn.Open();
                    string query = $"SELECT count(*) FROM {DbTable} WHERE HabitName=@habitname ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@habitname", name);
                        int result = (int)cmd.ExecuteScalar();
                        if (result > 0)
                        {

                            bool inp = true;
                            while (inp)
                            {
                                Console.WriteLine(@"Press 1 to update HabitName parameter:
 Press 2 to update Description parameter:
 Press 3 to update Status:(Note: Status could only have {'completed','inprogress','onhold'} parameter)
 Press 4 to Update quantity parameter:
 Press 5 to update DateofCreation parameter: ");
                               object parameter = null;
                                string userinp = Console.ReadLine();
                                List<string> validvalues = new List<string>() { "1", "2", "3", "4", "5" };
                                while (!validvalues.Contains(userinp))
                                {
                                    Console.WriteLine("Please enter Valid response of 1,2,3,4 or 5");
                                    userinp = Console.ReadLine();
                                }
                                switch (userinp)
                                {
                                    case "1":
                                        query = "UPDATE habitDetails SET HabitName=@parameter WHERE HabitName=@olderhabitname";
                                        Console.WriteLine("Enter the HabitName value to be updated: ");
                                        parameter = Console.ReadLine();
                                        inp = false;
                                        break;
                                    case "2":
                                        query = "UPDATE habitDetails SET Description=@parameter WHERE HabitName=@olderhabitname";
                                        parameter = UserInputValidation.EnterDescription();

                                        inp = false;
                                        break;
                                    case "3":
                                        query = "UPDATE habitDetails SET Status=@parameter WHERE HabitName=@olderhabitname";                                      
                                        parameter = UserInputValidation.EnterStatus(); 
                                        inp = false;
                                        break;
                                    case "4":
                                        query = "UPDATE habitDetails SET quantity=@parameter WHERE HabitName=@olderhabitname";
                                        parameter = UserInputValidation.EnterQuantity();  
                                        break;
                                    case "5":
                                        query = "UPDATE habitDetails SET DateofCreation=@parameter WHERE HabitName=@olderhabitname";
                                        parameter = UserInputValidation.EnterDateofCreation();
                                        break;
                                    default:
                                        Console.WriteLine("Please choose the valid option: ");
                                        break;
                                }
                                using (SqlCommand updatecmd = new SqlCommand(query, conn))
                                {
                                    updatecmd.Parameters.AddWithValue("@parameter", parameter);
                                    updatecmd.Parameters.AddWithValue("@olderhabitname", name);
                                    int updatedrow = updatecmd.ExecuteNonQuery();
                                    if (updatedrow != 0)
                                    {
                                        Console.WriteLine("row updated successfully!");
                                        Console.WriteLine("Press any key to go back to main menu!");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("There is an issue in updating no row has been updated");
                                        Console.WriteLine("Press any key to go back to main menu!");
                                        Console.ReadLine();
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please check the name of the habit and retry again!");
                            Console.WriteLine("Press any key to go back to main menu!");
                            Console.ReadLine();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("enter any key for main menu");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("enter any key for main menu");
                Console.ReadLine();
            }
        }
    }
}
