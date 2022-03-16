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
    public class ChartMasterSimple: ChartMaster
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

        public ChartMasterSimple(Font f):base(f)
        {
            mChartType = CHART_COMPARING_SECOND_SHARE;
            mVolume = "";
            mOpen = "";
            mClose = "";
            mAllowRepositionCursor = false;
            mLineThink = 1.0f;

            mCursorType = CURSOR_CROSSHAIR;
            mShare = new Share(Share.MAX_CANDLE_CHART_COUNT);

            //CHART_BORDER_SPACING_Y_MASTER = 20;
            //CHART_BORDER_SPACING_Y = CHART_BORDER_SPACING_Y_MASTER;
        }

        String _code;
        public void setShareCode(String code)
        {
            _code = code;
            setShare(null);
        }

        override public Share getShare()
        {
            return mShare;
        }

        override public void setShare(Share share)
        {
            //mShare = new Share(Share.MAX_CANDLE_CHART_COUNT);
            mShare.setCode(_code, 1);
            mShare.loadShareFromFile(true);
        }


        override public void render(xGraphics g)
        {
            if (refChart() == null || refChart().getShare() == null)
            {
                return;
            }
            Share refShare = refChart().getShare();
            Share share = getShare();

            if (share == null || share.mCode == null || share.mCode.Length == 0 || share.getCandleCount() < 3)
            {
                return;
            }

            //==================
            if (refShare.mCandleType != share.mCandleType)
            {
                share.mCandleType = refShare.mCandleType;
                if (share.mCandleType == Share.CANDLE_WEEKLY)
                {
                    share.toWeekly();
                }
                else if (share.mCandleType == Share.CANDLE_MONTHLY)
                {
                    share.toMonthly();
                }
                else
                {
                    share.loadShareFromFile(true);
                }
            }
            //=======================

            int beginDate = refShare.getDate(refShare.mBeginIdx);
            int endDate = refShare.getDate(refShare.mEndIdx);

            int beginIdx = share.dateToIndex(beginDate);
            int endIdx = share.dateToIndex(endDate);

            int date = refShare.getDate();
            int selCandleIdx = share.dateToIndex(date);
            if (selCandleIdx >= 0)
            {
                share.selectCandle(selCandleIdx);
            }

            //share.setCursorScope(refShare.getCursorScope());



            share.setStartEndIndex(beginIdx, endIdx);

            mChartType = refChart().getChartType();
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

            mShouldDrawTitle = true;
            mShouldDrawCursor = true;
 
            getTitles();
            renderCursor(g);
        }

        override public int renderTitles(xGraphics g, int x, int y)
        {
            return base.renderTitles(g, 80, y);
        }

        override public void renderOverlayAttachedCharts(xGraphics g)
        {
            
        }

        override public void renderAttachedCharts(xGraphics g)
        {
        }

        override public String getTitle()
        {
            return "";
        }

        override public xVector getTitles()
        {
            xVector v = new xVector();

            Share share = getShare();
            if (share == null){
                return v;
            }
            if (refChart() == null || refChart().getShare() == null)
            {
                return v;
            }
            Share refShare = refChart().getShare();
            int date = refShare.getDate();
            int selCandleIdx = share.dateToIndex(date);
            if (selCandleIdx < 0)
            {
                return v;
            }
            String inf = "";

            uint color = 0;

            if (selCandleIdx >= share.mBeginIdx && selCandleIdx <= share.mEndIdx)
            {
                float changed = 0;
                if (selCandleIdx > 0 && share.getClose(selCandleIdx - 1) > 0)
                {
                    changed = share.getClose(selCandleIdx) - share.getClose(selCandleIdx - 1);
                }
                if (changed >= 0)
                {
                    color = C.COLOR_GREEN;
                    inf = String.Format("{0:F1}(+{1:F1})",
                        share.getClose(selCandleIdx),
                        changed);
                }
                else
                {
                    color = C.COLOR_RED;
                    inf = String.Format("{0:F1}({1:F1})",
                           share.getClose(selCandleIdx),
                           changed);
                }
            }

            //String s1 = String.Format("     ");
            String s2 = inf;
            //v.addElement(new stTitle(s1, themeDark() ? C.COLOR_WHITE : C.COLOR_BLACK));
            v.addElement(new stTitle(s2, color));

            return v;

        }

        override protected void renderMasterCursor(xGraphics g, float cx, float cy)
        {
            
        }

        /*
        override public void renderCursorCrossHair(xGraphics g)
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
        */
        override public void renderSelection(xGraphics g)
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

    }
}
