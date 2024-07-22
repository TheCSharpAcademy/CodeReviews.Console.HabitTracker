using DB;
using HabitLogger;
string connString = "Data Source=Habits.db";

DbContext dbContext = new DbContext(connString);
App app = new(dbContext);

app.Run();