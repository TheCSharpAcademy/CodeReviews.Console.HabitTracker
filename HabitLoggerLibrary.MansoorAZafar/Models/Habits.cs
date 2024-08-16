using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLoggerLibrary.MansoorAZafar.Models
{
    public enum HabitSelections
    {
        None   = -1,
        exit   = 0,
        update = 1,
        delete,
        insert,
        data,
        reports
    }

    public enum ReportSelections
    {
        None = -1, 
        exit = 0,
        startFromXDaysAgo = 1,
        dateToToday = 2,
        totalAllTime = 3,
        startToEnd,
        totalForMonth
    }

}
