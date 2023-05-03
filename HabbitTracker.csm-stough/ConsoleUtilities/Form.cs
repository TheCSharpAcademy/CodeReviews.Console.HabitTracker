using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUtilities
{
    public class Form
    {

        private struct FormStep
        {
            public string type;
            public string prompt;

            public FormStep(string type, string prompt)
            {
                this.type = type;
                this.prompt = prompt;
            }
        }

        public delegate void Process(List<Object> values);
        private Process processor;

        private List<FormStep> steps;

        public Form(Process processor)
        {
            steps = new List<FormStep>();
            this.processor = processor;
        }

        /// <summary>
        /// Add a query for type T
        /// Currently supports:
        /// - string
        /// - int
        /// - float
        /// - DateTime (dd/MM/yyyy format only)
        /// **All other types default to string**
        /// </summary>
        /// <typeparam name="T">Type to query</typeparam>
        /// <param name="prompt">Prompt to display</param>
        public void AddQuery<T>(string prompt)
        {
            steps.Add(new FormStep(typeof(T).ToString(), prompt));
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
                        values.Add(Input.GetDate(step.prompt));
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
