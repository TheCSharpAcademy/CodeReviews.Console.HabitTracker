using HabitLoggerApp.Application;
using HabitLoggerApp.Application.Handlers.HabitLogs;
using HabitLoggerApp.Application.Handlers.Habits;
using HabitLoggerApp.Fixtures;
using HabitLoggerLibrary.DbManager;
using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.HabitLogs;
using HabitLoggerLibrary.Ui.Habits;
using HabitLoggerLibrary.Ui.Input;
using HabitLoggerLibrary.Ui.Menu;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>();
    }).ConfigureServices((context, services) =>
    {
        services.Configure<SpeechRecognizerFactoryOptions>(
            context.Configuration.GetSection(SpeechRecognizerFactoryOptions.Key));
        services.AddSingleton<App>();
        services.AddSingleton<DatabaseManagerFactory>();
        services.AddSingleton<RepositoryFactory>();
        services.AddSingleton<Loop>();
        services.AddSingleton<FixturesGenerator>();
        services.AddSingleton<IKeyAwaiter, ConsoleKeyAwaiter>();
        services.AddSingleton<IConsoleWrapper, ConsoleWrapper>();
        services.AddSingleton<IMainMenuChoiceReader, MainMenuChoiceReader>();
        services.AddSingleton<IDatabaseManager>(serviceProvider =>
            serviceProvider.GetService<DatabaseManagerFactory>()!.Create());
        services.AddSingleton<IHabitsRepository>(serviceProvider =>
            serviceProvider.GetService<RepositoryFactory>()!.CreateHabitsRepository());
        services.AddSingleton<IHabitChoiceReader, ConsoleHabitChoiceReader>();
        services.AddSingleton<DeleteHabitHandler>();
        services.AddSingleton<ViewHabitsHandler>();
        services.AddSingleton<InsertHabitHandler>();
        services.AddSingleton<IInputChoiceReader, ConsoleInputChoiceReader>();
        services.AddSingleton<IInputReaderFactory, InputReaderFactory>();
        services.AddSingleton<EditHabitHandler>();
        services.AddSingleton<IInputReaderSelector, InputReaderSelector>();
        services.AddSingleton<HabitLogsMainMenuHandler>();
        services.AddSingleton<IHabitLogsMenuChoiceReader, HabitLogsMenuChoiceReader>();
        services.AddSingleton<ViewHabitLogsHandler>();
        services.AddSingleton<IHabitLogsRepository>(serviceProvider =>
            serviceProvider.GetService<RepositoryFactory>()!.CreateHabitLogsRepository());
        services.AddSingleton<DeleteHabitLogHandler>();
        services.AddSingleton<IHabitLogChoiceReader, ConsoleHabitLogChoiceReader>();
        services.AddSingleton<InsertHabitLogHandler>();
        services.AddSingleton<EditHabitLogHandler>();
        services.AddSingleton<StatisticsHandler>();
        services.AddSingleton<ISpeechInputReaderFactory, SpeechInputReaderFactory>();
    });

using var host = builder.Build();

var app = host.Services.GetService<App>()!;
app.Run();