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
    s,
    min,
    hr,
}
internal class MeasurementUnits
{
    internal static Dictionary<MeasurementType, string> MeasurementValidation { get; private set; } = new Dictionary<MeasurementType, string>
    {
        { MeasurementType.blank, "aaa" },
        { MeasurementType.l, "aaa" },
        { MeasurementType.ml, "aaa" },
        { MeasurementType.cm, "aaa" },
        { MeasurementType.m, "aaa" },
        { MeasurementType.km, "aaa" },
        { MeasurementType.inch, "aaa" },
        { MeasurementType.ft, "aaa" },
        { MeasurementType.g, "aaa" },
        { MeasurementType.kg, "aaa" },
        { MeasurementType.lbs, "aaa" },
        { MeasurementType.stones, "aaa" },
        { MeasurementType.s, "aaa" },
        { MeasurementType.min, "aaa" },
        { MeasurementType.hr, "aaa" },
    };

    internal static Dictionary<MeasurementType, string> MeasurementFullName { get; private set; } = new Dictionary<MeasurementType, string>
    {
        { MeasurementType.blank, "leave blank" },
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
        { MeasurementType.s, "seconds" },
        { MeasurementType.min, "minutes" },
        { MeasurementType.hr, "hours" },
    };
}
