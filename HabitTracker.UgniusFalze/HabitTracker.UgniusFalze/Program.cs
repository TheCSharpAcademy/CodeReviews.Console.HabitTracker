using HabitTracker.UgniusFalze;
void Tracker()
{
    bool exitTracker = false;
    Display.DisplayIntroMessage();
    SqliteOperations sQLLiteOperations = new SqliteOperations();

    do
    {
        Display.DisplaySeperator();
        Display.DisplayMenu();
        string input = Console.ReadLine();
        try
        {
            int option = Int32.Parse(input);
            switch (option)
            {
                case 1:
                    List<Habbit> habbits = sQLLiteOperations.GetHabbitHistory();
                    Display.DisplayAllHabbits(habbits);
                    break;
                case 2:
                    HandleInput(sQLLiteOperations);
                    break;
                case 3:
                    HandleDelete(sQLLiteOperations);
                    break;
                case 4:
                    HandleUpdate(sQLLiteOperations);
                    break;
                case 0: exitTracker = true; break;
                default: Display.DisplayIncorrectMenuOption(); break;
            }
        }
        catch(Exception e)
        {
            if (e is ArgumentNullException || e is FormatException || e is OverflowException)
            {
                Display.DisplayIncorrectNumber();
            }
        }

    } while (!exitTracker);
}

void HandleInput(SqliteOperations sQLLiteOperations)
{
    Display.DisplaySeperator();
    try
    {
        DateOnly date = (DateOnly)HandleDateInput();
        long quantity = (long)HandleQuantityInput();
        sQLLiteOperations.InsertHabbit(quantity, date);
    }
    catch (Exception e)
    {
        if (e is ArgumentNullException)
        {
            return;
        }
        Display.SqliteException();
        return;
    }
}

void HandleDelete(SqliteOperations sQLLiteOperations)
{
    Display.DisplayDeleteHabbit();
    Display.DisplaySeperator();
    Display.DisplayAllHabbits(sQLLiteOperations.GetHabbitHistory());
    bool exitDelete = false;

    do
    {
        try
        {
            string input = Console.ReadLine();
            long id = Int64.Parse(input);
            if (id <= 0)
            {
                throw new FormatException();
            }
            sQLLiteOperations.DeleteHabbit(id);
            exitDelete = true;
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException || e is FormatException || e is OverflowException)
            {
                Display.DisplayIncorrectNumber();
            }
            else
            {
                Display.IncorrectId();
            }
        }


    } while (!exitDelete);
}

void HandleUpdate(SqliteOperations sQLLiteOperations)
{
    Display.DisplaySeperator();
    Display.DisplayWhichRecordToUpdate();
    Display.DisplayAllHabbits(sQLLiteOperations.GetHabbitHistory());
    long id = -1;
    bool exitHabbitChoice = false;

    do
    {
        try
        {
            string input = Console.ReadLine();
            id = Int64.Parse(input);
            if (id <= 0)
            {
                throw new FormatException();
            }
            sQLLiteOperations.CheckIfRecordWithIdExsists(id);
            exitHabbitChoice = true;
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException || e is FormatException || e is OverflowException)
            {
                Display.DisplayIncorrectNumber();
            }
            else
            {
                Display.IncorrectId();
            }
        }


    } while (!exitHabbitChoice);

    if (id > 0)
    {
        Display.DisplaySeperator();
        Display.DisplayUpdateMenu();

        bool updateLoop = false;

        do
        {
            try
            {
                string menuInput = Console.ReadLine();
                int menuChoice = Int32.Parse(menuInput);
                switch (menuChoice)
                {
                    case 1:
                        var quantityInput = HandleQuantityInput();
                        if (quantityInput == null)
                        {
                            return;
                        }
                        long quantity = (long)quantityInput;
                        sQLLiteOperations.UpdateQuantity(id, quantity);
                        return;
                    case 2:
                        var dateInput = HandleDateInput();
                        if (dateInput == null)
                        {
                            return;
                        }
                        DateOnly dateOnly = (DateOnly)dateInput;
                        sQLLiteOperations.UpdateDate(id, dateOnly);
                        return;
                    case 0:
                        return;
                }
            }catch(Exception e)
            {
                if (e is ArgumentNullException || e is FormatException || e is OverflowException)
                {
                    Display.DisplayIncorrectNumber();
                }
                else
                {
                    Display.SqliteException();
                    return;
                }
            }

        } while (!updateLoop);
    }
}

DateOnly? HandleDateInput()
{
    Display.DisplayDateInput();
    do
    {
        try
        {
            string input = Console.ReadLine();
            DateOnly dateOnly = DateOnly.ParseExact(input, SqliteOperations.GetDateFormat());
            return dateOnly;
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException || e is FormatException)
            {
                Display.DisplayIncorrectFormat();
            }
            else
            {
                Display.IOException();
                return null;
            }
        }

    } while (true);
}
long? HandleQuantityInput()
{

    Display.DisplayQuantityInput();
    do
    {
        try
        {
            string input = Console.ReadLine();
            long quantity = Int64.Parse(input);
            if (quantity < 0)
            {
                throw new FormatException();
            }
            return quantity;
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException || e is FormatException || e is OverflowException)
            {
                Display.DisplayIncorrectNumber();
            }
            else
            {
                Display.IOException();
                return null;
            }
        }

    } while (true);
}

Tracker();