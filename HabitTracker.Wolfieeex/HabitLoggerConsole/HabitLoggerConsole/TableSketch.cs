/*Bool[] userOptions = new Bool[9];
List<Tuple<int, userDataCoded[]>> data = new List<Tuple<int,userDataCoded[]>>();

int tableVerticalSpace = 0;
for (int i = 0; i < 6; i++)
{ 
    if (userOptions[i] == true)
    {
        tableVerticalSpace++;
    }
}

// Create table:
//  Create left edge:
Console.Clear();

WriteTableWall(TableWalls.BLeft, TableWalls.BUp, 0, -1);
WriteTableWall(TableWalls.BLeft, Blank, 0, -1);
WriteTableWall(TableWalls.BLeft, TableWalls.BMiddle, 0, -1);

for (int i = 0; i < data.Count; i++)
{
    for (int j = 0; j < tableVerticalSpace; j++)
    {
        WriteTableWall(TableWalls.BLeft, Blank, 0, -1);
    }
    if (i + 1 == data.Count)
    {
        WriteTableWall(TableWalls.BLeft, TableWalls.BDown, 0, -1);
    }
    else
    {
        WriteTableWall(TableWalls.BLeft, TableWalls.BMiddle, 1, 0);
    }
}

//  Create left edge content (years) and table edges:
//      Top bit:
Console.SetCursorPosition(1, 0);
for (int i = 0; i < 7; i++)
{
    WriteTableWall(TableWalls.Blank, TableWalls.BTop, 1, 0);
}
WriteTableWall(TableWalls.Middle, TableWalls.BTop, 1, 0);
Console.SetCursorPosition(1, 2);
for (int i = 0; i < 7; i++)
{
    WriteTableWall(TableWalls.Blank, TableWalls.BMiddle, 1, 0);
}

//      Year bit:
for (int i = 0; i < data.Count; i++)
{
    int yearPosition = 0;
    yearPosition = tableVerticalSpace % 2 == 0 ? yearPosition = tableVerticalSpace / 2; : yearPosition = (tableVerticalSpace + 1) / 2;

    Console.SetCursorPosition(1, Console.CursorTop + yearPosition);
    Console.Write(data[i].Item1);

    //          Bottom edge:
    if (i + 1 == data.Count)
    {

    }
    else
    {

    }
}

//  Create table contents:

Console.SetCursorPosition(Console.GetCursorLeft, initialYPosition - 1);

int alterations = userOptions[7] ? 13 : 12;

//      Go through each Month (and a yearly summation if user selected this option to first insert the data.
//      If there are no records for this month, skip drawing it.)
for (int i = 0; i < alterations; i++)
{
    int longestRecord = 3;
    


    if (i == 12)
    {

    }
    else
    {

    }
}*/