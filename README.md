# 💧 Habit Logger – Drinking Water Tracker

This is a simple console application built with C# and ADO.NET that allows users to log occurrences of a **habit** — in this case, the habit of drinking water. It is designed to help beginners understand how to perform **CRUD operations** (Create, Read, Update, Delete) against a **real SQLite database**.

---

## 📚 Features

- ✅ Create a local SQLite database and table automatically on startup
- 📝 Insert new records of water consumption by date and quantity
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
2. It ensures that the `drinking_water` table exists.
3. A menu appears where you can:
   - View all records
   - Add a new record
   - Delete a record
   - Update a record
   - Exit the application

Each record contains:
- A unique ID
- A **date** (format: `dd-mm-yy`)
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

Type 0 to Close Application.
Type 1 to View All Records.
Type 2 to Insert Record.
Type 3 to Delete Record.
Type 4 to Update Record.
------------------------------------------
