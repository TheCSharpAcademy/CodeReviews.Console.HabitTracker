## **📊 Simple Habit Tracker Console Application**

This is a basic console-based application built with C# and SQLite, designed to help users track daily habits. It allows you to log daily quantities for a predefined habit, update previous entries, delete records, and view your progress.
Based on: https://www.thecsharpacademy.com/project/12/habit-logger

**✨ Features**
 - Create Table: Automatically creates a habit_tracker table in a SQLite
   database upon first run to store your habit data.
 - Insert Record: Add new entries for your habit, including the date and
   quantity.
 - View All Records: Display all logged habit entries in a clear,
   formatted table.
 - Update Record: Modify existing habit entries by their ID.
 - Delete Record: Remove specific habit entries using their ID.
 - Basic Input Validation: Ensures date and quantity inputs are in the
   correct format and type.

## 🚀 How to Use

**Prerequisites**
.NET SDK (Version 6.0 or higher recommended)

You might need to install the Microsoft.Data.Sqlite NuGet package if it's not already referenced in your project. You can add it via:

    dotnet add package Microsoft.Data.Sqlite

Running the Application
 - Clone the repository (if applicable) or save the Program.cs file to a
   local directory.
 - Open your terminal or command prompt and navigate to the directory
   containing Program.cs.

Run the application using the .NET CLI:

    dotnet run

Follow the on-screen menu instructions to interact with the habit tracker. The database file (HabitTracker.db) will be created in the same directory where you run the application.

## 🛠️ Technologies Used

 - C#: The primary programming language.
 - SQLite: A lightweight, file-based database for data storage.
 - Microsoft.Data.Sqlite: The ADO.NET provider for SQLite.

## 🛣️ Challenges

This was my first project using ADO.NET and parameterized queries, which was new to me. Furthermore, I tried to solve the task without object-oriented programming and did not implement date handling using the `DateTime` type, as the description requested. I feel the code is a bit "spaghetti code" in places, but it works, and I believe I learned a lot with this project.
