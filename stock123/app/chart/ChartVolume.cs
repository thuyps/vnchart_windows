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
    public class ChartVolume: ChartBase
    {
        String mHighestVolume;
        //=======================================
        public ChartVolume(Font f)
            : base(f)
        {
        }

        float[] mSMAVolumeXY;

        double biggest = 0;
        double lowest = -1;
        public override void render(xGraphics g)
        {
            if (isHiding())
                return;
            if (getShare(3) == null)
                return;
            Share share = getShare();
            int mX = 0;
            int mY = 0;

            Utils.trace(String.Format("volume render: {0}", getStartX()));

            if (detectShareCursorChanged())
            {
                mHighestVolume = "";
                if (mChartLineXY == null)
                    mChartLineXY = new float[2 * MAX_DRAW_POINT];

                //	get biggest volume
                biggest = 0;
                lowest = -1;
                int vol;
                int i, j;
                for (i = share.mBeginIdx; i <= share.mEndIdx; i++)
                {
                    vol = share.getVolume(i);
                    if (lowest == -1)
                        lowest = vol;
                    if (biggest < vol)
                        biggest = vol;
                    if (lowest > vol)
                        lowest = vol;
                }

                if (biggest == 0)
                    return;

                StringBuilder sb = Utils.sb;
                sb.Length = 0;

                lowest -= 1000;

                if (lowest < 0) lowest = 0;

                if (biggest < 1000)
                {
                    sb.Append(biggest);
                }
                else if (biggest < 1000000)
                    sb.AppendFormat("{0}KB", (int)(biggest / 1000));
                else
                    sb.AppendFormat("{0}M", (int)(biggest / 1000000));

                mHighestVolume = sb.ToString();

                lowest /= 4;

                double ry = (float)getDrawingH() / (biggest - lowest);
                double rw = (float)getDrawingW() / mChartLineLength;
                mVolumeBarW = (((float)getDrawingW() / mChartLineLength) * 2.0f / 3);

                if (mVolumeBarW < 1) mVolumeBarW = 1;
                float volumeBarWHalf = mVolumeBarW / 2;
                int vH = 0;
                int vL = 0xffffff;
                for (i = 0; i < mChartLineLength; i++)
                {
                    j = (i + share.mBeginIdx);
                    mChartLineXY[i * 2] = (float)(mX + CHART_BORDER_SPACING_X + i * rw + getStartX() - volumeBarWHalf);	//	x
                    mChartLineXY[i * 2 + 1] = (float)(mY + getMarginY() + getDrawingH() - (share.getVolume(j) - lowest) * ry);

                    if (share.mCVolume[j] > vH) vH = share.mCVolume[j];
                    if (share.mCVolume[j] < vL) vL = share.mCVolume[j];
                }

                //==========================    
                mSMAVolumeXY = allocMem(mSMAVolumeXY, share.getCandleCount() * 2);
                share.calcSMAVolume(0, share.getCandleCount());    
                mCurrentShare = share;

                pricesToYs(share.pSMAVolume, share.mBeginIdx, mSMAVolumeXY, mChartLineLength, vL, vH); 
            }

            if (mShouldDrawGrid)
                drawGrid(g);

            //g.setColor(0xffff00ff);
            //g.drawLine(0, 150, getW(), 150);

            g.setColor(0xff00ff00);
            float tmp = mY + getH() - getMarginY();

            for (int i = 0; i < mChartLineLength; i++)
            {
                if (share.mBeginIdx + i > 0)
                {
                    if (share.getClose(share.mBeginIdx + i) > share.getClose(share.mBeginIdx + i - 1))
                        g.setColor(0xff00c000);
                    else if (share.getClose(share.mBeginIdx + i) < share.getClose(share.mBeginIdx + i - 1))
                        g.setColor(0xffa00000);
                    else
                        g.setColor(0xffa08000);
                }
                g.fillRectF(mChartLineXY[2 * i], mChartLineXY[2 * i + 1], mVolumeBarW, tmp - mChartLineXY[2 * i + 1]);
            }

            //  sma
            g.setColor(C.COLOR_ORANGE);
            g.drawLines(mSMAVolumeXY, mChartLineLength);

            g.setColor(C.COLOR_GRAY_LIGHT);
            g.drawString(mFont, mHighestVolume, getW() - 8, 0, xGraphics.RIGHT | xGraphics.TOP);
            g.drawString(mFont, "0", getW() - 8, getH(), xGraphics.RIGHT | xGraphics.BOTTOM);

            //  calc mMouseTitle
            mMouseTitle = Utils.formatNumber((int)yToPrice(mLastY, lowest, biggest));

            renderCursor(g);
        }

        public override xVector getTitles()
        {
            xVector v = new xVector(1);
            Share share = getShare(3);
            if (share != null)
            {
                int idx = share.getCursor();
                {
                    long vol = share.getVolume(idx);
                    vol *= share.mVolumeDivided;

                    String s = Utils.formatNumber(vol);
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat("Vol: {0}", s);

                    v.addElement(new stTitle(Utils.sb.ToString(), 0xffffffff));
                }
            }
            return v;
        }
    }
}
