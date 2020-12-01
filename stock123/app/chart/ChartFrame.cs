using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.framework;
using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;

namespace stock123.app.chart
{
    public class ChartFrame: xBaseControl
    {
        //=================================================================
        //xVector mCharts = new xVector(10);
        xBaseControl[] mCharts = { null, null };
        Font mFont;
        Context mContext;
        public bool mShouldDrawGrid = false;
        public ChartFrame()
            : base(null)
        {
            makeCustomRender(true);

            setBackgroundColor(C.COLOR_BLACK);
            mContext = Context.getInstance();
            mFont = mContext.getFontSmall();
        }

        public override void render(xGraphics g)
        {
            for (int i = 0; i < mCharts.Length; i++ )
            {
                xBaseControl c = (xBaseControl)mCharts[i];
                if (c != null)
                {
                    c.render(g);
                }
            }
        }

        public void setFirstChart(ChartBase c)
        {
            c.setSize(this);

            mCharts[0] = c;
        }

        public void setSecondChart(ChartBase c)
        {
            c.setSize(this);

            mCharts[1] = c;
        }

        public override void onMouseDown(int x, int y)
        {
            for (int i = 0; i < mCharts.Length; i++)
            {
                xBaseControl c = (xBaseControl)mCharts[i];
                if (c != null)
                {
                    c.onMouseDown(x, y);
                    break;
                }
            }
        }

        public override void  onMouseMove(int x, int y)
        {
            for (int i = 0; i < mCharts.Length; i++)
            {
                xBaseControl c = (xBaseControl)mCharts[i];
                if (c != null)
                {
                    c.onMouseMove(x, y);
                    break;
                }
            }
        }


        public override void onMouseUp(int x, int y)
        {
            for (int i = 0; i < mCharts.Length; i++)
            {
                xBaseControl c = (xBaseControl)mCharts[i];
                if (c != null)
                {
                    c.onMouseUp(x, y);
                    break;
                }
            }
        }

        public override void setSize(int w, int h)
        {
            base.setSize(w, h);

            for (int i = 0; i < mCharts.Length; i++)
            {
                xBaseControl c = (xBaseControl)mCharts[i];
                if (c != null)
                {
                    c.setSize(this);
                    break;
                }
            }
        }

        public void clearCharts()
        {
            mCharts[0] = null;
            mCharts[1] = null;
        }
    }
}
