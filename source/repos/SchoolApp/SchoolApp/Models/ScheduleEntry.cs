using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models
{
    public class ScheduleEntry
    {
        public Guid EntryId { get; set; } = Guid.NewGuid();
        public Professor Professor { get; set; }
        public SchoolClass Class { get; set; }

        public List<Student> Students { get; set; } = [];
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


    }
}
