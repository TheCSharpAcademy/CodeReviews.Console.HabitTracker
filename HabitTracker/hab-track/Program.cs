namespace hab_track
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new DataAccess();
            db.CreateTables();
            Helpers.GetUserMenu(db);
        }
    }
}
