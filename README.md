# Habit Tracker
This is a console application for tracking habits. The application allows users to view, insert, update, and delete habits, as well as seed the database with random data.

Developed using C# and SQLite.

# Features
- View all habits
- Insert a new habit
- Update an existing habit
- Delete a habit
- Seed the database with random habits

# Given Requirements:
- [x] This is an application where you’ll log occurrences of a habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] Users need to be able to input the date of the occurrence of the habit
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- [x] Follow the DRY Principle, and avoid code repetition.

# Challenges
	
- I have never worked with a database outside of native queries. Interacting with SQLite programatically helped grow my mental model of software development. 
- I couldn't get over the idea that there had to be a better way to interact with the database.
- My code feels like spaghetti but I don't exactly know how to go about fixing it. 
	
# Lessons Learned
- In response to my second challenge, I learned a bit about ORM's and am looking forward to using them in the future. 
- Try to separate concerns as much as possible. 
- Give my methods useful and clear names. 
- Query parameterization! 

# Areas to Improve
- I want to learn more about ORMs and interacting with the Data layer.
- Separation of concerns
- Create functions with less side-effects, or single responsibilities. I feel I have a lot of "super functions". 
- Better error handling. I wrapped certain DB interactions in Try-Catch but this is not robust. 

