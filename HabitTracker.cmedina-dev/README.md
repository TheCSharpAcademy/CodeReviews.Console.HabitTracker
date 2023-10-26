# Habit Tracker

## Table of Contents

- [Description](#description)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Steps](#steps)
- [Usage](#usage)
- [Features](#features)

## Description

This project is a habit tracker featuring a local SQLite database. It enables CRUD operations for habits, allowing users to create, modify, and delete their own habits.

## Requirements

* Create a sqlite database, if one isn’t present.
* Create a table in the database, where the habit will be logged.
* Show the user a menu of options.
* Able to insert, delete, update and view logged habits.
* Handle all possible errors so that the application never crashes.
* Only terminate when the user inserts 0.
* Only interact with the database using raw SQL.

## Installation

### Prerequisites

- .NET 7.0

### Steps

1. **Clone the Repository**

    ```bash
    git clone https://github.com/cmedina-dev/CodeReviews.Console.HabitTracker.git
    ```

2. **Open the Solution File**

Build the file in HabitLogger. You can open the solution file and build in Visual Studio.

## Usage

Navigation of the program is done via the keyboard. Follow the on-screen prompts as directed to add your own habits to the tracker.


## Features

- Create, view, update, and delete habits
- Habits are stored in a local .sqlite file
- Modify the stat tracked within each habit

---
