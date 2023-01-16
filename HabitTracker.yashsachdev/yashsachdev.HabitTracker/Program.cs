// See https://aka.ms/new-console-template for more information
using Microsoft.Data.Sqlite;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


StringBuilder sb = new StringBuilder(1024); //capacity 
var con = new SqliteConnection("Data Source=hello.db");
sb.AppendLine("Connection String:"+con.ConnectionString); // prints a string value used to connect to Database.
sb.AppendLine("State:"+con.State.ToString()); // State of the connection.
sb.AppendLine("Connection Timeout"+con.ConnectionTimeout.ToString());
sb.AppendLine("Database"+con.Database);
sb.AppendLine("Data source"+con.DataSource);
sb.AppendLine("Server Version"+con.ServerVersion);

Console.WriteLine(sb);