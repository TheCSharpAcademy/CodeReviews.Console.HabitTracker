using Terminal.Gui;

namespace TerminalGUILibrary
{
    public class MainMenu : Window
    {
        public static bool isSeeded;
        public static bool isStart = false;
        public MainMenu()
        {
            Title = "Habit Tracker (Ctrl+Q to quit)";

            // Create input components and labels
            var title = new Label()
            {
                Text = @"
██╗  ██╗ █████╗ ██████╗ ██╗████████╗    ████████╗██████╗  █████╗  ██████╗██╗  ██╗███████╗██████╗ 
██║  ██║██╔══██╗██╔══██╗██║╚══██╔══╝    ╚══██╔══╝██╔══██╗██╔══██╗██╔════╝██║ ██╔╝██╔════╝██╔══██╗
███████║███████║██████╔╝██║   ██║          ██║   ██████╔╝███████║██║     █████╔╝ █████╗  ██████╔╝
██╔══██║██╔══██║██╔══██╗██║   ██║          ██║   ██╔══██╗██╔══██║██║     ██╔═██╗ ██╔══╝  ██╔══██╗
██║  ██║██║  ██║██████╔╝██║   ██║          ██║   ██║  ██║██║  ██║╚██████╗██║  ██╗███████╗██║  ██║
╚═╝  ╚═╝╚═╝  ╚═╝╚═════╝ ╚═╝   ╚═╝          ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝
                                                                                                 
",
                X = Pos.Center(),
            };

            var btnStart = new Button("Start")
            {
                Y = Pos.Bottom(title) + 3,
                X = Pos.Center(),
            };

            var btnSeededStart = new Button("Start with sample data")
            {
                Y = Pos.Bottom(btnStart) + 2,
                X = Pos.Center(),
                IsDefault = true,
            };

            btnStart.Clicked += () =>
            {
                isStart = true;
                Application.RequestStop();
            };

            btnSeededStart.Clicked += () =>
            {
                isStart = true;
                isSeeded = true;
                MessageBox.Query(
                    "Seeded Start",
                    "This is only for demonstration purposes only! Data are saved in seperate database file.",
                    "Ok");

                Application.RequestStop();
            };

            // Add the views to the Window
            Add(title, btnStart, btnSeededStart);
        }
    }
}



