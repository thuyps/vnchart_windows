using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.framework;
using xlib.ui;
using xlib.utils;
using stock123.app.chart;
using stock123.app.data;

namespace stock123.app
{
    public class MiniIndex: xBaseControl
    {
        int mMarketID;
        Context mContext;
        Font mFont;
        Image mArrows;
        ImageList mIcons;
        public MiniIndex(int marketID)
            : base(null)
        {
            makeCustomRender(true);

            mMarketID = marketID;
            mContext = Context.getInstance();
            mFont = mContext.getFontTextB();
            mArrows = mContext.getImage(C.IMG_SMALL_ARROWS);

            mIcons = mContext.getImageList(C.IMG_MARKET_ICONS_MINI, 16, 19);

            setSize(330, 20);
        }

        public override void render(xGraphics g)
        {
            if (isFocus())
                g.setColor(0xff306AC5);
            else
                g.setColor(C.COLOR_BLACK);
            g.clear();
            g.setColor(C.COLOR_GRAY_LIGHT);
            g.drawLine(0, 0, getW(), 0);
            g.setColor(C.COLOR_GRAY_LIGHT);
            g.drawLine(0, getH()-2, getW(), getH()-2);
            stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(mMarketID);
            if (pi == null || pi.current_point == 0)
                return;

            //===========================================
            //  VNIndex 375.3 (-1.86, -0.49%)   KL: 37,716,548 GT: 560.9 ty     up:84 no:53 down:169
            //===========================================
            int x = 0;
            int y = 0;
            uint color;
            /*
            g.setColor(C.COLOR_WHITE);
            g.drawString(mFont, pi.code, x, y);
             */
            int iconIdx = mMarketID == 1 ? 0 : 1;
            g.drawImage(mIcons.Images[iconIdx], 0, 1);

            x = mIcons.ImageSize.Width + 1;

            if (pi.changed_point > 0) color = C.COLOR_GREEN;
            else if (pi.changed_point < 0) color = C.COLOR_RED;
            else color = C.COLOR_YELLOW;
            g.setColor(color);

            String s;
            s = String.Format("{0:F2}", pi.current_point);
            //  point
            g.drawString(mFont, s, x, y);
            x = 77;
            //  change
            String s1 = String.Format("{0:F2}", pi.changed_point);
            float changedPercent = (pi.changed_point * 100) / pi.current_point;
            String s2 = String.Format("{0:F2}", changedPercent);
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            if (pi.changed_point >= 0)
                sb.AppendFormat("(+{0}, {1})", s1, s2);
            else sb.AppendFormat("(-{0}, {1})", s1, s2);
            s = sb.ToString();
            g.drawString(mFont, s, x, y);
            x = 160;
            //  KL
            g.setColor(C.COLOR_WHITE);
            s = Utils.formatNumber((int)pi.total_volume);
            sb.Length = 0;
            sb.AppendFormat("KL:{0:F1} tr", (double)(pi.total_volume/1000000));
            g.drawString(mFont, sb.ToString(), x, y);

            x = 245;
            //  GT
            sb.Length = 0;
            sb.AppendFormat("GT:{0:F1} tỷ", (double)pi.totalGTGD/1000);
            g.drawString(mFont, sb.ToString(), x, y);
            /*
            x = 520;

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
            g.drawString(mFont, "" + pi.floor_cnt, x, y);
            x += 40;
            //  down
            g.drawImage(mArrows, x, y, frmW, frmH, frmW, 0);
            x += frmW + 2;
            g.setColor(C.COLOR_RED);
            g.drawString(mFont, "" + pi.dec_cnt, x, y);
             */
        }

        public override void onMouseDown(int x, int y)
        {
            base.onMouseDown(x, y);

            //setFocus();

            //invalidate();

            //mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, getID(), null);
        }

        public override void onMouseUp(int x, int y)
        {
            base.onMouseUp(x, y);

            //invalidate();
        }

        public override void onLostFocus()
        {
            //invalidate();
        }

        public int getMarketID()
        {
            return mMarketID;
        }
    }
}
