using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models
{
    public class Student : Person
    {
        public Guid StudentId { get; set; }= Guid.NewGuid();
        public List<Grade> Grades { get; set; } = [];

        public double AverageGrade => Grades.Count == 0 ? 0 : Grades.Average(g => g.Value);

        public override string ToString() => $"{FirstName} {LastName} (Student)";
    }
}
