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
    class ChartXTrender: ChartBase
    {
        bool[] _bear;
        bool[] _bull;
        float[] _osc;
        int[] _line;

        public ChartXTrender(Font f)
            : base(f)
        {
            mChartType = CHART_XTRENDER;

            initBuffers(10);

            _shortTermXtrender = new float[Share.MAX_CANDLE_CHART_COUNT];
            _maShortTermXtrender = new float[Share.MAX_CANDLE_CHART_COUNT];
            _longTermXtrender = new float[Share.MAX_CANDLE_CHART_COUNT];
        }

        float[] _shortTermXtrender;  //  histogram
        float[] _maShortTermXtrender;
        float[] _longTermXtrender;

        public void calcBExtrender(){
            Share share = getShare();

            int cnt = share.getCandleCnt();
            if (cnt == 0){
                return;
            }

            int short_l1 = 5;
            int short_l2 = 20;
            int short_l3 = 15;

            int long_l1 = 20;
            int long_l2 = 15;

            //===============================
            //shortTermXtrender = rsi(ema(close, short_l1) - ema(close, short_l2), short_l3 ) - 50
            float[] closes = mBuffers[0];
            share.readCloses(closes);

            float[] ema_l1 = mBuffers[1];
            float[] ema_l2 = mBuffers[2];
            float[] ema_l1_l2 = mBuffers[3];

            TA.EMA(closes, cnt, short_l1, ema_l1);
            TA.EMA(closes, cnt, short_l2, ema_l2);
            for (int i = 0; i < cnt; i++)
            {
                ema_l1_l2[i] = ema_l1[i] - ema_l2[i];
            }

            float[] rsi_short = mBuffers[4];
            TA.calcRSI(ema_l1_l2, cnt, short_l3, rsi_short);
            for (int i = 0; i < cnt; i++)
            {
                _shortTermXtrender[i] = rsi_short[i] - 50;
            }

            //longTermXtrender  = rsi( ema(close, long_l1), long_l2 ) - 50
            TA.EMA(closes, cnt, long_l1, ema_l1);
            float[] rsi_long = mBuffers[2];
            TA.calcRSI(ema_l1, cnt, long_l2, rsi_long);
            for (int i = 0; i < cnt; i++)
            {
                _longTermXtrender[i] = rsi_long[i] - 50;
            }

            float[] xe1_1 = mBuffers[5];
            float[] xe2_1 = mBuffers[6];
            float[] xe3_1 = mBuffers[7];
            float[] xe4_1 = mBuffers[8];
            float[] xe5_1 = mBuffers[9];
            float[] xe6_1 = mBuffers[0];

        /*
        t3(src, len)=>
            xe1_1 = ema(src,    len)
            xe2_1 = ema(xe1_1,  len)
            xe3_1 = ema(xe2_1,  len)
            xe4_1 = ema(xe3_1,  len)
            xe5_1 = ema(xe4_1,  len)
            xe6_1 = ema(xe5_1,  len)
            b_1 = 0.7
            c1_1 = -b_1*b_1*b_1
            c2_1 = 3*b_1*b_1+3*b_1*b_1*b_1
            c3_1 = -6*b_1*b_1-3*b_1-3*b_1*b_1*b_1
            c4_1 = 1+3*b_1+b_1*b_1*b_1+3*b_1*b_1
            nT3Average_1 = c1_1 * xe6_1 + c2_1 * xe5_1 + c3_1 * xe4_1 + c4_1 * xe3_1
            */
            //maShortTermXtrender = t3( shortTermXtrender , 5 )
            int len = 5;
            TA.EMA(_shortTermXtrender, cnt, len, xe1_1);
            TA.EMA(xe1_1, cnt, len, xe2_1);
            TA.EMA(xe2_1, cnt, len, xe3_1);
            TA.EMA(xe3_1, cnt, len, xe4_1);
            TA.EMA(xe4_1, cnt, len, xe5_1);
            TA.EMA(xe5_1, cnt, len, xe6_1);
            float b_1 = 0.7f;
            float c1_1;
            float c2_1;
            float c3_1;
            float c4_1;
            c1_1 = -b_1*b_1*b_1;
            c2_1 = 3*b_1*b_1+3*b_1*b_1*b_1;
            c3_1 = -6*b_1*b_1-3*b_1-3*b_1*b_1*b_1;
            c4_1 = 1+3*b_1+b_1*b_1*b_1+3*b_1*b_1;
            for (int i = 0; i < cnt; i++)
            {
                //nT3Average_1 = c1_1 * xe6_1 + c2_1 * xe5_1 + c3_1 * xe4_1 + c4_1 * xe3_1
                _maShortTermXtrender[i] = c1_1 * xe6_1[i] + c2_1 * xe5_1[i] + c3_1 * xe4_1[i] + c4_1 * xe3_1[i];
            }
        }

        public float[] longterm(){
            return _longTermXtrender;
        }
        public float[] shortterm(){
            return _maShortTermXtrender;
        }

        public override void render(xGraphics g)
        {
            if (getShare(3) == null)
                return;
            Share share = getShare();

            float mX = 0;
            int mY = 0;


            if (detectShareCursorChanged())
            {
                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);

                calcBExtrender();
                detectBullBearDivergence();

                mHighest = 47;
                mLowest = -47;

                mVolumeBarW = (((float)getDrawingW() / mChartLineLength) * 2.0f / 3);
                if (mVolumeBarW < 1) mVolumeBarW = 1;
            }

            g.setColor(0xff00ff00);

            uint colorUp = themeDark()?(C.COLOR_TD_UP&0x90ffffff): C.COLOR_TL_UP;
            uint colorDown = themeDark()?(C.COLOR_TD_DOWN&0xa0ffffff): C.COLOR_TL_DOWN;

            uint preColor = colorUp;

            //  zero line
            float y0 = priceToY(0, mLowest, mHighest);
            g.setColor(colorPriceline());
            g.drawHorizontalLine(0, y0, getW()-20);

            //============long term histogram
            pricesToYs(_longTermXtrender, share.mBeginIdx, mChartLineXY, mChartLineLength, mLowest, mHighest);

            uint acColorUp1 = 0x8000ff00;
            uint acColorUp2 = 0x5000ff00;
            uint acColorDn1 = 0x80ff1212;
            uint acColorDn2 = 0x60ff1212;

            g.drawHistogram(mChartLineXY, 0, mChartLineLength, y0, 1.0f,
                    acColorUp1, acColorUp2,
                    acColorDn1, acColorDn2);

            //  long term line
            g.drawLinesUpDown(mChartLineXY, 0, mChartLineLength, _longTermXtrender, share.mBeginIdx,1.0f, acColorUp1, acColorDn1);
            //===================================

            uint cLime = Utils.ARGB(0xaf, 0, 230, 118);
            uint cLimeDark = Utils.ARGB(0xaf, 0x22, 0x8b, 0x22);
            uint cRed = Utils.ARGB(0xaf, 0xff, 0, 0);
            uint cRedDark = Utils.ARGB(0xaf, 0x8b, 0, 0);

            //  histogram: _shortTermXtrender
            pricesToYs(_shortTermXtrender, share.mBeginIdx, mChartLineXY, mChartLineLength, mLowest, mHighest);

            g.drawHistogram(mChartLineXY, 0, mChartLineLength, y0, mVolumeBarW,
                    cLime, cLimeDark, cRed, cRedDark);

            //  maShortTerm
            pricesToYs(_maShortTermXtrender, share.mBeginIdx, mChartLineXY, mChartLineLength, mLowest, mHighest);

            uint acColorUp = 0xff00ff00;
            uint acColorDn = 0xffff1212;

            g.setColor(0xff000000);
            g.drawLines(mChartLineXY, mChartLineLength, 3.5f);
            g.drawLinesUpDown(mChartLineXY, 0, mChartLineLength,
                    _maShortTermXtrender, share.mBeginIdx, 2.0f, acColorUp, acColorDn);

            //  bull/bear divergence
            renderBullBearLines(g, _osc, _bull, _bear, _line, getShare().mBeginIdx, getShare().mEndIdx);

            //  title
            if (mShouldDrawTitle) {
                g.setColor(C.COLOR_GRAY_LIGHT);
                g.drawString(mFont, "0", getW() - 2, mY + getH(), xGraphics.RIGHT | xGraphics.BOTTOM);
            }

            //  calc mMouseTitle
            mMouseTitle = null;

            renderCursor(g);
            renderDrawer(g);
        }

        public override xVector getTitles()
        {
            xVector v = new xVector(1);

            int sel = getShare().mSelectedCandle;
            if (sel >= 0 && sel < getShare().getCandleCnt()) {
                v.addElement(new stTitle(String.Format("XTrender: {0:F1}", _shortTermXtrender[sel]), colorTitle()));
            }
            else{
                v.addElement(new stTitle("XTrender", C.COLOR_GRAY));
            }

            return v;
        }


        public void detectBullBearDivergence(){
            if (_line == null){
                _osc = new float[Share.MAX_CANDLE_CHART_COUNT];
                _bull = new bool[Share.MAX_CANDLE_CHART_COUNT];
                _bear = new bool[Share.MAX_CANDLE_CHART_COUNT];
                _line = new int[Share.MAX_CANDLE_CHART_COUNT];
            }

            int cnt = getShare().getCandleCnt();

            Utils.arraycopy(_maShortTermXtrender, 0, _osc, 0, getShare().getCandleCnt());
            TA.detectConvergenceDivergence(getShare(), _osc, 0, cnt, 7, 3,
                    _bear, _bull, _line);
        }

        public bool hasBullDivergence(int lookback)
        {
            if (lookback <= 0){
                lookback = 10;
            }
            int cnt = getShare().getCandleCnt();
            if (lookback >= cnt){
                return false;
            }

            int begin = cnt - lookback;

            for (int i = cnt - 1; i >= begin; i--) {
                if (_bull[i]) {
                    return true;
                }
            }

            return false;
        }
    }
}
