<h1>Habit Tracker Console Application</h1>
<h3>Authored by Corey Jordan</h3>
<h3>May 12th, 2023</h3>
<hr>
<p>
  This a C# Console Application designed to store and track habits on a very basic level.<br>
  This application makes use of Sqlite and CRUD operations.
</p>
<h1>Requirements</h1>
<ul>
  <li>Application must store and retrieve data from a database</li>
  <li>Application must create a db if one does not exist</li>
  <li>The app must create a table in the db where the habit will be stored</li>
  <li>The app must be able to perform CRUD operations</li>
  <li>The application must exit only upon user request</li>
  <li>Cannot use data mappers, raw SQL only</li>
  <li>Project must contain a ReadMe file</li>
</ul>
<h1>Challenges</h1>
<ul>
  <li>User can create custom habits and units of measurement</li>
  <li>App to have report functionality</li>
</ul>
<h1>Thoughts</h1>
<ol>
  <li>My knowledge of SQL is limited. Basic crud ops are simple. I ran into trouble creating a table to store units of measurement. In the end, I used the habit's tabe name as a key for the unit table but this feels incorrect. I also was unsure how to combine the queries into some sort of joiner and ended up performing 2 queries for the report function.</li>
  <li>A lot of the program code is UI specific but feels a bit long and jumbled. In the future I might look for a better way to further seperate the program responsibilities. I do feel as though I seperated data logic and UI logic fairly well.</li>
  <li>I need to improve writing my commits. I do well enough to keep the commit message concise but I seldom explain in more detail afterwards.</li>
