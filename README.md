# HabitLogger

An console application to log when you drink water so that you can build the habit of drinking water more often.

This is my first C# CRUD application. It was built using the Microsoft.Data.SQLite package and a SQLite database.

## Application Details
- When the application starts, it creates a sqlite database and the necessary table, it the database file does not exist.
- Users are able to read the logs in the database, insert a new log, delete a log in the database, and update a log in the database.
- The application lets users know whether a CRUD operation was successful and has validation checks.
- The application performs to operations on the database using RAW SQL commands.


## Walkthrough

A simple walkthrough of how the application works.

It starts by displaying the application introduction and a menu of operations that the user can perform:

![image](https://user-images.githubusercontent.com/69612398/209496080-f3828e7b-23ea-4b81-ba07-34d3842348d8.png)

If you choose to display the current logs by choosing option 1:

![image](https://user-images.githubusercontent.com/69612398/209496252-0e35a92a-8f87-4fd1-ac04-d208ab5d6e94.png)


The application ends by typing 0 which will exit the application:

![image](https://user-images.githubusercontent.com/69612398/209496300-9b150f51-4441-41ff-9b8e-c9dab4b2031f.png)


## Challenges
- Challege 1:  Since this was my first time using a database in C#, I had to familiarize myself with the Microsoft.Data.SQLite package and the correct way
to work with a SQLite database. For example, I didn't know about the using statement and how it was used but after reading about it, I learned that
it is similar to a try-finally block where it will automatically cleaned up with SQLConnection class' Dispose method.
- Challenge 2: Figuring out the right way to structure my application was a hurdle I had to overcome. At first, I was going to implement all the database operations
in the program runner (HabitLogger/Program.cs) but I decided that it would make more sense to separate the console application and the database operations
into separate files. Therefore, I made a HabitLoggerLibrary/HabitLoggerConnection which is a class that connections to the database and performs all the database
operations.
- Challenge 3: Figuring out how I should structure my HabitLoggerConnection class is challenge that I had to handle. While designing to the class, I started to design for cases that my application will never encounter. For example, I started thinking about handling cases where multiple threads would use the HabitLoggerLibrary, but I realized that I was over engineering the library. So I took a step back and remembered to follow the KISS principal (Keep it simple stupid). I ended up keeping the class simple and make it work for the console application. 

## Lessons Learned
- I learned more about the IDisposable Interface and how useful it is. For example, using statements and objects of classes that implement IDisposable work great together since the object's Dispose method is called after leaving the scope it was created in. I found amazing since it made my code a lot more readable and concise than having to use a try-finally.
- I learned more about working the SQLiteConnection and the SQLiteCommand class which I had no experience working with before. It was a new experience to run SQL commands through these packages.
- I was able to work with RAW SQL commands and improve my SQL skills which I found to be a great experience. I know working with RAW SQL is not a good idea since it leaves your application prone to SQL Injection but I found it to be a great experience nevertheless. 
- I learned to improve my decsion making when it comes to designing a project and to not over engineer for situations that will not be encountered.

## Areas to Improve about myself
- When it comes to working with Visual Studio, I still have to get better at working in Visual Studio and the different tools that offered in the IDE that I have to take advantage. I have the basics down such the different views avaiable and debugging tools but I still have a lot to learn about Visual Studio.
- While I tried to make sure that each method had a single responsibility to reduce redundancy, I still have to improve on this skill and learn to implement the SOLID princiapls better. I made multiple methods that did similar operations with small differences which I could refactor.


## Areas to Improve about the project
- Improvement 1: Not using RAW SQL commands would be one area to improve. Using a tool such as EF or Dapper would be able to fix this which I still have to learn.
- Improvement 2: The design of the HabitLoggerConnection class. At the moment, if I create two instances of the class it will cause problems since I'm opening the database twice. A good fix for this would be to implement a singleton design where only one instance of the HabitLoggerConnection class can be used at any time.
- Improvement 3: Expanding on the project and adding more features. At the moment, the project only logs the amount of water the user drinks but there are improvements such as categorizing different habits and tracking each one. In the future, I can add this.


## Resouces Used
- For the project idea - [C# Academy HabitLogger project](https://www.thecsharpacademy.com/project/12)
- For information about the SQLiteConnection class - [Microsoft.Data.SQLite documentation](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
- For SQL practice - [W3schools SQL Tutorial](https://www.w3schools.com/sql/)
- Learning Resources for C# - [Plural Sight](https://www.pluralsight.com/)
- Various StackOverflow posts
