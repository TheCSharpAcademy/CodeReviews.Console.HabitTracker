using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Mo3ses
{
    public class DbConn
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        SQLiteDataReader dr;

        public void DbCreate()
        {
            if (!File.Exists("HabitTracker.db"))
            {
                SQLiteConnection.CreateFile("HabitTracker.db");

                string sql = @"CREATE TABLE HABIT(
                               ID INTEGER NOT NULL,
                               HABITNAME TEXT NOT NULL,
                               HABITMEASURE TEXT NOT NULL,
                               HABITVALUE INTEGER NOT NULL,
                               PRIMARY KEY(ID AUTOINCREMENT)
                               );
                                CREATE TABLE HABITTRACKER (
                            	ID	INTEGER NOT NULL,
                            	HABITID	INTEGER NOT NULL,
                            	DATETIME	DATE,
                            	PRIMARY KEY(ID AUTOINCREMENT),
                            	FOREIGN KEY(HABITID) REFERENCES HABIT(ID) ON DELETE CASCADE
                            );";

                conn = new SQLiteConnection("Data Source=HabitTracker.db");
                conn.Open();
                cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                conn = new SQLiteConnection("Data Source=HabitTracker.db");
            }
        }

        public void CreateHabit(string habitName, string habitMeasure, int habitValue)
        {
            cmd = new SQLiteCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = $"INSERT INTO HABIT(HABITNAME, HABITMEASURE, HABITVALUE) VALUES(@habitName, @habitMeasure, @habitValue)";
            cmd.Parameters.AddWithValue("@habitName", habitName);
            cmd.Parameters.AddWithValue("@habitMeasure", habitMeasure);
            cmd.Parameters.AddWithValue("@habitValue", habitValue);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public bool GetHabits()
        {
            DataTable tab = new DataTable();
            cmd = new SQLiteCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM HABIT";
            tab.Load(cmd.ExecuteReader());
            conn.Close();

            if (tab.Rows.Count > 0)
            {
                ConsoleTableBuilder.From(tab)
               //.WithFormat(ConsoleTableBuilderFormat.MarkDown)
               .WithTextAlignment(new Dictionary<int, TextAligntment> {
                    { 0, TextAligntment.Left },
                    { 1, TextAligntment.Left },
                    { 3, TextAligntment.Left },
                    { 100, TextAligntment.Left }
               })
               .WithMinLength(new Dictionary<int, int> {
                    { 1, 30 }
               })
               .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
               .WithTitle("HABITS LIST", ConsoleColor.Green, ConsoleColor.DarkGray, TextAligntment.Left)
               .WithFormatter(1, (text) =>
               {
                   return text.ToString().ToUpper().Replace(" ", "-") + " «";
               })
               .ExportAndWriteLine(TableAligntment.Left);
                return true;
            }
            else
            {
                Console.WriteLine("THERE IS NO HABITS, CREATE A NEW ONE.");
                return false;
                
            }
            
        }

        public void DeleteHabit(int id)
        {
            cmd = new SQLiteCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM HABIT WHERE ID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UpdateHabit(int id, string newHabitName, string newHabitMeasure, int newHabitValue)
        {
            cmd = new SQLiteCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE HABIT SET HABITNAME = @newHabitName, HABITMEASURE = @newHabitMeasure, HABITVALUE = @newHabitValue WHERE ID = @id";
            cmd.Parameters.AddWithValue("@newHabitName", newHabitName);
            cmd.Parameters.AddWithValue("@newHabitMeasure", newHabitMeasure);
            cmd.Parameters.AddWithValue("@newHabitValue", newHabitValue);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void TrackHabit(int id)
        {
            cmd = new SQLiteCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = $"INSERT INTO HABITTRACKER(HABITID, DATETIME) VALUES (@id, DATE())";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void HabitReport(int id)
        {
            DataTable reportTab = new DataTable();
            cmd = new SQLiteCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT
                                  H.HABITNAME,
                                  T.TIMESTRACKED,
                                  T.TOTALMEASURE,
                                    H.HABITMEASURE
                                FROM
                                  (	SELECT COUNT(HT.HABITID) TIMESTRACKED, (COUNT(HT.HABITID) * H.HABITVALUE) AS TOTALMEASURE
                                	FROM HABITTRACKER HT
                                	JOIN HABIT H ON H.ID = HT.HABITID
                                	WHERE HT.HABITID = @id
                                	GROUP BY H.HABITVALUE ) AS T
                                  JOIN HABIT H ON H.ID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            reportTab.Load(cmd.ExecuteReader());
            conn.Close();
            if (reportTab.Rows.Count > 0)
            {
                ConsoleTableBuilder.From(reportTab)
             //.WithFormat(ConsoleTableBuilderFormat.MarkDown)
             .WithTextAlignment(new Dictionary<int, TextAligntment> {
                    { 0, TextAligntment.Left },
                    { 1, TextAligntment.Left },
                    { 3, TextAligntment.Left },
                    { 100, TextAligntment.Left }
             })
             .WithMinLength(new Dictionary<int, int> {
                    { 1, 30 }
             })
             .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
             .WithTitle("HABIT REPORT STATUS", ConsoleColor.Green, ConsoleColor.DarkGray, TextAligntment.Left)
             .WithFormatter(1, (text) =>
             {
                 return text.ToString().ToUpper().Replace(" ", "-") + " «";
             })
             .ExportAndWriteLine(TableAligntment.Left);
            }
            else
            {
                Console.WriteLine("THERE IS NO DATE TRACKED IN THIS HABIT");
            }

        }

    }
}
