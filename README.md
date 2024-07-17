# Habit Tracker

Console-based CRUD application to track habits.
Developed using C#.
Library used: [Terminal.Gui](https://gui-cs.github.io/Terminal.Gui/index.html) and [SQlite](https://www.nuget.org/packages/System.Data.SQLite/)


# Given Requirements:
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
- [x] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [x] Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values.
- [x] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

# Features

* A console based application with basic GUI-based control (point and click, keyboard arrow and enter to navigate)

	- By using the powerful Terminal.Gui libarary, I was able to create a console application with graphical user interface
	- ![HabitTrackerLoop](https://github.com/user-attachments/assets/e5fd430d-4d82-45e2-90d6-94929997decf)

* View all your habit in tabular form

    - Navigate your habit with scroll wheel and arrow keys
 	- ![HabitTrackerView](https://github.com/user-attachments/assets/0f51d34e-d8b1-469e-a74c-6417328ff9b3)

* Delete habit easily

	- Allows for point and double click the habit you want to delete
	- Allows for arrow keys to navigate and enter to delete
    - ![HabitTrackerDelete](https://github.com/user-attachments/assets/356f3e58-186f-4914-9afd-df930ad2e7a1)

* Edit habit easily

	- Allows for point and double click a specific cell in the table to edit the habit data
    - ![HabitTrackerEdit](https://github.com/user-attachments/assets/ec222343-117e-4db4-80cd-6fd35106c951)

* Create habit

	- Create habit with the goals and measurements you want (optional)
	- ![HabitTrackerCreate](https://github.com/user-attachments/assets/05882023-420c-4096-804e-e005481e2d14)

* Filter/Search habit

	- Filter data by various field
	- ![HabitTrackerEditSpecific](https://github.com/user-attachments/assets/ab020e49-fcb5-4c64-ac6f-62249024ba57)
	- ![HabitTrackerEditSpecific2](https://github.com/user-attachments/assets/c6bf2399-f134-48a2-82f6-ac1401836349)

# Challenges
	
- Learning to navigate the Terminal-Gui library was a challenge as it requires to read the API documentation in detail
- Structuring the code in an object-oriented way took a lot of time to understand
- There were a lot of SQL related errors encounter when creating this application, mostly because validation of data wasn't done beforehand
- Deciding how to display the data is a difficult choice. At first, I wanted to display them in a list. Later I found out table form is supported and ultimately made more sense.
	
# Lessons Learned
- I learned how to navigate a big code base and properly read API documentation. Granted Terminal.Gui is a well-supported open source project, so it was very helpful and rewarding.
- I learned how to dynamically create instances on run-time and set up custom attribute classes. This helps reduce development time if future changes and additional features are needed.
- I learned basic SQL syntax, CRUD operation, and some queries for looking up specific data.
- Be consistent. Navigating a library was out of my comfort zone for me and I am glad I kept on with it.

# Areas to Improve
- Object-oriented concept: Some OOP concepts like Inheritance do not come naturally to me. A lot of time I wasn't sure why do I need to inherit the class.
- Speed: This application took longer to complete than expected. There are VSCode optimizations I can learn (like keyboard shortcuts I don't know yet). I also wish to put more time per day into future projects.
- Overall, this application can do a lot more and is made to make it easy to create new features. But currently, I wish to move on and learn different things.
