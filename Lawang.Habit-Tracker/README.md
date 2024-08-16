
# Console.Habit Tracker

This is the Console based CRUD application using .Net environment and
sqlite as Database. 

## Requirements

- When the application starts, it should create a sqlite database, if one isn’t present.
 
- It should also create a table in the database, where the hours will be logged.
- You need to be able to insert, delete, update and view your logged hours.
- You should handle all possible errors so that the application never crashes
- The application should only be terminated when the user inserts 0.
- You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework

## Features
* Can simply be navigated by pressing the number keys, moreover the interface is self explainatory.
* SQLite database is used to store and read data from.





* #### Screen shots:

* 
![Screenshot from 2024-08-16 13-50-04](https://github.com/user-attachments/assets/950501d6-137f-4732-9d3c-49cf15e01d72)

* Users can Perform CRUD function on the database.
* Users can enter date in (dd-MM-yy) format and enter the distance (in km) ran on that day.

* ![Screenshot from 2024-08-16 13-50-32](https://github.com/user-attachments/assets/c6f9a311-fa1e-4c3c-9587-7a3fece5ac83)

- Data is presented to user in Table format, using the external library Spectre.Console.



## Project Summary
#### What challenges did you face and how did you overcome them?

* First thing first, I didn't even knew where to start the project from, so i started breaking the project down to its requirement:
  * It requires database to store and read data -> For this project suggested me to use sqlite, so I learned about sqlite about how its different from other database and lightweight.

  * To interact with the database we needed something in .net to interact with database. so, as project suggested I started learning about ADO.NET.
  * learning ADO.NET was a hurdle in itself so I reffered the Microsoft docs, but still faced difficulty understanding concepts so, I jumped to Youtube tutorials if I had any problems understanding the concept in Microsoft.docs.
* When Implementing the database with ADO.NET I had some intital error, and used the (try, catch block) to capture the error 
* But the real problem started here the connection to database reamained opened when error was caught, so to tackle that problem i saw the actual usecase of "finally" in try, catch, finally trio. "finally" blocked was used to close the database connection.

* Since the data was read and displayed in the terminal, I was dissatisfied with my custom table, so i used the external library Spectre.Console.

* Spaghetti code was the issue when I tried to put all code inside Program.cs, So i made a different database class just to deal with database and used Program.cs to deal with users Input. By doing thisI achieved less coupling and sepeartion of concern.
* Always create a new git branch for each issue or feature that is currently being worked on. This helps me stay focused on that single issue or feature so that I can merge that branch. And there is always satisfaction with switching back to main and merging the branch.

* Dealing with DateOnly object was a hassle, Project suggested me to use DateTime Object but I had no requirement of Time Object so I used DateOnly object.Again to tackle this problem Microsoft.Docs was used.




## 🛠 Skills Learned
#### ADO.NET 
* I had an experience with Entity-Framework Core while building web based CRUD application, but had no Idea how did the functionality provided by the EF Core implemented. Working on this project gave me a high on overview that EF core implements ADO.Net not only that i learned to use ADO.NET classes to perform basic CRUD operation. 

#### Spectre.Console
* As mentioned above, My own custom class was hedious to look at as it was hard to achieve alignment among different record. So, I learned to build table in Spectre.Console. Although I am not proficcient in using this terminal library but for now building the table is enough and hope to learn more and implement in future projects.

#### SQL
* I already mentioned I had previous experiences with EF Core, which lets you use database without SQL statement, so to interact with database in this project using ADO.NET I needed to use raw SQL statement, and had to learn and implement it for basic operation.


## FAQ

#### How to beautify the table in the project?

Answer I used the Microsoft.Spectre.Console package, which you can get for Nuget package manager. Install it and add Reference to your project. 

For more information u can visit the docs https://spectreconsole.net




## Feedback

If you have any feedback, please reach out to us at depeshgurung44@gmail.com

