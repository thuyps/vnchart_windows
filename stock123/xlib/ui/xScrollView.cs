using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.ui;
using xlib.framework;

namespace xlib.ui
{
    class xScrollView: xBaseControl
    {
        Panel mPanel;
        public xScrollView(xIEventListener listener, int w, int h)
            : base(listener)
        {
            mPanel = new Panel();
            mPanel.AutoScroll = true;
            setControl(mPanel);

            setSize(w, h);
        }

        public void addControl(xBaseControl c)
        {
            mPanel.Controls.Add(c.getControl());
        }

        public override void invalidate()
        {
            base.invalidate();

            utils.Utils.trace("refresh xScrollView");

            for (int i = 0; i < mPanel.Controls.Count; i++)
            {
                Control c = mPanel.Controls[i];
                if (c.Tag is xBaseControl)
                {
                    xBaseControl xc = (xBaseControl)c.Tag;
                    xc.invalidate();
                }
            }
        }
    }
}
