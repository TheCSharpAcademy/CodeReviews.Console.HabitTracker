using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.GoldRino456
{
    public sealed class InputManager
    {
        #region Singleton Logic & Properties
        private static InputManager? _instance = null;
        private static readonly object _instanceLock = new object();

        public static InputManager Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new InputManager();
                    }
                    return _instance;
                }

            }
        }
        #endregion
    }
}
