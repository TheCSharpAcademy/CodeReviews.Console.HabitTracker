using System.Globalization;

namespace MiroiuDev.HabitTracker;
internal class DrinkingWaterRepository
{
    private readonly Database _db;

    internal DrinkingWaterRepository(Database db)
    {
        _db = db;
    }

    internal List<DrinkingWater> GetAll()
    {
        List<DrinkingWater> drinkingWaters = new();

        _db.Select("SELECT * FROM drinking_water", (reader) => drinkingWaters.Add(new DrinkingWater
        {
            Id = reader.GetInt32(0),
            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
            Quantity = reader.GetInt32(2),
        }));

        return drinkingWaters;
    }

    internal int Update(string date, int quantity, string recordId)
    {
        int rowCount = _db.Execute($"UPDATE drinking_water SET Date = '{date}', Quantity = {quantity} WHERE Id = {recordId}");

        return rowCount;
    }

    internal int Delete(string recordId)
    {
        int rowCount = _db.Execute($"DELETE FROM drinking_water WHERE Id = {recordId}");

        return rowCount;
    }

    internal void Insert(string date, int quantity)
    {
        _db.Execute($"INSERT INTO drinking_water (Date, Quantity) VALUES ('{date}', {quantity})");
    }

    internal bool Exists(string recordId)
    {
        int exists = _db.ExecuteScalar($"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})");

        return exists != 0;
    }

    internal int TotalWaterDrank()
    {
        int total = _db.ExecuteScalar($"SELECT SUM(Quantity) FROM drinking_water");

        return total;
    }

    internal int TotalWaterDrankInYear(int year)
    {
        int total = _db.ExecuteScalar($"SELECT SUM(Quantity) FROM drinking_water WHERE SUBSTR(Date, 7, 4) = '{year}'");

        return total;
    }
}


