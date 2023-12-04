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
    public class ChartBWAwesome : ChartBase
    {
        float[] AO;

        //===================================
        public ChartBWAwesome(Font f)
            : base(f)
        {
            mShouldDrawCursor = true;
            mChartType = CHART_AWESOME;
            //CHART_BORDER_SPACING_Y = 1;
            AO = new float[Share.MAX_CANDLE_CHART_COUNT];
        }

        void calcAO()
        {
            Share share = getShare();
            if (share.getCandleCnt() == 0)
            {
                return;
            }

            int period1 = GlobalData.getData().getValueInt(GlobalData.kBWAwesomeShortPeriod, 5);
            int period2 = GlobalData.getData().getValueInt(GlobalData.kBWAwesomeLongPeriod, 34);
            if (period1 == 0)
            {
                period1 = 5;
            }
            if (period2 == 0)
            {
                period2 = 34;
            }

            share.calcAO(period1, period2, AO);
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

                calcAO();
                mHighest = AO[0];
                mLowest = AO[0];
                for (int i = share.mBeginIdx; i <= share.mEndIdx; i++)
                {
                    if (AO[i] > mHighest)
                    {
                        mHighest = AO[i];
                    }
                    if (AO[i] < mLowest)
                    {
                        mLowest = AO[i];
                    }
                }

                double ry = (float)getDrawingH() / (mHighest - mLowest);
                double rw = (float)getDrawingW() / mChartLineLength;
                mVolumeBarW = (((float)getDrawingW() / mChartLineLength) * 2.0f / 3);
                if (mVolumeBarW < 1) mVolumeBarW = 1;
                float volumeBarWHalf = mVolumeBarW / 2;

                for (int i = 0; i < mChartLineLength; i++)
                {
                    int j = (i + share.mBeginIdx);

                    mChartLineXY[i * 2] = (float)(mX + CHART_BORDER_SPACING_X + i * rw + getStartX() - volumeBarWHalf);	//	x

                    mChartLineXY[i * 2 + 1] = (float)(mY + getMarginY() + getDrawingH() - (AO[j] - mLowest) * ry);
                }
            }

            g.setColor(0xff00ff00);
            //int tmp = mY + getH() - (CHART_BORDER_SPACING_Y) - Y0Means();
            uint colorUp = themeDark() ? (C.COLOR_TD_UP & 0x90ffffff) : C.COLOR_TL_UP;
            uint colorDown = themeDark() ? (C.COLOR_TD_DOWN & 0xa0ffffff) : C.COLOR_TL_DOWN;

            uint preColor = colorUp;
            uint color;

            //  zero line
            float y0 = priceToY(0, mLowest, mHighest);
            g.setColor(colorPriceline());
            g.drawHorizontalLine(0, (float)y0, getW() - 20);

            g.drawHistogram(mChartLineXY, 0, mChartLineLength, y0, mVolumeBarW,
                colorUp, colorUp, colorDown, colorDown); 

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

            String s1 = "BW Awesome";

            v.addElement(new stTitle(s1, themeDark() ? C.COLOR_RED : C.COLOR_RED));

            return v;

        }
    }
}
