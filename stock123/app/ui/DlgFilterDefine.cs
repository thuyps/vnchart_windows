using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DlgFilterDefine : Form
    {
        int mType;
        int mIDX;
        Context mContext;
        public DlgFilterDefine()
        {
            InitializeComponent();

            mContext = Context.getInstance();
        }

        public void setFilter(int type, int idx)
        {
            mType = type;
            mIDX = idx;

            string name = (string)mContext.mSortTechnicalName[mType].elementAt(mIDX);
            int flags = mContext.mSortTechnicalParams[mType].elementAt(mIDX);

            tb_Name.Text = name;

            if ((flags & C._SORT_MACD_CUT_SIGNAL) != 0)
            {
                cb_MACD_Signal.Checked = true;
            }
            if ((flags & C._SORT_MACD_CONVERGENCY) != 0)
            {
                cb_MACDConvergency.Checked = true;
            }
            if ((flags & C._SORT_SLOW_STOCHASTIC_K_CUT_D) != 0)
            {
                cb_SlowStochastics_K_D.Checked = true;
            }
            if ((flags & C._SORT_TENKAN_CUT_KIJUN) != 0)
            {
                cb_Tenkan_Kijun.Checked = true;
            }
            if ((flags & C._SORT_RSI_CUT_SMA) != 0)
            {
                cb_RSI_SMA.Checked = true;
            }
            if ((flags & C._SORT_MFI_CUT_SMA) != 0)
            {
                cb_MFI_SMA.Checked = true;
            }
            if ((flags & C._SORT_ROC_CUT_SMA) != 0)
            {
                cb_ROC_SMA.Checked = true;
            }
            if ((flags & C._SORT_ADL_CUT_SMA) != 0)
            {
                cb_ADL_SMA.Checked = true;
            }
            if ((flags & C._SORT_ADX_CUT_DMIs) != 0)
            {
                cb_cut_DMIs.Checked = true;
            }
            if ((flags & C._SORT_BULLISH_NVI) != 0)
            {
                cb_NVI_Bullish.Checked = true;
            }
            if ((flags & C._SORT_RSI_HIGHER) != 0)
            {
                cb_RSI_higher.Checked = true;
            }
            if ((flags & C._SORT_MFI_HIGHER) != 0)
            {
                cb_MFI_higher.Checked = true;
            }
            if ((flags & C._SORT_ROC_HIGHER) != 0)
            {
                cb_ROC_higher.Checked = true;
            }
            if ((flags & C._SORT_PSAR_REVERT) != 0)
            {
                cb_PSAR_Bullish.Checked = true;
            }
            if ((flags & C._SORT_PRICE_ABOVE_KUMO) != 0)
            {
                cb_PriceAboveKumo.Checked = true;
            }
            if ((flags & C._SORT_VOLUME_IS_UP) != 0)
            {
                cb_VolumeUp.Checked = true;
            }
            if ((flags & C._SORT_ACCUMULATION) != 0)
            {
                cb_Accumulation.Checked = true;
            }

            //  SMA cut price
            if ((flags & C._SORT_SMA_PRICE_5) != 0)
            {
                cbSMACutPrice5.Checked = true;
            }
            if ((flags & C._SORT_SMA_PRICE_9) != 0)
            {
                cbSMACutPrice9.Checked = true;
            }
            if ((flags & C._SORT_SMA_PRICE_12) != 0)
            {
                cbSMACutPrice12.Checked = true;
            }
            if ((flags & C._SORT_SMA_PRICE_26) != 0)
            {
                cbSMACutPrice26.Checked = true;
            }
            if ((flags & C._SORT_SMA_PRICE_50) != 0)
            {
                cbSMACutPrice50.Checked = true;
            }
            if ((flags & C._SORT_SMA_PRICE_100) != 0)
            {
                cbSMACutPrice100.Checked = true;
            }

            tb_SMA1.Text = "" + mContext.mOptFilterSMA1;
            tb_SMA2.Text = "" + mContext.mOptFilterSMA2;

            if ((flags & C._SORT_SMA1_CUT_SMA2) != 0)
            {
                cb_SMA1_SMA2.Checked = true;
                lb_SMA1.Enabled = true;
                lb_SMA2.Enabled = true;
                tb_SMA1.Enabled = true;
                tb_SMA2.Enabled = true;
            }
            else
            {
                cb_SMA1_SMA2.Checked = false;
                lb_SMA1.Enabled = false;
                lb_SMA2.Enabled = false;
                tb_SMA1.Enabled = false;
                tb_SMA2.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Advance_Click(object sender, EventArgs e)
        {
            DlgFilterConfig dlg = new DlgFilterConfig();
            dlg.ShowDialog();
        }

        private void bt_Back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            int flags = 0;
            if (cb_MACD_Signal.Checked)
            {
                flags |= C._SORT_MACD_CUT_SIGNAL;
            }
            if (cb_MACDConvergency.Checked)
            {
                flags |= C._SORT_MACD_CONVERGENCY;
            }
            if (cb_SlowStochastics_K_D.Checked)
            {
                flags |= C._SORT_SLOW_STOCHASTIC_K_CUT_D;
            }
            if (cb_Tenkan_Kijun.Checked)
            {
                flags |= C._SORT_TENKAN_CUT_KIJUN;
            }
            if (cb_RSI_SMA.Checked)
            {
                flags |= C._SORT_RSI_CUT_SMA;
            }
            if (cb_MFI_SMA.Checked)
            {
                flags |= C._SORT_MFI_CUT_SMA;
            }
            if (cb_ADL_SMA.Checked)
            {
                flags |= C._SORT_ADL_CUT_SMA;
            }
            if (cb_ROC_SMA.Checked)
            {
                flags |= C._SORT_ROC_CUT_SMA;
            }
            if (cb_cut_DMIs.Checked)
            {
                flags |= C._SORT_ADX_CUT_DMIs;
            }
            if (cb_NVI_Bullish.Checked)
            {
                flags |= C._SORT_BULLISH_NVI;
            }
            if (cb_RSI_higher.Checked)
            {
                flags |= C._SORT_RSI_HIGHER;
            }
            if (cb_MFI_higher.Checked)
            {
                flags |= C._SORT_MFI_HIGHER;
            }
            if (cb_ROC_higher.Checked)
            {
                flags |= C._SORT_ROC_HIGHER;
            }
            if (cb_PSAR_Bullish.Checked)
            {
                flags |= C._SORT_PSAR_REVERT;
            }
            if (cb_PriceAboveKumo.Checked)
            {
                flags |= C._SORT_PRICE_ABOVE_KUMO;
            }
            if (cb_VolumeUp.Checked)
            {
                flags |= C._SORT_VOLUME_IS_UP;
            }
            if (cb_Accumulation.Checked)
            {
                flags |= C._SORT_ACCUMULATION;
            }
            if (cb_SMA1_SMA2.Checked)
            {
                flags |= C._SORT_SMA1_CUT_SMA2;
                try
                {
                    int sma1 = Int32.Parse(tb_SMA1.Text);
                    int sma2 = Int32.Parse(tb_SMA2.Text);
                    mContext.mOptFilterSMA1 = sma1;
                    mContext.mOptFilterSMA2 = sma2;

                    mContext.saveOptions();
                }
                catch (Exception ex)
                {
                }
            }

            //  SMA
            if (cbSMACutPrice5.Checked)
            {
                flags |= C._SORT_SMA_PRICE_5;
            }
            if (cbSMACutPrice9.Checked)
            {
                flags |= C._SORT_SMA_PRICE_9;
            }
            if (cbSMACutPrice12.Checked)
            {
                flags |= C._SORT_SMA_PRICE_12;
            }
            if (cbSMACutPrice26.Checked)
            {
                flags |= C._SORT_SMA_PRICE_26;
            }
            if (cbSMACutPrice50.Checked)
            {
                flags |= C._SORT_SMA_PRICE_50;
            }
            if (cbSMACutPrice100.Checked)
            {
                flags |= C._SORT_SMA_PRICE_100;
            }
            //==================================

            string old_name = (string)mContext.mSortTechnicalName[mType].elementAt(mIDX);
            int old_flags = mContext.mSortTechnicalParams[mType].elementAt(mIDX);

            if ((old_name.CompareTo(tb_Name.Text) == 0) && old_flags == flags)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                DialogResult = DialogResult.OK;

                mContext.mSortTechnicalName[mType].insertElementAt(tb_Name.Text, mIDX);
                mContext.mSortTechnicalParams[mType].insertElementAt(flags, mIDX);

                mContext.mSortTechnicalName[mType].removeElementAt(mIDX + 1);
                mContext.mSortTechnicalParams[mType].removeElementAt(mIDX + 1);
            }

            this.Close();
        }

        private void cb_SMA1_SMA2_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = cb_SMA1_SMA2.Checked;

            lb_SMA1.Enabled = enable;
            lb_SMA2.Enabled = enable;
            tb_SMA1.Enabled = enable;
            tb_SMA2.Enabled = enable;
        }

        private void cb_NVI_Bullish_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cb_RSI_higher_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}