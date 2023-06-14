using System;
using System.Windows.Forms;
using System.Windows.Input;
using WIN_SHORTCUTS_CL.Structure;

namespace WIN_SHORTCUTS_CL.Functions
{
    /// <summary>
    /// 이 클래스는 Windows 32bit Forms 기반의 핸들 생성과 wndProc 후킹 방식
    /// </summary>
    internal class HookHandleCreater_NativeWindow: NativeWindow, IDisposable
    {
        private static int WM_HOTKEY = 0x0312;

        public HookHandleCreater_NativeWindow()
        {
            // create the handle for the window.
            this.CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // check if we got a hot key pressed.
            if (m.Msg == WM_HOTKEY)
            {
                // get the keys.
                Key key = (Key)(((int)m.LParam >> 16) & 0xFFFF);
                ModiKey modifier = (ModiKey)((int)m.LParam & 0xFFFF);

                // invoke the event to notify the parent.
                if (KeyPressed != null)
                    KeyPressed(this, new KeyPressedEventArgs(modifier, key));
            }
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            this.DestroyHandle();
        }

        #endregion
    }
}
