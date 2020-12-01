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
    public class RowQuoteList: xListViewItem
    {
        public Share mShare;
        Context mContext;

        public const int P_CODE = 0;
        public const int P_PRICE = 1;
        public const int P_COMPARE = 2;
        public const int TOTAL_COLUMES = 3;

        public RowQuoteList(xIEventListener listener, Share share)
            : base(listener, TOTAL_COLUMES)
        {
            mContext = Context.getInstance();
            mShare = share;

            uint b = C.COLOR_BLACK;
            uint g = C.COLOR_GRAY_DARK;

            uint[] bg = {b, g, b};
            Font f = mContext.getFontText();
            Font fb = mContext.getFontTextB();

            if (mShare.isIndex())
            {
                bg[0] = 0xff804000;
                bg[1] = 0xff804000;
                bg[2] = 0xff804000;
            }

            Font[] fs = { fb, fb, f};

            for (int i = 0; i < bg.Length; i++)
            {
                setBackgroundColorForCell(i, bg[i]);
                setTextFont(i, fs[i]);
            }

        }

        static public RowQuoteList createRowQuoteList(Share share, xIEventListener listener)
        {
            RowQuoteList row = new RowQuoteList(listener, share);

            return row;
        }

        public override void invalidate()
        {
            base.invalidate();

            update();
        }

        public void update()
        {
            if (mShare.isIndex())
            {
                if (mShare.mMarketID == 1)
                {
                    setImageIndex(0);
                }
                else if (mShare.mMarketID == 2)
                {
                    setImageIndex(1);
                }
                //  truncate the code
                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                for (int i = 0; i < mShare.mCode.Length; i++)
                {
                    if (mShare.mCode[i] == '^')
                        continue;
                    sb.Append(mShare.mCode[i]);
                    //if (sb.Length > 5)
                        //break;
                }
                setTextFont(P_CODE, mContext.getFontSmall());
                setTextForCell(P_CODE, sb.ToString(), C.COLOR_WHITE);

                stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(mShare.mMarketID);
                //  khop
                uint color = C.COLOR_YELLOW;
                if (pi.changed_point > 0) color = C.COLOR_GREEN;
                else color = C.COLOR_RED;

                setTextForCell(P_PRICE, String.Format("{0:F2}", pi.current_point), color);

                Utils.sb.Length = 0;
                if (pi.changed_point > 0)
                    Utils.sb.AppendFormat("+{0:F2}", (float)(pi.changed_point/100));
                else
                    Utils.sb.AppendFormat("{0:F2}", ((float)pi.changed_point/100));
                //  change
                setTextForCell(P_COMPARE, Utils.sb.ToString(), color);
            }
            else
            {
                stPriceboardState ps = mContext.mPriceboard.getPriceboard(mShare.mID);
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

                    if (price == 0)
                    {
                        price = ps.getRef();
                    }

                    //  khop
                    uint color = mContext.valToColorF(price, ce, reference, floor);
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat("{0:F2}", (float)price);
                    setTextForCell(P_PRICE, Utils.sb.ToString(), color);

                    //  extra info
                    if (mShare.mCompareText == null)
                    {
                        Utils.sb.Length = 0;
                        Utils.sb.AppendFormat("{0:F2}", (float)ps.getChange());
                        mShare.mCompareText = Utils.sb.ToString();
                    }

                    if (mShare.mCompareText != null)
                    {
                        setTextForCell(P_COMPARE, mShare.mCompareText, C.COLOR_WHITE);
                    }
                }
            }
        }
    }
}
