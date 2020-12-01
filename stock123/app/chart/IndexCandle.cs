using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using xlib.framework;
using xlib.utils;
using xlib.ui;

using stock123.app;
using stock123.app.data;

namespace stock123.app.chart
{
    public class IndexCandle: xBaseControl
    {
        Share mShare;
        Context mContext;
        bool mHasTitle = false;
        bool mDrawRefLabel = true;
        //===============================
        public IndexCandle()
            :base(null)
        {
            makeCustomRender(true);
            mContext = Context.getInstance();
        }
        public void setShare(Share share) { mShare = share; }
        public void setHasTitle(bool has) { mHasTitle = has; }
        public void setDrawRefLabel(bool has) { mDrawRefLabel = has; }
        override public void render(xGraphics g)
        {
            g.setColor(C.COLOR_BLACK);
            g.fillRect(0, 0, getW(), getH());

            g.setColor(C.COLOR_GRAY_LIGHT);
            g.drawRect(0, 0, getW()-1, getH()-1);
    
            Share share = mShare;
            if (share == null)
                return ;

            int x, y;
            x = 0;
            y = 0;
            int h = getH();
            //===========================================
            drawCandle(g, x, y, getW(), h);
            //==================
        }

        void drawCandle(xGraphics g, int x, int y0, int w, int h)
        {
            Share share = mShare;

            int y = 0;
            Font f = mContext.getFontSmall();

	        uint color;    
            
            stPriceboardState ps = mContext.mPriceboard.getPriceboard(share.getID());
            if (ps == null)
                return ;
            
            TradeHistory trade = mContext.getTradeHistory(share.getID());
            float price = ps.getCurrentPrice();
            float open = price;//ps.getRef(); //  should be open - testing
            if (trade != null && trade.getTransactionCount() > 0)
                open = trade.getPrice(0);
            else
                open = mContext.mPriceboard.getOpen(mShare.mID);

            if (open != 0 && mContext.mPriceboard.getOpen(mShare.mID) == 0 && !share.isIndex())
            {
                mContext.mPriceboard.setOpen(mShare.mID, (int)open);
            }
            
            float hi = ps.getMax();
            float lo = ps.getMin();
            float priceLen = hi - lo;

            float reference = ps.getRef();
            float min = ps.getFloor() - (float)reference / 30;
            float max = ps.getCe() + (float)reference / 30;

            if (share.isIndex())
            {
                price = trade.mClose/10.0f;
                open = trade.mOpen / 10.0f;
                reference = trade.mPriceRef / 10.0f;
                hi = trade.mHighest / 10.0f;
                lo = trade.mLowest / 10.0f;
                min = reference - reference / 40;
                max = reference + reference / 40;

                if (min > lo) min = lo;
                if (max < hi) max = hi;

                priceLen = (int)(hi - lo);
            }

            if (price == 0)
                return;
            
            //if (min > ps.getMin() && ps.getMin() > 0) min = ps.getMin();
            //if (max < ps.getMax()) max = ps.getMax();
            
            float totalPrice = (max-min);  //(10%);
            if (totalPrice < priceLen)
                totalPrice = priceLen;
            
            if (totalPrice == 0)
                return ;    
            
            float ry = (float)(h)/totalPrice;    
            
            int totalH = (int)(ry*totalPrice);
            int bodyW = w/3;
            StringBuilder sb = Utils.getSB();
            //================frame=============================
            //  line ref
            g.setColor(0x30ffff00);
            y = (int)(y0 + totalH - (reference - min)*ry);
            g.drawLineDotHorizontal(1, y, getW()-2, y);
            g.setColor(0xa0ffff00);
/*
            if (mDrawRefLabel)
            {
                sb.AppendFormat("{0:F1}", (float)reference / 10);
                g.drawString(f, sb.ToString(), 1, y - f.Height / 2, 0);
            }
 */
            //  CE line
            if (!share.isIndex())
            {
                g.setColor(0x30ff00ff);
                y = (int)(y0 + totalH - (ps.getCe() - min) * ry);
                g.drawLineDotHorizontal(1, y, getW() - 2, y);
                g.setColor(0xa0ff00ff);
                sb.Length = 0;
                sb.AppendFormat("{0:F1}", (float)(ps.getCe() / 10));

                if (mDrawRefLabel)
                    g.drawString(f, sb.ToString(), 1, y, 0);

                //  FLOOR line
                g.setColor(0x3000FFFF);
                y = (int)(y0 + totalH - (ps.getFloor() - min) * ry);
                g.drawLineDotHorizontal(1, y, getW() - 2, y);
                g.setColor(0xa000FFFF);
                sb.Length = 0;
                sb.AppendFormat("{0:F1}", (float)(ps.getFloor() / 10));
                if (mDrawRefLabel)
                    g.drawString(f, sb.ToString(), 1, y - f.Height, 0);
            }
            //===================================================
            color = price < open? C.COLOR_RED:C.COLOR_GREEN;
            if (price == open)
                color = C.COLOR_WHITE;
            if (price == 0)
                return;
            //  draw shadow
            g.setColor(C.COLOR_WHITE);
            x = getW()/2;

            if (share.isIndex() && hi > 0 && lo > 0)
            {
                int minY = (int)(y0 + totalH - (lo-min)*ry);
                int maxY = (int)(y0 + totalH - (hi-min)*ry);    

                g.drawLine(x, maxY, x, minY);
            }
            int centerX = x + bodyW/2;
            //  candle's body
            int oY = (int)(y0 + totalH - (open - min)*ry);
            int cY = (int)(y0 + totalH - (price - min)*ry);
            y = oY < cY?oY:cY;
            int bodyH = Utils.ABS_INT(cY-oY);
            if (bodyH < 2)
                bodyH = 2;
            g.setColor(color);
            g.fillRect(x - bodyW/2, y, bodyW, bodyH);
            /*
            if (lo > 0 && lo != open && lo != price)
            {
                y = (int)(y0 + totalH - (lo - min)*ry);
                g.setColor(C.COLOR_WHITE);
                sb.Length = 0;
                sb.AppendFormat("{0:F1}", (float)lo/10);
                g.drawString(f, sb.ToString(), centerX - 44, y + 1, 0);
            }
            if (hi > 0 && hi != open && hi != price)
            {
                y = (int)(y0 + totalH - (hi - min)*ry);
                g.setColor(C.COLOR_WHITE);
                sb.Length = 0;
                sb.AppendFormat("{0:F1}", (float)hi/10);
                g.drawString(f, sb.ToString(), centerX - 44, y - f.Height, 0);       
            }
            //  2 lines
            g.setColor(C.COLOR_WHITE);
            sb.Length = 0;
            sb.AppendFormat("{0:F1}", (float)open/10);

            //  open
            if (oY < cY)
                y = oY - f.Height;
            else
                y = oY + 1;
            if (y < 0) y = 0;
            if (y + f.Height > getH())
                y = getH() - f.Height;
            g.drawString(f, sb.ToString(), x + bodyW / 2, y, 0);
            //  price
            sb.Length = 0;
            sb.AppendFormat("{0:F1}", (float)price/10);
            if (cY < oY)
                y = cY - f.Height;
            else
                y = cY + 1;
            if (y < 0) y = 0;
            if (y + f.Height > getH())
                y = getH() - f.Height;
            g.drawString(f, sb.ToString(), x + bodyW / 2, y, 0);    
            */
        }

        override public void onMouseDown(int x, int y)
        {
            invalidate();
        }

        override public void onMouseUp(int x, int y)
        {
            invalidate();

            if (Utils.isInRect(x, y, 0, 0, getW(), getH()))
            {
                if (mListener != null)
                {
                    mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, C.ID_SELECT_SHARE_CANDLE, mShare);
                }
            }
        }

        override public void onMouseDoubleClick()
        {
            if (mListener != null)
            {
                mListener.onEvent(this, xBaseControl.EVT_ON_MOUSE_DOUBLE_CLICK, C.ID_SELECT_SHARE_CANDLE, mShare);
            }
        }
    }
}
