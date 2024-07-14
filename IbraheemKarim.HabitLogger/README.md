# WaterDrinkingHabitTracker
This is a simple console app made using C# & Visual Studio, sql server, and ADO.NET to connect between the app and a sql server database. The app makes use of CRUD operations to enable the user of recording a habit (in our case, the number of water glasses that they drink on a given day) that will be stored in the database. The user can show all the previous records of the habit, and they can also delete or update these records.
# Requirements
- The app should register one habit.
- The app is supposed to use a real database.
- When the app starts it should check if there's a database already, and if there isn't one, it will create a new database and a table.
- The app should show the user a menu to choose what they want to do.
- The users should have the ability to insert, delete, update and view the recorded habit.
- The app is supposed to contain proper error handling, and to not allow incorrect input.
- Using mappers such as EF Core is not allowed, and interactions with the database should happen only using raw SQL queries.
- The project needs to include a README file (yeah this is the README file we're talking about).
# What did building this project add to me?
- I Consolidated my knowledge of ADO.NET and raw SQL queries by putting many things into a fully functioning application at once.
- I learned about the existence of SQL commands that create databases and tables only if they don't exist.
- This is the first time I write a README file that describes what I did in my project. So, by doing this project, I practiced on conveying my thoughts and message and explaining my project, and not just writing code.
# Areas that can be improved
- I'm sure that there might be a way to manage the dependencies between classes in a better way or to reduce them if i kept on reviewing the project.
- Maybe I could've also designed the project in a better way, but there's always a tradeoff between the perfect design and resources like time, and I thought it was enough.
- Some methods might need some refactoring or need to become smaller to have less responsibility. 
# Resources Used
- The instructions and project requirements found on the great csharpacademy website: https://www.thecsharpacademy.com/project/12/habit-logger
- My previous knowledge gained through watching courses on the youtube channel of the amazing .NET tutor Issam Abdelnabi: https://www.youtube.com/@Metigator, and reading some of MS docs. And also my general knowledge of programming basics.
- Other than that, of course I used the help of google and AI tools like ChatGPT to remind me of things I had forgotten or to find missing pieces of information I needed. 
