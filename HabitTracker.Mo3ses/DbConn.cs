using ConsoleTableExt;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;


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
                                QUANTITY INTEGER NOT NULL,
                            	DATESTART	DATE,
                                DATEEND	DATE,
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

        public bool HasHabit(int id)
        {
            DataTable tab = new DataTable();
            cmd = new SQLiteCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM HABIT WHERE ID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            tab.Load(cmd.ExecuteReader());
            conn.Close();

            if (tab.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                Console.WriteLine("HABIT NOT FOUND");
                return false;

            }
        }
        public void TrackHabit(int id)
        {
            try
            {
                Console.Write("ENTER THE START DATE (YYYY-MM-DD): ");
                string dateStart = Console.ReadLine();
                Console.Write("ENTER THE END DATE (YYYY-MM-DD): ");
                string dateEnd = Console.ReadLine();
                Console.Write("ENTER THE QUANTITY OF TIMES THAT YOU DID THE HABIT: ");
                string qtdHabit = Console.ReadLine();
                int intQtdHabit = ProgramHelpers.ValidateInputs(qtdHabit);
                var date1 = DateTime.ParseExact(dateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var date2 = DateTime.ParseExact(dateEnd, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                cmd = new SQLiteCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = $"INSERT INTO HABITTRACKER(HABITID, QUANTITY, DATESTART, DATEEND) VALUES (@id, @quantity, @dateStart, @dateEnd)";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@quantity", intQtdHabit);
                cmd.Parameters.AddWithValue("@dateStart", date1);
                cmd.Parameters.AddWithValue("@dateEnd", date2);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine("INSERT A VALID DATE EX: 2023-07-05");
            }
            
        }

        public void HabitReportInterval(int id, string dateStart, string dateEnd)
        
        {
            try
            {
                var date1 = DateTime.ParseExact(dateStart.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var date2 = DateTime.ParseExact(dateEnd.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DataTable reportTab = new DataTable();
                Console.WriteLine(dateStart.ToString());
                cmd = new SQLiteCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT
                                  H.HABITNAME,
                                  T.TIMESTRACKED,
                                  T.TOTALMEASURE,
                                    H.HABITMEASURE
                                FROM
                                  (	SELECT COUNT(HT.HABITID) TIMESTRACKED, (SUM(HT.QUANTITY) * H.HABITVALUE) AS TOTALMEASURE
                                	FROM HABITTRACKER HT
                                	JOIN HABIT H ON H.ID = HT.HABITID
                                	WHERE HT.HABITID = @id
									AND DATESTART >= @dateStart AND DATEEND <= @dateEnd
                                	GROUP BY H.HABITVALUE ) AS T
									JOIN HABIT H ON H.ID = @id
                                    ";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@dateStart", dateStart.ToString());
                cmd.Parameters.AddWithValue("@dateEnd", dateEnd.ToString());
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
                 .WithTitle($"HABIT REPORT STATUS  ({(date2 - date1).TotalDays} days)", ConsoleColor.Green, ConsoleColor.DarkGray, TextAligntment.Left)
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
            catch (Exception e)
            {
                Console.WriteLine("INSERT A VALID DATE EX: 2023-07-05");
            }
           
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
                                  (	SELECT COUNT(HT.HABITID) TIMESTRACKED, (SUM(HT.QUANTITY) * H.HABITVALUE) AS TOTALMEASURE
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
