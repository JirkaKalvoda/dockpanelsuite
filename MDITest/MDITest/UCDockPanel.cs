using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MDITest
{
    public partial class UCDockPanel : DockPanel
    {
        public UCDockPanel()
        {
            InitializeComponent();
        }

        public override void OnFloatWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.Enter)
            {
                FloatWindow fw = sender as FloatWindow;
                if (!fw.IsFullScreen)
                {
                    fw.PreviousWindowState = fw.WindowState;
                    fw.PreviousBorderStyle = fw.FormBorderStyle;
                    // 先取消边框，上方不超出屏幕，下方遮挡任务栏；先最大化，上方超出屏幕，下方不遮挡任务栏
                    fw.FormBorderStyle = FormBorderStyle.None;
                    fw.WindowState = FormWindowState.Maximized;
                    fw.IsFullScreen = true;
                }
                else
                {
                    // 如果重复切换窗口、全屏，出现窗口越来越大/小，可以交换下面2行顺序
                    fw.WindowState = fw.PreviousWindowState;
                    fw.FormBorderStyle = fw.PreviousBorderStyle;
                    fw.IsFullScreen = false;
                }
            }
        }
    }
}
