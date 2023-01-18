

namespace yashsachdev.HabitTracker
{
    public class CreateDatabaseTables
    {
        public DataSet BuildDataTable()
        {
            DataSet ds = new DataSet();
            DataTable userTable = new DataTable("User");
            userTable.Columns.Add("Id", typeof(int));
            userTable.Columns.Add("Name", typeof(string));
            userTable.Columns.Add("Email", typeof(string));
            userTable.Columns.Add("Password", typeof(string));
            userTable.PrimaryKey = new DataColumn[] { userTable.Columns["Id"] };
            ds.Tables.Add(userTable);
           
            DataTable habitTable = new DataTable("Habit");
            habitTable.Columns.Add("Id", typeof(int));
            userTable.Columns.Add("HabitId", typeof(int));

            habitTable.Columns.Add("Name", typeof(string));
            habitTable.Columns.Add("Frequency", typeof(int));
            habitTable.Columns.Add("UnitMeasurementId", typeof(int));
            habitTable.PrimaryKey = new DataColumn[] { habitTable.Columns["Id"] };
            ds.Tables.Add(habitTable);

            DataTable habitEventTable = new DataTable("HabitEvent");
            habitEventTable.Columns.Add("Id", typeof(int));
            habitEventTable.Columns.Add("HabitEventId", typeof(int));
            habitEventTable.Columns.Add("Date", typeof(DateTime));
            habitEventTable.Columns.Add("Completion", typeof(bool));
          
            habitEventTable.PrimaryKey = new DataColumn[] { habitEventTable.Columns["Id"] };
            ds.Tables.Add(habitEventTable);

           DataRelation userHabitRelation=ds.Relations.Add(ds.Tables["User"].Columns["HabitId"], ds.Tables["Habit"].Columns["Id"]);
           DataRelation HabitHabitEventRelation= ds.Relations.Add(ds.Tables["Habit"].Columns["Id"], ds.Tables["HabitEvent"].Columns["HabitEventIdId"]);

            foreach (DataRow custRow in ds.Tables["User"].Rows)
            {
                Console.WriteLine(custRow["Id"].ToString());
                foreach (DataRow orderRow in custRow.GetChildRows(userHabitRelation))
                {
                    Console.WriteLine(orderRow["HabitId"].ToString());
                }
            }

            foreach (DataRow custRow in ds.Tables["Habit"].Rows)
            {
                Console.WriteLine(custRow["Id"].ToString());
                foreach (DataRow orderRow in custRow.GetChildRows(HabitHabitEventRelation))
                {
                    Console.WriteLine(orderRow["HabitEventId"].ToString());
                }
            }
            return ds;
        }

    }
}
