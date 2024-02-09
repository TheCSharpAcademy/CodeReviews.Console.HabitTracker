namespace Drinks.StanimalTheMan;

internal class UserInput
{
	DrinksService drinksService = new();

	internal void GetCategoriesInput()
	{
		drinksService.GetCategories();
	}
}
