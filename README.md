# HabitLogger

Console based CRUD application to log and track any habit. Developed using C#/.NET and SQLite

# Requirements: 

## Initial:
- [x] Register a single habit

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

## Extra Challenges:
- [x] Let the users create their own habits to track.

- [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?).

# Features

- SQLite database connection

  - The program uses a SQLite db connection to store and read information.
  - If no database exists, or the correct table does not exist they will be created on program start.

- Console based UI with all available commands

![image](https://user-images.githubusercontent.com/64802476/221361596-0abe2117-5f70-4902-a5ee-aec2092efb22.png)
![image](https://user-images.githubusercontent.com/64802476/221361768-23ff3cff-cf09-4c49-b8df-2961a7c84065.png)


- Insert and track multiple habits

![image](https://user-images.githubusercontent.com/64802476/221361731-9dc9b708-5b7b-4c08-98c6-495649b78d56.png)

- Generate yearly reports

![image](https://user-images.githubusercontent.com/64802476/221361941-6e0e07eb-abc5-4d19-b327-0a050ff81e55.png)


# Challenges

- It was my first time using SQL and by extension SQLite
  - I had to learn the basic syntax
  - Access the SQL database and write the statements that retrieve the exact information I wanted
  - Overall it was very enjoyable and it got me curious to do more things with it.
- User testing
  - I had to test for multiple inputs to make sure the app didn't crash, which took me quite a while and I'm still not 100% sure it won't crash.

# Lessons learned

- I got more used to implementing classes from the start, which helped me manage the growing complexity of the code.
- As I keep using Visual Studio, I learn more "tricks" that help me speed up the development proccess.
- Method overloading is a great resource for simplicity


# Areas to improve

- Although in this project, I've only used git to essentiatly save my work, I still want to learn more on how to use it.
- I need to improve my efficiency debugging. Altough I know how to use some of Visual Studio debugging features, I'm not proficient with them so I end up reverting to testing most things by running the program from the start.
  - As part of this, I also need to improve my user testing.


# Resources Used

- The [C#Academy project](https://www.thecsharpacademy.com/project/12) was the project guide.
- The [C#Academy Tutorial](https://www.youtube.com/watch?v=d1JIJdDVFjs) for this project, which helped me kickstart everything SQL and SQLite related.
- The [C#Academy discord community](https://discord.com/invite/JVnwYdM79C) that are always ready to help!
- Various resources from all over the web. Special shoutout to [geeksforgeeks](geeksforgeeks.org)!
