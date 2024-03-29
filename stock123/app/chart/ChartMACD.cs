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
    public class ChartMACD: ChartBase
    {
        float[] mLineMACD;
        int mLineMACDSize;
        float[] mLineSignal9;
        int mLineSignal9Size;
        float[] mHistogramXY;
        int mHistogramXYSize;
        float[] mHistogramH;
        int mHistogramHSize;

        float mOY;
        float ry = 0;
        public ChartMACD(Font f)
            : base(f)
        {
            mChartType = CHART_MACD;
        }

        public override void render(xGraphics g)
        {
            if (isHiding())
                return;
            if (getShare(3) == null)
                return;
            if (detectShareCursorChanged())
            {
                recalcMACD();

                detectBullBearLines();
            }

            if (mChartLineLength == 0)
                return;

            if (mShouldDrawGrid)
                drawGrid(g);

            //mOY = getH() / 2;
            g.setColor(C.COLOR_FADE_YELLOW);
            g.drawLine(0, mOY, getW() - 34, mOY);
            g.setColor(C.COLOR_FADE_YELLOW0);
            g.drawString(mFont, "0", getW() - 8, mOY, xGraphics.VCENTER | xGraphics.RIGHT);

            int hisW = (int)(((float)getDrawingW() / mChartLineLength) * 2.0f / 3);
            
            for (int i = 0; i < mChartLineLength; i++)
            {
                if (mHistogramH[i] > 0)
                    g.setColor(0xffff0000);
                else
                    g.setColor(0xff00ff00);
                g.fillRect(mHistogramXY[2 * i], mHistogramXY[2 * i + 1], hisW, mHistogramH[i]);
            }

            //  macd
            g.setColor(C.COLOR_BLUE_LIGHT);
            g.drawLines(mLineMACD, mChartLineLength, 2.0f);

            //  signal
            g.setColor(0xffff0000);
            g.drawLines(mLineSignal9, mChartLineLength, 1.0f);

            //  bull/bear
            renderBullBearLines(g, _osc, _bull, _bear, _line, getShare().mBeginIdx, getShare().mEndIdx);
            
            StringBuilder sb = Utils.sb;
            //=========================
            sb.Length = 0;
            float v = (mOY - mLastY) / ry;
            sb.AppendFormat("{0:F2}", v);

            mMouseTitle = sb.ToString();
            renderCursor(g);
            //	bottom border
            g.setColor(0xffa0a0a0);
            g.drawHorizontalLine(0, 0, getW());

            renderDrawer(g);
        }

        void recalcMACD()
        {
            float[] macd;
            float[] signal;
            float[] his;

            int i;
            int cnt;

            Share s = getShare();
            s.calcMACD();

            Share share = s;

            //======================
            his = share.pMACDHistogram;
            for (i = 0; i < s.getCandleCount(); i++)
            {
                his[i] = share.pMACD[i] - share.pMACDSignal9[i];
            }
            //===========================

            macd = share.pMACD;
            signal = share.pMACDSignal9;
            his = share.pMACDHistogram;

            cnt = mChartLineLength;

            mLineMACD = allocMem(mLineMACD, cnt * 2);
            mLineSignal9 = allocMem(mLineSignal9, cnt * 2);
            mHistogramXY = allocMem(mHistogramXY, cnt * 2);
            mHistogramH = allocMem(mHistogramH, cnt * 1);

            //	get the highest
            float rx = (float)getDrawingW() / cnt;

            mHighest = -99999;
            mLowest = 99999;
            //	lo/hi
            for (i = 0; i < cnt; i++)
            {
                if (macd[i+s.mBeginIdx] > mHighest)
                    mHighest = macd[i + s.mBeginIdx];
                if (macd[i + s.mBeginIdx] < mLowest)
                    mLowest = macd[i + s.mBeginIdx];
            }
            for (i = 0; i < cnt; i++)
            {
                if (signal[i + s.mBeginIdx] > mHighest)
                    mHighest = signal[i + s.mBeginIdx];
                if (signal[i + s.mBeginIdx] < mLowest)
                    mLowest = signal[i + s.mBeginIdx];
            }

            if (mHighest < 0) mHighest = 0;
            if (mLowest < 0) mLowest = -mLowest;

            float double_hi = Math.Abs(mHighest) > Math.Abs(mLowest) ? mHighest : mLowest;// hi + lo;
            //double_hi = 2 * xUtils.ABS_FLOAT(double_hi);
            mHighest = double_hi;
            mLowest = -double_hi;

            float minHistoH = 9;
            float signalDrawH = getDrawingH() - 2 * minHistoH;
            float OY = getH() / 2;// 0 + CHART_BORDER_SPACING_Y + minHistoH;
            //float ry = 0;
            if (double_hi != 0)
            {
                OY = getH() / 2;// (int)((float)hi * signalDrawH / double_hi);
                ry = (float)signalDrawH / double_hi;
            }
            mOY = OY;

            //	double_hi	==	100 pixels
            //	make sure all macd is now > 0 && < 100
            float tmp_macd;
            float tmp_signal;
            for (i = 0; i < cnt; i++)
            {
                tmp_macd = macd[i+s.mBeginIdx];// + hi;
                tmp_signal = signal[i+s.mBeginIdx];// + hi;
                //if (hi != 0)
                {
                    mLineMACD[2 * i + 1] = priceToY(tmp_macd);// (float)(OY - tmp_macd * ry);
                    mLineSignal9[2 * i + 1] = priceToY(tmp_signal);// (float)(OY - tmp_signal * ry);
                }
            }

            //	cnt == CHART_W
            for (i = 0; i < cnt; i++)
            {
                mLineMACD[2 * i] = (float)(0 + CHART_BORDER_SPACING_X + i * rx + getStartX());
                mLineSignal9[2 * i] = mLineMACD[2 * i];
            }

            //	histogram
            /*
            mHighest = 0;
            for (i = 0; i < cnt; i++)
            {
                if (Utils.ABS_FLOAT(his[i + s.mBeginIdx]) > mHighest)
                    mHighest = Utils.ABS_FLOAT(his[i + s.mBeginIdx]);
            }
            //	int deltaY = 0;//deltaLoHi*mDrawingH/double_hi;
            double_hi = 2 * mHighest;
            */
            //	double_hi	==	100 pixels
            float halfH = getDrawingH() / 2;
            float hry = 0;
            if (mHighest != 0)
            {
                hry = (float)halfH / mHighest;//double_hi;
            }

            for (i = 0; i < cnt; i++)
            {
                if (double_hi != 0)
                    mHistogramH[i] = (float)(-his[i + s.mBeginIdx] * ry);
                else
                    mHistogramH[i] = 0;

                if (mHistogramH[i] == 0)
                {
                    if (his[i + s.mBeginIdx] > 0) mHistogramH[i] = -1;
                    else mHistogramH[i] = 1;
                }
                mHistogramXY[2 * i] = (float)(0 + CHART_BORDER_SPACING_X + i * rx + getStartX());
                mHistogramXY[2 * i + 1] = (float)OY;//mY + CHART_BORDER_SPACING_Y + halfH - deltaY;
            }
        }

        override public xVector getTitles()
        {
            xVector v = new xVector(5);
            StringBuilder sb = Utils.sb;

            Share share = getShare(3);

            int idx = share.getCursor();
            float v1 = share.pMACDHistogram[idx];
            float v2 = share.pMACD[idx];
            float v3 = share.pMACDSignal9[idx];

            sb.Length = 0;
            //  macd
            sb.AppendFormat("MACD: ({0}, {1}, {2}) HIS={3:F2}", (int)mContext.mOptMACDFast, (int)mContext.mOptMACDSlow, (int)mContext.mOptMACDSignal, v1);
            v.addElement(new stTitle(sb.ToString(), 0xfff0f0f0));

            //  macd
            sb.Length = 0;
            sb.AppendFormat("MACD: {0:F2}", v2);
            v.addElement(new stTitle(sb.ToString(), C.COLOR_BLUE_LIGHT));

            sb.Length = 0;
            //  signal 9
            sb.AppendFormat("Sig: {0:F2}", v3);
            v.addElement(new stTitle(sb.ToString(), 0xffff0000));

            return v;
        }

        bool[] _bear;
        bool[] _bull;
        float[] _osc;
        int[] _line;
        public void detectBullBearLines()
        {
            if (_line == null)
            {
                _osc = new float[Share.MAX_CANDLE_CHART_COUNT];
                _bull = new bool[Share.MAX_CANDLE_CHART_COUNT];
                _bear = new bool[Share.MAX_CANDLE_CHART_COUNT];
                _line = new int[Share.MAX_CANDLE_CHART_COUNT];
            }
            //float[] price = mBuffers[4];
            float[] pMACD = getShare().pMACD;
            Utils.arraycopy(pMACD, 0, _osc, 0, getShare().getCandleCnt());
            ///getShare().readCloses(price);
            TA.detectConvergenceDivergence(getShare(),
                    _osc, 0,
                    getShare().getCandleCnt(),
                    7,
                    2,
                    _bear,
                    _bull,
                    _line
            );
        }
    }
}
