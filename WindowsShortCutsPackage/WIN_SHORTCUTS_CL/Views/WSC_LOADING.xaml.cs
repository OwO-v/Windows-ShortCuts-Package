using System.Windows;
using WIN_SHORTCUTS_CL.Functions;
using WIN_SHORTCUTS_CL.Structure;
using System.Diagnostics;

namespace WIN_SHORTCUTS_CL.Views
{
    public partial class WSC_LOADING : Window
    {
        ScConfFileTool ConfigTool = null;

        public WSC_LOADING()
        {
            this.DialogResult = false;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 동일 프로세스 존재하는지 확인
            procActiveCheck("ShortCutsHandle");//Process.GetCurrentProcess().ProcessName);

            // ConfFile 생성/접근 -> 설정 데이터 호출
            ConfigTool = new ScConfFileTool(KeyboardDTO.UserKeyPairFilePath);
            ConfigTool.ReadConfFileStream();

            DialogResult = true;
            this.Close();
        }

        private int procActiveCheck(string targetProcName)
        {
            int procCount = 0;
            Process currentProcess = Process.GetCurrentProcess();

            foreach (Process p in Process.GetProcessesByName(targetProcName))
            {
                if (p.Id == currentProcess.Id)
                {
                    procCount++;
                    break;
                }
            }

            return procCount;
        }
    }
}
