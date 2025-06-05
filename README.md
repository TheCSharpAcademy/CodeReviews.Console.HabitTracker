# 📝 Habit Tracker - A Simple CRUD App with ADO.NET and SQLite

Welcome to the Habit Tracker app! This is a simple console application built in C# that teaches the fundamentals of **CRUD (Create, Read, Update, Delete)** operations using **ADO.NET** and **SQLite**.

Made as a part of https://thecsharpacademy.com course

---

## 📦 Features

- CRUD for both **habits** and their **categories**
- Allow users to create their own habit categories with custom names and units of measurement
- Uses a **real SQLite database** created automatically on startup
- All interactions use **ADO.NET** only
- **Parameterized queries** used for security
- Input validation to ensure a smooth user experience
- Data seeded at the start automatically

---

### ▶️ Run the App

1. Clone this repository
2. Build and run the application:

    ```bash
    dotnet run
    ```

3. Follow the on-screen menu to:
    - Manage habits
        - View all (Update, Delete)
        - Insert
    - Manage categories
        - View all (Update, Delete)
        - Insert
