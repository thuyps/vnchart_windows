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
    public class ChartAroon: ChartBase
    {
        float[] mAroonUp = new float[Share.MAX_CANDLE_CHART_COUNT];
        float[] mAroonDown = new float[Share.MAX_CANDLE_CHART_COUNT];
        float[] mAroonOscillator = new float[Share.MAX_CANDLE_CHART_COUNT];

        short[] mPricelines = new short[10];
        short[] mAroonUpXY;
        short[] mAroonDownXY;
        short[] mAroonOscillatorXY;
        //==================
        short[] mChartEMA;
        //===================================
        public ChartAroon(Font f, int aroonType)
            : base(f)
        {
            mShouldDrawCursor = true;
            mChartType = aroonType;
            CHART_BORDER_SPACING_Y = 10;
        }

        public override void render(xGraphics g)
        {
            int mH = getH();
            int mW = getW();

            if (isHiding())
                return;
            Share share = getShare();
            if (share == null)
                return;

            if (mChartType == CHART_AROON)
            {
                drawChartAroon(g);
            }
            if (mChartType == CHART_AROON_OSCILLATOR)
            {
                drawChartAroonOscillator(g);
            }
        }

        void drawChartAroon(xGraphics g)
        {
            int idx = 0;
            int maxPrice = 80;
            if (detectShareCursorChanged())
            {
                Share s = getShare();
                s.calcARoon(mAroonUp, mAroonDown, mAroonOscillator, (int)mContext.mOptAroonPeriod);

                mAroonUpXY = allocMem(mAroonUpXY, mChartLineLength * 2);
                mAroonDownXY = allocMem(mAroonDownXY, mChartLineLength * 2);
                mAroonOscillatorXY = allocMem(mAroonOscillatorXY, mChartLineLength * 2);

                pricesToYs(mAroonUp, s.mBeginIdx, mAroonUpXY, mChartLineLength, 0, 100);
                pricesToYs(mAroonDown, s.mBeginIdx, mAroonDownXY, mChartLineLength, 0, 100);
            }

            if (mShouldDrawGrid)
                drawGrid(g);

            String[] ss = { "30", "50", "70" };
            float[] tmp = { 30, 50, 70 };
            pricesToYs(tmp, 0, mPricelines, 3, 0, 100);
            for (int i = 0; i < 3; i++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, mPricelines[2 * i + 1], getW() - 20, mPricelines[2 * i + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ss[i], getW() - 20, mPricelines[2 * i + 1], xGraphics.VCENTER);
            }

            g.setColor(C.COLOR_GREEN_DARK);
            g.drawLines(mAroonUpXY, mChartLineLength, mLineThink);

            g.setColor(C.COLOR_MAGENTA);
            g.drawLines(mAroonDownXY, mChartLineLength, mLineThink);

            //        g.setColor(C.COLOR_WHITE);
            //        g.drawLines(mAroonOscillatorXY, mChartLineLength, 2.0f);

            mMouseTitle = null;//"" + (int)yToPrice(mLastY, 0, 100);

            renderCursor(g);
        }

        protected void drawChartAroonOscillator(xGraphics g)
        {
            if (detectShareCursorChanged())
            {
                Share s = getShare();
                s.calcARoon(mAroonUp, mAroonDown, mAroonOscillator, (int)mContext.mOptAroonPeriod);

                mAroonOscillatorXY = allocMem(mAroonOscillatorXY, mChartLineLength * 2);

                pricesToYs(mAroonOscillator, s.mBeginIdx, mAroonOscillatorXY, mChartLineLength, -100, 100);
            }

            if (mShouldDrawGrid)
                drawGrid(g);

            String[] ss = { "-50", "0", "50" };
            float[] tmp = { -50, 0, 50 };
            pricesToYs(tmp, 0, mPricelines, 3, -100, 100);
            for (int i = 0; i < 3; i++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, mPricelines[2 * i + 1], getW() - 20, mPricelines[2 * i + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ss[i], getW() - 20, mPricelines[2 * i + 1], xGraphics.VCENTER);
            }

            fillColorGreen(g, mAroonOscillatorXY, mChartLineLength, mPricelines[3]);
            fillColorRed(g, mAroonOscillatorXY, mChartLineLength, mPricelines[3]);

            g.setColor(C.COLOR_YELLOW);
            g.drawLines(mAroonOscillatorXY, mChartLineLength, 2.0f);

            mMouseTitle = null;//"" + (int)yToPrice(mLastY, 0, 100);

            renderCursor(g);
        }

        public override string getTitle()
        {
            Share share = getShare();
            if (share != null)
            {
                int idx = share.getCursor();

                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                if (mChartType == CHART_AROON)
                {
                    sb.AppendFormat("Aroon({0}): Up: {1:F1}    Down: {2:F1}", (int)mContext.mOptAroonPeriod, mAroonUp[idx], mAroonDown[idx]);
                }
                else if (mChartType == CHART_AROON_OSCILLATOR)
                {
                    sb.AppendFormat("AroonOscillator({0}): {1:F1}", (int)mContext.mOptAroonPeriod, mAroonOscillator[idx]);
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
