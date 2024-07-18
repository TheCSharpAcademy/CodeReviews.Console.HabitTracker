using ConnectionLibrary;
using Terminal.Gui;

namespace TerminalGUILibrary.Feature
{
    [FeaturesMetadata(Name: "Delete Specific Habit", Description: "Provide UI for deleting specific habits. Filtered by Habit name, Date, Goal Type and/or Completion Status")]
    [FeaturesCategory("Delete")]
    public class DeleteSpecificHabits : Features
    {
        public override void Setup()
        {
            ConnectionService connection = new ConnectionService();
            if (MainMenu.isSeeded)
            {
                connection = new ConnectionService("SeededDb.db");
            }

            var habitLabel = new Label("Habit: ")
            {
                X = 2,
                Y = 2,
            };
            Win.Add(habitLabel);

            var habitTextField = new TextField("")
            {
                X = Pos.Right(habitLabel),
                Y = 2,
                Width = Dim.Fill(),
            };
            Win.Add(habitTextField);

            var dateLabel = new Label("Date: ")
            {
                X = 2,
                Y = 4,
            };
            Win.Add(dateLabel);

            var dateTextField = new TextField("")
            {
                X = Pos.Right(dateLabel),
                Y = 4,
                Width = Dim.Fill(),
            };
            Win.Add(dateTextField);

            var goalTypeLabel = new Label("Goal Type: ")
            {
                X = 2,
                Y = 8,
            };
            Win.Add(goalTypeLabel);

            var goalTypeComboBox = new ComboBox("")
            {
                X = Pos.Right(goalTypeLabel),
                Y = 8,
                Height = Dim.Fill(2),
                Width = Dim.Percent(40),
                HideDropdownListOnClick = true,
            };
            Win.Add(goalTypeComboBox);

            var items = new List<string>() { "Daily", "Weekly", "Monthly", "Yearly" };
            goalTypeComboBox.SetSource(items);

            var goalTypeTipsLabel = new Label("(click the down arrow to select)")
            {
                X = Pos.Right(goalTypeComboBox),
                Y = 8,
            };
            Win.Add(goalTypeTipsLabel);

            var goalCompletionLabel = new Label("Goal Completion(0 means incomplete, 1 means completed): ")
            {
                X = 2,
                Y = 6,
            };
            Win.Add(goalCompletionLabel);

            var goalCompletionTextField = new TextField("")
            {
                X = Pos.Right(goalCompletionLabel),
                Y = 6,
                Width = Dim.Fill(),
            };
            Win.Add(goalCompletionTextField);

            var searchButton = new Button("Search")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(goalTypeLabel) + 2,
            };
            Win.Add(searchButton);

            searchButton.Clicked += () =>
            {
                if
                (
                    habitTextField.Text.ToString() == "" &&
                    dateTextField.Text.ToString() == "" &&
                    goalTypeComboBox.Text.ToString() == "" &&
                    goalCompletionTextField.Text.ToString() == ""
                )
                {
                    MessageBox.ErrorQuery("Error", "At least one field must be filled", "Ok");
                    return;
                }
                var title = "CTRL + Q to quit";
                var activatedWindow = new Window(title);

                var currDate = new DateField(DateTime.Now)
                {
                    X = Pos.Center(),
                    Y = 2,
                    IsShortFormat = false,
                    ReadOnly = true,
                };

                var currDayOfWeek = new Label(DateTime.Now.DayOfWeek.ToString())
                {
                    X = Pos.Right(currDate) + 1,
                    Y = 2,
                    Width = Dim.Fill(),
                };
                activatedWindow.Add(currDate, currDayOfWeek);

                var tableView = new TableView
                {
                    X = Pos.Center(),
                    Y = 4,
                    Width = Dim.Fill(4),
                    Height = Dim.Fill(2),
                    FullRowSelect = true,
                    MultiSelect = true,
                };
                try
                {
                    tableView.Table = connection.GetFilteredResults
                    (
                        habitTextField.Text.ToString(),
                        dateTextField.Text.ToString(),
                        goalTypeComboBox.Text.ToString(),
                        goalCompletionTextField.Text.ToString()
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.ErrorQuery(60, 20, "Failed to filter results", ex.Message, "Ok");
                    return;
                }
                activatedWindow.Add(tableView);

                tableView.CellActivated += (e) => {
                var delete = MessageBox.Query(
                    "Delete Habit?",
                    "Are you sure you want to delete this habit?",
                    "Yes",
                    "No");

                long id = (long)tableView.Table.Rows[e.Row][0];    
                if (delete == 0)
                {
                    connection.ExecuteDbCommand("DeleteHabit", id);
                    tableView.Table = connection.GetFilteredResults
                    (
                        habitTextField.Text.ToString(),
                        dateTextField.Text.ToString(),
                        goalTypeComboBox.Text.ToString(),
                        goalCompletionTextField.Text.ToString()
                    );
                    tableView.SetNeedsDisplay();
                }
            };
                Application.Run(activatedWindow);
            };

            var clearButton = new Button("Clear")
            {
                X = Pos.Right(searchButton) + 2,
                Y = Pos.Bottom(goalTypeLabel) + 2,
            };
            Win.Add(clearButton);

            clearButton.Clicked += () =>
            {
                habitTextField.Text = "";
                dateTextField.Text = "";
                goalTypeComboBox.Text = "";
                goalCompletionTextField.Text = "";
            };
        }
    }
}