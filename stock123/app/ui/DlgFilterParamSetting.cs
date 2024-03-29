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
    public partial class DlgFilterParamSetting : Form
    {
        FilterItem filterItem;

        public DlgFilterParamSetting()
        {
            InitializeComponent();
        }

        public void setFilterItem(FilterItem item)
        {
            filterItem = item;

            setupUI();
        }

        string intToString(float t)
        {
            return string.Format("{0}", (int)t);
        }

        string floatToString(float t)
        {
            return string.Format("{0:F3}", t);
        }

        void setupUI()
        {
            int type = filterItem.type;
            label2.Hide();
            textBox2.Hide();
            label3.Visible = false;
            textBox3.Visible = false;

            //  RS
            if (type == FilterManager.SIGNAL_RS)
            {
                label1.Text = "MA1 (cắt trên MA2)";
                textBox1.Text = intToString((int)filterItem.param1);

                label2.Visible = true;
                textBox2.Visible = true;
                label2.Text = "MA2";
                textBox2.Text = intToString((int)filterItem.param2);

                label3.Visible = true;
                textBox3.Visible = true;
                label3.Text = "Trong khoảng ngày (0=bỏ qua)";
                textBox3.Text = intToString((int)filterItem.param3);
            }
            if (type == FilterManager.SIGNAL_STOCH_RSI1 || type == FilterManager.SIGNAL_STOCH_RSI2)
            {
                label1.Text = "LengthRSI/LengthStoch/SmoothK/SmoothD (ex:14/14/3/3)";
                int t = (int)filterItem.param1;
                int SmoothK = t&0xff;
                //int SmoothD = (t >> 24) & 0xf;
                int StochLen = (t >> 8) & 0xff;
                int RSI = (t >> 16) & 0xff;
                if (StochLen <= 0) StochLen = 14;
                if (SmoothK <= 0) SmoothK = 3;
                //if (SmoothD <= 0) SmoothD = 3;
                t = (int)filterItem.param2;
                int SmoothD = t & 0xff; 

                if (RSI == 0)
                {
                    RSI = 14;
                    StochLen = 14;
                    SmoothK = 3;
                    SmoothD = 3;
                }

                textBox1.Text = String.Format("{0}/{1}/{2}/{3}", RSI, StochLen, SmoothK, SmoothD);

                t = (int)filterItem.param3;
                int min = t & 0xff;
                int max = (t >> 8) & 0xff;

                if (min > max || min < 0 || max < 0 || min > 100 || max > 100)
                {
                    min = 0;
                    max = 100;
                }

                //  lon hon
                label2.Visible = true;
                textBox2.Visible = true;
                label2.Text = "StochRSI lớn hơn:";
                textBox2.Text = intToString(min);

                //  nho hon
                label3.Visible = true;
                textBox3.Visible = true;
                label3.Text = "Nhỏ hơn:";
                textBox3.Text = intToString(max);
            }
            //  RS performance
            if (type == FilterManager.SIGNAL_RS_PERFORMANCE)
            {
                label1.Text = "So sánh với giá của ngày trước đó";
                textBox1.Text = intToString((int)filterItem.param1);

                label2.Visible = true;
                textBox2.Visible = true;
                label2.Text = "RS cắt trên đường trung bình";
                textBox2.Text = intToString((int)filterItem.param2);

                label3.Visible = true;
                textBox3.Visible = true;
                label3.Text = "Trong khoảng ngày (bỏ qua)";
                textBox3.Text = intToString((int)filterItem.param3);
            }
            //  volume
            if (type == FilterManager.SIGNAL_VOLUME)
            {
                label1.Text = "Khối lượng trung bình";
                textBox1.Text = intToString((int)filterItem.param1);
            }
            //  prices
            if (type == FilterManager.SIGNAL_PRICE)
            {
                label1.Text = "Giá thấp nhất";
                textBox1.Text = intToString((int)filterItem.param1);

                label2.Visible = true;
                textBox2.Visible = true;
                label2.Text = "Giá cao nhất";
                textBox2.Text = intToString((int)filterItem.param2);
            }
            //  trade value
            if (type == FilterManager.SIGNAL_TRADE_VALUE)
            {
                label1.Text = "Giá trị giao dịch trung bình";
                textBox1.Text = intToString((int)filterItem.param1);
            }

            //  Moving average
            if (type == FilterManager.SIGNAL_MA1_CUT_ABOVE_MA2
                || type == FilterManager.SIGNAL_MA1_CUT_ABOVE_MA2_2
                || type == FilterManager.SIGNAL_MA1_CUT_ABOVE_MA2_3
                || type == FilterManager.SIGNAL_MA1_CUT_BELOW_MA2
                || type == FilterManager.SIGNAL_MA3_CUT_BELOW_MA4
                )
            {
                label1.Text = "MA 1";
                textBox1.Text = intToString(filterItem.param1);

                label2.Visible = true;
                textBox2.Visible = true;
                label2.Text = "MA 2";
                textBox2.Text = intToString(filterItem.param2);

                label3.Visible = true;
                label3.Text = "Trong khoảng ngày (0: tất cả)";
                textBox3.Visible = true;
                textBox3.Text = intToString(filterItem.param3);
            }

            //  dao dong gia
            if (type == FilterManager.SIGNAL_TOP_PRICE_UP
                || type == FilterManager.SIGNAL_TOP_PRICE_DOWN)
            {
                label1.Text = "Min (%)";
                textBox1.Text = intToString(filterItem.param1);

                label2.Visible = true;
                textBox2.Visible = true;
                label2.Text = "Max (%)";
                textBox2.Text = intToString(filterItem.param2);

                label3.Visible = true;
                label3.Text = "Trong khoảng ngày";
                textBox3.Visible = true;
                textBox3.Text = intToString(filterItem.param3);
            }

            if (type == FilterManager.SIGNAL_PRICE_FLUCTUATION)
            {
                label1.Text = "Khoảng dao động (%)";
                textBox1.Text = intToString(filterItem.param2);

                label2.Visible = true;
                textBox2.Visible = true;
                label2.Text = "Trong khoảng ngày";
                textBox2.Text = intToString(filterItem.param3);
            }

            if (type == FilterManager.SIGNAL_PSAR_TURNS_GREEN
                || type == FilterManager.SIGNAL_PSAR_TURNS_RED
                )
            {
                label1.Text = "Alpha";
                if (filterItem.param1 == 0)
                {
                    filterItem.param1 = Context.getInstance().mOptPSAR_alpha;
                }
                if (filterItem.param2 == 0)
                {
                    filterItem.param2 = Context.getInstance().mOptPSAR_alpha_max;
                }
                textBox1.Text = floatToString(filterItem.param1);

                label2.Visible = true;
                textBox2.Visible = true;
                label2.Text = "Alpha max";
                textBox2.Text = floatToString(filterItem.param2);

                //  days
                label3.Visible = true;
                textBox3.Visible = true;
                label3.Text = "Trong khoảng ngày (0=bỏ qua)";
                textBox3.Text = intToString(filterItem.param3);
            }

            if (type == FilterManager.SIGNAL_VSTOP_TURN_GREEN
                || type == FilterManager.SIGNAL_VSTOP_TURNS_RED)
            {
                if (filterItem.param1 == 0)
                {
                    filterItem.param1 = Context.getInstance().mOptVSTOP_ATR_Loopback;
                }
                if (filterItem.param2 == 0)
                {
                    filterItem.param2 = Context.getInstance().mOptVSTOP_MULT;
                }

                label1.Text = "Period";
                textBox1.Text = intToString(filterItem.param1);

                label2.Visible = true;
                textBox2.Visible = true;
                label2.Text = "Mult factor";
                textBox2.Text = floatToString(filterItem.param2);

                //  days
                label3.Visible = true;
                textBox3.Visible = true;
                label3.Text = "Trong khoảng ngày (0=bỏ qua)";
                textBox3.Text = intToString(filterItem.param3);
            }

            if (type == FilterManager.SIGNAL_BUY_ADX_HIGHER_THAN)
            {
                label1.Text = "ADX lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }
            if (type == FilterManager.SIGNAL_BUY_ADX_DIs_CROSS)
            {
                label1.Text = "DI+ cắt trên DI- trong số ngày:";
                textBox1.Text = intToString(filterItem.param1);

            }
            
            if (type == FilterManager.SIGNAL_FULLSTO_UP_AND_HIGHER_THAN)
            {
                label1.Text = "Full Stochartics lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }

            if (type == FilterManager.SIGNAL_FULLSTO_HIGHER_THAN)
            {
                label1.Text = "Full Stochartics lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }

            //  RSI
            if (type == FilterManager.SIGNAL_RSI_LOWER_THAN)
            {
                label1.Text = "RSI nhỏ hơn:";
                textBox1.Text = floatToString(filterItem.param1);

                label3.Visible = true;
                label3.Text = "Trong khoảng ngày (0: tất cả)";
                textBox3.Visible = true;
                textBox3.Text = intToString(filterItem.param3);
            }

            if (type == FilterManager.SIGNAL_RSI_HIGHER_THAN)
            {
                label1.Text = "RSI lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

                label3.Visible = true;
                label3.Text = "Trong khoảng ngày (0: tất cả)";
                textBox3.Visible = true;
                textBox3.Text = intToString(filterItem.param3);
            }

            if (type == FilterManager.SIGNAL_RSI_UP_AND_HIGHER_THAN)
            {
                label1.Text = "RSI tăng và lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }

            //  MFI
            if (type == FilterManager.SIGNAL_MFI_LOWER_THAN)
            {
                label1.Text = "MFI nhỏ hơn:";
                textBox1.Text = floatToString(filterItem.param1);

                label3.Visible = true;
                label3.Text = "Trong khoảng ngày (0: tất cả)";
                textBox3.Visible = true;
                textBox3.Text = intToString(filterItem.param3);
            }

            if (type == FilterManager.SIGNAL_MFI_HIGHER_THAN)
            {
                label1.Text = "MFI lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

                label3.Visible = true;
                label3.Text = "Trong khoảng ngày (0: tất cả)";
                textBox3.Visible = true;
                textBox3.Text = intToString(filterItem.param3);
            }

            if (type == FilterManager.SIGNAL_MFI_UP_AND_HIGHER_THAN)
            {
                label1.Text = "MFI tăng và lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }

            //  ROC
            if (type == FilterManager.SIGNAL_ROC_LOWER_THAN)
            {
                label1.Text = "ROC nhỏ hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }
            if (type == FilterManager.SIGNAL_ROC_HIGHER_THAN)
            {
                label1.Text = "ROC lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }

            if (type == FilterManager.SIGNAL_ROC_UP_AND_HIGHER_THAN)
            {
                label1.Text = "ROC lớn hơn: (%)";
                textBox1.Text = floatToString(filterItem.param1);

            }
            if (type == FilterManager.SIGNAL_ROC_MA1_CUT_ABOVE_MA2)
            {
                label1.Text = "MA 1";
                textBox1.Text = floatToString(filterItem.param1);

                label2.Visible = true;
                label2.Text = "MA 2";
                textBox2.Visible = true;
                textBox2.Text = floatToString(filterItem.param2);

                label3.Visible = true;
                label3.Text = "Cắt trong số ngày";
                textBox3.Visible = true;
                textBox3.Text = floatToString(filterItem.param3);
            }

            //  WilliamR
            if (type == FilterManager.SIGNAL_WILLIAM_HIGHER_THAN)
            {
                label1.Text = "WilliamR lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }

            if (type == FilterManager.SIGNAL_WILLIAM_LOWER_THAN)
            {
                label1.Text = "WilliamR nhỏ hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }

            if (type == FilterManager.SIGNAL_WILLIAM_UP_AND_HIGHER_THAN)
            {
                label1.Text = "WilliamR tăng và lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }

            if (type == FilterManager.SIGNAL_WILLIAM_CUT_SMA)
            {
                label1.Text = "SMA1:";
                textBox1.Text = intToString(filterItem.param1);

                label2.Text = "SMA2:";
                textBox2.Text = intToString(filterItem.param2);
                label2.Visible = true;
                textBox2.Visible = true;

                label3.Text = "Số ngày: (0 = bỏ qua)";
                textBox3.Text = intToString(filterItem.param3);
                label3.Visible = true;
                textBox3.Visible = true;
            }

            if (type == FilterManager.SIGNAL_WILLIAM_DOWN_AND_LOWER_THAN)
            {
                label1.Text = "WilliamR giảm và nhỏ hơn:";
                textBox1.Text = floatToString(filterItem.param1);

            }

            if (type == FilterManager.SIGNAL_PRICE_HIGHEST_IN)
            {
                label1.Text = "Giá cao nhất trong (X ngày):";
                textBox1.Text = intToString(filterItem.param1);
            }

            if (type == FilterManager.SIGNAL_TICHLUY)
            {
                label1.Text = "% dao động";
                textBox1.Text = intToString(filterItem.param1);

                label2.Visible = true;
                textBox2.Visible = true;
                label2.Text = "Số ngày";
                textBox2.Text = intToString(filterItem.param2);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            try
            {
                float param1 = 0;
                if (filterItem.type == FilterManager.SIGNAL_STOCH_RSI1 || filterItem.type == FilterManager.SIGNAL_STOCH_RSI2)
                {
                    String s = textBox1.Text;
                    String[] ss = s.Split('/');
                    if (ss.Length == 4)
                    {
                        int rsi = (int)float.Parse(ss[0]);
                        int stoch = (int)float.Parse(ss[1]);
                        int smoothK = (int)float.Parse(ss[2]);
                        int smoothD = (int)float.Parse(ss[3]);
                        //smoothD = smoothD & 0xf;

                        int v = (rsi << 16)|(stoch<<8)|smoothK;
                        filterItem.param1 = v;
                        filterItem.param2 = smoothD;
                        int min = (int)float.Parse(textBox2.Text);
                        int max = (int)float.Parse(textBox3.Text);
                        int t = (max<<8)|min;
                        filterItem.param3 = t;

                        return;
                    }
                }
                else{
                    param1 = float.Parse(textBox1.Text);
                }
                float param2 = -1;
                if (textBox2.Visible)
                {
                    param2 = float.Parse(textBox2.Text);
                }
                if (textBox3.Visible){
                    filterItem.param3 = (int)float.Parse(textBox3.Text);
                }

                filterItem.param1 = param1;
                filterItem.param2 = param2;
                
                //  exception for fluc
                if (filterItem.type == FilterManager.SIGNAL_PRICE_FLUCTUATION)
                {
                    filterItem.param2 = param1;
                    filterItem.param3 = (int)param2;
                }
            }
            catch (Exception e1)
            {

            }
            
        }
    }
}