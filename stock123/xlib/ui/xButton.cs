using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.framework;

namespace xlib.ui
{
    public class xButton: xBaseControl
    {
        public xButton(xIEventListener listener):base(listener)
        {
            Button bt = new Button();
            bt.Click += new EventHandler(onClick);
            //bt.FlatStyle = FlatStyle.Flat;
            setControl(bt);
        }

        public xButton(Button bt, int id, xIEventListener listener):base(listener)
        {
            bt.Click += new EventHandler(onClick);
            setControl(bt);
            setID(id);
        }

        public static xButton createStandardButton(int id, xIEventListener listener, string text, int w)
        {
            xButton bt = new xButton(listener);
            bt.setText(text);
            if (w != -1)
                bt.setSize(w, -1);

            bt.setID(id);

            return bt;
        }

        public static xButton createImageButton(int id, xIEventListener listener, ImageList imglist, int imgIndex)
        {
            Button bt = new Button();
            bt.ImageList = imglist;
            bt.ImageIndex = imgIndex;

            xButton xbt = new xButton(bt, id, listener);
            xbt.setSize(imglist.ImageSize.Width + 2, imglist.ImageSize.Height + 2);
            //xbt.makeFlat();

            return xbt;
        }

        public void makeFlat()
        {
            ((Button)getControl()).FlatStyle = FlatStyle.Flat;
        }

        void onClick(object sender, EventArgs arg)
        {
            mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, mID, null);
        }

        Button getButton()
        {
            return (Button)getControl();
        }
    }
}
