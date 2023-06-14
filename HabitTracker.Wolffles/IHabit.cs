interface IHabit
{
	bool Insert(int value, string time);
	void Update(string index, int value);
	void Delete(string index);
	void Read();
	void DeleteAll();
	string getMeasureUnit();
}