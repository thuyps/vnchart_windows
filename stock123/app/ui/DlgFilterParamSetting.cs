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
            return string.Format("{0:F2}", t);
        }

        void setupUI()
        {
            int type = filterItem.type;
            label2.Hide();
            textBox2.Hide();
            label3.Visible = false;
            textBox3.Visible = false;
            //  Moving average
            if (type == FilterManager.SIGNAL_MA1_CUT_ABOVE_MA2
                || type == FilterManager.SIGNAL_MA3_CUT_ABOVE_MA4)
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

            if (type == FilterManager.SIGNAL_BUY_ADX_HIGHER_THAN)
            {
                label1.Text = "ADX lớn hơn:";
                textBox1.Text = floatToString(filterItem.param1);

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
                float param1 = float.Parse(textBox1.Text);
                float param2 = -1;
                if (textBox2.Visible)
                {
                    param2 = float.Parse(textBox2.Text);
                }
                if (textBox3.Visible){
                    filterItem.param3 = int.Parse(textBox3.Text);
                }

                filterItem.param1 = param1;
                filterItem.param2 = param2;
            }
            catch (Exception e1)
            {

            }
            
        }
    }
}