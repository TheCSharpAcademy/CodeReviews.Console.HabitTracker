# ConsoleTimeLogger
	An application that let users keep track of their habits.
# Given Requirements:
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit infromation will be stored.
- [x] You need to be able to insert, delete, update and view your logged hours. 
- [x] You should handle all possible errors so that the application never crashes 
- [x] The application should only be terminated when the user inserts q. 
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework

- [x] using parameterized queries to prevent SQL Injection.
- [x] Seed Data into the database automatically when the database gets created for the first time, generating
	a few habits and inserting a 20 records with randomly generated values.
- [x] Let users choose an option using their voice leveraging Azure Language Services.

# Features

* SQLite database connection

	- The program uses a SQLite db connection to store and read information. 
	- If no database or table exists, they will be created once the program starts.

* A console based UI where users can navigate by entering or saying an option from the Menu
	-------------------------- Available options ---------------------------
			a - Add a habit
			v - View a habit
			d - Delete a habit
			u - Update a habit
			q - exit
	Your option? q
 	

* CRUD Operations

	- From the main menu users can create, read, update or delete entries. 
	- Dates inputted are checked to make sure they are in the correct and realistic format. 


* View habit data output uses ConsoleTableExt library to output in a more pleasant way

	----------------------------------------------------------
	| 2  | cycling | 2 times per week | 9/20/2024 12:00:00AM |
	----------------------------------------------------------

	- [GitHub for ConsoleTableExt Library](https://github.com/minhhungit/ConsoleTableExt)

# Challenges
	
 - I have faced an issue with sqlite3 engine not working properly on my unix machine so, I had to find out a way to make it work, since all my code wasn't working as it should.
	After two days of searching through chatgpt and claude answers, I found out that I had to change the namespace from "system.data.sqlite" to "Microsoft.data.sqlite" because
	sqlite core implementation of Ado.net was a bit old so I had to use the one that microsoft provided. The next challenge was to make the code robust and maintainable, so I was obliged to adhere to the KISS and SOLID principles. Which makes the developing time rise a bit since I had to think of a good way
	to organize and refactor the code.Lastly, the last challenge was to implement speech recognition through azure, so I had to make many configuration
	before writing the first line of code. It was really challenging because Visual studio on mac hasn't the required permission to use the mic. So I had to use
	ITerm2 wich works perfectly.
	
# Lessons Learned

- Do it the right way the first time. There were several instances where I just wanted to see if something would work so I did it in a quick
	way in which I knew it would need to be fixed or cleaned up later. 9 times out of 10 later never came. 
- Have a testing grounds readily set up. This way small things can be tested seperately from the within the app. This can speed up creation.
 
# Areas to Improve
- Learn more about the KISS principle and put it to use in real world application.

# Resources Used
 - [MS docs for setting up SQLite with C#](https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
 - [MS reference for speech recognition](https://learn.microsoft.com/en-us/dotnet/api/system.speech.recognition?view=net-8.0)
  