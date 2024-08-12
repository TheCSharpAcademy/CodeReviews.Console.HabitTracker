Got it! I'll include this function in the README. Here's the updated version:

---

# Console Habit Tracker

## Overview

This is a console application built with C# and SQLite to help you track a specific habit by quantity. For example, you can track the number of water glasses you drink each day.

## Features

- Register and track one habit by quantity (e.g., number of water glasses per day).
- Store and retrieve data from a SQLite database.
- Automatically create the database and table if they do not exist.
- Menu options to insert, delete, update, and view logged habits.
- Error handling to ensure the application never crashes.
- Application terminates only when the user inputs `0`.

## Requirements

- .NET Core SDK
- SQLite

## Installation

1. **Clone the repository:**
    ```bash
    git clone https://github.com/yourusername/habit-tracker.git
    cd habit-tracker
    ```

2. **Build the project:**
    ```bash
    dotnet build
    ```

3. **Run the application:**
    ```bash
    dotnet run
    ```

## Usage

When you run the application, you will see the following main menu:

```plaintext
Main Menu

What would you like to do?
Type 0 To Close Application.
Type 1 To View All Records.
Type 2 To Insert Record.
Type 3 To Delete Record
Type 4 To Update Record
Type 5 To Quantity In Specific Day
----------------------------------
```


## Challenges

### 1. Seed Data

When the database is created for the first time, it will automatically generate a few habits and insert a hundred records with randomly generated values.

### 2. Sum of Water Cups

A function to determine the sum of water cups consumed on a specific day.

## Code Snippets

### Database Initialization

```csharp
using System;
using System.Data.SQLite;

internal class DataBaseManager
{
    static readonly string _connectionString = @"Data Source=habit-tracker.db";

    public static void CreateDataBase()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER
                    )";

            tableCmd.ExecuteNonQuery();
            DatabaseSeeder.SeedData(connection);
            connection.Close();
        }
    }
}
```

### Seed Data

```csharp
using System;
using System.Data.SQLite;

public static class DatabaseSeeder
{
    private static Random _random = new();

    public static void SeedData(SQLiteConnection connection)
    {
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(*) FROM drinking_water";
        long count = (long)checkCmd.ExecuteScalar();

        if (count == 0)
        {
            Console.WriteLine("Seeding data...");

            for (int i = 0; i < 100; i++)
            {
                int habitId = i + 1; 
                string date = DateTime.Now.AddDays(-_random.Next(0, 365)).ToString("dd-MM-yyyy");
                int quantity = _random.Next(1, 10);

                var recordCmd = connection.CreateCommand();
                recordCmd.CommandText = "INSERT INTO drinking_water (Id, Date, Quantity) VALUES (@habitId, @date, @quantity)";
                recordCmd.Parameters.AddWithValue("@habitId", habitId);
                recordCmd.Parameters.AddWithValue("@date", date);
                recordCmd.Parameters.AddWithValue("@quantity", quantity);
                recordCmd.ExecuteNonQuery();
            }

            Console.WriteLine("Seeding complete.");
        }
    }
}
```



### Quantity of water glasses in Specific Day

```csharp
public static void QuantityInSpecificDay()
{
    Console.Clear();
    string dateToCheck = GetDateInput();

    using (var connection = new SQLiteConnection(_connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @$"SELECT Id, Date, COALESCE(SUM(Quantity), 0) as Quantity
                                  FROM drinking_water
                                  WHERE Date = '{dateToCheck}'";

        using (SQLiteDataReader reader = tableCmd.ExecuteReader())
        {
            if (!reader.Read() || reader.IsDBNull(0))
            {
                Console.WriteLine("There's no records for this day");
            }
            else
            {
                Console.WriteLine("ID  | Date       | Quantity");
                Console.WriteLine("----------------------------");
                int id = reader.GetInt32(0);
                string date = reader.GetString(1);
                int quantity = reader.GetInt32(2);

                Console.WriteLine($"{id}   | {date} |    {quantity}");
            }
        }
        connection.Close();
    }
    Console.WriteLine("----------------------------------------\n\nPress any key to Continue...");
    Console.ReadKey();
}
```

## Conclusion

This habit tracker app is a simple yet effective way to monitor your daily habits. It ensures data persistence through SQLite and provides a user-friendly interface for managing your habit entries.

Feel free to contribute or suggest improvements!
