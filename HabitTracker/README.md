# **ConsoleHabitLogger**

Console based CRUD application to track habits. Developed using C# and SQLite3.

# Requirments

✅ When the application starts, it should create a sqlite database, if one isn’t present.

✅ Create a table in the database where habits can be logged.

✅ The users should be able to insert, delete, update and view their logged habit.

✅ handle all possible errors so that the application never crashes.

✅ interact with the database using [ADO.NET](http://ado.net/). You can’t use mappers such as Entity Framework or    Dapper.

✅ Try Follow coding principles such as DRY , SOLID, SOC, KISS , etc..

# Challenges

✅ If no database was present during start, seed 100 random rows into the database.

✅ Used parameterized queries to make the application more secure.

✅ Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.

✅ Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?)

# Features

- Program initlization
    - As the application starts , an SQL connection is established
    - If no database exists, or the table does not exist they will be created on program start.
    - Optional random data generation with 100 rows into the database
- Console UI
    - Used [Spectre console](https://spectreconsole.net/) for more pleasent way to show the menu and the options.
    - Enforce user correct input using the Spectre.Console Ansi.Console
    - User can cycle between menu options with keyboard arrows

    ![1](https://github.com/user-attachments/assets/ea094aa9-17d0-4973-8f01-3e501aa23c20)

    
- CRUD  functions queris against the database
    - Users can insert records with habit name, quantity,quantity measurement unit  and a ocurrance date in the format “yyyy-MM-dd”
    - Users can view all the records shown in a table in pleasent way
    
![2](https://github.com/user-attachments/assets/da89fded-7317-4f9f-81c9-7e841801cbc5)

    
    
- Basic report functionality with a table to show each report in a pleasent way using microsoft [**Table Class**](https://learn.microsoft.com/en-us/dotnet/api/system.windows.documents.table?view=windowsdesktop-8.0)
    
  ![3](https://github.com/user-attachments/assets/c29f0a0c-118c-4cf6-9bda-50d81d5f31d2)

    

# Challenges

- It was my first time using SQLite3 in C#,I had to revise the SQL syntax and the SQLite3 support for the main SQL syntax , and also i had to learn how to use it in C#
- I had to learn how to parse return values of select queries and format it in a good way for the user
- The reports needed some specific SQL queries to make them work , and i had to learn that as well.

# **Areas to Improve**

- I think I can work on tidying up my code more and apply the coding principles i mentioned before better than what i add
- I used classes very basically and have not delved deep in what they really excess at I need to work on my OOP side of coding
- I tried to utilize Method Overloading in the SQLquery class but i had some trouble , i could work on that as well.

# Resources Used

- Help from CSharpAcademy Discord community [**The C# Academy](https://discord.com/invite/aDMDET8ywB)**
- [Basics learnt from this website for SQLITE3 in C#](https://www.sqlitetutorial.net/?s=C%23)
- [Microsoft documentation for SQLite3 to setup in my coding environment](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=net-cli)
- Some stackoverflow to help me fix issues in my code
- [How to handle unique constrains exceptions](https://enterprisecraftsmanship.com/posts/handling-unique-constraint-violations/)
- [Learning about parameterized queries](https://reintech.io/blog/mastering-parameterized-queries-ado-net)
