using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DialogAbout : Form
    {
        public DialogAbout()
        {
            InitializeComponent();
        }

        private void onOK(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://soft123.com.vn");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}