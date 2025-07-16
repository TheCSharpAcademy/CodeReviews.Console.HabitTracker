using HabitTracker.Application.Services;
using HabitTracker.Domain.Repositories;
using HabitTracker.Infrastructure.Persistence;
using HabitTracker.Infrastructure.Repositories;
using HabitTracker.UI;
using HabitTracker.UI.Controllers;
using HabitTracker.UI.Interfaces;

const string connectionString = "Data Source = habitdb";
Initializer initializer = new Initializer(connectionString);
IHabitRepository habitRepo = new HabitRepository(connectionString);
IOccurrenceRepository occurrenceRepo = new OccurrenceRepository(connectionString);

HabitService habitService = new HabitService(habitRepo);
OccurrenceService occurrenceService = new OccurrenceService(occurrenceRepo, habitRepo);

IView view = new ConsoleView();
ConsoleUiController controller = new ConsoleUiController(habitService, occurrenceService, view);

initializer.Initialize();
controller.Initialize();
controller.Execute();