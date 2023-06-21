interface IHabit
{
	bool Insert(string time, int value);
	bool Update(string time, int value);
	void Delete(string index);
	void Read();
	void DeleteAll();
	string GetMeasureUnit();
}