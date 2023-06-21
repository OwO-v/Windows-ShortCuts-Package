using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;
using WIN_SHORTCUTS_CL.Structure;

namespace WIN_SHORTCUTS_CL.Functions
{
    /// <summary>
    /// wndProc후킹을 위한 Win32API 창은 MSDN 참고해서 뚝딱
    /// https://learn.microsoft.com/ko-kr/dotnet/desktop/wpf/advanced/walkthrough-hosting-a-win32-control-in-wpf?view=netframeworkdesktop-4.8
    /// </summary>
    internal class ScHandleController : HwndHost
    {
        internal event EventHandler<KeyPressedEventArgs> KeyPressed;
        IntPtr hwndHost = IntPtr.Zero;

        /// <summary>
        /// BuildWindowCore 호출 시 창 생성
        /// </summary>
        /// <returns>핸들 참조값</returns>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            hwndHost = Win32API.CreateWindowEx(0, "ShortCutsHandle", "", Win32API.WS_INVISIBLE | Win32API.WS_EX_TRANSPARENT, 0, 0, 0, 0,
                                      hwndParent.Handle, (IntPtr)Win32API.HOST_ID, IntPtr.Zero, 0);

            return new HandleRef(this, hwndHost);
        }

        /// <summary>
        /// 윈도우 메시지 Hook 진행하는 함수 Override
        /// </summary>
        /// <returns></returns>
        protected override IntPtr WndProc(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (Msg)
            {
                case (int)Win32API.WM_HOTKEY:
                    Key key = (Key)(((int)lParam >> 16) & 0xFFFF);
                    ModiKey modifier = (ModiKey)((int)lParam & 0xFFFF);

                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
                    handled = true;
                
                    break;

                default:
                    return base.WndProc(hWnd, Msg, wParam, lParam, ref handled);
            }

            // base.WndProc 혹은 IntPtr.Zero 둘 중 하나만 적용
            // 컨트롤 기본 동작 유지를 위해 base.WndProc() 이용
            return base.WndProc(hWnd, Msg, wParam, lParam, ref handled);
            //return IntPtr.Zero;
        }
        
        //Handle을 명시적으로 삭제, Dispose를 따로 구현하지 않아도 부모 클래스 HwndHost에서 DestoryWindowCore를 통해 구현한다.
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            Win32API.DestroyWindow(hwnd.Handle);
        }
    }
}
