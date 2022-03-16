using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;
using xlib.framework;

namespace stock123.app.chart
{
    public class ChartOBV : ChartBase
    {
        float[] mPricelines = new float[10];
        float[] mChartEMA;
        //==============================

        public ChartOBV(Font f)
            : base(f)
        {
            mChartType = CHART_OBV;
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
                share.calcOBV();

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                float min = 0xffffff;
                float max = -0xffffff;

                for (int i = share.mBeginIdx; i <= share.mEndIdx; i++)
                {
                    if (share.pOBV[i] > max) max = share.pOBV[i];
                    if (share.pOBV[i] < min) min = share.pOBV[i];
                }

                pricesToYs(share.pOBV, share.mBeginIdx, mChartLineXY, mChartLineLength, min, max);

                if (mContext.mOptOBV_EMA > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);

                    if (mContext.mOptOBV_EMA_ON[0])
                    {
                        Share.EMA(share.pOBV, share.getCandleCount(), (int)mContext.mOptOBV_EMA, share.pTMP);
                    }
                    else
                    {
                        Share.SMA(share.pOBV, 0, share.getCandleCount(), (int)mContext.mOptOBV_EMA, share.pTMP);
                    }
                    pricesToYs(share.pTMP, share.mBeginIdx, mChartEMA, mChartLineLength, min, max);
                }
            }

            if (mChartLineLength == 0)
                return;

            //========================
            if (mShouldDrawGrid)
                drawGrid(g);
            //===============================================
            //  OBV
            g.setColor(0xff00f000);
            g.drawLines(mChartLineXY, mChartLineLength, 1.5f);

            if (mContext.mOptOBV_EMA > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                g.drawLines(mChartEMA, mChartLineLength, 1.0f);
            }

            renderCursor(g);

            renderDrawer(g);
        }

        override public xVector getTitles()
        {
            xVector v = new xVector(1);

            v.addElement(new stTitle("On Balance Volume", 0xffffffff));

            return v;
        }
    }
}
