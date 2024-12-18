# Habit Logger Console App

Console-based app that uses CRUD operations against an SQLite database to track user habits.

### Users can:
- [x] Create a new habit
- [x] Add, update, or delete records to existing habits
- [x] View all records in a habit in a table. Includes record id, quantity recorded, and the date.
- [x] View various statistics in a Report option in the main menu.

### Features:
- [x] Speech-to-Text Voice Recognition mode using Azure's Mircosoft.CognitiveServices.Speech library
- [x] paramaterized queries (where possible) and input-validation where not mitigate the risk of sql-injections
- [x] no known reasons for the app to crash currently

### How to Use
  - Build and run as a dotnet application
  - use `--voice-input` command-line argument for Voice-Recognition mode

### Project Requirements 
 - [x] This is an application made to log occurrences of a habit.
 - [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day).
   - [x] Users can create their own habits and set their own unit of measurement.
 - [x] Users need to be able to input the date of the occurrence of the habit.
   - [x] Option to select 'today' for convenience.
 - [x] The application should store and retrieve data from a real database (sqlite version 3 used)
 - [x] When the application starts, it should create a sqlite database, if one isn’t present.
   - [x] It should also create a table in the database, where a sample habit will be logged.
 - [x] The users should be able to insert, delete, update and view their logged habit.
 - [x] Project hould handle all possible errors so that the application never crashes.
 - [x] App can only interact with the database using ADO.NET. Can’t use mappers such as Entity Framework or Dapper.
 - [x] The project needs to contain a Read Me file.

## Challenges / Lessons Learned:
- new to sql and sqlite
- ms ado.net SQLite ["hello world" documentation](https://github.com/dotnet/docs/blob/main/samples/snippets/standard/data/sqlite/HelloWorldSample/Program.cs) 
        was out-of-date. The examples used .net 6, not .net 8
        -> and didn't release the file after I created / edited it for deletion. 
        -> now, I know to clear .ClearAllPools() to release the file handles. 
      Hours of frustration later taught me about file handles, and I contributed a single line of code to the official MS docs to keep them up-to-date.
- familiar with the idea of classes to represent "objects" (like a calculator), 
        but conceptually new to using oop for things like creating a connection layer, etc. 


## Areas To Improve:
- Plan Ahead!
  - Many of my methods mix getting user inputs and interacting with SqliteConnections to the database.
    -  I did start implementing optional parameters to help with input validation of dates towards the end, but it would've saved me a lot of time and repetitive to plan for these types of issues from the beginning.
  - **For The Future:** separate these into different methods and different classes from the beginning, rather than at the end as an afterthought.
- Refactor Even Further. 
  - Could I have used enumeration to simplify?
  - ie, enumerate either menu options or sqlite queries until methods like:
    - create / edit / delete a habit become one main HabitInteraction() method with different, potentially optional parameters that call more specific methods when needed?
             
- More Advanced Use of Git Branching
  - Separate new features / bigger issues into a seaparate branch that will later be merged with the main, as opposed to just making commits when I'm done coding for the day / done with a major section of code.
    - Branching off of master like this could make it easier to review the development process later + is more realisticly close to what would be expected of me in a professional environment.
      
- Are there better ways of testing?
  - I still largely debug by running the app and seeing what pops up on the screen. I have now leveled up to:
    - being comfortable with using breaklines, looking at local variables, and stepping over / into a function when debugging. But what else can help me improve?          
  + Buuuuut ... some of these options weren't available for checking if my sql queries made sense or had syntax issues. 
  + I guess the first question of any new concept should be - "how do I debug / get unstuck / look at or interact with this thing I'm using"?

## Resources used:
- sqlite tutorial [here](https://www.youtube.com/watch?v=HQKwgk6XkIA)
- MS [ADO.Net docs](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview)
- [sqlite docs](https://www.sqlite.org/docs.html)
- chatgpt for answering dumb questions. especially about sqlite syntax issues.
  
  
