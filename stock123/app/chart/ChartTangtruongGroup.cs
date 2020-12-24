using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.framework;
using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;
using System.Threading;
using stock123.app.ui;

namespace stock123.app.chart
{
    public class ChartTangtruongGroup: xBaseControl
    {
        public delegate void OnBack();
        public delegate void OnClickShare(string code);
        public delegate void OnHistoryClick(string code);
        public delegate void OnProcessingCompleted();
        public event OnBack onBack;
        public event OnClickShare onClickShare;
        public event OnHistoryClick onHistoryClick;

        public event OnProcessingCompleted onProcessingCompleted;
        public event OnProcessingCompleted onProcessingStart;

        class ShareChanges
        {
            public String code;
            public double changedPercent;
            public double GTGD;

            public int x, y, w, h;
        }

        Context mContext;

        xVector mChanges = new xVector();
        xVector mButtonPositions = new xVector();
        xVectorInt mPeriods = new xVectorInt();
        int mPeriod = 1;

        bool isProcessing = false;
        stShareGroup shareGroup;
        //==========================================
        public ChartTangtruongGroup()
            : base(null)
        {
            makeCustomRender(true);

            mContext = Context.getInstance();
        }

        public void setGroup(stShareGroup g)
        {
            shareGroup = g;

            doCalcChanged();
        }

        void doCalcChanged()
        {
            isProcessing = true;
            invalidate();
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
             {
                 Thread.Sleep(100);

                 calcChanged(mPeriod);

                 isProcessing = false;
                 invalidate();

                 onProcessingCompleted();
             }, null);
        }

        override public void render(xGraphics g)
        {
            g.setColor(C.COLOR_BLACK);
            g.clear();

            if (isProcessing)
            {
                g.setColor(C.COLOR_ORANGE);
                g.drawStringInRect(mContext.getFontSmallB(), "Đang xử lý", 0, 0, getW(), getH(), xGraphics.HCENTER | xGraphics.VCENTER);
                return;
            }

            if (onBack != null)
            {
                g.setColor(C.COLOR_ORANGE);
                g.drawString(mContext.getFontSmall(), "<<<BACK", 10, 4);
            }
            if (onHistoryClick != null)
            {
                g.setColor(C.COLOR_ORANGE);
                g.drawString(mContext.getFontSmall(), ">>>Đồ Thị", getW()-10, 4, xGraphics.RIGHT);
            }

            //  title
            //      [Hom nay] [1 Tuan] [1 Thang] [3 thang] [1 nam]
            //          Tang          |          Giam          |
            //  VN30: +15%; GTDG: 30.1ti    ->Chart
            //  
            if (mChanges.size() == 0){
                calcChanged(mPeriod);
            }
            if (mChanges.size() == 0){
                return;
            }

            
            float x, y;
            int buttonW = 76;
            int buttonH = 26;

            int gap = 4;
            x = (getW() - 6*(buttonW+gap))/2;

            //  button
            y = 2;
            mButtonPositions.removeAllElements();
            mPeriods.removeAllElements();
            string[] buttons = { "1 Ngày", "1 Tuần", "1 Tháng", "3 Tháng", "6 Tháng", "1 Năm" };
            int[] periods = { 1, 5, 22, 67, 130, 260};
            for (int i = 0; i < buttons.Length; i++)
            {
                string s = buttons[i];
                g.setColor(C.COLOR_GRAY_LIGHT);
                g.drawRect((int)x, 2, buttonW, buttonH);
                g.setColor(mPeriod == periods[i]?C.COLOR_ORANGE:C.COLOR_WHITE);
                g.drawStringInRect(mContext.getFontSmallB(), s, (int)x, (int)y, buttonW, buttonH, xGraphics.HCENTER | xGraphics.VCENTER);

                Rectangle rc = new Rectangle((int)x, 2, buttonW, buttonH);
                mButtonPositions.addElement(rc);
                mPeriods.addElement(periods[i]);
                
                x += buttonW + gap;
            }

            g.setColor(C.COLOR_GRAY_DARK);
            g.drawHorizontalLine(0, buttonH + 4, getW());

            int numberOfColumns = 4;
            int maxItemPerColumns = mChanges.size() / numberOfColumns;
            if ((mChanges.size() % numberOfColumns) != 0) maxItemPerColumns++;

            ItemH = (getH() - 40) / maxItemPerColumns;

            int columnH = ItemH;
            float maxPercent = 0;
            for (int i = 0; i < mChanges.size(); i++)
            {
                ShareChanges gc = (ShareChanges)mChanges.elementAt(i);
                if (Math.Abs(gc.changedPercent) > maxPercent)
                {
                    maxPercent = (float)Math.Abs(gc.changedPercent);
                }
            }
            //--------------------
            int buttonY = 30;

            columnH = ItemH;
            float cellW = getW() / numberOfColumns;
            float cellH = ItemH;
            maxPercent *= 1.15f;
            //  Left side
            int j = 0;

            y = buttonY + 4;

            int itemPerColumn = maxItemPerColumns;

            x = 2;

            drawItems(0, maxItemPerColumns, x, y, cellW, columnH, maxPercent, numberOfColumns, g);
            x += getW()/numberOfColumns;

            drawItems(maxItemPerColumns, maxItemPerColumns, x, y, cellW, columnH, maxPercent, numberOfColumns, g);
            x += getW() / numberOfColumns;

            drawItems(2 * maxItemPerColumns, maxItemPerColumns, x, y, cellW, columnH, maxPercent, numberOfColumns, g);
            x += getW()/numberOfColumns;

            drawItems(3 * maxItemPerColumns, maxItemPerColumns, x, y, cellW, columnH, maxPercent, numberOfColumns, g);
        }

        void drawItems(int offset, int itemPerColumn, float x, float y0, float cellW, float cellH, float maxPercent, int numberOfColumns, xGraphics g)
        {

            //====================================
            for (int i = 0; i < itemPerColumn; i++)
            {
                int j = i + offset;
                if (j >= mChanges.size())
                {
                    return;
                }

                int y = (int)(y0 + i * cellH);

                ShareChanges gc = (ShareChanges)mChanges.elementAt(j);
                cellW = (float)((Math.Abs(gc.changedPercent) / maxPercent) * getW() / numberOfColumns);
                //if (gc.changedPercent >= 0)
                {
                    g.setColor(gc.changedPercent>0?C.COLOR_GREEN_DARK:C.COLOR_RED);
                    g.fillRectF(x, y, cellW, cellH - 2);

                    //  text
                    g.setColor(C.COLOR_WHITE);
                    if (gc.changedPercent > 0)
                    {
                        string s = String.Format("{0} +{1:F2} %", gc.code, gc.changedPercent);
                        g.drawString(mContext.getFontText(), s, (int)x, (int)(y + cellH / 2), xGraphics.VCENTER);
                    }
                    else
                    {
                        string s = String.Format("{0} {1:F2} %", gc.code, gc.changedPercent);
                        g.drawString(mContext.getFontText(), s, (int)x, (int)(y + cellH / 2), xGraphics.VCENTER);
                    }
                    j++;

                    gc.x = (int)x;
                    gc.y = (int)y;
                    gc.w = (int)getW() / numberOfColumns - 10;
                    gc.h = (int)cellH;
                }
            }
            //g.setColor(C.COLOR_GRAY_DARK);
            //g.drawVerticalLine((int)(x + cellW), (int)y0, getH());
        }
        int ItemH = 24;

        void calcChanged(int period)
        {
            stCandle _c0 = new stCandle();
            stCandle _c1 = new stCandle();

            Share vnindex = mContext.mShareManager.getShare("^VNINDEX");
            vnindex.loadShareFromFile(true);

            if (vnindex.getCandleCnt() < period){
                return;
            }

            stCandle c0 = vnindex.getCandle(vnindex.getCandleCnt()-1, _c0);
            stCandle c1 = vnindex.getCandle(vnindex.getCandleCnt()-1-period, _c1);

            int startDate = c1.date;
            int endDate = c0.date;

            mChanges.removeAllElements();
            for (int i = 0; i < shareGroup.getTotal(); i++)
            {
                string code = shareGroup.getCodeAt(i);
                Share share = Context.getInstance().mShareManager.getShare(code);
                if (share != null)
                {
                    if (share.getShareID() == 1032)
                    {
                        Utils.trace("---");
                    }
                    share.loadShareFromCommonData(true);

                    if (share.getCandleCnt() > 0)
                    {
                        c0 = share.getCandleByDate(startDate, startDate, _c0);
                        c1 = share.getCandleByDate(endDate, startDate, _c1);

                        if (c0 == null || c1 == null)
                        {
                            continue;
                        }

                        ShareChanges change = new ShareChanges();
                        change.code = code;
                        change.changedPercent = 100*(c1.close - c0.close) / c0.close;

                        mChanges.addElement(change);
                    }
                }
            }

            //  sort abs
            for (int i = 0; i < mChanges.size() - 1; i++)
            {
                ShareChanges biggest = (ShareChanges)mChanges.elementAt(i);
                int biggestIdx = i;
                for (int j = i + 1; j < mChanges.size(); j++)
                {
                    ShareChanges gc = (ShareChanges)mChanges.elementAt(j);
                    if (Math.Abs(gc.changedPercent) > Math.Abs(biggest.changedPercent))
                    {
                        biggest = gc;
                        biggestIdx = j;
                    }
                }

                if (biggestIdx != i)
                {
                    mChanges.swap(i, biggestIdx);
                }
            }

            while (mChanges.size() > 120)
            {
                mChanges.removeElementAt(mChanges.size() - 1);
            }

            //  sort
            for (int i = 0; i < mChanges.size() - 1; i++)
            {
                ShareChanges biggest = (ShareChanges)mChanges.elementAt(i);
                int biggestIdx = i;
                for (int j = i + 1; j < mChanges.size(); j++)
                {
                    ShareChanges gc = (ShareChanges)mChanges.elementAt(j);
                    if (gc.changedPercent > biggest.changedPercent)
                    {
                        biggest = gc;
                        biggestIdx = j;
                    }
                }

                if (biggestIdx != i)
                {
                    mChanges.swap(i, biggestIdx);
                }
            }
        }

        public override void onMouseUp(int x, int y)
        {
            if (x >= 4 && x < 80 && y >= 4 && y < 20)
            {
                if (onBack != null)
                {
                    onBack();
                    return;
                }
            }
            if (x >= getW()-80 && x < getW() && y >= 4 && y < 20)
            {
                if (onHistoryClick != null)
                {
                    mContext.mShareManager.calcIndexOfGroup(shareGroup);
                    onHistoryClick(shareGroup.getName());
                    return;
                }
            }

            for (int i = 0; i < mButtonPositions.size(); i++)
            {
                Rectangle rc = (Rectangle)mButtonPositions.elementAt(i);
                if (rc.Contains(x, y))
                {
                    mPeriod = mPeriods.elementAt(i);

                    isProcessing = true;
                    if (onProcessingStart != null)
                    {
                        onProcessingStart();
                    }
                    
                    doCalcChanged();
                    return;
                }
            }

            if (onClickShare != null)
            {
                for (int i = 0; i < mChanges.size(); i++)
                {
                    ShareChanges gc = (ShareChanges)mChanges.elementAt(i);
                    if (x >= gc.x && x <= gc.x + gc.w
                        && y >= gc.y && y <= gc.y + gc.h)
                    {
                        onClickShare(gc.code);
                    }
                }
            }
        }

        public int estimateH(int containerH, int items)
        {
            int itemH = getItemH(containerH, items);
            int h = 0;
            if (mChanges.size() % 2 == 0)
            {
                h = 36 + itemH * items / 2;
            }
            else
            {
                h = 36 + itemH * (items / 2 + 1);
            }
            return h;
        }

        int getItemH(float viewH, int totalItems)
        {
            if ((totalItems % 2) == 0)
            {
                ItemH = (int)((viewH / totalItems) * 2);
            }
            else
            {
                ItemH = (int)((viewH / (totalItems + 1)) * 2);
            }

            if (ItemH < 20) ItemH = 20;
            if (ItemH > 40) ItemH = 40;
            return ItemH;
        }
    }
}
