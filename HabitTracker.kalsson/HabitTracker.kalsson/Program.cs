DatabaseManager.InitializeDatabase();
DatabaseManager.CreateTable();

ConsoleMessages.DisplayAppInformation();

int userInput;

while (true)
    {
    ConsoleMessages.ShowMainMenu();
    userInput = ConsoleMessages.GetUserInput();

    List<string>? habits;

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
                habits = HabitManager.GetAllHabits();
                Console.WriteLine();
                Console.WriteLine("List of habits:");
                Console.WriteLine();

                // Table headers
                Console.WriteLine(String.Format("{0,-5} {1,-20} {2,-10} {3,-10}", "No.", "Habit", "Quantity", "Unit"));
                Console.WriteLine("-----------------------------------------------------");

                if (habits != null)
                    {
                    for (int i = 0; i < habits.Count; i++)
                        {
                        string[] parts = habits[i].Split(" - ");
                        if (parts.Length == 2) // Your habit string seems to have only one delimiter ' - '
                            {
                            string habitName = parts[0];
                            string habitDetail = parts[1]; // This should be "QUANTITY UNIT"

                            string[] details = habitDetail.Split(" "); // Assume space between quantity and unit
                            if (details.Length == 2)
                                {
                                string habitQuantity = details[0];
                                string habitUnit = details[1];

                                // Display habit in formatted manner
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(String.Format("{0,-5} {1,-20} {2,-10} {3,-10}", i + 1, habitName, habitQuantity, habitUnit));
                                Console.ResetColor();
                                }
                            else
                                {
                                // Log malformed data for debugging
                                Console.WriteLine($"Malformed habit detail for item {i + 1}. Detail: {habitDetail}");
                                }
                            }
                        else
                            {
                            // Log malformed data for debugging
                            Console.WriteLine($"Malformed habit data for item {i + 1}. Raw data: {habits[i]}");
                            }
                        }
                    }
                Console.WriteLine();
                break;
            case 3:
                Console.Clear();
                habits = HabitManager.GetAllHabits();
                Console.WriteLine();
                Console.WriteLine("List of habits:");
                Console.WriteLine();
                
                for (int i = 0; i < habits.Count; i++)
                    {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{i + 1}. {habits[i]}");
                    Console.ResetColor();
                    }
                
                Console.WriteLine();
                
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
                habits = HabitManager.GetAllHabits();
                Console.WriteLine();
                Console.WriteLine("List of habits:");
                Console.WriteLine();
                
                for (int i = 0; i < habits.Count; i++)
                    {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{i + 1}. {habits[i]}");
                    Console.ResetColor();
                    }
                
                Console.WriteLine();
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