using HabitDatabaseLibrary;
using System.Data.SQLite;


namespace HabitTrackerProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();
            bool exit = false;
            string? userInput;
            int optionSelected;

            SQLiteConnection sqlite_conn = db.CreateConnection();    
            db.CreateTable(sqlite_conn);


            do
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("------------------------------------------");
                Console.Write("Option: ");
                userInput = Console.ReadLine();
                if (userInput == null || userInput == "")
                {
                    Console.WriteLine("Error: Menu option cannot be empty.");
                    continue;
                }
                else if (!int.TryParse(userInput, out optionSelected))
                {
                    Console.WriteLine("Error: Menu option must be an intergal number.");
                    continue;
                }
                else
                {
                    switch (optionSelected)
                    {
                        case 0: // Close application
                            Console.Clear();

                            db.CloseConnection(sqlite_conn);

                            Console.WriteLine("Application will close in 3 seconds.");
                            Thread.Sleep(3000);

                            exit = true;

                            break;
                        case 1: // View All Records - view all habits

                            db.ReadData(sqlite_conn);

                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();

                            break;
                        case 2: // Insert Records - Add a habit
                            exit = false;
                            do
                            {
                                Console.Clear();
                                Console.Write("Enter Habit Name (or enter 'e' to return to menu): ");
                                userInput = Console.ReadLine();
                                
                                if (userInput == "" || userInput == null)
                                {
                                    continue;
                                }
                                
                                else if (userInput.ToLower() == "e")
                                    exit = true;
                                
                                else
                                {
                                    db.InsertData(sqlite_conn, userInput.ToLower());
                                    exit = true;
                                }

                            } while (!exit);
                            exit = false;
                            
                            break;
                        case 3: // Delete Records - Delete a habit
                            exit = false;
                            string? confirm;
                            do
                            {
                                Console.Clear();
                                Console.Write("Name of habit record to be deleted (or enter 'e' to return to menu: ");
                                userInput = Console.ReadLine();
                                if (userInput == "" || userInput == null)
                                    continue;

                                else if (userInput.ToLower() == "e")
                                {
                                    exit = true;
                                    continue;
                                }

                                Console.Write($"Are you sure you want to delete '{userInput}' (Y/N): ");
                                confirm = Console.ReadLine();

                                if (confirm == "" || confirm == null)
                                    continue;

                                else if (confirm.ToLower() == "y")
                                {
                                    db.DeleteData(sqlite_conn, userInput.ToLower());
                                    exit = true;
                                }
                                else
                                    continue;
                                
                            }while (!exit);
                            exit = false;
                            break;
                        case 4: // Update Records - Update Habbit count
                            exit = false;
                            int countToBeAdded;
                            string habitName;
                            do
                            {
                                Console.Clear();
                                Console.Write("Name of habit record to be update (ore enter 'e' to return to menu: ");
                                userInput = Console.ReadLine();
                                
                                if (userInput == "" || userInput == null)
                                    continue;
                                
                                else if (userInput.ToLower() == "e")
                                {
                                    exit = true;
                                    continue;
                                }

                                habitName = userInput;
                                Console.Write($"\nHow much would you like to increase {userInput} Count by?: ");
                                userInput = Console.ReadLine();
                                if (!int.TryParse(userInput, out countToBeAdded))
                                {
                                    Console.WriteLine("Invalid Input. Please Try Again.");
                                    Console.WriteLine("Returning...");
                                    Thread.Sleep(1000);
                                    continue;
                                }
                                db.UpdateData(sqlite_conn, habitName.ToLower(), countToBeAdded);
                                exit = true;
                            } while (!exit);
                            exit = false;
                            break;
                        default:
                            Console.WriteLine($"Error: {optionSelected} is not a menu option.");
                            break;
                    }
                }
            } while (!exit);
        }
    }
}