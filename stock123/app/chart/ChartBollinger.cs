using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;

namespace stock123.app.chart
{
    public class ChartBollinger: ChartBase
    {	
	    short[] mBBUpperXY;	//	bollinger band
	    short[] mBBLowerXY;
        short[] mCenterXY;
	    short[] mBBLine;
	    uint mBBColorBG;
        int mPointCnt;
        //====================================
        public ChartBollinger(Font f)
            : base(f)
        {
            mChartType = CHART_BOLLINGER;

            mBBColorBG = 0x20F8D4CF;
            mBBUpperXY = null;
            mBBLowerXY = null;
            mBBLine = null;
            mPointCnt = 0;
            CHART_BORDER_SPACING_Y = CHART_BORDER_SPACING_Y_MASTER;
        }

        public override void render(xGraphics g)
        {
            if (isHiding())
                return;
            if (getShare(3) == null)
                return;

            Share share = getShare();
            if (share == null)
                return;

            if (detectShareCursorChanged())
            {
                share.calcBollinger();

                mBBUpperXY = allocMem(mBBUpperXY, mChartLineLength * 2 + 10);
                mBBLowerXY = allocMem(mBBLowerXY, mChartLineLength * 2 + 10);
                mCenterXY = allocMem(mCenterXY, mChartLineLength * 2 + 10);
                mBBLine = allocMem(mBBLine, mChartLineLength * 4 + 10);

                pricesToYs(share.pBBUpper, share.mBeginIdx, mBBUpperXY, mChartLineLength, share.getLowestPrice(), share.getHighestPrice());
                pricesToYs(share.pBBLower, share.mBeginIdx, mBBLowerXY, mChartLineLength, share.getLowestPrice(), share.getHighestPrice());

                int j = 0;

                j = 0;

                for (int i = 0; i < mChartLineLength; i++)
                {
                    mBBLine[j++] = mBBUpperXY[2 * i];
                    mBBLine[j++] = mBBUpperXY[2 * i + 1];

                    mCenterXY[2 * i] = mBBLowerXY[2 * i];
                    mCenterXY[2*i+1] = (short)((mBBUpperXY[2*i+1] + mBBLowerXY[2*i+1])/2);
                }
                mBBLine[j++] = mBBUpperXY[2 * (mChartLineLength - 1)];      //  x
                mBBLine[j++] = mBBLowerXY[2 * (mChartLineLength - 1) + 1];    //  y
                for (int i = mChartLineLength - 1; i >= 0; i--)
                {
                    mBBLine[j++] = mBBLowerXY[2 * i];
                    mBBLine[j++] = mBBLowerXY[2 * i + 1];
                }

                mPointCnt = j / 2;
            }

            if (mChartLineLength == 0)
                return;

            int mX = 0;
            int mY = 0;
            int mW = getW();
            int mH = getH();

            g.setColor(mBBColorBG);
            g.fillShapes(mBBLine, mPointCnt);

            g.setColor(0xa00080ff);
            g.drawLinesDot(mCenterXY, mChartLineLength);

            g.setColor(0xff752922);
            g.drawLines(mBBUpperXY, mChartLineLength);
            g.drawLines(mBBLowerXY, mChartLineLength);

            //=================================
            int cur = share.getCursor();
            if (mShouldDrawValue && cur >= 0 && cur < share.getCandleCount())
            {
                g.setColor(0xffffa0a0);

                String s1 = formatPrice(share.pBBUpper[cur]);
                String s2 = formatPrice(share.pBBLower[cur]);
                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                sb.AppendFormat("BB({0},{1}) U:{2}     L:{3}", (int)mContext.mOptBBPeriod, (int)mContext.mOptBBD, s1, s2);
                s1 = sb.ToString();
                g.drawString(mFont, s1, 150, 12, 0);
            }
        }
    }
}
