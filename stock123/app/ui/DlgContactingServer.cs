using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DlgContactingServer : Form
    {
        public DlgContactingServer()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void setMsg2(string msg)
        {
            lb_Downloaded.Text = msg;
        }
    }
}