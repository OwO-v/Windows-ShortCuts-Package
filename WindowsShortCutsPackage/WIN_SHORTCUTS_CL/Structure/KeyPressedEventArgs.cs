using System;
using System.Windows.Input;

namespace WIN_SHORTCUTS_CL.Structure
{
    /// <summary>
    /// 이벤트 Args
    /// </summary>
    /// public class KeyPressedEventArgs
    internal class KeyPressedEventArgs : EventArgs
    {
        internal KeyPressedEventArgs(ModiKey _modifier, Key _data)
        {
            EvtModifier = _modifier;
            EvtData = _data;
        }

        internal ModiKey EvtModifier { get; }
        internal Key EvtData { get; }
    }
}
