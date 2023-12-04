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
    public class ChartSupertrend: ChartBase
    {
        float[] pAtr;
        float[] pUp;
        float[] pDown;
        float[] prices;
        int[] pTrend;
        float[] pATR;

        public ChartSupertrend(Font f)
            : base(f)
        {
            mChartType = CHART_SUPERTREND;
            //CHART_BORDER_SPACING_Y = CHART_BORDER_SPACING_Y_MASTER;

            pAtr = new float[Share.MAX_CANDLE_CHART_COUNT];

            pUp = new float[Share.MAX_CANDLE_CHART_COUNT];
            pDown = new float[Share.MAX_CANDLE_CHART_COUNT];
            prices = new float[Share.MAX_CANDLE_CHART_COUNT];

            pTrend = new int[Share.MAX_CANDLE_CHART_COUNT];
            

            initBuffers(4);
        }

        public void calcATR(int period, float[] outAtr)
        {
            //int period = appContext.mOptATRLoopback;
            if (period <= 0 || period > 100)
                period = 14;

            if (outAtr == null)
            {
                outAtr = pATR;
            }

            float[] closes = getShare().readCloses(mBuffers[0]);
            float[] hs = getShare().readHighests(mBuffers[1]);
            float[] ls = getShare().readLowest(mBuffers[2]);
            TA.TRUERANGE_Average(closes, hs, ls, getShare().getCandleCnt(), period, outAtr);
        }

        public void calcSuperTrend()
        {
            int Periods = 10;
            float Multiplier = 2.85f;

            Share share = getShare();

            calcATR(Periods, pAtr);

            int cnt = share.getCandleCnt();
            float[] closes = mBuffers[0];
            //    share->readClose(closes, cnt);
            //  use hl2
            for (int i = 0; i < cnt; i++)
            {
                prices[i] = (share.getHighest(i) + share.getLowest(i)) / 2;
                closes[i] = share.getClose(i);
            }

            //up=src-(Multiplier*atr)
            for (int i = 0; i < cnt; i++)
            {
                pUp[i] = prices[i] - (Multiplier * pAtr[i]);
            }
            float[] up = pUp;

            float[] up1 = mBuffers[1];

            //  up1 = nz(up[1],up)
            up1[0] = up[0];
            for (int i = 1; i < cnt; i++)
            {
                //  up1 = nz(up[1],up)
                up1[i] = up[i - 1];
                //  up := close[1] > up1 ? max(up,up1) : up
                up[i] = (closes[i - 1] > up1[i]) ? Math.Max(up[i], up1[i]) : up[i];
            }

            float[] dn = pDown;
            //  dn=src+(Multiplier*atr)
            for (int i = 0; i < cnt; i++)
            {
                dn[i] = prices[i] + (Multiplier * pAtr[i]);
            }
            //  dn1 = nz(dn[1], dn)
            float[] dn1 = mBuffers[2];
            dn1[0] = dn[0];
            for (int i = 1; i < cnt; i++)
            {
                //  dn1 = nz(dn[1], dn)
                dn1[i] = dn[i - 1];
                //  dn := close[1] < dn1 ? min(dn, dn1) : dn
                dn[i] = (closes[i - 1] < dn1[i]) ? Math.Min(dn[i], dn1[i]) : dn[i];
            }

            //trend = 1
            //trend := nz(trend[1], trend)
            //trend := trend == -1 and close > dn1 ? 1 : trend == 1 and close < up1 ? -1 : trend
            int[] trend = pTrend;
            for (int i = 0; i < cnt; i++)
            {
                trend[i] = 1;
            }
            trend[0] = closes[0] > dn1[0] ? 1 : -1;

            for (int i = 1; i < cnt; i++)
            {
                //trend := nz(trend[1], trend)
                trend[i] = trend[i - 1];

                //  trend := trend == -1 and close > dn1 ? 1 : trend == 1 and close < up1 ? -1 : trend
                if (trend[i] == -1 && closes[i] > dn1[i])
                {
                    trend[i] = 1;
                }
                else if (trend[i] == 1 && closes[i] < up1[i])
                {
                    trend[i] = -1;
                }
            }
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
                calcSuperTrend();

                int newSize = mChartLineLength * 2;
                mChartLineXY = allocMem(mChartLineXY, newSize);
                mChartLineXY2 = allocMem(mChartLineXY2, newSize);

                float priceLow = share.getLowestPrice();
                float priceHi = share.getHighestPrice();

                pricesToYs(pUp, share.mBeginIdx, mChartLineXY,
                        mChartLineLength, priceLow, priceHi);

                pricesToYs(pDown, share.mBeginIdx, mChartLineXY2,
                        mChartLineLength, priceLow, priceHi);

            }

            if (mChartLineLength == 0)
                return;

            bool isDark = themeDark();
		    uint[] color = {
				    isDark? C.COLOR_GREEN: C.COLOR_GREEN_DARK,
				    isDark? C.COLOR_RED: C.COLOR_RED};

            int idx = share.mBeginIdx;
            int startTrend = 0;
            int endTrend = 0;
            int currentTrend = pTrend[idx];

            float[] xy = null;
            uint trendColor;

            for (int i = 0; i < mChartLineLength; i++)
            {
                if (pTrend[idx + i] != currentTrend)
                {
                    if (currentTrend == 1)
                    {
                        xy = mChartLineXY;
                        trendColor = color[0];
                    }
                    else
                    {
                        xy = mChartLineXY2;
                        trendColor = color[1];
                    }

                    drawTrend(currentTrend, xy, startTrend, endTrend, trendColor, g);

                    currentTrend = pTrend[idx + i];
                    startTrend = i;   //  new
                }
                endTrend = i;
            }

            if (startTrend <= endTrend)
            {
                if (currentTrend == 1)
                {
                    xy = mChartLineXY;
                    trendColor = color[0];
                }
                else
                {
                    xy = mChartLineXY2;
                    trendColor = color[1];
                }
                drawTrend(currentTrend, xy, startTrend, endTrend, trendColor, g);
            }

		    renderCursor(g);
        }

        void drawTrend(int trend,
                  float[] xy,
                  int start,
                  int end,
                  uint color, xGraphics g)
        {
            g.setColor(color);
            int len = end - start + 1;
            g.drawLines(xy, 2 * start, len, 2.0f);

            if (trend == 1)
            {
                g.setColor(C.COLOR_MAGENTA);
                float r = 1.5f;//xUtils.pointToPixels(3);
                g.drawPoint(xy[2 * start] - r, xy[2 * start + 1] - r, 2 * r);
            }
        }
    }
}
