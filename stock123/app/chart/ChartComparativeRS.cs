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
    public class ChartComparativeRS : ChartBase
    {
        short[] mPricelines = new short[10];
        short[] mChartEMA1;
        short[] mChartEMA2;
        Share baseShare;
        int baseMAPeriod1;
        int baseMAPeriod2;
        //==============================

        public ChartComparativeRS(Font f)
            : base(f)
        {
            mChartType = CHART_COMPARING_RS;

            loadSavedBase();
        }

        public void loadSavedBase()
        {
            String symbol = GlobalData.getData().getValueString(GlobalData.kCRSBaseSymbol);
            if (symbol == null)
            {
                symbol = "^VNINDEX";
            }
            int ma1 = 5;
            int ma2 = 20;

            if (GlobalData.getData().hasKey(GlobalData.kCRSBaseMa1)){
                ma1 = GlobalData.getData().getValueInt(GlobalData.kCRSBaseMa1);
                ma2 = GlobalData.getData().getValueInt(GlobalData.kCRSBaseMa2);
            }

            setBaseSymbol(symbol, ma1, ma2);

        }

        public void setBaseSymbol(String symbol, int ma1, int ma2)
        {
            Share share = Context.getInstance().mShareManager.getShare(symbol);
            if (share == null)
            {
                share = Context.getInstance().mShareManager.getShare("^VNINDEX");
            }
            baseShare = share;
            baseMAPeriod1 = ma1;
            baseMAPeriod2 = ma2;
        }

        public override void render(xGraphics g)
        {
            int mH = getH();
            int mW = getW();

            if (isHiding())
                return;
            Share share = getShare(3);

            if (share == null || baseShare == null)
            {
                return;
            }
            if (detectShareCursorChanged())
            {
                share.calcCRS(baseShare, baseMAPeriod1, baseMAPeriod2);

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                float min = 0xffffff;
                float max = -0xffffff;

                for (int i = share.mBeginIdx; i <= share.mEndIdx; i++)
                {
                    if (share.pCRS[i] > max) max = share.pCRS[i];
                    if (share.pCRS[i] < min) min = share.pCRS[i];
                }

                pricesToYs(share.pCRS, share.mBeginIdx, mChartLineXY, mChartLineLength, min, max);

                if (baseMAPeriod1 > 0)
                {
                    mChartEMA1 = allocMem(mChartEMA1, mChartLineLength * 2);

                    pricesToYs(share.pCRS_MA1, share.mBeginIdx, mChartEMA1, mChartLineLength, min, max);
                }
                if (baseMAPeriod2 > 0)
                {
                    mChartEMA2 = allocMem(mChartEMA2, mChartLineLength * 2);

                    pricesToYs(share.pCRS_MA2, share.mBeginIdx, mChartEMA2, mChartLineLength, min, max);
                }
            }

            if (mChartLineLength == 0)
            {
                return;
            }

            //========================
            if (mShouldDrawGrid)
            {
                drawGrid(g);
            }
            //===============================================
            //  CRS
            g.setColor(0xff00f000);
            g.drawLines(mChartLineXY, mChartLineLength, 1.5f);

            if (baseMAPeriod1 > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                g.drawLines(mChartEMA1, mChartLineLength, 1.0f);
            }
            if (baseMAPeriod2 > 0)
            {
                g.setColor(C.COLOR_YELLOW);
                g.drawLines(mChartEMA2, mChartLineLength, 1.0f);
            }

            renderCursor(g);

            renderDrawer(g);
        }

        override public xVector getTitles()
        {
            xVector v = new xVector(1);

            v.addElement(new stTitle("CRS", 0xffffffff));

            return v;
        }
    }
}
