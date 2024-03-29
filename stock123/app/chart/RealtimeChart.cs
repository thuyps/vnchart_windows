using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.framework;
using xlib.utils;
using xlib.ui;
using stock123.app.data;

namespace stock123.app.chart
{
    public class RealtimeChart : xBaseControl
    {
        const int MAX_CANDLE = 2000;
        TradeHistory mTrade;
        TradeHistory mOldTrade;
        float[] mChartXYs;

        int mChartXYLength;
        float mPriceBase;

        bool mInitChartXY;

        Context mContext;

        String mTimeStart;
        String mTimeEnd;
        String mVolumeLabel;

        float price_distance;

        String mCurrentTrade;
        String mTotalVolume;
        int mCurrentTradeSel;
        int mLastX;
        bool mMouseDown = false;
        //  price
        int mCandleCnt = 0;
        float[] mPrices;
        int[] mVolumes;
        int[] mTimes;
        //  bollinger
        float[] pBBLowers;
        float[] pBBUppers;
        float[] pTmp;
        int[] pTmpInt;
        float[] mBBUpperXY;	//	bollinger band
        float[] mBBLowerXY;
        float[] mBBLine;

        //  MACD
        float[] pMACD = new float[MAX_CANDLE];
        float[] pMACDSignal9 = new float[MAX_CANDLE];
        float[] pMACDHistogram = new float[MAX_CANDLE];

        float[] mLineMACD = new float[2 * MAX_CANDLE];
        float[] mLineSignal9 = new float[2 * MAX_CANDLE];
        float[] mHistogramXY = new float[2 * MAX_CANDLE];
        float[] mHistogramH = new float[2 * MAX_CANDLE];

        public bool mShouldDrawMACD = true;
        public bool mShouldDrawADX = false;

        xButton mButtonMACD;

        string mChanged = "";
        float mChangedValue = 0;
        //==============================================
        public RealtimeChart(TradeHistory trade, xIEventListener listener)
            : base(listener)
        {
            makeCustomRender(true);

            mContext = Context.getInstance();
            mTrade = trade;

            mChartXYLength = 4000;
            mChartXYs = new float[2 * mChartXYLength];

            mCurrentTradeSel = 0;

            setBackgroundColor(C.COLOR_BLACK);

            pBBUppers = new float[10000];
            pBBLowers = new float[10000];
            pTmp = new float[10000];
            pTmpInt = new int[5000];

            mPrices = new float[10000];
            mVolumes = new int[10000];
            mTimes = new int[10000];

            mButtonMACD = xButton.createStandardButton(0, null, "MACD", 60);
        }

        public override void setSize(int w, int h)
        {
            base.setSize(w, h);

            mButtonMACD.setPosition(getW() - mButtonMACD.getW(), 0);

            mOldTrade = null;   //  force recalc everything
        }

        override public void render(xGraphics g)
        {
            g.setColor(C.GREY_LINE_COLOR);
            g.getGraphics().Clear(Color.Black);

            if (mTrade == null)
                return;

            Font f = mContext.getFontSmall();

            if (mTrade.mHasNewData || mTrade != mOldTrade)
            {
                calcChartXY();
                mTrade.mHasNewData = false;
            }

            mOldTrade = mTrade;

            if (mTrade == null || mTrade.mShare == null)
                return;

            //=====================draw chart==============
            int[] h = { getH() * 3 / 4, getH() / 3, getH() / 3 };   //  chart, MACD, vol
            int[] y = { 0, getH()/2, getH() - h[2] };
            int w = getW() - 2;
            int x = 0;
            //===================================================================
            if (mShouldDrawMACD)
            {
                h[0] = getH() * 3 / 4 - 12;
                y[0] = 12;

                h[1] = getH() / 2;
                y[1] = getH() / 2;

                h[2] = getH() / 5;
                y[2] = getH() - h[2] - 13;
            }
            else
            {
                h[0] = getH() * 3 / 4 - 12;
                y[0] = 12;

                h[1] = getH() / 3;
                y[1] = getH() / 2;

                h[2] = getH() / 3;
                y[2] = getH() - h[2] - 13;
            }
            //============================================
            drawPriceLines(g, y[0], h[0]);
            //===============chart line=====================
            //  bollinger
            drawBollingers(g, y[0], h[0]);

            drawChart(g, y[0], h[0]);

            //===================================================================
            drawVolume(g, y[2], h[2]);

            if (mShouldDrawMACD)
            {
                drawMACD(g, y[1], h[1]);
            }

            drawText(g, 65);

            //===========================
            drawMACDButton(g);
            
            //  xem chi tiet button
            Font font = mContext.getFontText();
            g.setColor(C.COLOR_ORANGE);
            g.drawString(font, "Chi tiết >>", getW() - 82, 2, xGraphics.LEFT);
        }

        public void setTrade(TradeHistory trade)
        {
            mTrade = trade;

            if (trade == null)
                return;
            mVolumeLabel = "";
            mTimeStart = "";
            mTimeEnd = "";

            mPriceBase = trade.mShare.getRefFromPriceboard();
            trade.mHasNewData = true;

            updateTradeInfo();
        }
        public void calcChartXY()
        {
            int mChartW = getW() - 2;
            if (mChartXYLength < mTrade.getTransactionCount())
            {
                mChartXYLength = mTrade.getTransactionCount() + 256;

                mChartXYs = new float[2 * mChartXYLength];
            }

            mInitChartXY = true;

            mCandleCnt = 0;
            if (mTrade == null || mTrade.mShare == null || mTrade.getTransactionCount() == 0)
                return;

            int btime = mTrade.getTime(0);
            int etime = mTrade.getTime(mTrade.getTransactionCount() - 1);

            //	time: 0x00HHMMSS
            int timeb = Utils.EXTRACT_HOUR(btime) * 3600 + Utils.EXTRACT_MINUTE(btime) * 60 + Utils.EXTRACT_SECOND(btime);
            int timee = Utils.EXTRACT_HOUR(etime) * 3600 + Utils.EXTRACT_MINUTE(etime) * 60 + Utils.EXTRACT_SECOND(etime);

            int time_distance = timee - timeb;
            if (time_distance < 0)
                return;

            //	re-update REF
            float reference = mTrade.mShare.getRefFromPriceboard();

            calcLabels();

            int max_time = time_distance + 30 * 60;	//	+ future 30minutes

            price_distance = 0;
            float price_max = -10000000;
            float price_min = 10000000;

            int cnt = mTrade.getTransactionCount();
            float price;
            for (int i = 0; i < cnt; i++)
            {
                price = mTrade.getPrice(i);
                if (price > price_max)
                    price_max = price;
                if (price < price_min)
                    price_min = price;
            }

            /*
            int tmp1 = price_max - mPriceBase;
            int tmp2 = price_min - mPriceBase;

            if (Utils.ABS_INT(tmp1) > Utils.ABS_INT(tmp2))
                price_distance = Utils.ABS_INT(2 * tmp1);
            else
                price_distance = Utils.ABS_INT(2 * tmp2);
            */

            if (price_max == price_min)
            {
                if (price_max > reference)  //  CE case
                {
                    price_min = reference;
                }
                else if (price_max < reference) //  FLOOR
                {
                    price_max = reference;
                }
                else
                {
                    //  REF
                    price_max = reference + ((float)reference * 2 / 100);
                    price_max = reference - ((float)reference * 2 / 100);
                }
            }

            float delta = (float)(price_max - price_min)/5;
            
            price_min -= delta;
            price_max += delta;

            price_min = ((int)(price_min * 100)) / 100.0f;
            price_max = ((int)(price_max * 100)) / 100.0f;

            if (price_min > reference - delta)
                price_min = reference - delta;
            if (price_max < reference + delta)
                price_max = (reference + delta);

            mPriceBase = price_min;
            price_distance = price_max - price_min;// 2 * price_distance;

            //  calc main-line
            int timeFrame = getTimeFrame();
            int lastTime = 0;
            mCandleCnt = 0;
            int vol = 0;
            int t = 0;
            for (int i = 0; i < cnt; i++)
            {
                t = mTrade.getTime(i);

                t = timeToSeconds(t);
                vol += mTrade.getTradeVolume(i);
                if (t - lastTime >= timeFrame)
                {
                    mPrices[mCandleCnt] = mTrade.getPrice(i);
                    mVolumes[mCandleCnt] = vol;
                    mTimes[mCandleCnt] = mTrade.getTime(i);

                    mCandleCnt++;
                    vol = 0;
                    lastTime = t;
                }
            }
            if (lastTime != t)
            {
                mPrices[mCandleCnt] = mTrade.getPrice(mTrade.getTransactionCount()-1);
                mVolumes[mCandleCnt] = vol;
                mTimes[mCandleCnt] = mTrade.getLastTime();

                mCandleCnt++;
            }

            //if (mCandleCnt > 10)      //  for debug
                //mCandleCnt = 5;
            mCurrentTradeSel = mCandleCnt - 1;
        }

        int timeToSeconds(int time)
        {
            int t = Utils.EXTRACT_HOUR(time) * 3600 + Utils.EXTRACT_MINUTE(time) * 60 + Utils.EXTRACT_SECOND(time);

            return t;
        }

        int getTimeFrame()
        {
            if (mTrade.getTransactionCount() > 100)
                return 60;

            return 1;
        }

        int getMaxTimeOfChart()
        {
            int btime = mTimes[0];
            int etime = mTimes[mCandleCnt - 1];

            //	time: 0x00HHMMSS
            int timeb = Utils.EXTRACT_HOUR(btime) * 3600 + Utils.EXTRACT_MINUTE(btime) * 60 + Utils.EXTRACT_SECOND(btime);
            int timee = Utils.EXTRACT_HOUR(etime) * 3600 + Utils.EXTRACT_MINUTE(etime) * 60 + Utils.EXTRACT_SECOND(etime);

            int time_distance = timee - timeb;
            if (time_distance < 0)
                return 1;

            int max_time = time_distance + 30 * 60;	//	+ future 30minutes

            return max_time;
        }

        void pricesToXYs(float[] prices, int cnt, float[] XYs, int[] times, float y, float h)
        {
            int time;
            if (cnt == 0 || mCandleCnt == 0)
                return;

            int timeb = Utils.EXTRACT_HOUR(mTimes[0]) * 3600
                            + Utils.EXTRACT_MINUTE(mTimes[0]) * 60
                            + Utils.EXTRACT_SECOND(mTimes[0]);
            int max_time = getMaxTimeOfChart();
            //	line chart
            //int h = getH() - 10;
            int w = getW();

            float ratioX = (float)w / max_time;
            float ratioY = (float)h / price_distance;

            for (int i = 0; i < cnt; i++)
            {
                time = timeToSeconds(times[i]);
                time -= timeb;

                XYs[2 * i] = (float)(0 + time * ratioX);
                XYs[2 * i + 1] = (float)(y + h - (prices[i] - mPriceBase) * ratioY);
            }
        }

        float priceToY(float price, float y, float h)
        {
            float ratioY = (float)h / price_distance;
            return (float)(y + h - (price - mPriceBase) * ratioY);
        }

        override public void onMouseDown(int x, int y)
        {
            mMouseDown = true;
            mLastX = x;

            mCurrentTradeSel = xToPriceIDX(x);
            updateTradeInfo();
        }
        public override void onMouseUp(int x, int y)
        {
            mMouseDown = false;

            if (x >= 2 && x <= 60 && y >= 2 && y <= 14)
            {
                mShouldDrawMACD = !mShouldDrawMACD;

                invalidate();
            }

            //  view in detail
            if (x >= getW() - 82 && y < 20)
            {
                viewRealtimeInDetail();
            }

            
        }

        public override void onMouseDoubleClick()
        {
            viewRealtimeInDetail();
        }

        override public void onMouseMove(int x, int y)
        {
            if (!mMouseDown)
                return;
            if (mTrade == null || mTrade.mShare == null)
                return;
            {
                mCurrentTradeSel = xToPriceIDX(x);
                updateTradeInfo();
                /*
                int newSel = mCurrentTradeSel;
                if (x > mLastX + 2)
                    newSel++;
                else if (x < mLastX - 2)
                    newSel--;

                if (newSel != mCurrentTradeSel)
                {
                    mLastX = x;
                    if (newSel >= 0 && newSel < mTrade.getTransactionCount())
                    {
                        mCurrentTradeSel = newSel;
                        updateTradeInfo();
                    }
                }
                 */
            }
        }

        void calcLabels()
        {
            if (mTrade.getTransactionCount() == 0)
                return;

            int btime = mTrade.getTime(0);
            int etime = mTrade.getTime(mTrade.getTransactionCount() - 1);
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            //	time labels
            sb.AppendFormat("{0}:{1:D2}", Utils.EXTRACT_HOUR(btime), Utils.EXTRACT_MINUTE(btime));
            mTimeStart = sb.ToString();
            sb.Length = 0;
            sb.AppendFormat("{0}:{1:D2}", Utils.EXTRACT_HOUR(etime), Utils.EXTRACT_MINUTE(etime));
            mTimeEnd = sb.ToString();

            int maxVol = 0;
            int transCnt = mTrade.getTransactionCount();
            int vol;
            for (int i = 0; i < transCnt; i++)
            {
                vol = mTrade.getTradeVolume(i);
                if (vol > maxVol) maxVol = vol;
            }
            /*
            String sz;
            if (maxVol > 1000000)
            {
                sz = Utils.formatDecimalNumber(maxVol, 1000000, 1);
                mVolumeLabel = sz + "M";
            }
            else if (maxVol > 1000)
            {
                sz = Utils.formatDecimalNumber(maxVol, 1000, 1);
                mVolumeLabel = sz + "K";
            }
            else
            {
                mVolumeLabel = "" + maxVol;
            }
             */
        }
        void updateTradeInfo()
        {
            if (mTrade == null)
                return;
            int transCnt = mTrade.getTransactionCount();
            if (mCurrentTradeSel >= transCnt)
                return;

            String sz;
            if (mTrade.mShare.isIndex())
            {
                sz = Utils.formatDecimalNumber((int)mPrices[mCurrentTradeSel], 100, 1);
            }
            else
            {
                sz = Utils.formatDecimalNumber((int)mPrices[mCurrentTradeSel], 1000, 2);
            }
            String sz2;
            sz2 = Utils.formatDecimalNumber(mVolumes[mCurrentTradeSel], 1000, 2);
            int time = mTimes[mCurrentTradeSel];
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            sb.AppendFormat("{0:D2}:{1:D2} - {2}:{3}  KL:{4}K", ((time >> 16) & 0xff), ((time >> 8) & 0xff), "Giá", sz, sz2);
            mCurrentTrade = sb.ToString();
            //  change +-
            float priceRef = mTrade.mShare.getRefFromPriceboard();
            float changed = mPrices[mCurrentTradeSel] - priceRef;
            sb.Length = 0;
            float tmp = 1000.0f;
            if (mTrade.mShare.isIndex())
                tmp = 100;
            if (changed > 0)
                sb.AppendFormat("+{0:F2}", (changed/tmp));
            else 
                sb.AppendFormat("{0:F2}", (changed/tmp));
            mChanged = sb.ToString();
            mChangedValue = changed;
            /*
            //  total
            sz = Utils.formatNumber(mTrade.getVolume(mCurrentTradeSel));
            sb.Length = 0;
            sb.AppendFormat("( {0} )", sz);
            mTotalVolume = sb.ToString();
             */

            invalidate();
        }

        public TradeHistory getTradeHistory()
        {
            return mTrade;
        }


        void drawBollingers(xGraphics g, int y, int h)
        {
            int[] times = mTimes;
            float[] prices = mPrices;
            int pricesCnt = mCandleCnt;

            if (pricesCnt <= 0)
                return;

            int pointCnt = 0;

            int i = 0;

            float lo = mPriceBase - price_distance / 2;
            float hi = mPriceBase + price_distance / 2;
            int period = 14;
            if (pricesCnt < 30)
                period = pricesCnt / 3;
            if (period < 2) period = 2;
            Share.calcBollinger(prices, pricesCnt, period, 2.0f, pBBLowers, pBBUppers);

            if (mBBLowerXY == null)
            {
                mBBUpperXY = new float[20000];
                mBBLowerXY = new float[20000];
                mBBLine = new float[40000];
            }

            pricesToXYs(pBBLowers, pricesCnt, mBBUpperXY, times, y, h);
            pricesToXYs(pBBUppers, pricesCnt, mBBLowerXY, times, y, h);

            int j = 0;

            j = 0;

            for (i = 0; i < pricesCnt; i++)
            {
                mBBLine[j++] = mBBUpperXY[2 * i];
                mBBLine[j++] = mBBUpperXY[2 * i + 1];
            }
            mBBLine[j++] = mBBUpperXY[2 * (pricesCnt - 1)];      //  x
            mBBLine[j++] = mBBLowerXY[2 * (pricesCnt - 1) + 1];    //  y
            for (i = pricesCnt - 1; i >= 0; i--)
            {
                mBBLine[j++] = mBBLowerXY[2 * i];
                mBBLine[j++] = mBBLowerXY[2 * i + 1];
            }

            pointCnt = j / 2;
            //=======river
            g.setColor(0x40F8D4CF);
            g.fillShapes(mBBLine, pointCnt);

            //===========border
            pointCnt = mTrade.getTransactionCount();
            g.setColor(0xff752922);
            g.drawLines(mBBUpperXY, pricesCnt);
            g.drawLines(mBBLowerXY, pricesCnt);
        }

        void drawPriceLines(xGraphics g, int y, int h)
        {
            int x = 0;
            int w = getW();

            g.drawHorizontalLine(x, y, w);

            //====================price lines=================================
            //  pricelines
            if (price_distance > 0 && mTrade.mShare != null)
            {
                x = 0 + 1;

                float priceRef = mTrade.mShare.getRefFromPriceboard();
                float step = (price_distance / 5);

                g.setColor(C.GREY_LINE_COLOR);

                String sz;

                Font fNum = mContext.getFontSmall();

                for (int i = -5; i < 5; i++)
                {
                    float price = priceRef + i*step;
                    float yy = priceToY(price, y, h);

                    if (yy < 15 || yy > h - 15)
                        continue;

                    if (i == 0)
                        g.setColor(C.YELLOW_LINE_COLOR);
                    else
                        g.setColor(C.GREY_LINE_COLOR);
                    g.drawHorizontalLine(x, yy, getW() - 4);
                    g.setColor(C.COLOR_GRAY_LIGHT);
                    g.drawString(fNum, String.Format("{0:F2}", price), x, yy, xGraphics.VCENTER | xGraphics.LEFT);
                }
            }
        }

        void drawVolume(xGraphics g, int y, int h)
        {
            y += h;
            //================================================
            int max_volume = 1;
            int vol;
            for (int i = 0; i < mCandleCnt; i++)
            {
                vol = mVolumes[i];
                if (vol > max_volume)
                    max_volume = vol;
            }

            float ratioY = (float)(h) / max_volume;
            int w = 1;
            g.setColor(0xff00ff00);
            int x = 0;

            //  volume
            for (int i = 0; i < mCandleCnt; i++)
            {
                vol = mVolumes[i];
                h = (int)(vol * ratioY);
                g.fillRect(mChartXYs[2 * i], y - h, w, h);
            }
            //Font fLabel = mContext.getFontText();
            //g.setColor(C.COLOR_GRAY_LIGHT);
            //g.drawString(fLabel, mVolumeLabel, x + getW() - 3, 0, xGraphics.RIGHT | xGraphics.TOP);
        }

        void drawChart(xGraphics g, float y, float h)
        {
            pricesToXYs(mPrices, mCandleCnt, mChartXYs, mTimes, y, h);

            g.setColor(0xffffffff);
            g.drawLines(mChartXYs, mCandleCnt);
        }

        void drawText(xGraphics g, int x)
        {
            Font f = mContext.getFontSmall();
            Font fLabel = mContext.getFontText();

            //  ma co phieu
            g.setColor(C.COLOR_WHITE);
            string s = "#" + mTrade.getCode();
            g.drawString(mContext.getFontTextB(), s, x, 1);
            x += g.getStringWidth(mContext.getFontTextB(), s) + 5;
            //	time
            if (mCandleCnt > 0)
            {
                g.setColor(C.COLOR_GRAY_DARK);
                g.fillRect(0, getH() - 12, getW(), 12);
                g.setColor(C.COLOR_WHITE);
                g.drawString(f, mTimeStart, 0 + 1, 0 + getH(), xGraphics.BOTTOM | xGraphics.LEFT);
                g.drawString(f, mTimeEnd, mChartXYs[2 * (mCandleCnt - 1)], 0 + getH() - 12, xGraphics.BOTTOM | xGraphics.LEFT);

                g.setColor(C.COLOR_WHITE);
                s = "KL: " + Utils.formatNumber(mTrade.getVolume(-1));
                g.drawString(f, s, (getW() - Utils.getStringW(s, f)) / 2, getH(), xGraphics.BOTTOM);
            }

            g.setColor(C.GREY_LINE_COLOR);
            g.drawHorizontalLine(x, 0 + getH(), getW());
            //=============cursor & some info========================
            if (mCandleCnt > 0)
            {
                //x = getW() / 2;
                g.setColor(C.COLOR_GRAY_LIGHT);
                g.drawString(fLabel, mCurrentTrade, x, 0 + 1, xGraphics.LEFT);// xGraphics.HCENTER | xGraphics.TOP);
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawVerticalLine(mChartXYs[2 * mCurrentTradeSel], 12, getH() - 26);
                //g.drawPoint(mChartXYs[2 * mCurrentTradeSel] - 2, mChartXYs[2 * mCurrentTradeSel + 1] - 4, 4);

                if (mChangedValue > 0)
                    g.setColor(C.COLOR_GREEN);
                else if (mChangedValue == 0)
                    g.setColor(C.COLOR_YELLOW);
                else
                    g.setColor(C.COLOR_RED);
                g.drawString(fLabel, mChanged, mChartXYs[2 * mCurrentTradeSel], 26);
            }
            //=====================================================
        }

        public void calcMACD()
        {
            int cnt = this.mCandleCnt;
            if (cnt < 5)
            {
                return;
            }

            float[] val = mPrices;

            float[] ema12 = Share.EMA(val, 0, cnt, 12, null);
            float[] ema26 = Share.EMA(val, 0, cnt, 26, null);

            float[] macd = pMACD;
            for (int i = 0; i < cnt; i++)
            {
                macd[i] = ema12[i] - ema26[i];
            }

            //======signal======EMA9 of MACD
            float[] sig = pMACDSignal9;
            sig[0] = macd[0];
            float exp = (float)2 / (Context.getInstance().mOptMACDSignal + 1);
            for (int i = 1; i < cnt; i++)
            {
                sig[i] = (float)macd[i] * exp + (float)sig[i - 1] * (1 - exp);
            }

            //=======histogram===============
            float[] his = pMACDHistogram;
            for (int i = 0; i < cnt; i++)
            {
                his[i] = macd[i] - sig[i];
            }
            //====================================
            ema12 = null;
            ema26 = null;
        }

        void drawMACD(xGraphics g, int y, int h)
        {
            calcMACD();

            if (mCandleCnt < 5)
                return;

            //  translate to XYs
            //======================
            float lo = 1000;
            float hi = -1000;

            int i;
            float[] his = pMACDHistogram;
            for (i = 0; i < mCandleCnt; i++)
            {
                his[i] = pMACD[i] - pMACDSignal9[i];
            }
            //===========================

            float[] macd = pMACD;
            float[] signal = pMACDSignal9;
            his = pMACDHistogram;

            int cnt = mCandleCnt;

            //	get the highest
            int w = getW();
            float rx = (float)w/ cnt;

            //	lo/hi
            for (i = 0; i < cnt; i++)
            {
                if (macd[i] > hi)
                    hi = macd[i];
                if (macd[i] < lo)
                    lo = macd[i];
            }
            for (i = 0; i < cnt; i++)
            {
                if (signal[i] > hi)
                    hi = signal[i];
                if (signal[i] < lo)
                    lo = signal[i];
            }

            if (hi < 0) hi = 0;
            if (lo < 0) lo = -lo;

            float double_hi = Utils.ABS_FLOAT(hi) > Utils.ABS_FLOAT(lo) ? hi : lo;// hi + lo;
            double_hi = 2 * Utils.ABS_FLOAT(double_hi);

            float minHistoH = 9;
            float signalDrawH = h - 2 * minHistoH;
            float OY = y + h / 2;
            float ry = 0;
            if (double_hi != 0)
            {
                OY = y + h / 2;
                ry = (float)signalDrawH / double_hi;
            }

            //	make sure all macd is now > 0 && < 100
            for (i = 0; i < cnt; i++)
            {
                mLineMACD[2 * i + 1] = (float)(OY - macd[i] * ry);
                mLineSignal9[2 * i + 1] = (float)(OY - signal[i] * ry);
            }

            //	cnt == CHART_W
            int max_time = getMaxTimeOfChart();
            int timeb = Utils.EXTRACT_HOUR(mTimes[0]) * 3600
                            + Utils.EXTRACT_MINUTE(mTimes[0]) * 60
                            + Utils.EXTRACT_SECOND(mTimes[0]);

            rx = (float)getW() / max_time;
            for (i = 0; i < cnt; i++)
            {
                int time = timeToSeconds(mTimes[i]);
                time -= timeb;

                mLineMACD[2 * i] = (float)(time * rx);
                mLineSignal9[2 * i] = mLineMACD[2 * i];
            }

            //	histogram
            hi = 0;
            for (i = 0; i < cnt; i++)
            {
                if (Utils.ABS_FLOAT(his[i]) > hi)
                    hi = Utils.ABS_FLOAT(his[i]);
            }
            //	int deltaY = 0;//deltaLoHi*mDrawingH/double_hi;
            double_hi = 2 * hi;
            //	double_hi	==	100 pixels
            float halfH = h / 2;
            float hry = 0;
            if (hi != 0)
                hry = (float)halfH / hi;//double_hi;
            i = 0;
            float drawW = 0;
            for (i = 0; i < cnt; i++)
            {
                if (double_hi != 0)
                    mHistogramH[i] = (float)(-his[i] * ry);
                else
                    mHistogramH[i] = 0;

                if (mHistogramH[i] == 0)
                {
                    if (his[i] > 0) mHistogramH[i] = -1;
                    else mHistogramH[i] = 1;
                }
                int time = timeToSeconds(mTimes[i]);
                time -= timeb;

                mHistogramXY[2 * i] = (float)(time * rx);
                drawW = mHistogramXY[2 * i];
                mHistogramXY[2 * i + 1] = (float)OY;
            }

            // ===========================================================
            //  ===== now draw =====
            // ===========================================================
            g.setColor(C.COLOR_FADE_YELLOW);
            g.drawLine(0, OY, getW() - 20, OY);
            g.setColor(C.COLOR_FADE_YELLOW0);
            g.drawString(mContext.getFontSmall(), "0", getW() - 20, OY, xGraphics.VCENTER);

            int hisW = (int)(((float)drawW/ mCandleCnt) * 2.0f / 3);
            int hisW2 = hisW / 2;
            if (hisW <= 0) hisW = 1;
            //g.setColor(0x6000ff00);
            for (i = 0; i < mCandleCnt; i++)
            {
                //g.fillRect(mHistogramXY[2 * i], mHistogramXY[2 * i + 1], hisW, mHistogramH[i]);
                if (mHistogramH[i] < 0)
                    g.setColor(0xff70ee00);
                else
                    g.setColor(0xffc03000);

                //g.drawRect(mHistogramXY[2 * i] - hisW2, mHistogramXY[2 * i + 1], hisW, mHistogramH[i]);
                g.drawRect(mHistogramXY[2 * i], mHistogramXY[2 * i + 1], 1, mHistogramH[i]);
                //g.drawVerticalLine(mHistogramXY[2 * i], mHistogramXY[2 * i + 1] + hisW / 2, mHistogramH[i]);
            }

            g.setColor(C.COLOR_BLUE_LIGHT);
            g.drawLines(mLineMACD, mCandleCnt, 2.0f);

            g.setColor(0xffff0000);
            g.drawLines(mLineSignal9, mCandleCnt, 1.0f);
        }

        void drawMACDButton(xGraphics g)
        {
            Font f = mContext.getFontSmall();
            int w = 60;
            int h = f.Height;
            int x = 2;
            int y = 2;

            if (mShouldDrawMACD)
            {
                g.setColor(0xc00080ff);
            }
            else
                g.setColor(0xc0ff8000);
            g.fillRect(x, y, w, h);

            g.setColor(C.COLOR_GRAY);
            g.drawRect(x, y, w, h);

            x += w / 2;// (w - g.getStringWidth(f, "MACD")) / 2;
            y += h / 2;

            g.setColor(C.COLOR_WHITE);
            g.drawString(f, "MACD", x, y, xGraphics.HCENTER | xGraphics.VCENTER);
        }
        //----------------------

        int xToPriceIDX(int x)
        {
            if (mCandleCnt < 2)
                return 0;
            int timeb = timeToSeconds(mTimes[0]);
            int timee = timeToSeconds(mTimes[mCandleCnt-1]);

            int max_time = getMaxTimeOfChart();
            float ratioX = (float)getW() / max_time;

            int time = (int)(x / ratioX);   //  timeAtX - in seconds

            time = timeb + time;
            //  now convert to realtime
            time = ((time / 3600) << 16) | (((time % 3600) / 60) << 8) | (time % 60);

            int IDX = mCandleCnt - 1;
            for (int i = 0; i < mCandleCnt; i++)
            {
                if (mTimes[i] > time)
                {
                    IDX = i - 1;
                    break;
                }
            }

            if (IDX >= mCandleCnt)
                IDX = mCandleCnt - 1;

            if (IDX < 0)
                IDX = 0;

            return IDX;
            /*
            for (int i = 0; i < cnt; i++)
            {
                time = timeToSeconds(times[i]);
                time -= timeb;

                XYs[2 * i] = (short)(0 + time * ratioX);
            }
             */
        }

        public bool isShouldDrawMACD()
        {
            return mShouldDrawMACD;
        }

        void viewRealtimeInDetail()
        {
            //  convert to share
            int cnt = mTrade.getTransactionCount();

            Share share = mContext.mRealtimeShare;
            share.setDataType(Share.DATATYPE_TICK);
            share.removeAllCandles();
            share.clearCalculations();
            share.setCode(mTrade.mCode, mTrade.mFloorID);
            share.setID(mTrade.getCodeID());

            float o, c, h, l;
            int v;
            for (int i = 0; i < cnt; i++)
            {
                o = mTrade.getPrice(i);
                c = o;
                h = o;
                l = o;
                //v = mTrade.getVolume(i);
                v = mTrade.getTradeVolume(i);

                int t1 = mTrade.getTime(i);
                int h1 = (t1 >> 16)&0xff;
                int m1 = (t1>>8)&0xff;
                int s1 = t1 & 0xff;
                int mh1 = h1*60+m1;
                float devision = 10.0f;
                if (share.isIndex())
                {
                    devision = 1.0f;// 100.0f;
                }
                for (int j = i + 1; j < cnt; j++)
                {
                    int t2 = mTrade.getTime(j);
                    int h2 = (t2 >> 16) & 0xff;
                    int m2 = (t2 >> 8) & 0xff;
                    int s2 = t2 & 0xff;
                    int mh2 = h2*60+m2;
                    if (mh2 > mh1)
                    {
                        share.addMoreCandle(o / devision, c / devision, o / devision, h / devision, l / devision, v, t1);
                        o = 0;
                    }
                    else
                    {
                        float v2 = mTrade.getPrice(j);
                        if (v2 > h) h = v2;
                        if (v2 < l) l = v2;

                        c = v2;
                        v += mTrade.getTradeVolume(j);
                    }
                }

                if (i == cnt - 1 && o > 0)
                {
                    share.addMoreCandle(o / devision, c / devision, o / devision, h / devision, l / devision, v, t1);
                }

                share.saveShare();
            }

            mListener.onEvent(this, xBaseControl.EVT_ON_MOUSE_DOUBLE_CLICK, C.ID_SELECT_SHARE_CANDLE_RT, share);
        }
    }
}
