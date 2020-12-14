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
    public class ChartCFM: ChartBase
    {
        float[] mCFM = new float[Share.MAX_CANDLE_CHART_COUNT];

        short[] mPricelines = new short[10];
        //==================
        short[] mChartEMA;
        float mHi = -1000000;
        float mLo = 1000000;
        //===================================
        public ChartCFM(Font f)
            : base(f)
        {
            mShouldDrawCursor = true;
            mChartType = CHART_CFM;
            CHART_BORDER_SPACING_Y = 1;
        }

        public override void render(xGraphics g)
        {
            if (detectShareCursorChanged())
            {
                mHi = -1000000;
                mLo = 1000000;
                Share s = getShare();

                int period = (int)mContext.mOptCFMPeriod;

                s.calcCFM(period, mCFM);

                for (int i = s.mBeginIdx; i < s.mEndIdx; i++)
                {
                    if (mCFM[i] > mHi) mHi = mCFM[i];
                    if (mCFM[i] < mLo) mLo = mCFM[i];
                }
                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                pricesToYs(mCFM, s.mBeginIdx, mChartLineXY, mChartLineLength, mLo, mHi);
            }

            if (mShouldDrawGrid)
                drawGrid(g);

            String[] ss = { "-0.2", "-0.1", "0.0", "0.1", "0.2" };
            float[] tmp = { -0.2f, -0.1f, 0.0f, 0.1f, 0.2f };
            pricesToYs(tmp, 0, mPricelines, 5, mLo, mHi);

            for (int i = 0; i < 5; i++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, mPricelines[2 * i + 1], getW() - 20, mPricelines[2 * i + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ss[i], getW() - 0, mPricelines[2 * i + 1], xGraphics.VCENTER | xGraphics.RIGHT);
            }

            g.setColor(C.COLOR_GREEN_DARK);
            g.drawLines(mChartLineXY, mChartLineLength, mLineThink);

            fillColorGreen(g, mChartLineXY, mChartLineLength, mPricelines[5]);
            fillColorRed(g, mChartLineXY, mChartLineLength, mPricelines[5]);

            mMouseTitle = null;//"" + (int)yToPrice(mLastY, 0, 100);

            renderCursor(g);
        }

        public override string getTitle()
        {
            Share share = getShare(3);
            if (share != null)
            {
                int idx = share.getCursor();

                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                if (mChartType == CHART_CFM)
                {
                    sb.AppendFormat("CMF({0}): {1:F2}", (int)mContext.mOptCFMPeriod, mCFM[idx]);
                }

                return sb.ToString();
            }

            return "";
        }

        override public xVector getTitles()
        {
            xVector v = new xVector();
            stTitle title = new stTitle(getTitle(), C.COLOR_WHITE);
            v.addElement(title);
            return v;
            /*
            xVector v = new xVector();

            Share share = getShare();
            if (share == null)
                return null;

            int idx = share.getCursor();
            int adx = (int)Share.pADX[idx];
            int pdi = (int)Share.pPLUS_DI[idx];
            int mdi = (int)Share.pMINUS_DI[idx];

            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            sb.AppendFormat("ADX({0})={1}", mContext.mOptADXPeriod, adx);
            v.addElement(new stTitle(sb.ToString(), 0xfff0f0f0));

            //  +di
            sb.Length = 0;
            sb.AppendFormat("DMI({0}): +DI={1}", (int)mContext.mOptADXPeriodDMI, pdi);
            v.addElement(new stTitle(sb.ToString(), 0xff00ff00));

            //  signal 9
            sb.Length = 0;
            sb.AppendFormat("-DI: {0}", mdi);

            v.addElement(new stTitle(sb.ToString(), 0xffff0000));

            return v;
             * */
        }
    }
}
