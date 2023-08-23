using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MDITest
{
    public class FrmDockContent : DockContent
    {
        private Panel panel1;

        private TextBox textBox1;

        public FrmDockContent()
        {
            textBox1 = new TextBox();
            this.Controls.Add(textBox1);
            textBox1.Dock = DockStyle.Top;
            textBox1.ReadOnly = true;
            panel1 = new Panel();
            this.Controls.Add(panel1);
            panel1.Dock = DockStyle.Fill;
            this.KeyPreview = true;

            FrmDock_LocationChanged(null, null);
            
            this.LocationChanged += FrmDock_LocationChanged;
            this.KeyDown += FrmDockContent_KeyDown;
        }


        protected override string GetPersistString()
        {
            return Name;
        }
        
        /// <summary>
        /// 证实内部可见控件不会被销毁重造
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmDock_LocationChanged(object sender, EventArgs e)
        {
            textBox1.Text = "0x" + panel1.Handle.ToString("x").PadLeft(16, '0');
            textBox1.Select(0, 0);
        }

        private void FrmDockContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.DockState == DockState.Float /*&& e.Modifiers == Keys.Alt && e.KeyCode == Keys.Enter*/)
            {
                this.DockPanel.OnFloatWindowKeyDown(this.Pane.FloatWindow, e);
            }
        }
        
    }
}
