
# Habit Tracker

A Console application developed with Visual Studio using C# and SQLite.

This CRUD application is used to track any quantifiable task or habit the user want's to keep a record of. Records are stored in a SQLite database on the user's local machine.


## Features

- SQLite Database connection using ADO.NET
    - If no database exists, the app will create one containing a table to store habit information.

- UI for quickly viewing all saved habits and navigating the app's other functions

- CRUD Database functions
    - Users are able to create new habit entries and read, edit, or delete existing ones.
    - All inputs are validated to ensure user choices are formatted correctly and/or acceptable answers.



## Tech Stack

**Runtime & Framework:** .NET 9

**Data Access Layer:** ADO.NET

**Database:** SQLite


## Lessons Learned

- I used the "Lock" keyword for the first time in this project. Mostly this was to get a surface level understanding of what it did and the purpose it served. This was utilized in the two instances in this project I implemented the Singleton Pattern as another possible way to prevent duplicate instances from being created. Looking forward to future projects where I'll need to delve more into multi-threaded processes, Jon Skeet's (see Acknowledgements below!) approach for implementing Singletons will provide a good jump start if I need to reuse this pattern in that type of scenario going forward. Here though, it serves as another possible way to implement the pattern that I hadn't encountered before.

- I have used SQL and specifically SQLite in the past, but typically with other languages like Python or Java. Working with ADO.NET and Parameterized Queries was brand new to me though. Clearly a game changer. A common flaw amongst older projects of mine using SQL in some form is that they are vulnerable in some way to SQL injection. For learning that really isn't too big of an issue, but applying that to a project that will actually be deployed is very different. Though there are likely other ways I still am yet to learn on how to properly protect my code on all fronts, this one was fairly straight forward to use and significantly bolstered the security of reciving a user's input.
## Acknowledgements

 - [The C# Academy](https://www.thecsharpacademy.com/) for the guided pathway to learning the .NET Framework.
 - ["C# in Depth", Jon Skeet: Implementing the Singleton Pattern](https://csharpindepth.com/articles/singleton)
 - [README Editor](https://readme.so/editor) for making this README super easy to format on Github.
 - [How to write a Good readme](https://bulldogjob.com/news/449-how-to-write-a-good-readme-for-your-github-project) for just being all around good advice.

