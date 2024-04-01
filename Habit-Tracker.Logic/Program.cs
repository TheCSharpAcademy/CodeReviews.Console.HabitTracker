using System.IO;
using System.Data.SQLite;

namespace Mothnue.Program
{
    class Logic
    {
        public string CreateDatabase()
        {
            const string databaseName = "myDatabase.db";
            if (!File.Exists(databaseName))
            {
                SQLiteConnection.CreateFile(databaseName);
                return "Database Created!";
            }
            else
            {
                return "Database already exist!";
            }
        }
    }
}