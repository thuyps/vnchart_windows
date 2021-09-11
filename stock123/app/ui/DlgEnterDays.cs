using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using xlib.framework;
using xlib.ui;
using xlib.utils;

namespace stock123.app.ui
{
    //public delegate void OnClickDelegate(int param);

    public partial class DlgEnterDay : Form
    {
        int mID;
        int mResultID = 0;


        public DlgEnterDay(int id)
        {
            InitializeComponent();
            mID = id;
        }

        static public DlgEnterDay createDialog(String title, int days)
        {
            DlgEnterDay dlg = new DlgEnterDay(0);

            dlg.label1.Text = title;
            dlg.textBox1.Text = "" + days;

            return dlg;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            this.Close();
            mResultID = C.ID_DLG_BUTTON_BACK;

            DialogResult = DialogResult.Cancel;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();

            mResultID = C.ID_DLG_BUTTON_OK;

            DialogResult = DialogResult.OK;
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

        public int getDays()
        {
            return Utils.stringToInt(getText());
        }
    }
}