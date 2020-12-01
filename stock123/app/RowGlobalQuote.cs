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
    public class RowGlobalQuote: xListViewItem
    {
        public stGlobalQuote mQuote;
        Context mContext;

        public const int P_NAME = 0;
        public const int P_CODE = 1;
        public const int P_PRICE = 2;
        public const int P_CHANGE = 3;
        public const int P_CHANGE_PERCENT = 4;
        //public const int P_VOLUME = 5;
        public const int TOTAL_COLUMES = 5;

        public RowGlobalQuote(xIEventListener listener, stGlobalQuote quote)
            : base(listener, TOTAL_COLUMES)
        {
            mContext = Context.getInstance();
            mQuote = quote;

            uint b = C.COLOR_BLACK;
            uint g = C.COLOR_GRAY_DARK;

            uint[] bg = {b, g, g, b, b, g};
            Font f = mContext.getFontText();
            Font fb = mContext.getFontTextB();

            Font[] fs = { f, fb, fb, f, f, f};

            for (int i = 0; i < bg.Length; i++)
            {
                setBackgroundColorForCell(i, bg[i]);
                setTextFont(i, fs[i]);
            }
        }

        static public RowGlobalQuote createRowQuoteList(stGlobalQuote quote, xIEventListener listener)
        {
            RowGlobalQuote row = new RowGlobalQuote(listener, quote);

            return row;
        }

        public override void invalidate()
        {
            base.invalidate();

            update();
        }

        public void update()
        {
            if (mQuote != null)
            {    
                setImageIndex(0);

                //  name
                setTextForCell(P_NAME, mQuote.name, C.COLOR_WHITE);
                //  code
                setTextForCell(P_CODE, mQuote.symbol, C.COLOR_WHITE);
                //  point
                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                sb.AppendFormat("{0:F2}", mQuote.point);
                uint color = C.COLOR_YELLOW;
                if (mQuote.change < 0)
                    color = C.COLOR_RED;
                else if (mQuote.change > 0)
                    color = C.COLOR_GREEN;

                setTextForCell(P_PRICE, sb.ToString(), color);
                //  change
                sb.Length = 0;
                sb.AppendFormat("{0:F2}", mQuote.change);
                setTextForCell(P_CHANGE, sb.ToString(), color);

                //  change percent
                sb.Length = 0;
                sb.AppendFormat("{0:F2} %", mQuote.changePercent);
                setTextForCell(P_CHANGE_PERCENT, sb.ToString(), color);
            }
        }
    }
}
