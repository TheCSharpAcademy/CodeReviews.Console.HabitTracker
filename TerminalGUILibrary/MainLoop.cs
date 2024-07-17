using Terminal.Gui;


namespace TerminalGUILibrary
{
    public class HabitTrackerTopLevel : Toplevel
    {
        static ColorScheme _colorScheme;
        static FrameView LeftPane;
        static FrameView RightPane;
        static ListView CategoryListView;
        static ListView ViewListView;
        static List<string> _category = Features.GetAllCategories();
        static List<Features> _features = Features.GetAllFeatures();
        static Features? _selectedFeature = null;
        private int _cachedCategoryIndex;
        private int _cachedViewIndex;

        public HabitTrackerTopLevel()
        {
            ColorScheme = _colorScheme = Colors.Base;
            MenuBar = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_File", new MenuItem[]
                {
                    new MenuItem("_Quit", "", () => {
                        _selectedFeature = null;
                        Application.RequestStop();
                        })
                }),
                new MenuBarItem("_Help", new MenuItem[]
                {
                    new MenuItem("_About", "", () => MessageBox.Query("About", "Habit Tracker\n\nVersion 1.0", "Ok"))
                })
            });

            LeftPane = new FrameView("Categories")
            {
                X = 0,
                Y = 1, // for menu
                Width = 25,
                Height = Dim.Fill(1),
                CanFocus = true,
                Shortcut = Key.CtrlMask | Key.C
            };
            LeftPane.Title = $"{LeftPane.Title} ({LeftPane.ShortcutTag})";
            LeftPane.ShortcutAction = () => LeftPane.SetFocus();

            CategoryListView = new ListView(_category)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                CanFocus = true
            };
            CategoryListView.SelectedItemChanged += CategoryListView_SelectedChanged;
            CategoryListView.OpenSelectedItem += (e) => RightPane.SetFocus();

            LeftPane.Add(CategoryListView);

            RightPane = new FrameView("Features")
            {
                X = Pos.Right(LeftPane),
                Y = 1, // for menu
                Width = Dim.Fill(),
                Height = Dim.Fill(1),
                CanFocus = true,
                Shortcut = Key.CtrlMask | Key.F
            };
            RightPane.Title = $"{RightPane.Title} ({RightPane.ShortcutTag})";
            RightPane.ShortcutAction = () => RightPane.SetFocus();

            ViewListView = new ListView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                CanFocus = true
            };
            ViewListView.OpenSelectedItem += ViewListView_OpenSelected;
            RightPane.Add(ViewListView);

            Add(MenuBar, LeftPane, RightPane);

            // Restore previous selections
            CategoryListView.SelectedItem = _cachedCategoryIndex;
            ViewListView.SelectedItem = _cachedViewIndex;
        }

        public Features Run()
        {
            Application.Run<HabitTrackerTopLevel>();
            Application.Shutdown();

            return _selectedFeature;
        }

        void CategoryListView_SelectedChanged(ListViewItemEventArgs e)
        {
            var item = _category[e.Item];
            List<Features> newlist = _features.Where(s => s.GetCategories().Contains(item)).ToList();

            ViewListView.SetSource(newlist.ToList());
        }

        void ViewListView_OpenSelected(ListViewItemEventArgs e)
        {
            _cachedCategoryIndex = CategoryListView.SelectedItem;
            _cachedViewIndex = ViewListView.SelectedItem;

            _selectedFeature = (Features)Activator.CreateInstance(ViewListView.Source.ToList()[ViewListView.SelectedItem].GetType());
            Application.RequestStop();
        }
    }
}