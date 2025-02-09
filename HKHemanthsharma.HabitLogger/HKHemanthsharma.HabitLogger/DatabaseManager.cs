using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace HKHemanthsharma.HabitLogger
{
    public class DatabaseManager
    {
        private static string _connectionstring = "Data Source=DESKTOP-3Q1SB5N\\MSSQLSERVER2022;Integrated Security=True";
        public DatabaseManager()
        {

        }
        public DatabaseManager(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        public void DBExists(string DBName)
        {
            try
            {

                string DbConnection = _connectionstring;
                using (SqlConnection conn = new SqlConnection(DbConnection))
                {
                    conn.Open();
                    string query = "select count(*) from sys.databases where name=@database";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@database", SqlDbType.NVarChar)).Value = DBName;
                        int returnvalue = (int)cmd.ExecuteScalar();
                        if (returnvalue > 0)
                        {
                            Console.WriteLine("The Database is already exists");

                        }
                        else
                        {
                            query = $"Create database {DBName}";
                            using (SqlCommand cmd1 = new SqlCommand(query, conn))
                            {
                                cmd1.ExecuteNonQuery();
                            }

                            Console.WriteLine("The Database is created for you!");


                        }
                    }


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
        public string DbTableexists(string database)
        {
            this.DBExists(database);
            string dbconnectionstring = _connectionstring + ";Initial Catalog=" + database;

            using (SqlConnection conn = new SqlConnection(dbconnectionstring))
            {
                conn.Open();
                string query = "select count(*) from INFORMATION_SCHEMA.TABLES where TABLE_NAME='habitDetails'";
                SqlCommand cmd = new SqlCommand(query, conn);
                int result = (int)cmd.ExecuteScalar();
                if (result == 0)
                {
                    Console.WriteLine("Habit Details Table doesn't exist and we are creating one for you");
                    query = @"create table habitDetails(
                    HabitID int Not null Identity(1,1) primary key,
                    HabitName varchar(10), 
                    Description varchar(100), 
                    Status varchar(20) check(status in ('completed','inprogress','onhold')),
                    quantity int,
                    DateofCreation Date);";
                    SqlCommand tablecreationcmd = new SqlCommand(query, conn);
                    tablecreationcmd.ExecuteNonQuery();
                    Console.WriteLine("habitDetails Table created!");


                }
                else
                {
                    Console.WriteLine("HabitDetails table already existed!");
                }
            }
            return dbconnectionstring;
        }

    }
}
