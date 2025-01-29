using System;
using System.Data.SQLite;
using System.IO;
using System.Globalization;
using System.Data;

namespace Habit_Logger
{
    internal class Program
    {

        static void Main(string[] args)
        {
            #region Database creation and connection test
            int keeprunning = 1;
            string dataBase = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\db\DBSQLite.db";
            string strConnection = @"Data Source = " + dataBase + "; Version = 3";
            if (!File.Exists(dataBase))
            {
                SQLiteConnection.CreateFile(dataBase);
            }
            SQLiteConnection conn = new SQLiteConnection(strConnection);
            try
            {
                conn.Open();
                Console.WriteLine("Connected to SQLite");
            }
            catch
            {
                Console.WriteLine("Error connecting to database.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            finally
            {
                conn.Close();
            }
            #endregion
            #region Table creation if not exists
            try
            {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = conn;

                // Table creation SQL
                command.CommandText = @"
                 CREATE TABLE IF NOT EXISTS habitable (
                 entrynumber INTEGER NOT NULL PRIMARY KEY,
                 date TEXT NOT NULL,
                 glnumber INTEGER NOT NULL
                 )";
                command.ExecuteNonQuery();
                Console.WriteLine("Table created in SQLite");
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            #endregion
            Console.WriteLine("Welcome to the Habit Logger!\nDrink plenty of water and register each day's number of glasses here.");
            int glNumber;
            string dateInput = "";
            bool validOption = false;
            int UserInput = -1;
            while (true)
            {
                validOption = false;
                Console.Clear();
                Console.WriteLine("1 - Insert new data");
                Console.WriteLine("2 - View/edit/delete previous data");
                Console.WriteLine("3 - Exit");
                Console.Write("Choose the desired option: ");
                while (!validOption)
                {
                    UserInput = int.Parse(Console.ReadKey().KeyChar.ToString());
                    if (UserInput == 1 || UserInput == 2 || UserInput == 3)
                    {
                        validOption = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option.");
                    }
                    switch (UserInput)
                    {
                        case 1:
                            {
                                insertInfo();
                                break;
                            }
                        case 2:
                            {
                                viewData();
                                innerMenu();
                                break;
                            }
                        case 3:
                            {
                                Console.WriteLine("\nExiting program. Goodbye!");
                                Environment.Exit(0);
                                break;
                            }
                    }
                }
            }

            void viewData()
            {
                try
                {
                    SQLiteCommand command = new SQLiteCommand();
                    command.Connection = conn;
                    DataTable data = new DataTable();
                    SQLiteDataAdapter adaptador = new SQLiteDataAdapter("SELECT * FROM habitable", strConnection);
                    conn.Open();
                    adaptador.Fill(data);
                    Console.WriteLine("\nContents of the 'habitable' table:");
                    if (data.Rows.Count > 0)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            Console.WriteLine($"Entry Number: {row["entrynumber"]}, Date: {row["date"]}, Glasses: {row["glnumber"]}");
                        }
                        Console.WriteLine("----------------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine("The table is empty.");
                    }
                    command.Dispose();
                }
                catch
                {
                    Console.Clear();
                    Console.Write("Data query failed. Press any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(0);

                }
                finally
                {
                    conn.Close();
                }
            }

            void innerMenu()
            {
                #region Enter submenu option and check if valid
                Console.WriteLine("Choose an option:");
                Console.Write("1 - Delete a record\n2 - Edit a record\n3 - Return to main menu: ");
                int userInput = -1; // Initialize with a default value
                bool validInput = false;
                while (!validInput)
                {
                    try
                    {
                        userInput = int.Parse(Console.ReadKey().KeyChar.ToString());
                        if (userInput == 1 || userInput == 2 || userInput == 3)
                        {
                            validInput = true; // Successfully parsed input
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("\nInvalid input. Please choose a valid option.");
                    }
                }
                #endregion
                switch (userInput)
                {
                    case 1: //Delete
                        {
                            SQLiteCommand command = new SQLiteCommand();
                            command.Connection = conn;
                            DataTable data = new DataTable();
                            SQLiteDataAdapter adaptador = new SQLiteDataAdapter("SELECT * FROM habitable", conn);
                            try
                            {
                                conn.Open();
                                adaptador.Fill(data);
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                                break;  // Exit or handle appropriately
                            }
                            finally
                            {
                                command.Dispose();
                            }

                            if (data.Rows.Count == 0)  // Check if the table is empty
                            {
                                Console.WriteLine("The table is empty.");
                            }

                            Console.Write("\nSelect the ID of the item to be deleted:");
                            validInput = false;
                            while (!validInput)
                            {
                                try
                                {
                                    userInput = int.Parse(Console.ReadLine().ToString());
                                    validInput = true; // Successfully parsed input
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("\nInvalid input. Please choose a valid option.");
                                }
                            }

                            conn.Open();
                            try
                            {
                                // Check if the entrynumber exists
                                SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM habitable WHERE entrynumber = @entrynumber", conn);
                                checkCommand.Parameters.AddWithValue("@entrynumber", userInput);

                                // Execute the query and get the result
                                int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                                checkCommand.Dispose();
                                if (count == 0)
                                {
                                    Console.WriteLine("No entry found with the specified entry number.\nPress any key to return to start menu.");
                                    Console.ReadKey();
                                    break;
                                }
                                else
                                {
                                    // Proceed to delete if the entry exists
                                    SQLiteCommand commandDelete = new SQLiteCommand("DELETE FROM habitable WHERE entrynumber = @entrynumber", conn);
                                    commandDelete.Parameters.AddWithValue("@entrynumber", userInput);

                                    commandDelete.ExecuteNonQuery();
                                    Console.WriteLine("Entry successfully deleted.\nPress any key to return to start menu.");
                                    commandDelete.Dispose();
                                    Console.ReadKey();
                                    break;
                                }


                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }
                            finally
                            {
                                conn.Close();
                            }


                            break;
                        }
                    case 2: //Edit
                        {
                            SQLiteCommand command = new SQLiteCommand();
                            command.Connection = conn;
                            DataTable data = new DataTable();
                            SQLiteDataAdapter adaptador = new SQLiteDataAdapter("SELECT * FROM habitable", conn);
                            try
                            {
                                conn.Open();
                                adaptador.Fill(data);
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                                break;  // Exit or handle appropriately
                            }
                            finally
                            {
                                command.Dispose();
                            }

                            if (data.Rows.Count == 0)  // Check if the table is empty
                            {
                                Console.WriteLine("The table is empty.");
                            }

                            Console.Write("\nSelect the ID of the item to be edited:");
                            validInput = false;
                            while (!validInput)
                            {
                                try
                                {
                                    userInput = int.Parse(Console.ReadLine().ToString());
                                    validInput = true; 
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("\nInvalid input. Please choose a valid option.");
                                }
                            }
                            validInput = false;
                            conn.Open();
                            try
                            {
                                // Check if the entrynumber exists
                                SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM habitable WHERE entrynumber = @entrynumber", conn);
                                checkCommand.Parameters.AddWithValue("@entrynumber", userInput);

                                // Execute the query and get the result
                                int glnumber2 = -1;
                                int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                                checkCommand.Dispose();
                                conn.Close();
                                if (count == 0)
                                {
                                    Console.WriteLine("No entry found with the specified entry number.\nPress any key to return to start menu.");
                                    Console.ReadKey();
                                    break;
                                }
                                else
                                {
                                    // Proceed to edit if the entry exists
                                    Console.WriteLine("----------------------------------------------");
                                    Console.Write($"\nEntry number {userInput}. Insert the new number of glasses: ");
                                    validInput = false;
                                    while (!validInput)
                                    {
                                        try
                                        {
                                            glnumber2 = int.Parse(Console.ReadLine().ToString());
                                            validInput = true; // Successfully parsed input
                                        }
                                        catch (FormatException)
                                        {
                                            Console.WriteLine("\nInvalid input. Please choose a valid option.");
                                        }
                                    }
                                    Console.Write("Enter the date in format YYYY-MM-DD: ");
                                    validInput = false;
                                    while (!validInput)
                                    {
                                        dateInput = Console.ReadLine();
                                        if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                                        {
                                            validInput = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid date format. Please try again.");
                                        }
                                    }
                                    validInput = false;
                                    SQLiteCommand editcommand = new SQLiteCommand();
                                    editcommand.CommandText = "UPDATE habitable SET glnumber = '" + glnumber2 + "', date = '" + dateInput + "' WHERE entrynumber LIKE '" + userInput + "'";
                                    editcommand.Connection = conn;
                                    conn.Open();
                                    editcommand.ExecuteNonQuery();
                                    Console.WriteLine("Entry updated successfully. Press any key to return to Start Menu.");
                                    Console.ReadKey();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("An error occurred: " + ex.Message);
                            }
                            finally
                            {
                                conn.Close();
                            }
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            break;
                        }
                }
            }

            void insertInfo()
            {
                try
                {
                    // Prompt user for data
                    Console.Write("\nEnter the number of glasses (integer): ");
                    glNumber = int.Parse(Console.ReadLine());

                    while (true)
                    {
                        Console.Write("Enter the date in format YYYY-MM-DD: ");
                        dateInput = Console.ReadLine();                        
                        if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        {
                            break; 
                        }
                        else
                        {
                            Console.WriteLine("Invalid date format. Please try again.");
                        }
                    }                    
                    conn.Open();
                    SQLiteCommand insertCommand = new SQLiteCommand(@"
                    INSERT INTO habitable (date, glnumber) 
                    VALUES (@date, @glnumber)", conn);
                    insertCommand.Parameters.AddWithValue("@date", dateInput);
                    insertCommand.Parameters.AddWithValue("@glnumber", glNumber);
                    insertCommand.ExecuteNonQuery();
                    Console.WriteLine("Data successfully inserted into the database.");
                    Console.Write("Press a key to return to Start Menu.");
                    Console.ReadKey();
                    Console.Clear();
                    insertCommand.Dispose();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter numeric values for the glasses.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}


