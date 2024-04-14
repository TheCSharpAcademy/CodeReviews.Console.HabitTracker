
# HabitTrackerConsole

Habit Logger is a console-based application designed to help users track and manage their habits effectively. This tool allows users to log their daily activities and monitor their progress over time, promoting consistent and mindful habit formation.

## Features

- **Manage Habits**: Add, update, view, and delete habits.
- **Log Entries**: Record, update, and delete log entries associated with each habit.
- **Data Visualization**: View summaries of habits and their logs.
- **Database Integration**: Utilize SQLite to store and manage data persistently.
- **User-friendly Interface**: Navigate through the application via a clean and interactive console interface.

## Installation

To run Habit Logger, ensure you have `.NET` installed on your system. Follow these steps to get started:

1. Clone the repository:
   ```bash
   git clone https://github.com/yourgithub/HabitTrackerConsole.git
   ```
2. Navigate to the project directory:
   ```bash
   cd HabitTrackerConsole
   ```
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

## Usage

After launching the application, you will be greeted with the main menu. Use the arrow keys to navigate through the options:

- **View and Edit Habits**
- **View and Edit Logs**
- **Seed Database**
- **Exit**

### Adding a Habit

1. Select "View and Edit Habits" from the main menu.
2. Choose "Add New Habit."
3. Follow the prompts to enter the habit details.

![image](https://github.com/rankdjr/HabitTrackerConsole/blob/master/screenshots/manage-habits.PNG)

![image](https://github.com/rankdjr/HabitTrackerConsole/blob/master/screenshots/add-habit.PNG)

![image](https://github.com/rankdjr/HabitTrackerConsole/blob/master/screenshots/habits-sqlview.PNG)

### Logging an Entry

1. Select "View and Edit Logs" from the main menu.
2. Choose "Add Log Entry."
3. Select a habit and enter the log details such as date and quantity.


![image](https://github.com/rankdjr/HabitTrackerConsole/blob/master/screenshots/manage-logs.PNG)

![image](https://github.com/rankdjr/HabitTrackerConsole/blob/master/screenshots/add-log-1.PNG)

![image](https://github.com/rankdjr/HabitTrackerConsole/blob/master/screenshots/add-log-2.PNG)

![image](https://github.com/rankdjr/HabitTrackerConsole/blob/master/screenshots/log-sqlview.PNG)


## Acknowledgments

- Thanks to the open-source community for continuous inspiration and feedback.
- Project based on requirements and guidelines from [The C# Academy](https://www.thecsharpacademy.com/project/12/habit-logger).
