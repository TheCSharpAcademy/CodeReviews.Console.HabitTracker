
using System.Globalization;
using System.Text.RegularExpressions;

namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Manages user input for <see cref="HabitTracker"/> database operations
    /// </summary>
    internal class InputManager
    {
        public string DateTimeFormat { get; init; }

        /// <summary>
        /// Initializes new object of <see cref="InputManager"/> class
        /// </summary>
        /// <param name="dateTimeFormat"></param>
        public InputManager(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }
        /// <summary>
        /// Gets new values for the record
        /// </summary>
        /// <returns><see cref="DatabaseRecord"/> object containing values entered by user</returns>
        public DatabaseRecord GetValuesForRecord()
        {
            DatabaseRecord newRecord = new DatabaseRecord();

            newRecord.Date = GetDateForRecord();
            newRecord.Volume = GetVolumeForRecord();

            return newRecord;
        }
        /// <summary>
        /// Gets new values for existing record
        /// </summary>
        /// <param name="ID"><see cref="int"/> value representing ID of modified record</param>
        /// <returns><see cref="DatabaseRecord"/> object containing values entered by user</returns>
        public DatabaseRecord GetValuesForRecord(int ID)
        {
            DatabaseRecord newRecord = new DatabaseRecord();

            newRecord.ID = ID;
            newRecord.Date = GetDateForRecord();
            newRecord.Volume = GetVolumeForRecord();

            return newRecord;
        }
        /// <summary>
        /// Helper method, which get a value for volume
        /// </summary>
        /// <returns>Valid <see cref="int"/> value representing volume</returns>
        private int GetVolumeForRecord()
        {

            string prompt = "Please enter positive numeric value for volume: ";
            int numericInput = GetValidIntegerInput(prompt, 0, int.MaxValue);

            return numericInput;
        }
        /// <summary>
        /// Method to retreive valid <see cref="int"/> value
        /// </summary>
        /// <param name="prompt"><see cref="string"/> representing prompt for the user</param>
        /// <param name="minValue"><see cref="int"/> representing minimum value for the record; value is inclusive</param>
        /// <param name="maxValue"><see cref="int "/>representin maximum value for the record; value is inclusive</param>
        /// <returns>Valid <see cref="int"/> value based on min and max value parameters</returns>
        private int GetValidIntegerInput(string prompt, int minValue, int maxValue)
        {
            int numericInput;
            string? input;

            Console.Write(prompt);
            input = Console.ReadLine();

            while (!int.TryParse(input, out numericInput) || numericInput < minValue || numericInput > maxValue)
            {
                Console.Write("Please enter valid value: ");
                input = Console.ReadLine();
            }

            return numericInput;
        }
        /// <summary>
        /// Method to retreive valid <see cref="int"/> value
        /// </summary>
        /// <param name="prompt"><see cref="string"/> representing prompt for the user</param>
        /// <param name="IDs"><see cref="HashSet{T}"/> of <see cref="int"/> values represeting data set of valid integers</param>
        /// <returns>Valid <see cref="int"/> value based on presented HashsSet</returns>
        private int GetValidIntegerInput(string prompt, HashSet<int> IDs)
        {
            int numericInput;
            string? input;

            Console.Write(prompt);
            input = Console.ReadLine();

            while (!int.TryParse(input, out numericInput) || (!IDs.Contains(numericInput) && numericInput != 0))
            {
                Console.Write("Please enter valid value: ");
                input = Console.ReadLine();
            }

            return numericInput;
        }
        /// <summary>
        /// Retreives <see cref="DateTime"/> value from user based on DateTime format property of <see cref="InputManager"/> object
        /// </summary>
        /// <returns>Valid <see cref="DateTime"/> value on DateTime format property of <see cref="InputManager"/> object</returns>
        public DateTime GetDateForRecord()
        {
            DateTime date;
            while (true)
            {
                Console.Write($"Please enter date value in {DateTimeFormat.ToUpper()} format, or enter \"today\" for today's date: ");

                string? userInput = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                    continue;
                }
                else if (userInput == "today")
                {
                    return DateTime.Now;
                }
                else if (DateTime.TryParseExact(userInput, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }

                Console.WriteLine("Invalid date format. Please try again.");
            }
        }
        /// <summary>
        /// Gets table name based on user input <see cref="int"/>value representing table in the output
        /// </summary>
        /// <param name="tables"><see cref="List{T}"/> of <see cref="string"/> values representing tables in the database</param>
        /// <returns><see cref="string"/> value representing name of the table or an empty string if user decided to abort the operation</returns>
        public string GetTableNameById(List<string> tables)
        {
            string prompt = "Please enter ID of the table for new record (or press 0 to return): ";
            int numericInput = GetValidIntegerInput(prompt,0,tables.Count);

            return numericInput == 0 ? string.Empty : tables[numericInput - 1];
        }
        /// <summary>
        /// Gets ID of <see cref="DatabaseRecord"/> representing record ID 
        /// </summary>
        /// <param name="records"><see cref="List{T}"/> of <see cref="DatabaseRecord"/> values represting records in the table</param>
        /// <param name="operation"><see cref="string"/> value representing the operation user wish to perform</param>
        /// <returns><see cref="int"/> value representing ID of the record in table, or 0 if user decided to abort the action</returns>
        public int GetRecordIdFromUser(List<DatabaseRecord> records, string operation)
        {
            int numericInput;
            HashSet<int> IDs = new HashSet<int>(records.Select(record => record.ID));

            string prompt = $"Please valid ID of the record you wish to {operation}, or enter \"0\" to return to menu:";

            numericInput = GetValidIntegerInput(prompt, IDs);
            return numericInput;
        }
        /// <summary>
        /// Retreives <see cref="int"/> value when user is in the menu, input is validated to make sure only valid option is processed further
        /// </summary>
        /// <param name="maxValue"><see cref="int"/> value representing maximum valid option in the menu</param>
        /// <returns><see cref="int"/> value representing user choice</returns>
        public int GetInputInMenu(int maxValue)
        {   
            int numericInput = GetValidIntegerInput("", 1, maxValue);
            return numericInput;
        }
        /// <summary>
        /// Gets a <see cref="string"/> value representing name of new table, contains validation to make sure same name isn't present twice
        /// </summary>
        /// <param name="existingTables"><see cref="List{T}"/> of <see cref="string"/> values representing current tables in the database</param>
        /// <returns><see cref="string"/> value representing name of new table</returns>
        public string GetNewTableName(List<string> existingTables)
        {
            string? name;
            Console.WriteLine("Name of the habit can contain only alpha-numeric characters or space between words, leading or trailing spaces are not permitted");
            Console.Write("Please enter habit name: ");
            name = Console.ReadLine();
            while (!IsTableNameValid(name) || existingTables.Contains(name))
            {
                Console.WriteLine("Name invalid or already exists");
                Console.Write("Please enter valid name: ");
                name = Console.ReadLine();
            }
            return name;
        }
        /// <summary>
        /// Gets a <see cref="string"/> value representing of table currently present in the database
        /// </summary>
        /// <param name="existingTables"><see cref="List{T}"/> of <see cref="string"/> values representing current tables in the database</param>
        /// <returns><see cref="string"/> value representing of table currently present in the database, or 0 if user decides to abort the operation</returns>
        public string GetExistingTableName(List<string> existingTables)
        {
            string? name;
            Console.Write("Please enter name of the table to be deleted (or enter \"0\" to return to main menu): ");
            name = Console.ReadLine();
            while (!existingTables.Contains(name) && name != "0")
            {
                Console.Write("Please enter valid name (or enter 0 to return to main menu): ");
                name = Console.ReadLine();
            }
            return name;
        }
        /// <summary>
        /// Checks whether provided <see cref="string"/> value is valid name for the table
        /// Valid name can contain alphanumeric characters and single space between words. No leading or trailing spaces
        /// </summary>
        /// <param name="name"><see cref="string"/> value representing name of the table</param>
        /// <returns><see cref="bool"/> true, if the name is valid; <see cref="bool"/> false otherwise</returns>
        private bool IsTableNameValid(string name)
        {
            if (name == null || name.Length == 0)
                return false;

            return Regex.IsMatch(name, @"^[a-zA-Z0-9]+( [a-zA-Z0-9]+)*$");
        }
        /// <summary>
        /// Gets a valid unit for the table
        /// </summary>
        /// <returns><see cref="string"/> value representing the unit</returns>
        public string GetNewTableUnit()
        {
            string? name;
            Console.Write("Please enter measurement unit: ");
            name = Console.ReadLine();

            while (name == null)
            {
                Console.Write("Please enter valid unit");
            }

            return name;
        }
    }
}
