using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DlgConfig : Form
    {
        Context mContext;
        public const int RESULT_OK = 0;
        public const int RESULT_RELOAD = 1;
        public const int RESULT_EXPAND_ACCOUNT = 2;
        public const int RESULT_CHANGE_PASSWORD = 3;
        public const int RESULT_RESET_PASSWORD = 4;
        public int mDialogResult = RESULT_OK;

        public DlgConfig()
        {
            mContext = Context.getInstance();
            InitializeComponent();

            try
            {
                cb_Autologin.Checked = mContext.mOptAutologin;
                //cb_AutorunAtStartup.Checked = mContext.mOptAutorunAtStartup;
                cb_Gainloss.Checked = mContext.mOptDontUseGainloss;
                checkBox1.Checked = mContext.mHasPreviewChart;

                lb_RemainingDays.Text = "Tài khoản bạn còn " + mContext.mDaysLeft + " ngày";

                trackBarFont.Value = mContext.mFontAdjust;
            }catch(Exception e){
                
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            mDialogResult = RESULT_CHANGE_PASSWORD;
        }

        private void bt_ReloadSystem_Click(object sender, EventArgs e)
        {
            this.Close();
            mDialogResult = RESULT_RELOAD;
        }

        private void cb_Autologin_CheckedChanged(object sender, EventArgs e)
        {
            mContext.mOptAutologin = cb_Autologin.Checked;
            mContext.saveOptions();
        }

        private void bt_ExpandAccount_Click(object sender, EventArgs e)
        {
            this.Close();
            mDialogResult = RESULT_EXPAND_ACCOUNT;
        }

        private void bt_ForgotPass_Click(object sender, EventArgs e)
        {
            this.Close();
            mDialogResult = RESULT_RESET_PASSWORD;
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            mContext.saveOptions();

            this.Close();
            mDialogResult = RESULT_OK;
        }
        /*
        private void cb_AutorunAtStartup_CheckedChanged(object sender, EventArgs e)
        {
            mContext.mOptAutorunAtStartup = cb_AutorunAtStartup.Checked;
            mContext.saveOptions();
        }
         */

        private void cb_Gainloss_CheckedChanged(object sender, EventArgs e)
        {
            mContext.mOptDontUseGainloss = cb_Gainloss.Checked;
            mContext.saveOptions();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            mContext.mHasPreviewChart = checkBox1.Checked;
            mContext.saveOptions2();
        }

        private void onFontChanged(object sender, EventArgs e)
        {
            mContext.mFontAdjust = trackBarFont.Value;

            mContext.saveOptions2();
        }
    }
}