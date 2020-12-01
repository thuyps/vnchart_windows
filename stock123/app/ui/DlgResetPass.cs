using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DlgResetPass : Form
    {
        public DlgResetPass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bt_Back_Click(object sender, EventArgs e)
        {
            if (tb_Password.Text == null || tb_Password.Text.Length < 0)
            {
                lbStatus.Text = "Password không hợp lệ!";
                tb_Password.Focus();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        public string getNewPassword()
        {
            return tb_Password.Text;
        }
    }
}