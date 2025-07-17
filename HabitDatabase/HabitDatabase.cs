using System.Data.SQLite;
using System.Globalization;

namespace HabitDatabase;

public class DatabaseManager
{
    readonly private string filename = "habitdatabase.db";
    public DatabaseManager()
    {
        //File.Delete(filename);
        CreateEntitiesAndBootstrapData();
    }

    private void CreateEntitiesAndBootstrapData()
    {
        using SQLiteConnection openCon = GetOpenConnection();
        using SQLiteCommand cmd = new SQLiteCommand(openCon);
        cmd.CommandText = "SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='Habit';";
        bool parseSuccess = int.TryParse(cmd.ExecuteScalar().ToString(), out int retrievedCount);

        if (parseSuccess && retrievedCount == 0)
        {
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Habit(Id INTEGER PRIMARY KEY, Name TEXT);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Occurrence(
                    HabitId INTEGER NOT NULL,
                    OccurrenceDate DATE NOT NULL,
                    Frequency INTEGER,
                    PRIMARY KEY (HabitId, OccurrenceDate),
                    FOREIGN KEY (HabitId) REFERENCES Habit(Id)
                );";
            cmd.ExecuteNonQuery();

            // Bootstrap data
            cmd.CommandText = @"INSERT INTO Habit(Name) VALUES (@name)";
            cmd.Parameters.AddWithValue("@name", "Drink a cup of water");
            cmd.ExecuteNonQuery();
            cmd.Parameters.AddWithValue("@name", "Do daily drums exercise");
            cmd.ExecuteNonQuery();
        }
    }

    public bool InsertOccurrence(DateOnly occurrenceTime, int numberOfOccurrences, int HabitId)
    {
        using SQLiteConnection openCon = GetOpenConnection();
        using SQLiteCommand cmd = new SQLiteCommand(openCon);
        string isoDate = occurrenceTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        cmd.CommandText = "INSERT INTO Occurrence(HabitId, Frequency, OccurrenceDate) VALUES(@HabitId, @Frequency, @OccurrenceDate);";
        cmd.Parameters.AddWithValue("@HabitId", HabitId);
        cmd.Parameters.AddWithValue("@Frequency", numberOfOccurrences);
        cmd.Parameters.AddWithValue("@OccurrenceDate", isoDate);

        // What about INSERT INTO ... ON CONFLICT(...) DO UPDATE SET ... 
        try
        {
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                return false;
            }
        }
        catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint)
        {
            Console.WriteLine("Constraint violation!");
        }
        catch (SQLiteException)
        {
            Console.WriteLine("Constraint violation!");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Unable to insert occurrence: " + ex.Message);
        }

        return true;
    }

    public bool DeleteRecord(HabitOccurrence habitOccurrence)
    {
        using SQLiteConnection openCon = GetOpenConnection();
        using SQLiteCommand cmd = new SQLiteCommand(openCon);
        string isoDate = habitOccurrence.OccurrenceDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        cmd.CommandText = @"DELETE FROM Occurrence WHERE Occurrence.HabitId = @HabitId AND Occurrence.Frequency = @Frequency AND Occurrence.OccurrenceDate = @OccurrenceDate;";
        cmd.Parameters.AddWithValue("@HabitId", habitOccurrence.HabitId);
        cmd.Parameters.AddWithValue("@Frequency", habitOccurrence.NumberOfOccurrences);
        cmd.Parameters.AddWithValue("@OccurrenceDate", isoDate);

        int rowsAffected = 0;
        try
        {
            rowsAffected = cmd.ExecuteNonQuery();
        } catch (SQLiteException ex) {
            System.Console.WriteLine("Unable to delete occurrence: " + ex.Message);
        }

        return rowsAffected > 0;
    }

    public bool UpdateOccurrence(HabitOccurrence original, HabitOccurrence updated){
        using SQLiteConnection openCon = GetOpenConnection();
        using SQLiteCommand cmd = new SQLiteCommand(openCon);
        string isoDate = original.OccurrenceDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        cmd.CommandText = @"UPDATE Occurrence 
                            SET HabitId = @HabitIdUpdated,
                                Frequency = @FrequencyUpdated, 
                                OccurrenceDate = @OccurrenceDateUpdated
                            WHERE HabitId = @HabitId
                              AND OccurrenceDate = @OccurrenceDate;";
        cmd.Parameters.AddWithValue("@HabitId", original.HabitId);
        cmd.Parameters.AddWithValue("@OccurrenceDate", isoDate);
        string updatedIsoDate = updated.OccurrenceDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        cmd.Parameters.AddWithValue("@HabitIdUpdated", updated.HabitId);
        cmd.Parameters.AddWithValue("@FrequencyUpdated", updated.NumberOfOccurrences);
        cmd.Parameters.AddWithValue("@OccurrenceDateUpdated", updatedIsoDate);

        int rowsAffected = 0;
        try
        {
            rowsAffected = cmd.ExecuteNonQuery();
        } catch (SQLiteException ex) {
            System.Console.WriteLine("Unable to update occurrence: " + ex.Message);
        }

        return rowsAffected > 0;
    }

    private SQLiteConnection GetOpenConnection()
    {
        var connection = new SQLiteConnection(@$"URI=file:{filename}");
        connection.Open();

        using (var command = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
        {
            command.ExecuteNonQuery();
        }

        return connection;
    }

    public List<Habit> GetAvailableHabits()
    {
        using SQLiteConnection openCon = GetOpenConnection();
        using SQLiteCommand cmd = new SQLiteCommand(openCon);
        cmd.CommandText = "SELECT * FROM Habit ORDER BY 1;";
        SQLiteDataReader rdr = cmd.ExecuteReader();

        List<Habit> result = new List<Habit>();
        while (rdr.Read())
            result.Add(new Habit(rdr.GetInt32(0), rdr.GetString(1)));

        return result;
    }

    public List<HabitOccurrence> GetAllHabitOccurrences()
    {
        using SQLiteConnection openCon = GetOpenConnection();
        using SQLiteCommand cmd = new SQLiteCommand(openCon);
        cmd.CommandText = @"SELECT Occurrence.*, Habit.Name 
                            FROM Occurrence
                            JOIN Habit ON Habit.Id = Occurrence.HabitId;";
        SQLiteDataReader rdr = cmd.ExecuteReader();

        List<HabitOccurrence> result = new List<HabitOccurrence>();
        while (rdr.Read())
            result.Add(new HabitOccurrence(rdr.GetInt32(0), DateOnly.FromDateTime(rdr.GetDateTime(1)), rdr.GetInt32(2), rdr.GetString(3)));

        return result;
    }

}

public record Habit(int HabitId, string HabitDescription)
{
    public override string ToString()
    {
        return $"{HabitId.ToString().PadLeft(2)} - {HabitDescription}";
    }
}

public record HabitOccurrence(int HabitId, DateOnly OccurrenceDate, int NumberOfOccurrences, string HabitDescription) : IComparable<HabitOccurrence>
{
    public int CompareTo(HabitOccurrence? other)
    {
        if (other == null) return 1;

        // Primary sort: by OccurrenceDate, then by HabitId
        int result = OccurrenceDate.CompareTo(other.OccurrenceDate);
        if (result != 0) return result;

        return HabitId.CompareTo(other.HabitId);
    }

    public override string ToString()
    {
        return $"{OccurrenceDate}: {NumberOfOccurrences}x {HabitDescription}";
    }
}

