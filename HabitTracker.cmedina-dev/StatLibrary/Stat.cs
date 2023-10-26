#nullable enable
namespace StatLibrary
{
    public sealed class Stat
    {
        public string Name;
        public int? Value;

        public Stat(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}