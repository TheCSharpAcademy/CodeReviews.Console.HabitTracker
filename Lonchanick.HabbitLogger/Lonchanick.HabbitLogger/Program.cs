




using Lonchanick.HabbitLogger;

string aux;
int op;

do
{
    Console.Clear();
    Console.WriteLine("\tHabbit Logger");
    Console.WriteLine($"1) {Options.CreateNewRecord}");
    Console.WriteLine($"2) {Options.ReadAllRecords}");
    Console.WriteLine($"3) {Options.UpdateRecord}");
    Console.WriteLine($"4) {Options.DeleteRecord}");
    Console.Write($"0) {Options.Exit}\t");

    aux = Console.ReadLine();

    if (!int.TryParse(aux, out op))
        op = -1;

    switch (op)
    {
        case 1:
            Console.WriteLine("\tCreateNewRecord");
            CaseSolutions.AddNewRecord();
            break;
        case 2:
            Console.WriteLine("\tReadAllRecords");
            CaseSolutions.ReadAllRecords();
            break;
        case 3:
            Console.WriteLine("\tUpdateRecord");
            CaseSolutions.Update();
            break;
        case 4:
            Console.WriteLine("\tDeleteRecord");
            CaseSolutions.Delete();
            break;

        case 0:
            return;

        default:
            Console.WriteLine("Invalid option!");
            Console.ReadLine();
            break;
    }


} while (true);


int GetValidInteger(string param)
{
    string aux = "";
    int result;

    while (!int.TryParse(aux, out result))
    {
        Console.Write($"Type {param}: ");
        aux = Console.ReadLine();
    }

    return result;
}

enum Options
{
    CreateNewRecord,
    ReadAllRecords,
    UpdateRecord,
    DeleteRecord,
    Exit
}

