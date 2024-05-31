# Habit Tracker

A console-based CRUD application to track various habits. Developed using C# and SQLite.

## Table of Contents

1.  [Introduction](#introduction)
2.  [Requirements](#requirements)
3.  [Features](#features)
4.  [Usage](#usage)

## Introduction

This Habit Tracker is a simple console application designed to help users manage and track their habits. Users can add, update, delete, and view habits, which are stored in an SQLite database. The application also supports seeding the database with random test data for development and testing purposes.

## Requirements

- [x]  When the application starts, it should create an SQLite database if one isn’t present.
- [x]  It should also create a table in the database where habits will be logged.
- [x]  Users should be able to insert, delete, update, and view their logged habits.
- [x]  The application should handle all possible errors so that it never crashes.
- [x]  The application should only be terminated when the user inputs 0.
- [x]  You can only interact with the database using raw SQL. No ORM tools such as Entity Framework are allowed.
- [x]  Include a ReadMe file explaining how the app works.

## Features

### SQLite Database Connection

The program uses an SQLite database to store and read habit information. If no database exists, or if the correct table does not exist, they will be created at program start.

### Console-Based UI

The application provides a console-based UI where users can navigate using key presses.

### CRUD Operations

-   **Create**: Users can add new habits.
-   **Read**: Users can view all logged habits.
-   **Update**: Users can update existing habits.
-   **Delete**: Users can delete habits.

### Database Seeding

Users can insert test data into the database for testing purposes.

### Error Handling

All possible errors are handled to prevent the application from crashing.
    
## Usage

### Main Menu

When the application starts, you will see the following options:

![](https://i.imgur.com/sU45IX8.png)

### Adding a New Habit

Select `1` to add a new habit. You will be prompted to enter details such as name, measurement, quantity, frequency, notes, and status.

### Updating a Habit

Select `2` to update an existing habit. You will be prompted to enter the ID of the habit you want to update and then provide the new details.

### Deleting a Habit

Select `3` to delete a habit. You will be prompted to enter the ID of the habit you want to delete.

### Viewing All Habits

Select `4` to view all logged habits. The habits will be displayed in a tabular format.

### Inserting Test Habits

Select `5` to insert test habits. This will add a specified number of randomly generated habits to the database.

### Exiting the Application

Select `0` to exit the application.