# HabitLogger

Console based CRUD application to log and track any habit. Developed using C#/.NET and SQLite

# Requirements: 

## Initial:
✔️ Register a single habit

✔️ This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)

✔️ The application should store and retrieve data from a real database

✔️ When the application starts, it should create a sqlite database, if one isn’t present.

✔️ It should also create a table in the database, where the habit will be logged.

✔️ The app should show the user a menu of options.

✔️ The users should be able to insert, delete, update and view their logged habit.

✔️ You should handle all possible errors so that the application never crashes.

✔️ The application should only be terminated when the user inserts 0.

✔️ You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.

✔️ Your project needs to contain a Read Me file where you'll explain how your app works.

## Extra Challenges:
✔️ Let the users create their own habits to track.

✔️ Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?).

# Features

- SQLite database connection

  - The program uses a SQLite db connection to store and read information.
  - If no database exists, or the correct table does not exist they will be created on program start.

- Console based UI with all available commands

![image](https://user-images.githubusercontent.com/64802476/221361596-0abe2117-5f70-4902-a5ee-aec2092efb22.png)     ![image](https://user-images.githubusercontent.com/64802476/221361768-23ff3cff-cf09-4c49-b8df-2961a7c84065.png)


- Track multiple habits

![image](https://user-images.githubusercontent.com/64802476/221361731-9dc9b708-5b7b-4c08-98c6-495649b78d56.png)

# Resources Used
