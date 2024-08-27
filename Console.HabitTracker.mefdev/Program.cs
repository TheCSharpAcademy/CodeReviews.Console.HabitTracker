using System.Globalization;
using HabitLogger.Models;
using HabitLogger.Services;
using HabitLogger.Shared.Logger;
using ConsoleTableExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Text.RegularExpressions;

class Application
{
    private static HabitService _habitService;
    private static Message _successMessage;
    private static ErrorsLogger _errorLogger;
    private static SpeechRecognizer recognizer;
    private static bool quit;
    private static bool useSpeechRecognition;
    const string DBNAME = "habit.db";

    private async static Task Main()
    {
        
        try
        {
            InitializeNeccessaryClasses();
            AddRandomHabits();
            if (UseAI()) useSpeechRecognition = true;
            while (!quit)
            {
                await DisplayHabitLoggerMenu();
            }
        }
        catch (Exception ex)
        {
            _errorLogger.DisplayError(ex.Message);
        }
    }
    private static bool UseAI()
    {
        Console.WriteLine("-------------------------- USE ARTIFICAL INTELIGENECE ---------------------------");
        Console.Write("Would you like to use AI to interact with the app Y/N: ");
        return Console.ReadLine()?.ToLower() == "y";
    }
    private static async Task<string> GetRecognizedSpeech()
    {
        var result = await recognizer.RecognizeOnceAsync();
        if (result.Reason == ResultReason.RecognizedSpeech)
        {
            return result.Text;
        }
        else return null;
    }
    private static async Task DisplayHabitLoggerMenu()
    {
        DisplayMenu();
        try
        {
            await DisplayNextOptions();
        }
        catch (Exception ex)
        {
            _errorLogger.DisplayError(ex.Message);
        }
    }
    private static string GetDBNameWithoutExtension(string dbName)
    {
        return dbName.Replace(".db", "");
    }
    private static void InitializeNeccessaryClasses()
    {
        string s_path = InitializeDB();
        InitializeHelperServices(s_path);
    }
    private static void InitializeSpeechRecognition()
    {
        string? REGION = Environment.GetEnvironmentVariable("REGION");
        string? SUBKEY = Environment.GetEnvironmentVariable("SUBKEY");

        var config = SpeechConfig.FromSubscription(SUBKEY, REGION);
        var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        recognizer = new SpeechRecognizer(config, audioConfig);
    }
    private static string InitializeDB()
    {
        string tableName = GetDBNameWithoutExtension(DBNAME);
        string s_path = GetFilePath(DBNAME);
        new DBStorage(s_path, tableName);
        return s_path;
    }
    private static void InitializeHelperServices(string path)
    {
        _habitService = new HabitService(path);
        _successMessage = new Message();
        _errorLogger = new ErrorsLogger();
        InitializeSpeechRecognition();
    }
    private static void DisplayMenu()
    {
        Console.WriteLine("-------------------------- Habit logger Menu ---------------------------");
        Console.WriteLine("Choose an option from the following list:");
        Console.WriteLine("-------------------------- Available options ---------------------------");
        Console.WriteLine("\ta - Add a habit");
        Console.WriteLine("\tv - View a habit");
        Console.WriteLine("\td - Delete a habit");
        Console.WriteLine("\tu - Update a habit");
        Console.WriteLine("\ts - View all habits");
        Console.WriteLine("\tq - exit");
        if(useSpeechRecognition)
        {
            Console.Write("Say your option? ");
        }
        else
        {
            Console.Write("Your option? ");
        }
    }
    private static string DisplayAddHabitOptions(string name)
    {
        string? option = null;
        switch (name)
        {
            case "id":
                Console.Write($"Enter habit {name}: ");
                break;
            case "name":
                Console.Write($"Enter habit {name}: ");
                break;
            case "quanity":
                Console.Write($"Enter habit {name}: ");
                break;
            case "date":
                Console.Write($"Enter start date (yyyy-MM-dd) {name}: ");
                break;
            default:
                break;
        }

        while (string.IsNullOrEmpty(option))
        {
            option = Console.ReadLine();
        }
        return option;

    }
    private static void DisplayHabitRetrieved(Habit Retrieved)
    {
        Console.WriteLine("--------------------------- Habit retreived ----------------------------");
        var tableData = new List<List<object>>
        {
                new List<object>{ "ID", "NAME", "QUANTITY", "DATE"},
                new List<object>{ Retrieved.Id, Retrieved.Name, Retrieved.Quantity, Retrieved.Date },
                
        };
        ConsoleTableBuilder
        .From(tableData)
        .ExportAndWriteLine();
    }
    private static void DisplayHabits(List<Habit> habits)
    {
        Console.WriteLine("--------------------------- Habits ----------------------------");
         
        List<List<object>> habitObjects = new List<List<object>>();
        foreach (var habit in habits)
        {
            habitObjects.Add(new List<object> { habit.Id, habit.Name, habit.Quantity, habit.Date });
        }
        
        ConsoleTableBuilder
        .From(habitObjects)
        .WithColumn("ID", "NAME", "QUANTITY", "DATE")
        .ExportAndWriteLine();
    }

    private static DateTime ConvertStringToDateTime(string date)
    {
        try
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        }
        catch
        {
            throw new FormatException("Invalid format: Unable to convert the requested date");
        }

    }
    private static int GenerateRandomID()
    {
        return new Guid().GetHashCode();

    }
    private static Habit AskUserEnterValues()
    {
         
        Console.WriteLine("-------------------------- Enter habit values --------------------------");
        string name = DisplayAddHabitOptions("name");
        string quantity = DisplayAddHabitOptions("quanity");
        string userDate = DisplayAddHabitOptions("date");
        DateTime date = ConvertStringToDateTime(userDate);
        return new Habit(GenerateRandomID(), name, quantity, date);
    }
    private static void AddHabit()
    {
        var newHabit = AskUserEnterValues();
        _habitService.AddHabit(newHabit);
        _successMessage.Display($"[{newHabit.Name}] has been inserted succefullly.");
    }
    private static string GetUpdateJsonData(string jsonData)
    {
        var habitsArray = JArray.Parse(jsonData);
        foreach (var habit in habitsArray)
        {
            var habitDate = habit["Date"]?.ToString();
            habit["Date"] = ConvertStringToDateTime(habitDate);
        }
        return habitsArray.ToString();

    }
    private static void AddRandomHabits()
    {
        var jsonFilePath = GetFilePath("habit.json");
        var jsonData = File.ReadAllText(jsonFilePath);
        jsonData = GetUpdateJsonData(jsonData);
        List<Habit>? habits = JsonConvert.DeserializeObject<List<Habit>>(jsonData);
        if (habits is null)
        {
            throw new Exception("An error happend while deserializing habit object");
        }
        foreach (var habit in habits)
        {
            habit.Id = GenerateRandomID();
            _habitService.AddHabit(habit);
        }
        _successMessage.Display($"[habits] has been inserted succefullly.");
    }

    private static Habit RetrieveHabit()
    {
        string userID = DisplayAddHabitOptions("id");
        if (!int.TryParse(userID, out int id))
        {
            Console.WriteLine("Invalid id");
        }
        var retrievedHabitToUpdate = _habitService.GetHabit(id);
        if (retrievedHabitToUpdate == null)
        {
            throw new Exception("Habit not found!");
        } 
        else return retrievedHabitToUpdate;
       
    }
    private static void DeleteHabit()
    {
        var RetrievedHabitToDelete = RetrieveHabit();
        _habitService.DeleteHabit(RetrievedHabitToDelete.Id);
        _successMessage.Display($"[{RetrievedHabitToDelete.Name}] has been deleted succefullly.");

    }
    private static void UpdateHabit()
    {
        var RetrievedHabitToUpdate = RetrieveHabit();
        var updatedHabit = AskUserEnterValues();
        updatedHabit.Id = RetrievedHabitToUpdate.Id;
        _habitService.UpdateHabit(updatedHabit);
        _successMessage.Display($"[{RetrievedHabitToUpdate.Name}] has been updated succefullly.");
         
    }
    private static void ViewHabit()
    {
        try
        {
            var RetrievedHabit = RetrieveHabit();
            if(RetrievedHabit == null)
                _errorLogger.DisplayError("Habit not found!");
            else DisplayHabitRetrieved(RetrievedHabit);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private static void ViewAllHabits()
    {
        List<Habit> habits = _habitService.GetAllHabits();
        DisplayHabits(habits);
    }

    private static async Task DisplayNextOptions()
    {
        if (useSpeechRecognition)
        {
            await DisplayNextAIOptions();
        }
        else
        {
            string? option = Console.ReadLine();
            switch (option)
            {
                case "a":
                    AddHabit();
                    break;
                case "v":
                    ViewHabit();
                    break;
                case "d":
                    DeleteHabit();
                    break;
                case "u":
                    UpdateHabit();
                    break;
                case "s":
                    ViewAllHabits();
                    break;
                default:
                    quit = true;
                    Environment.Exit(0);
                    break;
            }
        }
        
    }
    private static string FilterUserOption(string option)
    {
        option = option.ToLower().Replace(".", "");
        return Regex.Replace(option, @"[^a-z0-9\s]", "");
    }
    private static async Task DisplayNextAIOptions()
    {
        var option = await GetRecognizedSpeech();
        if(option == null)
        {
            _errorLogger.DisplayError("Am I deaf? I heard nothing");
            _errorLogger.DisplayError("bye bye..................");
            Environment.Exit(0);
        }
        option = FilterUserOption(option);
        Console.Write($"{option}\n");
        switch (option)
        {
            case "add a habit":
                AddHabit();
                break;
            case "view a habit":
                ViewHabit();
                break;
            case "delete a habit":
                DeleteHabit();
                break;
            case "update a habit":
                UpdateHabit();
                break;
            case "view all habits":
                ViewAllHabits();
                break;
            default:
                quit = true;
                Environment.Exit(0);
                break;
        }
    }

    private static string GetCurrentPath()
    {
        return Environment.CurrentDirectory.Replace("bin/Debug/net7.0", "");
    }

    private static string GetFilePath(string tableName)
    {
        return Path.Combine(GetCurrentPath(), tableName);
    }
}