using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models
{
    public class Professor: Person
    {
        public Guid ProfessorId { get; set; } = Guid.NewGuid();
        public override string ToString() => $"{FirstName} {LastName} (Professor)";
    }
    
}
