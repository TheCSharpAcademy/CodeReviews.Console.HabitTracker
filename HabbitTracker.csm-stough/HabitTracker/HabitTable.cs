using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    internal struct HabitTable
    {

        public int ID;
        public string HabitName;
        public string TableName;
        public string TableType;
        public string TableUnit;

        public HabitTable(int ID, string HabitName, string TableName, string TableType, string TableUnit)
        {
            this.ID = ID;
            this.HabitName = HabitName;
            this.TableName = TableName;
            this.TableType = TableType;
            this.TableUnit = TableUnit;
        }
    }
}
