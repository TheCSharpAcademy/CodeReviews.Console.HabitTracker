# <span style="font-size:larger;">Console Habit Tracker</span>

This was my first time using SQL database in C# project. 
Console based CRUD application that tracks how many cups of coffee is consumed per day.
Developed using C# and SQLite.

# <span style="font-size:larger;">Given Requirements:</span>
* This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day).
* The application should store and retrieve data from a real database.
* When the application starts a database will be created if it doesn't exist yet.
* It should also create a table in the database, where the habit will be logged.
* You need to be able to insert, delete, update, and view your habits.
* The application should only be terminated when the user inserts 0.
* You can only interact with the database using raw SQL. You can't use mappers such as Entity Framework.
* The app should show the user a menu of options.
* You should handle all possible errors so that the application never crashes.
* Your project needs to contain a Read Me file where you'll explain how your app works.

# <span style="font-size:larger;">Features:</span>
* CRUD functions
	- ◦ Insert a new habbit.
	- ◦ Delete exsisting habbit.
	- ◦ Update exsistings habbit date or quantity.
	- ◦ View all stored habbits
*  Console based UI, where navigation is done through menu choices.
* SQLlite connection
	- ◦ Program uses SQLlite database to store and read information.
	- ◦ Program will create the database file if it doesn't exsist. And it will create the habbit tracker table if it doesn't exsist in database.

# <span style="font-size:larger;">Challenges:</span>
* First time using C# for any SQL operations was quite fun.

# <span style="font-size:larger;">Lessons Learned:</span>
* How to mostly use SQLite with C#.

# <span style="font-size:larger;">Areas to improve:</span>
* Reduce redundancy in loops for menu choices and inputs. A higher order function could be useful here.

# <span style="font-size:larger;">Resources Used:</span>
* [MS Docs for Sqlite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)