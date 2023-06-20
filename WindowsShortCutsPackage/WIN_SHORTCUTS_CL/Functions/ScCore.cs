using System;
using System.Diagnostics;
using WIN_SHORTCUTS_CL.Structure;

namespace WIN_SHORTCUTS_CL.Functions
{
    internal class ScCore : IDisposable
    {
        #region Dispose 구현
        public void Dispose()
        {
            windowHandle.Dispose();
        }
        #endregion
        internal event EventHandler<KeyPressedEventArgs> KeyPressed;
        private readonly ScHandleController windowHandle;

        internal ScCore()
        {
            windowHandle = new ScHandleController();

            windowHandle.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
            {
                if (KeyPressed != null)
                    KeyPressed(this, args);
            };
        }

        internal int RegisterHotKey(ShortCutsPairList shortCutsPairs, int scCurrentId)
        {
            while (0 < scCurrentId)
            {
                if (Win32API.UnregisterHotKey(windowHandle.Handle, scCurrentId--) == false)
                    throw new ScDllException("단축키 저장 실패");
            }

            foreach(var pair in shortCutsPairs)
            {
                if (Win32API.RegisterHotKey(windowHandle.Handle, scCurrentId++, (uint)pair.ModifierKey, (uint)pair.DataKey) == false)
                    throw new ScDllException("단축키 저장 실패");
            }

            return scCurrentId;
        }

        internal void UnregisterHotKey(int scCurrentId)
        {
            while (0 < scCurrentId)
            {
                if (Win32API.UnregisterHotKey(windowHandle.Handle, scCurrentId--) == false)
                    throw new ScDllException("단축키 저장 실패");
            }
        }

        internal void ScKindAction(ShortCutsPair pair)
        {
            try
            {
                switch (pair.Action)
                {
                    case ScAction.run:
                        break;

                    case ScAction.textCopy:
                        break;

                    case ScAction.textPaste:
                        break;
                }
            }
            catch(ScDllException se)
            {
                throw se;
            }
        }
    }
}
