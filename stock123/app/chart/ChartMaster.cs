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
    public class ChartMaster: ChartBase
    {
        public const int CURSOR_NORMAL = 0;
        public const int CURSOR_CROSSHAIR = 1;

        int mMouseX = 0;
        int mMouseY = 0;

        bool mSelecting = false;
        int mSelectionBX = -1;  //  begin selection
        int mSelectionEX = -1;  //  end selection

        public bool mHasFibonacci = false;
        bool mAllowRepositionCursor;    

        short[] mPricelines = new short[10];

        ChartLine mChartSMA;

        int mCursorType;

        bool mIsMasterChart = false;

        public xVector mAttachedCharts = new xVector(10);
        public xVector mOverlayAttachedCharts = new xVector(10);

        String mComparingShareCode;
        //=========================================

        public ChartMaster(Font f):base(f)
        {
            mChartType = CHART_LINE;
            mVolume = "";
            mOpen = "";
            mClose = "";
            mAllowRepositionCursor = false;
            mLineThink = 1.0f;

            mCursorType = CURSOR_CROSSHAIR;

            //CHART_BORDER_SPACING_Y_MASTER = 20;
            //CHART_BORDER_SPACING_Y = CHART_BORDER_SPACING_Y_MASTER;
        }

        override public void render(xGraphics g)
        {
            if (isHiding())
                return;

            Share share = getShare(3);
            if (share == null || share.mCode == null || share.mCode.Length == 0 || share.getCandleCount() < 3)
            {
                return;
            }

            //g.setColor(Constants.COLOR_BLACK);
            //g.clear();

            if (getShare(3) == null)
            {
                return;
            }

            //-----------------------------------
            for (int i = 0; i < mAttachedCharts.size(); i++)
            {
                ChartBase c = (ChartBase)mAttachedCharts.elementAt(i);
                c.setRefChart(this);
                c.setSize(this);

                c.setRefChartForSize(this);
            }
            if (mChartSMA != null)
            {
                mChartSMA.setRefChart(this);
                mChartSMA.setSize(this);

                mChartSMA.setRefChartForSize(this);
            }
            //-----------------------------------

            if (isAttachedOn(CHART_PAST_1_YEAR))
            {
                getShare().calcYearOfPast(1);
            }
            if (isAttachedOn(CHART_PAST_2_YEARS))
            {
                getShare().calcYearOfPast(2);
            }

            if (mChartType == CHART_LINE)
            {
                drawChartLine(g);
            }
            else if (mChartType == CHART_CANDLE)
            {
                drawChartCandle(g);
            }
            else if (mChartType == CHART_CANDLE_HEIKEN)
            {
                drawChartCandleHeiken(g);
            }
            else if (mChartType == CHART_OHLC)
            {
                drawChartOHLC(g);
            }
            else if (mChartType == CHART_HLC)
            {
                drawChartOHC(g);
            }

            if (mHasFibonacci && mDrawer != null)
            {
                mDrawer.render(g);
            }

            //  refresh
            if (mIsMasterChart)
            {
                g.setColor(C.COLOR_GREEN);
                int refreshing = GlobalData.vars().getValueInt("share_refreshing", 0);
                g.drawString(Context.getInstance().getFontSmaller(), refreshing==0?"[Refresh]":"...", 
                    getW() - 32, getH() - 20, xGraphics.HCENTER);
            }
        }

        virtual public void renderOverlayAttachedCharts(xGraphics g)
        {
            if (mOverlayAttachedCharts != null)
            {
                for (int i = 0; i < mOverlayAttachedCharts.size(); i++)
                {
                    ChartBase c = (ChartBase)mOverlayAttachedCharts.elementAt(i);
                    c.render(g);
                }
            }

            if (mChartSMA != null)
            {
                mChartSMA.render(g);
            }
        }

        virtual public void renderAttachedCharts(xGraphics g)
        {
            if (mAttachedCharts != null)
            {
                for (int i = 0; i < mAttachedCharts.size(); i++)
                {
                    ChartBase c = (ChartBase)mAttachedCharts.elementAt(i);
                    c.render(g);
                }
            }
        }

        override public String getTitle()
        {
            return "";
        }
        public void allowRepositionCursor()
        {
            mAllowRepositionCursor = true;
        }

        protected void drawChartLine(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            Share share = getShare(3);
            if (share == null)
                return;

            //	update line ordinates
            if (detectShareCursorChanged() && mChartLineLength > 0)		//	share's cursor has been changed
            {
                if (mHasFibonacci && mDrawer != null)
                {
                    mDrawer.setChart(this);
                    mDrawer.recalcPosition();
                }
                //=======================
                int items = mChartLineLength * 2;

                mChartLineXY = allocMem(mChartLineXY, items);

                float lo = share.getLowestPrice();
                float hi = share.getHighestPrice();
                mPriceDistance = share.getHighestPrice() - lo;

                pricesToYs(share.mCClose, share.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);
            }

            if (mChartLineXY == null)
                return;

            //===============================================
            if (mShouldDrawPriceLine)
            {
                drawPriceLines(g);
            }

            drawDateSeparator(g);

            //=============attached charts=====
            renderAttachedCharts(g);
            //=============main chart=======
            g.setColor(0xffa0a0a0);
            float thinkness = 2.0f;
            if (share.getCursorScope() > 600)
                thinkness = 1.0f;
            g.drawLines(mChartLineXY, mChartLineLength, thinkness);
            //===================overlay charts============
            renderOverlayAttachedCharts(g);
            //==========================
            //  share code
            g.setColor(C.COLOR_WHITE);
            stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(share.mID);
            if (inf == null)
            {
                if (share.mName != null)
                    g.drawString(mFont, "#" + share.mCode + " - " + share.mName, 2, 0);
                else
                    g.drawString(mFont, "#" + share.getCode(), 2, 0);
            }
            else
                g.drawString(mFont, "#" + share.mCode + " - " + inf.company_name, 2, 0);

            if (true)//!mIsBackgroundChart)
            {
                int selCandleIdx = share.mSelectedCandle;

                if (selCandleIdx >= share.mBeginIdx && selCandleIdx <= share.mEndIdx)
                {
                    int i = selCandleIdx - share.mBeginIdx;
                    int x = mChartLineXY[2 * i];
                    int y = mChartLineXY[2 * i + 1];

                    renderMasterCursor(g, x, y);
                }
            }
        }

        protected void drawChartCandle(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            Share share = getShare(3);
            if (share == null)
                return;

            //	update line ordinates
            if (detectShareCursorChanged())		//	share's cursor has been changed
            {
            }

            //===============================================
            if (mShouldDrawPriceLine)
            {
                drawPriceLines(g);
            }

            drawDateSeparator(g);

            //=============attached charts=====
            renderAttachedCharts(g);
            //=============main chart=======

            //-------draw candles-------------
            //	PriceDistance	- drawingH
            //	price			- ?
            int candleW = (int)(((float)getDrawingW() / mChartLineLength) * 2.0f / 3);

            if (candleW % 2 == 0) candleW++;
            int candleWHalf = candleW / 2 + 1;

            Share s = getShare();
            int b = s.mBeginIdx;
            int e = s.mEndIdx;

            float candleBodyH = 0;
            uint colorUp = 0xff00ff00;
            uint colorDown = 0xffff0000;
            uint color;

            int x, y;
            float low = s.getLowestPrice();
            float ry = (float)getDrawingH() / mPriceDistance;
            float rw = (float)getDrawingW() / mChartLineLength;

            int shadowH = 0;
            int shadowY = 0;

            //	cursor
            int cursorX = 0, cursorY = 0;
            int selCandle = s.mSelectedCandle;

            int maxCandleH = getDrawingH() / 2;

            int j = 0;
            float o, c, h, l;

            for (int i = b; i <= e; i++)
            {
                o = share.getOpen(i);
                c = share.getClose(i);
                h = share.getHighest(i);
                l = share.getLowest(i);
                candleBodyH = c - o;
                candleBodyH = (float)candleBodyH * ry;

                if (candleBodyH > maxCandleH) candleBodyH = maxCandleH;
                if (candleBodyH == 0) candleBodyH = 1;

                color = colorUp;

                if (candleBodyH < 0)
                {
                    color = colorDown; candleBodyH = -candleBodyH;
                    y = (int)(mY + getMarginY() + getDrawingH() - (o - low) * ry);
                }
                else
                    y = (int)(mY + getMarginY() + getDrawingH() - (c - low) * ry);

                x = (int)(mX + CHART_BORDER_SPACING_X + j * rw - candleW / 2 + getStartX());
                //x += 1;

                //	candle shadow	
                float tmp = h - l;
                shadowH = (int)((float)tmp * ry);
                shadowY = (int)(mY + getMarginY() + getDrawingH() - (h - low) * ry);
                g.setColor(0xffffffff);
                g.drawLine(x + candleWHalf-1, shadowY, x + candleWHalf-1, shadowY + shadowH);

                if (i == selCandle)
                {
                    cursorX = x;
                    cursorY = shadowY - 1;
                }

                //	candle body
                g.setColor(color);
                //if (candleBodyH == 0)
                //	g.drawLine(x, y, x+candleW, y);
                //else
                if (candleBodyH < 1)
                    candleBodyH = 1;
                g.fillRect(x, y, candleW, (int)candleBodyH);
                j++;
            }

            //===================overlay charts============
            renderOverlayAttachedCharts(g);
            //==========================

            //  share code
            g.setColor(C.COLOR_WHITE);
            stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(share.mID);
            if (inf == null)
            {
                if (share.mName != null)
                    g.drawString(mFont, "#" + share.mCode + " - " + share.mName, 2, 0);
                else
                    g.drawString(mFont, "#" + share.getCode(), 2, 0);
            }
            else
                g.drawString(mFont, "#" + share.mCode + " - " + inf.company_name, 2, 0);

            //  draw cursor
            if (true)//!mIsBackgroundChart)
            {
                int selCandleIdx = share.mSelectedCandle;

                if (selCandleIdx >= share.mBeginIdx && selCandleIdx <= share.mEndIdx)
                {
                    renderMasterCursor(g, cursorX, cursorY);
                }
            }
        }

        protected void drawChartCandleHeiken(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            Share share = getShare(3);
            if (share == null)
                return;

            //	update line ordinates
            if (detectShareCursorChanged())		//	share's cursor has been changed
            {
            }

            //===============================================
            if (mShouldDrawPriceLine)
            {
                drawPriceLines(g);
            }

            drawDateSeparator(g);

            //=============attached charts=====
            renderAttachedCharts(g);
            //=============main chart=======

            //-------draw candles-------------
            //	PriceDistance	- drawingH
            //	price			- ?
            int candleW = (int)(((float)getDrawingW() / mChartLineLength) * 2.0f / 3);

            if (candleW % 2 == 0) candleW++;
            int candleWHalf = candleW / 2 + 1;

            Share s = getShare();
            int b = s.mBeginIdx;
            int e = s.mEndIdx;

            float candleBodyH = 0;
            uint colorUp = 0xff00ff00;
            uint colorDown = 0xffff0000;
            uint color;

            int x, y;
            float low = s.getLowestPrice();
            float ry = (float)getDrawingH() / mPriceDistance;
            float rw = (float)getDrawingW() / mChartLineLength;

            int shadowH = 0;
            int shadowY = 0;

            //	cursor
            int cursorX = 0, cursorY = 0;
            int selCandle = s.mSelectedCandle;

            int maxCandleH = getDrawingH() / 2;

            int j = 0;
            float o, c, h, l;
            float ho, hc, hh, hl;
            float last_ho = -1;
            float last_hc = -1;
            /*
                –haClose = (Open + High + Low+ Close) / 4
                –haOpen = (haOpen(previous bar) + haClose(previous bar))/2
                –haHigh = Maximum(High, haOpen)
                –haLow = Minimum(Low,haOpen)
             */

            int pre = 4;            //  try to calc a few of pre-candles
            if (b - pre < 0)
                pre = b;
            for (int i = b-pre; i <= e; i++)
            {
                o = share.getOpen(i);
                c = share.getClose(i);
                h = share.getHighest(i);
                l = share.getLowest(i);
                //-----------------------------------
                if (last_hc == -1)
                {
                    last_hc = (o + c + h + l)/4;
                    last_ho = (o + c + h + l) / 4;
                }

                hc = (o + h + l + c) / 4;
                ho = (last_ho + last_hc) / 2;
                hh = h > ho ? h : ho;
                hl = l < ho ? l : ho;

                last_ho = ho;
                last_hc = hc;

                if (i < b)
                    continue;
                //-------------------------
                candleBodyH = hc - ho;
                candleBodyH = (float)candleBodyH * ry;

                if (candleBodyH > maxCandleH) candleBodyH = maxCandleH;
                if (candleBodyH == 0) candleBodyH = 1;

                color = colorUp;

                if (candleBodyH < 0)
                {
                    color = colorDown; candleBodyH = -candleBodyH;
                    y = (int)(mY + getMarginY() + getDrawingH() - (ho - low) * ry);
                }
                else
                    y = (int)(mY + getMarginY() + getDrawingH() - (hc - low) * ry);

                x = (int)(mX + CHART_BORDER_SPACING_X + j * rw - candleW / 2 + getStartX());
                //x += 1;

                //	candle shadow	
                float tmp = hh - l;
                shadowH = (int)((float)tmp * ry);
                shadowY = (int)(mY + getMarginY() + getDrawingH() - (hh - low) * ry);
                g.setColor(0xffffffff);
                g.drawLine(x + candleWHalf - 1, shadowY, x + candleWHalf - 1, shadowY + shadowH);

                if (i == selCandle)
                {
                    cursorX = x;
                    cursorY = shadowY - 1;
                }

                //	candle body
                g.setColor(color);
                //if (candleBodyH == 0)
                //	g.drawLine(x, y, x+candleW, y);
                //else
                g.fillRect(x, y, candleW, (int)candleBodyH);
                j++;
            }

            //===================overlay charts============
            renderOverlayAttachedCharts(g);
            //==========================

            //  share code
            g.setColor(C.COLOR_WHITE);
            stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(share.mID);
            if (inf == null)
            {
                if (share.mName != null)
                    g.drawString(mFont, "#" + share.mCode + " - " + share.mName, 2, 0);
                else
                    g.drawString(mFont, "#" + share.getCode(), 2, 0);
            }
            else
                g.drawString(mFont, "#" + share.mCode + " - " + inf.company_name, 2, 0);

            //  draw cursor
            if (true)//!mIsBackgroundChart)
            {
                int selCandleIdx = share.mSelectedCandle;

                if (selCandleIdx >= share.mBeginIdx && selCandleIdx <= share.mEndIdx)
                {
                    renderMasterCursor(g, cursorX, cursorY);
                }
            }
        }

        protected void drawChartOHLC(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            Share share = getShare(3);
            if (share == null)
                return;

            //	update line ordinates
            if (detectShareCursorChanged())		//	share's cursor has been changed
            {
            }

            //===============================================
            if (mShouldDrawPriceLine)
            {
                drawPriceLines(g);
            }

            drawDateSeparator(g);

            //=============attached charts=====
            renderAttachedCharts(g);
            //=============main chart=======

            //-------draw candles-------------
            //	PriceDistance	- drawingH
            //	price			- ?
            int candleW = (int)(((float)getDrawingW() / mChartLineLength) * 2.0f / 3);

            if (candleW % 2 == 0) candleW++;
            int candleWHalf = candleW / 2 + 1;

            Share s = getShare();
            int b = s.mBeginIdx;
            int e = s.mEndIdx;

            uint colorUp = 0xff00ff00;
            uint colorDown = 0xffff0000;
            uint color;

            int x, y;
            float low = s.getLowestPrice();
            float ry = (float)getDrawingH() / mPriceDistance;
            float rw = (float)getDrawingW() / mChartLineLength;

            int shadowH = 0;
            int shadowY = 0;

            //	cursor
            int cursorX = 0, cursorY = 0;
            int selCandle = s.mSelectedCandle;

            int maxCandleH = getDrawingH() / 2;

            int j = 0;
            float o, c, h, l;

            for (int i = b; i <= e; i++)
            {
                o = share.getOpen(i);
                c = share.getClose(i);
                h = share.getHighest(i);
                l = share.getLowest(i);

                color = colorUp;

                if (c < o)
                {
                    color = colorDown;
                }
                if (c == o)
                    color = C.COLOR_YELLOW;

                //y = (int)(mY + CHART_BORDER_SPACING_Y + mDrawingH - (o - low) * ry);

                x = (int)(mX + CHART_BORDER_SPACING_X + j * rw - rw / 2 + getStartX());

                //	H-L vertical line
                float tmp = h - l;
                shadowH = (int)((float)tmp * ry);
                shadowY = (int)(mY + getMarginY() + getDrawingH() - (h - low) * ry);
                g.setColor(color);

                g.drawLine(x + candleWHalf, shadowY, x + candleWHalf, shadowY + shadowH);

                if (i == selCandle)
                {
                    cursorX = x;
                    cursorY = shadowY - 1;
                }

                //  open
                y = (int)(mY + getMarginY() + getDrawingH() - (o - low) * ry);
                g.drawHorizontalLine(x, y, candleWHalf);
                //  close
                y = (int)(mY + getMarginY() + getDrawingH() - (c - low) * ry);
                g.drawHorizontalLine(x+candleWHalf+1, y, candleWHalf);

                j++;
            }

            //===================overlay charts============
            renderOverlayAttachedCharts(g);
            //==========================
            //  share code
            g.setColor(C.COLOR_WHITE);
            stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(share.mID);
            if (inf == null)
            {
                if (share.mName != null)
                    g.drawString(mFont, "#" + share.mCode + " - " + share.mName, 2, 0);
                else
                    g.drawString(mFont, "#" + share.getCode(), 2, 0);
            }
            else
                g.drawString(mFont, "#" + share.mCode + " - " + inf.company_name, 2, 0);

            //  draw cursor
            if (true)//!mIsBackgroundChart)
            {
                int selCandleIdx = share.mSelectedCandle;

                if (selCandleIdx >= share.mBeginIdx && selCandleIdx <= share.mEndIdx)
                {
                    renderMasterCursor(g, cursorX, cursorY);
                }
            }
        }

        protected void drawChartOHC(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            Share share = getShare(3);
            if (share == null)
                return;

            //	update line ordinates
            if (detectShareCursorChanged())		//	share's cursor has been changed
            {
            }

            //===============================================
            if (mShouldDrawPriceLine)
            {
                drawPriceLines(g);
            }

            drawDateSeparator(g);

            //=============attached charts=====
            renderAttachedCharts(g);
            //=============main chart=======

            //-------draw candles-------------
            //	PriceDistance	- drawingH
            //	price			- ?
            int candleW = (int)(((float)getDrawingW() / mChartLineLength) * 2.0f / 3);

            if (candleW % 2 == 0) candleW++;
            int candleWHalf = candleW / 2 + 1;

            Share s = getShare();
            int b = s.mBeginIdx;
            int e = s.mEndIdx;

            uint colorUp = 0xff00ff00;
            uint colorDown = 0xffff0000;
            uint color;

            int x, y;
            float low = s.getLowestPrice();
            float ry = (float)getDrawingH() / mPriceDistance;
            float rw = (float)getDrawingW() / mChartLineLength;

            int shadowH = 0;
            int shadowY = 0;

            //	cursor
            int cursorX = 0, cursorY = 0;
            int selCandle = s.mSelectedCandle;

            int maxCandleH = getDrawingH() / 2;

            int j = 0;
            float o, c, h, l;

            for (int i = b; i <= e; i++)
            {
                o = share.getOpen(i);
                c = share.getClose(i);
                h = share.getHighest(i);
                l = share.getLowest(i);

                color = colorUp;

                if (c < o)
                {
                    color = colorDown;
                }
                if (c == o)
                    color = C.COLOR_YELLOW;

                //y = (int)(mY + CHART_BORDER_SPACING_Y + mDrawingH - (o - low) * ry);

                x = (int)(mX + CHART_BORDER_SPACING_X + j * rw - rw / 2 + getStartX());

                //	H-L vertical line
                float tmp = h - l;
                shadowH = (int)((float)tmp * ry);
                shadowY = (int)(mY + getMarginY() + getDrawingH() - (h - low) * ry);
                g.setColor(color);

                g.drawLine(x + candleWHalf, shadowY, x + candleWHalf, shadowY + shadowH);

                if (i == selCandle)
                {
                    cursorX = x;
                    cursorY = shadowY - 1;
                }

                //  open
                //==========y = (int)(mY + CHART_BORDER_SPACING_Y + mDrawingH - (o - low) * ry);
                //==========g.drawHorizontalLine(x, y, candleWHalf);
                //  close
                y = (int)(mY + getMarginY() + getDrawingH() - (c - low) * ry);
                g.drawHorizontalLine(x+1, y, 2*candleWHalf-1);

                j++;
            }

            //===================overlay charts============
            renderOverlayAttachedCharts(g);
            //==========================
            //  share code
            g.setColor(C.COLOR_WHITE);
            stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(share.mID);
            if (inf == null)
            {
                if (share.mName != null)
                    g.drawString(mFont, "#" + share.mCode + " - " + share.mName, 2, 0);
                else
                    g.drawString(mFont, "#" + share.getCode(), 2, 0);
            }
            else
                g.drawString(mFont, "#" + share.mCode + " - " + inf.company_name, 2, 0);

            //  draw cursor
            if (true)//!mIsBackgroundChart)
            {
                int selCandleIdx = share.mSelectedCandle;

                if (selCandleIdx >= share.mBeginIdx && selCandleIdx <= share.mEndIdx)
                {
                    renderMasterCursor(g, cursorX, cursorY);
                }
            }
        }

        virtual protected void renderMasterCursor(xGraphics g, int cx, int cy)
        {
            if (!mRenderCursor)
                return;

            //	candle cursor
            Share share = getShare();
            if (share == null)
                return;
            int selCandleIdx = share.mSelectedCandle;

            if (selCandleIdx >= share.mBeginIdx && selCandleIdx <= share.mEndIdx)
            {
                //g.drawImage(mImgCursor, cx - mImgCursor.getWidth()/2, cy-5);
                if (true)//!mSelecting && isKeyPressing(System.Windows.Forms.Keys.Alt)) //  crosshair
                {
                    renderCursorCrossHair(g);
                }
                else
                {
                    renderCursor(g);
                }

                drawCandleInfo(g);
            }

            renderSelection(g);
        }

        override public void onMouseDoubleClick(int x, int y)
        {
            if (mHasFibonacci)
            {
                if (mDrawer.onMouseDoubleClick(x, y))
                {
                    return;
                }
            }

            Share share = getShare();
            if (share != null)
            {
                if (share.focusAtSelected())
                {
                    mListener.onEvent(this, C.EVT_FOCUS_AT_CURSOR, 0, null);
                    mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
                }
            }
        }

        override public void onMouseDown(int x, int y)
        {
            mMouseX = x;
            mMouseY = y;

            //----zooming area------------------
            zoomVx = getW() - 60;
            if (x > zoomVx)
            {
                //  zooming???
                isZoomingV = true;
                zoomVX0 = x;
                zoomVY0 = y;
            }
            //----------------------

            if (mSelecting)
            {
                if (!isKeyPressing(System.Windows.Forms.Keys.Shift))
                {
                    mSelecting = false;
                    mSelectionBX = -1;
                }
            }
            //=================================

            Share s = getShare();
            if (s == null)
                return;

            mOnMouseDown = true;
            if (mHasFibonacci)
            {
                if (isKeyPressing(System.Windows.Forms.Keys.Control))
                {
                    if (!mDrawer.isShow())
                    {
                        mDrawer.show(true);
                    }
                }
                if (mDrawer.isShow())
                {
                    if (mDrawer.onMouseDown(x, y))
                    {
                        if (mListener != null)
                            mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
                        return;
                    }
                }
            }
            if (mAllowRepositionCursor)
            {
                int cursor = s.getCursor();
                setCandleCursor(x, y);

                if (s.getCursor() != cursor)
                {
                    if (mListener != null)
                        mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
                }
            }
        }

        int zoomVx = 0;
        int zoomVX0 = 0;
        int zoomVY0 = 0;
        bool isZoomingV = false;
        override public void onMouseMove(int x, int y)
        {
            if (!mOnMouseDown)
            {
                return;
            }

            mMouseX = x;
            mMouseY = y;

            Share s = getShare();
            if (s == null)
            {
                return;
            }

            //  zooming???
            if (isZoomingV)
            {
                if (x < getW() - 60)
                {
                    isZoomingV = false;
                    return;
                }
                float delta = zoomVY0 - y;

                float t = delta / getH();
                float scale = getScaleY();
                scale += t;

                setScaleY(scale);
                //xUtils.trace(String.format("scaling: change: %.2f scale=%.2f", t, scale));

                zoomVY0 = y;
                clearModifyKey();
                if (mListener != null)
                {
                    mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
                }

                return;
            }
            //------------------------------

            //===============detect selection===========
            if (isKeyPressing(System.Windows.Forms.Keys.Shift))
            {
                mSelecting = true;
                if (mSelectionBX == -1) mSelectionBX = x;
                else mSelectionEX = x;

                //if (mListener != null)
                    //mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
                invalidate();

                return;
            }

            if (mHasFibonacci)
            {
                if (mDrawer.isShow())
                {
                    if (mDrawer.onMouseMove(x, y))
                    {
                        if (mListener != null)
                            mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);

                        return;
                    }
                }
            }

            if (mAllowRepositionCursor)
            {
                int cursor = s.getCursor();
                setCandleCursor(x, y);

                if (s.getCursor() != cursor)
                {
                    if (mListener != null)
                        mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
                }
                else
                {
                    /*
                    if (isKeyPressing(System.Windows.Forms.Keys.Alt))
                    {
                        if (mListener != null)
                            mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
                    }
                     */
                }
                invalidate();
            }
        }
        override public void onMouseUp(int x, int y)
        {
            mMouseX = x;
            mMouseY = y;
            invalidate();
            getControl().Focus();
            if (!mIsMasterChart)
            {
                base.onMouseUp(x, y);
                return;
            }
            mOnMouseDown = false;
            Share s = getShare();
            if (s == null)
            {
                return;
            }


            if (mIsMasterChart && x > getW() - 50 && y > getH() - 22)
            {
                //  refresh
                if (mListener != null)
                {
                    mListener.onEvent(this, C.EVT_REFRESH_SHARE_DATA, 0, null);

                    invalidate();

                    return;
                }
            }


            if (mHasFibonacci)
            {
                if (mDrawer.isShow())
                {
                    if (mDrawer.onMouseUp(x, y))
                    {
                        if (mListener != null)
                            mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);

                        return;
                    }
                }
            }
        }

        public void attachChart(int chart)
        {
            Share share = getShare();
            if (share == null)
            {
                return;
            }

            if (CHART_COMPARING_SECOND_SHARE == chart)
            {
                ChartLine c = new ChartLine(mFont);
                c.setSize(this);
                c.setChartType(chart);
                mAttachedCharts.addElement(c);
            }
            else if (CHART_PAST_1_YEAR == chart || CHART_PAST_2_YEARS == chart)
            {
                ChartLine c = new ChartLine(mFont);
                c.setSize(this);
                c.setChartType(chart);
                mAttachedCharts.addElement(c);
            }
            if (CHART_ENVELOP == chart)
            {
                ChartEnvelopes en = new ChartEnvelopes(mFont);
                en.setSize(this);
                mAttachedCharts.addElement(en);
            }
            if (CHART_VOLUMEBYPRICE == chart)
            {
                ChartVolumeByPrice vp = new ChartVolumeByPrice(mFont);
                vp.setSize(this);
                mAttachedCharts.addElement(vp);
            }
            if (CHART_ZIGZAG == chart)
            {
                ChartZigzag zz = new ChartZigzag(mFont);
                zz.setSize(this);
                mAttachedCharts.insertElementAt(zz, 0);
            }
            if (CHART_BOLLINGER == chart)
            {
                ChartBollinger bb = new ChartBollinger(mFont);
                bb.setSize(this);
                mAttachedCharts.insertElementAt(bb, 0);
                share.mIsCalcBollinger = false;
            }
            if (CHART_ICHIMOKU == chart)
            {
                ChartIchimoku ichi = new ChartIchimoku(mFont);
                ichi.setSize(this);
                mAttachedCharts.insertElementAt(ichi, 0);
                share.mIsCalcIchimoku = false;
            }
            if (CHART_PSAR == chart)
            {
                ChartParabollic psar = new ChartParabollic(mFont);
                psar.setSize(this);
                mAttachedCharts.addElement(psar);

                share.mIsCalcPSAR = false;
            }
            if (CHART_VSTOP == chart)
            {
                ChartVSTOP vstop = new ChartVSTOP(mFont);
                vstop.setSize(this);
                mAttachedCharts.addElement(vstop);
            }

            invalidate();
        }

        public void showAttachChart(int chart)
        {
            Share share = getShare();
            if (share == null)
                return;
            for (int i = 0; i < mAttachedCharts.size(); i++)
            {
                ChartBase c = (ChartBase)mAttachedCharts.elementAt(i);
                if (c.getChartType() == chart)
                {
                    //  added already
                    return;
                }
            }

            attachChart(chart);
        }

        public void hideAttachChart(int chart)
        {
            for (int i = 0; i < mAttachedCharts.size(); i++)
            {
                ChartBase c = (ChartBase)mAttachedCharts.elementAt(i);
                if (c.getChartType() == chart)
                {
                    mAttachedCharts.removeElementAt(i);
                    invalidate();
                    return;
                }
            }
            invalidate();
        }

        public void compareToChart(String code)
        {
            mComparingShareCode = code;
            showAttachChart(CHART_COMPARING_SECOND_SHARE);
        }

        //  bollinger, psar or ichimoku
        public void toggleAttachChart(int chart)
        {
            Share share = getShare();
            if (share == null)
                return;
            for (int i = 0; i < mAttachedCharts.size(); i++)
            {
                ChartBase c = (ChartBase)mAttachedCharts.elementAt(i);
                if (c.getChartType() == chart)
                {
                    mAttachedCharts.removeElementAt(i);
                    invalidate();
                    return;
                }
            }

            attachChart(chart);
        }

        public bool isSMAOn()
        {
            return mChartSMA != null;
        }

        bool[] mIsMAOn = { false, false, false, false };

        void saveOption()
        {
            xDataOutput o = new xDataOutput(1024);
            o.writeInt(1);
            o.writeBoolean(mIsMAOn[0]);
            o.writeBoolean(mIsMAOn[1]);

            xFileManager.saveFile(o, "mainchart");
        }

        public void loadOption()
        {
            xDataInput di = xFileManager.readFile("mainchart", false);
            if (di != null)
            {
                if (di.readInt() == 1)
                {
                    mIsMAOn[0] = di.readBoolean();
                    mIsMAOn[1] = di.readBoolean();
                }
            }

            if (mIsMAOn[0])
                showSMA(0);
            else if (mIsMAOn[1])
                showSMA(1);
        }

        public void toggleSMA(int smaIdx)
        {
            if (mChartSMA != null)
            {
                mChartSMA = null;
                mIsMAOn[0] = mIsMAOn[1] = false;
            }
            else
            {
                mChartSMA = new ChartLine(mFont);
                mChartSMA.setSize(this);
                mChartSMA.setChartType(CHART_SMA);
                mChartSMA.setSMAIdx(smaIdx);

                mChartSMA.setShare(getShare());
            }

            mIsMAOn[smaIdx] = mChartSMA != null;
            if (mIsMAOn[smaIdx] == true)
            {
                mIsMAOn[1 - smaIdx] = false;
            }
            saveOption();

            invalidate();
        }

        public void showSMA(int smaIDX, int period)
        {
            if (mChartSMA == null)
            {
                mChartSMA = new ChartLine(mFont);
                mChartSMA.setSize(this);
                mChartSMA.setChartType(CHART_SMA);
            }
            mChartSMA.setSMAIdx(smaIDX);
            mChartSMA.mSMAPeriod = period;
            mChartSMA.clearModifyKey();

            if (smaIDX == 3){
                mIsMAOn[1] = true;
                mIsMAOn[0] = false;
            }
            else{
                mIsMAOn[smaIDX] = true;
                mIsMAOn[1 - smaIDX] = false;
                saveOption();
            }

            invalidate();
        }

        public void showSMA(int smaIDX)
        {
            if (mChartSMA == null)
            {
                mChartSMA = new ChartLine(mFont);
                mChartSMA.setSize(this);
                mChartSMA.setChartType(CHART_SMA);
            }
            mChartSMA.setShare(getShare());
            mChartSMA.setSMAIdx(smaIDX);
            mChartSMA.mSMAPeriod = 0;
            mChartSMA.clearModifyKey();

            mIsMAOn[smaIDX] = true;
            mIsMAOn[1 - smaIDX] = false;
            saveOption();

            invalidate();
        }

        public void hideSMA(int smaIDX)
        {
            if (mChartSMA == null)
                return;
            if (mChartSMA.getSMAIdx() == smaIDX || smaIDX == -1)
            {
                mChartSMA = null;
                invalidate();
            }
        }

        public override void onKeyDown(int key)
        {
            base.onKeyDown(key);
        }

        override public void onKeyPress(int key)
        {
            Console.WriteLine("=============Key: " + key);
            //  key delete
            Share share = getShare();
            if (share == null)
                return;

            if (mHasFibonacci && mDrawer != null)
            {
                if (mDrawer.onKeyUp(key))
                    invalidate();
            }
        }

        public override void setSize(int w, int h)
        {
            base.setSize(w, h);
            
            if (mAttachedCharts == null)
                return;

            for (int i = 0; i < mAttachedCharts.size(); i++)
            {
                ChartBase c = (ChartBase)mAttachedCharts.elementAt(i);
                c.setSize(this);
            }

            if (mChartSMA != null)
            {
                mChartSMA.setSize(this);
            }
        }

        public void setCursortType(int type)
        {
            mCursorType = type;
        }

        virtual public void renderCursorCrossHair(xGraphics g)
        {
            g.setColor(C.COLOR_FADE_YELLOW0);
            Share share = getShare();
            //g.drawVerticalLine(mMouseX, 0, getH());
            //  vertical line
            int sel = share.getCursor();
            int x = candleToX(sel);
            g.drawLine(x, 0, x, getH());
            //  horizon line
            g.drawHorizontalLine(0, mMouseY, getW());

            if (share != null && mMouseX != 0)
            {
                //  info
                StringBuilder sb = Utils.sb;

                float v = yToPrice(mMouseY);
                sb.Length = 0;

                int c = (int)xToCandleIdx(mMouseX);
                if (c < share.mBeginIdx || c > share.mEndIdx)
                    return;
                int date = share.getDate(c);
                //  date
                int sw = 0;
                sb.AppendFormat("{0:D2}/{1:D2}/{2:D4}",
                    Utils.EXTRACT_DAY(date), 
                    Utils.EXTRACT_MONTH(date),
                    Utils.EXTRACT_YEAR(date));

                sw = Utils.getStringW(sb.ToString(), mContext.getFontSmall());
                int y = 0;
                if (mOnMouseDown && !mSelecting)
                    y = mMouseY - 2*mContext.getFontSmall().Height - 3;
                g.setColor(0xa0000000);
                g.fillRect(mMouseX, y, sw, mContext.getFontSmall().Height);
                g.setColor(C.COLOR_ORANGE);
                g.drawString(mContext.getFontSmall(), sb.ToString(), mMouseX+2, y);

                sb.Length = 0;
                sb.AppendFormat("close: {0:F1}", v);
                g.setColor(0x80000000);
                sw = Utils.getStringW(sb.ToString(), mContext.getFontSmall());
                x = getW() - 10 - sw;
                int flag = xGraphics.LEFT;
                if (mOnMouseDown && !mSelecting)
                {
                    x = mMouseX;
                    if (x > getW() - 80) x = getW() - 80;
                    flag = xGraphics.LEFT;
                }
                g.fillRect(x, mMouseY - 15, sw, mContext.getFontSmall().Height);
                g.setColor(C.COLOR_ORANGE);
                g.drawString(mContext.getFontSmall(), sb.ToString(), x, mMouseY - 15, flag);
            }
        }

        virtual public void renderSelection(xGraphics g)
        {
            if (mSelecting && mSelectionBX >= 0 && mSelectionEX >= 0)
            {
                int min = mSelectionBX < mSelectionEX ? mSelectionBX : mSelectionEX;
                int max = mSelectionBX > mSelectionEX ? mSelectionBX : mSelectionEX;

                g.setColor(0x60803000);
                g.fillRect(min, 0, max - min, getH());

                //  info
                int bc = (int)xToCandleIdx(min);
                int ec = (int)xToCandleIdx(max);
                Share share = getShare();
                if (share != null)
                {
                    if (ec > share.mEndIdx)
                        ec = share.mEndIdx;
                    float bp = share.getClose(bc);
                    int bd = share.getDate(bc);

                    float ep = share.getClose(ec);
                    int ed = share.getDate(ec);

                    float delta = ep - bp;
                    float percent = 0;
                    if (bp != 0)
                        percent = delta * 100 / bp;

                    //  25/5 - 16/9
                    //  price: +20.4 (10.1%)
                    //  vol: 45.32 tr

                    StringBuilder sb = Utils.sb;
                    sb.Length = 0;
                    Font f = mContext.getFontSmall();
                    int x = mSelectionEX;
                    int y = mMouseY - 50;
                    if (x + 160 > getW()) x = getW() - 160;

                    g.setColor(C.COLOR_ORANGE);
                    //  date
                    sb.AppendFormat("ngày: {0:D2}/{1} - {2:D2}/{3} ({4})", 
                        Utils.EXTRACT_DAY(bd), Utils.EXTRACT_MONTH(bd),
                        Utils.EXTRACT_DAY(ed), Utils.EXTRACT_MONTH(ed),
                        ec-bc);
                    g.drawString(f, sb.ToString(), x, y);

                    sb.Length = 0;
                    //  price
                    if (delta > 0)
                        sb.AppendFormat("price: +{0:F1} ({1:F1}%)", delta, percent);
                    else
                        sb.AppendFormat("price: {0:F1} ({1:F1}%)", delta, percent);

                    y += 15;
                    g.drawString(f, sb.ToString(), x, y);
                    //  vol
                    int totalVol = 0;
                    for (int i = bc; i <= ec; i++)
                    {
                        totalVol += share.getVolume(i);
                    }
                    String s = Utils.formatNumber(totalVol);
                    sb.Length = 0;
                    sb.AppendFormat("vol: {0}", s);
                    y += 15;
                    g.drawString(f, sb.ToString(), x, y);
                }
            }
        }

        //  for master chart only
        public void flush(xDataOutput o)
        {
            o.writeInt(getW());
            o.writeInt(getH());

            o.writeInt(mChartType);

            o.writeInt(mAttachedCharts.size());
            for (int i = 0; i < mAttachedCharts.size(); i++)
            {
                ChartBase c = (ChartBase)mAttachedCharts.elementAt(i);
                o.writeInt(c.getChartType());
            }
        }

        public void load(xDataInput di)
        {
            if (di == null || di.size() < 16)
                return;
            int w = di.readInt();
            int h = di.readInt();
            setSize(getW(), h);

            mChartType = di.readInt();

            int attached = di.readInt();
            mAttachedCharts.removeAllElements();

            for (int i = 0; i < attached; i++)
            {
                int chart = di.readInt();
                if (chart == ChartBase.CHART_COMPARING_SECOND_SHARE)
                {
                    continue;
                }
                showAttachChart(chart);
            }

            loadOption();
        }

        public void setIsMasterChart(bool master)
        {
            mIsMasterChart = master;
            //if (master)
                //mDrawer = Drawer.getInstance();
            //else
                //mDrawer = null;
        }

        public override void setChartType(int type)
        {
            mCurrentShare = null;
            base.setChartType(type);
        }

        public bool isAttachedOn(int chart)
        {
            for (int i = 0; i < mAttachedCharts.size(); i++)
            {
                ChartBase c = (ChartBase)mAttachedCharts.elementAt(i);
                if (c.getChartType() == chart)
                {
                    return true;
                }
            }
            return false;
        }

        public void setDrawer(Drawer drawer)
        {
            mDrawer = drawer;
        }

        public Drawer getDrawer()
        {
            return mDrawer;
        }
        override public void setShare(Share share)
        {
            mShare = share;

            if (mDrawer != null)
            {
                mDrawer.initFibonaccie(this, mFont, mShare);
            }
        }
    }
}
