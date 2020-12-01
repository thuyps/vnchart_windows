using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DlgLineColorPicker : Form
    {
        float mThickness = 1.0f;
        int mColor = 0;
        public DlgLineColorPicker(int color, float thickness)
        {
            InitializeComponent();
            bt_Color.BackColor = Color.FromArgb(color);
            cb_Thickness.Text = "" + thickness;
        }

        private void bt_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color c = colorDialog1.Color;
                bt_Color.BackColor = c;
            }
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            mColor = bt_Color.BackColor.ToArgb();
            this.Close();
        }

        public int getColor()
        {
            return mColor;
        }

        public float getThickness()
        {
            try
            {
                mThickness = float.Parse(cb_Thickness.Text);
            }catch(Exception e)
            {
            }

            return mThickness;
        }
    }
}