namespace HabitTracker
{
    internal class Menu
    {
        public string Title { get; private set; }
        public string Message { get; private set; }
        public List<string> Options { get; private set; }

        public Menu(string title, string message, List<string> options) 
        {
            Title = title;
            Message = message;
            Options = options;
        }

        public void PrintMenu()
        {
            Console.WriteLine(Message);
            Console.WriteLine();

            PrintOptions();
            Console.WriteLine();
            Console.WriteLine();
        }
        private void PrintOptions()
        {
            int i = 1;

            foreach (string option in Options)
            {
                Console.WriteLine($"\t{i,4}: {option}");
                i++;
            }
        }
    }
}
