using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using xlib.ui;

namespace stock123.app.ui
{
    public partial class AlarmDialog : Form
    {
        xBaseControl mControl;
        ScreenBase mScreen;
        public AlarmDialog(ScreenBase screen, int firedCnt, xBaseControl c)
        {
            InitializeComponent();

            panel_Alarm.Controls.Add(c.getControl());
            panel_Alarm.AutoScroll = true;

            this.label1.Text = "Có tất cả <" + firedCnt + "> Cảnh báo được bật";

            mControl = c;
            mScreen = screen;
        }

        private void onCloseForm(object sender, FormClosingEventArgs e)
        {
            mControl.dispose();

//            mScreen.onCloseAlarmDialog();
        }

        public void setSizeH(int h)
        {
            panel_Alarm.Size = new Size(panel_Alarm.Size.Width, h - 60);
            this.Size = new Size(this.Width, h);
        }
    }
}