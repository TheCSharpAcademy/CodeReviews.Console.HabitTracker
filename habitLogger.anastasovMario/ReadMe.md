# Habit Tracker Application

## Overview
This application allows users to log and track habits based on quantity (e.g., glasses of water consumed) on specific dates. The data is stored in a SQLite database, ensuring persistence between application sessions.

## Features
Log habits with date and quantity.
Retrieve all logged habits.
Update existing habits.
Delete habits.
User-friendly console interface.

## Setup Instructions
Ensure you have .NET SDK installed on your machine.
Clone this repository or download the source code.
Open the solution in your preferred IDE.
Build the application and run it. The SQLite database will be created automatically if it doesn't exist.

## How to Use
Upon starting the application, you'll be presented with a menu.
Select an option to log a habit, view existing habits, update or delete an entry, or exit the application.
Follow the prompts to input data.

## Notes
Input dates should be in the format `dd/MM/yyyy`.
Quantities must be positive integers.
Handle invalid inputs gracefully; the application will prompt you to re-enter values when necessary.