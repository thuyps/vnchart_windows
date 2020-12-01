using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

using xlib.framework;

namespace xlib.ui
{
    public class xContainer: xBaseControl
    {
        ContainerControl mContainer;
        public xContainer(xIEventListener listener)
            : base(listener)
        {
            mContainer = new ContainerControl();
            setControl(mContainer);
        }

        public xContainer()
            : base(null)
        {
            mContainer = new ContainerControl();
            setControl(mContainer);
        }

        public void addControl(xContainer c)
        {
            mContainer.Controls.Add(c.getControl());
        }

        public void addControl(Control c)
        {
            mContainer.Controls.Add(c);
        }

        public void addControl(xBaseControl c)
        {
            mContainer.Controls.Add(c.getControl());
        }
        public void removeControl(xBaseControl c)
        {
            if (c != null)
                mContainer.Controls.Remove(c.getControl());
        }

        public void removeAllControls()
        {
            mContainer.Controls.Clear();
        }

        public int getItemCount()
        {
            return mContainer.Controls.Count;
        }

        public xBaseControl getControlAt(int idx)
        {
            if (idx < mContainer.Controls.Count)
            {
                Control c = mContainer.Controls[idx];
                if (c.Tag is xBaseControl)
                    return (xBaseControl)c.Tag;
            }

            return null;
        }

        public bool contains(xBaseControl c)
        {
            Control cc = c.getControl();
            return mContainer.Controls.Contains(cc);
        }

        virtual public void onSizeChanged()
        {
        }

        override public void dispose()
        {
            int cnt = getItemCount();
            for (int i = 0; i < cnt; i++)
            {
                Control c = mContainer.Controls[i];
                if (c.Tag is xBaseControl)
                {
                    xBaseControl xc = (xBaseControl)c.Tag;
                    xc.dispose();
                }
            }

        }

        public override void invalidate()
        {
            base.invalidate();

            int cnt = getItemCount();
            for (int i = 0; i < cnt; i++)
            {
                Control c = mContainer.Controls[i];
                if (c.Tag is xBaseControl)
                {
                    xBaseControl xc = (xBaseControl)c.Tag;
                    xc.invalidate();
                }
            }
        }
    }
}
