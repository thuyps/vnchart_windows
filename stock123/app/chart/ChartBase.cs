using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.framework;
using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;

namespace stock123.app.chart
{
    public class stTitle
    {
        public stTitle(string _title, uint _color)
        {
            title = _title;
            color = (int)_color;
        }

        public stTitle(string _title, int _color)
        {
            title = _title;
            color = _color;
        }

        public string title;
        public int color;
    }

    public class ChartBase : xBaseControl
    {
        public const int CHART_LINE = 100;
        public const int CHART_CANDLE = 101;
        public const int CHART_OHLC = 102;

        public const int CHART_SMA = 103;
        public const int CHART_EMA = 104;

        public const int CHART_BOLLINGER = 105;
        public const int CHART_PSAR = 106;
        public const int CHART_ICHIMOKU = 107;
        public const int CHART_MACD = 108;
        public const int CHART_STOCHASTIC_FAST = 109;
        public const int CHART_STOCHASTIC_SLOW = 110;
        public const int CHART_VOLUME = 111;
        public const int CHART_ADX = 112;
        public const int CHART_RSI = 113;
        public const int CHART_MFI = 114;
        public const int CHART_MAE = 115;
        public const int CHART_CHAIKIN = 116;
        public const int CHART_ROC = 117;
        public const int CHART_ZIGZAG = 118;
        public const int CHART_THUYPS = 119;
        public const int CHART_STOCHRSI = 120;

        public const int CHART_HLC = 121;
        public const int CHART_VOLUMEBYPRICE = 122;
        public const int CHART_OBV = 123;
        public const int CHART_TRIX = 124;
        public const int CHART_ADL = 125;

        public const int CHART_CANDLE_HEIKEN = 126;
        public const int CHART_ENVELOP = 127;
        public const int CHART_NVI = 128;
        public const int CHART_PVI = 129;
        public const int CHART_WILLIAMR = 130;
        //  new
        public const int CHART_PVT = 32;
        public const int CHART_CCI = 33;
        
        public const int CHART_PVO = 34;
        public const int CHART_VSTOP = 35;
        public const int CHART_MASSINDEX = 36;

        public const int CHART_ATR = 37;

        public const int CHART_AROON = 38;
        public const int CHART_AROON_OSCILLATOR = 39;
        public const int CHART_CFM = 40;

        public const int CHART_PAST_1_YEAR = 41;
        public const int CHART_PAST_2_YEARS = 42;
        public const int CHART_COMPARING_SECOND_SHARE = 43;
        //--------------------------------------------

        public int CHART_BORDER_SPACING_Y = 5;
        public int CHART_BORDER_SPACING_Y_MASTER = 20;
        public const int CHART_BORDER_SPACING_X = 2;
        public const int MAX_DRAW_POINT = 10000;

        protected bool mOnMouseDown = false;

        protected Context mContext;
        protected int mChartType;
        protected bool mHasSecondChart = false;

        public bool mShouldDrawPriceLine;
        public bool mShouldDrawCursor;
        protected float mLineThink;

        public int mShouldDrawAt;
        public bool mShouldDrawDateSeparator;
        public bool mShouldDrawValue;
        public bool mShouldDrawGrid = false;
        //xImage* mImgCursor;

        protected int mInternalW;
        protected int mDrawingH;
        protected int mDrawingW;

        protected int mCurrentKey;	//	for detecting share's cursor change
        protected int mLastScope;

        protected float[] mPrices = new float[5];
        protected int[] mPricesY = new int[5];
        protected short[] mChartLineXY;
        protected short[] mChartLineXY2;
        protected int mChartLineXYSize;

        protected int mVolumeBarW;

        protected float mPriceDistance;
        protected int mChartLineLength;

        protected short[] mChartLineColorArea;

        protected Font mFont;
        //	====================mouse/keyboard================
        protected bool mIsControlEnable;
        //	====================END OF mouse/keyboard================
        protected bool mRenderCursor;
        public bool mRenderOHLCInfo = true;
        protected String mTitle;
        protected String mMouseTitle;

        protected String mDate;
        protected String mVolume;
        protected String mOpen;
        protected String mClose;
        protected String mHigh;
        protected String mLow;
        protected bool mSupportDrawingTrend;

        protected Drawer mDrawer;
        public bool mShouldDrawTitle = true;
        public bool mShouldDrawPriceLabelOnRight = true;
        protected Share mShare;
        protected int mStartX;
        ChartBase mRefChart;
        //=================================================================
        public ChartBase(Font f)
            : base(null)
        {
            makeCustomRender(true);

            mFont = f;

            mRenderCursor = true;
            mIsControlEnable = true;
            mShouldDrawValue = true;
            mLastScope = -1;

            mContext = Context.getInstance();

            mStartX = 0;

            setBackgroundColor(C.COLOR_BLACK);
        }
        void test(xGraphics g)
        {
            g.setColor(C.COLOR_RED);

            g.drawLineDotHorizontal(10, 10, 100, 100);

            g.setColor(C.COLOR_MAGENTA);
            g.fillRect(30, 40, 15, 15);

            g.setColor(C.COLOR_GREEN);
            g.drawString(mContext.getFontText(), "This is a realtime chart", 100, 10, 0);

            short[] path = { 10, 20, 35, 20, 48, 30, 35, 35 };
            g.setColor(C.COLOR_BLUE);
            g.fillShapes(path, 4);
            g.drawLines(path, 4);
        }

        //======================================
        protected void drawPriceLines(xGraphics g)
        {
            if (getShare() == null)
                return;

            for (int i = 0; i < 5; i++)
            {
                if (mPricesY[i] < -2000)    //  bug
                    break;
                g.setColor(C.GREY_LINE_COLOR);
                g.drawLine(0, mPricesY[i], mInternalW, mPricesY[i]);

                String sz = (mPrices[i]).ToString("0.0");
                g.setColor(C.COLOR_GRAY_LIGHT);
                g.drawString(mFont, sz, getW() - 2, mPricesY[i], xGraphics.RIGHT | xGraphics.VCENTER);
            }

            int y = getH() - 4;

            g.setColor(C.GREY_LINE_COLOR);
            g.drawLine(0, y, getW(), y);
        }

        protected void drawPriceLines(xGraphics g, float[] prices)
        {
            if (getShare() == null)
                return;

            Share share = getShare();
            float hi = -100000;
            float lo = 100000;
            for (int i = share.mBeginIdx; i <= share.mEndIdx; i++)
            {
                if (prices[i] > hi) hi = prices[i];
                if (prices[i] < lo) lo = prices[i];
            }
            float distance = hi - lo;
            if (distance <= 0)
                return;
            float rY = (float)mDrawingH / distance;
            int mH = getH();
            int mY = 0;
            //		float rX = (float)mDrawingW/mChartLineLength;
            //price
            //rY = (float)priceDistance/drawingH;
            float priceStep = distance / 5;

            for (int i = 0; i < 5; i++)
            {
                mPrices[i] = lo + i * priceStep + priceStep / 2;
                mPricesY[i] = mY + (mH - CHART_BORDER_SPACING_Y) - (int)((mPrices[i] - lo) * rY);
            }
            //===================================
            for (int i = 0; i < 5; i++)
            {
                g.setColor(C.GREY_LINE_COLOR);
                g.drawLine(0, mPricesY[i], mInternalW, mPricesY[i]);

                String sz = (mPrices[i]).ToString("0.0");
                g.setColor(C.COLOR_GRAY_LIGHT);
                g.drawString(mFont, sz, getW() - 2, mPricesY[i], xGraphics.RIGHT | xGraphics.VCENTER);
            }

            int y = getH() - 4;

            g.setColor(C.GREY_LINE_COLOR);
            g.drawLine(0, y, getW(), y);
        }

        protected void drawDateSeparator(xGraphics g)
        {
            if (!mShouldDrawDateSeparator)
                return;

            if (getShare() == null)
                return;

            Share share = getShare();

            if (share.isRealtime())
            {
                return;
            }

            int y = getH() - 4;

            //----------draw vertical separators
            int range = share.getCursorScope();
            int[] scopes = { Share.SCOPE_1WEEKS, Share.SCOPE_1MONTH, Share.SCOPE_3MONTHS, Share.SCOPE_6MONTHS, Share.SCOPE_1YEAR, Share.SCOPE_2YEAR, Share.SCOPE_ALL, -1 };
            int scope = 1;
            int i = 1;
            while (scopes[i] != -1)
            {
                if (scopes[i] >= range)
                {
                    scope = scopes[i];
                    break;
                }
                i++;
            }
            if (range > Share.SCOPE_2YEAR)
                scope = Share.SCOPE_ALL;

            //-----------------------------------------
            int lastSeperator = -1;
            int flag = 0;
            int jump = 0;
            StringBuilder sb = new StringBuilder(100);
            String sz;
            for (i = share.mBeginIdx; i < share.mEndIdx; i++)
            {
                if (scope == Share.SCOPE_1WEEKS)
                {
                }
                else if (scope == Share.SCOPE_1MONTH)
                {
                    i = seekToFirstDayOfWeek(share, i);
                    jump = 2;
                }
                else if (scope == Share.SCOPE_3MONTHS)
                {
                    i = seekToFirstDayOfWeek(share, i);
                    jump = 2;
                }
                else if (scope == Share.SCOPE_6MONTHS)
                {
                    i = seekToFirstDayOfMonth(share, i);
                    jump = 10;
                }
                else if (scope == Share.SCOPE_1YEAR)
                {
                    i = seekToFirstDayOfMonth(share, i);
                    jump = 10;
                }
                else if (scope == Share.SCOPE_2YEAR)
                {
                    i = seekToFirstDayOfMonth(share, i);
                    jump = 16;
                    flag++;
                    if (flag == 3)
                        flag = 0;
                }
                else if (scope == Share.SCOPE_ALL)
                {
                    i = seekToFirstDayOfQ(share, i);
                    jump = 100;  //  about 6 months
                    //flag++;
                    //if (flag == 3)
                        ///flag = 0;
                }

                if (i == -1 || i >= share.mEndIdx)
                    break;

                if (flag != 0)
                    continue;

                lastSeperator = share.getDate(i);

                int m = ((lastSeperator >> 8) & 0xff) - 1;
                int d = (lastSeperator & 0xff);
                int year = (lastSeperator >> 16) & 0xffff;
                year = year % 100;

                sb.Length = 0;
                if (scope == Share.SCOPE_ALL)
                {
                    sb.AppendFormat("{0:D2}/{1:D2}", (m + 1), year);
                }
                else
                {
                    sb.AppendFormat("{0:D2}/{1:D2}", d, m + 1);
                }
                sz = sb.ToString();
                int x = candleToX(i);

                //g.setColor(C.GREY_LINE_COLOR);
                g.setColor(0x70454545);
                g.drawLine(x, y - 30, x, y);
                //g.drawLine(x, 0, x, y);
                g.setColor(C.COLOR_GRAY_LIGHT);
                Font fs = mContext.getFontSmaller();
                g.drawString(fs, sz, x, y - fs.Height, xGraphics.LEFT);

                //--------------------
                i += jump;
            }
        }
        virtual public void setChartType(int type)
        { 
            mChartType = type; 
        }

        virtual protected void clear()
        {
        }
        public void clearModifyKey() { 
            mCurrentKey = 0x0fffffff;
            mInternalW = getW() - 40;
            mDrawingH = getH() - 2 * CHART_BORDER_SPACING_Y;
            mDrawingW = mInternalW - 2 * CHART_BORDER_SPACING_X;
        }
        //=============================================
        protected int screenToCandleIndex(int x, int y)
        {
            return 0;
        }
        //=============================================
        protected void setCandleCursor(int x, int y)
        {
            int mX = 0;
            int mY = 0;
            int mH = getH();
            if (getShare() == null)
                return;

            if (y > mY && y < mY + mH)
            {
                int candleIdx = (int)xToCandleIdx(x);
                if (candleIdx >= 0 && candleIdx < getShare().getCandleCount())
                {
                    getShare().selectCandle(candleIdx);
                }
            }
        }

        protected bool isHiding() { return isShow() == false; }

        protected int getInternalW() { return mInternalW; }

        public void setRenderCursor(bool render) { mRenderCursor = render; }

        float getCandleW()
        {
            int w = mInternalW - 2 * CHART_BORDER_SPACING_X;
            if (mChartLineLength == 0)
                return 0;
            return (float)w / mChartLineLength;
        }

        public int dxToCandles(int dx)
        {
            int w = mInternalW - 2 * CHART_BORDER_SPACING_X;

            int candles = (dx * mChartLineLength) / w;

            return candles;
        }

        public int candlesToDx(int candles)
        {
            if (mChartLineLength == 0)
            {
                Share share = getShare();
                if (share != null)
                    mChartLineLength = share.mEndIdx - share.mBeginIdx + 1;
            }
            int w = mInternalW - 2 * CHART_BORDER_SPACING_X;
            float rX = (float)w / mChartLineLength;

            return (int)(candles * rX);
        }

        protected int candlesToDx(float candles)
        {
            if (mChartLineLength == 0)
            {
                Share share = getShare();
                if (share != null)
                    mChartLineLength = share.mEndIdx - share.mBeginIdx + 1;
            }
            int w = mInternalW - 2 * CHART_BORDER_SPACING_X;
            float rX = (float)w / mChartLineLength;

            return (int)(candles * rX);
        }

        protected int candleToX(int candle)
        {
            int mX = 0;
            int mY = 0;
            int mH = getH();
            if (getShare() == null)
                return 0;
            int deltaC = candle - getShare().mBeginIdx;
            int dx = candlesToDx(deltaC);

            return (mX + CHART_BORDER_SPACING_X + dx + getStartX());
        }

        public void supportDrawingTrend(bool set) { mSupportDrawingTrend = set; }

        public void drawFibonacci(xGraphics g)
        {
            if (mDrawer != null)
            {
                mDrawer.render(g);
            }
        }

        public void setTitle(String title)
        {
            mTitle = title;
        }
        virtual public String getTitle()
        {
            return mTitle;
        }

        //============================
        /*
        virtual protected void renderTitle(xGraphics g)
        {
            mTitle = getTitle();
            if (mTitle != null)
            {
                g.setColor(C.COLOR_WHITE);
                g.drawString(mFont, mTitle, 2, 1, xGraphics.LEFT | xGraphics.TOP);
            }

            renderCursor(g);
            //	bottom border
            g.setColor(0xffa0a0a0);
            g.drawHorizontalLine(0, 0, getW());

            //=============
            g.setColor(C.COLOR_FADE_YELLOW);
            g.drawHorizontalLine(0, mLastY, getW());

            if (mMouseTitle != null && mLastY != 0 & mLastY != 0)
            {
                int y = mLastY;
                int x = mLastX;
                if (y < 12)
                {
                    y = mLastY + 12;
                    x += 6; //  stay away from the mouse
                }
                g.setColor(0xa0000000);
                int sw = Utils.getStringW(mMouseTitle, mContext.getFontSmall());
                g.fillRect(x, y-12, sw, mContext.getFontSmall().Height);

                g.setColor(C.COLOR_ORANGE);
                g.drawString(mContext.getFontSmall(), mMouseTitle, x, y-12);
            }
        }
        */
        protected float[] allocMem(float[] p, int items)
        {
            if (p == null || p.Length < items)
            {
                return new float[items];
            }
            return p;
        }

        protected short[] allocMem(short[] p, int items)
        {
            if (p == null || p.Length < items)
            {
                return new short[items];
            }
            return p;
        }
        protected Share mCurrentShare;
        virtual protected bool detectShareCursorChanged()
        {
            int mH = getH();
            int mY = 0;
            Share share = getShare();

            if (share == null)
                return false;

            //	update line ordinates
            int key = share.getModifiedKey();

            int newScope = share.mEndIdx - share.mBeginIdx + 1;

            if (key != mCurrentKey || newScope != mLastScope || share != mCurrentShare)		//	share's cursor has been changed
            {
                mCurrentKey = key;
                mChartLineLength = share.mEndIdx - share.mBeginIdx + 1;
                mLastScope = share.mEndIdx - share.mBeginIdx + 1;
                if (mChartLineLength > MAX_DRAW_POINT)
                    mChartLineLength = MAX_DRAW_POINT;

                if (mInternalW == 0)
                    mInternalW = getW() - 40;
                mDrawingH = mH - 2 * CHART_BORDER_SPACING_Y;
                mDrawingW = mInternalW - 2 * CHART_BORDER_SPACING_X;

                mPriceDistance = share.getHighestPrice() - share.getLowestPrice();
                //	price lines
                float low = share.getLowestPrice();
                //	priceDistance will fit drawingH
                //	ex: 100k == 120 pixels . 1k = 1.2 pixels
                float rY = (float)mDrawingH / mPriceDistance;
                //		float rX = (float)mDrawingW/mChartLineLength;
                //price
                //rY = (float)priceDistance/drawingH;
                float priceStep = (float)mPriceDistance / 5;

                for (int i = 0; i < 5; i++)
                {
                    mPrices[i] = low + i * priceStep + priceStep / 2;
                    mPricesY[i] = mY + (mH - CHART_BORDER_SPACING_Y) - (int)((mPrices[i] - low) * rY);
                }

                mCurrentShare = share;

                if (hasDrawer())
                    mDrawer.recalcPosition();

                return true;
            }

            mCurrentShare = share;

            return false;
        }
        //=============================================
        protected void drawChartBar(xGraphics g, int[] _x, int[] _y, int barW, int[] barH, int cnt, int color)
        {
            g.setColor(color);

            for (int i = 0; i < cnt; i++)
            {
                g.fillRect(_x[i], _y[i], barW, barH[i]);
            }
        }

        //=============================================
        protected void pricesToYs(float[] price, int offset, short[] xy, int len, bool detectLowHi)
        {
            Share share = getShare();
            if (share == null)
                return;

            float priceDistance = 0;
            float low = 0;
            if (detectLowHi)
            {
                float hi = -0x0fffff;
                float lo = 0xffffff;
                for (int i = 0; i < len; i++)
                {
                    if (price[i + offset] > hi) hi = price[i + offset];
                    if (price[i + offset] < lo) lo = price[i + offset];
                }
                priceDistance = hi - lo;
                low = lo;
            }
            else
            {
                priceDistance = mPriceDistance;
                low = share.getLowestPrice();
            }

            //	priceDistance will fit drawingH
            //	ex: 100k == 120 pixels . 1k = 1.2 pixels
            float rY = (float)mDrawingH / priceDistance;

            float rX = (float)mDrawingW / (share.mEndIdx - share.mBeginIdx + 1);

            int mX = 0;
            int mY = 0;
            int mH = getH();

            //	int begin = share.mBeginIdx;
            for (int i = 0; i < len; i++)
            {
                xy[2 * i] = (short)(mX + CHART_BORDER_SPACING_X + (int)(i * rX) + getStartX());
                xy[2 * i + 1] = (short)(mY + CHART_BORDER_SPACING_Y + mDrawingH - (int)((price[i + offset] - low) * rY));
            }
        }
        protected void pricesToYs(float[] price, int offset, short[] xy, int len, float price_low, float price_hi)
        {
            Share share = getShare();
            if (share == null)
                return;

            int mX = 0;
            int mY = 0;
            int mH = getH();

            float priceDistance = price_hi - price_low;
            float low = price_low;

            //	priceDistance will fit drawingH
            //	ex: 100k == 120 pixels . 1k = 1.2 pixels
            float rY = (float)mDrawingH / priceDistance;

            float rX = (float)mDrawingW / (share.mEndIdx - share.mBeginIdx + 1);

            //	int begin = share.mBeginIdx;
            for (int i = 0; i < len; i++)
            {
                xy[2 * i] = (short)(mX + CHART_BORDER_SPACING_X + i * rX + getStartX());
                xy[2 * i + 1] = (short)(mY + CHART_BORDER_SPACING_Y + mDrawingH - (price[i+offset] - low) * rY);
            }
        }

        protected void renderCursor(xGraphics g)
        {
            if (!mShouldDrawCursor)
                return;
            Share share = getShare();
            if (share == null)
                return;

            int sel = share.getCursor();
            int x = candleToX(sel);
            g.setColor(C.COLOR_GRAY);
            g.drawLine(x, 0, x, getH());

            //  mouse title
            g.setColor(C.COLOR_FADE_YELLOW);
            g.drawLineDotHorizontal(0, mLastY, getW());

            if (mMouseTitle != null && mLastY != 0 & mLastY != 0)
            {
                int y = mLastY;
                x = mLastX;
                if (y < 12)
                {
                    y = mLastY + 12;
                    x += 6; //  stay away from the mouse
                }
                g.setColor(0xa0000000);
                int sw = Utils.getStringW(mMouseTitle, mContext.getFontSmall());
                g.fillRect(x, y - 12, sw, mContext.getFontSmall().Height);

                g.setColor(C.COLOR_ORANGE);
                g.drawString(mContext.getFontSmall(), mMouseTitle, x, y - 12);
            }

            if (mShouldDrawTitle)
            {
                renderTitles(g, 2, 0);
            }
        }

        protected void drawCandleInfo(xGraphics g)
        {
            if (!mRenderOHLCInfo)
                return;
            if (!mRenderCursor)
                return;

            int mX = 0;
            int mY = 0;
            int mH = getH();

            //	candle cursor
            Share share = getShare();
            if (share == null)
                return;
            int selCandleIdx = share.mSelectedCandle;

            if (selCandleIdx >= share.mBeginIdx && selCandleIdx <= share.mEndIdx)
            {
                //		int i = selCandleIdx - share.mBeginIdx;

                //		char sz[128];
                refreshCandleLabels(selCandleIdx);

                int y = mY + 20;
                int x = mX + 4;

                Font f = mFont;
                //f.setColor(0xff00ff00);
                g.setColor(C.COLOR_ORANGE);

                g.drawString(f, mOpen, x, y); y += mFont.Height - 2;
                //g.drawString(f, mClose, x, y); y += mFont.Height - 2;
                g.drawString(f, mHigh, x, y); y += mFont.Height - 2;
                //g.drawString(f, mLow, x, y); y += mFont.Height - 2;
                g.drawString(f, mVolume, x, y); y += mFont.Height - 2;
                g.drawString(f, mDate, x, y); y += mFont.Height - 2;
            }
        }
        virtual protected void refreshCandleLabels(int candleIdx)
        {
            Share share = getShare();
            if (share == null)
                return;
            if (candleIdx < share.getCandleCount())
            {
                String s;
                StringBuilder sb = new StringBuilder(100);
                mVolume = Utils.formatNumber(share.getVolume(candleIdx));
                sb.AppendFormat("KL: {0}", mVolume);
                mVolume = sb.ToString();

                //  open
                sb.Length = 0;
                sb.AppendFormat("O/C: {0}/{1}", formatPrice(share.getOpen(candleIdx)), formatPrice(share.getClose(candleIdx)));
                mOpen = sb.ToString();
                sb.Length = 0;
                /*
                //  close
                sb.AppendFormat("C: {0}", formatPrice(share.getClose(candleIdx)));
                mClose = sb.ToString();
                sb.Length = 0;
                 */
                //  highest
                sb.AppendFormat("H/L: {0}/{1}", formatPrice(share.getHighest(candleIdx)), formatPrice(share.getLowest(candleIdx)));
                mHigh = sb.ToString();
                sb.Length = 0;
                /*
                //  lowest
                sb.AppendFormat("L: {0}", formatPrice(share.getLowest(candleIdx)));
                mLow = sb.ToString();
                sb.Length = 0;
                */
                if (share.isRealtime())
                {
                    int date = share.getDate(candleIdx);
                    int h = (date >> 16) & 0xff;
                    int m = (date >> 8) & 0xff;
                    sb.AppendFormat("{0:D2}:{1:D2}", h, m);
                }
                else
                {
                    int date = share.getDate(candleIdx);
                    int m = ((date >> 8) & 0xff);
                    int y = ((date >> 16) % 100);
                    if (y < 100)
                        y += 2000;

                    y = y % 100;

                    int d = (date & 0xff);

                    sb.AppendFormat("{0:D2}/{1:D2}/{2:D2}", d, m, y);
                }
                mDate = sb.ToString();
            }
            else
            {
                mVolume = "0";
                mOpen = "0";
                mClose = "0";
                mHigh = "0";
                mLow = "0";
            }
        }

        StringBuilder mSb = new StringBuilder(30);
        protected String formatPrice(float v)
        {
            mSb.Length = 0;
            mSb.AppendFormat("{0:F1}", v);
            return mSb.ToString();
        }

        protected int seekToFirstDayOfWeek(Share share, int idx)
        {
            int candles = share.getCandleCount();
            if (idx >= 0 && idx < candles)
            {
                int i = 0;
                int ew = -1;    //  end of week
                
                for (i = idx; i < candles; i++)
                {
                    int date = share.getDate(i);
                    if (date == 0)
                    {
                        continue;
                    }
                    int day = Utils.dayOfWeek((date >> 16) & 0xffff, (date >> 8) & 0xff, date & 0xff);
                    if (ew == -1)
                    {
                        ew = day;
                        continue;
                    }
                    if (day < ew)
                        return i;
                    else
                        ew = day;
                }
            }
            return -1;
        }
        protected int seekToFirstDayOfMonth(Share share, int idx)
        {
            int em = -1;    //  end of month
            int candles = share.getCandleCount();
            for (int i = idx; i < candles; i++)
            {
                int date = share.getDate(i);
                int day = (date & 0xff);
                if (em == -1)
                    em = day;

                if (day < em)
                    return i;
                else
                    em = day;
            }
            return -1;
        }

        protected int seekToFirstDayOfQ(Share share, int idx)
        {
            int em = -1;    //  end of month
            int candles = share.getCandleCount();
            for (int i = idx; i < candles; i++)
            {
                int date = share.getDate(i);
                int day = (date & 0xff);
                int m = (date >> 8) & 0xff;
                if (em == -1)
                    em = day;

                if (day < em && (m == 1 || m == 6))
                {    
                    return i;
                }
                else
                    em = day;
            }
            return -1;
        }
        
        override public void setSize(int w, int h)
        {
            base.setSize(w, h);

            clearModifyKey();

            if (mDrawer != null)
            {
                mDrawer.recalcPosition();
            }
        }

        override public void setSize(xBaseControl c)
        {
            base.setSize(c.getW(), c.getH());
            clearModifyKey();

            if (mDrawer != null)
            {
                mDrawer.recalcPosition();
            }
        }

        //=======================FIBONACCIE=====================
        //===========================================
        public float xToCandleIdx(int x)
        {
            int mX = 0;
            int mY = 0;
            int mH = getH();
            if (getShare() == null)
                return 0;

            int dx = x - getStartX() - mX - CHART_BORDER_SPACING_X;

            float candle = getShare().mBeginIdx;
            candle += ((float)dx / getCandleW());

            return candle;
        }

        public float yToPrice(int y)
        {
            int mX = 0;
            int mY = 0;
            int mH = getH();

            Share share = getShare();
            if (share == null)
                return 0;

            float priceDistance = share.getHighestPrice() - share.getLowestPrice();

            int dy = mY + mDrawingH + CHART_BORDER_SPACING_Y - y;

            return share.getLowestPrice() + (float)((dy * priceDistance)) / mDrawingH;
        }

        public double yToPrice(int y, double min, double max)
        {
            int mH = getH();

            double priceDistance = max - min;

            int dy = mDrawingH + CHART_BORDER_SPACING_Y - y;

            return min + (float)((dy * priceDistance)) / mDrawingH;
        }

        public int candleToX(float candle)
        {
            int mX = 0;
            int mY = 0;
            int mH = getH();
            if (getShare() == null)
                return 0;
            float deltaC = candle - getShare().mBeginIdx;
            int dx = candlesToDx(deltaC);

            return (mX + CHART_BORDER_SPACING_X + dx + getStartX());
        }
        public int priceToY(float price)
        {
            int mX = 0;
            int mY = 0;
            int mH = getH();

            Share share = getShare();
            if (share == null)
                return 0;

            float priceDistance = share.getHighestPrice() - share.getLowestPrice();
            if (priceDistance <= 0)
                return 0;

            int dy = (int)((price - share.getLowestPrice()) * mDrawingH / priceDistance);

            return mY + CHART_BORDER_SPACING_Y + mDrawingH - dy;//y + mY + CHART_BORDER_SPACING_Y;
        }
        //=============================END FIBONACCIE========================

        //=====================default mouse=================
        protected int mLastX;
        protected int mLastY;
        bool mIsHoldingMouse = false;
        public override void onMouseDown(int x, int y)
        {
            mLastX = x;
            mLastY = y;
            mIsHoldingMouse = true;
            invalidate();
            if (hasDrawer())
                mDrawer.onMouseDown(x, y);
        }

        public override void onMouseMove(int x, int y)
        {
            if (!mIsHoldingMouse)
                return;
            if (getShare() == null)
                return;

            mLastY = y;
            invalidate();

            if (hasDrawer())
            {
                if (mDrawer.onMouseMove(x, y))
                {
                    invalidate();
                    return;
                }
            }

            int dx = x - mLastX;
            //		int dy = y - mLastY;

            int movedCandles = this.dxToCandles(dx);
            int remainX = dx - this.candlesToDx(movedCandles);

            mLastX = x - remainX;

            getShare().mModifiedKey2++;

            if (mStartX > 0)
            {
                mStartX = 0;
            }

            if (getShare() != null && movedCandles != 0)
            {
                //	add gap to the right
                if (mStartX < 0)
                {
                    mStartX += dx;
                }
                else
                {
                    if (getShare().canMove(-movedCandles))
                    {
                        getShare().moveCursor(-movedCandles);
                    }
                    else
                    {
                        mStartX += dx;
                    }
                }

                if (mStartX > 0)
                {
                    mStartX = 0;
                }

                if (mRefChart != null)
                {
                    mRefChart.setStartX(mStartX);
                }

                invalidate();

                if (dx != 0)// || mContext.mChartDrawingStart < 0)
                {
                    mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
                }
                if (mDrawer != null)
                    mDrawer.recalcPosition();

                if (hasDrawer())
                    mDrawer.recalcPosition();
            }

            //	adjust right gap
            if (false)//mContext.mChartDrawingStart < 0)
            {
                if (mStartX < 0)
                {
                    mStartX += dx;
                }
                mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);

                return;
            }
        }

        public override void onMouseUp(int x, int y)
        {
            mIsHoldingMouse = false;
            if (hasDrawer())
            {
                if (mDrawer.onMouseUp(x, y))
                    invalidate();
            }
        }

        public int getChartType()
        {
            return mChartType;
        }

        public void drawGrid(xGraphics g)
        {
            g.setColor(C.GREY_LINE_COLOR);
            int h = getH();
            int w = getW();
            int gH = h / 10;
            int gW = w / 10;
            int i = 0;
            int y = gH;
            int x = gW;
            for (i = 0; i < 9; i++)
            {
                g.drawLineDotHorizontal(0, y, getW()-40);
                g.drawVerticalLine(x, 0, h);
                y += gH;
                x += gW;
            }
        }

        public bool isSupportSecondChart()
        {
            switch (mChartType)
            {
                case CHART_MFI:
                case CHART_RSI:
                case CHART_CHAIKIN:
                case CHART_ROC:
                    return true;
                default:
                    return false;
            }
        }

        virtual public bool isSecondChartDisplaying()
        {
            return false;
        }

        virtual public void toggleSecondChart()
        {
        }

        public void setHasDrawer(bool hasDrawer)
        {
            if (mDrawer == null && hasDrawer)
            {
                mDrawer = new Drawer();
                mDrawer.setChart(this);
                mDrawer.enableSaveFile(true);

                mDrawer.initFibonaccie(this, mContext.getFontSmall(), getShare());
            }
            else
            {
                mDrawer = null;
            }
        }

        public bool hasDrawer()
        {
            return mDrawer != null;
        }

        public void renderDrawer(xGraphics g)
        {
            if (hasDrawer())
            {
                mDrawer.render(g);
            }
        }

        override public void onKeyPress(int key)
        {
            if (hasDrawer())
            {
                if (mDrawer.onKeyUp(key))
                    invalidate();
            }
        }

        override public void onMouseDoubleClick()
        {
            if (mListener != null)
            {
                mListener.onEvent(this, xBaseControl.EVT_ON_MOUSE_DOUBLE_CLICK, 0, null);
            }
        }

        virtual public xVector getTitles()
        {
            return null;
        }

        virtual public int renderTitles(xGraphics g, int x, int y)
        {
            xVector v = getTitles();
            if (v != null)
            {
                for (int i = 0; i < v.size(); i++)
                {
                    stTitle t = (stTitle)v.elementAt(i);
                    g.setColor(t.color);
                    g.drawString(mFont, t.title, x, y);

                    x += g.getStringWidth(mFont, t.title) + 20;
                }
            }

            return x;
        }

        public void fillColorGreen(xGraphics g, short[] line, int pointCount, short baseline)
        {
            mChartLineColorArea = allocMem(mChartLineColorArea, pointCount * 2 + 20);

            short x = 0;
            short x0 = line[0];
            int j = 0;
            g.setColor(0x6000ff00);
            for (int i = 1; i < pointCount; i++)
            {
                int y = line[2 * i + 1];
                x = line[2 * i];
                if (y <= baseline)
                {
                    if (j == 0)
                    {
                        mChartLineColorArea[2 * j] = (short)(x0 + (x - x0) / 2);
                        mChartLineColorArea[2 * j + 1] = (short)baseline;
                        j++;
                    }
                    {
                        mChartLineColorArea[2 * j] = x;
                        mChartLineColorArea[2 * j + 1] = line[2 * i + 1];
                        j++;
                    }
                }
                else if (y > baseline)
                {
                    if (j >= 2)
                    {
                        mChartLineColorArea[2 * j] = (short)(x0 + (x - x0) / 2);
                        mChartLineColorArea[2 * j + 1] = (short)baseline;
                        j++;

                        g.fillShapes(mChartLineColorArea, j);
                    }
                    j = 0;
                }
                x0 = x;
            }
            if (j > 1 && pointCount > 2)
            {
                x = line[2 * (pointCount - 1)];
                mChartLineColorArea[2 * j] = x;
                mChartLineColorArea[2 * j + 1] = (short)baseline;
                j++;

                g.fillShapes(mChartLineColorArea, j);
            }
        }

        public void fillColorRed(xGraphics g, short[] line, int pointCount, short baseline)
        {
            mChartLineColorArea = allocMem(mChartLineColorArea, pointCount * 2 + 20);
            //========red area
            int j = 0;
            g.setColor(0x80ff0000);
            short x = 0;
            int x0 = line[0];
            for (int i = 1; i < pointCount; i++)
            {
                x = line[2 * i];
                int y = line[2 * i + 1];
                if (y >= baseline)
                {
                    if (j == 0)
                    {
                        mChartLineColorArea[2 * j] = (short)(x0 + ((x - x0) / 2));
                        mChartLineColorArea[2 * j + 1] = (short)baseline;
                        j++;
                    }
                    {
                        mChartLineColorArea[2 * j] = x;
                        mChartLineColorArea[2 * j + 1] = line[2 * i + 1];
                        j++;
                    }
                }
                else if (y < baseline)
                {
                    if (j >= 2)
                    {
                        mChartLineColorArea[2 * j] = (short)(x0 + ((x - x0) / 2));
                        mChartLineColorArea[2 * j + 1] = (short)baseline;
                        j++;

                        g.fillShapes(mChartLineColorArea, j);
                    }
                    j = 0;
                }
                x0 = x;
            }
            if (j > 1 && pointCount > 2)
            {
                x = line[2 * (pointCount - 1)];

                mChartLineColorArea[2 * j] = x;
                mChartLineColorArea[2 * j + 1] = (short)baseline;
                j++;

                g.fillShapes(mChartLineColorArea, j);
            }
        }

        public void setShare(Share share)
        {
            mShare = share;
        }

        public Share getShare(int days)
        {
            Share share = getShare();
            if (share != null)
            {
                return share.getCandleCnt() >= days ? share : null;
            }
            return null;
        }
        public Share getShare()
        {
            if (mRefChart != null)
            {
                return mRefChart.getShare();
            }

            if (mShare == null && mContext.mShareManager.getVnindexCnt() > 0)
            {
                mShare = mContext.mShareManager.getVnindexShareAt(0);
            }

            if (mShare == null)
            {
                mShare = mContext.getSelectedShare();
            }

            return mShare;
        }

        public void setRefChart(ChartBase refChart)
        {
            mRefChart = refChart;
        }

        public void setStartX(int x)
        {
            mStartX = x;
            if (mRefChart != null)
            {
                mRefChart.setStartX(x);
            }
        }

        public int getStartX()
        {
            if (mRefChart != null)
            {
                return mRefChart.getStartX();
            }

            return mStartX;
        }
    }
}
