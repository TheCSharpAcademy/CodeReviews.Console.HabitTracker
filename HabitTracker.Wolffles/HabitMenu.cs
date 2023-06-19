namespace habit_tracker;
internal class HabitMenu
{
	enum Options
	{
		closeApp,
		viewRecord,
		insertRecord,
		deleteRecord,
		updateRecord,
		deleteAllRecord,
		maxMenuOptions
	}
	private IHabit m_habit;
	public HabitMenu(IHabit habit)
	{
		m_habit = habit;
	}
	public bool MainMenu()
	{
		Console.Clear();

		Console.WriteLine(@"Welcome to your habit tracker. Please enter a number to select an option below:
        0. Close Application
        1. View All Records
        2. Insert Record
        3. Delete Record
        4. Update Record");

		int maxMenuOptions = (int)Options.maxMenuOptions;
		int selectedOption;

		do
		{
			selectedOption = InputValidation.GetUserInputAsInt();
		}
		while (selectedOption > maxMenuOptions && selectedOption < 0);

		switch ((Options)selectedOption)
		{
			case Options.closeApp:
				return false;
			case Options.viewRecord:
				ViewRecord();
				break;
			case Options.insertRecord:
				InsertRecord();
				break;
			case Options.deleteRecord:
				DeleteRecord();
				break;
			case Options.updateRecord:
				UpdateRecord();
				break;

		}
		return true;
	}
	private void ViewRecord()
	{
		Console.Clear();
		m_habit.Read();
		Console.WriteLine("Press any key to exit.");
		Console.ReadKey();
	}

	private void InsertRecord()
	{
		Console.Clear();

		do
		{
			Console.WriteLine($"Enter a new record. Enter number of {m_habit.GetMeasureUnit()}: ");
		}
		while (!m_habit.Insert(InputValidation.GetUserInputAsInt(), InputValidation.GetUserInputAsDate()));

		Console.ReadKey();
		m_habit.Read();
	}
	private void UpdateRecord()
	{
		Console.Clear();
		m_habit.Read();

		Console.WriteLine("Select a record to update by typing it's DATE");
		string date = InputValidation.GetUserInputAsDate();

		Console.WriteLine($"{date} selected. Enter value:");
		int value = InputValidation.GetUserInputAsInt();

		m_habit.Read();
		Console.ReadKey();

	}
	private void DeleteRecord()
	{
		Console.Clear();
		m_habit.Read();

		Console.WriteLine(@"Select a record to delete by typing it's DATE.
------------------------------------------------------------------------");
		m_habit.Delete(InputValidation.GetUserInputAsDate());
		m_habit.Read();

	}

}