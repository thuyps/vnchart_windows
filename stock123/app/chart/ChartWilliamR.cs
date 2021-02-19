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
    class ChartWilliamR: ChartBase
    {
        short[] mChartEMA;
        short[] mChartEMA2;
        short[] mPricelines = new short[10];
        public ChartWilliamR(Font f)
            : base(f)
        {
            mChartType = CHART_WILLIAMR;
        }

        override public void render(xGraphics g)
        {
            int mH = getH();
            int mW = getW();

            if (isHiding())
                return;
            Share share = getShare(3);

            if (share == null)
                return;

            int ma1 = 0;
            int ma2 = 0;
            if (detectShareCursorChanged())
            {
                share.calcWilliamR();

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                pricesToYs(share.pWilliamR, share.mBeginIdx, mChartLineXY, mChartLineLength, -100, 0);
                float[] tmp = { -20, -50, -80 };
                pricesToYs(tmp, 0, mPricelines, 3, -100, 0);

                ma1 = GlobalData.getData().getValueInt(GlobalData.kWilliamR_MA1);
                ma2 = GlobalData.getData().getValueInt(GlobalData.kWilliamR_MA2);

                if (ma1 > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);
                    Share.SMA(share.pWilliamR, 0, share.getCandleCount(), ma1, share.pEMAIndicator);

                    pricesToYs(share.pEMAIndicator, share.mBeginIdx, mChartEMA, mChartLineLength, -100, 0);
                }
                if (ma2 > 0)
                {
                    mChartEMA2 = allocMem(mChartEMA2, mChartLineLength * 2);
                    Share.SMA(share.pWilliamR, 0, share.getCandleCount(), ma2, share.pEMAIndicator);

                    pricesToYs(share.pEMAIndicator, share.mBeginIdx, mChartEMA2, mChartLineLength, -100, 0);
                }
            }

            if (mChartLineLength == 0)
                return;

            //========================
            if (mShouldDrawGrid)
                drawGrid(g);
            //===============================================
            String[] ss = { "-20", "-50", "-80" };

            g.setColor(C.COLOR_GRAY_DARK);
            for (int i = 0; i < 3; i++)
            {
                g.setColor(C.COLOR_GRAY_DARK);
                g.drawLine(0, mPricelines[2 * i + 1], getW() - 34, mPricelines[2 * i + 1]);

                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ss[i], getW() - 3, mPricelines[i * 2 + 1], xGraphics.VCENTER | xGraphics.RIGHT);
            }

            //  williamR
            g.setColor(0xffff8000);
            g.drawLines(mChartLineXY, mChartLineLength, 1.5f);

            ma1 = GlobalData.getData().getValueInt(GlobalData.kWilliamR_MA1);
            ma2 = GlobalData.getData().getValueInt(GlobalData.kWilliamR_MA2);
            if (ma1 > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                g.drawLines(mChartEMA, mChartLineLength, 1.0f);
            }
            if (ma2 > 0)
            {
                g.setColor(C.COLOR_CYAN);
                g.drawLines(mChartEMA2, mChartLineLength, 1.0f);
            }

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
                String s;

                vs = share.pWilliamR[idx];

                if (mContext.mOptWR_EMA > 0)
                {
                    s = String.Format("William%R{0}={1:F1}    MA={2:F1}", (int)mContext.mOptWilliamRPeriod, vs, share.pEMAIndicator[idx]);
                }
                else
                {
                    s = String.Format("William%R{0}={1:F1}", (int)mContext.mOptWilliamRPeriod, vs);
                }
                v.addElement(new stTitle(s, C.COLOR_WHITE));
            }
            return v;
        }
    }
}
