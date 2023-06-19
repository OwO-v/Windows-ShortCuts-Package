using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    //
    // 2023.06.19
    // 어째 점점 범위가 엄청나게 커지고 있다. 플로우 그려놓고 다시 시작
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

        private static Task[] ShortCutsWorker = null;

        public int Mode
        {
            get
            {
                return KeyboardDTO.Mode;
            }
            set
            {
                KeyboardDTO.Mode = value;
            }
        }
        public int TrayMode
        {
            get { return KeyboardDTO.TrayMode; }
            set { KeyboardDTO.TrayMode = value; }
        }

        public System.Windows.Controls.UserControl ViewUC_Load
        {
            get { return KeyboardDTO.ViewUC_Load; }
            set { KeyboardDTO.ViewUC_Load = value; }
        }
        public System.Windows.Controls.UserControl ViewUC_Conf
        {
            get { return KeyboardDTO.ViewUC_Conf; }
            set { KeyboardDTO.ViewUC_Conf = value; }
        }

        public Task<bool>Run()
        {
            if (ShortCutsWorker != null)
            {
                throw new ScDllException("이미 단축키 프로그램이 실행중입니다.");
            }
            
            try
            {
                Task<int> tskLoad = View_Loading();
                Task<int> tskMain = View_Main();
                Task<int>[] WindowsShortCutsPackageTask = new[] {tskLoad,tskMain};

                ShortCutsWorker = WindowsShortCutsPackageTask.Select(async t =>
                {
                    var tskResult = await t;
                }).ToArray();

                Task.WhenAll(ShortCutsWorker);

            }
            catch (InvalidOperationException te)
            {
                throw te;
            }
            catch (ScDllException se)
            {
                throw se;
            }
            catch (Exception ex) 
            {
                throw ex;
            }

            return Task.FromResult(true);
        }

        public void testc()
        {
            var a = Run();
            Console.WriteLine(a.Result);
        }

        public bool Stop()
        {
            try
            {
                ShortCutsWorker = null;
                this.Dispose();
            }
            catch(ScDllException se)
            {
                throw se;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return true;
        }

        private Task<int> View_Loading()
        {
            var loadView = new WSC_LOADING();
            loadView.ShowDialog();
            if (!loadView.DialogResult.HasValue || !loadView.DialogResult.Value)
            {
                throw new ScDllException("ShortCuts Initialize를 실패했습니다.");
            }
            return Task.FromResult(1);
        }

        private Task<int> View_Main()
        {
            var trayView = new WSC_MAINTRAY();
            trayView.Show();

            return Task.FromResult(1);
        }
    }
}
