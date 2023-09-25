using HabitTracker.Forser;

    string connectionString = @"Data Source=habittracker.db";
    Helpers.ConnectionString = connectionString;
    HabitTrackerLibrary.ConnectionString = connectionString;
    Helpers.Start();
    Helpers.GetUserInput();