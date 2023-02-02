namespace yashsachdev.HabitTracker;
public class User
{
    public User():this(0)
    {

    }
    public User(int user_Id)
    {
        user_Id = User_Id;
        habitEnrolls = new List<HabitEnroll>();
    }
    public int User_Id { get; set; }
    public List<HabitEnroll> habitEnrolls { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
   /// <summary>
   /// Validate the properties of User class.
   /// </summary>
   /// <returns></returns>
   public bool validate()
    {
        var isValid = true;
        if(string.IsNullOrEmpty(Name)) isValid = false; 
        if(string.IsNullOrWhiteSpace(Password)) isValid = false;
        if(string.IsNullOrWhiteSpace(Email))isValid= false;
        return isValid; 
    }
}