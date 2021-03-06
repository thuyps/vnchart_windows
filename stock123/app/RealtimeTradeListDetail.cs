using System;
using System.Collections.Generic;
using System.Text;

using stock123.app;
using stock123.app.rowlist;
using stock123.app.data;
using stock123.app.chart;
using xlib.framework;
using xlib.ui;


namespace stock123.app
{
    public class RealtimeTradeListDetail
    {
        xListView mList;
        TradeHistory mTrade;
        int mLastTime;
        public RealtimeTradeListDetail(TradeHistory trade, int w, int h)
        {
            mTrade = trade;
            float[] columnPercents = { 25, 35, 40};
            String[] columnTexts = {"Giờ", "Giá", "Khối lượng"};
            xListView l = xListView.createListView(null, columnTexts, columnPercents, w, h, Context.getInstance().getImageList(C.IMG_BLANK_ROW_ICON, 1, 21), false);
            l.setID(-1);
            l.setBackgroundColor(C.COLOR_GRAY);
            mList = l;

            for (int i = trade.getTransactionCount() - 1; i >= 0; i--)
            {
                RowOnlineTrade r = RowOnlineTrade.createRowQuoteList(trade, i, null);
                l.addRow(r);
            }

            mLastTime = trade.getLastTime();
        }

        public xListView getListCtrl()
        {
            return mList;
        }

        public void updateList()
        {
            if (mTrade == null)
                return;

            for (int i = 0; i < mTrade.getTransactionCount(); i++ )
            {
                int time = mTrade.getTime(i);
                if (time > mLastTime)
                {
                    RowOnlineTrade r = RowOnlineTrade.createRowQuoteList(mTrade, i, null);
                    mList.addRowAtTop(r);

                    mLastTime = time;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
