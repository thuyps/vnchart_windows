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
    public class ChartStochRSI : ChartBase
    {
        short[] mPricelines = new short[10];
        //==============================

        public ChartStochRSI(Font f)
            : base(f)
        {
            mChartType = CHART_STOCHRSI;
        }

        public override void render(xGraphics g)
        {
            int mH = getH();
            int mW = getW();

            if (isHiding())
                return;
            Share share = getShare(3);

            if (share == null)
                return;
            if (detectShareCursorChanged())
            {
                share.calcStochRSI();

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                pricesToYs(share.pStochRSI, share.mBeginIdx, mChartLineXY, mChartLineLength, -0.1f, 1.1f);
                float[] tmp = { 0, 0.2f, 0.5f, 0.8f, 1};
                pricesToYs(tmp, 0, mPricelines, 5, -0.1f, 1.1f);

                //  SMA
                mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);
                pricesToYs(share.pStochRSISMA, share.mBeginIdx, mChartLineXY2, mChartLineLength, -0.1f, 1.1f);
            }

            if (mChartLineLength == 0)
                return;

            //========================
            if (mShouldDrawGrid)
                drawGrid(g);
            //===============================================
            String[] ss = {"0.0",  "0.2", "0.5", "0.8", "1.0" };

            for (int i = 0; i < 5; i++)
            {
                if (i == 0 || i == 4 || i == 2)
                {
                    g.setColor(C.COLOR_GRAY_DARK);
                    g.drawLineDotHorizontal(0, mPricelines[2 * i + 1], getW() - 20, mPricelines[2 * i + 1]);
                }
                else
                {
                    g.setColor(C.COLOR_FADE_YELLOW);
                    g.drawLine(0, mPricelines[2 * i + 1], getW() - 20, mPricelines[2 * i + 1]);
                }
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ss[i], getW() - 20, mPricelines[2 * i + 1], xGraphics.VCENTER);
            }

            //  stochRSI
            g.setColor(0xffff8000);
            g.drawLines(mChartLineXY, mChartLineLength, 1.5f);

            g.setColor(0xffb000b0);
            g.drawLines(mChartLineXY2, mChartLineLength, 1.5f);

            renderDrawer(g);

            renderCursor(g);
        }
        override public xVector getTitles()
        {
            xVector v = new xVector(1);
            Share share = getShare(3);
            if (share != null)
            {
                int idx = share.getCursor();
                float vs;
                StringBuilder sb = Utils.sb;
                sb.Length = 0;

                vs = share.pStochRSI[idx];

                sb.AppendFormat("StochRSI({0:F0})={1:F1}", mContext.mOptStochRSIPeriod, vs);
                v.addElement(new stTitle(sb.ToString(), C.COLOR_ORANGE));

                sb.Length = 0;
                sb.AppendFormat("SMA({0})", mContext.mOptStochRSISMAPeriod);
                v.addElement(new stTitle(sb.ToString(), C.COLOR_MAGENTA));
            }
            return v;
        }
    }
}
