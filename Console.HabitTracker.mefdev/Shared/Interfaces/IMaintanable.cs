namespace HabitLogger.Shared
{
    public interface IMaintanable<T>
    {
        void Create(T obj);
        T Retrieve(int key);
        void Update(T obj);
        void Delete(int key);
    }
}

