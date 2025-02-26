namespace HabitTracker
{
    internal class CliHandler
    {
        private readonly DatabaseManager _db;

        internal CliHandler(DatabaseManager db)
        {
            _db = db;
        }
    }
}
