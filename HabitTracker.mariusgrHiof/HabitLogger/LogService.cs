namespace HabitLogger
{
    public class LogService
    {
        private readonly DbManager _dbManager;

        public LogService(DbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void SetupDatabase(string dbName)
        {
            _dbManager.CreateDb(dbName);
        }

        public CSharpLog? AddLog(CSharpLog newLog)
        {
            if (newLog != null && newLog.Hours > 0)
            {
                _dbManager.Add(newLog.Hours);
                Console.WriteLine("Log has been created!");
            }
            else
            {
                return null;
            }

            return newLog;
        }

        public CSharpLog Update(int id, CSharpLog updateLog)
        {
            if (updateLog != null && updateLog.Hours > 0)
            {
                _dbManager.Update(id, updateLog.Hours, updateLog.DateUpdated);
                Console.WriteLine("Log has been updated!");
            }
            else
            {
                return null;
            }

            return updateLog;
        }

        public CSharpLog GetLog(int id)
        {
            var log = _dbManager.Get(id);
            if (log == null) return null;

            return log;
        }

        public List<CSharpLog> GetAll()
        {
            List<CSharpLog> logs = _dbManager.GetAll();

            return logs;
        }

        public CSharpLog? DeleteLog(int id)
        {
            var log = _dbManager.Get(id);
            if (log is null) return null;

            int result = _dbManager.Delete(log.Id);
            if (result == 0)
            {
                Console.WriteLine("Failed to delete record.");
                return null;
            }
            Console.WriteLine("Log has been deleted!");

            return log;
        }
    }
}
