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
        float[] priceValues = { 0, 0, 0, 0, 0};
        int priceValuesCnt = 0;

        short[] mPricelines = new short[10];
        short[] mChartEMA1;
        short[] mChartEMA2;

        int baseMAPeriod1;
        int baseMAPeriod2;
        int mPeriod;
        //==============================

        public ChartComparativeRS(int chartType, Font f)
            : base(f)
        {
            setChartType(chartType);

            loadSavedBase();
        }

        public void loadSavedBase()
        {
            if (mChartType == CHART_CRS_RATIO)
            {
                String symbol = "^VNINDEX";
                int ma1 = 5;
                int ma2 = 20;
                VTDictionary dict = GlobalData.getCRSRatio();
                symbol = dict.getValueString(GlobalData.kCRSBaseSymbol);
                if (symbol == null)
                {
                    symbol = "^VNINDEX";
                }

                if (dict.hasKey(GlobalData.kCRSBaseMa1))
                {
                    ma1 = dict.getValueInt(GlobalData.kCRSBaseMa1);
                    ma2 = dict.getValueInt(GlobalData.kCRSBaseMa2);
                }
                
                setBaseSymbol(symbol, ma1, ma2);

            }
            else if (mChartType == CHART_CRS_PERCENT)
            {
                String symbol = "^VNINDEX";
                int ma1 = 5;
                int ma2 = 20;
                int period = 20;

                VTDictionary dict = GlobalData.getCRSPercent();
                symbol = dict.getValueString(GlobalData.kCRSBaseSymbol);
                if (symbol == null)
                {
                    symbol = "^VNINDEX";
                }

                if (dict.hasKey(GlobalData.kCRSBaseMa1))
                {
                    ma1 = dict.getValueInt(GlobalData.kCRSBaseMa1);
                    ma2 = dict.getValueInt(GlobalData.kCRSBaseMa2);
                    period = dict.getValueInt(GlobalData.kCRSPeriod);
                }

                setBaseSymbol(symbol, ma1, ma2);
                mPeriod = period;
            }
            
        }

        Share baseShare()
        {
            String symbol = "^VNINDEX";
            if (mChartType == CHART_CRS_RATIO)
            {
                VTDictionary dict = GlobalData.getCRSRatio();
                symbol = dict.getValueString(GlobalData.kCRSBaseSymbol);
                if (symbol == null)
                {
                    symbol = "^VNINDEX";
                }
            }
            else if (mChartType == CHART_CRS_PERCENT)
            {
                VTDictionary dict = GlobalData.getCRSPercent();
                symbol = dict.getValueString(GlobalData.kCRSBaseSymbol);
                if (symbol == null)
                {
                    symbol = "^VNINDEX";
                }
            }
            Share share = Context.getInstance().mShareManager.getShare(symbol);
            if (share == null)
            {
                share = Context.getInstance().mShareManager.getShare("^VNINDEX");
            }
            return share;
        }

        public void setBaseSymbol(String symbol, int ma1, int ma2)
        {
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

            if (share == null || baseShare() == null)
            {
                return;
            }

            if (mChartType == CHART_CRS_RATIO)
            {
                drawComparativeCRSRatio(g);
            }
            else if (mChartType == CHART_CRS_PERCENT)
            {
                drawComparativeCRSPercent(g);
            }
        }

        void drawComparativeCRSRatio(xGraphics g)
        {
            Share share = getShare();

            if (detectShareCursorChanged())
            {
                loadSavedBase();
                share.calcCRS(baseShare(), baseMAPeriod1, baseMAPeriod2);

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                float min = 0xffffff;
                float max = -0xffffff;

                for (int i = share.mBeginIdx; i <= share.mEndIdx; i++)
                {
                    if (share.pCRS[i] > max) max = share.pCRS[i];
                    if (share.pCRS[i] < min) min = share.pCRS[i];
                }

                float dis = (max - min);
                priceValuesCnt = 4;
                for (int i = 0; i < priceValuesCnt; i++)
                {
                    priceValues[i] = min + (i+0.5f)*(dis / 4);
                }

                pricesToYs(priceValues, 0, mPricelines, priceValuesCnt, min, max);

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

            for (int i = 0; i < priceValuesCnt; i++)
            {
                String priceS = String.Format("{0:F3}", priceValues[i]);
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, mPricelines[2 * i + 1], getW() - 34, mPricelines[2 * i + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, priceS, getW() -2, mPricelines[2 * i + 1], xGraphics.VCENTER|xGraphics.RIGHT);
            }
            //===============================================
            //  CRS
            g.setColor(0xffffffff);
            g.drawLines(mChartLineXY, mChartLineLength, 1.5f);

            if (baseMAPeriod1 > 0)
            {
                g.setColor(C.COLOR_GREEN);
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

        void drawComparativeCRSPercent(xGraphics g)
        {
            Share share = getShare();

            if (detectShareCursorChanged())
            {
                loadSavedBase();
                share.calcCRSPercent(baseShare(), mPeriod, baseMAPeriod1, baseMAPeriod2);

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                float min = 0xffffff;
                float max = -0xffffff;

                for (int i = share.mBeginIdx; i <= share.mEndIdx; i++)
                {
                    if (share.pCRS_Percent[i] > max) max = share.pCRS_Percent[i];
                    if (share.pCRS_Percent[i] < min) min = share.pCRS_Percent[i];
                }

                float dis = (max - min);
                priceValuesCnt = 4;
                for (int i = 0; i < priceValuesCnt; i++)
                {
                    priceValues[i] = min + (i + 0.5f) * (dis / 4);
                }

                pricesToYs(priceValues, 0, mPricelines, priceValuesCnt, min, max);

                pricesToYs(share.pCRS_Percent, share.mBeginIdx, mChartLineXY, mChartLineLength, min, max);

                float[] tmp = { 0 };
                pricesToYs(tmp, 0, mPricelines, 1, min, max);

                if (baseMAPeriod1 > 0)
                {
                    mChartEMA1 = allocMem(mChartEMA1, mChartLineLength * 2);

                    pricesToYs(share.pCRS_MA1_Percent, share.mBeginIdx, mChartEMA1, mChartLineLength, min, max);
                }
                if (baseMAPeriod2 > 0)
                {
                    mChartEMA2 = allocMem(mChartEMA2, mChartLineLength * 2);

                    pricesToYs(share.pCRS_MA2_Percent, share.mBeginIdx, mChartEMA2, mChartLineLength, min, max);
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
            for (int i = 0; i < priceValuesCnt; i++)
            {
                String priceS = String.Format("{0:F3}", priceValues[i]);
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, mPricelines[2 * i + 1], getW() - 34, mPricelines[2 * i + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, priceS, getW() - 2, mPricelines[2 * i + 1], xGraphics.VCENTER|xGraphics.RIGHT);
            }
            //  CRS
            g.setColor(0xffffffff);
            g.drawLines(mChartLineXY, mChartLineLength, 1.5f);

            if (baseMAPeriod1 > 0)
            {
                g.setColor(C.COLOR_GREEN);
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

            if (mChartType == CHART_CRS_RATIO)
            {
                String s;
                s = String.Format("RS ({0}/{1})", mShare.getCode(), baseShare().getCode());
                v.addElement(new stTitle(s, 0xffffffff));
            }
            else if (mChartType == CHART_CRS_PERCENT)
            {
                String s;
                int idx = mShare.getCursor();
                s = String.Format("cRS ({0}/{1})=[c/c({2})]/[base/base({3})]", mShare.getCode(), 
                    baseShare().getCode(), mPeriod, mPeriod);
;
                v.addElement(new stTitle(s, 0xffffffff));
            }

            return v;
        }
    }
}
