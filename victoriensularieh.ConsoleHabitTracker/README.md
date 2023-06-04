# create and setup project (in visual studio Code)
1. folder structure
    1. create folder
    2. open folder in VS Code (right-click -- open in visual studio code)
2. commands
    1. open new terminal
    2. dotnet new console --name ConsoleHabitTracker
    3. dotnet new gitignore (this is important to do at the beginning so that you wont push unnessecary files and folders to your remote repo)
3. get nuget packages
4. create repository on github

# what i learned from this project
1. how to use VS code in .net development
2. how to make use of nuget packages
3. how to organize code in seperated classes, and practice seperation of concerns
4. what files are necessary to keep in the remote repo. creating the gitignore file before creating the repo was essential to make sure nothing build related gets pushed. it was also great to realize the dependancies are only referred to in the csproj file.

# how to use the application
1. the application allow you to define Units and habits and link them together.
2. you add an entry for each habit tracked.
3. you can display, add, update and delete habits, units and entries
4. no need to worry about wrong inputs, validation has been implemented.
5. there is a level of data security, you cannot delete a unit that is used as for a habit, and you cannot delete a habit that has entries.

# todo later
1. cancel functionality in the middle of an operation