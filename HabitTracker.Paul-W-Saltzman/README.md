# HabitTracker.Paul-W-Saltzman

Console based CRUD application to track Habits.
Developed using C# and SQLite.

# Requirements
 - [x] This is an application where you’ll register one habit.
 - [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
 - [x] The application should store and retrieve data from a real database
 - [x] When the application starts, it should create a sqlite database, if one isn’t present.
 - [x] It should also create a table in the database, where the habit will be logged.
 - [x] The app should show the user a menu of options.
 - [x] The users should be able to insert, delete, update and view their logged habit.
 - [x] You should handle all possible errors so that the application never crashes.
 - [x] The application should only be terminated when the user inserts 0.
 - [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
 - [x] Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:
   
# Additional Challanges to push Programming ability
  - [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
 - [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

# Features
* SQLite data base connection
  - The program uses a SQLite db connection to store and read information. 
  - If no database exists, or the correct table does not exist they will be created on program start.
  - If there is no information in the database the program will load some starter habbits and measurments.
    
* A console app where the user can navigate by key presses.

* CRUD DB Functions
 - All inputs are checked and sanitized to make sure data stays formatted
 - All non-queary calls to the database are tried and any errors are caught and displayed to the user.

* Test Mode
  -Test mode allows a product tester the ability to load random data for testing.
  
* Themes in settings

  # Challanges
- This was my first app where I directly dealt with a SQLite Database.  Syntax while familiar took a litte while to figure out.  This is the reason I ended up cathcing any errors and out putting them to the string.
- Dates are alwayse a bit fun, Making sure the user enteres the right information and the corect information comes back from the database.
- I'm still working out where functions should go and how to break them up.  I ended up refactoring the program after all functionality was complete to make functions easier to find and make a little more sense.

  # Lessons Learned
- While I went through some basic planning for this program I think I'll probably spend more time planning out my next program.  Cowboy coding caused me to repeat code in multiple places in a way that if I think I rewrote this code I feel like I could reduce the amount of code significantly.
- There where a couple points where I probable should have branched when I was refactoring.  I'll do better in the future.
- It's a small thing but I really like the new console writeline format that allowed me to put out the table in a readable format -Thanks Stack Overflow.

  # Areas to Improve
- So Many
- The main one is planning.  I learned a lot going through this app, but a good plan of attack would have prevented some of the harder lessons.
- I think the main thing other than planning I could improve on is just thinking like a developer. As I found solutions for my issues there where solutions that I didn't think of.
    Where I'm preloading some data I came across the idea to sent the sql command to a function where I could catch the errors.
    I found this looking for a solution to writing a catch block for each sql command.  It hadn't even occured to me that I could write a function to send any sql statement to.

  # Resourses Used
- C# Academy (https://www.thecsharpacademy.com/project/12)
- C# Academy lession YouTube (https://www.youtube.com/watch?v=d1JIJdDVFjs&t=1225s)
- Various StackOverflow articles
  
  

  

