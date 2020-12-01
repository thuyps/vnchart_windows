using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using xlib.framework;
using xlib.ui;

using stock123.app.data;

namespace stock123.app.ui
{
    public partial class DlgSellStock : Form
    {
        xIEventListener mListener;
        stGainloss mGainloss;

        public DlgSellStock(stGainloss g)
        {
            mGainloss = g;
            InitializeComponent();

            lb_Quote.Text = g.code;
            lb_Price.Text = "" + g.price;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            //mListener.onEvent(this, xBaseControl.EVT_CLOSE_DIALOG, Constants.ID_DLG_BUTTON_BACK, mID);
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void centerPosition(int w, int h)
        {
            Point pt = new Point((w-this.Size.Width)/2, (h-this.Size.Height)/2);
            this.SetBounds(pt.X, pt.Y, this.Size.Width, this.Size.Height);
            //this.Location = new Point(400, 400);
        }

        public int getVolume()
        {
            int r = 0;
            try
            {
                r = Int32.Parse(tb_Volume.Text);
            }
            catch (Exception e)
            {
            }

            return r;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}