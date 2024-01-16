# Console Habit Tracker
#### Authored by Crockett Ford

A simple console-based CRUD application for habit tracking. 
Developed using C# and SQLite.

--------------------------------------------------------------------------------

# Features

- Perform CRUD applications on an SQLite database to track habits in terms of performance per day.
- Add custom habits and units of measurement
- A console UI menu 
- View past data


# Challenges

- Design. It was difficult to juggle all the correct access modifiers. The initial tutorial comprised one file which contained all the program's methods as static methods. This quickly grew out of control, and once I started trying to untangle it, I struggled for some time. In the future I will prioritize planning the design of the program (however simple) before writing much code at all.
- SQL. Being very new to it, I struggled to implement helpful features like foreign keys. Writing effecting queries is still not easy for me. I learned quite a bit through the process, but I was unable to implement a number of the features I wanted.


# Possible Additions
- Should I come back to this project, I should consider adding
	- Custom reports defined by year, month, total units in a year, etc.
	- More robust relations between the unit and habit tables in the database
	- A more appealing UI
	- More readable reports, using a library like [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt)

------------------------------------------------------------------------------------

# Final Thoughts
- I decided to let this project rest after meeting the core requirements and two of the challenges. Originally, I wanted to complete it to a high standard of polish and add several features, including features not included in the project [specifications](https://thecsharpacademy.com/project/12). After some reflection, I've decided to leave it in this state and possibly return to it in the future.
