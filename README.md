# CodeReviews.Console.HabitTracker

Terminal (console) application for recording and managing habits, using C# and SQLite. Allows you to insert, view, update and delete habit occurrences, all via the command line interface.

---

## Requirements Met
- [x] On startup, creates the SQLite database and table if they do not exist.
- [x] Allows inserting, deleting, updating, and viewing registered habits.
- [x] Handles all possible errors to prevent the application from crashing.
- [x] The program only ends when the user chooses the exit option.
- [x] Interacts with the database using only raw SQL (ADO.NET), no ORMs.

---

## Features

- **SQLite Database**
  - Connection and data manipulation using ADO.NET.
  - Automatic creation of the database and table on first run.

- **Simple and Intuitive Console Interface**
  - Main menu with options for CRUD operations on habits.
  - Navigation by typing numbers.

- **Full CRUD**
  - Insert new habits with name, quantity, and date.
  - List all registered habits.
  - Dynamic update: choose which fields to update.
  - Delete habits by ID.

- **Validation and Error Handling**
  - Validates user input (quantity, dates, IDs).
  - Clear messages in case of error or invalid operation.

---

## Challenges
- First real experience with C#, SQLite, and ADO.NET.
- Handling date and quantity validation in the console.
- Learning to build dynamic queries for partial updates.
- Ensuring the app never crashes, even with invalid input.

---

## Lessons Learned
- How to structure a CRUD in C# using only raw SQL.
- Separating user input logic from data manipulation logic.
- How to organize code into classes and methods for easier maintenance.

---

## Areas for Improvement
- Implement reports and habit statistics.
- Improve user experience with more visual feedback.
- Refactor to further separate responsibilities (e.g., service layer).
- Add automated tests.

---

## Resources Used
- [Official .NET and C# Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Zetcode - SQLite with C#](https://zetcode.com/csharp/sqlite/)
- Videos and articles about C# and SQLite

---

**Developed for learning purposes at C# Academy.**
