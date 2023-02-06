using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MDITest.Enums;

namespace MDITest
{
    public partial class Form1 : Form
    {

        private Dictionary<string, FrmDockContent> sideFormDict = new Dictionary<string, FrmDockContent>();

        private Dictionary<string, FrmDockContent> docuFormDict = new Dictionary<string, FrmDockContent>();

        private List<FrmDockContent> formList = new List<FrmDockContent>();

        private volatile int index = 1;

        private string layoutFile = Path.Combine(Application.StartupPath, "layout.xml");

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;
            var theme = new VS2015LightTheme();
            dockPanel1.Theme = theme;
            dockPanel1.DockRightPortion = 250;       // 限定四周窗口的最大宽度
            dockPanel1.DockLeftPortion = 250;        // 限定四周窗口的最大宽度

            this.Load += Form1_Load;
            buttonDocu.Click += ButtonDocu_Click;
            buttonSide.Click += ButtonSide_Click;
            buttonSave.Click += ButtonSave_Click;
            buttonLoad.Click += ButtonLoad_Click;
            buttonClear.Click += ButtonClear_Click;
            dockPanel1.DocumentDragged += DocumentDragged;
        }


        private void ResetFloatWindow(FloatWindow fw)
        {
            fw.FormBorderStyle = FormBorderStyle.Sizable;
            fw.ShowInTaskbar = true;             // 这句导致不能foreach
            fw.ShowIcon = false;
        }

        /// <summary>
        /// 根据程序集名.类名字符串创建窗口，给委托用
        /// </summary>
        /// <param name="persistString"></param>
        /// <returns></returns>
        private IDockContent CreateDockContent(string persistString)
        {
            FrmDockContent ret = null;
            //if (persistString == typeof(FrmDockContent).ToString())
            //{
                ret = new FrmDockContent();
            //}
            ret.Name = persistString;
            formList.Add(ret);
            return ret;
        }

        private void SetDockContent(FrmDockContent dc, DockType type)
        {
            dc.Text = dc.Name;
            dc.TabText = dc.Name;
            switch (type)
            {
                case DockType.Side:
                    dc.DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.Float;
                    dc.FormClosing += SideClosing;
                    break;

                case DockType.Document:
                    dc.DockAreas = DockAreas.Document | DockAreas.Float;
                    dc.FormClosing += DocuClosing;
                    break;

                default:
                    break;
            }
            dc.DoubleClick += DockStateChanged;
            dc.DockStateChanged += DockStateChanged;
        }

        private void ClearDockContents()
        {
            docuFormDict.Clear();
            sideFormDict.Clear();
            formList.Clear();
            index = 1;
            // 清理DockContent和Pane
            List<IDockContent> forms = dockPanel1.Contents.ToList();
            for (int i = forms.Count - 1; i >= 0; i--)
            {
                (forms[i] as FrmDockContent).Dispose();
                forms.RemoveAt(i);
            }
            // FloatWindow是DockContent外面包的一层窗口，需要单独清理，DockWindow固定是5个不用管
            // 每次读取会多出1-2个空的FloatWindow，如果不清理会指数增长
            List<FloatWindow> floats = dockPanel1.FloatWindows.ToList();
            for (int i = floats.Count - 1; i >= 0; i--)
            {
                floats[i].Dispose();
                floats.RemoveAt(i);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            var frms = new List<FrmDockContent>();
            var enums = Enum.GetValues(typeof(WeifenLuo.WinFormsUI.Docking.DockState));
            for (int i = 0; i < enums.Length; i++)
            {
                var state = (WeifenLuo.WinFormsUI.Docking.DockState)enums.GetValue(i);
                var f = new FrmDockContent()
                {
                    Text = "Text---" + i.ToString() + "---" + state.ToString(),
                    TabText = "TabText---" + i.ToString() + "---" + state.ToString(),
                };
                f.DockPanel = dockPanel1;
                f.DockState = state;
                frms.Add(f);
            }
            frms.ForEach(f =>
            {
                f.Show();
            });
            */
            /*
            FrmDockContent dc1 = new FrmDockContent();
            FrmDockContent dc2 = new FrmDockContent();
            FrmDockContent dc3 = new FrmDockContent();
            FrmDockContent dc4 = new FrmDockContent();
            FrmDockContent dc5 = new FrmDockContent();
            FrmDockContent dc6 = new FrmDockContent();
            dc1.TabText = "1";
            dc2.TabText = "2";
            dc3.TabText = "3";
            dc4.TabText = "4";
            dc5.TabText = "5";
            dc6.TabText = "6";
            dc1.Show(dockPanel1, DockState.Document);
            dc2.Show(dockPanel1, DockState.Document);
            dc3.Show(dc2.Pane, DockAlignment.Right, 0.5);
            dc4.Show(dockPanel1, DockState.DockLeft);
            dc5.Show(dc4.Pane, DockAlignment.Bottom, 0.5);
            dc4.DockState = DockState.DockLeft;
            dc5.DockState = DockState.DockLeft;
            dc6.Show(dc4.Pane, dc4);
            //dc1.FormBorderStyle = FormBorderStyle.Sizable;     // 里面多了边框
            //dc1.Size = new Size(200, 200);       // 无效
            var dw = dockPanel1.DockWindows;
            var fw = dockPanel1.FloatWindows;
            var p = dockPanel1.Panes;
            */
        }
        
        
        private void ButtonDocu_Click(object sender, EventArgs e)
        {
            FrmDockContent dc = new FrmDockContent();
            dc.Name = "docu " + index.ToString();
            SetDockContent(dc, DockType.Document);
            //bool isShown = false;
            //if (docuFormDict.Count > 0)
            //{
            //    List<FrmDockContent> dclist = docuFormDict.Values.ToList();
            //    for (int i = dclist.Count - 1; i >= 0; --i)
            //    {
            //        if (!dclist[i].DockState.IsValid())
            //        {
            //            continue;
            //        }
            //        isShown = true;
            //        dc.Show(dclist[i].Pane, DockAlignment.Right, 0.5);
            //    }
            //}
            //if (docuFormDict.Count == 0 || !isShown)
            //{
                dc.Show(dockPanel1, DockState.Document);
            //}
            docuFormDict.Add(dc.Name, dc);
            index += 1;
        }


        private void ButtonSide_Click(object sender, EventArgs e)
        {
            FrmDockContent dc = new FrmDockContent();
            dc.Name = "side " + index.ToString();
            SetDockContent(dc, DockType.Side);
            //bool isShown = false;
            //if (sideFormDict.Count > 0)
            //{
            //    List<FrmDockContent> dclist = sideFormDict.Values.ToList();
            //    for (int i = dclist.Count - 1; i >= 0; --i)
            //    {
            //        if (!dclist[i].DockState.IsValid())
            //        {
            //            continue;
            //        }
            //        isShown = true;
            //        dc.Show(dclist[i].Pane, DockAlignment.Bottom, 0.5);
            //    }
            //}
            //if (sideFormDict.Count == 0 || !isShown)
            //{
                dc.Show(dockPanel1, DockState.DockLeft);
            //}
            sideFormDict.Add(dc.Name, dc);
            index += 1;
        }


        private void ButtonSave_Click(object sender, EventArgs e)
        {
            dockPanel1.SaveAsXml(layoutFile);
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            ClearDockContents();
            dockPanel1.LoadFromXml(layoutFile, new DeserializeDockContent(CreateDockContent));
            for (int i = 0; i < formList.Count && formList[i] != null; ++i)
            {
                if (formList[i].Name.StartsWith("docu"))
                {
                    docuFormDict.Add(formList[i].Name, formList[i]);
                    SetDockContent(formList[i], DockType.Document);
                }
                else
                {
                    sideFormDict.Add(formList[i].Name, formList[i]);
                    SetDockContent(formList[i], DockType.Side);
                }
            
                int indextemp = Convert.ToInt32(formList[i].Name.Split(new char[] {' '})[1]);
                index = indextemp > index ? indextemp : index;
            }
            index++;
            DocumentDragged(dockPanel1, null);
        }


        private void ButtonClear_Click(object sender, EventArgs e)
        {
            ClearDockContents();
        }


        private void DocuClosing(object sender, FormClosingEventArgs e)
        {
            docuFormDict.Remove((sender as FrmDockContent).Name);
        }

        private void SideClosing(object sender, FormClosingEventArgs e)
        {
            sideFormDict.Remove((sender as FrmDockContent).Name);
        }


        /// <summary>
        /// <see cref="DockState"/>变化事件，但是如果在FloatWindow里面的窗口拖出来生成单独的窗口就不会触发该函数，没有最小最大按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DockStateChanged(object sender, EventArgs e)
        {
            FrmDockContent dc = sender as FrmDockContent;
            if (dc.DockState == DockState.Float)
            {
                ResetFloatWindow(dc.Pane.FloatWindow);
            }
        }

        /// <summary>
        /// 文档、四周窗口拖动后事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocumentDragged(object sender, EventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            List<FloatWindow> fwlist = dp.FloatWindows.ToList();
            for (int i = 0; i < fwlist.Count; ++i)
            {
                ResetFloatWindow(fwlist[i]);
            }
        }
    }
}
