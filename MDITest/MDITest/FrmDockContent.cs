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

            this.LocationChanged += FrmDock_LocationChanged;
            FrmDock_LocationChanged(null, null);

        }

        protected override string GetPersistString()
        {
            return Name;
        }
        

        private void FrmDock_LocationChanged(object sender, EventArgs e)
        {
            textBox1.Text = "0x" + panel1.Handle.ToString("x").PadLeft(16, '0');
            textBox1.Select(0, 0);
        }
    }
}
