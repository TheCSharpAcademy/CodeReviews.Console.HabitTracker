# Console Habit tracker
My first encounter with databases. Also first time installing packages (NuGet this time).
Console based CRUD app to track various habits. Developed using C# and SQLite.

# Given requirements
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
- [x] Your project needs to contain a Read Me file where you'll explain how your app works.

## Optional challenges
- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

# Features
* SQlite database connection
    - The program uses SQLite db connection to store and read information.
    - If database file or desired table doesn't exist, it will be created when the program starts.

* A console based UI
    - Users can navigate through menu by entering numbers
    - There are two menus, Main menu and Question menu
    -  When inserting, updating, or deleting a record, a confirmation message is shown
    - After an invalid input message, user is asked to press any key
    - Clearing the console screen is implemented to make the best possible user experience

    - ![image](https://github.com/czerviik/CodeReviews.Console.HabitTracker/assets/137193704/cad8a8c0-5962-4302-9ee0-e6cb85e5b742)
    - ![image](https://github.com/czerviik/CodeReviews.Console.HabitTracker/assets/137193704/6e4084ef-4661-4e85-8083-913ba9e5b5bd)

* DB functions
    - Users can create new tables for habits they wish to track
    - Each database table consists of 3 columns: Id, Date (dd-MM-yyyy) and Quantity
    - The Quantity column can be named as users wish based on intention of what habit they wish to track
    - Users can create, read, update or delete entries from a habit they choose
    - Users can view a sum, average, minimum or maximum of Quantity of a habit they choose, whether by a specific year or from all entries

# Challenges

  - Setting the project took me a while as I didn't get why should I change the project's working directory as said in the tutorial.
  - At first I had to undestand basics of SQLite with which I'm still not perfectly familiar, but improving.
  - Setting a database connection felt quite messy to me at the beginning but I manage to understand it later on. 
  - Though it is recommended to not to use classes in this project, I felt that it would be benefical. Refactorization into classes is still a matter of consideration.
  - When I moved to challenge tasks, I had to restructurize some methods and console outputs because of the functionality of adding more than one habit.

# Lessons learned

  - Instal new packages. Should look for them more often and try to combine them. 
  - Think about encapsulating functionality into classes right from the beginning. This is not the first time I'm regreting it.
  - Practice SQL commands more before using it in the code. It saves time at the end of the day.
  - During the project I learned some basic git commit description conventions.
  - Learned some new C# principles (new to me).
  - Write this readme.md file.
