// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Managers.SqliteDataManager.SeedData
// --------------------------------------------------------------------------------------------------
// Partial class for data manager methods specific to seeding the databe with mock data.
// --------------------------------------------------------------------------------------------------
using Microsoft.Data.Sqlite;

namespace HabitTracker.Data.Managers;

public partial class SqliteDataManager
{
    #region Constants
    
    private readonly string SeedTableHabitQuery =
        @"
        insert into habit(name, measure, is_active) values ('drinking water', 'glasses', true);
        insert into habit(name, measure, is_active) values ('morning coffee', 'cups', true);
        insert into habit(name, measure, is_active) values ('afternoon walk', 'kms', true);
        insert into habit(name, measure, is_active) values ('reading before bed', 'pages', true);
        insert into habit(name, measure, is_active) values ('meditation', 'minutes', false);
        insert into habit(name, measure, is_active) values ('yoga', 'minutes', false);
        insert into habit(name, measure, is_active) values ('listening to music', 'minutes', true);
        ";

    #endregion
    #region Methods: Public

    public void SeedDatabase()
    {
        // Only seed once / new database.
        if (GetHabits().Count == 0)
        {
            // Put all seed database methods here, in dependency order.
            SeedTableHabit();
            SeedTableHabitLog();
        }
    }

    #endregion
    #region Methods: Private

    private void SeedTableHabit()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = SeedTableHabitQuery;
        command.ExecuteNonQuery();
        
        connection.Close();
    }

    private void SeedTableHabitLog()
    {
        // Cache the habits in the database.
        var habits = GetHabits();

        // Generate a hundred randomly generated habit log entries.
        var generateCount = 100;

        for (int i = 0; i < generateCount; i++)
        {
            // Get a habit id to record against.
            var habitId = habits[Random.Shared.Next(0, habits.Count)].Id;

            // Get random Date in the past 90 days.
            var date = DateTime.Now.AddDays(-Random.Shared.Next(0, 90)).Date;

            // Get random Quantity between 1 and 10.
            var quantity = Random.Shared.Next(1, 11);

            // Add to database.
            AddHabitLog(habitId, date, quantity);
        }
    }

    #endregion
}
