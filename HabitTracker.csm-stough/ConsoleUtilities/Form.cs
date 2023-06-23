namespace ConsoleUtilities
{
    public class Form
    {
        /// <summary>
        /// Empty choice structure used for the the form step type switch statement
        /// </summary>
        private struct Choice { };

        /// <summary>
        /// Represents a single step of the form input process
        /// </summary>
        private struct FormStep
        {
            public string type;
            public string prompt;
            public string[]? props;

            public FormStep(string type, string prompt, string[]? props)
            {
                this.type = type;
                this.prompt = prompt;
                this.props = props;
            }
        }

        public delegate void Process(List<Object> values);
        private Process processor;

        private List<FormStep> steps;

        /// <summary>
        /// Provides a delegate to be called after all values are entered by the user
        /// </summary>
        /// <param name="processor"></param>
        public Form(Process processor)
        {
            steps = new List<FormStep>();
            this.processor = processor;
        }

        /// <summary>
        /// Used as a common entry point for all query steps
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prompt"></param>
        /// <param name="props"></param>
        private void AddQuery<T>(string prompt, params string[]? props)
        {
            steps.Add(new FormStep(typeof(T).ToString(), prompt, props));
        }

        /// <summary>
        /// Adds an integer value query to the current form
        /// </summary>
        /// <param name="prompt"></param>
        public void AddIntQuery(string prompt)
        {
            AddQuery<int>(prompt);
        }

        /// <summary>
        /// Adds a floating point value query to the current form
        /// </summary>
        /// <param name="prompt"></param>
        public void AddFloatQuery(string prompt)
        {
            AddQuery<float>(prompt);
        }

        /// <summary>
        /// Adds a string query to the current form
        /// </summary>
        /// <param name="prompt"></param>
        public void AddStringQuery(string prompt)
        {
            AddQuery<string>(prompt);
        }

        /// <summary>
        /// Adds a DateTime query to the current form with the given format
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="format"></param>
        public void AddDateTimeQuery(string prompt, string format)
        {
            AddQuery<DateTime>(prompt + $" ({format})", format);
        }

        /// <summary>
        /// Add a choice query to the current form
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="choices"></param>
        public void AddChoiceQuery(string prompt, params string[] choices)
        {
            AddQuery<Choice>(prompt, choices);
        }

        /// <summary>
        /// Begins the form entry process, then calls the supplied Processor delegate upon completion
        /// </summary>
        public void Start()
        {
            List<Object> values = new List<Object>();

            steps.ForEach((step) =>
            {
                switch(step.type)
                {
                    case "int":
                        values.Add(Input.GetInt(step.prompt));
                        break;
                    case "float":
                        values.Add(Input.GetFloat(step.prompt));
                        break;
                    case "System.DateTime":
                        values.Add(Input.GetDate(step.prompt, step.props[0]));
                        break;
                    case "ConsoleUtilities.Form+Choice":
                        values.Add(Input.GetChoice(step.prompt, step.props));
                        break;
                    case "string":
                    default:
                        Console.WriteLine(step.prompt);
                        values.Add(Console.ReadLine());
                        break;
                }
            });

            this.processor(values);
        }
    }
}
