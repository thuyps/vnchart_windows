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
    public class ChartHeikenAShiEMA: ChartBase
    {
        float[] _o2;
        float[] _h2;
        float[] _c2;
        float[] _l2;

        public ChartHeikenAShiEMA(Font f)
            : base(f)
        {
            mChartType = CHART_HEIKEN_ASHI_EMA;
            //CHART_BORDER_SPACING_Y = CHART_BORDER_SPACING_Y_MASTER;

            initBuffers(8);
            _o2 = mBuffers[0];
            _c2 = mBuffers[1];
            _h2 = mBuffers[2];
            _l2 = mBuffers[3];
        }

        public void calcSmoothedHeiken()
        {
            Share share = getShare();
            if (share.getCandleCnt() < 30)
            {
                return;
            }
            int cnt = share.getCandleCnt();

            float[] haclose = mBuffers[4];
            float[] haopen = mBuffers[5];
            float[] hahigh = mBuffers[6];
            float[] halow = mBuffers[7];

            float[] prices = haclose;

            int period1 = 10;//configValueInt(kChartParam1.UTF8String);
            int period2 = 10;//configValueInt(kChartParam2.UTF8String);

            share.readOpens(prices);
            TA.EMA(prices, cnt, period1, _o2);

            share.readCloses(prices);
            TA.EMA(prices, cnt, period1, _c2);

            share.readHighests(prices);
            TA.EMA(prices, cnt, period1, _h2);

            share.readLowest(prices);
            TA.EMA(prices, cnt, period1, _l2);

            for (int i = 0; i < cnt; i++)
            {
                haclose[i] = (_o2[i] + _h2[i] + _l2[i] + _c2[i]) / 4;

                if (i == 0)
                {
                    haopen[i] = (_o2[i] + _c2[i]) / 2;
                }
                else
                {
                    haopen[i] = (haopen[i - 1] + haclose[i - 1]) / 2;
                }

                hahigh[i] = Math.Max(_h2[i], Math.Max(haopen[i], haclose[i]));

                halow[i] = Math.Min(_l2[i], Math.Min(haopen[i], haclose[i]));
            }

            //
            TA.EMA(haopen, cnt, period2, _o2);
            TA.EMA(haclose, cnt, period2, _c2);
            TA.EMA(halow, cnt, period2, _l2);
            TA.EMA(hahigh, cnt, period2, _h2);
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
                calcSmoothedHeiken();
            }

            if (mChartLineLength == 0)
                return;

            //-------draw candles-------------
            float candleW = (((float)getDrawingW() / mChartLineLength) * 2.0f / 3);

            if (candleW % 2 == 0) candleW++;

            Share s = getShare();
            int b = s.mBeginIdx;
            int e = s.mEndIdx;

            float candleBodyH = 0;
            uint colorUp = C.COLOR_GREEN_DARK;
            uint colorDown = C.COLOR_RED;
            uint color;

            bool isUptrend = false;
            bool trendReverted = false;

            float x, y;

            float ry = (float)getDrawingH() / mPriceDistance;
            float rw = (float)getDrawingW() / mChartLineLength;

            int j = 0;
            float ho, hc, hh, hl;

            float yEnter = 0;
            float yExit = 0;

            for (int i = b; i <= e; i++)
            {
                trendReverted = false;

                hc = _c2[i];
                ho = _o2[i];
                hh = _h2[i];
                hl = _l2[i];

                //-------------------------
                candleBodyH = hc - ho;
                candleBodyH = candleBodyH * ry;

                //  trend & color
                color = colorUp;

                if (candleBodyH < 0)
                {
                    color = colorDown; candleBodyH = -candleBodyH;
                    y = priceToY(ho);

                    if (isUptrend)
                    {
                        trendReverted = true;
                        yEnter = priceToY(ho + 4 * ho / 100.0f);
                    }
                    isUptrend = false;
                }
                else
                {
                    y = priceToY(hc);
                    if (!isUptrend)
                    {
                        trendReverted = true;
                        yExit = priceToY(hc - 4 * hc / 100.0f);
                    }
                    isUptrend = true;
                }
                if (candleBodyH == 0) candleBodyH = 1;

                x = (float)(0 + CHART_BORDER_SPACING_X + j * rw - candleW / 2 + getStartX());
                //x += 1;

                //	candle body
                g.setColor(color);

                if (candleBodyH < 1.2f)
                {
                    candleBodyH = 1.2f;
                }

                g.fillRect(x, y, candleW, candleBodyH);
                if (trendReverted)
                {
                    g.setColor(C.COLOR_MAGENTA);
                    //g.drawRect(x, y, candleW, candleBodyH);
                    g.drawPoint(x - 4, y + candleBodyH / 2, 4);
                }

                j++;
            }

		    renderCursor(g);
        }

    }
}
