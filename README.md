# Habit Tracker

## Introduction

This is a very simple app that will teach you how to perform CRUD operations against a real database. These operations are the base of web-development and you’ll be using them throughout your career in any most applications. We think it’s very important to do it from the start of your journey, since everything that will happen from here is just adding complexity to CRUD operations. No matter how complex and fancy the app you’re building is, in the end it all comes down to executing CRUD calls to a database.

For that you’ll have to learn very simple SQL commands. I know it sounds scary, but you’ll be amazed about how little SQL knowledge you need to build a full-stack app. Don’t worry, we will take you by the hand and by the end you’ll have completed your first fully functioning CRUD app. The most common ways of calling a SQL database with C# are through ADO.NET, Dapper and Entity Framework. We will start by using ADO.NET, because it’s the closest to raw SQL.

If you think this project is too hard for you and you have no idea where to even start, you’re probably right. You might need an extra hand to build a real application on your own. If that’s the case, watch the video tutorial for this project and then come back and try it again on your own. It’s perfectly ok to feel lost, since most beginner courses don’t actually teach you how to build something.

So let’s go!

## Requirements

- This is an application where you’ll register one habit.
- This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- The application should store and retrieve data from a real database
- When the application starts, it should create a sqlite database, if one isn’t present.
- It should also create a table in the database, where the habit will be logged.
- The app should show the user a menu of options.
- The users should be able to insert, delete, update and view their logged habit.
- You should handle all possible errors so that the application never crashes
- The application should only be terminated when the user inserts 0.
- You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
- Your project needs to contain a Read Me file where you'll explain how your app works.

## Features

### SQLite Database Connection

- This project utilises a SQLite database to store the information input by the user.
- If no database exists at the point this project is run the project will create a new database and seed it with 50 randomly generated records.

### Console App

- This is a console app that works using the users operating system terminal to take inputs and provide the output data.

### CRUD functionality

- The purpose of this project was to introduce myself to the CRUD functionality. CRUD stands for Create, Read, Update, Delete and is the foundation for inputting and amending data within a database.
- Users are able to use CRUD through a main menu interface that triggers different methods based on the user input.

### Validation

- This app contains multiple different ways of validating the data provided by the user such as checking a number range, trying to parse a date into a string and checking if the database contains a record before proceeding with any actions.

## Lessons Learned

Throughout the development of this project there were many new tools that I needed to learn to effectively complete the requirements. Below is a list of the lessons I learned:

- Always plan the project from the start and break each requirement into smaller parts. I spent the best part of 3 hours just trying to write code without thinking how it might link into other functions.
- Don't try and create validation from the start. It seems to be better to try and build the barebones of the project before trying to figure out how the user input might be validated.
- Regular expressions are incredibly powerful but also incredibly complicated. Initially tried to validate date inputs using regex before finding a simpler way through TryParse.
- Following on from the above point. KISS: Keep It Simple Stupid.
- SQLite was new and finding good places on the internet to get examples of syntax use was challenging. I began using ChatGPT to get some examples and then made sure to read all the documentation that Rider provided on tooltips to understand what each function was doing.
- Spend more time getting to know the IDE. I am using JetBrains Rider and needed to change the working directory of the project. This turned out to be very simple to do but took me over an hour to find the correct menu option.
- DateTime is incredibly convoluted.

## Areas to Improve

- Become less reliant on YouTube videos and start to try things independently. This project initially started using the YouTube video available on The C Sharp Academy project page and then I tried my own ideas after getting a nudge from that video.
- Need to start being comfortable with Object Oriented Programming. Do not currently feel comfortable creating classes for everything.
- Finding time to commit entirely to coding exercises.
- My SQL knowledge is very limited and I needed to keep visiting websites to write the correct scripts.

## Resources

- [Markdown Cheatsheet for Readme file](https://github.com/lifeparticle/Markdown-Cheatsheet)
- [CSharp Academy Project Page](https://thecsharpacademy.com/project/12/habit-logger)
- [YouTube video by CSharp Academy](https://youtu.be/d1JIJdDVFjs?si=S8jiMEhyH0FFbfe0)
- [ChatGPT to generate examples](https://chat.openai.com/)
- [Microsoft Learn - Sqlite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
- [Microsoft Learn - DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime?view=net-8.0)
- [W3 Schools - SQL](https://www.w3schools.com/sql/)
