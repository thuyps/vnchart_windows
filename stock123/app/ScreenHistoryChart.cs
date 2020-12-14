using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.ui;
using xlib.framework;
using xlib.utils;
using stock123.app.data;
using stock123.app.chart;
using stock123.app.ui;
using stock123.app.net;

namespace stock123.app
{
    public class ScreenHistoryChart : ScreenBase
    {
        xTabPage mTabHistoryCharts;
        public ScreenHistoryChart()
            : base()
        {

        }

        public override void onActivate()
        {
            mTabHistoryCharts = new xTabPage("");
        }

        public override void onDeactivate()
        {

        }

        override public void onTick()
        {

        }

        void newHistoryChart()
        {
        }
    }
}
