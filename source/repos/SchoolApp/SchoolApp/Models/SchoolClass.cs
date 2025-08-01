using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models
{
    public class SchoolClass
    {
        public string  Location { get; set; }
        public string ClassName { get; set; }
        public int MaxStudents { get; set; } = 30;

        public override string ToString()=>  $"{ClassName} at {Location} (Max: {MaxStudents})";
        
    }
}
