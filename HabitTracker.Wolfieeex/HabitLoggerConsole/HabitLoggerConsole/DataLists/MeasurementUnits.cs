namespace HabitLoggerConsole.Models;

public enum MeasurementType
{
    blank,
    l,
    ml,
    cm,
    m,
    km,
    inch,
    ft,
    g,
    kg,
    lbs,
    stones,
}

public enum MeasurementDivision
{
    None,
    Liquid,
    Length,
    Weight,
    Time
}
internal class MeasurementUnits
{
    internal static Dictionary<MeasurementType, MeasurementDivision> MeasurementDivisions { get; private set; } = new Dictionary<MeasurementType, MeasurementDivision>()
    {
        { MeasurementType.blank, MeasurementDivision.None },
        { MeasurementType.l, MeasurementDivision.Liquid },
        { MeasurementType.ml, MeasurementDivision.Liquid },
        { MeasurementType.cm, MeasurementDivision.Length },
        { MeasurementType.m, MeasurementDivision.Length },
        { MeasurementType.km, MeasurementDivision.Length },
        { MeasurementType.inch, MeasurementDivision.Length },
        { MeasurementType.ft, MeasurementDivision.Length },
        { MeasurementType.g, MeasurementDivision.Weight },
        { MeasurementType.kg, MeasurementDivision.Weight },
        { MeasurementType.lbs, MeasurementDivision.Weight },
        { MeasurementType.stones, MeasurementDivision.Weight },
    }; 

    internal static Dictionary<MeasurementType, string> MeasurementFullName { get; private set; } = new Dictionary<MeasurementType, string>
    {
        { MeasurementType.blank, "(blank)" },
        { MeasurementType.l, "liters" },
        { MeasurementType.ml, "mililiters" },
        { MeasurementType.cm, "centimeters" },
        { MeasurementType.m, "meters" },
        { MeasurementType.km, "kilometers" },
        { MeasurementType.inch, "inches" },
        { MeasurementType.ft, "feet" },
        { MeasurementType.g, "grams" },
        { MeasurementType.kg, "kilograms" },
        { MeasurementType.lbs, "pounds" },
        { MeasurementType.stones, "stones" },
    };

    internal static MeasurementType[] DisplayMeasurements()
    {
        var sortedMeasurementList = from entry in MeasurementUnits.MeasurementDivisions orderby entry.Value ascending select entry;
        var sortedDivisionDictionary = sortedMeasurementList.ToDictionary(pair => pair.Key, pair => pair.Value);
        var arrayOfKeys = sortedDivisionDictionary.Keys;

        MeasurementDivision? previousDivision = null;
        int listCounter = 0;
        foreach (MeasurementType key in arrayOfKeys)
        {
            listCounter++;

            if (previousDivision == null || previousDivision != MeasurementUnits.MeasurementDivisions[key])
            {
                Console.WriteLine($"\n{MeasurementUnits.MeasurementDivisions[key]}:");
                previousDivision = MeasurementUnits.MeasurementDivisions[key];
            }

            string measurementName = MeasurementUnits.MeasurementFullName[key];
            measurementName = measurementName[0].ToString().ToUpper() + measurementName.Substring(1);


            Console.WriteLine($"{listCounter} - {measurementName}");
        }

        return arrayOfKeys.ToArray();
    }
}
