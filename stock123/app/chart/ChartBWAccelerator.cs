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
    public class ChartBWAccelerator: ChartBase
    {
        float[] AO;

        float lowest;
        float highest;
        //===================================
        public ChartBWAccelerator(Font f)
            : base(f)
        {
            mShouldDrawCursor = true;
            mChartType = CHART_BW_Accelerator;
            //CHART_BORDER_SPACING_Y = 1;
            AO = new float[Share.MAX_CANDLE_CHART_COUNT];
        }

        void calcAC()
        {
            Share share = getShare();
            if (share.getCandleCnt() == 0)
            {
                return;
            }

            int cnt = share.getCandleCnt();

            float[] mediumPrice = share.pTMP1;
            float[] sma5 = share.pTMP2;
            float[] sma34 = share.pTMP3;

            stCandle c = new stCandle();
            for (int i = 0; i < cnt; i++)
            {
                share.getCandle(i, c);
                mediumPrice[i] = (c.highest + c.lowest) / 2;
            }

            int period1 = GlobalData.getData().getValueInt(GlobalData.kBW_ACShortPeriod, 5);
            int period2 = GlobalData.getData().getValueInt(GlobalData.kBW_ACLongPeriod, 34);
            int baseSMAOfAO = GlobalData.getData().getValueInt(GlobalData.kBWAC_baseSMAOfAO, 5);
            if (period1 == 0)
            {
                period1 = 5;
            }
            if (period2 == 0)
            {
                period2 = 34;
            }
            if (baseSMAOfAO == 0)
            {
                baseSMAOfAO = 5;
            }

            share.calcAC(period1, period2, baseSMAOfAO, AO);
        }

        public override void render(xGraphics g)
        {
            Share share = getShare(3);

            if (share == null)
            {
                return;
            }

            int mX = 0;
            int mY = 0;

            if (detectShareCursorChanged())
            {
                if (mChartLineXY == null)
                {
                    mChartLineXY = new float[2 * Share.MAX_CANDLE_CHART_COUNT];
                }

                calcAC();
                highest = AO[0];
                lowest = AO[0];
                for (int i = share.mBeginIdx; i <= share.mEndIdx; i++)
                {
                    if (AO[i] > highest)
                    {
                        highest = AO[i];
                    }
                    if (AO[i] < lowest)
                    {
                        lowest = AO[i];
                    }
                }

                double ry = (float)getDrawingH() / (highest - lowest);
                double rw = (float)getDrawingW() / mChartLineLength;
                mVolumeBarW = (((float)getDrawingW() / mChartLineLength) * 2.0f / 3);
                if (mVolumeBarW < 1) mVolumeBarW = 1;
                float volumeBarWHalf = mVolumeBarW / 2;

                for (int i = 0; i < mChartLineLength; i++)
                {
                    int j = (i + share.mBeginIdx);

                    mChartLineXY[i * 2] = (float)(mX + CHART_BORDER_SPACING_X + i * rw + getStartX() - volumeBarWHalf);	//	x

                    mChartLineXY[i * 2 + 1] = (float)(mY + getMarginY() + getDrawingH() - (AO[j] - lowest) * ry);
                }
            }

            g.setColor(0xff00ff00);
            //int tmp = mY + getH() - (CHART_BORDER_SPACING_Y) - Y0Means();
            uint colorUp = themeDark() ? (C.COLOR_TD_UP & 0x90ffffff) : C.COLOR_TL_UP;
            uint colorDown = themeDark() ? (C.COLOR_TD_DOWN & 0xa0ffffff) : C.COLOR_TL_DOWN;

            uint preColor = colorUp;
            uint color;

            //  zero line
            float y0 = priceToY(0, lowest, highest);
            g.setColor(colorPriceline());
            g.drawHorizontalLine(0, (float)y0, getW() - 20);

            for (int i = 0; i < mChartLineLength; i++)
            {
                int j = share.mBeginIdx + i;
                if (j <= 0)
                {
                    continue;
                }
                //if (j)
                {
                    if (AO[j] >= AO[j - 1])
                    {
                        color = colorUp;
                    }
                    else if (AO[j] < AO[j - 1])
                    {
                        color = colorDown;
                    }
                    else
                    {
                        color = preColor;
                    }
                    preColor = color;
                    g.setColor(color);
                }

                //if (AO[j] > 0)
                {
                    g.fillRect((float)(mChartLineXY[2 * i] - mVolumeBarW / 2),
                        (float)mChartLineXY[2 * i + 1],
                        (float)mVolumeBarW,
                        (float)y0 - mChartLineXY[2 * i + 1]);
                }
                //else{
                //g->fillRect(mChartLineXY[2*i]-mVolumeBarW/2, mChartLineXY[2*i+1], mVolumeBarW, y0 - mChartLineXY[2*i+1]);
                //}
            }

            if (mShouldDrawTitle)
            {
                g.setColor(C.COLOR_GRAY_LIGHT);
                g.drawString(mFont, "0", getW() - 2, mY + getH(), xGraphics.RIGHT | xGraphics.BOTTOM);
            }

            mMouseTitle = null;//"" + (int)yToPrice(mLastY, 0, 100);

            renderCursor(g);
        }


        override public xVector getTitles()
        {
            xVector v = new xVector();

            String s1 = "BW Accelerator";

            v.addElement(new stTitle(s1, themeDark()?C.COLOR_RED:C.COLOR_RED));

            return v;
            
        }
    }
}
