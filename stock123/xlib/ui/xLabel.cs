using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.ui;
using xlib.framework;

namespace xlib.ui
{
    public class xLabel: xBaseControl
    {
        public xLabel()
            : base(null)
        {
        }
        public xLabel(string text):base(null)
        {
            Label l = new Label();
            l.Text = text;

            setControl(l);

            int w = utils.Utils.getStringW(text, l.Font);
            if (l.Size.Width < w)
                setSize(w, getH());
        }

        static public xLabel createSingleLabel(string text, Font f, int w)
        {
            xLabel l = new xLabel(text);

            if (f != null)
                l.setFont(f);

            if (w == -1)
            {
                w = utils.Utils.getStringW(text, f) + 40;   //  40??? don't know why
            }
            l.setSize(w, f.Height);

            return l;
        }

        static public xLabel createSingleLabel(string text, int w)
        {
            xLabel l = new xLabel(text);

            l.setSize(w, l.getFont().Height);

            return l;
        }

        static public xLabel createSingleLabel(string text)
        {
            xLabel l = new xLabel(text);

            l.setSize(utils.Utils.getStringW(text, l.getControl().Font), l.getFont().Height);

            return l;
        }

        static public xLabel createMultiLineLabel(string text, Font f, int w)
        {
            xLabel l = new xLabel();
            Label _l = new Label();

            //  calc height of text
            int tw = utils.Utils.getStringW(text, f) + 10;
            int lines = tw/w;
            if (lines < 1)
                lines = 1;
            if (tw - lines * w > 0) lines++;

            _l.Font = f;
            _l.Size = new Size(w, lines*f.Height);
            _l.Text = text;
            l.setControl(_l);

            return l;
        }

        static public xLabel createMultiLineLabel(string text, int w)
        {
            xLabel l = new xLabel();
            Label _l = new Label();

            _l.Size = new Size(w, _l.Size.Height);
            _l.Text = text;
            l.setControl(_l);

            return l;
        }

        public string getText()
        {
            return mControl.Text;
        }

        Label getLabel()
        {
            return (Label)getControl();
        }

        public void setAlign(int align)
        {
            if ((align & xGraphics.HCENTER) != 0)
            {
                Label l = (Label)getControl();
                l.TextAlign = ContentAlignment.MiddleCenter;
            }
            if ((align & xGraphics.RIGHT) != 0)
            {
                Label l = (Label)getControl();
                l.TextAlign = ContentAlignment.MiddleRight;
            }
        }

        public void enableClick(int id, xIEventListener listener)
        {
            setID(id);
            mListener = listener;

            Label l = (Label)getControl();
            l.Click += new EventHandler(onClick);
        }

        void onClick(object sender, EventArgs e)
        {
            if (mListener != null)
                mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, getID(), null);
        }

        Font getFont()
        {
            return getControl().Font;
        }
    }
}
