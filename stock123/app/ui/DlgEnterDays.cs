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
    public partial class DlgEnterDay : Form
    {
        int mID;
        xIEventListener mListener;
        int mResultID = 0;
        public DlgEnterDay(int id, xIEventListener listener)
        {
            InitializeComponent();
            mID = id;
            mListener = listener;
        }

        static public DlgEnterDay createDialog(xIEventListener listener, String label)
        {
            DlgEnterDay dlg = new DlgEnterDay(0, listener);
            dlg.label1.Text = label;

            return dlg;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            this.Close();
            mResultID = C.ID_DLG_BUTTON_BACK;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();

            mResultID = C.ID_DLG_BUTTON_OK;
        }

        public void centerPosition(int w, int h)
        {
            Point pt = new Point((w-this.Size.Width)/2, (h-this.Size.Height)/2);
            this.SetBounds(pt.X, pt.Y, this.Size.Width, this.Size.Height);
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