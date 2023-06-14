using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using WIN_SHORTCUTS_CL.Functions;
using WIN_SHORTCUTS_CL.Structure;
using WIN_SHORTCUTS_CL.Views;

namespace WIN_SHORTCUTS_CL
{
    #region Comment History

    // 2023.06.13
    // 내가 무슨 생각으로 Dll을 먼저 만들었을까?
    // 오늘은 단일 단축키만 만들자
    //
    // 2023.06.14
    // 오늘은 기본 View와 Tray, 사용자 커스텀 만들자
    #endregion

    #region Develop Guide

    // Endpoint는 여기!
    // Dispose 시 모두 UnHook하기
    // 실행 단계에서 파일을 읽고 다시 Hook 걸기
    // WinForm을 오래하다보니 System.Windows.Forms에 너무 의존하고있다. Forms 쓰지말자

    #endregion

    /// <summary>
    /// 사용 방법
    /// 
    /// </summary>
    public class ShortCuts : IDisposable
    {
        #region Dispose 구현
        
        public void Dispose()
        {
            // 전부 다 UnHook 해야 함!!!

        }

        #endregion
        
        public bool Run()
        {
            try
            {
                var loadView = new WSC_LOADING();
                loadView.ShowDialog();
                if (!loadView.DialogResult.HasValue && !loadView.DialogResult.Value)
                {
                    throw new Exception("ShortCuts Initialize를 실패했습니다.");
                }

                var trayView = new WSC_TRAY();
                trayView.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
    }
}
