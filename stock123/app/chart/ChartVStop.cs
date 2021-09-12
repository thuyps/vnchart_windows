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
    public class ChartVSTOP: ChartBase
    {
        public ChartVSTOP(Font f)
            : base(f)
        {
            mChartType = CHART_VSTOP;
            //CHART_BORDER_SPACING_Y = CHART_BORDER_SPACING_Y_MASTER;
        }

        public override void render(xGraphics g)
        {
            if (isHiding())
                return;
            if (getShare(3) == null)
                return;

            Share share = getShare();
            if (detectShareCursorChanged())
            {
                share.calcVSTOP(0, 0);

                int newSize = mChartLineLength * 2;
                mChartLineXY = allocMem(mChartLineXY, newSize);

                pricesToYs(share.pVSTOP, share.mBeginIdx, mChartLineXY, mChartLineLength, false);
            }

            if (mChartLineLength == 0)
                return;

            int mX = 0;
            int mY = 0;

            g.setColor(0xffff0000);

            uint color;
            for (int i = 0; i < mChartLineLength; i++)
            {
                if (share.pVSTOP_SignalUp[share.mBeginIdx + i])	//	is up
                {
                    color = 0xff00ff00;
                }
                else
                    color = 0xffff0000;

                g.setColor(color);
                g.drawPoint(mChartLineXY[2 * i], mChartLineXY[2 * i + 1], 2);
            }

            //  info
            int cur = share.getCursor();
            if (mShouldDrawValue && cur >= 0 && cur < share.getCandleCount())
            {
                if (share.pVSTOP_SignalUp[cur])
                    g.setColor(0xff00a000);
                else
                    g.setColor(0xffa00000);

                String s = formatPrice(share.pVSTOP[cur]);
                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                sb.AppendFormat("VSTOP({0},{1:F2}) {2}", (int)mContext.mOptVSTOP_ATR_Loopback, mContext.mOptVSTOP_MULT, s);
                s = sb.ToString();
                g.drawString(mFont, s, 375, 12, 0);
            }
        }
    }
}
