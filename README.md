💧 Drinking Water Habit Logger (Console App with ADO.NET)
📋 Overview
This is a simple console-based habit tracking application that allows users to create, read, update, and delete records of their daily water intake. It uses SQLite for data storage and demonstrates ADO.NET for database interaction along with strong user input validation.

🔧 Features
1.Create and manage a drinking_water table in SQLite.

2.Insert records with date and water quantity.

3.View all records in a tabular format.

4.Update existing records by ID.

4.Delete records by ID.

5.Input validation for date and quantity to ensure data accuracy.

🛠 Technologies Used
Technology	Purpose
C# (.NET)	Core language
SQLite	Lightweight relational database
ADO.NET	Database communication (via Microsoft.Data.Sqlite)
Console UI	User interface for input/output


✅ Note: All SQL queries are written manually, giving full control over the database logic and structure. This is typical of ADO.NET-based applications.

🧪 Input Validation
This project carefully validates user input before interacting with the database.

1. Date Validation
Expects format: dd-MM-yy.

Invalid dates trigger retry logic.

Method used: DateTime.ParseExact() in a try-catch block.

2. Integer Validation for Quantity and IDs
Uses int.TryParse() to ensure user enters a valid positive integer.

Loops back on invalid input to re-prompt the user.

Ensures robust and error-free database transactions.

Example:
csharp
Copy
Edit
if (!int.TryParse(input, out int quantity) || quantity <= 0)
{
    Console.WriteLine("Invalid number. Try again.");
    return getUserQuantity(); // Retry
}
🧑‍💻 How to Use the App
Run the application.

Choose from the main menu:

0 → Exit.

1 → View all records.

2 → Insert a new record (date + quantity).

3 → Update a record by ID.

4 → Delete a record by ID.

Follow input prompts for each action. Validation ensures only correct data is processed.

📚 Learning Highlights
Deepened understanding of ADO.NET.

Gained practical experience with SQL parameterization.

Built resilience with user input validation techniques.

Reinforced concepts of CRUD operations using raw SQL.

