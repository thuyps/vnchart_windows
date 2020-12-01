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
    public class TodayCandle: xBaseControl
    {
        Share mShare;
        Context mContext;
        bool mHasTitle = false;
        bool mDrawRefLabel = true;
        bool mIsSelected = false;
        //===============================
        public TodayCandle()
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
            Utils.trace("=====render today candle");

            if (mIsSelected)
            {
                g.setColor(C.COLOR_BLUE);
                g.fillRect(0, 0, getW(), getH());

                g.setColor(C.COLOR_BLACK);
                g.fillRect(3, 3, getW() - 6, getH() - 6);
            }
            else
            {
                g.setColor(C.COLOR_BLACK);
                g.fillRect(0, 0, getW(), getH());
            }

            g.setColor(C.COLOR_GRAY_LIGHT);
            g.drawRect(0, 0, getW()-1, getH()-1);
    
            Share share = mShare;
            if (share == null)
                return ;

            int x, y;
            x = 0;
            y = 0;
            int h = getH();
            if (mHasTitle)
            {
                Font ft = mContext.getFontTextB();
                //  title background
                if (mIsSelected)
                    g.setColor(C.COLOR_BLUE);
                else
                    g.setColor(0xff204040);
                    //g.setColor(0xff004070);
                g.fillRect(1, 1, getW() - 2, 1*ft.Height);

                //  code
                g.setColor(C.COLOR_WHITE);
                g.drawString(ft, mShare.mCode, 1, 1);
                //  point
                stPriceboardState ps = mContext.mPriceboard.getPriceboard(share.getID());
                float price = ps.getCurrentPrice();
                float change = price - ps.getRef();
                StringBuilder sb = Utils.getSB();
                if (change >= 0)
                    sb.AppendFormat("{0:F2} (+{1:F2})", (float)price, (float)change);
                else
                    sb.AppendFormat("{0:F2} ({1:F2})", (float)price, (float)change);

                x += g.getStringWidth(ft, mShare.mCode) + 4;

                uint color = mContext.valToColorF(price, ps.getCe(), ps.getRef(), ps.getFloor());
                g.setColor(color);
                g.drawString(ft, sb.ToString(), x, 1);
                //=====vol
                g.setColor(C.COLOR_ORANGE);
                x = 1;
                ft = mContext.getFontSmall();
                y = getH() - ft.Height;

                sb.Length = 0;
                if (ps.getTotalVolume() > 100000)
                    sb.AppendFormat("{0}K", (ps.getTotalVolume()/1000));
                else if (ps.getTotalVolume() > 1000)
                    sb.AppendFormat("{0:F1}K", (float)(ps.getTotalVolume()/1000));
                else
                    sb.AppendFormat("{0}", ps.getTotalVolume());
                g.drawString(ft, sb.ToString(), x, y);
                //=========================
                x = 0;
                h -= (int)ft.Height;
                y = ft.Height;
            }
            //===========================================
            drawCandle(g, x, y, getW(), h);
        }

        float getMax(float v1, float v2, float v3)
        {
            float max = v1;
            if (v2 > max) max = v2;
            if (v3 > max) max = v3;

            return max;
        }

        float getMin(float v1, float v2, float v3)
        {
            float min = v1;
            if (v2 < min) min = v2;
            if (v3 < min) min = v3;
            
            return min;
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
            float open = mContext.mPriceboard.getOpen(ps.getID());
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
            //  check hi/lo valid
            if ((hi == 0 || lo == 0))
            {
                float[] hl = new float[2];
                if (trade.getHiLo(hl))
                {
                    if (hi == 0) hi = hl[0];
                    if (lo == 0) lo = hl[1];
                }
            }

            if (hi == 0) hi = open > price ? open : price;
            if (lo == 0) lo = open < price ? open : price;
            if (lo == 0) lo = hi;
            //---------------------------------------------
            float priceLen = hi - lo;
            float reference = ps.getRef();
            float min, max;
            if (share.isIndex())
            {
                stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(mShare.getMarketID());
                price = pi.current_point;

                min = lo - price / 40;
                max = hi + price / 40;
            }
            else
            {
                min = ps.getFloor() - reference / 30;
                max = ps.getCe() + reference / 30;
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
            if (mDrawRefLabel)
            {
                sb.AppendFormat("{0:F2}", reference);
                g.drawString(f, sb.ToString(), 1, y - f.Height / 2, 0);
            }
            //  CE line
            if (!share.isIndex())
            {
                g.setColor(0x30ff00ff);
                y = (int)(y0 + totalH - (ps.getCe() - min) * ry);
                g.drawLineDotHorizontal(1, y, getW() - 2, y);
                g.setColor(0xa0ff00ff);
                sb.Length = 0;
                sb.AppendFormat("{0:F2}", ps.getCe());

                if (mDrawRefLabel)
                    g.drawString(f, sb.ToString(), 1, y, 0);

                //  FLOOR line
                g.setColor(0x3000FFFF);
                y = (int)(y0 + totalH - (ps.getFloor() - min) * ry);
                g.drawLineDotHorizontal(1, y, getW() - 2, y);
                g.setColor(0xa000FFFF);
                sb.Length = 0;
                sb.AppendFormat("{0:F2}", ps.getFloor());
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

            if (hi > 0 && lo > 0)
            {
                int minY = (int)(y0 + totalH - (lo-min)*ry);
                int maxY = (int)(y0 + totalH - (hi-min)*ry);    

                g.drawLine(x, maxY, x, minY);
            }
            int centerX = x;
            //  candle's body
            int oY = (int)(y0 + totalH - (open - min)*ry);
            int cY = (int)(y0 + totalH - (price - min)*ry);
            y = oY < cY?oY:cY;
            int bodyH = Utils.ABS_INT(cY-oY);
            if (bodyH < 2)
                bodyH = 2;
            g.setColor(color);
            g.fillRect(x - bodyW/2, y, bodyW, bodyH);

            if (lo > 0 && lo != open && lo != price)
            {
                y = (int)(y0 + totalH - (lo - min)*ry);
                g.setColor(C.COLOR_YELLOW);
                sb.Length = 0;
                sb.AppendFormat("{0:F2}", lo);
                g.drawString(f, sb.ToString(), centerX - 10, y + 1, 0);
            }
            if (hi > 0 && hi != open && hi != price)
            {
                y = (int)(y0 + totalH - (hi - min)*ry);
                g.setColor(C.COLOR_YELLOW);
                sb.Length = 0;
                sb.AppendFormat("{0:F2}", hi);
                g.drawString(f, sb.ToString(), centerX - 10, y - f.Height, 0);
            }
            //  2 lines
            g.setColor(C.COLOR_WHITE);
            sb.Length = 0;
            sb.AppendFormat("{0:F2}", open);

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
            sb.AppendFormat("{0:F2}", price);
            if (cY < oY)
                y = cY - f.Height;
            else
                y = cY + 1;
            if (y < 0) y = 0;
            if (y + f.Height > getH())
                y = getH() - f.Height;
            g.drawString(f, sb.ToString(), x + bodyW / 2, y, 0);    
        }

        override public void onMouseDown(int x, int y)
        {
            mIsSelected = true;
            invalidate();
        }

        override public void onMouseUp(int x, int y)
        {
            mIsSelected = false;
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
