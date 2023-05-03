using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{

    /**
     * An active record for record in our drinking_water table
     */
    internal struct WaterRecord
    {

        public DateTime Date;
        public int Quantity;
        public int ID;

        public WaterRecord(DateTime date, int quantity, int id)
        {
            this.Date = date;
            this.Quantity = quantity;
            this.ID = id;
        }
    }
}
