# Habit Tracker Console Application

## Overview

This is a simple **Habit Tracker** console application built in C#. 
It allows you to manage and track daily habits, with a focus on tracking the consumption of drinking water. 
The application provides a user-friendly menu-driven interface for various operations, including viewing habits, adding new entries, updating existing entries, and deleting entries.

## Features

- **View All Habit Data:** View all recorded habit entries.
- **Add New Habit Entry:** Add new entries, specifying the date and quantity of water consumed.
- **Update Existing Entry:** Modify existing entries, including changing the date and quantity.
- **Delete Entry:** Delete habit entries.
- **Exit Habit Tracker:** Exit the application.

## Getting Started

Before running the application, ensure you have the necessary dependencies:

- [SQLite](https://www.sqlite.org/index.html): This application uses SQLite as its database system. Make sure you have SQLite installed or available in your project.

To run the application:

1. Clone this repository or download the code.
2. Open the project in your preferred C# development environment (e.g., Visual Studio, Visual Studio Code).
3. Build and run the application.

## Usage

- When you run the application, you'll see the main menu.
- Use the number keys (1, 2, 3, 4, 0) to select the desired action.
- Follow the prompts to interact with the application:
  - **View all habit data:** Displays all recorded habit entries.
  - **Add new habit entry:** Input the date and quantity of water consumed.
  - **Update an existing entry:** Update the date and quantity of an existing entry.
  - **Delete an entry:** Delete a habit entry by specifying its ID.
  - **Exit Habit Tracker:** Exits the application.

## Database

The application relies on an SQLite database to store habit data. Be sure to configure the database connection in the code according to your setup.

## Contributing

Contributions are welcome! Feel free to fork this repository and submit pull requests.

## License

This project is licensed under the MIT License. See the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

- This application provides a simple example and can serve as a starting point for building more complex habit tracking systems.
