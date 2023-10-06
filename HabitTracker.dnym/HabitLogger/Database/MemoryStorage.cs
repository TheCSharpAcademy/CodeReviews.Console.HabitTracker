using HabitLogger.Models;

namespace HabitLogger.Database;

internal class MemoryStorage : IDatabase
{
    private readonly List<HabitRecord> _records = new();

    public void InsertRecord(HabitRecord record)
    {
        var r = new HabitRecord() { Id = _records.Count + 1, Date = record.Date, Quantity = record.Quantity };
        _records.Add(r);
        _records.Sort((a, b) => b.Date.CompareTo(a.Date));
    }

    public HabitRecord? GetRecord(int id)
    {
        var foundRecord = _records.Find(r => r.Id == id);
        if (foundRecord != null)
        {
            foundRecord = new() { Id = foundRecord.Id, Date = foundRecord.Date, Quantity = foundRecord.Quantity };
        }
        return foundRecord;
    }

    public List<HabitRecord> GetRecords(int limit = int.MaxValue, int skip = 0)
    {
        var output = new List<HabitRecord>();
        foreach (var record in _records.Skip(skip).Take(limit))
        {
            output.Add(new HabitRecord() { Id = record.Id, Date = record.Date, Quantity = record.Quantity });
        }
        return output;
    }

    public int GetRecordsCount()
    {
        return _records.Count;
    }

    public void UpdateRecord(HabitRecord record)
    {
        var foundRecord = _records.Find(r => r.Id == record.Id);
        if (foundRecord != null)
        {
            foundRecord.Date = record.Date;
            foundRecord.Quantity = record.Quantity;
            _records.Sort((a, b) => b.Date.CompareTo(a.Date));
        }
    }

    public void DeleteRecord(int id)
    {
        var foundRecord = _records.Find(r => r.Id == id);
        if (foundRecord != null)
        {
            _records.Remove(foundRecord);
        }
    }
}
