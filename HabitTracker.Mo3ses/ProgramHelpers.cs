public static class ProgramHelpers
{

    public static int ValidateInputs(string input)
    {
        int output = 0;
        if (int.TryParse(input, out output))
        {
            return output;
        }
        else
        {
            Console.WriteLine("Please Type a Number.");
            return 0;
        }
    }
}