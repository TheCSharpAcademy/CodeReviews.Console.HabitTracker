# Habit Tracker

My first C# CRUD application. 

Console based application to track book reading.

## Requirements

- [X] This is an application where you’ll log occurrences of a habit.

- [X] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)

- [X] Users need to be able to input the date of the occurrence of the habit

- [X] The application should store and retrieve data from a real database

- [X] When the application starts, it should create a sqlite database, if one isn’t present.

- [X] It should also create a table in the database, where the habit will be logged.

- [X] The users should be able to insert, delete, update and view their logged habit.

- [X] You should handle all possible errors so that the application never crashes.

- [X] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.

- [X] Follow the DRY Principle, and avoid code repetition.

- [X] Your project needs to contain a Read Me file where you'll explain how your app works.

## Features

* App will create database and table when they are not currently existing.
  
* Console based UI
  * ![VsDebugConsole_RdJPF03RBG](https://github.com/user-attachments/assets/c9c037b0-6529-42ff-9388-e245f3f81ed8)
* Date must be inserted in dd.mm.yyyy format
* Every input is protected from potential errors

## Challenges

* I've had a break during this project so i had to work with code that I didn't remember
* Using SQLite commands for the first time was really challenging for me

## Areas to improve

* I need to start planning my code instead of going with the flow
* In next project i will focus on splitting project into more files, working in on big file of code is not great in terms of readability
* I want to get comfortable using Debugger in Visual Studio
