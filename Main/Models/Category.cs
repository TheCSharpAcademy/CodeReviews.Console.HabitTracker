using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    internal class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public Category(int id = 0, string name = "", string unit = "")
        {
            Id = id;
            Name = name;
            Unit = unit;
        }
    }
}
