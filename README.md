# 💧 Habit Logger

This is a simple console application built with C# and ADO.NET that allows users to log occurrences of a **habit**. It is designed to help beginners understand how to perform **CRUD operations** (Create, Read, Update, Delete) against a **real SQLite database**.

---

## 📚 Features

- ✅ Create a local SQLite database and table automatically on startup
- 📝 Insert new records of habits created by the user, listed by date and quantity
- 👁️ View all previously logged records
- 🛠️ Update existing records by ID
- ❌ Delete records by ID
- 🛡️ Handles invalid input and prevents application crashes
- 🔒 Uses parameterized queries to prevent SQL injection
- ♻️ Follows the DRY principle and clean code practices

---

## 🚀 How It Works

When you launch the application:

1. It checks if the database file (`habit-Logger.db`) exists. If not, it creates it.
2. A menu appears where you can:
   - View all records
   - Add a new record
   - Delete a record
   - Update a record
   - Add a new, user specified habit
   - Generate a report for the specified habit (such as total use, average etc)
   - Exit the application

Each record contains:
- A unique ID
- A **date** (format: `dd-mm-yy` (visually, stored as yyyy-MM-dd to allow simple search queries by date)
- A **quantity** (e.g., number of water glasses)

---

## 🛠️ Technologies Used

- C# (.NET)
- ADO.NET (`Microsoft.Data.Sqlite`)
- SQLite (file-based database)
- Console application (no UI)

---



## 🧪 Example Usage

```plaintext
MAIN MENU
What would you like to do?
Type 0 to Close the Application.
Type 1 to View All Records.
Type 2 to Insert Record.
Type 3 to Delete Record.
Type 4 to Update Record.
Type 5 to Add a new habit.
Type 6 to generate a report.
------------------------------------------