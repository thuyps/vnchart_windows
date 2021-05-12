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
        float[] pStochRSI;
        float[] pStochRSISMA;
        //==============================

        public ChartStochRSI(Font f)
            : base(f)
        {
            mChartType = CHART_STOCHRSI;
            pStochRSI = new float[Share.MAX_CANDLE_CHART_COUNT];
            pStochRSISMA = new float[Share.MAX_CANDLE_CHART_COUNT];
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
                int period = 14;
                if (mChartType == ChartBase.CHART_STOCHRSI)
                {
                    period = (int)Context.getInstance().mOptStochRSIPeriod; 
                    //GlobalData.getData().getValueInt(GlobalData.kStockRSIPeriod1, 14);
                }
                else
                {
                    period = GlobalData.getData().getValueInt(GlobalData.kStockRSIPeriod2, 25);
                }
                share.calcStochRSI(period, pStochRSI, pStochRSISMA);

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                pricesToYs(pStochRSI, share.mBeginIdx, mChartLineXY, mChartLineLength, -10, 110);
                float[] tmp = { 0, 20, 50, 80, 100};
                pricesToYs(tmp, 0, mPricelines, 5, -10, 110);

                //  SMA
                mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);
                pricesToYs(pStochRSISMA, share.mBeginIdx, mChartLineXY2, mChartLineLength, -10, 110);
            }

            if (mChartLineLength == 0)
                return;

            //========================
            if (mShouldDrawGrid)
                drawGrid(g);
            //===============================================
            String[] ss = {"0",  "20", "50", "80", "100" };

            for (int i = 0; i < 5; i++)
            {
                if (i == 0 || i == 4 || i == 2)
                {
                    g.setColor(C.COLOR_GRAY_DARK);
                    g.drawLineDotHorizontal(0, mPricelines[2 * i + 1], getW() - 44, mPricelines[2 * i + 1]);
                }
                else
                {
                    g.setColor(C.COLOR_FADE_YELLOW);
                    g.drawLine(0, mPricelines[2 * i + 1], getW() - 44, mPricelines[2 * i + 1]);
                }
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ss[i], getW() - 8, mPricelines[2 * i + 1], xGraphics.VCENTER | xGraphics.RIGHT);
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

                vs = pStochRSI[idx];

                int period = 14;
                if (mChartType == ChartBase.CHART_STOCHRSI)
                {
                    period = GlobalData.getData().getValueInt(GlobalData.kStockRSIPeriod1, 14);
                }
                else
                {
                    period = GlobalData.getData().getValueInt(GlobalData.kStockRSIPeriod1, 25);
                }
                sb.AppendFormat("StochRSI({0:F0})={1:F1}", period, vs);
                v.addElement(new stTitle(sb.ToString(), C.COLOR_ORANGE));

                sb.Length = 0;
                sb.AppendFormat("SMA({0})", mContext.mOptStochRSISMAPeriod);
                v.addElement(new stTitle(sb.ToString(), C.COLOR_MAGENTA));
            }
            return v;
        }
    }
}
