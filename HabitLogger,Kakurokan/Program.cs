using Dapper;
using Habit_Logger.Kakurokan;
using HabitLogger_Kakurokan;
using Spectre.Console;
using System.Data.SQLite;
using System.Globalization;

namespace Habit_Logger_Kakurokan
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                DataAcces dataAcces = new();
                AnsiConsole.Clear();

                var select_menu = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(@"Welcome to your [blue]Habit logger![/] 
What would you like to do?").MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                                                                                            .AddChoices(new[] { "Insert new record", "View all records", "Update record", "Delete record", "Exit Habit Logger" }));
                AnsiConsole.Clear();
                switch (select_menu)
                {
                    case "Insert new record":
                        Insert(CreateNewHabit(), dataAcces);
                        break;
                    case "View all records":
                        ViewAll(dataAcces);
                        break;
                    case "Update record":
                        Update(dataAcces);
                        break;
                    case "Delete record":
                        Delete(dataAcces);
                        break;
                    case "Exit Habit Logger":
                        Environment.Exit(0);
                        break;
                };
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup("Sorry a [red]error[/] ocurred. Here is the information: " + ex.Message);
                Thread.Sleep(3000);
                Main();
            }
        }

        public static void Insert(Habit habit, DataAcces data)
        {
            using (var conn = new SQLiteConnection(data.MyConnection))
            {
                int id = conn.QuerySingle<int>("INSERT INTO Habits (Name, Date, Quantity, Unit) VALUES (@Name, @Date, @Quantity, @Unit) returning Id;", new { Name = habit.Name, Date = habit.Date, Quantity = habit.Quantity, Unit = habit.Unit });
                habit.Id = id;
            }
            Thread.Sleep(2000);
            Main();
        }

        public static void ViewAll(DataAcces data)
        {
            using (var conn = new SQLiteConnection(data.MyConnection))
            {
                var output = conn.Query<Habit>("SELECT * FROM Habits", new DynamicParameters());
                output = output.ToList();

                var table = new Table();
                table.AddColumns(new[] { "Id", "Name", "Date", "Quantity" });
                foreach (var item in output)
                {
                    table.AddRow(new[] { item.Id.ToString(), item.Name, item.Date.ToString("dd-MM-yyyy"), item.Quantity.ToString() + item.Unit });
                }
                table.Border(TableBorder.Square);
                AnsiConsole.Write(table);
                AnsiConsole.WriteLine();

                var select_menu = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "Insert new", "Return to menu" }));
                switch (select_menu)
                {
                    case "Insert new":
                        Insert(CreateNewHabit(), data);
                        break;
                    case "Return to menu":
                        Main();
                        break;
                };
            }
        }

        public static void Update(DataAcces data)
        {
            using (var conn = new SQLiteConnection(data.MyConnection))
            {
                var output = conn.Query<Habit>("SELECT * FROM Habits", new DynamicParameters());
                output = output.ToList();

                var options_to_update = new SelectionPrompt<string>().Title("What Habit you want to [blue]update[/]?")
                .MoreChoicesText("[grey](Move up and down to reveal more habits)[/]");
                foreach (Habit item in output)
                {
                    options_to_update.AddChoice(item.ToString());
                }

                string toupdate = AnsiConsole.Prompt(options_to_update);

                AnsiConsole.MarkupLine("You want to [red]Update:[/]");

                AnsiConsole.WriteLine(toupdate);

                var select_menu = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "[blue]Update quantity[/]", "Return to menu" }));
                if (select_menu == "Return to menu") Main();

                Habit clean_to_update = new();

                foreach (Habit item in output)
                {
                    if (item.ToString() == toupdate)
                    {
                        clean_to_update = item;
                    }
                }
                AnsiConsole.Clear();

                int new_quantity = AnsiConsole.Ask<int>($"You current quantity is [green]{clean_to_update.Quantity}[/] what´s the [blue]new value[/]? ");

                conn.Execute(@$"UPDATE Habits SET Quantity = {new_quantity} WHERE Id = {clean_to_update.Id}");

                AnsiConsole.Markup("You habit was [green]successfully Updated[/]");
                Thread.Sleep(2000);
                Main();

            }
        }

        public static void Delete(DataAcces data)
        {
            List<Habit> todelete = new();
            using (var conn = new SQLiteConnection(data.MyConnection))
            {
                var output = conn.Query<Habit>("SELECT * FROM Habits", new DynamicParameters());
                output = output.ToList();

                var deletes = new MultiSelectionPrompt<string>().Title("What Habit you want to [red]delete[/]?").NotRequired()
                .MoreChoicesText("[grey](Move up and down to reveal more habits)[/]").InstructionsText(
            "[grey](Press [blue]<space>[/] to toggle a Habit, " +
            "[green]<enter>[/] to accept)[/]");

                foreach (Habit item in output)
                {
                    deletes.AddChoice(item.ToString());
                }

                List<string> deleted = AnsiConsole.Prompt(deletes);

                AnsiConsole.MarkupLine("You want to [red]delete:[/]");
                foreach (var item in deleted)
                {
                    AnsiConsole.WriteLine(item);
                    AnsiConsole.WriteLine();
                }

                var select_menu = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "[red]Delete all[/]", "Return to menu" }));
                if (select_menu == "Return to menu") Main();

                foreach (Habit item in output)
                {
                    foreach (string item2 in deleted)
                    {
                        if (item.ToString() == item2)
                        {
                            todelete.Add(item);
                        }
                    }
                }

                foreach (Habit item in todelete)
                {
                    conn.Execute("DELETE FROM Habits WHERE Id = @Id;", new { Id = item.Id });
                }

                AnsiConsole.Markup("You habit was [green]successfully deleted[/]");
                Thread.Sleep(2000);
                Main();
            }


        }
        public static Habit CreateNewHabit()
        {
            string name = AnsiConsole.Ask<string>("What`s the name of the [green]habit?[/]");
            string date = AnsiConsole.Ask<string>("What`s the date?(Format: dd-MM-yyyy)");
            DateTime clean_date;
            while (!DateTime.TryParseExact(date, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out clean_date))
            {
                date = AnsiConsole.Ask<String>("[red][[Invalid date]][/] Please, try again (with the format: dd-MM-yyyy):");
            }
            int quantity = AnsiConsole.Ask<int>($"How many [green]{name}[/] you did at [green]{date}[/]?");
            string unit = AnsiConsole.Ask<string>("What`s the unit of [green]measurement?[/]");

            Habit habit = new(name, clean_date, quantity, unit);

            AnsiConsole.Markup("You habit was [green]successfully added[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine(habit.ToStringWithoutId());
            Thread.Sleep(2000);
            return habit;
        }
    }
}
