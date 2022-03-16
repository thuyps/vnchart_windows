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
    public class ChartZigzag: ChartBase
    {
        xVectorInt mZigzag;

        float mCurrentZigzagPercent;
        int mZigzagPointCnt;

        Share currentShare;

        public ChartZigzag(Font f)
            : base(f)
        {
            mChartType = CHART_ZIGZAG;
        }

        public void calcZigzag()
        {
            Share share = getShare(3);
            if (share == null)
                return;
            if (mZigzag != null && currentShare == share && mCurrentZigzagPercent == mContext.mOptZigzagPercent)
            {
            }
            else
            {
                mZigzag = null;

                currentShare = share;
                mCurrentZigzagPercent = mContext.mOptZigzagPercent;

                mZigzag = share.calcZigzag();
            }
        }

        override public void render(xGraphics g)
        {
            if (isHiding())
                return;


            Share share = getShare(3);
            if (share == null)
                return;

            int mX = 0;
            int mY = 0;
            //	update line ordinates
            if (detectShareCursorChanged() && mChartLineLength > 0)		//	share's cursor has been changed
            {                    
                calcZigzag();
                //=======================
                int items = (mChartLineLength + 10)* 2;

                if (items < 2*mZigzag.size() + 100){
                    items = mZigzag.size()*2 + 100;
                }

                mZigzagPointCnt = 0;

                mChartLineXY = allocMem(mChartLineXY, items);

                mChartLineLength = 0;

                if (mZigzag != null && mZigzag.size() > 0)
                {
                    //  seek to the nearest point
                    int i;
                    float low = share.getLowestPrice();
                    float rY = (float)getDrawingH() / mPriceDistance;
                    float rX = (float)getDrawingW() / mChartLineLength;
                    int begin = -1;
                    for (i = 0; i < mZigzag.size(); i++)
                    {
                        int idx = mZigzag.elementAt(i);
                        if (idx >= share.mBeginIdx)
                        {
                            begin = i-1;
                            break;
                        }
                    }

                    if (begin < 0)
                        begin = 0;
                    mChartLineLength = 0;
                    for (i = begin; i < mZigzag.size(); i++)
                    {
                        int idx = mZigzag.elementAt(i);

                        mChartLineXY[2 * mZigzagPointCnt] = (float)candleToX(idx);
                        float close = share.getClose(mZigzag.elementAt(i));
                        mChartLineXY[2 * mZigzagPointCnt + 1] = (float)priceToY(close);

                        mZigzagPointCnt++;

                        if (mChartLineXY[2 * (mZigzagPointCnt-1)] > mX + getW())
                            break;
                    }
                }
            }
            //==========draw now
            if (mZigzagPointCnt > 0)
            {
                g.setColor(C.COLOR_RED_ORANGE);
                g.drawLines(mChartLineXY, mZigzagPointCnt, 2.0f);
            }
        }
    }
}
