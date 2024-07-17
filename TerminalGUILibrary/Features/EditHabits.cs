using ConnectionLibrary;
using Terminal.Gui;

namespace TerminalGUILibrary.Feature
{
    [FeaturesMetadata(Name: "Edit Habits", Description: "Provide UI for editing habits.")]
    [FeaturesCategory("Edit")]
    public class EditDailyHabits : Features
    {
        TableView tableView;
        public override void Setup()
        {
            ConnectionService connection = new ConnectionService();
            if (MainMenu.isSeeded)
            {
                connection = new ConnectionService("SeededDb.db");
            }

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
            Win.Add(currDate, currDayOfWeek);

            tableView = new TableView
            {
                X = Pos.Center(),
                Y = 4,
                Width = Dim.Fill(4),
                Height = Dim.Fill(2),
                Table = connection.ExecuteDbCommand("ViewHabits"),
            };
            Win.Add(tableView);

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
                        newValueID =  (long)e.Table.Rows[e.Row][0];
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

            var scrollBar = new ScrollBarView(tableView, true);

            scrollBar.ChangedPosition += () =>
            {
                tableView.RowOffset = scrollBar.Position;
                tableView.SetNeedsDisplay();
            };
            Win.Add(scrollBar);

            scrollBar.OtherScrollBarView.ChangedPosition += () =>
            {
                tableView.ColumnOffset = scrollBar.OtherScrollBarView.Position;
                tableView.SetNeedsDisplay();
            };

            tableView.DrawContent += (e) =>
            {
                scrollBar.Size = tableView.Table?.Rows?.Count ?? 0;
                scrollBar.Position = tableView.RowOffset;
                scrollBar.OtherScrollBarView.Size = tableView.MaxCellWidth;
                scrollBar.OtherScrollBarView.Position = tableView.ColumnOffset;
                scrollBar.Refresh();
            };
        }
    }

}