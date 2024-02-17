# Habit Tracker

## Table of Contents
- [General Info](#general-info)
- [Technologies](#technologies)
- [Features](#features)
- [Examples](#examples)
- [Requirements](#requirements)
- [Challenges](#challenges)
- [Used Resources](#used-resources)

## General Info
Project made for @TheCSharpAcademy.
It provides a command-line application for managing your daily habits. It allows you to create, view, edit, and delete habits, helping you stay motivated and track your progress over time.

## Technologies
- C#
- SQLite
- ADO.NET
- [ConsoleTables](https://github.com/khalidabuhakmeh/ConsoleTables)

## Features
- SQLite database connection
	- Populate the database with initial sample data if it's empty
- A Console Based UI
	- List habits in a clear and organized table format 
- Input validation: Ensure data entered by the user is valid.
- Perform CRUD operations
	- Create new habits
	- View all habits
	- Edit exisitng habits
	- Delete habits

## Examples
- Main Menu
  ![image](https://github.com/Dejmenek/CodeReviews.Console.HabitTracker/assets/83865666/1c7772d4-402e-45fc-9f99-985f3452c20a)
- View all habits
  ![image](https://github.com/Dejmenek/CodeReviews.Console.HabitTracker/assets/83865666/811a05c8-7f9d-41a5-ae32-0e30aa6a28c8)

## Requirements
- [x] This is an application where you’ll register one habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The app should show the user a menu of options.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] The application should only be terminated when the user inserts 0.
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works.

## Challenges
- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values.
- [ ] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?)

## Used Resources
- [Microsoft Docs for SQLite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
- Various StackOverflow articles
