using Newtonsoft.Json.Bson;
using SQLitePCL;
using yashsachdev.HabitTracker;
namespace Habit_tracher.tests;
public class HabitEnrollTest
{
    [Fact]
    public void DisplayAllHabits()
    {
        //Arrange
        HabitEnrollRepo repo = new HabitEnrollRepo();
        string name = "Test";
        string email = "Test@gmail.com";
        var expected = "";
        //Act
        using (var writer = new StringWriter())
        {
            Console.SetOut(writer);
            repo.DisplayUserHabit(name, email);
            var actualOutput = writer.ToString();
            //Assert
            Assert.Equal(expected, actualOutput);
        }

    }
    [Fact]
    public void DisplayUserHabits() { }
}