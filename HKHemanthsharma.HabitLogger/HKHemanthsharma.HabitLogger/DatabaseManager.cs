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
                    string query = $"if DB_ID('{DBName}') is null BEGIN Create DataBase {DBName}; END";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("The DataBase existance check complete!");
                    _connectionstring = _connectionstring + ";Initial Catalog=" + DBName;

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
        public string DbTableexists()
        {
            string dbconnectionstring = _connectionstring;
            using (SqlConnection conn = new SqlConnection(dbconnectionstring))
            {

                string query = @"if object_Id('habitDetails') is null
BEGIN
create table habitDetails(
HabitID int Not null Identity(1,1) primary key,
HabitName varchar(10),
Description varchar(100),
Status varchar(20) check(status in ('completed','inprogress','onhold')),
quantity int,
DateofCreation Date);
END";
                SqlCommand tablecreationcmd = new SqlCommand(query, conn);
                conn.Open();
                tablecreationcmd.ExecuteNonQuery();
                Console.WriteLine("habitDetails Table created!");
            }
            return dbconnectionstring;
        }

    }
}
