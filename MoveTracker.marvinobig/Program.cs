using MoveTracker;
using MoveTracker.Data;

string dbPath = Path.Combine(Environment.CurrentDirectory, "MoveDB.sql");
int seperator = 51;
MoveRepository repository = new(dbPath);

App.DisplayIntro(seperator);
App.DisplayMenu(repository, seperator);
