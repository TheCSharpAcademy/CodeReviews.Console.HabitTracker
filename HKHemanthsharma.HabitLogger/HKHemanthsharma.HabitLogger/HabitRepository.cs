using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;

namespace HKHemanthsharma.HabitLogger
{
    public class HabitRepository
    {
        public static string dbconnection = "";
        public static string Dbtable;
        public HabitRepository(DatabaseManager dbmanager, string DbTable)
        {
            dbconnection = dbmanager.DbTableexists(DbTable);
            Dbtable = DbTable;
        }

        public void FetchAllRecords()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(dbconnection))
                {
                    string query = $"select * from {Dbtable}";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@habitDetails", Dbtable);
                        DataSet records = new DataSet();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(records, "habitDetails");
                        Console.WriteLine("The fetched Records: ");
                        foreach (DataColumn col in records.Tables["habitDetails"].Columns)
                        {
                            Console.Write(col.ColumnName + "  ");
                        }
                        Console.WriteLine(); // New line after column headers
                        Console.WriteLine("-------------------------------------------------------------------------------");

                        foreach (DataRow row in records.Tables["habitDetails"].Rows)
                        {
                            foreach (DataColumn col in records.Tables["habitDetails"].Columns)
                            {
                                Console.Write(row[col.ColumnName] + "  ");
                            }
                            Console.WriteLine();

                        }
                    }
                    Console.WriteLine("Enter any key to go to main menu");
                    Console.ReadLine();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                    string query = "select * from habitDetails where HabitName=@name";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", Habitname);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Console.WriteLine("The Habit already entered please enter the quantity to add to habit");
                            int quantity;
                            bool res = int.TryParse(Console.ReadLine(), out quantity);
                            while (!res)
                            {
                                Console.WriteLine("Please enter a valid integer!");
                                res = int.TryParse(Console.ReadLine(), out quantity);
                            }
                            int existingquantity = reader.GetInt32("quantity") + quantity;
                            cmd.Dispose();
                            reader.Close();
                            query = "update habitDetails set quantity=@existingquant where HabitName=@habitname";
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
                            Console.WriteLine("enter the Habit Description");
                            string description = Console.ReadLine();
                            Console.WriteLine("enter the Status");
                            string status = Console.ReadLine();
                            Console.WriteLine("enter the quantity:");
                            int quant;
                            bool res = int.TryParse(Console.ReadLine(), out quant);
                            while (!res)
                            {
                                Console.WriteLine("please enter a valid integer!");
                                res = int.TryParse(Console.ReadLine(), out quant);
                            }
                            Console.WriteLine("enter the DateofCreation or enter 't' for todays's date:");
                            string userinp = Console.ReadLine();
                            DateTime habitdate;
                            if (!(userinp == "t"))
                            {

                                bool result = DateTime.TryParse(Console.ReadLine(), out habitdate);
                                while (!result)
                                {
                                    Console.WriteLine("Please enter the valid Date in 'YYYY-mm-DD' format ");
                                    result = DateTime.TryParse(Console.ReadLine(), out habitdate);
                                }
                            }
                            else
                            {
                                habitdate = DateTime.Now;
                            }

                            query = $"insert into habitDetails(HabitName,Description,Status,quantity,DateofCreation) values(@habitName,@Description,@Status,@quantity,@date);";
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
            Console.WriteLine("Please enter the habit name to delete the record: ");
            string name = Console.ReadLine();
            using (SqlConnection conn = new SqlConnection(dbconnection))
            {
                conn.Open();
                string query = "delete from habitDetails where HabitName =@habitname";
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
                Console.WriteLine("Enter the name of the habit to be updated: ");
                string name = Console.ReadLine();


                using (SqlConnection conn = new SqlConnection(dbconnection))
                {
                    conn.Open();
                    string query = $"select count(*) from {Dbtable} where HabitName=@habitname ";

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
                                        query = "update habitDetails set HabitName=@parameter where HabitName=@olderhabitname";
                                        Console.WriteLine("Enter the HabitName value to be updated: ");
                                        parameter = Console.ReadLine();


                                        inp = false;
                                        break;
                                    case "2":
                                        query = "update habitDetails set Description=@parameter where HabitName=@olderhabitname";
                                        Console.WriteLine("Enter the Description value to be updated: ");
                                        parameter = Console.ReadLine();

                                        inp = false;
                                        break;
                                    case "3":
                                        query = "update habitDetails set Status=@parameter where HabitName=@olderhabitname";
                                        Console.WriteLine("Enter the Status value to be updated: ");
                                        parameter = Console.ReadLine();

                                        inp = false;
                                        break;
                                    case "4":
                                        query = "update habitDetails set quantity=@parameter where HabitName=@olderhabitname";
                                        Console.WriteLine("Enter the quantity value to be updated: ");
                                        if (int.TryParse(Console.ReadLine(), out int quantity))
                                        {
                                            parameter = quantity;
                                            inp = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Enter valid int type for quantiy!");
                                            parameter = int.Parse(Console.ReadLine());
                                            break;
                                        }
                                        break;
                                    case "5":
                                        query = "update habitDetails set DateofCreation=@parameter where HabitName=@olderhabitname";
                                        Console.WriteLine("Enter the DateofCreation value to be updated: ");
                                        if (DateTime.TryParse(Console.ReadLine(), out DateTime dateofcreation))
                                        {
                                            parameter = dateofcreation;
                                            inp = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Please enter the valid Date in 'YYYY-mm-DD' format ");
                                            parameter = DateTime.Parse(Console.ReadLine());
                                        }


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
