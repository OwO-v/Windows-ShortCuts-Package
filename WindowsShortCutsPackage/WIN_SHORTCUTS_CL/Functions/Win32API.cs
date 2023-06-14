using System;
using System.Runtime.InteropServices;

namespace WIN_SHORTCUTS_CL.Functions
{
    internal static class Win32API
    {
        // Win32API에 쓰이는 상수
        // 전부 uint 선언할 것, int로 변환하면 0xffff로 마스킹
        internal const uint
            WS_CHILD = 0x40000000,
            WS_VISIBLE = 0x10000000,
            LBS_NOTIFY = 0x00000001,
            HOST_ID = 0x00000002,
            LISTBOX_ID = 0x00000001,
            WS_VSCROLL = 0x00200000,
            WS_BORDER = 0x00800000,
            WM_HOTKEY = 0x00000312,
            WS_INVISIBLE = 0x00000002,
            WS_EX_TRANSPARENT = 0x00000020;
            


        // HotKey 새로 등록하는 함수
        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);   
        
        // HotKey 설정을 해제하는 함수
        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);                                               



        // Win32API를 이용해 창을 생성하는 함수
        // -> WPF는 창을 그릴 때 Win32API를 이용하지 않아 WndProc을 Override 하기 위해서 이용
        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CreateWindowEx(
            int dwExStyle,                                                        
            string lpszClassName,
            string lpszWindowName,
            uint style,
            int x, int y,
            int width, int height,
            IntPtr hwndParent,
            IntPtr hMenu,
            IntPtr hInst,
            [MarshalAs(UnmanagedType.AsAny)] object pvParam);

        // CreateWindowEx를 이용해 생성한 창을 제거하는 메소드
        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        internal static extern bool DestroyWindow(IntPtr hwnd);
    }
}
