using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;
using xlib.framework;

namespace stock123.app.chart
{
    class ChartBankerFund: ChartBase
    {
        bool[] _bear;
        bool[] _bull;
        float[] _osc;
        int[] _line;

        public ChartBankerFund(Font f)
            : base(f)
        {
            mChartType = CHART_L3BANKER_FUNDTREND_FLOW;

            _fundtrend = new float[Share.MAX_CANDLE_CHART_COUNT];
            _bullbearline = new float[Share.MAX_CANDLE_CHART_COUNT];
            _bankerentry = new bool[Share.MAX_CANDLE_CHART_COUNT];

            initBuffers(7);
        }

        float[] mPricelines = new float[10];

        float[] _fundtrend;
        float[] _bullbearline;
        bool[] _bankerentry;

        //  https://www.tradingview.com/script/791WkWcm-blackcat-L3-Banker-Fund-Flow-Trend-Oscillator/
        void xsa(float[] src, int period, int cnt, float weight, float[] xsaOut)
        {
            float[] ma = mBuffers[5];
            TA.SMA(src, cnt, period, ma);

            int j = 0;
            xsaOut[j] = ma[0];
            for (int i = 1; i < cnt; i++)
            {
                xsaOut[i] = (src[i] * weight + xsaOut[i - 1] * (period - weight)) / period;
            }
            /*
            float sumf = 0.0;
            float ma = 0.0;
            float outValue = 0.0;
            sumf = nz(sumf[1]) - nz(src[len]) + src
            ma  :=  na(src[len]) ? na : sumf/len
            out  :=  na(out[1]) ? ma : (src*wei+out[1]*(len-wei))/len
            out
             */
        }

        public void calcFundTrend()
        {
            Share share = getShare();
            //  values = [(close-lowest(low,27))/(highest(high,27)-lowest(low,27)]
            float[] values = mBuffers[1];
            int period = 27;
            int cnt = share.getCandleCnt();

            stCandle c = new stCandle();

            for (int i = 0; i < cnt; i++)
            {
                share.getCandle(i, c);
                float lowest = c.lowest;
                float highest = c.highest;
                for (int j = i - period + 1; j <= i; j++)
                {
                    if (j < 0)
                    {
                        continue;
                    }
                    share.getCandle(j, c);
                    if (c.lowest < lowest)
                    {
                        lowest = c.lowest;
                    }
                    if (c.highest > highest)
                    {
                        highest = c.highest;
                    }
                }
                if (highest - lowest > 0)
                {
                    values[i] = 100 * (share.getClose(i) - lowest) / (highest - lowest);
                }
                else
                {
                    values[i] = 0;
                }
            }
            //---------------------------
            float[] xsa5_1 = mBuffers[2];
            xsa(values, 5, cnt, 1, xsa5_1);

            float[] xsa3_1 = mBuffers[3];
            xsa(xsa5_1, 3, cnt, 1, xsa3_1);

            //set up a simple model of banker fund flow trend
            for (int i = 0; i < cnt; i++)
            {
                _fundtrend[i] = (3 * xsa5_1[i] - 2 * xsa3_1[i] - 50) * 1.032f + 50;
            }
            /*
            fundtrend = (
                          (3*xsa((close-lowest(low,27))/(highest(high,27)-lowest(low,27))*100,5,1)
                            -2*xsa(
                                   xsa((close-lowest(low,27))/(highest(high,27)-lowest(low,27))*100,5,1),3,1
                                  ) -50
                          )
                         *1.032+50
                        )
             */

            //define typical price for banker fund
            //typ = (2*close+high+low+open)/5
            float[] typ = mBuffers[2];
            for (int i = 0; i < cnt; i++)
            {
                share.getCandle(i, c);
                typ[i] = (2 * c.close + c.highest + c.lowest + c.open) / 5.0f;
            }

            //lowest low with mid term fib # 34
            period = 34;    //  fib
            float[] lol = mBuffers[3];
            //highest high with mid term fib # 34
            float[] hoh = mBuffers[4];

            for (int i = 0; i < cnt; i++)
            {
                share.getCandle(i, c);
                float lowest = c.lowest;
                float highest = c.highest;
                for (int j = i - period + 1; j <= i; j++)
                {
                    if (j < 0)
                    {
                        continue;
                    }
                    share.getCandle(j, c);
                    if (c.lowest < lowest)
                    {
                        lowest = c.lowest;
                    }
                    if (c.highest > highest)
                    {
                        highest = c.highest;
                    }
                }

                lol[i] = lowest;
                hoh[i] = highest;
            }

            //define banker fund flow bull bear line
            //bullbearline = ema((typ-lol)/(hoh-lol)*100,13)
            float[] t = mBuffers[3];
            for (int i = 0; i < cnt; i++)
            {
                if (hoh[i] - lol[i] > 0)
                {
                    t[i] = (typ[i] - lol[i]) / (hoh[i] - lol[i]) * 100;
                }
                else
                {
                    if (i > 0)
                    {
                        t[i] = t[i - 1];
                    }
                    else
                    {
                        t[i] = 0;
                    }
                }

                //if (i > cnt - 100){
                //NSLog(@"_bullbearline=: %.2f", t[i]);
                //}
            }

            TA.EMA(t, cnt, 13, _bullbearline);
            //  for test only
            /*
            for (int i = cnt - 200; i < cnt; i++)
            {
                NSLog(@"_fundtrend/_bullbearline=: %.2f/%.2f", _fundtrend[i], _bullbearline[i]);
            }
             */

            //define banker entry signal
            //bankerentry = crossover(fundtrend,bullbearline) and bullbearline<25
            _bankerentry[0] = false;
            for (int i = 1; i < cnt; i++)
            {
                _bankerentry[i] = (_fundtrend[i] > _bullbearline[i]
                        && _fundtrend[i - 1] < _bullbearline[i - 1]
                        && _bullbearline[i] < 25
                );
            }



        }

        void drawCandle(xGraphics g,
                    float x,
                    float cw,
                    float v0, float v1,
                    uint color)
        {
            float min = Math.Min(v0, v1);
            float max = Math.Max(v0, v1);

            float y0 = priceToY(max, mLowest, mHighest);
            float y1 = priceToY(min, mLowest, mHighest);

            g.setColor(color);
            g.fillRect(x - cw / 2, y0, cw, y1 - y0);
        }

        public override void render(xGraphics g)
        {
            if (getShare(3) == null)
                return;
            Share share = getShare();

            float mX = 0;
            int mY = 0;

            mLowest = -10;
            mHighest = 100;

            if (detectShareCursorChanged())
            {
                calcFundTrend();

                if (mChartLineXY == null)
                {
                    mChartLineXY = new float[MAX_DRAW_POINT];
                }
                mChartLineXY = allocMem(mChartLineXY, 2 * mChartLineLength);
                mChartLineXY2 = allocMem(mChartLineXY2, 2 * mChartLineLength);

                double rw = (float)getDrawingW() / mChartLineLength;
                mVolumeBarW = (((float)getDrawingW() / mChartLineLength) * 2.0f / 3);
                if (mVolumeBarW < 1) mVolumeBarW = 1;
                float volumeBarWHalf = mVolumeBarW / 2;

                int idx = share.mBeginIdx;
                pricesToYs(_fundtrend, idx, mChartLineXY, mChartLineLength, mLowest, mHighest);
                pricesToYs(_bullbearline, idx, mChartLineXY2, mChartLineLength, mLowest, mHighest);
            }

            //  0/50/80
            String[] ss = { "0", "50", "80" };
            float[] v = { 0, 50, 80 };
            pricesToYs(v, 0, mPricelines, 3, mLowest, mHighest);

            for (int i = 0; i < 3; i++)
            {
                g.setColor(colorPriceline());
                g.drawLine(0, mPricelines[2 * i + 1], getW()-20, mPricelines[2 * i + 1], 0.6f);

                g.setColor(colorPriceLabel());
                g.drawString(mFont, ss[i], getW() - 4, mPricelines[2 * i + 1], xGraphics.VCENTER | xGraphics.RIGHT);
            }

            //------------------


            for (int i = 1; i < mChartLineLength; i++)
            {
                int j = (i + share.mBeginIdx);

                mChartLineXY[i * 2] = candleIdxToX(j);

                //banker fund entry with yellow candle
                //plotcandle(0,50,0,50,color=bankerentry ? color.new(color.yellow,0):na)
                if (_bankerentry[j])
                {
                    //y1 = priceToY(0, min, max);
                    //y0 = priceToY(50, min, max);

                    drawCandle(g, mChartLineXY[2 * i], mVolumeBarW, 0, 50, C.COLOR_YELLOW_DARK);
                }

                //banker increase position with green candle
                //plotcandle(fundtrend,bullbearline,fundtrend,bullbearline,color=fundtrend>bullbearline ? color.new(color.green,0):na)

                if (_fundtrend[j] > _bullbearline[j])
                {
                    g.setColor(C.COLOR_GREEN_DARK);

                    drawCandle(g, mChartLineXY[2 * i], mVolumeBarW, _bullbearline[j], _fundtrend[j], C.COLOR_GREEN_DARK);
                }

                //banker decrease position with white candle
                //plotcandle(fundtrend,bullbearline,fundtrend,bullbearline,color=fundtrend<(xrf(fundtrend*0.95,1)) ? color.new(color.white,0):na)
                float _xrfValue = (float)(_fundtrend[j - 1] * 0.95);//xrf(_fundtrend, 1, 0.95f);
                if (_fundtrend[j] < _xrfValue)
                {
                    drawCandle(g, mChartLineXY[2 * i], mVolumeBarW, _bullbearline[j], _fundtrend[j], C.COLOR_GRAY_LIGHT);
                }

                //banker fund exit/quit with red candle
                //plotcandle(fundtrend,bullbearline,fundtrend,bullbearline,color=fundtrend<bullbearline ? color.new(color.red,0):na)
                if (_fundtrend[j] < _bullbearline[j])
                {
                    drawCandle(g, mChartLineXY[2 * i], mVolumeBarW, _bullbearline[j], _fundtrend[j], C.COLOR_RED_DARK);
                }

                //banker fund Weak rebound with blue candle
                //plotcandle(fundtrend,bullbearline,fundtrend,bullbearline,color=fundtrend<bullbearline and fundtrend>(xrf(fundtrend*0.95,1)) ? color.new(color.blue,0):na)
                _xrfValue = (float)(_fundtrend[j - 1] * 0.95);//xrf(_fundtrend, 1, 0.95f);
                if (_fundtrend[j] < _bullbearline[j] && _fundtrend[j] > _xrfValue)
                {
                    drawCandle(g, mChartLineXY[2 * i], mVolumeBarW, _bullbearline[j], _fundtrend[j], C.COLOR_BLUE);
                }

            }

            renderCursor(g);
        }

        public override xVector getTitles()
        {
            xVector v = new xVector(1);

            int sel = getShare().mSelectedCandle;
            if (sel >= 0 && sel < getShare().getCandleCnt()) {
                v.addElement(new stTitle(String.Format("Banker Fund: {0:F1}", _fundtrend[sel]), colorTitle()));
            }
            else{
                v.addElement(new stTitle("Banker Fund", C.COLOR_GRAY));
            }

            return v;
        }

    }
}
