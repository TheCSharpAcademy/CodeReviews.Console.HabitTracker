using ConnectionLibrary;
using Terminal.Gui;

namespace TerminalGUILibrary.Feature
{
    [FeaturesMetadata(Name: "Edit Specific Habit", Description: "Provide UI for editing specific habits. Filtered by Habit name, Date, Goal Type and/or Completion Status")]
    [FeaturesCategory("Edit")]
    public class EditSpecificHabits : Features
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
                Y = 6,
            };
            Win.Add(goalTypeLabel);

            var goalTypeComboBox = new ComboBox("")
            {
                X = Pos.Right(goalTypeLabel),
                Y = 6,
                Height = Dim.Fill(2),
                Width = Dim.Percent(40),
                HideDropdownListOnClick = true,
                ReadOnly = true,
            };
            Win.Add(goalTypeComboBox);

            var items = new List<string>() { "Daily", "Weekly", "Monthly", "Yearly" };
            goalTypeComboBox.SetSource(items);

            var goalCompletionLabel = new Label("Goal Completion(0 means incomplete, 1 means completed): ")
            {
                X = 2,
                Y = 8,
            };
            Win.Add(goalCompletionLabel);

            var goalCompletionTextField = new TextField("")
            {
                X = Pos.Right(goalCompletionLabel),
                Y = 8,
                Width = Dim.Fill(),
            };
            Win.Add(goalCompletionTextField);

            var searchButton = new Button("Search")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(goalCompletionTextField) + 2,
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

                currDate.MouseClick += (e) =>
                {
                    MessageBox.Query(
                        "Change Date?",
                        "Enter date: ____/__/__",
                        "Ok");
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

                tableView.CellActivated += (e) =>
                {
                    if (e.Table == null)
                        return;
                    var columnName = e.Table.Columns[e.Col].ColumnName;

                    var title = "Enter new value";

                    var oldValue = e.Table.Rows[e.Row][e.Col].ToString();
                    bool okPressed = false;

                    var ok = new Button("Ok", is_default: true);
                    ok.Clicked += () => { okPressed = true; Application.RequestStop(); };
                    var cancel = new Button("Cancel");
                    cancel.Clicked += () => { Application.RequestStop(); };
                    var activatedDialog = new Dialog(title, 60, 20, ok, cancel);

                    var dialogLabel = new Label()
                    {
                        X = 0,
                        Y = 1,
                        Text = columnName
                    };

                    var dialogTextField = new TextField()
                    {
                        Text = oldValue,
                        X = 0,
                        Y = 2,
                        Width = Dim.Fill()
                    };

                    activatedDialog.Add(dialogLabel, dialogTextField);
                    dialogTextField.SetFocus();

                    Application.Run(activatedDialog);

                    if (okPressed)
                    {
                        long newValueID = 0;
                        string? newValue = null;

                        try
                        {
                            e.Table.Rows[e.Row][e.Col] = string.IsNullOrWhiteSpace(dialogTextField.Text.ToString()) ? DBNull.Value : (object)dialogTextField.Text;
                            newValueID = (long)e.Table.Rows[e.Row][0];
                            newValue = dialogTextField.Text.ToString();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.ErrorQuery(60, 20, "Failed to set text", ex.Message, "Ok");
                            return;
                        }

                        tableView.Update();
                        connection.ExecuteDbCommand("UpdateHabit", newValueID, columnName, newValue);
                    }
                };

                Application.Run(activatedWindow);
            };

            var clearButton = new Button("Clear")
            {
                X = Pos.Right(searchButton) + 2,
                Y = Pos.Bottom(goalCompletionTextField) + 2,
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