DatabaseManager.InitializeDatabase();
DatabaseManager.CreateTable();

ConsoleMessages.DisplayAppInformation();

int userInput;

while (true)
    {
    ConsoleMessages.ShowMainMenu();
    userInput = ConsoleMessages.GetUserInput();
    
    switch (userInput)
    {
            case 1:
                Console.Clear();
                int quantity;
                string input;
                bool isValid;
                string newHabit;

                do
                    {
                    Console.Write("Enter the name of the new habit: ");
                    newHabit = Console.ReadLine().ToUpper();
                    } while (string.IsNullOrEmpty(newHabit));

                do
                    {
                    Console.Write("Enter the quantity: ");
                    input = Console.ReadLine();
                    isValid = int.TryParse(input, out quantity);

                    if (!isValid || quantity <= 0)
                        {
                        Console.WriteLine("Invalid input. Quantity must be an integer greater than 0.");
                        }
                    } while (!isValid || quantity <= 0);

                
                Console.Write("Enter the unit: ");
                string unit = Console.ReadLine();
                
                HabitManager.InsertHabit(newHabit, quantity, unit);
                Console.WriteLine("New habit created.");
                Console.WriteLine();
                break;
            case 2:
                Console.Clear();
                List<string>? habits = HabitManager.GetAllHabits();
                Console.WriteLine();
                Console.WriteLine("List of habits:");
                Console.WriteLine();
                foreach (string habit in habits)
                    {
                    Console.WriteLine(habit);
                    }
                Console.WriteLine();
                break;
            case 3:
                Console.Clear();
                Console.Write("Enter the name of the habit to update: ");
                string oldHabit = Console.ReadLine().ToUpper();
                if (HabitManager.DoesHabitExist(oldHabit))
                    {
                    Console.Write("Enter the new name: ");
                    string updatedHabit = Console.ReadLine().ToUpper();
                    HabitManager.UpdateHabit(oldHabit, updatedHabit);
                    Console.WriteLine("Habit updated.");
                    }
                else
                    {
                    Console.WriteLine("Habit does not exist.");
                    }
                Console.WriteLine();
                break;
            case 4:
                Console.Clear();
                Console.Write("Enter the name of the habit to delete: ");
                string habitToDelete = Console.ReadLine().ToUpper();
                if (HabitManager.DoesHabitExist(habitToDelete))
                    {
                    HabitManager.DeleteHabit(habitToDelete);
                    Console.WriteLine("Habit deleted.");
                    }
                else
                    {
                    Console.WriteLine("Habit does not exist.");
                    }
                Console.WriteLine();
                break;
            case 5:
                Console.Clear();
                Console.WriteLine("Close");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice. Try again.");
                break;
    }
    }