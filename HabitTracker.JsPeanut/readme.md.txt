# HabitTracker

This is a console CRUD application, in which you can track your habits. This application was based on the [Habit Tracker App tutorial](https://www.youtube.com/watch?v=d1JIJdDVFjs&t=1245s) by [The C# Academy](https://www.youtube.com/@thecacademy5376), since I had no previous SQLite knowledge.

# Features

- The application has a menu in which users can choose the CRUD operations to perform![Menu](https://user-images.githubusercontent.com/110200246/233725165-d12ca3f1-cd95-4e34-b50c-3ab26feabd8e.PNG)
- CRUD operations
  - Users can perform CRUD operations, such as creating, reading, updating and deleting habits.
- Statistics/report function
  - Users can see the habits they've been the most and the least consistent with.

# Challenges

- One of the challenges I faced was trying to make my application more "original", since I felt it was a copy of the tutorial. I'm not sure if I could have made it more "original".
- The report functionality.
  - I had to search how to select the rows which had the same name in SQLite, as a way to check if a user had performed a habit more than once, and if so, how many times.
- Parsing methods.
  - Had to learn about the parsing methods during the process of developing the application.

# Resources I Used

- [6 ways to select duplicate rows in SQLite](https://database.guide/6-ways-to-select-duplicate-rows-in-sqlite/)
- [DateTime Microsoft Docs, which include parsing methods](https://learn.microsoft.com/es-es/dotnet/api/system.datetime.parseexact?view=net-7.0)
- [Habit Tracker App tutorial by The C# Academy](https://www.youtube.com/watch?v=d1JIJdDVFjs&t=1245s)