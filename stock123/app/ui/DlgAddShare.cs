using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using xlib.framework;
using xlib.ui;

namespace stock123.app.ui
{
    public partial class DlgAddShare : Form
    {
        int mID;
        xIEventListener mListener;
        int mResultID = 0;
        public DlgAddShare(int id, xIEventListener listener)
        {
            InitializeComponent();
            mID = id;
            mListener = listener;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            //mListener.onEvent(this, xBaseControl.EVT_CLOSE_DIALOG, Constants.ID_DLG_BUTTON_BACK, mID);
            this.Close();
            mResultID = C.ID_DLG_BUTTON_BACK;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            //mListener.onEvent(this, xBaseControl.EVT_CLOSE_DIALOG, Constants.ID_DLG_BUTTON_OK, mID);
            this.Close();

            mResultID = C.ID_DLG_BUTTON_OK;
        }

        public void centerPosition(int w, int h)
        {
            Point pt = new Point((w-this.Size.Width)/2, (h-this.Size.Height)/2);
            this.SetBounds(pt.X, pt.Y, this.Size.Width, this.Size.Height);
            //this.Location = new Point(400, 400);
        }

        public int getResultID()
        {
            return mResultID;
        }

        public String getText()
        {
            return textBox1.Text;
        }
    }
}