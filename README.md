# Console HabitLogger application:

Simple habit logger application made for The C# Academy following the given specifications

## Requirements

- This is an application where you’ll log occurrences of a habit.<br/>
- This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)<br/>
- Users need to be able to input the date of the occurrence of the habit<br/>
- The application should store and retrieve data from a real database<br/>
- When the application starts, it should create a sqlite database, if one isn’t present.<br/>
- It should also create a table in the database, where the habit will be logged.<br/>
- The users should be able to insert, delete, update and view their logged habit.<br/>
- You should handle all possible errors so that the application never crashes.<br/>
- You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.<br/>
-  Follow the DRY Principle, and avoid code repetition.<br/>
- Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:<br/>

## Optional Requirements

- If you haven't, try using parameterized queries to make your application more secure. (This one was not done)<br/>

- Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.<br/>

- Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database<br/>.

- Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.<br/>

## The program

In this program you see a main menu that asks you for an input and it executes tha basic CRUD operations, create, read, update, delete and also an extra function that filters the type of habits, this extra option was my poor attempt for a report function, its very basic and its not what I would have wanted but I realized too late that the kind of information I was saving was a bit problematic to work with so I decided to go for this simple reporting function instead of giving a wider range of reporting options as I initally wanted, like filtering by year. 

##What I accomplished:

In this project I tried to follow the specifications as much as I could but I had difficulties because for my liking, some things where a bit vague, like what information we were exactly working with, that made me go back and forth a bit trying to decide exactly what to do and how, specially trying to figure what to do for the second side challenge, I am not sure if my solution is the best and if it follows exactly what was intended with them but I think it works well.
I also think I didnt understand the logic behid the parametrized queries initially, I thought I did but later I realized I didnt, if I understand it corectly the parametrized queries revolved around the usage of the parameters collection of the sqlCommand class, and I realized that after writing the program, so its not implemented in this exercise.
Regardless of the quality and level of this program, I feel I learnt a lot from it, its the first time I work on this kind of program on my own without following a step by step tutorial so it was a very fruitful project in that regard, and im looking forward to keep doing more proejcts to apply what I learnt and keep acquiring mileage programming
