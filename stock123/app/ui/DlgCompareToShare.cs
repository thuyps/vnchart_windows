using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DlgCompareToShare : Form
    {
        public DlgCompareToShare()
        {
            InitializeComponent();
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        public void setShareCode(String code)
        {
            if (code != null)
            {
                this.tb_ShareCode.Text = code;
            }
        }

        public String getShareCode()
        {
            return this.tb_ShareCode.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.tb_ShareCode.Text = "^VNINDEX";
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.tb_ShareCode.Text = "^HASTC";
            this.DialogResult = DialogResult.OK;

            this.Close();
        }
    }
}