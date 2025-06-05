using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    internal class Habit
    {
        public int Id { get; set; }
        public string Date { get; set; } = DateTime.Now.ToString("yyyy-mm-dd");
        public int Quantity { get; set; }
        public Category? Category { get; set; }
    }
}
