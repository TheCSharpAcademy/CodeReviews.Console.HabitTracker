using Newtonsoft.Json;

namespace Drinks.StanimalTheMan.Models;

public class DrinksCollection
{
	[JsonProperty("drinks")]
	public List<Drink> DrinksList { get; set; }
}

public class Drink
{
	public string idDrink { get; set; }
	public string strDrink { get; set; }
}
