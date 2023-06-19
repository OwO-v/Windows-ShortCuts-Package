using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace WIN_SHORTCUTS_CL.Views
{
    public partial class WSC_MAINTRAY : Window, IDisposable
    {
        #region Dispose 구현
        public void Dispose()
        {
            // 트레이메뉴는 .net 2.0부터 유서깊은 트러블 메이커로
            // 단골 좀비니까 꼭 하나하나 정리해주자

            tryItem_Title = null;

            tryItem_Enable.Click -= TryItem_Enable_Click;
            tryItem_Enable.VisibleChanged -= TryItem_Enable_VisibleChanged;
            tryItem_Enable = null;

            tryItem_Disable.Click -= TryItem_Disable_Click;
            tryItem_Disable.VisibleChanged -= TryItem_Disable_VisibleChanged;
            tryItem_Disable = null;

            tryItem_UserConfig.Click -= TryItem_UserConfig_Click;
            tryItem_UserConfig = null;

            tryItem_Exit.Click -= TryItem_Exit_Click;
            tryItem_Exit = null;

            tryItem_TitleSeparator = null;
            tryItem_ExitSeparator = null;

            tryMenu.Opening -= TryMenu_Opening;
            tryMenu.Dispose();

            tryIcon.DoubleClick -= TryIcon_DoubleClick;
            tryIcon.Dispose();
        }

        #endregion

        #region TryIcon 디자인 선언
        private NotifyIcon tryIcon = null;
        private ContextMenuStrip tryMenu = null;

        private ToolStripMenuItem tryItem_Title = null;
        private ToolStripMenuItem tryItem_Enable = null;
        private ToolStripMenuItem tryItem_Disable = null;
        private ToolStripMenuItem tryItem_UserConfig = null;
        private ToolStripMenuItem tryItem_Exit = null;

        private ToolStripSeparator tryItem_TitleSeparator = null;
        private ToolStripSeparator tryItem_ExitSeparator = null;
        
        #endregion

        public WSC_MAINTRAY()
        {
            InitializeComponent();
            SetTrayIcon((Icon)Properties.Resources.ICON_MAINTRAY);
            InitMenuItem();
        }

        public WSC_MAINTRAY(Icon _customIcon)
        {
            InitializeComponent();
            SetTrayIcon(_customIcon);
            InitMenuItem();
        }

        private void SetTrayIcon(Icon _icon)
        {
            tryIcon = new NotifyIcon();
            tryMenu = new ContextMenuStrip();

            //tryIcon Initialize
            tryIcon.Icon = _icon;
            tryIcon.Visible = true;
            tryIcon.Text = "전역 단축키 지정 프로그램";
            
            tryIcon.DoubleClick += TryIcon_DoubleClick;


            tryMenu.SuspendLayout();
            tryMenu.Name = "ShortCuts Tray Menu";
            tryMenu.ShowImageMargin = false;
            
            tryMenu.Opening += TryMenu_Opening;
        }


        private void InitMenuItem()
        {
            #region Initialize
            tryItem_Title = new ToolStripMenuItem();
            tryItem_Enable = new ToolStripMenuItem();
            tryItem_Disable = new ToolStripMenuItem();
            tryItem_UserConfig = new ToolStripMenuItem();
            tryItem_Exit = new ToolStripMenuItem();

            tryItem_TitleSeparator = new ToolStripSeparator();
            tryItem_ExitSeparator = new ToolStripSeparator();

            #endregion

            #region 전역 단축키 프로그램 설명
            tryItem_Title.Text = "윈도우 단축키 지정 프로그램";

            #endregion

            #region 전역 단축키 활성화
            tryItem_Enable.Text = "단축키 활성화";
            tryItem_Enable.Visible = true;
            tryItem_Enable.Enabled = true;

            tryItem_Enable.Click += TryItem_Enable_Click;
            tryItem_Enable.VisibleChanged += TryItem_Enable_VisibleChanged;
            
            #endregion

            #region 전역 단축키 비활성화
            tryItem_Disable.Text = "단축키 비활성화";
            tryItem_Disable.Visible = false;
            tryItem_Disable.Enabled = false;

            tryItem_Disable.Click += TryItem_Disable_Click;
            tryItem_Disable.VisibleChanged += TryItem_Disable_VisibleChanged;

            #endregion

            #region 단축키 사용자 설정, 실행 
            tryItem_UserConfig.Text = "단축키 설정";
            tryItem_UserConfig.Visible = false;
            tryItem_UserConfig.Enabled = false;

            tryItem_UserConfig.Click += TryItem_UserConfig_Click;

            #endregion

            #region 종료
            tryItem_Exit.Text = "종료";
            tryItem_Exit.Visible = true;
            tryItem_Exit.Enabled = true;

            tryItem_Exit.Click += TryItem_Exit_Click;

            #endregion


            tryMenu.Items.AddRange(new ToolStripItem[]
            {
                tryItem_Title,
                tryItem_TitleSeparator,
                tryItem_Enable,
                tryItem_Disable,
                tryItem_ExitSeparator,
                tryItem_Exit
            });
            tryMenu.ResumeLayout();
        }

        

        #region 트레이메뉴 클릭 이벤트

        private void TryMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void TryIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
        }

        private void TryItem_Enable_Click(object sender, EventArgs e)
        {

        }

        private void TryItem_Enable_VisibleChanged(object sender, EventArgs e)
        {
            tryItem_Enable.Visible = false;
            tryItem_Enable.Enabled = false;

            tryItem_Disable.Visible = true;
            tryItem_Disable.Enabled = true;
            tryItem_UserConfig.Visible = true;
            tryItem_UserConfig.Enabled = true;
        }

        private void TryItem_Disable_Click(object sender, EventArgs e)
        {

        }

        private void TryItem_Disable_VisibleChanged(object sender, EventArgs e)
        {
            tryItem_Disable.Visible = false;
            tryItem_Disable.Enabled = false;
            tryItem_UserConfig.Visible = false;
            tryItem_UserConfig.Enabled = false;

            tryItem_Enable.Visible = true;
            tryItem_Enable.Enabled = true;
        }

        private void TryItem_UserConfig_Click(object sender, EventArgs e)
        {

        }

        private void TryItem_Exit_Click(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown(0);
            this.Dispose();
        }

        #endregion
    }
}
