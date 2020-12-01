using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DlgFilterConfig : Form
    {
        Context mContext;
        public DlgFilterConfig()
        {
            mContext = Context.getInstance();
            InitializeComponent();

            this.cb_KLTB.Checked = mContext.mOptFilterKLTB30Use;
            this.tb_GTTB.Text = "" + mContext.mOptFilterGTGD;

            this.cb_HiPrice.Checked = mContext.mOptFilterHiPriceUse;
            this.tb_HiPrice.Text = "" + mContext.mOptFilterHiPrice;

            this.cbLoPrice.Checked = mContext.mOptFilterLowPriceUse;
            this.tb_LoPrice.Text = "" + mContext.mOptFilterLowPrice;

            //this.cbVnindex.Checked = mContext.mOptFilterVNIndex;
            //this.cbHASTC.Checked = mContext.mOptFilterHNX;
        }

        private void DlgFilterConfig_Load(object sender, EventArgs e)
        {

        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            try
            {
                mContext.mOptFilterKLTB30Use = this.cb_KLTB.Checked;
                mContext.mOptFilterGTGD = (int)UInt32.Parse(this.tb_GTTB.Text);

                mContext.mOptFilterHiPriceUse = this.cb_HiPrice.Checked;
                mContext.mOptFilterHiPrice = (int)UInt32.Parse(this.tb_HiPrice.Text);

                mContext.mOptFilterLowPriceUse = this.cbLoPrice.Checked;
                mContext.mOptFilterLowPrice = (int)UInt32.Parse(this.tb_LoPrice.Text);

                //mContext.mOptFilterVNIndex = this.cbVnindex.Checked;
                //mContext.mOptFilterHNX = this.cbHASTC.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            this.Close();
        }
    }
}