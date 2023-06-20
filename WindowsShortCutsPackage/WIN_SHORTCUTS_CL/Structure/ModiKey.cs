using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIN_SHORTCUTS_CL.Structure
{
    [Flags]
    internal enum ModiKey : uint
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}
