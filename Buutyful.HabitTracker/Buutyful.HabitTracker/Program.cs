using Buutyful.HabitTracker;
using Buutyful.HabitTracker.State;


DataAccess dbHelper = new(Constants.DbPath);
dbHelper.CreateDatabase();
var stateManager  = new StateManager(dbHelper);
stateManager.Run(new MainMenuState(stateManager));