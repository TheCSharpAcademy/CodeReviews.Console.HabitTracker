using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    class User
    {
        readonly DatabaseHandler db;

        public User()
        {
            this.db = new DatabaseHandler();
        }

        public void AppMenu()
        {
            bool isDone = false;
            bool validInput = false;

            do
            {
                Console.Clear();
                Console.WriteLine("Habit Tracker");
                Console.WriteLine("Enter the process you want to do:");
                Console.WriteLine("0. Exit application.");
                Console.WriteLine("1. Add a new habit.");
                Console.WriteLine("2. View habits.");
                Console.WriteLine("3. Update a habit.");
                Console.WriteLine("4. Delete a habit.");
                Console.WriteLine();


                string? userInput = Console.ReadLine();

                if (userInput != null)
                {
                    validInput = int.TryParse(userInput, out int choice);

                    if (validInput)
                    {
                        switch (choice)
                        {
                            case 0:
                                isDone = true;
                                break;
                            case 1:
                                AddHabit();
                                break;
                            case 2:
                                ViewHabits();
                                break;
                            case 3:
                                UpdateHabit();
                                break;
                            case 4:
                                DeleteHabit();
                                break;
                            default:
                                break;
                        }
                    }

                }
            } while (isDone == false);

        }

        void AddHabit()
        {
            bool alreadyExists = true;

            Console.Clear();
            Console.WriteLine("Add a habit.");

            do
            {
                string? userInput = Console.ReadLine();

                if (userInput != null)
                {
                    string habitName = userInput.ToLower().Replace(" ", "_");

                    try
                    {
                        DateTime date = DateTime.Now.Date;
                        string dateString = date.ToString("yyyy-MM-dd");

                        db.Insert(name: habitName, dateToday: dateString);
                        Console.WriteLine("Habit added!");
                        alreadyExists = false;
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            } while (alreadyExists);

            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }

        void ViewHabits()
        {
            Console.Clear();
            Console.WriteLine("View Habits: ");

            db.View();

            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }

        void UpdateHabit()
        {
            bool validInput = false;

            Console.Clear();
            Console.WriteLine("Update a habit. Please enter the id of the habit you want to update: ");
            db.View();
            Console.WriteLine();

            string? userInput = Console.ReadLine();

            if (userInput != null)
            {
                validInput = int.TryParse(userInput, out int idNumber);

                if (validInput)
                {
                    db.Update(idNumber);
                    Console.WriteLine("\nHabit updated! This is the updated list:");
                    db.View();
                }
            }

            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }

        void DeleteHabit()
        {
            bool validInput = false;

            Console.Clear();
            Console.WriteLine("Delete a habit. Please enter the id of the habit you want to delete: ");
            db.View();
            Console.WriteLine();

            string? userInput = Console.ReadLine();

            if (userInput != null)
            {
                validInput = int.TryParse(userInput, out int idNumber);

                if (validInput)
                {
                    db.Delete(idNumber);
                    Console.WriteLine("\nHabit deleted! This is the updated list:");
                    db.View();
                }
            }

            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }
    }
}