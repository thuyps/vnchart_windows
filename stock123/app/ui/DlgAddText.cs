using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stock123.app.ui
{
    public partial class DlgAddText : Form
    {
        public DlgAddText()
        {
            InitializeComponent();
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        public void setText(string text)
        {
            this.tb_Text.Text = text;
        }

        public string getText()
        {
            return this.tb_Text.Text;
        }
    }
}