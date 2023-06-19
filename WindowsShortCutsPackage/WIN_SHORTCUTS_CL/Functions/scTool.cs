using System;
using System.Collections.Generic;
using System.Windows.Input;
using WIN_SHORTCUTS_CL.Structure;

namespace WIN_SHORTCUTS_CL.Functions
{
    internal class ScTool
    {
        private event EventHandler<KeyPressedEventArgs> KeyPressed;

        private readonly HookHandleCreater windowHandle;
        private readonly KeyboardDTO keyboardValues;

        //Id값은 2개 이상의 HotKey 저장을 위한 Values -> 아직 사용하지 않음
        private List<int> _currentId = new List<int>();
        private int _lastId = 0;


        internal ScTool()
        {
            windowHandle = new HookHandleCreater();
            keyboardValues = new KeyboardDTO();

            windowHandle.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
            {
                if (KeyPressed != null)
                    KeyPressed(this, args);
            };
        }

        internal void RegisterHotKey()
        {
            if (Win32API.UnregisterHotKey(windowHandle.Handle, _lastId))
                _currentId.Remove(_lastId);

            if (Win32API.RegisterHotKey(windowHandle.Handle, ++_lastId, (uint)keyboardValues.ModifierKey, (uint)keyboardValues.DataKey))
                _currentId.Add(_lastId);
            else
                throw new InvalidOperationException("단축키 저장 실패");
        }
        internal void RegisterHotKey(ModiKey _modifier, Key _key)
        {
            keyboardValues.ModifierKey = _modifier;
            keyboardValues.DataKey = _key;

            if (Win32API.UnregisterHotKey(windowHandle.Handle, _lastId))
                _currentId.Remove(_lastId);

            if (Win32API.RegisterHotKey(windowHandle.Handle, ++_lastId, (uint)keyboardValues.ModifierKey, (uint)keyboardValues.DataKey))
                _currentId.Add(_lastId);
            else
                throw new InvalidOperationException("단축키 저장 실패");
        }
    }
}
