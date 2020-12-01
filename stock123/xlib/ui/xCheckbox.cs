using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using xlib.framework;

namespace xlib.ui
{
    public class xCheckbox: xBaseControl
    {
        bool[] mValue;
        public xCheckbox(String text, bool[] o, xIEventListener listener): base(listener)
        {
            CheckBox cb = new CheckBox();
            setControl(cb);
            cb.Text = text;

            setCheck(o[0]);

            cb.CheckedChanged += new EventHandler(cb_CheckedChanged);

            mValue = o;
        }

        public void setTextAlign(ContentAlignment align)
        {
            CheckBox cb = (CheckBox)getControl();
            cb.TextAlign = align;
        }

        void cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)getControl();
            if (cb.CheckState == CheckState.Checked)
            {
                mValue[0] = true;
            }
            else
            {
                mValue[0] = false;
            }

            if (mListener != null)
                mListener.onEvent(this, xBaseControl.EVT_CHECKBOX_VALUE_CHANGED, 0, null);
        }

        static public xCheckbox createCheckbox(String text, bool[] o, xIEventListener listener, int w)
        {
            xCheckbox cb = new xCheckbox(text, o, listener);
            cb.setSize(w, cb.getH());
            return cb;
        }

        public void setCheck(bool check)
        {
            CheckBox cb = (CheckBox)getControl();
            if (check)
            {
                cb.CheckState = CheckState.Checked;
            }
            else
            {
                cb.CheckState = CheckState.Unchecked;
            }
        }

        public bool isCheck()
        {
            CheckBox cb = (CheckBox)getControl();
            return cb.Checked;
        }

        void setOutputReturn(bool[] v) 
        { 
            mValue = v; 
        }
    }
}
