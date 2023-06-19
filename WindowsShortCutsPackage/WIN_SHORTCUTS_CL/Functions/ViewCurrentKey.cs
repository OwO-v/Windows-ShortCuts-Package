using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Interop;

namespace WIN_SHORTCUTS_CL.Functions
{
    /// <summary>
    /// 해당 코드는 https://github.com/PabloHorno/HotKeyDialog/tree/master/HotKeyDialog를 참조하여 만들었으며, WPF에 적용 가능하도록 수정
    /// </summary>
    internal class ViewCurrentKey : KeyEventArgs
    {
        internal ViewCurrentKey(Key keyData) : base(Keyboard.PrimaryDevice, new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero), 0, keyData)
        {

        }

        internal bool IsValid
        {
            get
            {
                return (Key != Key.None && Key != (Key)ModifierKeys.Shift && Key != (Key)ModifierKeys.Alt && Key != (Key)ModifierKeys.Control && Key != (Key)ModifierKeys.Windows);
            }
        }

        public override string ToString()
        {
            List<string> strKeys = new List<string>();
            if (IsDown && (uint)Key == (uint)ModifierKeys.Control)
                strKeys.Add("Ctrl");
            if (IsDown && (uint)Key == (uint)ModifierKeys.Shift)
                strKeys.Add("Shift");
            if (IsDown && (uint)Key == (uint)ModifierKeys.Alt)
                strKeys.Add("Alt");
            if (IsDown && (uint)Key == (uint)ModifierKeys.Windows)
                strKeys.Add("Win");
            if ((IsDown && (uint)Key != (uint)ModifierKeys.Control) && 
                (IsDown && (uint)Key != (uint)ModifierKeys.Alt) && 
                (IsDown && (uint)Key != (uint)ModifierKeys.Shift) && 
                (IsDown && (uint)Key != (uint)ModifierKeys.Windows))
                strKeys.Add(Key.ToString());

            return String.Join("+", strKeys.ToArray());
        }

        public static bool operator ==(ViewCurrentKey hk1, ViewCurrentKey hk2)
        {
            return  ((uint)hk1.Key == (uint)ModifierKeys.Control) == ((uint)hk2.Key == (uint)ModifierKeys.Control) && 
                    ((uint)hk1.Key == (uint)ModifierKeys.Alt) == ((uint)hk2.Key == (uint)ModifierKeys.Alt) && 
                    ((uint)hk1.Key == (uint)ModifierKeys.Shift) == ((uint)hk2.Key == (uint)ModifierKeys.Shift) && 
                    ((uint)hk1.Key == (uint)ModifierKeys.Windows) == ((uint)hk2.Key == (uint)ModifierKeys.Windows) &&
                    ((uint)hk1.Key == (uint)hk2.Key);
        }
        public static bool operator !=(ViewCurrentKey hk1, ViewCurrentKey hk2)
        {
            return !(hk1 == hk2);
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
