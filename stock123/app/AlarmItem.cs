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
    public class AlarmItem: xBaseControl
    {
        stAlarm mAlarm;
        Context mContext;
        Image mAlarmImage;
        Image mArrowImage;

        public AlarmItem(stAlarm alarm)
            : base(null)
        {
            makeCustomRender(true);

            mAlarm = alarm;
            mContext = Context.getInstance();
            mAlarmImage = mContext.getImage(C.IMG_ALARM_BELL);
            mArrowImage = mContext.getImage(C.IMG_SMALL_ARROWS);

            xMainApplication.getxMainApplication().registerTick(this);
        }

        public override void render(xGraphics g)
        {    
            g.setColor(C.COLOR_BLACK);
            g.clear();

            stPriceboardState ps = mContext.mPriceboard.getPriceboard(mAlarm.code);
            if (ps == null)
                return;

            Font f = mContext.getBigFont();
            int x = 0;
            int y = 0;

            //  code
            x = 4;
            y = (getH() - f.Height) / 2 - 4;
            g.setColor(C.COLOR_WHITE);
            g.drawString(f, mAlarm.code, x, y - 4);

            x += 120;
            //  gia hien tai
            drawQuotePoint(g, x);

            x += 120;

            //  upper alarm
            int ch = getH() / 2 - 5;
            f = mContext.getFontText();
            y = (ch - f.Height)/2;
            int x0 = drawThreahold(g, x, y, true);   //  upper

            y = ch + (ch-f.Height)/2;
            int x1 = drawThreahold(g, x, y, false);   //  lower

            x = x0 > x1? x0+2:x1+2;
            //=========================================
            int alarm = mAlarm.hasAlarm();
            if (alarm != 0)
            {
                String s;
                if (alarm == 1)
                {
                    s = "Giá đã vượt lên trên " + Utils.formatNumber(mAlarm.upperPrice);
                    g.setColor(C.COLOR_GREEN);
                    y = (ch - mAlarmImage.Height) / 2;
                }
                else
                {
                    s = "Giá đã xuống thấp hơn " + Utils.formatNumber(mAlarm.lowerPrice);
                    g.setColor(C.COLOR_RED);
                    y = ch + (ch - mAlarmImage.Height) / 2;
                }
                if (mShowingAlarm)
                    g.drawImage(mAlarmImage, x, y);
                g.drawString(f, s, x + mAlarmImage.Width + 10, y+2);
            }

            if (mAlarm.comment != null && mAlarm.comment.Length > 0)
            {
                y = getH() - f.Height - 1;
                g.setColor(C.COLOR_YELLOW);
                g.drawString(mContext.getFontTextItalic(), "(" + mAlarm.comment + ")", 8, y);
            }

            //=======================
            g.setColor(C.COLOR_WHITE);
            g.drawHorizontalLine(0, getH() - 1, getW());
        }

        int drawThreahold(xGraphics g, int x, int y, bool isUpper)
        {
            int imgw = mArrowImage.Width / 3;
            int imgh = mArrowImage.Height;
            Font f = mContext.getFontText();
            String s;
            StringBuilder sb = Utils.getSB();
            if (isUpper)
            {
                g.drawImage(mArrowImage, x, y, imgw, imgh, 0, 0);

                x += 10;
                g.setColor(C.COLOR_WHITE);
                if (mAlarm.upperPrice > 0)
                {
                    sb.AppendFormat("{0:F1}", (float)mAlarm.upperPrice);
                    s = sb.ToString();
                }
                else
                    s = "  -";
                g.drawString(f, s, x, y);

                x += g.getStringWidth(f, s);
            }
            else
            {
                g.drawImage(mArrowImage, x, y, imgw, imgh, imgw, 0);

                x += 10;
                g.setColor(C.COLOR_WHITE);
                if (mAlarm.lowerPrice > 0)
                {
                    sb.AppendFormat("{0:F1}", (float)mAlarm.lowerPrice);
                    s = sb.ToString();
                }
                else
                    s = "  -";
                g.drawString(f, s, x, y);
                x += g.getStringWidth(f, s);
            }

            return x;
        }

        void drawQuotePoint(xGraphics g, int x)
        {
            stPriceboardState ps = mContext.mPriceboard.getPriceboard(mAlarm.code);

            //  current price
            float price = ps.getCurrentPrice();
            uint color = mContext.valToColorF(price, ps.getCe(), ps.getRef(), ps.getFloor());
            if (price == 0)
                price = ps.getRef();

            Font f = mContext.getBigFont();
            StringBuilder sb = Utils.getSB();
            //  current price
            sb.AppendFormat("{0:F2}", (float)price);
            int y = 0;

            //l.setFont(f);
            g.setColor(color);
            g.drawString(f, sb.ToString(), x, y);

            y += f.Height;
            //  change
            float change = price - ps.getRef();
            float changePercent = 0;
            if (ps.getRef() > 0)
                changePercent = (change * 100) / ps.getRef();
            sb.Length = 0;
            sb.AppendFormat("{0:F1} ({1:F2}%)", (float)change, changePercent);

            f = mContext.getFontText();

            g.setColor(color);
            g.drawString(f, sb.ToString(), x, y);
        }

        bool mShowingAlarm = true;
        xTimer mShowingAlarmTimer = new xTimer(500);
        public override void onTick()
        {
            if (mShowingAlarmTimer.isExpired())
            {
                mShowingAlarmTimer.reset();
                mShowingAlarm = !mShowingAlarm;
                invalidate();
            }
        }

        public override void dispose()
        {
            xMainApplication.getxMainApplication().unregisterTick(this);
        }

        override public void onMouseUp(int x, int y)
        {
            mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, C.ID_ALARM_MODIFY, mAlarm);
        }
    }
}
