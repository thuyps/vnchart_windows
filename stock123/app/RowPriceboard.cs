using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using stock123.app.data;
using xlib.framework;
using xlib.ui;
using xlib.utils;

namespace stock123.app
{
    public class RowPriceboard: xListViewItem
    {
        public int mShareID;
        Context mContext;

        public const int P_CODE = 0;
        public const int P_REF = 1;

        public const int P_BUY3 = 2;
        public const int P_BUY_V3 = 3;
        public const int P_BUY2 = 4;
        public const int P_BUY_V2 = 5;
        public const int P_BUY1 = 6;
        public const int P_BUY_V1 = 7;

        public const int P_KHOP = 8;
        public const int P_KL_KHOP = 9;
        public const int P_CHANGE = 10;

        public const int P_SELL1 = 11;
        public const int P_SELL_V1 = 12;
        public const int P_SELL2 = 13;
        public const int P_SELL_V2 = 14;
        public const int P_SELL3 = 15;
        public const int P_SELL_V3 = 16;

        public const int P_HIGH = 17;
        public const int P_LOW = 18;

        public const int P_TOTAL_VOLUME = 19;

        public const int TOTAL_COLUMES = 20;

        public RowPriceboard(xIEventListener listener)
            : base(listener, TOTAL_COLUMES)
        {
            mContext = Context.getInstance();

            uint b = C.COLOR_BLACK;
            uint g = C.COLOR_GRAY_DARK;
            uint[] bg = {b, g, b, b, b, b, b, b, g, g, g, b, b, b, b, b, b, g, g, g};
            Font f = mContext.getFontText();
            Font fb = mContext.getFontTextB();
            Font sfb = mContext.getFontSmallB();

            Font[] fs = { sfb, fb, f, f, f, f, f, f, fb, fb, fb, f, f, f, f, f, f, sfb, sfb, sfb };

            for (int i = 0; i < bg.Length; i++)
            {
                setBackgroundColorForCell(i, bg[i]);
                setTextFont(i, fs[i]);
            }
        }

        static public RowPriceboard createRowPriceboard(int shareID, xIEventListener listener)
        {
            RowPriceboard row = new RowPriceboard(listener);
            row.mShareID = shareID;

            return row;
        }

        public override void invalidate()
        {
            base.invalidate();

            update();
        }

        public void update()
        {
            stPriceboardState ps = mContext.mPriceboard.getPriceboard(mShareID);
            if (ps != null)
            {
                if (ps.getMarketID() == 1)
                {
                    if (mContext.mAlarmManager.isAlarmInstalled(ps.getCode()))
                        setImageIndex(2);
                    else
                        setImageIndex(0);
                }
                else if (ps.getMarketID() == 2)
                {
                    if (mContext.mAlarmManager.isAlarmInstalled(ps.getCode()))
                        setImageIndex(3);
                    else
                        setImageIndex(1);
                }
                //  code
                setTextForCell(P_CODE, ps.getCode(), C.COLOR_WHITE);
                //setID(ps.getID());
                //  ref
                float reference = ps.getRef();
                float ce = ps.getCe();
                float floor = ps.getFloor();
                float price = ps.getCurrentPrice();
                int[] rbc = {P_BUY3, P_BUY2, P_BUY1};
                float[] rb = {ps.getRemainBuyPrice2(), ps.getRemainBuyPrice1(), ps.getRemainBuyPrice0()};
                int[] rbv = {ps.getRemainBuyVolume2(), ps.getRemainBuyVolume1(), ps.getRemainBuyVolume0()};

                int[] rsc = {P_SELL1, P_SELL2, P_SELL3};
                float[] rs = {ps.getRemainSellPrice0(), ps.getRemainSellPrice1(), ps.getRemainSellPrice2()};
                int[] rsv = {ps.getRemainSellVolume0(), ps.getRemainSellVolume1(), ps.getRemainSellVolume2()};

                setTextForCell(P_REF, String.Format("{0:F2}", reference), C.COLOR_YELLOW);
                //  buy
                uint color;
                int i = 0;
                for (i = 0; i < 3; i++)
                {
                    color = mContext.valToColorF(rb[i], ce, reference, floor);
                    setTextForCell(rbc[i], String.Format("{0:F2}", rb[i]), color);
                    if (i == 0 || i == 1 || rbv[i] > 100000)
                        setTextForCell(rbc[i]+1, Utils.formatDecimalNumber(rbv[i], 1000, 1), color);
                    else
                        setTextForCell(rbc[i] + 1, Utils.formatDecimalNumber(rbv[i], 1000, 2), color);
                }
                //  khop
                color = mContext.valToColorF(price, ce, reference, floor);
                setTextForCell(P_KHOP, String.Format("{0:F2}", price), color);
                setTextForCell(P_KL_KHOP, Utils.formatDecimalNumber(ps.getCurrentVolume(), 1000, 2), color);
                if (price == 0)
                    price = reference;
                float changed = price - reference;
                setTextForCell(P_CHANGE, String.Format("{0:F2}", changed), color);

                //  sell
                for (i = 0; i < 3; i++)
                {
                    color = mContext.valToColorF(rs[i], ce, reference, floor);
                    setTextForCell(rsc[i], String.Format("{0:F2}", rs[i]), color);
                    if (i == 1 || i == 2 || rsv[i] >= 100000)
                        setTextForCell(rsc[i] + 1, Utils.formatDecimalNumber(rsv[i], 1000, 1), color);
                    else
                        setTextForCell(rsc[i] + 1, Utils.formatDecimalNumber(rsv[i], 1000, 2), color);
                }

                //  hi
                price = ps.getMax();
                color = mContext.valToColorF(price, ce, reference, floor);
                setTextForCell(P_HIGH, String.Format("{0:F1}", price), color);
                //  low
                price = ps.getMin();
                color = mContext.valToColorF(price, ce, reference, floor);
                setTextForCell(P_LOW, String.Format("{0:F2}", price), color);
                //  total volume
                setTextForCell(P_TOTAL_VOLUME, Utils.formatDecimalNumber(ps.getTotalVolume(), 1000, 1), C.COLOR_WHITE);
            }
        }
    }
}
