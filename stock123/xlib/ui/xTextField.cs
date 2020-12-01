using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using xlib.framework;

namespace xlib.ui
{
    public class xTextField: xBaseControl
    {
        public xTextField(xIEventListener listener, int w, bool multiLine)
            : base(listener)
        {
            TextBox tb = new TextBox();

            if (multiLine)
            {
                tb.ScrollBars = ScrollBars.Vertical;
                tb.WordWrap = true;
            }

            setControl(tb);
        }

        static public xTextField createTextField(int w)
        {
            xTextField tf = new xTextField(null, w, false);
            tf.setSize(w, tf.getH());

            return tf;
        }

        static public xTextField createTextField(int w, int h)
        {
            xTextField tf = new xTextField(null, w, true);
            tf.setSize(w, h);

            return tf;
        }

        public void setText(String text)
        {
            TextBox tb = (TextBox)getControl();
            tb.Text = text;
        }

        public String getText()
        {
            TextBox tb = (TextBox)getControl();
            return tb.Text;
        }

        public void setButtonEvent(int id, xIEventListener listener)
        {
            TextBox tb = (TextBox)getControl();
            //tb.KeyUp += new KeyEventHandler(tb_KeyUp);
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);

            mListener = listener;
            mID = id;
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (mListener != null)
                {
                    mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, mID, null);
                }
            }
        }

        void tb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                TextBox tb = (TextBox)getControl();

                if (mListener != null)
                {
                    mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, mID, null);
                }
            }
        }
    }
}
