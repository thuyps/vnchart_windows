using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using xlib.utils;
using xlib.ui;
using xlib.framework;
using stock123.app.data;

namespace stock123.app
{
    public class IndexBriefLine: xBaseControl
    {
        int mMarketID;
        Context mContext;
        Font mFont;
        Image mArrows;
        public IndexBriefLine(int marketID)
            : base(null)
        {
            makeCustomRender(true);

            mMarketID = marketID;
            mContext = Context.getInstance();
            mFont = mContext.getFontTextB();
            mArrows = mContext.getImage(C.IMG_SMALL_ARROWS);
            setSize(1000, 18);
        }

        public override void render(xGraphics g)
        {
            if (isFocus())
                g.setColor(0xff306AC5);
            else
                g.setColor(C.COLOR_BLACK);
            g.clear();
            g.setColor(0xffffffff);
            g.drawLine(0, getH()-1, getW(), getH()-1);
            stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(mMarketID);
            if (pi == null/* || pi.current_point == 0*/)
                return;

            //===========================================
            //  VNIndex 375.3 (-1.86, -0.49%)   KL: 37,716,548 GT: 560.9 ty     up:84 no:53 down:169
            //===========================================
            int x = 0;
            int y = 0;
            uint color;
            g.setColor(C.COLOR_WHITE);
            g.drawString(mFont, pi.code, x, y);

            x = 75;

            if (pi.changed_point > 0) color = C.COLOR_GREEN;
            else if (pi.changed_point < 0) color = C.COLOR_RED;
            else color = C.COLOR_YELLOW;
            g.setColor(color);

            String s;
            s = String.Format("{0:F2}", pi.current_point);
            //  point
            g.drawString(mFont, s, x, y);
            x = 130;
            //  change
            String s1 = String.Format("{0:F2}", pi.changed_point);
            float changedPercent = (pi.changed_point * 100) / pi.current_point;
            String s2 = String.Format("{0:F2}%", changedPercent);
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            sb.AppendFormat("({0}, {1})", s1, s2);
            s = sb.ToString();
            g.drawString(mFont, s, x, y);
            x = 220;
            //  KL
            g.setColor(C.COLOR_WHITE);
            double t = pi.total_volume;
            t /= 1000000;
            //s =
            s = Utils.formatNumber((float)t, 2);
                //Utils.formatNumber(0, 1);
            sb.Length = 0;
            sb.AppendFormat("KLGD: {0} tr", s);
            g.drawString(mFont, sb.ToString(), x, y);

            x = 330;
            //  GT
            s = Utils.formatDecimalNumber(pi.totalGTGD, 1000, 1);
            sb.Length = 0;
            sb.AppendFormat("GTGD: {0} tỷ", s);
            g.drawString(mFont, sb.ToString(), x, y);

            x = 440;

            //  up/none/down
            int frmW = mArrows.Width/3;
            int frmH = mArrows.Height;
            g.drawImage(mArrows, x, y, frmW, frmH, 0, 0);
            x += frmW + 2;
            g.setColor(C.COLOR_GREEN);
            g.drawString(mFont, "" + pi.inc_cnt, x, y);
            x += 40;
            //  none
            g.drawImage(mArrows, x, y, frmW, frmH, 2*frmW, 0);
            x += frmW + 2;
            g.setColor(C.COLOR_YELLOW);
            g.drawString(mFont, "" + pi.ref_num, x, y);
            x += 40;
            //  down
            g.drawImage(mArrows, x, y, frmW, frmH, frmW, 0);
            x += frmW + 2;
            g.setColor(C.COLOR_RED);
            g.drawString(mFont, "" + pi.dec_cnt, x, y);
            //  date

            x += 50;

            //  cung-cau
            g.setColor(C.COLOR_WHITE);

            double[] tt = { 0, 0 };
            calcBuySellRemains(pi.marketID, tt);
            //  mua
            t = tt[0] / 1000000;
            s = Utils.formatNumber((float)t, 1);
            //  ban
            t = tt[1] / 1000000;
            s1 = Utils.formatNumber((float)t, 1);
            sb.Length = 0;
            sb.AppendFormat("Dư M/B={0} / {1}", s, s1);
            g.drawString(mFont, sb.ToString(), x, y);

            x += 150;

            g.setColor(C.COLOR_WHITE);
            g.drawString(mFont, pi.mDate, x, y);
        }

        public override void onMouseDown(int x, int y)
        {
            base.onMouseDown(x, y);

            setFocus();

            invalidate();

            mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, getID(), null);
        }

        public override void onMouseUp(int x, int y)
        {
            base.onMouseUp(x, y);

            invalidate();
        }

        public override void onMouseDoubleClick()
        {
            mListener.onEvent(this, xBaseControl.EVT_ON_ROW_DOUBLE_CLICK, getID(), null);
        }

        public override void onLostFocus()
        {
            invalidate();
        }

        public int getMarketID()
        {
            return mMarketID;
        }

        void calcBuySellRemains(int marketID, double[] buysell)
        {
            double buy = 0;
            double sell = 0;
            int total = mContext.mShareManager.getTotalShareIDCount();
            int[] mid = {0};
            for (int i = 0; i < total; i++)
            {
                mid[0] = -1;
                int shareID = mContext.mShareManager.getShareIDAt(i, mid);
                if (shareID == 257)
                {
                    shareID = 257;
                }
                if (/*mid[0] == marketID && */shareID != 0)
                {
                        stPriceboardState ps = mContext.mPriceboard.getPriceboard(shareID);
                        if (ps.getMarketID() == marketID)
                        {
                            buy += ps.getRemainBuyVolume0();
                            buy += ps.getRemainBuyVolume1();
                            buy += ps.getRemainBuyVolume2();

                            sell += ps.getRemainSellVolume0();
                            sell += ps.getRemainSellVolume1();
                            sell += ps.getRemainSellVolume2();
                        }
                }
            }
            buysell[0] = buy;
            buysell[1] = sell;
        }
    }
}
