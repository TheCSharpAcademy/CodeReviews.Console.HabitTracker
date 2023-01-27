// See https://aka.ms/new-console-template for more information
DatabaseClass db = new DatabaseClass("Data Source=Habit-tracker.db;");
DisplayClass displayClass = new DisplayClass();
string databaseName = "Habit-tracker.db";
db.CreateDatabase(databaseName);
db.CreateTable();
bool endApp = false;

while(!endApp)
{
    Console.WriteLine("Habit-Tracker Application");
   displayClass.UserLogin();
 
}
