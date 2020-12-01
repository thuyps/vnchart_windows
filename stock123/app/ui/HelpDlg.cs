using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace stock123.app.ui
{
    public partial class HelpDlg : Form
    {
        public HelpDlg()
        {
            InitializeComponent();

            string path = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().GetModules()
                [0].FullyQualifiedName) + "\\help\\content.htm";
            System.Console.WriteLine(path);

            webBrowser1.Navigate("file:///" + path);
        }

        public HelpDlg(string url)
        {
            InitializeComponent();

            webBrowser1.Navigate(url);
        }
    }
}