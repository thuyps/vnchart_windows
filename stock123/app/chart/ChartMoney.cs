using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.framework;
using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;

namespace stock123.app.chart
{
    public class ChartMoney: xBaseControl
    {
        class MoneyVol
        {
            public float price;
            public int volume;
        }

        Context mContext;
        const int mMapW = 10;
        const int mMapH = 10;
        short[] mMap = new short[mMapW*mMapH];

        int mLastTime;
        int mShareID;

        xVector mFreeBlocks = new xVector();    //  <stBubbleBlock*>
        xVector mUsedBlocks = new xVector();    //  <stBubbleBlock*>

        float rx;
        float ry;
        //==========================================
        public ChartMoney(int shareID)
            : base(null)
        {
            makeCustomRender(true);

            mContext = Context.getInstance();
            mShareID = shareID;
            for (int i = 0; i < 100; i++)
            {
                stBubbleBlock b = new stBubbleBlock();
                mFreeBlocks.addElement(b);
            }
        }

        override public void render(xGraphics g)
        {
            g.setColor(C.COLOR_BLACK);
            g.clear();

            //  title

            xVector v = getMoneyVolumeArray();
            if (v == null || v.size() == 0)
                return;
            int biggest = 0;
            int i = 0;
            for (i = 0; i < v.size(); i++)
            {
                MoneyVol mv = (MoneyVol)v.elementAt(i);
                if (mv.volume > biggest)
                    biggest = mv.volume;
            }

            float rx;
            StringBuilder sb = Utils.sb;
            uint color;
            int itemH = (getH() - 18)/ v.size() - 4;
            int y = 4;
            int x = 60;
            stPriceboardState ps = mContext.mPriceboard.getPriceboard(mShareID);
            if (ps == null)
                return;

            Font f = mContext.getFontSmall();
            int x1 = g.getStringWidth(f, "444.44") + 2;
            int w0 = getW() - x1;
            string s;

            g.setColor(C.COLOR_GRAY_DARK);
            g.fillRect(0, 0, x1, 14);
            g.setColor(C.COLOR_WHITE);
            g.drawString(f, "Giá", 2, 0);

            g.setColor(C.COLOR_GRAY_DARK);
            g.fillRect(x1+2, 0, getW()-x1-5, 14);
            g.setColor(C.COLOR_WHITE);
            g.drawString(f, "Khối lượng", x1+4, 0);

            y = 18;
            for (i = 0; i < v.size(); i++)
            {
                MoneyVol mv = (MoneyVol)v.elementAt(i);
                sb.Length = 0;
                sb.AppendFormat("{0:F2}", (float)mv.price);
                //  gia
                g.setColor(C.COLOR_GRAY_DARK);
                g.fillRect(0, y, x1, itemH);
                g.setColor(0xffffffff);
                g.drawString(f, sb.ToString(), 2, y);

                if (x1 == 0)
                    x1 = g.getStringWidth(f, sb.ToString());
                //  column
                color = mContext.valToColorF(mv.price, ps.getCe(), ps.getRef(), ps.getFloor());
                g.setColor(color);
                float w = ((float)mv.volume / biggest) * (w0-10);
                g.fillRect(2 + x1, y, (int)w, itemH);
                //  volume
                s = Utils.formatNumber(mv.volume);
                sb.Length = 0;
                sb.AppendFormat("kl={0:}", s);
                s = sb.ToString();
                int w2 = g.getStringWidth(f, s) + 2;
                g.setColor(C.COLOR_GRAY_DARK);
                g.fillRect(4 + x1, y, w2, 14);
                g.setColor(C.COLOR_ORANGE);
                g.drawString(f, s, 5 + x1, y);
                y += itemH + 3;
            }
        }

        public xVector getMoneyVolumeArray()
        {
            //=================================
            TradeHistory trade = mContext.getTradeHistory(mShareID);
            if (trade == null)
                return null;
            xVector v = new xVector(20);    //  MoneyVolume
            int total = 0;
            int lastVol = 0;
            for (int i = 0; i < trade.getTransactionCount(); i++)
            {
                float price = trade.getPrice(i);

                //int vol = trade.getTradeVolume(i);
                int vol = trade.getVolume(i);

                total += (vol - lastVol);

                bool ok = false;
                MoneyVol mv;
                for (int j = 0; j < v.size(); j++)
                {
                    mv = (MoneyVol)v.elementAt(j);
                    if (mv.price == price)
                    {
                        mv.volume += (vol-lastVol);//vol;
                        ok = true;
                        break;
                    }
                }
                if (!ok)
                {
                    mv = new MoneyVol();
                    mv.price = price;
                    mv.volume = (vol-lastVol);//vol;

                    v.addElement(mv);
                }

                lastVol = vol;
            }

            while (v.size() > 10)   //  too many
            {
                xVector vv = new xVector(20);
                int remain = 1;
                if ((v.size() % 2) == 0)
                    remain = 2;

                int removingItem = v.size() - 10;

                for (int i = 0; i < v.size()-1; i+=2)
                {
                    MoneyVol m0 = (MoneyVol)v.elementAt(i);
                    MoneyVol m1 = (MoneyVol)v.elementAt(i+1);

                    m0.volume += m1.volume;
                    m0.price = m1.price < m0.price?m0.price:m1.price;

                    vv.addElement(m0);

                    removingItem--;
                    if (removingItem <= 0)
                    {
                        i += 2;
                        for (; i < v.size() - 1; i++)
                        {
                            vv.addElement(v.elementAt(i));
                        }
                        break;
                    }
                }
                //  always keep the last elements
                if (remain == 2)
                    vv.addElement(v.elementAt(v.size()-2));
                vv.addElement(v.lastElement());

                v = vv;
            }

            sortBlocks(v);
            return v;
        }

        void sortBlocks(xVector v)
        {
            MoneyVol smallest;
            int smallestIdx;
            for (int i = 0; i < v.size() - 1; i++)
            {
                smallest = (MoneyVol)v.elementAt(i);
                smallestIdx = i;
                for (int j = i; j < v.size(); j++)
                {
                    MoneyVol mv = (MoneyVol)v.elementAt(j);

                    if (smallest.price > mv.price)
                    {
                        smallest = mv;
                        smallestIdx = j;
                    }
                }

                v.swap(i, smallestIdx);
            }
        }

        public void setShareID(int shareID)
        {
            mShareID = shareID;
            mLastTime = 0;
            invalidate();
        }
    }
}
