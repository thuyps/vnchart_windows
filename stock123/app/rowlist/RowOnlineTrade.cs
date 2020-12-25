using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using stock123.app.data;
using xlib.framework;
using xlib.ui;
using xlib.utils;

namespace stock123.app.rowlist
{
    public class RowOnlineTrade: xListViewItem
    {
        public TradeHistory mTrade;
        int mTradeIndex;
        Context mContext;

        public const int P_TIME = 0;
        public const int P_PRICE = 1;
        public const int P_VOLUME = 2;
        public const int TOTAL_COLUMES = 3;

        public RowOnlineTrade(xIEventListener listener, TradeHistory trade, int idx)
            : base(listener, TOTAL_COLUMES)
        {
            mContext = Context.getInstance();
            mTrade = trade;
            mTradeIndex = idx;

            uint b = C.COLOR_BLACK;
            uint g = C.COLOR_GRAY_DARK;

            uint[] bg = {g, b, b};
            Font f = mContext.getFontText();
            Font fb = mContext.getFontTextB();

            Font[] fs = { f, fb, fb};

            for (int i = 0; i < bg.Length; i++)
            {
                setBackgroundColorForCell(i, bg[i]);
                setTextFont(i, fs[i]);
            }
            //====================
            update();
        }

        static public RowOnlineTrade createRowQuoteList(TradeHistory trade, int idx, xIEventListener listener)
        {
            RowOnlineTrade row = new RowOnlineTrade(listener, trade, idx);

            return row;
        }

        public override void invalidate()
        {
            base.invalidate();

            update();
        }

        void update()
        {
            if (mTrade == null)
                return;
            if (mTradeIndex >= 0 && mTradeIndex < mTrade.getTransactionCount())
            {
                int time = mTrade.getTime(mTradeIndex);
                float price = mTrade.getPrice(mTradeIndex);
                int vol = mTrade.getTradeVolume(mTradeIndex);

                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                //  time
                sb.AppendFormat("{0:D2}:{1:D2}:{2:D2}", ((time>>16)&0xff), ((time>>8)&0xff), (time&0xff));
                setTextForCell(P_TIME, sb.ToString(), C.COLOR_WHITE);
                
                //  price
                stPriceboardState ps = mContext.mPriceboard.getPriceboard(mTrade.mShareID);
                if (ps != null)
                {
                    uint color = mContext.valToColorF(price, ps.getCe(), ps.getRef(), ps.getFloor());
                    sb.Length = 0;
                    sb.AppendFormat("{0:F2}", (float)price);
                    setTextForCell(P_PRICE, sb.ToString(), color);
                    //  volume
                    string sv = Utils.formatNumber(vol);
                    setTextForCell(P_VOLUME, sv, color);
                }
            }
        }
    }
}
