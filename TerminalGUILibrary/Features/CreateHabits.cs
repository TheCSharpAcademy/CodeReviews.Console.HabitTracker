using ConnectionLibrary;
using Terminal.Gui;

namespace TerminalGUILibrary.Feature
{
    [FeaturesMetadata(Name: "Create Habits", Description: "Provide UI for creating habits.")]
    [FeaturesCategory("Create")]
    public class CreateHabits : Features
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

            var deadlineLabel = new Label("Deadline: ")
            {
                X = 2,
                Y = 4,
            };
            Win.Add(deadlineLabel);

            var deadlineTextField = new TextField("")
            {
                X = Pos.Right(deadlineLabel),
                Y = 4,
                Width = Dim.Percent(50),
            };
            Win.Add(deadlineTextField);

            var deadlineTipsLabel = new Label("(Recommended format: yyyy-MM-dd HH:mm:ss)")
            {
                X = Pos.Right(deadlineTextField),
                Y = 4,
            };
            Win.Add(deadlineTipsLabel);

            var goalLabel = new Label("Goal(number only): ")
            {
                X = 2,
                Y = 6,
            };
            Win.Add(goalLabel);

            var goalTextField = new TextField("")
            {
                X = Pos.Right(goalLabel),
                Y = 6,
                Width = Dim.Fill(),
            };
            Win.Add(goalTextField);

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
                Height = Dim.Fill(3),
                Width = Dim.Percent(40),
                HideDropdownListOnClick = true
            };
            Win.Add(goalTypeComboBox);

            var items = new List<string>() { "Daily", "Weekly", "Monthly", "Yearly" };
            goalTypeComboBox.SetSource(items);

            var goalMeasurementLabel = new Label("Goal Measurement(ex: km for jogging): ")
            {
                X = 2,
                Y = 10,
            };
            Win.Add(goalMeasurementLabel);

            var goalMeasurementTextField = new TextField("")
            {
                X = Pos.Right(goalMeasurementLabel),
                Y = 10,
                Width = Dim.Fill(),
            };
            Win.Add(goalMeasurementTextField);

            var goalCompletionLabel = new Label("Goal Completion(0 means incomplete, 1 means completed): ")
            {
                X = 2,
                Y = 12,
            };
            Win.Add(goalCompletionLabel);

            var goalCompletionTextField = new TextField("")
            {
                X = Pos.Right(goalCompletionLabel),
                Y = 12,
                Width = Dim.Fill(),
            };
            Win.Add(goalCompletionTextField);

            var addButton = new Button("Add")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(goalCompletionTextField) + 2,
            };
            Win.Add(addButton);

            addButton.Clicked += () =>
            {
                bool validDeadline = DateTime.TryParse(deadlineTextField.Text.ToString(), out DateTime parsedDeadline);
                var validGoal = long.TryParse(goalTextField.Text.ToString(), out long parsedGoal);
                long? goal = validGoal ? parsedGoal : (long?)null;
                var validGoalCompletion = long.TryParse(goalCompletionTextField.Text.ToString(), out long parsedGoalCompletion);
                long? goalCompletion = validGoalCompletion ? parsedGoalCompletion : (long?)null;

                if (habitTextField.Text.ToString() == "")
                {
                    MessageBox.ErrorQuery("Error", "Habit cannot be empty", "Ok");
                    return;
                }
                if (!validDeadline && deadlineTextField.Text.ToString() != "")
                {
                    MessageBox.ErrorQuery("Error", "Deadline must be a date", "Ok");
                    return;
                }
                if (validDeadline && parsedDeadline <= DateTime.Now)
                {
                    MessageBox.ErrorQuery("Error", "Deadline must be in the future", "Ok");
                    return;
                }
                if (!validGoal && goalTextField.Text.ToString() != "")
                {
                    MessageBox.ErrorQuery("Error", "Goal must be a number", "Ok");
                    return;
                }
                if (parsedGoal < 0)
                {
                    MessageBox.ErrorQuery("Error", "Goal must be a positive number", "Ok");
                    return;
                }
                if (goalCompletion != 0 && goalCompletion != 1 && goalCompletionTextField.Text.ToString() != "")
                {
                    MessageBox.ErrorQuery("Error", "Goal completion must be 0 or 1 or empty", "Ok");
                    return;
                }
                try
                {
                    connection.ExecuteDbCommand
                (
                    "InsertHabit",
                    0,
                    "",
                    habitTextField.Text.ToString(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    deadlineTextField.Text.ToString(),
                    goal,
                    goalTypeComboBox.Text.ToString(),
                    goalCompletion,
                    goalMeasurementTextField.Text.ToString()
                );
                }
                catch (Exception ex)
                {
                    MessageBox.ErrorQuery(60, 20, "Failed to create habit", ex.Message, "Ok");
                    return;
                }
                MessageBox.Query("Success", "Habit added successfully", "Ok");
            };
        }
    }
}