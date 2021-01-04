using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using xlib.framework;

namespace xlib.ui
{
    public class xTabPage: xBaseControl
    {
        public Object userData;
        public Object userData2;

        public xTabPage(string title)
            : base(null)
        {
            TabPage page = new TabPage(title);
            page.Tag = this;
            setControl(page);
        }

        public void addControl(xBaseControl c)
        {
            TabPage page = (TabPage)getControl();
            page.Controls.Add(c.getControl());
        }

        public void addControl(Control c)
        {
            TabPage page = (TabPage)getControl();
            page.Controls.Add(c);
        }

        public void resetContent()
        {
            TabPage page = (TabPage)getControl();
            page.Controls.Clear();
        }

    }
}
