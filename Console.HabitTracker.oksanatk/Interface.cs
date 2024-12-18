using Microsoft.CognitiveServices.Speech;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace HabitLogger;

internal class Interface
{
    internal static string? readResult;
    internal static string? speechKey = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Key");
    internal static string? speechRegion = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Region");

    internal static void ShowMainMenu(bool voiceMode)
    {
        bool endApp = false;
        string userMenuOption = "";

        while (!endApp)
        {
            List<String> currentHabits = HabitInteractions.ReadHabitsFromFile();
            currentHabits.Remove("sqlite_sequence");

            Console.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Welcome to your personal Habit Logger.[/]\n");
            AnsiConsole.MarkupLine("Please select what you'd like to do:\n");

            Panel mainMenuPanel = ShowMenuOptionsPanel(voiceMode);
            AnsiConsole.Write(mainMenuPanel);

            AnsiConsole.MarkupLine("\nOR enter [bold yellow]exit[/] to exit the application.");

            if (voiceMode)
            {
                userMenuOption = GetVoiceInput().Result;
            }
            else
            {
                readResult = Console.ReadLine();
                if (readResult != null)
                {
                    userMenuOption = readResult.Trim().ToLower();
                }
            }

            switch (userMenuOption)
            {
                case "create":
                case "1":
                    HabitInteractions.CreateNewHabit(currentHabits, voiceMode);
                    break;
                case "edit":
                case "2":
                    HabitInteractions.EditExistingHabit(currentHabits, voiceMode);
                    break;
                case "delete":
                case "3":
                    HabitInteractions.DeleteExistingHabit(currentHabits, voiceMode);
                    break;
                case "report":
                case "4":
                    HabitInteractions.ViewHabitReport(currentHabits, voiceMode);
                    break;
                case "exit":
                    endApp = true;
                    break;

                default:
                    if (voiceMode)
                    {
                        AnsiConsole.MarkupLine("I'm sorry, but I didn't understand that menu option. Please try again in a few seconds.");
                        Thread.Sleep(2500);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("I'm sorry, but I didn't understand that menu option. Press [bold yellow]Enter[/] to try again.");
                        Console.ReadLine();
                    }
                    break;
            }
        }
    }

    internal static string GetUserDateInput(bool voiceMode)
    {
        List<string> datePieces = new();
        string? readResult;
        bool useTodayDate = false;
        int preformattedDate = -1;
        string date = "";
        string userIntAsString = "";

        AnsiConsole.MarkupLine("We need the date for the new record of your habit. Would you like to use [bold yellow]today[/] as the date? (y/n)");
        if (voiceMode)
        {
            readResult = GetVoiceInput().Result;
            readResult = readResult.Trim().ToLower();
            if (readResult.StartsWith("y") || readResult.Contains("today"))
            {
                useTodayDate = true;
            }
        }
        else
        {
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                readResult = readResult.Trim().ToLower();
                if (readResult.StartsWith("y") || readResult.Contains("today"))
                {
                    useTodayDate = true;
                }
            }
        }

        if (!useTodayDate)
        {
            AnsiConsole.MarkupLine("\nPlease enter the [bold yellow]year[/] of the date (yyyy).");
            datePieces.Add(GetUserIntInput(voiceMode, gettingYear: true).ToString());

            AnsiConsole.MarkupLine("\nPlease enter the [bold yellow]month[/] of the date of your new habit record (MM)");
            preformattedDate = GetUserIntInput(voiceMode, gettingMonth: true);
            if (preformattedDate < 10)
            {
                userIntAsString = "0" + preformattedDate.ToString();
            }
            else
            {
                userIntAsString = preformattedDate.ToString();
            }
            datePieces.Add(userIntAsString);

            AnsiConsole.MarkupLine("\nPlease enter the [bold yellow]DAY[/] of the month for the date of your new habit record (dd)");
            preformattedDate = GetUserIntInput(voiceMode, gettingDay: true);
            if (preformattedDate < 10)
            {
                userIntAsString = "0" + preformattedDate.ToString();
            }
            else
            {
                userIntAsString = preformattedDate.ToString();
            }

            datePieces.Add(userIntAsString);
            date = String.Join("-", datePieces);
        }
        else
        {
            DateTime dateAndTime = DateTime.Now;
            date = DateTime.Now.ToString("yyyy-MM-dd");
            Console.WriteLine(date);
        }
        return date;
    }

    internal static int GetUserIntInput(bool voiceMode, bool gettingYear = false, bool gettingDay = false, bool gettingMonth = false)
    {
        bool gettingDate = false;
        string? maybeNumber = "";
        bool validNumber = false;
        int realNumber = -1;

        if (gettingYear || gettingMonth || gettingDay)
        {
            gettingDate = true;
        }

        do
        {
            if (voiceMode)
            {
                maybeNumber = GetVoiceInput().Result;
            }
            else
            {
                maybeNumber = Console.ReadLine();
            }

            if (int.TryParse(maybeNumber, out realNumber))
            {
                if (gettingDate)
                {
                    if (realNumber <= 0)
                    {
                        AnsiConsole.MarkupLine("Dates can't be negative or zero. Please enter a positive number.");
                    }

                    if (gettingYear && (realNumber > 3000 || realNumber < 1000))
                    {
                        AnsiConsole.MarkupLine("Wow! You must live in another age. We can only do years between 1000 and 3000. Please try again.");
                    }
                    else if (gettingDay && realNumber > 31)
                    {
                        AnsiConsole.MarkupLine("Woah! I don't know which cool time system you're on, but our months only have days between 1 and 31. Please try again.");
                    }
                    else if (gettingMonth && realNumber > 12)
                    {
                        AnsiConsole.MarkupLine("Woah! I don't know what cool calendar you're on, but ours only has months between 1 and 12.");
                    }
                    else
                    {
                        validNumber = true;
                    }
                }
                else
                {
                    validNumber = true;
                }
            }
            else
            {
                AnsiConsole.MarkupLine("I'm sorry, but I didn't understand that number. Please try again.");
            }
        } while (!validNumber);
        return realNumber;
    }

    internal static async Task<String> GetVoiceInput()
    {
        int repeatCounter = 0;
        RecognitionResult result;
        SpeechConfig speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
        speechConfig.SpeechRecognitionLanguage = "en-US";

        using SpeechRecognizer recognizer = new SpeechRecognizer(speechConfig);

        do
        {
            result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                string userVoiceInput = result.Text;
                AnsiConsole.MarkupLine($"[bold yellow]Speech Input Recognized:[/] {userVoiceInput}");

                userVoiceInput = userVoiceInput.Trim().ToLower();
                userVoiceInput = Regex.Replace(userVoiceInput, @"[^a-z0-9\s]", "");

                return userVoiceInput;
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                AnsiConsole.MarkupLine($"An error occured during speech recognition: \n\t{CancellationDetails.FromResult(result)}");
            }
            else
            {
                if (repeatCounter < 1)
                {
                    AnsiConsole.MarkupLine("I'm sorry, but I didn't understand what you said. Please try again.");
                }
                repeatCounter++;
            }
        } while (result.Reason != ResultReason.RecognizedSpeech);

        return "UnexpectedVoiceResult Error";
    }

    internal static Panel ShowMenuOptionsPanel(bool voiceMode)
    {
        Grid grid = new Grid();
        grid.AddColumn();

        grid.AddRow("[aqua]Create[/] - Create a new habit");
        grid.AddRow("[aqua]Edit[/] - Edit or Update an existing habit");
        grid.AddRow("[aqua]Delete[/] - Delete all data relating to a habit.");
        grid.AddRow("[aqua]Report[/] - View statistics about all habits.");

        return new Panel(grid)
        {
            Header = new PanelHeader("[yellow]Choose an Option:[/]"),
            Border = BoxBorder.Rounded,
            Padding = new Padding(1)
        };
    }
}
