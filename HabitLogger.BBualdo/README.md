## Console Habit Logger

CRUD Console App where user can create habits, insert new records, update, delete and track them. Data is stored in SQLite Database.

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

## Challenges

- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
- [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

## What I've learned

It was my first time connecting application to Database, so this is most important skill I took from this challenge. I've also started to get better at organising my code to reusable methods (it's far from SOLID, but it's not a mess).

## Things I have to practice

I'm struggling with understanding SQL JOIN queries, all of them. Creating summary report for habit was really confusing and when I'm reading my query, I still don't know what SQL does with it and how it works.