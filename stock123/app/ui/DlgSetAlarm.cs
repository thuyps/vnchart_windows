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
using stock123.app.data;
using stock123.app.chart;

namespace stock123.app.ui
{
    public partial class DlgSetAlarm : Form, xIEventListener
    {
        stAlarm mAlarm;
        Context mContext;
        HistoryChartControl mChart;
        public DlgSetAlarm(stAlarm a)
        {
            InitializeComponent();

            mAlarm = a;
            this.Text = "Alarm: " + a.code;

            if (a.upperPrice == 0)
            {
                cb_Upper.Checked = false;
                tb_Upper.Enabled = false;
            }
            else
            {
                cb_Upper.Checked = true;
                tb_Upper.Text = "" + (a.upperPrice);
            }
            if (a.lowerPrice == 0)
            {
                cb_Lower.Checked = false;
                tb_Lower.Enabled = false;
            }
            else
            {
                cb_Lower.Checked = true;
                tb_Lower.Text = "" + (a.lowerPrice);
            }

            tb_comment.Text = a.comment;

            //================================
            mContext = Context.getInstance();
            mContext.setCurrentShare(a.code);
            if (mContext.getSelectedDrawableShare() != null)
            {
                if (!mContext.getSelectedDrawableShare().loadShareFromFile(true))
                    mContext.getSelectedDrawableShare().loadShareFromCommonData(true);
            }
            //================================
            lb_UpperPercent.Text = "-";
            lb_LowerPercent.Text = "-";
            updatePercentLabel(mAlarm.lowerPrice, lb_LowerPercent);
            updatePercentLabel(mAlarm.upperPrice, lb_UpperPercent);
            //================================
            HistoryChartControl chart = new HistoryChartControl("panel1", panel_Chart.Width, panel_Chart.Height, false);
            chart.setListener(this);
            mChart = chart;

            panel_Chart.Controls.Add(chart.getControl());
        }

        private void Back_Click(object sender, EventArgs e)
        {
            //mListener.onEvent(this, xBaseControl.EVT_CLOSE_DIALOG, Constants.ID_DLG_BUTTON_BACK, mID);
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            //mListener.onEvent(this, xBaseControl.EVT_CLOSE_DIALOG, Constants.ID_DLG_BUTTON_OK, mID);
            mAlarm.comment = tb_comment.Text;

            this.Close();

            this.DialogResult = DialogResult.OK;
        }

        public int getLowerPrice()
        {
            try
            {
                if (cb_Lower.Checked)
                    return Int32.Parse(tb_Lower.Text);
            }
            catch (Exception e)
            {
            }

            return 0;
        }

        public int getUpperPrice()
        {
            try
            {
                if (cb_Upper.Checked)
                    return Int32.Parse(tb_Upper.Text);
            }
            catch (Exception e)
            {
            }

            return 0;
        }

        private void cb_Upper_CheckedChanged(object sender, EventArgs e)
        {
            tb_Upper.Enabled = cb_Upper.Checked;
        }

        private void cb_Lower_CheckedChanged(object sender, EventArgs e)
        {
            tb_Lower.Enabled = cb_Lower.Checked;
        }

        private void DlgSetAlarm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Context.getInstance().mAlarmManager.removeAlarm(mAlarm);
            Context.getInstance().mAlarmManager.saveAlarms();

            this.Close();
        }

        public void onEvent(object sender, int evt, int aIntParameter, object aParameter)
        {
            if (evt == C.EVT_REPAINT_CHARTS)
            {
                mChart.invalidateCharts();
            }
        }

        private void tb_Upper_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int price = Int32.Parse(tb_Upper.Text);
                updatePercentLabel(price, lb_UpperPercent);
            }
            catch (Exception ex)
            {
            }
        }

        private void tb_Lower_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int price = Int32.Parse(tb_Lower.Text);

                updatePercentLabel(price, lb_LowerPercent);
            }
            catch (Exception ex)
            {
            }
        }

        void updatePercentLabel(int price, Label l)
        {
            stPriceboardState ps = mContext.mPriceboard.getPriceboard(mAlarm.code);
            if (ps != null && price > 0)
            {
                float percent = (float)(price - ps.getCurrentPrice()) / ps.getCurrentPrice();
                percent *= 100;

                StringBuilder sb = Utils.getSB();
                if (percent > 0)
                    sb.AppendFormat("+{0:F1} %", percent);
                else
                    sb.AppendFormat("{0:F1} %", percent);

                l.Text = sb.ToString();
            }
        }
    }
}