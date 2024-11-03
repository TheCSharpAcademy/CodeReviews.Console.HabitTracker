using Microsoft.Data.Sqlite;
using Spectre.Console;
namespace StartUp;
public static class InitializeProgram
{


    public static void StartDatabase(string dataBasePath, string desiredDatabaseSchema)
    {
        // Checks if a database with the given name exists to read and write to it , if it doesnt exist it will create the database with the given name.
        using (var connection = new SqliteConnection($"Data Source={dataBasePath}"))
        {
            // Checks if the database has the specifed table name, if not it will create the given table name with the given schema query.
            connection.Open();
            var checkTableCommand = connection.CreateCommand();
            checkTableCommand.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='habit';";
            var tableName = checkTableCommand.ExecuteScalar() as string;

            // Only create the table if it doesn't exist
            if (string.IsNullOrEmpty(tableName))
            {
                var createDatabaseSchema = connection.CreateCommand();
                createDatabaseSchema.CommandText = desiredDatabaseSchema;
                createDatabaseSchema.ExecuteNonQuery();
                GenerateRandomData(dataBasePath, true);
            }
            else
            {
                //Logging and debugging purposes.
                AnsiConsole.MarkupLine("[yellow]Table 'habit' already exists, skipping table creation...[/]");
            }
            connection.Close();
        }
        //Logging and debugging purposes.
        AnsiConsole.MarkupLine("[green]Initlization Successfull\nPress any key to continue[/]");
        Console.ReadKey();
        return;
    }


    //seed random data into the database Upon Creation if the option was toggled true in the method call.
    private static void GenerateRandomData(string databasePath, bool seedData = false, int generatedRowsNumber = 100)
    {
        string dateFormat = "yyyy-MM-dd";
        string[] randomHabits = ["Reading(Hrs)", "Running(Kms)", "Playing Chess(Hrs)", "Swimming(Kms)", "Coding(Hrs)", "Walking(Kms)", "Working Out(Hrs)", "Smoking(Ciggaretes)"];
        int numberOfRandomHabits = randomHabits.Length;
        string randomRecordQuery =
                @"
                INSERT INTO habit 
                (habitname,quantity,quantityunit,date) VALUES (@habitname,@quantity,@measurementunit,@outputdate);
                ";
        Random gen = new Random();

        string ExtractMeasurementUnit(string habitName, char unitDelimiter = '(')
        {
            string unit = "";
            int delimiterLocation = habitName.IndexOf(unitDelimiter);
            int delimiterClosingLocation = habitName.IndexOf(')');

            int startingPosition = delimiterLocation + 1;
            int endingPosition = delimiterClosingLocation - startingPosition;

            unit = habitName.Substring(startingPosition, endingPosition);
            return unit;
        }

        string ExtractHabitName(string habitName, char unitDelimiter = '(')
        {
            string parsedHabitName = "";

            int trimStartingPosition = habitName.IndexOf(unitDelimiter);

            parsedHabitName = habitName.Remove(trimStartingPosition);
            return parsedHabitName;
        }

        DateTime RandomDay()
        {
            // Defining a range to  for random date to be from a specific start date untill today.
            DateTime start = new DateTime(2010, 01, 01);
            int range = (DateTime.Today - start).Days;

            return start.AddDays(gen.Next(range));
        }
        // Forloop generates exactly 100 row.
        for (int i = 0; i < generatedRowsNumber; i++)
        {
            string randomHabit = randomHabits[gen.Next(0, numberOfRandomHabits)];
            string habitName = ExtractHabitName(randomHabit);
            int randomQuantity = gen.Next(1, 11);
            string randomDate = DateOnly.FromDateTime(RandomDay()).ToString(dateFormat);
            string randomMeasurmentUnit = ExtractMeasurementUnit(randomHabit);

            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();

                var writeQuery = connection.CreateCommand();
                writeQuery.CommandText = randomRecordQuery;
                try
                {
                    writeQuery.Parameters.AddWithValue("@habitname", habitName);
                    writeQuery.Parameters.AddWithValue("@quantity", randomQuantity);
                    writeQuery.Parameters.AddWithValue("@measurementunit", randomMeasurmentUnit);
                    writeQuery.Parameters.AddWithValue("@outputdate", randomDate);
                    writeQuery.ExecuteNonQuery();
                    connection.Close();

                }
                // Rare chance => if there is duplication generate another row and the duplicated row won't be added.
                catch (SqliteException sqlEx)
                {
                    if (sqlEx.Message.Contains("UNIQUE constraint failed"))
                    {
                        Console.WriteLine("Unique constraint violated - Generating another random habit...");
                        i--;
                        connection.Close();
                        continue;

                    }
                    else
                        throw;

                }
            }
        }
    }

}

