using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using stock123.app.data;

namespace stock123.app.ui
{
    public partial class DlgBuyMore : Form
    {
        public DlgBuyMore(stGainloss g)
        {
            InitializeComponent();

            lb_Quote.Text = g.code;
        }

        private void DlgBuyMore_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public int getPrice()
        {
            int r = 0;
            try
            {
                r = Int32.Parse(tb_Price.Text);
            }
            catch (Exception e)
            {
            }

            return r;
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
    }
}