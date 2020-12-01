using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DlgSMA : Form
    {
        Context mContext;
        uint[] colors = { 0xffff0000, 0xff00ff00, 0xff0000ff};
        float[] thinkness = { 1.0f, 1.0f, 1.0f};
        bool[] smas = {true, true, false};
        int id;

        public DlgSMA(int idx)
        {
            id = idx;
            mContext = Context.getInstance();
            InitializeComponent();

            this.cb_SMA1.Checked = mContext.mOptSMAUse[id,0];
            this.cb_SMA2.Checked = mContext.mOptSMAUse[id,1];
            this.cb_SMA3.Checked = mContext.mOptSMAUse[id,2];
            this.cb_SMA4.Checked = mContext.mOptSMAUse2[id, 0];
            this.cb_SMA5.Checked = mContext.mOptSMAUse2[id, 1];

            this.cb_Thickness1.Text = "" + mContext.mOptSMAThickness[id, 0];
            this.cb_Thickness2.Text = "" + mContext.mOptSMAThickness[id, 1];
            this.cb_Thickness3.Text = "" + mContext.mOptSMAThickness[id, 2];
            this.cb_Thickness4.Text = "" + mContext.mOptSMAThickness2[id, 0];
            this.cb_Thickness5.Text = "" + mContext.mOptSMAThickness2[id, 1];

            this.tb_Days1.Text = "" + mContext.mOptSMA[id, 0];
            this.tb_Days2.Text = "" + mContext.mOptSMA[id, 1];
            this.tb_Days3.Text = "" + mContext.mOptSMA[id, 2];
            this.tb_Days4.Text = "" + mContext.mOptSMA2[id, 0];
            this.tb_Days5.Text = "" + mContext.mOptSMA2[id, 1];

            this.bt_Color1.BackColor = Color.FromArgb((int)mContext.mOptSMAColor[id, 0]);
            this.bt_Color2.BackColor = Color.FromArgb((int)mContext.mOptSMAColor[id, 1]);
            this.bt_Color3.BackColor = Color.FromArgb((int)mContext.mOptSMAColor[id, 2]);
            this.bt_Color4.BackColor = Color.FromArgb((int)mContext.mOptSMAColor2[id, 0]);
            this.bt_Color5.BackColor = Color.FromArgb((int)mContext.mOptSMAColor2[id, 1]);

            rb_SMA.Checked = mContext.mIsSMA[id];
            rb_EMA.Checked = !mContext.mIsSMA[id];

            updateItems();
        }

        void updateItems()
        {
            {
                this.cb_Thickness1.Enabled = mContext.mOptSMAUse[id, 0];
                this.tb_Days1.Enabled = mContext.mOptSMAUse[id, 0];
                this.bt_Color1.Enabled = mContext.mOptSMAUse[id, 0];
            }

            {
                this.cb_Thickness2.Enabled = mContext.mOptSMAUse[id, 1];
                this.tb_Days2.Enabled = mContext.mOptSMAUse[id, 1];
                this.bt_Color2.Enabled = mContext.mOptSMAUse[id, 1];
            }

            {
                this.cb_Thickness3.Enabled = mContext.mOptSMAUse[id, 2];
                this.tb_Days3.Enabled = mContext.mOptSMAUse[id, 2];
                this.bt_Color3.Enabled = mContext.mOptSMAUse[id, 2];
            }

            {
                this.cb_Thickness4.Enabled = mContext.mOptSMAUse2[id, 0];
                this.tb_Days4.Enabled = mContext.mOptSMAUse2[id, 0];
                this.bt_Color4.Enabled = mContext.mOptSMAUse2[id, 0];
            }

            {
                this.cb_Thickness5.Enabled = mContext.mOptSMAUse2[id, 1];
                this.tb_Days5.Enabled = mContext.mOptSMAUse2[id, 1];
                this.bt_Color5.Enabled = mContext.mOptSMAUse2[id, 1];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color c = colorDialog1.Color;
                bt_Color1.BackColor = c;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            mContext.mOptSMAUse[id, 0] = this.cb_SMA1.Checked;
            mContext.mOptSMAUse[id, 1] = this.cb_SMA2.Checked;
            mContext.mOptSMAUse[id, 2] = this.cb_SMA3.Checked;
            mContext.mOptSMAUse2[id, 0] = this.cb_SMA4.Checked;
            mContext.mOptSMAUse2[id, 1] = this.cb_SMA5.Checked;

            try
            {
                mContext.mOptSMAThickness[id, 0] = float.Parse(this.cb_Thickness1.Text);
                mContext.mOptSMAThickness[id, 1] = float.Parse(this.cb_Thickness2.Text);
                mContext.mOptSMAThickness[id, 2] = float.Parse(this.cb_Thickness3.Text);
                mContext.mOptSMAThickness2[id, 0] = float.Parse(this.cb_Thickness4.Text);
                mContext.mOptSMAThickness2[id, 1] = float.Parse(this.cb_Thickness5.Text);

                mContext.mOptSMA[id, 0] = int.Parse(this.tb_Days1.Text);
                mContext.mOptSMA[id, 1] = int.Parse(this.tb_Days2.Text);
                mContext.mOptSMA[id, 2] = int.Parse(this.tb_Days3.Text);
                mContext.mOptSMA2[id, 0] = int.Parse(this.tb_Days4.Text);
                mContext.mOptSMA2[id, 1] = int.Parse(this.tb_Days5.Text);

                mContext.mOptSMAColor[id, 0] = (uint)this.bt_Color1.BackColor.ToArgb();
                mContext.mOptSMAColor[id, 1] = (uint)this.bt_Color2.BackColor.ToArgb();
                mContext.mOptSMAColor[id, 2] = (uint)this.bt_Color3.BackColor.ToArgb();
                mContext.mOptSMAColor2[id, 0] = (uint)this.bt_Color4.BackColor.ToArgb();
                mContext.mOptSMAColor2[id, 1] = (uint)this.bt_Color5.BackColor.ToArgb();
            }
            catch (Exception ex)
            {
            }


            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void DlgSMA_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bt_Color2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color c = colorDialog1.Color;
                bt_Color2.BackColor = c;
            }
        }

        private void cb_SMA1_CheckedChanged(object sender, EventArgs e)
        {
            mContext.mOptSMAUse[id, 0] = this.cb_SMA1.Checked;
            updateItems();
        }

        private void cb_SMA2_CheckedChanged(object sender, EventArgs e)
        {
            mContext.mOptSMAUse[id, 1] = this.cb_SMA2.Checked;
            updateItems();
        }

        private void cb_SMA3_CheckedChanged(object sender, EventArgs e)
        {
            mContext.mOptSMAUse[id, 2] = this.cb_SMA3.Checked;
            updateItems();
        }

        private void bt_Color1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color c = colorDialog1.Color;
                bt_Color1.BackColor = c;
            }
        }

        private void bt_Color3_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color c = colorDialog1.Color;
                bt_Color3.BackColor = c;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_SMA.Checked)
            {
                rb_EMA.Checked = false;
                mContext.mIsSMA[id] = true;
            }
        }

        private void rb_EMA_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_EMA.Checked)
            {
                rb_SMA.Checked = false;
                mContext.mIsSMA[id] = false;
            }
        }

        private void bt_Color4_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color c = colorDialog1.Color;
                bt_Color4.BackColor = c;
            }
        }

        private void cb_SMA4_CheckedChanged(object sender, EventArgs e)
        {
            mContext.mOptSMAUse2[id, 0] = this.cb_SMA4.Checked;
            updateItems();
        }

        private void cb_SMA5_CheckedChanged(object sender, EventArgs e)
        {
            mContext.mOptSMAUse2[id, 1] = this.cb_SMA5.Checked;
            updateItems();
        }

        private void bt_Color5_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color c = colorDialog1.Color;
                bt_Color5.BackColor = c;
            }
        }
    }
}