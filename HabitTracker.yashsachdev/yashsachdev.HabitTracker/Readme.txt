Console Habit Tracker:

Requirment:
1. This is an application where you’ll register one habit.
2.This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
3. The application should store and retrieve data from a real database
4. When the application starts, it should create a sqlite database, if one isn’t present.
5. It should also create a table in the database, where the habit will be logged.
6. The app should show the user a menu of options.
7. The users should be able to insert, delete, update and view their logged habit.
8. You should handle all possible errors so that the application never crashes.
9. The application should only be terminated when the user inserts 0.
10.You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
11. Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:

Challenges:
1.Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
2.Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

Features
1.Register: You can register for the application by providing your name, email, and password.
2.Login: You can log in to the application using the email and password.
3.Habit Tracker: Once you have logged in, you can add, delete, update and view your habits. Data stored in a SQLite database. 


Approach :

1.A database was created using SQLite and the tables User, Habit, and Habit Enroll were created if they don't exist. This was done using the ADO.NET and SQLite libraries.
2.A many-to-many relationship was established between the User and Habit entities using the intermediate table, Habit Enroll.
3.SQLite commands were used to perform CRUD operations, with some challenges in working with foreign key constraints.
4.A menu was created in the console for registration, login, and habit management.





