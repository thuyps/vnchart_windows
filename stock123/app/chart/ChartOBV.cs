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
        short[] mPricelines = new short[10];
        short[] mChartEMA;
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
            Share share = mContext.getSelectedDrawableShare(3);

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
                    if (Share.pOBV[i] > max) max = Share.pOBV[i];
                    if (Share.pOBV[i] < min) min = Share.pOBV[i];
                }

                pricesToYs(Share.pOBV, share.mBeginIdx, mChartLineXY, mChartLineLength, min, max);

                if (mContext.mOptOBV_EMA > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);

                    if (mContext.mOptOBV_EMA_ON[0])
                    {
                        Share.EMA(Share.pOBV, share.getCandleCount(), (int)mContext.mOptOBV_EMA, Share.pTMP);
                    }
                    else
                    {
                        Share.SMA(Share.pOBV, 0, share.getCandleCount(), (int)mContext.mOptOBV_EMA, Share.pTMP);
                    }
                    pricesToYs(Share.pTMP, share.mBeginIdx, mChartEMA, mChartLineLength, min, max);
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
