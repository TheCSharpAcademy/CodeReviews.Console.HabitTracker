namespace iamryanmacdonald.Console.HabitTracker.Controllers;

internal interface IBaseController
{
    void DeleteItem();
    void InsertItem();
    void UpdateItem();
    void ViewItems();
}