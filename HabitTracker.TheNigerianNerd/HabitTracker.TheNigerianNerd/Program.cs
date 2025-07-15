using HabitTracker.TheNigerianNerd;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
       .AddJsonFile("appsettings.json")
       .Build();

string connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];

HabitFunctions habitFunctions = new(connectionString);
Data data = new(connectionString, habitFunctions);

data.CreateDatabase();
habitFunctions.MainMenu();

record Habit(int Id, string Name, string UnitOfMeasurement);
record RecordWithHabit(int Id, DateTime Date, int Quantity, string HabitName, string MeasurementUnit);