/*
    * Table layouts:
    *   HABITDEFS -> table containing different types of habits that the user can log
    *   HABITLOGS -> table containing actual habits and their values
*/
using Microsoft.Data.Sqlite;

using (var connection = new SqliteConnection("Data Source=habittracker.db"))
{
    connection.Open();

    // var command = connection.CreateCommand();
    // command.CommandText =
    // @"
    //     SELECT *
    //     FROM user
    //     WHERE id = $id
    // ";
    // command.Parameters.AddWithValue("$id", 1);

    // using (var reader = command.ExecuteReader())
    // {
    //     while (reader.Read())
    //     {
    //         var name = reader.GetString(0);

    //         Console.WriteLine($"Hello, {name}!");
    //     }
    // }
}
