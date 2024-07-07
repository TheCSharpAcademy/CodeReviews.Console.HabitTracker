/* 
REQUIREMENTS:
- Register one habit
- habit is trackable by quantity, not time
- post/get from a db
- on start: creates sqlite db if one is not present
- should create table in db where habit will be stored
- show user a menu of options
- User should get/post/put/delete habits
- handle all errors so it won't crash
- application should be terminated when user inserts 0
- can only interact with the db using raw sql, no ef **use ADO.net
- needs readme

CHALLENGES:
- Lets user create their own habits to track, which will require lettings them choose their own unit of measurement
- seed db automatically on load (generate a few habits and inserting random generated values)
- Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?)
  SQL allows you to ask very interesting things from your database.
 */
namespace HabitTracker.kwm0304;

class Program
{
  static void Main(string[] args)
  {
    
  }
}
