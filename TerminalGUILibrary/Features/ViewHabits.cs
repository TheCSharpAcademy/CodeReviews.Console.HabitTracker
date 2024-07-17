using ConnectionLibrary;
using Terminal.Gui;

namespace TerminalGUILibrary.Feature
{
    [FeaturesMetadata(Name: "View Habits", Description: "Provide UI for viewing habits.")]
    [FeaturesCategory("View")]
    public class ViewHabits : Features
    {
        public override void Setup()
        {
            ConnectionService connection = new ConnectionService();
            if (MainMenu.isSeeded)
            {
                connection = new ConnectionService("SeededDb.db");
            }

            var currDate = new DateField (DateTime.Now) {
				X = Pos.Center (),
				Y = 2,
				IsShortFormat = false,
				ReadOnly = true,
			};

            currDate.MouseClick += (e) => {
                MessageBox.Query(
                    "Change Date?",
                    "Enter date: ____/__/__",
                    "Ok");
            };

            var currDayOfWeek = new Label (DateTime.Now.DayOfWeek.ToString()) {
				X = Pos.Right (currDate) + 1,
                Y = 2,
                Width = Dim.Fill(),
			};
            Win.Add(currDate, currDayOfWeek);

            var tableView = new TableView
            {
                X = Pos.Center(),
                Y = 4,
                Width = Dim.Fill(4),
                Height = Dim.Fill(2),
                FullRowSelect = true,
                MultiSelect = true,
                Table = connection.ExecuteDbCommand("ViewHabits"),
            };
            Win.Add(tableView);

            var scrollBar = new ScrollBarView (tableView, true);

            scrollBar.ChangedPosition += () => {
                tableView.RowOffset = scrollBar.Position;
                tableView.SetNeedsDisplay();
            };
            Win.Add(scrollBar);

            scrollBar.OtherScrollBarView.ChangedPosition += () => {
                tableView.ColumnOffset = scrollBar.OtherScrollBarView.Position;
                tableView.SetNeedsDisplay();
            };

            tableView.DrawContent += (e) => {
                scrollBar.Size = tableView.Table?.Rows?.Count ?? 0;
                scrollBar.Position = tableView.RowOffset;
                scrollBar.OtherScrollBarView.Size = tableView.MaxCellWidth;
                scrollBar.OtherScrollBarView.Position = tableView.ColumnOffset;
                scrollBar.Refresh();
            };
        }
    }

}