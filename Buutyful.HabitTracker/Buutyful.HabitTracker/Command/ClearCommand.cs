using Buutyful.HabitTracker.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buutyful.HabitTracker.Command;

public class ClearCommand : ICommand
{
    public void Execute()
    {
        Console.Clear();
    }
}
