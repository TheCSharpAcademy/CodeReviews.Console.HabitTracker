using Microsoft.Data.Sqlite;
namespace DataService
{
	public class DataServices
	{
		public DataServices() 
		{
		
		}


		/// <summary>
		/// General methode used to recieve user input
		/// </summary>
		/// <param name="messageToUser">Message to write to console</param>
		/// <returns>USer input as a string value</returns>
		public string UserUnput(string messageToUser) 
		{
			Console.WriteLine(messageToUser);
			return Console.ReadLine(); 
		}


		/// <summary>
		/// Creates an instance of the HabbitLogger Database if not present
		/// </summary>
		public void CreateDB() 


		{
			var connectionString = @"Data Source = HabbitLogger.db";

			try
			{
				using (var connection = new SqliteConnection(connectionString))
				{
					connection.Open();
					var tableCmd = connection.CreateCommand();
					tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS habbit_logger (
											Id INTEGER PRIMARY KEY AUTOINCREMENT,
											Date Text,
											Quantity Integer						
											)";
				}
			}
			catch (Exception ex) 
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void InsertRecord() 
		{
		
		
		}

	}
}
