Hi, In this project I have used ADO.NET to interact With SqlServer to create a 'habits' database and HabitDetails Table. 
when the project starts, it will check if the database existing or not in the server specified by the user with connectionstring. if it does not exist then the database and table will be created for the user.
<h1>Challeneges</h1>
The main challenge i faced is to learn about the datasources and objects the ADO.Net offers to interact with sql server. 
checking if table and DB exists.. and creating the table and database if it doesn't exists is another challenge. i learnt a lot about SQL queries to figure out that.
handling all sorts of user inputs such that the application never crashes is another challenge. i used error handling and tryParse combined with while loops in some parts to allow user to enter the expected input until they get it right.
the exception handling using try{} and catch{} blocks allowed me gracefully handle the exceptions and errors without crashing the application.
