using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models
{
    public class Grade
    {
        public string Subject { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    }
}
