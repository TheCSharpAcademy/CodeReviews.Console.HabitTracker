<h1>Habit Tracker App</h1>
<p>A basic console application for creating and tracking occurrences of habits.
</p>

<h2>Project Design Requirements</h2>
<p>Below are the design requirements provided for the project. Itemse with the strikethrough font were not implemented in the current version.
</p>

<ul>
<li>Habits can't be tracked by time, only by quantity</li>
<li>Users need to be able to input the date of the occurrence of the habit</li>
<li>The application should store and retrieve data from a real database</li>
<li>Creates a sqlite database with relevant tables on startup, if one isn’t present.</li>
<li>The users should be able to insert, delete, update and view their logged habit.</li>
<li>Application should never crash.</li>
<li>You can only interact with the database using ADO.NET.</li>
<li>Follow the DRY Principle, and avoid code repetition.</li>
<li>Project must contain a Read Me file</li>
<li><s>(Optional) Use parameteized queries</s></li>
<li>(Optional) Allow user to create a new habit</li>
<li>(Optional) Generate seed data for database</li>
<li>(Optional) Create reports</li>
</ul>

<h2>Features</h2>
<ul>
<li>Self-generating SQLite DB</li>
<li>Console UI with user input via keyboard</li>
<li>Basic CRUD</li>
<li>Individuals view each habit</li>
<li>Select report date range or enter custom</li>
</ul>

<h2>Components</h2>
<ul>
  <li>habitTrack.jzhartman - Main console application project</li>
  <li>HabitTrackerLibrary - Library with data access and models</li>
</ul>

<h2>Operation</h2>
<p>Running the application opens a console window displaying the following data:
</p>
<ul>
  <li>Application name/version number</li>
  <li>List of all habits in current databasee</li>
  <li>Main menu with detailed selection options</li>
</ul>

From here, the user can enter the menu item number to select their desired function.

<h3>Select Habit</h3>
This menu item allows the user to select one of the habits displayed in the list. Once selected, the complete list of records will be printed to the console. They are then given the option to add, change, or delete any given record. Additionaly, a report can be generated for that habit that provides detailed information based on a selected date range.

<h3>Add New Habit</h3>
Allows the user to enter a new habit to track. Habit name must be unique and cannot be blank. Once the habit name is entered, the user must determine the unit of measure. They can select from a printed list of existing units, or enter a new unit.

Once the habit name and unit are selected, the user can confirm the addition of the new habit. An affirmative answer will add that habit to the database.

<h3>Delete Habit</h3>
This allows the user to delete a habit from the Habit Tracker. Doing so will also delete all records associated with that record. This is non-reversible and requires a user confirmation to complete.

<h2>Future Additions</h2>
<p>Below is a list of future additions that could be used to improve the current application.</p>

<ul>
  <li>Implement optional parameterized queries feature for sqlite</li>
  <li>Create escape sequence for user during data input</li>
  <li></li>
  
</ul>

