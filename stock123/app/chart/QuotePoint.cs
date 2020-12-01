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
    public class QuotePoint: xBaseControl
    {
        Share mShare;
        Context mContext;
        //===============================
        public QuotePoint()
            :base(null)
        {
            makeCustomRender(true);
            mContext = Context.getInstance();
        }
        public void setShare(Share share) { mShare = share; }
        override public void render(xGraphics g)
        {
            g.setColor(C.COLOR_BLACK);
            g.fillRect(0, 0, getW(), getH());

            g.setColor(C.COLOR_GRAY_LIGHT);
            g.drawRect(0, 0, getW(), getH());
    
            Share share = mShare;
            if (share == null)
                return ;

            if (share.mCClose == null && share.getID() > 0)
            {
                share.loadShareFromCommonData(true);
                mContext.setCurrentShare(share.getID());
            }
            else
            {
                //return;
            }
            if (share.mCClose == null || share.getID() == 0)
            {
                return;
            }

            Font f = mContext.getFontText();
            int x, y;

            x = 0;
            y = 0;

            stPriceboardState ps = mContext.mPriceboard.getPriceboard(share.getID());
            if (ps == null)
                return;

            StringBuilder sb = Utils.getSB();

            f = mContext.getBigFont();
            //  current price
            float price = ps.getCurrentPrice();
            uint color = mContext.valToColorF(price, ps.getCe(), ps.getRef(), ps.getFloor());
            if (price == 0)
                price = ps.getRef();

            //  current price
            sb.AppendFormat("{0:F2}", price);

            //l.setFont(f);
            x = 10;
            g.setColor(color);
            g.drawString(f, sb.ToString(), x, y);

            y += f.Height;
            //  change
            float change = price - ps.getRef();
            float changePercent = 0;
            if (ps.getRef() > 0)
                changePercent = (change * 100) / ps.getRef();
            sb.Length = 0;
            sb.AppendFormat("{0:F1} ({1:F2}%)", (float)(change / 10), changePercent);
            
            f = mContext.getFontText();

            g.setColor(color);
            g.drawString(f, sb.ToString(), x, y);
            //  --------------------vol
            x = 4;
            y += f.Height + 10;
            int vol = ps.getTotalVolume();
            int vol10 = share.getAveVolumeInDays(10);

            int barMax = getW() - 20;
            float bar0 = 0;
            float bar10 = 0;
            if (vol < vol10)
            {
                bar0 = ((float)vol / vol10) * barMax;
                bar10 = barMax;
            }
            else if (vol != 0)
            {
                bar0 = barMax;
                bar10 = ((float)vol10 / vol) * barMax;
            }
            if (bar0 < 1) bar0 = 1;
            if (bar10 < 1) bar10 = 1;

            string tmp = Utils.formatNumber(vol);
            sb.Length = 0;
            sb.AppendFormat("Vol: {0}", tmp);

            g.setColor(C.COLOR_WHITE);
            g.drawString(f, sb.ToString(), x, y);

            y += (int)f.Height;

            g.setColor(C.COLOR_ORANGE);
            g.fillRect(x, y, (int)bar0, 10);
            y += 20;
            //  --------------------vol10
            tmp = Utils.formatNumber(vol10);
            sb.Length = 0;
            sb.AppendFormat("TB 10phiên: {0}", tmp);

            g.setColor(C.COLOR_WHITE);
            g.drawString(f, sb.ToString(), x, y);

            y += f.Height;
            g.setColor(C.COLOR_ORANGE);
            g.fillRect(x, y, (int)bar10, 10);
        }
    }
}
