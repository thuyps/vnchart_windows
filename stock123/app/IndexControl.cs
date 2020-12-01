using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.framework;
using xlib.utils;
using xlib.ui;
using stock123.app.data;
using stock123.app.chart;
using System.Windows.Forms;

namespace stock123.app
{
    public class IndexControl: xContainer, xIEventListener
    {
        public const int ID_ONLINE_CHART = 1;
        public const int ID_HIS_CHART = 2;
        public const int ID_MONEY_CHART = 3;
        public const int ID_VOLUMN_CHART = 4;
        public const int ID_CANDLE = 5;
        public const int ID_INDEX_EFFECT_INC = 6;
        public const int ID_INDEX_EFFECT_DEC = 7;

        int[] TAB_INDEX = { ID_ONLINE_CHART, ID_CANDLE, ID_MONEY_CHART, ID_VOLUMN_CHART, ID_INDEX_EFFECT_INC, ID_INDEX_EFFECT_DEC};
        String[] TAB_TITLE = { "Đồ thị phiên", "Summary", "Dòng tiền", "Trọng số", "Tăng index", "Giảm index" };

        int mMarketID;
        Context mContext;
        xBaseControl mCurrentChart;
        RealtimeChart mRealtimeChart;

        xTabControl mTab;
        public IndexControl(xIEventListener listener, int marketID, int w, int h)
            : base(listener)
        {
            mMarketID = marketID;
            mContext = Context.getInstance();
            setSize(w, h);

            //setBackgroundColor(0xffff0000);

            mTab = new xTabControl();
            addControl(mTab);

            mTab.setSize(w, h);

            TabControl tc = (TabControl)mTab.getControl();
            tc.Selected += new TabControlEventHandler(tabControlSelected);

            int y = 0;
            for (int i = 0; i < TAB_TITLE.Length; i++)
            {
                xTabPage page = new xTabPage(TAB_TITLE[i]);
                mTab.addPage(page);

                if (i == 0)
                {
                    stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(marketID);
                    TradeHistory trade = mContext.getTradeHistory(pi.id);

                    //  realtime
                    RealtimeChart rc = new RealtimeChart(trade, this);
                    h = getH() - y;
                    rc.setPosition(0, y);
                    rc.setSize(w, h);
                    page.addControl(rc);

                    mRealtimeChart = rc;
                    mCurrentChart = mRealtimeChart;
                }
            }

            int currentTab = mContext.getMarketControlTab(mMarketID);
            if (currentTab < 0 || currentTab >= TAB_INDEX.Length)
                currentTab = 0;
            if (currentTab != -1)
            {
                ((TabControl)mTab.getControl()).SelectedIndex = currentTab;

                onPageSelected(currentTab);
            }

            /*
            //  Do thi phien

            int[] ids = {ID_ONLINE_CHART, ID_MONEY_CHART, ID_VOLUMN_CHART, ID_HIS_CHART};
            int x = 0;
            int y = 0;
            int bw = (w / 4) - 2;
            for (int i = 0; i < text.Length; i++)
            {
                bt = xButton.createStandardButton(ids[i], this, text[i], bw);
                bt.setPosition(x, 0);

                addControl(bt);
                x = bt.getRight() + 2;
                y = bt.getBottom() + 4;
            }
            */
        }

        override public void invalidate()
        {
            mCurrentChart.invalidate();
        }

        virtual public void onEvent(object sender, int evt, int aIntParameter, object aParameter)
        {
            mListener.onEvent(sender, evt, aIntParameter, aParameter);
            /*
            if (evt == xBaseControl.EVT_BUTTON_CLICKED)
            {
                if (aIntParameter == ID_ONLINE_CHART)
                {
                    if (mCurrentChart != mRealtimeChart)
                    {
                        removeControl(mCurrentChart);
                        addControl(mRealtimeChart);
                        mCurrentChart = mRealtimeChart;
                    }
                    mCurrentChart.invalidate();
                }
                if (aIntParameter == ID_HIS_CHART)
                {
                    mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, C.ID_HISTORY_CHART, (Int32)mMarketID);
                }
                if ((aIntParameter == ID_MONEY_CHART) || (aIntParameter == ID_VOLUMN_CHART))
                {
                    if (mBubbleChart == null)
                    {
                        mBubbleChart = new ChartStatistics(mMarketID);
                        mBubbleChart.setSize(mRealtimeChart);
                        mBubbleChart.setPosition(mRealtimeChart.getX(), mRealtimeChart.getY());
                    }
                    bool flag = false;
                    if (mCurrentChart == mBubbleChart)
                    {
                        mCurrentChart = null;
                        flag = true;
                    }
                    if (mCurrentChart != mBubbleChart || flag)
                    {
                        removeControl(mCurrentChart);
                        addControl(mBubbleChart);
                        mCurrentChart = mBubbleChart;
                        if (aIntParameter == ID_MONEY_CHART)
                            mBubbleChart.setChartType(ChartStatistics.BUBBLE_TRADE_MONEY);
                        else
                            mBubbleChart.setChartType(ChartStatistics.BUBBLE_VOLUME_WEIGHT);
                    }

                    mCurrentChart.invalidate();
                }
            }
             */
        }

        void tabControlSelected(object sender, TabControlEventArgs e)
        {
            onPageSelected(e.TabPageIndex);
        }

        void onPageSelected(int pageIdx)
        {
            mContext.setMarketControlTab(mMarketID, pageIdx);

            int idx = TAB_INDEX[pageIdx];
            xTabPage page = mTab.getPageAtIndex(pageIdx);
            if (idx == ID_ONLINE_CHART)
            {
                mRealtimeChart.invalidate();
            }
            else if (idx == ID_CANDLE)
            {
                xContainer c = new xContainer();
                c.setSize(mCurrentChart);

                TodayCandle candle = new TodayCandle();
                stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(mMarketID);
                Share share = mContext.mShareManager.getShare(pi.code);
                candle.setShare(share);

                int w = mCurrentChart.getW();
                int h = mCurrentChart.getH();
                int cw = w*3/7;
                if (cw > 160)
                    cw = 160;
                candle.setSize(cw, h);

                c.addControl(candle);

                //  summary
                xBaseControl summary = createMarketSummary(w - cw, h);
                summary.setPosition(cw, 0);
                c.addControl(summary);

                mCurrentChart = c;
                //==============================
                page.resetContent();
                page.addControl(c);
            }
            else if (idx == ID_MONEY_CHART)
            {
                page.resetContent();

                ChartStatistics moneyChart = new ChartStatistics(mMarketID, mListener);
                moneyChart.setSize(mRealtimeChart.getW(), mRealtimeChart.getH() - 18);
                moneyChart.setPosition(mRealtimeChart.getX(), mRealtimeChart.getY());
                moneyChart.setChartType(ChartStatistics.BUBBLE_TRADE_MONEY);

                moneyChart.doCalcChanged();

                page.addControl(moneyChart);

                mCurrentChart = moneyChart;
            }
            else if (idx == ID_VOLUMN_CHART)
            {
                page.resetContent();

                ChartStatistics moneyChart = new ChartStatistics(mMarketID, mListener);
                moneyChart.setSize(mRealtimeChart.getW(), mRealtimeChart.getH() - 18);
                moneyChart.setPosition(mRealtimeChart.getX(), mRealtimeChart.getY());
                moneyChart.setChartType(ChartStatistics.BUBBLE_VOLUME_WEIGHT);

                moneyChart.doCalcChanged();

                page.addControl(moneyChart);

                mCurrentChart = moneyChart;
            }
            else if (idx == ID_INDEX_EFFECT_INC)
            {
                page.resetContent();

                ChartStatistics moneyChart = new ChartStatistics(mMarketID, mListener);
                moneyChart.setSize(mRealtimeChart.getW(), mRealtimeChart.getH() - 18);
                moneyChart.setPosition(mRealtimeChart.getX(), mRealtimeChart.getY());
                moneyChart.setChartType(ChartStatistics.BUBBLE_INDEX_WEIGHT_RATIO_INC);

                moneyChart.doCalcChanged();

                page.addControl(moneyChart);

                mCurrentChart = moneyChart;
            }
            else if (idx == ID_INDEX_EFFECT_DEC)
            {
                page.resetContent();

                ChartStatistics moneyChart = new ChartStatistics(mMarketID, mListener);
                moneyChart.setSize(mRealtimeChart.getW(), mRealtimeChart.getH() - 18);
                moneyChart.setPosition(mRealtimeChart.getX(), mRealtimeChart.getY());
                moneyChart.setChartType(ChartStatistics.BUBBLE_INDEX_WEIGHT_RATIO_DEC);

                moneyChart.doCalcChanged();

                page.addControl(moneyChart);

                mCurrentChart = moneyChart;
            }
        }

        xBaseControl createMarketSummary(int w, int h)
        {
            MarketSummary summary = new MarketSummary(mMarketID);
            summary.setSize(w, h);
            return summary;
        }
    }
}
