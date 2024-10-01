Hello.
#Project: Console Habit Logger:
my first C# CRUD application  Developed with {C# , SQLite, Rider}
The idea is simple you just make habit logs using previous types you made or make a new one.

#Requirements:
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habits will be logged [Habit Name , Time, Quantity(optional paramter)].
- [x] You need to be able to add, delete, update and view your logged habits. 
- [x] You should handle all possible errors so that the application never crashes 
- [x] The application must not be stopped unless the user types the exit option which mean i must handle as much edge cases as possible.
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework

# Features:
* Connection with a local SQLite databse
     - the Program uses a SQLite database and connects with ADO.NET NUGET package ( Framework )
     - if no database exist a new table is already made which is easy in ADO.NET since is it automatically handled
     
* CL-Interface
    - the whole application is made with the console so no fancy UI stuff actually just a few keystrokes to get what you want
* CRUD DB functions
    - the main functions you can do in the app is just Creating ,deleting,updating,reading habit logs
    
# Challenges
    - First time doing real C# application with a database
    - No prior Knowledge in SQL so had to spend time while development learning basic commands : DELETE, SELECT, UPDATE, CREATE, INSET DISTINCT and etc
    - Rapid Development . due to other life activities i am busy so the project had to be done as soon as possible to me my DeadLine
    - Technical Dept. writing spagetti code or copying and pasting repeated function instead of making them as functions for more convenience making the project much more Compicated to work with especially at the end of the project when you are tempted to increase your speed which by default creates messy code.

# Lessons Learned :
    - Before Jumping straight into the project just allocate some time in you schedule to brainstorm the over all architecture and functionality of the program.
    - From the beginning make sure your code is well organized and structured and make things more modular in other terms instead of habit 2 files maximum for the whole application try to make more files for instance one for habits and one Database operations and one for the interface and the program itself.
    - some things might seem simple or useless as a beginner like debugging although it saved lots of time instead of having to print every single variable i just check not only values of variables but also the flow of the program itself and even when error messages are not much meaningful you can observe when the project collapsed and fix it.
    - Make Input Methods much of development time was wasted copying and pasting input handling code blocks which also made the project a huge mess which would have been avoided if i used input methods and put them in a class like "InputManager" .

# Areas to Improve:
    - Learn more about Code snippets since I know NONE of them which would make it much easier and faster to write code.
    - Project Design and architecture and writing code itself. must learn more about good programming habits like DRY- Do Not Repeat Yourself and learn more about the Solid Principle;
    - Time Management . Make more balance between School and code since i end up damaging one everytime.
# Resources used:
- [MS docs for DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime?view=net-5.0)
- Various StackOverflow articles
- Chatgpt for basic sql commands learning
- C# academy TimeLogger project for learning how to make a decent Readme
