# ConsoleRunningTracker
One of my first C# console applications.

This is a rudimentary CRUD console application which can be used to keep track of runs, developed using C# and SQLite.

## Requirements and challenges
### Requirements
- [x] Once started if an SQLite database isn't already present, one should be created 
- [x] A table should be created created in the database so that the habit can be logged
- [x] The user should have a menu of options, where the user should be able to Create, Read, Update and Delete records(CRUD)
- [x] Testing/Error handling should be completed so that the application does not crash
- [x] The application should only exit/close when the user inserts 0
- [x] Only raw SQL can be used to interact with the database
### Challenge requirements
- [x] Allow the user(s) to create their own habits to track, user will need to choose their unit of measurement for each habit
- [x] Create a report functionality to let users view specific data

## Features
- Establish an SQLite database connection to create a database if one does not exist, store and read information in said database
- There will be a console UI menu where users will be given the below options
    - Enter 0 to Close this application
    - Enter 1 to View all records
    - Enter 2 to Insert a record
    - Enter 3 to Delete a record
    - Enter 4 to Update a record
    - Enter 5 to Change logged habit
    - Enter 6 to View reports
        - Enter T for the count of x being done (use COUNT function) e.g. how many days a run was completed
        - Enter S for the sum total of measurement tracked e.g. how many miles/kms ran
        - Enter A for the average number of miles ran of all time
        - Enter M to return to the main menu

## Challenges faced
- Establishing a connection to an SQLite database and how to interact with it as I have never used SQLite before, let alone as part of an application in another language, I overcame this by watching [TheC#Academy's HabitTracker tutorial](https://www.youtube.com/watch?v=d1JIJdDVFjs)
- Learning to think ahead with code, what sort of methods/functions might only be used once and which ones might need to be used more than once i.e. getting input to update a record but also getting input to insert a record or using a function to display data from the table, I used my knowledge of object oriented programming to help with this and a pad and pen to help put my ideas down somewhere before they ran away
- I tend to overcomplicate a lot of issues and tunnel vision on things, which leads to burning myself out however just getting more of an understanding of how certain things work and using a pen and pad I am able to simplify things/break them down into much more manageable issues.

## Improvements
- I think I made a good effort to ensure I utilised single responsibility where possible and that my code was modular however I think I could have done a better job implementing more OOP rules/conventions and will strive to do this in the future, this will require some more studying and practical experience on my part which I will hopefully do as I move on through the C# academy!
- At times I struggled with consistently coming back to this project, for future projects I will try to set myself smaller tasks within notion/trello
- I could also do with learning more code snippets/shortcuts just to improve how efficient I work
- For my next project I will try to include more file structure and remember to close down class files when not in use as this made it feel very messy/unorganised to work on the project
