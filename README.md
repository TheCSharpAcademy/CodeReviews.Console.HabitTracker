Habit Tracker Console App
A simple and effective console-based habit tracker built with C# and SQLite. This project is part of The C# Academy's code review series, designed to help learners practice CRUD operations, user input handling, and database integration in a real-world context.

🚀 Features
Add New Habits: Define and track personal habits.

View All Habits: Display a list of all tracked habits.

Log Daily Habits: Record whether you completed each habit for the day.

View Habit Details: Display summary statistics for a specified habit (current streak, longest streak, success rate, etc.).

Delete Habits: Remove habits from your list.

🧰 Technologies Used
C#: Core programming language.

.NET Console Application: User interface.

SQLite: Lightweight database for storing habit data.

ADO.NET: Data access layer for interacting with the SQLite database.

🛠️ Getting Started
Prerequisites
.NET SDK installed on your machine.

Installation
Clone the repository:


git clone https://github.com/MartinDrapak/CodeReviews.Console.HabitTracker.git
cd CodeReviews.Console.HabitTracker
Build the application:

dotnet build
Run the application:


dotnet run
📂 Project Structure
Program.cs: Entry point of the application.

Habit.cs: Model representing a habit.

HabitRepository.cs: Handles database operations.

HabitService.cs: Contains business logic.

Menu.cs: Manages user interactions and menu navigation.

Database: Contains the SQLite database file.

🤝 Contributing
Contributions are welcome! If you'd like to improve the application or add new features, feel free to fork the repository and submit a pull request.
