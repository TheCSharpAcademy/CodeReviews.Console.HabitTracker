namespace HabitLogger;

public static class ValidatorFactory
{
    public static IValidator<T> GetValidator<T>()
    {
        if(typeof(T) == typeof(int))
        {
            return (IValidator<T>)new IntValidator();
        }

        if(typeof(T).IsEnum)
        {
            return (IValidator<T>)Activator.CreateInstance(typeof(EnumValidator<>).MakeGenericType(typeof(T)));
        }

        if(typeof (T) == typeof(string))
        {
            return (IValidator<T>)new StringValidator();
        }

        if(typeof(T) == typeof(DateTime))
        {
            return (IValidator<T>)new DateTimeValidator();
        }

        throw new NotImplementedException($"There's no validator for {typeof(T)}");
    }
}
