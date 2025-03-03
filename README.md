# **HabitTracker**

Console CRUD app to track different daily habits using C# and SQLite.

# **Requirements**

- This is an application where you’ll log occurrences of a habit.<br>
- This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)<br>
- Users need to be able to input the date of the occurrence of the habit<br>
- The application should store and retrieve data from a real database<br>
- When the application starts, it should create a sqlite database, if one isn’t present.<br>
- It should also create a table in the database, where the habit will be logged.<br>
- The users should be able to insert, delete, update and view their logged habit.<br>
- You should handle all possible errors so that the application never crashes.<br>
- You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.<br>
- Follow the DRY Principle, and avoid code repetition.<br>
- Your project needs to contain a Read Me file where you'll explain how your app works.

# **Challenges**

- Using parameterized queries to make your application more secure.<br>
- Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.<br>
- Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.<br>
- Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?).

# **Features**

1. ### **Database and table creation**
    - On start, the app creates a database `HabitTracker` with two tables, one stores all the habits and the unit of measurement and the other stores the data for each habit.
    - The application automatically seeds the database with 10 habits and 100 entries in total with random values.
2. ### **Crud Operations**
    - **CREATE**: User can create habits and insert data for each habit. When adding data to habit, user can choose to add current date.
    - **UPDATE**: User can update each habit and its data.
    - **READ**: User can view all habits and habit data separately. User can filter the habits and data with basic filters for name, date and quantity.
    - **DELETE**: User can delete a habit data or delete a habit which also deletes all of the data of the habit.
3. ### **Interface and error handling**
    - The application has a console menu for interaction.
    - If user provides incorrect input, the application either repeats the input until it's correct or it returns back to the menu.

# **Personal Challenges**

- It was my first time working with an SQLite and ADO.NET. I had to learn the syntax for the SQL itself and the C# syntax for interacting with the SQLite database. Biggest trouble was properly opening and closing the database and using the keyword using.
- Proper SoC, DRY, KISS and OOP. I have tried to separate it while keeping it simple but it has proved quite difficult. I'm sure there is a lot of room for improvement to make it cleaner and more modular.
- Correctly deciding where and how to implement error handling has been difficult as well and it feels like there's not enough. Especially try-catch.

# **Learned Lessons and Things to Improve**
- Start making the project properly from the start so that I don't have to waste time refactoring code that I have intended to change from the beginning.
- In future projects I should create a simple design documents with the features to implement, what I'm working on, etc. so that I don't have to guess and forget.
- Improve in the DRY, SoC and KISS principles.
- Get better at properly implementing error handling.