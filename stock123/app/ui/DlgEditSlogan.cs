using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DlgEditSlogan : Form
    {
        Context mContext;

        public DlgEditSlogan()
        {
            mContext = Context.getInstance();
            InitializeComponent();

            btTextColor.BackColor = Color.FromArgb((int)mContext.mSloganColor);
            btBackColor.BackColor = Color.FromArgb((int)mContext.mSloganColorBG);

            tbSloganText.Text = mContext.mSloganText;
            tbSloganText.ForeColor = btTextColor.BackColor;
            tbSloganText.BackColor = btBackColor.BackColor;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void bt_OK_Click(object sender, EventArgs e)
        {
            mContext.mSloganText = tbSloganText.Text;

            mContext.saveProfile();

            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                mContext.mSloganColorBG = (uint)colorDialog1.Color.ToArgb();
                btBackColor.BackColor = colorDialog1.Color;
            }
        }

        private void btTextFont_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowColor = false;
            fontDialog1.Font = mContext.getFontSlogan();
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                Font f = fontDialog1.Font;
                mContext.mSloganFont = f.Name;
                mContext.mSloganFontSizeInPoint = f.SizeInPoints;
                mContext.mSloganFontStrikeout = f.Strikeout;
                mContext.mSloganFontStyle = f.Style;
            }
        }

        private void btTextColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                mContext.mSloganColor = (uint)colorDialog1.Color.ToArgb();

                btTextColor.BackColor = colorDialog1.Color;
            }
        }
    }
}