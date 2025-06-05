namespace Main.Models
{
    internal class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public Category(int id = 0, string name = "", string unit = "")
        {
            this.Id = id;
            this.Name = name;
            this.Unit = unit;
        }
    }
}
