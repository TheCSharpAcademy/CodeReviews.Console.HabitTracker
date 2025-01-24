ConsoleTimeLogger
My first C# application, and first time using Visual Studio.

Console based CRUD application to track time spent coding. Developed using C# and SQLite.

Given Requirements:
 When the application starts, it should create a sqlite database, if one isn’t present.
 It should also create a table in the database, where the hours will be logged.
 You need to be able to insert, delete, update and view your logged hours.
 You should handle all possible errors so that the application never crashes
 The application should only be terminated when the user inserts 0.
 You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework
 Reporting Capabilities
Features
SQLite database connection

The program uses a SQLite db connection to store and read information.
If no database exists, or the correct table does not exist they will be created on program start.
A console based UI where users can navigate by key presses

image
CRUD DB functions

From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in mm-DD-yyyy format. Duplicate days will not be inputted.
Time and Dates inputted are checked to make sure they are in the correct and realistic format.
Basic Reports of Cumulative hours

image
Reporting and other data output uses ConsoleTableExt library to output in a more pleasant way

image
GitHub for ConsoleTableExt Library
Challenges
It was my first time using C#, Visual Studio or SQLite. I had to learn each of these technologies from the beginning in order to complete this project.
DateTime was a hurdle to get over. I had to learn how to parse into and from DateTime into either more storable or human readable formats. I also had to learn how to subtract or add time together for reporting functionality.
There was also a globalization issue with DateTime. My mentor from Australia had an issue inputting Dates in the format my program wanted. Resulting in him never being able to input a date. I was able to resolve this with adding a Culture argument to the DateTime parse.
Spaghetti code was a big issue at the beggining. I had just functions everywhere. After the first couple days of the project I decided to start from scratch and started to implement classes instead. This helped clean up the code to make it more readable and changeable.
SQLite. Even though I mentioned it above, it is worth mentioning again by itself. I had not used SQLite before and had never used C# to interact with it. I had to learn how to create commands to do all the CRUD operations and how to execute those commands. I also had to learn some SQL to use the proper SQL commands to SELECT, UPDATE, INSERT or DELETE along with modifiers to SELECT or DELETE only the desired rows. The reports also heavily relied on correct SQL statements.
Lessons Learned
Create a basic map of what some of the objects in the program should be and some of their basic methods before starting to code. I think this would help in not making spaghetti code from the beginning.
Do it the right way the first time. There were several instances where I just wanted to see if something would work so I did it in a quick way in which I knew it would need to be fixed or cleaned up later. 9 times out of 10 later never came. This piled onto itself and caused the spaghetti code to get worse.
Have a testing grounds readily set up. This way small things can be tested seperately from the within the app. This can speed up creation. For example, I was learning how to parse and use DateTime. A testing ground just to check parseing from and to DateTime could help speed up coding, since I would not need to compile this application each change/test and go through the UI.
Always create a new git branch for each issue or feature that is currently being worked on. This helps me stay focused on that single issue or feature so that I can merge that branch. And there is always satisfaction with switching back to main and merging the branch.
Seperate user input from other methods. At first I made methods that would be called, take user input within the method then alter that data within the same method. This made my code less re-usable. I should seperate these two things, I can have a method first get the user input and validate it, then send it to another method as an argument to do any alteration or display needed. This means I could reuse the second method even if I am not getting user input first, or the first method if I want user input but not take it along the same route.
Areas to Improve
I want to learn more about using the text editor portion of Visual Studio. I am use to Visual Studio code, and in VScode I know how to make multiple cursors, select all of a specific string or select the next occurence. There were many times in Visual Studio where utilizing this sort of functionality would have been very beneficial, but I did not know how to do it, or if it is even possible.
Learn more about code snippets. I currently only know the code snipping for Console.WriteLine();. I should learn more, as this did spead up my coding. I should look to see if there is one for Console.ReadLine(); at the very least.
single responsibility. I did improve at this along the way, but I still have some work to do on it. I think I could have made my methods a little better so they only had a single use.
Method overloading and out arguments. I did start utilizing these during the end of this project. But I should work to see where they can be used more in my next project as hey helped me to clean up my code a bit and the out arguments helped me a lot in the logic of telling if a user input was in a correct format or not.
Resources Used
The help and advice of my mentor Cappuccinocodes
Mosh C# for beginners youtube video
Zetcode to get the basics of SQLite with C#
MS docs for setting up SQLite with C#
MS docs for DateTime
CodeCademy C# course to get some basic practice with C# variables, methods and classes.
Various StackOverflow articles
