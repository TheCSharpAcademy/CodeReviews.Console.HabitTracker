using HabitTracker.UgniusFalze;


void tracker()
{
    bool exitTracker = false;
    Display.DisplayIntroMessage();
    SQLLiteOperations sQLLiteOperations = new SQLLiteOperations();

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
                    handleInput(sQLLiteOperations);
                    break;
                case 3:
                    handleDelete(sQLLiteOperations);
                    break;
                case 4:
                    handleUpdate(sQLLiteOperations);
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

void handleInput(SQLLiteOperations sQLLiteOperations)
{
    Display.DisplaySeperator();
    try
    {
        DateOnly date = (DateOnly)handleDateInput();
        long quantity = (long)handleQuantityInput();
        sQLLiteOperations.InsertHabbit(quantity, date);
    }
    catch (Exception e)
    {
        if (e is ArgumentNullException)
        {
            return;
        }
        Display.SQLLiteException();
        return;
    }
}

void handleDelete(SQLLiteOperations sQLLiteOperations)
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

void handleUpdate(SQLLiteOperations sQLLiteOperations)
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
                        var quantityInput = handleQuantityInput();
                        if (quantityInput == null)
                        {
                            return;
                        }
                        long quantity = (long)quantityInput;
                        sQLLiteOperations.UpdateQuantity(id, quantity);
                        return;
                    case 2:
                        var dateInput = handleDateInput();
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
                    Display.SQLLiteException();
                    return;
                }
            }

        } while (!updateLoop);
    }
}

DateOnly? handleDateInput()
{
    Display.DisplayDateInput();
    do
    {
        try
        {
            string input = Console.ReadLine();
            DateOnly dateOnly = DateOnly.ParseExact(input, SQLLiteOperations.GetDateFormat());
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
long? handleQuantityInput()
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

tracker();