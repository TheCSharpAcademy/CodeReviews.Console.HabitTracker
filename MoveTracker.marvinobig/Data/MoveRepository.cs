using MoveTracker.Models;
using SQLite;


namespace MoveTracker.Data
{
    internal class MoveRepository
    {
        private readonly string _dbPath;
        private SQLiteConnection? _dbConn;

        public MoveRepository(string dbPath)
        {
            _dbPath = dbPath;
            Init(_dbPath);
        }

        private void Init(string dbPath)
        {
            try
            {
                string dbCommands = @"CREATE TABLE IF NOT EXISTS move(
                                        id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                        numOfMoves INTEGER NOT NULL, 
                                        timeRecorded TEXT DEFAULT CURRENT_DATE
                                    );";

                _dbConn = new SQLiteConnection(dbPath);
                _dbConn.CreateCommand(dbCommands).ExecuteNonQuery();
                _dbConn.Close();
            }
            catch (SQLiteException err)
            {
                Console.WriteLine(err.Message);
            }
        }

        internal void AddMove(int move)
        {

            try
            {
                string dbCommands = @$"INSERT INTO move (numOfMoves) VALUES ({move})";
                _dbConn = new SQLiteConnection(_dbPath);
                _dbConn.CreateCommand(dbCommands).ExecuteNonQuery();
                _dbConn.Close();
            }
            catch (SQLiteException err)
            {
                Console.WriteLine(err.Message);
            }
        }

        internal List<Move> GetAllMove() 
        {
            try
            {
                string dbCommands = @"SELECT * FROM move;";
                _dbConn = new SQLiteConnection(_dbPath);

                List<Move> listOfMoves = _dbConn.CreateCommand(dbCommands).ExecuteQuery<Move>();
                _dbConn.Close();

                return listOfMoves;
            }
            catch (SQLiteException err) 
            { 
                Console.WriteLine(err.Message);
            }

            return new List<Move>();
        }

        internal void UpdateMove(int id, int newValue)
        {
            try
            {
                string dbCommands = @$"UPDATE move SET numOfMoves={newValue} WHERE Id={id}";
                _dbConn = new SQLiteConnection(_dbPath);
                _dbConn.CreateCommand(dbCommands).ExecuteNonQuery();
                _dbConn.Close();

            }
            catch (SQLiteException err)
            {
                Console.WriteLine(err.Message);
            }
        }

        internal void DeleteMove(int id)
        {
            try
            {
                string dbCommands = $@"DELETE FROM move WHERE Id = {id}";
                _dbConn = new SQLiteConnection(_dbPath);
                _dbConn.CreateCommand(dbCommands).ExecuteNonQuery();
                _dbConn.Close();
            }
            catch (SQLiteException err)
            {
                Console.WriteLine(err.Message);
            }
            
        }
    }
}
