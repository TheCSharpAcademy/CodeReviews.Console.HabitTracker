using System.Text.RegularExpressions;

namespace HabitTracker
{
	internal class Menu
	{
		public static void GetUserInput()
		{
			bool closeApp = false;
			while (closeApp == false)
			{
				Console.Clear();
				Console.WriteLine("\nMAIN MENU");
				Console.WriteLine("\n\nWhat would you like to do?");
				Console.WriteLine("\n0. Close the app");
				Console.WriteLine("1. View all habits");
				Console.WriteLine("2. Create a new habit");
				Console.WriteLine("3. Delete a habit");
				Console.WriteLine("4. Update a habit");
				Console.WriteLine("-------------------------------\n");

				var option = Console.ReadLine();

				switch (option.Trim())
				{
					case "0":
						Console.WriteLine("Thank you!!  Have a nice day!!");
						closeApp = true;
						Environment.Exit(1);
						break;
					case "1":
						ViewAll();
						break;
					case "2":
						Insert();
						break;
					case "3":
						Delete();
						break;
					case "4":
						Update();
						break;
					default:
						Console.WriteLine("Invalid Option!!  Please try again!!");
						Thread.Sleep(2000);
						GetUserInput();
						break;
				}
			}
		}

		private static void ViewAll()
		{
			DisplayAllEntries("\n\nSorry, no entries to view!!  Add some data first!!");

			Console.WriteLine("\n\nPress any key to return to the main menu...");
			Console.ReadKey();
		}

		private static void DisplayAllEntries(string message)
		{
			List<DrinkingWater> entries = new();
			entries = Database.GetEntries();

			if (entries == null)
			{
				Console.WriteLine(message);
				Console.WriteLine("\nPress any key to continue...");
				Console.ReadKey();
				GetUserInput();
			}
			else
			{
				Console.Clear();
				Console.WriteLine("Here is all of your data:  ");
				Console.WriteLine("------------------------------------------\n");

				foreach (var entry in entries)
				{
					Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MM-yyyy")} - Quantity: {entry.Quantity}");
				}
				Console.WriteLine("------------------------------------------\n");
			}
		}

		private static void Insert()
		{
			Console.Clear();

			Console.Write("How many cups of water did you drink today (whole numbers only, 0 for main menu):  ");
			var quantity = Console.ReadLine();

			while (!Int32.TryParse(quantity, out _) || Convert.ToInt32(quantity) < 0)
			{
				Console.WriteLine("\n\nInvalid entry.  Please try again: \n");
				quantity = Console.ReadLine();
			}

			if (Int32.Parse(quantity) == 0)
				GetUserInput();

			Database.AddEntry(Int32.Parse(quantity));

			Console.WriteLine($"{quantity} cups of water successfully put in table. Press any key for main menu...");
			Console.ReadKey();
		}

		private static void Delete()
		{
			DisplayAllEntries("\n\nSorry, no entries to delete!!  Add some data first!!");

			List<DrinkingWater> entries = new List<DrinkingWater>();
			entries = Database.GetEntries();

			bool found = false;

			Console.Write("\n\nWhich entry # would you like to delete (0 to return to main menu)? ");
			var id = Console.ReadLine();

			while (!Int32.TryParse(id, out _) || Convert.ToInt32(id) < 0)
			{
				Console.WriteLine("\n\nInvalid number.  Please try again: \n");
				id = Console.ReadLine();
			}

			if (Int32.Parse(id) == 0)
				GetUserInput();

			foreach (var entry in entries)
			{
				if (entry.Id == Int32.Parse(id))
				{
					found = true;
					break;
				}
			}

			while (!found)
			{
				Console.WriteLine($"\n\nSorry, entry #{id} doesn't exist. Please try again (0 for main menu): \n");
				id = Console.ReadLine();

				while (!Regex.IsMatch(id, @"^\d+$"))
				{
					Console.WriteLine("Sorry, please enter numbers only: \n");
					id = Console.ReadLine();
				}

				if (Int32.Parse(id) == 0)
					GetUserInput();

				foreach (var entry in entries)
				{
					if (entry.Id == Int32.Parse(id))
					{
						found = true;
						break;
					}
				}
			}

			Database.DeleteEntry(Int32.Parse(id));

			Console.WriteLine($"\nEntry with Id {id} was successfully deleted!!");
			Console.WriteLine("\nPress any key for main menu...");
			Console.ReadKey();
		}

		static void Update()
		{
			DisplayAllEntries("\n\nSorry, no entries to update!!  Add some data first!!");

			List<DrinkingWater> entries = new List<DrinkingWater>();
			entries = Database.GetEntries();

			bool found = false;

			Console.Write("\n\nWhich entry # would you like to update (0 for main menu)? ");
			var id = Console.ReadLine();

			while (!Int32.TryParse(id, out _) || Convert.ToInt32(id) < 0)
			{
				Console.WriteLine("\n\nInvalid number.  Please try again: \n");
				id = Console.ReadLine();
			}

			if (Int32.Parse(id) == 0)
				GetUserInput();

			while (!Regex.IsMatch(id, @"^\d+$"))
			{
				Console.WriteLine("Sorry, please enter numbers only: \n");
				id = Console.ReadLine();
			}

			foreach (var entry in entries)
			{
				if (entry.Id == Int32.Parse(id))
				{
					found = true;
					break;
				}
			}

			while (!found)
			{
				Console.WriteLine($"\n\nSorry, entry #{id} doesn't exist. Please try again: \n");
				id = Console.ReadLine();

				while (!Regex.IsMatch(id, @"^\d+$"))
				{
					Console.WriteLine("Sorry, please enter numbers only: \n");
					id = Console.ReadLine();
				}

				if (Int32.Parse(id) == 0)
					GetUserInput();

				foreach (var entry in entries)
				{
					if (entry.Id == Int32.Parse(id))
					{
						found = true;
						break;
					}
				}
			}

			Console.Write("\n\nAnd what would you like to change the Quantity to? (0 for main menu) ");
			var quantity = Console.ReadLine();

			while (!Int32.TryParse(quantity, out _) || Convert.ToInt32(id) < 0)
			{
				Console.WriteLine("\n\nInvalid number.  Please try again: \n");
				quantity = Console.ReadLine();
			}

			if (Int32.Parse(quantity) == 0)
				GetUserInput();

			Database.UpdateEntry(Int32.Parse(quantity), Int32.Parse(id));

			Console.WriteLine($"\nEntry with Id {id} had it's quantity successfully changed to {quantity}!!");
			Console.WriteLine("\nPress any key for main menu...");
			Console.ReadKey();
		}
	}

}