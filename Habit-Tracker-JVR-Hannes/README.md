<h1>Usage</h1>
  <p>When you run the application, you'll be greeted with a menu that allows you to perform various actions. Here's an overview of the available commands:</p>

<h2>Main Menu Options:</h2>
<div>
  <ul>
    <li>0 - Exit: Exit the application.</li>
    <li>1 - View All Records: Lists all habit records.</li>
    <li>2 - Insert Record: Adds a new habit record by asking for the habit type, date, and quantity.</li>
    <li>3 - Delete Record: Deletes a specific habit record by entering its ID.</li>
    <li>4 - Update Record: Updates an existing record by entering the record ID.</li>
    <li>5 - Clear All Records: Deletes all habit records from the database.</li>
    <li>6 - View Records by Date Range: Shows habits between two dates.</li>
    <li>7 - View Records for a Specific Habit: Displays all records for a specific habit type.</li>
  </ul>
</div>
<h2>Adding a Habit Record:</h2>

<p>Select option 2 to insert a record.</p>
<p>You’ll be prompted to enter the habit type (e.g., "exercise").</p>
<p>Then, you’ll enter the date in the format dd-MM-yyyy.</p>
<p>Finally, you’ll provide the quantity for the habit (e.g., number of hours).</p>

<h2>Viewing Habit Records:</h2>
<div>
  Choose option 1 to see all habit records.<br>
  Option 6 allows you to filter habits by a specific date range.<br>
  Option 7 will let you view records for a specific habit type.<br>
</div>
<h2>Updating a Record:</h2>
<div>
To update a habit, choose option 4, provide the record ID, and then enter the new date and quantity.
</div>
<h2>Deleting Records:</h2>
<div>
Choose option 3 to delete a specific record by its ID.<br>
Select option 5 to clear all records from the database.<br>
</div>
<h2>Database</h2>
<p>This application uses SQLite to store habit records. The database is created automatically in the same directory as the executable with the filename habit-Tracker.db.</p>
