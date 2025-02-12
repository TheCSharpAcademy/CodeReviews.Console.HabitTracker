# Habit Tracker

Console-based CRUD application to track habits. Developed using C#, SQLite and ADO.NET.

## Features

- Option to define multiple habits, each with their own desired unit measure.
- Users are able to insert, delete, edit and view all of their data.
- When logging an entry, users can simply type "today" as shortcut for current date.
- Users can see a complete report that exhibits C.T.A., current and record streaks, and top 3s.
- Incorrect input statements are gracefully handled, as to not leave users in the dark.
- SQLite database created on startup.
- SQL queries are protected with parameterized queries.
- Option to fill database with random data, to facilitate testing the application.
- Organized code that follows DRY principle.

## Application usage details

- Users can only log a new habit entry to a previously defined habit.
- Users can only log a specific habit once per date. They can't log habits for days yet to come.
- Deleting a habit definition will delete all entries associated with it.

## Images

![image](https://github.com/user-attachments/assets/2d4b25b1-5795-4abe-aabf-9c5558027c65)

![image](https://github.com/user-attachments/assets/3ba583e9-720b-4e74-a0ca-450f68c14740)

![image](https://github.com/user-attachments/assets/cd88ff2e-e6fe-4040-9d36-02461b12314e)

![image](https://github.com/user-attachments/assets/a869e52d-9d8c-4bac-a020-55f14d5fdfd7)

## Things I learned

- How to launch external terminal window within VSCode
- You can have a "UNIQUE" key that is actually the combination of multiple columns
- DateOnly facilitates a lot of the operations I would have to do with DateTime
- Done trumps perfect. I identified a few opportunities for improvement, but opted to focus on delivering quickly and making it better next time/next project

> Project duration: ~16h
