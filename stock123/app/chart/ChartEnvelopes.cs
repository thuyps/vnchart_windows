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
    public class ChartEnvelopes: ChartBase
    {
        float[][] mLines = { null, null, null, null, null, null, null };

        //====================================
        public ChartEnvelopes(Font f)
            : base(f)
        {
            mChartType = CHART_ENVELOP;

            for (int i = 0; i < 7; i++)
            {
                mLines[i] = null;
            }
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
                share.calcEnvelop();

                float[] percent = { 110, 105, 102.5f, 100, 97.5f, 95.0f, 90 };
                float[] p = share.pTMP;
                float min = share.getLowestPrice();
                float max = share.getHighestPrice();
                for (int i = 0; i < 7; i++)
                {
                    mLines[i] = allocMem(mLines[i], mChartLineLength * 2 + 10);

                    for (int j = 0; j < mChartLineLength; j++)
                    {
                        p[j] = (percent[i] * share.pSMA_Envelop[share.mBeginIdx + j]) / 100;
                    }

                    pricesToYs(p, 0, mLines[i], mChartLineLength, min, max);
                }
            }

            if (mChartLineLength == 0)
                return;

            int mX = 0;
            int mY = 0;
            int mW = getW();
            int mH = getH();

            uint[] color = {0xffff00ff, 0xff00a000, 0xffa00000, 0xffff8000, 0xffa00000, 0xff00a000, 0xffff00ff};
            bool[] draws = { false, false, false, true, false, false, false };
            if (mContext.mOptEnvelopLine0[0])   //  2.5%
            {
                draws[2] = draws[4] = true;
            }
            if (mContext.mOptEnvelopLine1[0])  //  5%
            {
                draws[1] = draws[5] = true;
            }
            if (mContext.mOptEnvelopLine2[0])  //  10%
            {
                draws[0] = draws[6] = true;
            }
            for (int i = 0; i < 7; i++)
            {
                if (draws[i])
                {
                    g.setColor(color[i]);
                    g.drawLinesDot(mLines[i], mChartLineLength);
                }
            }
        }
    }
}
