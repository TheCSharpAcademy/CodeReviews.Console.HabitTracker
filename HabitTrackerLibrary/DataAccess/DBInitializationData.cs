namespace HabitTrackerLibrary.DataAccess
{
    public static class DBInitializationData
    {
        public static void InitializeTables(SqliteDataAccess db, SqlData sqlData)
        {
            InitializeUnitsTable(db);
            SeedUnitsTableData(db, sqlData);

            InitializeHabitsTable(db);
            SeedHabitsTableData(db, sqlData);

            InitializeRecordsTable(db);
            SeedRecordsTableData(db, sqlData);
        }

        private static void InitializeUnitsTable(SqliteDataAccess db)
        {
            db.Execute(
                @"  CREATE TABLE IF NOT EXISTS Units
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT
                    );
                ");
        }

        private static void SeedUnitsTableData(SqliteDataAccess db, SqlData sqlData)
        {
            if (sqlData.RecordExists("Units") == false)
            {
                db.Execute(
                    @"  INSERT INTO Units (Name)
                        SELECT 'Units' UNION ALL
                        SELECT 'Glasses' UNION ALL
                        SELECT 'Pages' UNION ALL
                        SELECT 'kCal' UNION ALL
                        SELECT 'Times' UNION ALL
                        SELECT 'Reps'
                    ");
            }
        }

        private static void InitializeHabitsTable(SqliteDataAccess db)
        {
            db.Execute(
                @" CREATE TABLE IF NOT EXISTS Habits
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        UnitsId INTEGER,
                        FOREIGN KEY (UnitsId) REFERENCES Units(Id)
                    )
                ");
        }

        private static void SeedHabitsTableData(SqliteDataAccess db, SqlData sqlData)
        {
            if (sqlData.RecordExists("Habits") == false)
            {
                db.Execute(GenerateHabitsSeedDataSql("Drinking Water", "Glasses"));
                db.Execute(GenerateHabitsSeedDataSql("Reading", "Pages"));
            }
        }

        private static string GenerateHabitsSeedDataSql(string habitName, string unitName)
        {
            return @$"  INSERT INTO Units (Name)
                        SELECT '{unitName}'
                        WHERE NOT EXISTS (SELECT 1 FROM Units WHERE Name = '{unitName}');

                        INSERT INTO Habits (Name, UnitsId)
                        SELECT '{habitName}', (select Id from Units where Name = '{unitName}')
                        WHERE NOT EXISTS (SELECT 1 FROM Habits WHERE Name = '{habitName}');";
        }

        private static void InitializeRecordsTable(SqliteDataAccess db)
        {
            db.Execute(
                @" CREATE TABLE IF NOT EXISTS Records
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        HabitId INTEGER,
                        Date TEXT,
                        Quantity REAL,
                        FOREIGN KEY (HabitId) REFERENCES Habits(Id)
                    )
                ");
        }

        private static void SeedRecordsTableData(SqliteDataAccess db, SqlData sqlData)
        {
            if (sqlData.RecordExists("Records") == false)
            {
                int iterations = 100;
                DateTime date = DateTime.Now.AddDays(-iterations);
                Random rnd = new Random();

                SeedHabitsTableData(db, sqlData);

                for (int i = 0; i < iterations; i++)
                {
                    sqlData.InsertRecordByHabitName("Drinking Water", date.ToString("yyyy-MM-dd"), rnd.Next(1, 25));
                    sqlData.InsertRecordByHabitName("Reading", date.ToString("yyyy-MM-dd"), rnd.Next(1, 100));

                    date = date.AddDays(1);
                }
            }
        }

    }


}
