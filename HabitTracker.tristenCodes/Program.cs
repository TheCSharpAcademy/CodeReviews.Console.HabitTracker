using Classes;

// Establish DB Connection
DBService dBService = new DBService("Data source=local.db");
string userInput = Console.ReadLine() ?? "";
testDelimiter(userInput);

// habit name, occurrences, date
void testDelimiter(string userEntry)
{
    var splitEntry = userEntry.Split(";");
    var habitName = splitEntry[0];
    var occurrences = splitEntry[1];
    var date = splitEntry[2];
    Console.WriteLine($"habitName: {habitName}, occurrences: {occurrences}, date: {date}");
}

