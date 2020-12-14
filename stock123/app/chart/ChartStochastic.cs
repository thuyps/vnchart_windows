using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.utils;
using xlib.ui;
using xlib.framework;
using stock123.app;
using stock123.app.data;


namespace stock123.app.chart
{
    public class ChartStochastic : ChartBase
    {
        bool mIsFastStochastic;
        int mStochasticLengthK;
        int mStochasticLengthD;

        short[] mK;
        short[] mD;
        short[] mPricelines = new short[6];
        //==============================

        public ChartStochastic(Font f, bool isFast)
            : base(f)
        {
            mIsFastStochastic = isFast;
            mChartType = CHART_STOCHASTIC_FAST;
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
                share.calcStochastic();

                int newSize = mChartLineLength * 2;
                mK = allocMem(mK, newSize);
                mD = allocMem(mD, newSize);

                if (mIsFastStochastic)
                {
                    pricesToYs(share.pStochasticFastK, share.mBeginIdx, mK, mChartLineLength, 0, 100);
                    pricesToYs(share.pStochasticFastD, share.mBeginIdx, mD, mChartLineLength, 0, 100);
                }
                else
                {
                    pricesToYs(share.pStochasticSlowK, share.mBeginIdx, mK, mChartLineLength, 0, 100);
                    pricesToYs(share.pStochasticSlowD, share.mBeginIdx, mD, mChartLineLength, 0, 100);
                }

                float[] tmp = { 20, 50, 80 };
                pricesToYs(tmp, 0, mPricelines, 3, 0, 100);
            }

            if (mChartLineLength == 0)
                return;

            //========================
            if (mShouldDrawGrid)
                drawGrid(g);
            //===============================================
            String[] ss = { "20", "50", "80" };

            for (int i = 0; i < 3; i++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, mPricelines[2 * i + 1], getW() - 20, mPricelines[2 * i + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ss[i], getW() - 20, mPricelines[2 * i + 1], xGraphics.VCENTER);
            }

            //  fast: pink
            g.setColor(0xc0ff00ff);
            g.drawLines(mK, mChartLineLength, 1.5f);

            //  slow: 
            g.setColor(0xc0f08000);
            g.drawLines(mD, mChartLineLength, 1.5f);

            mMouseTitle = "" + (int)yToPrice(mLastY, 0, 100);
            renderCursor(g);

            renderDrawer(g);
        }
        override public xVector getTitles()
        {
            Share share = getShare(3);
            xVector v = new xVector(3);
            if (share != null)
            {
                int idx = share.getCursor();
                float k, d;
                StringBuilder sb = Utils.sb;
                sb.Length = 0;

                if (mIsFastStochastic)
                {
                    k = share.pStochasticFastK[idx];
                    d = share.pStochasticFastD[idx];
                    int periodK = (int)mContext.mOptStochasticFastKPeriod;
                    int periodD = (int)mContext.mOptStochasticFastSMA;

                    sb.AppendFormat("Fast Stochastic ({0}, {1}) ", periodK, periodD);
                    v.addElement(new stTitle(sb.ToString(), 0xffffffff));

                    sb.Length = 0;
                    sb.AppendFormat("%K={0:F2}", k);
                    v.addElement(new stTitle(sb.ToString(), C.COLOR_MAGENTA));

                    sb.Length = 0;
                    sb.AppendFormat("%D={0:F2}", d);
                    v.addElement(new stTitle(sb.ToString(), C.COLOR_ORANGE));
                }
                else
                {
                    k = share.pStochasticSlowK[idx];
                    d = share.pStochasticSlowD[idx];
                    int periodK = (int)mContext.mOptStochasticSlowKPeriod;
                    int smooth = (int)mContext.mOptStochasticSlowKSmoothK;
                    int sma = (int)mContext.mOptStochasticSlowSMA;

                    sb.AppendFormat("Full Stochastic ({0}, {1}, {2}) ", periodK, smooth, sma);
                    v.addElement(new stTitle(sb.ToString(), 0xffffffff));

                    sb.Length = 0;
                    sb.AppendFormat("%K={0:F2}", k);
                    v.addElement(new stTitle(sb.ToString(), C.COLOR_MAGENTA));

                    sb.Length = 0;
                    sb.AppendFormat("%D={0:F2}", d);
                    v.addElement(new stTitle(sb.ToString(), C.COLOR_ORANGE));
                }
            }
            return v;
        }
    }
}
