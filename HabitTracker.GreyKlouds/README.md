# HabitTracker

A C# application I wrote using VS Code.
This is a console based CRUD application to track a habbit specified by the user. C# and SQLite were used to develop this application.

# Given Requirements:

    This is an application which allowws a user to log occurrences of a habit.
    Users need to be able to input the date of the occurrence of the habit
    The application stores and retrieves data from a database
    When the application starts,a sqlite database is created, if one isn’t present.
    The users is  able to insert, delete, update and view their logged habit.
    Application handles all possible errors so that the application never crashes.

# Features:

SQLite database connection

    The program uses a SQLite db connection to store and read information.
    If no database exists, or the correct table does not exist they will be created on program start.

    A console based UI where users can navigate by key entry of the given menu

# Main Menu:

![Screenshot 2024-11-13 072848](https://github.com/user-attachments/assets/176d9061-7b0b-4857-83a4-1b2fd1648dee)

# A list of all records

![Screenshot 2024-11-13 073200](https://github.com/user-attachments/assets/e276b7a0-7fc3-4f28-a586-a248690dbc40)

# Challenges

    This is the first time I've used SQLite. The only time I've used SQL before this was a database class in school, but learning how to use new technologies to complete this application which was pretty fun!

    Understanding how a database connects to my application through a connection string and writing sql queries within a C# program was a bit confusing to grasp. But after searching a few youtube tutorials and reading up on how the connection is made, I understand how the technologies come together.

# Resources Used

    For my C# practice and refreshers on how the language works: freecodecamp.org
    CodeBeauty C# playlist: https://www.youtube.com/playlist?list=PL43pGnjiVwgSG5lmEat2pLR5dzL86zNOB
    The C# Academy for the inital tutorial.
