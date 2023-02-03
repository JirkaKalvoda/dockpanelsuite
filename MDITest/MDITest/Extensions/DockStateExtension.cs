using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace MDITest
{
    public static class DockStateExtension
    {
        /// <summary>
        /// 在原有<see cref="DockPanel"/>里并且显示出来的返回true，缩起来和在<see cref="FloatWindow"/>里的返回false
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool IsValid(this DockState state)
        {
            switch (state)
            {
                case DockState.Document:
                case DockState.DockLeft:
                case DockState.DockRight:
                case DockState.DockTop:
                case DockState.DockBottom:
                    return true;

                default:
                    return false;
            }
        }
    }
}
