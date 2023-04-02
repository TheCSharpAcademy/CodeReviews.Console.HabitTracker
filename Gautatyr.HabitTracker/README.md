# HabitTracker
Console application using CRUD to track habits by number of "times" the habit is done in a day.
Developed using C# and SQLite.

# Given Requirements:
- When the application starts, it should create a sqlite database, if one isn’t present.
- It should also create a first table in the database.
- You need to be able to insert, delete, update and view your habits.
- The application should only be terminated when the user inserts 0, validations on user's inputs.
- You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework

# Features:
- SQLite database connection
-   - The program uses a SQLite db connection to store and read information.
-   - If no database exists, the program will create one on startup with a first habit.
- Console base UI using key inputs.
- CRUD DB function
-   - The user can Create, Read, Update and Delete habits, and the number of time they practiced the habit.

# Challenges:
- This is my first practical project with C# and SQLite.
- I also had to learn how to use raw SQL to query the database.

# Lessons learned:
- How to open a connection with the database.
- How to query the database using raw SQL and SQLite.
- How to find solutions when the IDE doesn't want to function like you want.

# Areas to improve:
- The app is kept into one single class for simplicity, but it could help having 1 or 2 classes more to keep it cleaner and simpler.
- Learning more SQL would be a good idea to be able to create more complex queries.

# Resources Used:
- StackOverflow
- CodeAcademy C# course
- Microsoft documentation
- sqlite.org
