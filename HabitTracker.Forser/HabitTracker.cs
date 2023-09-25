using HabitTracker.Forser;

    string connectionString = @"Data Source=habittracker.db";
    Helpers.connectionString = connectionString;
    HabitTrackerLibrary.connectionString = connectionString;
    Helpers.Start();
    Helpers.GetUserInput();