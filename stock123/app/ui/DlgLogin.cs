using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using xlib.framework;

namespace stock123.app.ui
{
    public partial class DlgLogin : Form
    {
        public const int DLG_LOGIN = 0;
        public const int DLG_OFFLINE = 1;
        public const int DLG_REGISTER = 2;
        public const int DLG_FORGOT_PASSWORD = 3;
        public int mDlgResult;
        public DlgLogin(string title, string email, string pass)
        {
            InitializeComponent();

            tb_Email.Text = email;
            tb_Password.Text = pass;
            lbStatus.Text = title;
            this.cb_Autologin.Checked = Context.getInstance().mOptAutologin;
        }

        private void DlgLogin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tb_Email.Text != null && tb_Email.Text.Length > 0
                && tb_Password.Text != null && tb_Password.Text.Length > 0)
            {
                mDlgResult = DLG_LOGIN;
                this.Close();
            }
            else
                lbStatus.Text = "Email/Password không hợp lệ!";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mDlgResult = DLG_REGISTER;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mDlgResult = DLG_OFFLINE;
            this.Close();
        }

        public string getEmail()
        {
            return tb_Email.Text;
        }

        public string getPassword()
        {
            return tb_Password.Text;
        }

        private void cb_Autologin_CheckedChanged(object sender, EventArgs e)
        {
            Context.getInstance().mOptAutologin = this.cb_Autologin.Checked;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mDlgResult = DLG_FORGOT_PASSWORD;
            this.Close();
        }
    }
}