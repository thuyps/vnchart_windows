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
    public class RowPriceboardGainLoss: xListViewItem
    {
        Context mContext;

        public const int P_CODE = 0;
        public const int P_REF = 1;

        public const int P_KHOP = 2;
        public const int P_KL_KHOP = 3;
        public const int P_CHANGE = 4;
        public const int P_TOTAL_VOLUME = 5;

        //====================
        public const int P_MUA = 6;
        public const int P_KL_MUA = 7;

        public const int P_VON = 8;

        public const int P_GAINLOSS_PERCENT = 9;
        public const int P_GAINLOSS_MONNEY = 10;

        public const int P_DATE = 11;

        public const int TOTAL_COLUMES = 12;

        public RowPriceboardGainLoss(xIEventListener listener)
            : base(listener, TOTAL_COLUMES)
        {
            mContext = Context.getInstance();
            /*
                {
                    //  code | ref | = GiaKhop | KLKhop | +/- | TongKL | GiaMua | KLMua | %GainLoss | $$$ | Date
                    xUIFont *font[13] = {f, f, f, f, f, f, f, f, f, f, f, f, f};        
                    float percents[13] = {7, 7, 
                        7, 10, 
                        8, 10, 8,
                        9, 10, 
                        8, 7.5, 9, -1};
                    int colors[13] = {BG_GRAY, BG_GRAY, -1, -1, -1, -1, BG_GRAY, BG_GRAY, BG_GRAY, -1, -1, BG_GRAY, -1};
                    init(w, h, percents, font, colors);
                } 
             */
            uint b = C.COLOR_BLACK;
            uint g = C.COLOR_GRAY_DARK;
            uint[] bg = {b, b, g, g, g, g, b, b, b, g, g, b};   //  12
            Font f = mContext.getFontText();
            Font fb = mContext.getFontTextB();
            Font sfb = mContext.getFontSmallB();

            Font[] fs = { fb, f, fb, fb, fb, fb, fb, fb, fb, fb, fb, fb};

            for (int i = 0; i < bg.Length; i++)
            {
                setBackgroundColorForCell(i, bg[i]);
                setTextFont(i, fs[i]);
            }
        }

        static public RowPriceboardGainLoss createRowPriceboardGainLoss(stGainloss a, xIEventListener listener)
        {
            RowPriceboardGainLoss row = new RowPriceboardGainLoss(listener);
            row.setData(a);

            return row;
        }

        public override void invalidate()
        {
            base.invalidate();

            update();
        }

        public void update()
        {
            stGainloss a = (stGainloss)getData();
            int shareID = Context.getInstance().mShareManager.getShareID(a.code);
            stPriceboardState ps = mContext.mPriceboard.getPriceboard(shareID);
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

                setTextForCell(P_REF, String.Format("{0:F2}", reference), C.COLOR_YELLOW);
                //  khop
                uint color = mContext.valToColorF(price, ce, reference, floor);
                setTextForCell(P_KHOP, String.Format("{0:F2}", price), color);
                setTextForCell(P_KL_KHOP, Utils.formatDecimalNumber(ps.getCurrentVolume(), 1000, 2), color);
                float changed = price - reference;
                float t = ps.getChange();
                setTextForCell(P_CHANGE, String.Format("{0:F2}", changed), color);

                //  total volume
                setTextForCell(P_TOTAL_VOLUME, Utils.formatDecimalNumber(ps.getTotalVolume(), 1000, 1), C.COLOR_WHITE);

                //=========GAIN-LOSS session==========
                stGainloss g = (stGainloss)getData();
                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                //  GiaMua
                sb.AppendFormat("{0:F2}", g.price);
                setTextForCell(P_MUA, sb.ToString(), C.COLOR_WHITE);
                //  KLMua
                sb.Length = 0;
                sb.AppendFormat("{0:F2}", (float)(g.volume / 1000));
                setTextForCell(P_KL_MUA, sb.ToString(), C.COLOR_WHITE);
                //  Tien von
                sb.Length = 0;
                double capital = g.price * g.volume;
                capital /= 1000;
                sb.AppendFormat("{0:F2}", capital);
                setTextForCell(P_VON, sb.ToString(), C.COLOR_WHITE);
                //  %GainLoss
                sb.Length = 0;
                price = g.price;
                float percent = 0;
                if (price > 0)
                {
                    percent = (float)(100 * (ps.getCurrentPrice() - price)) / price;
                }
                if (percent > 0)
                    sb.AppendFormat("+{0:F2}", percent);
                else
                    sb.AppendFormat("{0:F2}", percent);
                if (percent > 0)
                    color = C.COLOR_GREEN;
                else if (percent == 0)
                    color = C.COLOR_YELLOW;
                else
                    color = C.COLOR_RED;

                setTextForCell(P_GAINLOSS_PERCENT, sb.ToString(), color);
                //  gainloss money
                sb.Length = 0;
                double oValue = price * g.volume;
                double cValue = ps.getCurrentPrice() * g.volume;
                double benefit = (cValue - oValue) / 10;

                if (benefit > 0)
                    sb.AppendFormat("+{0:F2}", benefit);
                else
                    sb.AppendFormat("{0:F2}", benefit);
                setTextForCell(P_GAINLOSS_MONNEY, sb.ToString(), color);
                //  date
                string date = Utils.dateIntToString4(g.date);
                setTextForCell(P_DATE, date, C.COLOR_WHITE);
            }
        }
    }
}
