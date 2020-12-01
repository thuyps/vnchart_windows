using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.utils;
using xlib.ui;
using stock123.app.data;

namespace stock123.app
{
    public class CompanyInfo: xContainer
    {
        Context mContext;
        bool mIsInited;
        bool mShowCompanyName;
        int mShareID;
        uint mFontColor = C.COLOR_BLACK;
        Font mFont;
        bool mIsCompaq = false;
        public CompanyInfo(int shareID)
            : base()
        {
            mContext = Context.getInstance();
            mShareID = shareID;
            mShowCompanyName = true;
            mFont = mContext.getFontText();
        }

        public void compaq()
        {
            mIsCompaq = true;
        }

        public void init()
        {
            if (mIsInited)
                return;

            mIsInited = true;

            Font f;
            xLabel l;
            int x = 0;
            int y = 0;
            int w = getW();
            String s;

            stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(mShareID);
            if (inf == null)
                return;
            if (mShowCompanyName)
            {
                f = mContext.getFontTextB();
                l = xLabel.createMultiLineLabel(inf.company_name, f, getW());
                addControl(l);
                l.setTextColor(mFontColor);
                y = l.getBottom() + 2;
            }

            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            f = mContext.getFontSmall();// Text();
            //if (mFont != null)
              //  f = mFont;
  
            //  EPS
            sb.AppendFormat("EPS={0:F1} K; P/E={1:F1}; Beta={2:F2}", (float)(inf.EPS)/1000, (float)(inf.PE)/100, (float)(inf.Beta)/100);
            l = xLabel.createSingleLabel(sb.ToString(), f, w);
            l.setPosition(x, y);
            l.setTextColor(mFontColor);
            addControl(l);
            y = l.getBottom();
            /*
            //  P/E
            sb.Length = 0;
            sb.AppendFormat("P/E: {0:F1}", (float)inf.PE/100);
            l = xLabel.createSingleLabel(sb.ToString(), f, w);
            l.setPosition(x, y);
            l.setTextColor(mFontColor);
            addControl(l);

            y = l.getBottom();
            
            //  Beta
            sb.Length = 0;
            sb.AppendFormat("Beta: {0:F2}", (float)inf.Beta / 100);
            l = xLabel.createSingleLabel(sb.ToString(), f, w);
            l.setPosition(x, y);
            l.setTextColor(mFontColor);
            addControl(l);

            y = l.getBottom();
    */
            //  book value            
            sb.Length = 0;
            sb.AppendFormat("Giá sổ sách={0:F1} K", (float)(inf.book_value / 10));
            l = xLabel.createSingleLabel(sb.ToString(), f, w);
            l.setPosition(x, y);
            l.setTextColor(mFontColor);
            addControl(l);

            y = l.getBottom();
    
            //  KLTB 10 phien
            sb.Length = 0;
            Share share = mContext.mShareManager.getShare(mShareID);
            if (share == null)
                return;
            int cnt = share.getCandleCount();
            int nums = cnt > 10?10:cnt;
            int total = 0;
            for (int i = 0; i < nums; i++)
            {
                int volume = share.getVolume(cnt-i-1);
                total += volume;
            }
            if (nums == 0) nums = 1;
            total = total/nums;
            s = Utils.formatNumber((int)total);
            sb.Length = 0;
            sb.AppendFormat("KLTB 10 phiên={0}", s);
            l = xLabel.createSingleLabel(sb.ToString(), f, w);
            l.setPosition(x, y);
            l.setTextColor(mFontColor);
            addControl(l);
            y = l.getBottom();

            //  KLCP luu hanh
            sb.Length = 0;
            sb.AppendFormat("KLCP={0:F2} tr; ", (inf.volume/1000.0f));
//            l = xLabel.createSingleLabel(sb.ToString(), f, w);
//            l.setPosition(x, y);
//            l.setTextColor(mFontColor);
//            addControl(l);
//            y = l.getBottom();
            /*
            //  KLCP luu hanh owned by foreign
            s = Utils.formatNumber(inf.volumeOwnedByForeigner);
            sb.Length = 0;
            sb.AppendFormat("KL NN giữ: {0}", s);
            l = xLabel.createSingleLabel(sb.ToString(), f, w);
            l.setPosition(x, y);
            l.setTextColor(mFontColor);
            addControl(l);
            y = l.getBottom();
            */
            //  Von hoa tt
            stPriceboardState ps = mContext.mPriceboard.getPriceboard(mShareID);
            float price = 0;
            double vonhoa = 0;
            if (ps != null)
            {
                price = ps.getCurrentPrice();
                if (price == 0)
                    price = ps.getRef();
                vonhoa = price;
                vonhoa = vonhoa * inf.volume;
                vonhoa = vonhoa / 1000;
            }
            else
            {

            }

            sb.AppendFormat("Vốn hóa tt={0:F2} tỉ", vonhoa);
            l = xLabel.createSingleLabel(sb.ToString(), f, w);
            l.setPosition(x, y);
            l.setTextColor(mFontColor);
            addControl(l);

            y = l.getBottom();
            //  ROA
            sb.Length = 0;
            sb.AppendFormat("ROA={0:F1} %; ROE={1:F1}", (float)inf.ROA, (float)inf.ROE);
            l = xLabel.createSingleLabel(sb.ToString(), f, w);
            l.setPosition(x, y);
            l.setTextColor(mFontColor);
            addControl(l);
            /*
            y = l.getBottom();

            //  ROE
            sb.Length = 0;
            sb.AppendFormat("ROE: {0:F1} %", (float)inf.ROE);
            l = xLabel.createSingleLabel(sb.ToString(), f, w);
            l.setPosition(x, y);
            l.setTextColor(mFontColor);
            addControl(l);
            */
            setSize(getW(), l.getBottom());
        }

        public void setFont(Font f)
        {
            mFont = f;
        }

        public void setFontColor(uint color)
        {
            mFontColor = color;
        }

        public void dontShowName()
        {
            mShowCompanyName = false;
        }
    }
}
