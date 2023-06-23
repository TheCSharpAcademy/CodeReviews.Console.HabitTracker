using System.Data;

namespace HabitTracker.edvaudin;

public static class SqlDbTypeConverter
{
    private static Dictionary<Type, SqlDbType> typeMap;

    // Create and populate the dictionary in the static constructor
    static SqlDbTypeConverter()
    {
        typeMap = new Dictionary<Type, SqlDbType>();

        typeMap[typeof(string)] = SqlDbType.VarChar;
        typeMap[typeof(char[])] = SqlDbType.VarChar;
        typeMap[typeof(byte)] = SqlDbType.Int;
        typeMap[typeof(short)] = SqlDbType.Int;
        typeMap[typeof(int)] = SqlDbType.Int;
        typeMap[typeof(long)] = SqlDbType.Int;
        typeMap[typeof(bool)] = SqlDbType.Bit;
        typeMap[typeof(DateTime)] = SqlDbType.DateTime;
        typeMap[typeof(DateOnly)] = SqlDbType.Date;
        typeMap[typeof(double)] = SqlDbType.Float;
        typeMap[typeof(TimeSpan)] = SqlDbType.Time;
    }

    // Non-generic argument-based method
    public static SqlDbType GetDbType(Type giveType)
    {
        // Allow nullable types to be handled
        giveType = Nullable.GetUnderlyingType(giveType) ?? giveType;

        if (typeMap.ContainsKey(giveType))
        {
            return typeMap[giveType];
        }

        throw new ArgumentException($"{giveType.FullName} is not a supported .NET class");
    }

    public static SqlDbType GetDbType<T>()
    {
        return GetDbType(typeof(T));
    }
}
