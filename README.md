#My Third C# application.

Console based CRUD application to track number of glasses of water drank per day. Developed using C# and SQLite.
##Requirments Fulfilled

1. *Quantity-Based Habit Tracking*: Logs habits by quantity (e.g., glasses of water), not time.
2. *Date Input*: Users can input the date for each habit occurrence.
3. *SQLite Database*: Uses SQLite for storing and retrieving habit data.
4. *Database Setup*: Automatically creates a SQLite database and a table for logging habits when the app starts.
5. *CRUD Operations*: Users can insert, delete, update, and view logged habits.
6. *Error Handling*: All errors are handled to prevent app crashes.
7. *ADO.NET*: Database interaction is handled using ADO.NET without mappers (e.g., Entity Framework, Dapper).
##Learnings:


1. CRUD operations form the core of most applications. Building this app helped me understand that most web applications boil down to Create, Read, Update, and Delete operations with a database.

2. SQL commands turned out to be simpler than expected. Basic SQL statements like `INSERT`, `SELECT`, `UPDATE`, and `DELETE` were enough to build the app.

3. ADO.NET helped me understand raw SQL interaction. Without the abstraction of an ORM, I learned to manually write SQL queries, manage database connections, and handle data directly.

4. Setting up SQLite to create a database and table at startup ensured the app was always ready, even if no prior database existed.

5. Error handling was crucial for making the app reliable and crash-free. By handling exceptions, I ensured the app could manage unexpected database errors gracefully.

6. Starting with ADO.NET gave me foundational knowledge. While other tools like Dapper or Entity Framework simplify database operations, working closely with ADO.NET provided a deeper understanding of database interactions.

7. As applications grow, their complexity increases, but CRUD remains the essential building block.

8. ##Resources Used
9.  1)https://youtu.be/d1JIJdDVFjs
10. 2)https://www.thecsharpacademy.com/project/12/habit-logger
