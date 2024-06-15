
# ConsoleHabitTracker

C# Console Application to Log Habits.

## Features

- **Track a habit by date and quantity**: Log your daily coding habits with ease.
- **Create a db and table if there is none**: Sets up the database on the first run.
- **CRUD operations**: Perform Create, Read, Update, and Delete operations.
- **Error handling**: Manages and logs exceptions effectively to ensure smooth operation.

### Installation and usage

1. **Clone the repository:**

   ```sh
   git clone https://github.com/kjanos89/CodeReviews.Console.HabitTracker.git
   cd habit-tracker
   ```

2. **Restore dependencies just in case**

   ```sh
   dotnet restore
   dotnet build
   ```

3. **Run the application**

   ```sh
   dotnet run
   ```

4. **Choose an option from the menu**

- View records will show all records in the db.
- Delete record will delete the record you choose by its id.
- Update will update the solved issues on the chosen record.
- With Insert record you can input new data.
