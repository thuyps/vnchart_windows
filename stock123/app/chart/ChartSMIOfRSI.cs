using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;
using xlib.framework;

namespace stock123.app.chart
{
    public class ChartSMIOfRSI: ChartBase
    {
        bool[] _bear;
        bool[] _bull;
        float[] _osc;
        int[] _line;

        int _periodShortTerm = 14;
        int _periodLongTerm = 40;

        float[] _TSI;  //  histogram
        float[] _Signal;
        float[] _Histogram;

        public ChartSMIOfRSI(Font f)
            : base(f)
        {
            mChartType = CHART_SMI_RSI;
            //CHART_BORDER_SPACING_Y = CHART_BORDER_SPACING_Y_MASTER;

            initBuffers(10);

            _TSI = new float[Share.MAX_CANDLE_CHART_COUNT];
            _Signal = new float[Share.MAX_CANDLE_CHART_COUNT];
            _Histogram = new float[Share.MAX_CANDLE_CHART_COUNT];
        }

        public void calcSmi()
        {
            Share share = getShare();

            int cnt = share.getCandleCnt();
            if (cnt == 0)
            {
                return;
            }

            float[] hlc3 = mBuffers[0];
            float[] rsi = mBuffers[1];

            int rsiPeriod = _periodShortTerm;
            int stochLongLen = 13;
            int stochShortLen = 8;
            int stochSigLen = 8;

            _periodLongTerm = 40;
            int rsiPeriodHistogram = _periodLongTerm;
            int stochLongLenHistogram = 40;
            int stochShortLenHistogram = 8;
            int stochSigLenHistogram = 8;

            //  shortterm smi
            share.readCloses(hlc3);
            TA.calcRSI(hlc3, cnt, rsiPeriod, rsi);

            TA.SMI(rsi, cnt, stochShortLen, stochLongLen, stochSigLen, _TSI, _Signal, _Histogram);

            //  longterm histogram
            share.readHLC(hlc3);

            float[] tsi = mBuffers[2];
            float[] signal = mBuffers[3];

            TA.calcRSI(hlc3, cnt, rsiPeriodHistogram, rsi);

            TA.SMI(rsi, cnt,
                    stochShortLenHistogram,
                    stochLongLenHistogram,
                    stochSigLenHistogram,
                    tsi, signal, _Histogram);

            for (int i = 0; i < cnt; i++)
            {
                _Histogram[i] *= 4.0f;
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
                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);

                calcSmi();

                detectBullBearDivergence();

                mHighest = 50;
                mLowest = -50;

                mVolumeBarW = (((float)getDrawingW() / mChartLineLength) * 2.0f / 3);
                if (mVolumeBarW < 1) mVolumeBarW = 1;
            }

            if (mChartLineLength == 0)
                return;

            g.setColor(0xff00ff00);

            uint colorUp = themeDark() ? (C.COLOR_TD_UP & 0x90ffffff) : C.COLOR_TL_UP;
            uint colorDown = themeDark() ? (C.COLOR_TD_DOWN & 0xa0ffffff) : C.COLOR_TL_DOWN;

            uint preColor = colorUp;

            //  zero line
            float y0 = priceToY(0, mLowest, mHighest);
            g.setColor(colorPriceline());
            g.drawHorizontalLine(0, y0, getW() - 20);

            uint acColorUp1 = 0x8000ff00;
            uint acColorUp2 = 0x5000ff00;
            uint acColorDn1 = 0x80ff1212;
            uint acColorDn2 = 0x60ff1212;

            //==================
            //  histogram
            float baseY0 = priceToY(0, mLowest, mHighest);
            pricesToYs(_Histogram, share.mBeginIdx, mChartLineXY, mChartLineLength, mLowest, mHighest);
            g.drawHistogram(mChartLineXY, 0, mChartLineLength, baseY0, mVolumeBarW,
                    acColorUp1, acColorUp2,
                    acColorDn1, acColorDn2);


            //  signal
            pricesToYs(_Signal, share.mBeginIdx, mChartLineXY, mChartLineLength, mLowest, mHighest);
            g.setColor(C.COLOR_MAGENTA);
            g.drawLines(mChartLineXY, mChartLineLength, 1.0f);

            //  tsi
            pricesToYs(_TSI, share.mBeginIdx, mChartLineXY, mChartLineLength, mLowest, mHighest);
            g.setColor(themeDark() ? C.COLOR_WHITE : C.COLOR_GRAY_DARK);
            g.drawLines(mChartLineXY, mChartLineLength, 1.5f);

            //  bull/bear divergence
            renderBullBearLines(g, _osc, _bull, _bear, _line, getShare().mBeginIdx, getShare().mEndIdx);

            //  title
            if (mShouldDrawTitle)
            {
                g.setColor(C.COLOR_GRAY_LIGHT);
                g.drawString(mFont, "0", getW() - 2, 0 + getH(), xGraphics.RIGHT | xGraphics.BOTTOM);
            }

		    renderCursor(g);
        }

        override public xVector getTitles()
        {
            xVector v = new xVector(1);

            int sel = getShare().mSelectedCandle;
            if (sel >= 0 && sel < getShare().getCandleCnt()) {
                String s = String.Format("Smi RSI ({0}/{1})): %.1f",
                        _periodShortTerm,
                        _periodLongTerm,
                        _TSI[sel]);
                v.addElement(new stTitle(s, colorTitle()));
            }
            else{
                String s = String.Format("Smi RSI ({0}/{1}))",
                        _periodShortTerm,
                        _periodLongTerm);
                v.addElement(new stTitle(s, colorTitle()));
            }

            return v;
        }

        public void detectBullBearDivergence()
        {
            if (_line == null)
            {
                _osc = new float[Share.MAX_CANDLE_CHART_COUNT];
                _bull = new bool[Share.MAX_CANDLE_CHART_COUNT];
                _bear = new bool[Share.MAX_CANDLE_CHART_COUNT];
                _line = new int[Share.MAX_CANDLE_CHART_COUNT];
            }

            int cnt = getShare().getCandleCnt();

            Utils.arraycopy(_TSI, 0, _osc, 0, getShare().getCandleCnt());
            TA.detectConvergenceDivergence(getShare(), _osc, 0, cnt, 7, 3,
                    _bear, _bull, _line);
        }
    }

    


}
