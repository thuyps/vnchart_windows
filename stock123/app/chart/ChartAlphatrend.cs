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
    public class ChartAlphatrend: ChartBase
    {
        float[] pAtr;
        float[] pUp;
        float[] pDown;
        float[] prices;
        int[] pTrend;
        float[] pATR;
        float[] pAlphatrend;
        float[] pAlphatrend2;

        public ChartAlphatrend(Font f)
            : base(f)
        {
            mChartType = CHART_ALPHATREND;
            //CHART_BORDER_SPACING_Y = CHART_BORDER_SPACING_Y_MASTER;

            pAtr = new float[Share.MAX_CANDLE_CHART_COUNT];

            pUp = new float[Share.MAX_CANDLE_CHART_COUNT];
            pDown = new float[Share.MAX_CANDLE_CHART_COUNT];
            prices = new float[Share.MAX_CANDLE_CHART_COUNT];

            pTrend = new int[Share.MAX_CANDLE_CHART_COUNT];
            pAlphatrend = new float[Share.MAX_CANDLE_CHART_COUNT];
            pAlphatrend2 = new float[Share.MAX_CANDLE_CHART_COUNT];

            initBuffers(4);
        }

        public void calcAlphatrend()
        {
            int Periods = 14;
            float Multiplier = 1.0f;

            Share share = getShare();
            float[] closes = share.readCloses(mBuffers[0]);
            float[] hi = share.readHighests(mBuffers[1]);
            float[] low = share.readLowest(mBuffers[2]);

            //--------------
            int cnt = getShare().getCandleCnt();
            float[] pTmp = pAlphatrend;
            TA.TRUERANGE(closes, hi, low, cnt, pTmp);
            TA.SMA(pTmp, cnt, Periods, pAtr);
            //--------------

            float[] rsi = mBuffers[3];
            TA.calcRSI(closes, cnt, Periods, rsi);

            for (int i = 0; i < cnt; i++)
            {
                pUp[i] = low[i] - (Multiplier * pAtr[i]);
                pDown[i] = hi[i] + (Multiplier * pAtr[i]);
            }

            pAlphatrend[0] = (pUp[0] + pDown[0]) / 2;
            pAlphatrend2[0] = pAlphatrend[0];
            pAlphatrend2[1] = pAlphatrend[0];
            for (int i = 1; i < cnt; i++)
            {
                if (rsi[i] >= 50)
                {
                    //  alphatrend increasing
                    pAlphatrend[i] = (pUp[i] < pAlphatrend[i - 1]) ? pAlphatrend[i - 1] : pUp[i];
                }
                else
                {
                    //  alphatrend decreasing
                    pAlphatrend[i] = (pDown[i] > pAlphatrend[i - 1]) ? pAlphatrend[i - 1] : pDown[i];
                }
                if (i > 1)
                {
                    pAlphatrend2[i] = pAlphatrend[i - 2];
                }
            }

            //trend = 1
            //trend := nz(trend[1], trend)
            //trend := trend == -1 and close > dn1 ? 1 : trend == 1 and close < up1 ? -1 : trend
            pTrend[0] = 1;
            pTrend[1] = 1;
            for (int i = 2; i < cnt; i++)
            {
                if (i == 519)
                {
                    //xUtils.trace("");
                }
                if (pAlphatrend[i] > pAlphatrend[i - 2])
                {
                    pTrend[i] = 1;
                }
                else
                {
                    if (pAlphatrend[i] < pAlphatrend[i - 2])
                    {
                        pTrend[i] = -1;
                    }
                    else if (i >= 3)
                    {
                        if (pAlphatrend[i - 1] > pAlphatrend[i - 3])
                        {
                            pTrend[i] = 1;
                        }
                        else if (pAlphatrend[i - 1] < pAlphatrend[i - 3])
                        {
                            pTrend[i] = -1;
                        }
                        else
                        {
                            pTrend[i] = pTrend[i - 1];
                        }
                    }
                    else
                    {
                        pTrend[i] = 1;
                    }
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
                calcAlphatrend();

                int newSize = mChartLineLength * 2;
                mChartLineXY = allocMem(mChartLineXY, newSize);
                mChartLineXY2 = allocMem(mChartLineXY2, newSize);

                //float priceLow = share.getLowestPriceInRange();
                //float priceHi = share.getHighestPriceInRange();
                float priceLow = share.getLowestPrice();
                float priceHi = share.getHighestPrice();

                pricesToYs(pAlphatrend, share.mBeginIdx, mChartLineXY,
                        mChartLineLength, priceLow, priceHi);

                pricesToYs(pAlphatrend2, share.mBeginIdx, mChartLineXY2,
                        mChartLineLength, priceLow, priceHi);

            }

            if (mChartLineLength == 0)
                return;

            bool isDark = themeDark();
		    uint[] color = {
				    isDark? C.COLOR_GREEN: C.COLOR_GREEN_DARK,
				    isDark? C.COLOR_RED: C.COLOR_RED};

		    g.setColor(0xffffff00);
		    g.drawLines(mChartLineXY, mChartLineLength, 1.5f);
		    g.setColor(0xffff00ff);
		    g.drawLines(mChartLineXY2, mChartLineLength, 1.5f);

		    int offset = share.mBeginIdx;

		    uint colorUp = 0x9000A60F;
		    uint colorDown = 0x90A0000B;

		    fillShape2Lines(g, mChartLineXY, mChartLineXY2, mChartLineLength, pTrend, offset, colorUp, colorDown);
		    drawShape2Lines(g, mChartLineXY, mChartLineXY2, mChartLineLength, pTrend, offset, 0xffffff00, 0xffff00ff);

		    renderCursor(g);
        }
    }
}
