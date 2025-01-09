See https://www.thecsharpacademy.com/project/12/habit-logger for more details.

## Requirements

### Rules

* This is an application where you’ll log occurrences of a habit.
* This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
* Users need to be able to input the date of the occurrence of the habit
* The application should store and retrieve data from a real database
* When the application starts, it should create a sqlite database, if one isn’t present.
* It should also create a table in the database, where the habit will be logged.
* The users should be able to insert, delete, update and view their logged habit.
* You should handle all possible errors so that the application never crashes.
* You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.

### Challenges

* If you haven't, try using parameterized queries to make your application more secure.
* Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
* Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
* Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

### AI Challenge

* Can you let the users add records using their voice?

## Running the app

```bash
dotnet restore
dotnet build
dotnet run --project HabitLoggerApp
```

## Running tests

```bash
dotnet test
```

## Configuring Azure SDK for Speech Recognizer

Add your subscription details into user secrets like this:

```json
{
  "SpeechRecognizer": {
    "Region": "",
    "SubscriptionKey": ""
  }
}
```
