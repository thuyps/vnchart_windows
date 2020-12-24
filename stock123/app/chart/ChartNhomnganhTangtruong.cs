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
    public class ChartNhomnganhTangtruong: xBaseControl
    {
        const int VIEWSTATE_ALL_NHOMNGANH = 0;
        const int VIEWSTATE_1_NHOMNGANH = 1;
        int viewState = VIEWSTATE_ALL_NHOMNGANH;

        //public delegate void DelegateShowHistoryChartOfGroup(String code);
        //public delegate void DelegateShowHistoryChartOfShare(String code);

        public event ChartTangtruongGroup.OnClickShare delegateShowHistoryChartOfShare;
        public event ChartTangtruongGroup.OnHistoryClick delegateShowHistoryChartOfGroup;
        class GroupChanges
        {
            public stShareGroup group;
            public String code;
            public double changedPercent;
            public double GTGD;

            public int x, y, w, h;
        }

        Context mContext;

        xVector mGroups;
        xVector mChanges = new xVector();
        xVector mButtonPositions = new xVector();
        xVectorInt mPeriods = new xVectorInt();
        int mPeriod = 1;

        bool isProcessing = false;
        DlgProcessing dlgProcessing;

        ChartTangtruongGroup chartG = new ChartTangtruongGroup();
        //==========================================
        void showDetailOfGroup(stShareGroup g)
        {
            viewState = VIEWSTATE_1_NHOMNGANH;

            if (g.getTotal() == 1 && 
                (g.getName().CompareTo("^VNINDEX") == 0
                || g.getName().CompareTo("^HASTC") == 0
                || g.getName().CompareTo("^UPCOM") == 0))
            {
                int cnt = mContext.mShareManager.getShareCount();
                int[] market = { 0 };

                int marketID = 1;
                if (g.getName().CompareTo("^HASTC") == 0){
                    marketID = 2;
                }
                else if (g.getName().CompareTo("^UPCOM") == 0){
                    marketID = 3;
                }

                for (int i = 0; i < cnt; i++)
                {
                    int shareID = mContext.mShareManager.getShareIDAt(i, market);

                    if (shareID > 0 && market[0] == marketID)
                    {
                        g.addCode(mContext.mShareManager.getShareCode(shareID));
                    }

                }
            }

            chartG = new ChartTangtruongGroup();

            chartG.setGroup(g);
            chartG.setPosition(0, 0);

            chartG.onBack += dismissGroupDetail;
            //chartG.setSize(mPriceboardContainer);

            int h = getH();// chartG.estimateH(getH(), g.getTotal());

            if (h < getH()) h = getH();
            //list.setSize(W_PRICEBOARD, mPriceboardH);
            chartG.setSize(getW() - 20, h);

            chartG.onBack += dismissGroupDetail;
            chartG.onHistoryClick += delegateShowHistoryChartOfGroup;
            chartG.onClickShare += delegateShowHistoryChartOfShare;
            chartG.onProcessingCompleted += onProcessingCompleted;
            chartG.onProcessingStart += onProcessingStart;

            invalidate();
        }

        void dismissGroupDetail()
        {
            viewState = VIEWSTATE_ALL_NHOMNGANH;
            chartG = null;

            invalidate();
        }

        void onProcessingStart()
        {
            invalidate();
        }

        void onProcessingCompleted()
        {
            System.Threading.Thread.Sleep(500);
            invalidate();
        }

        //==========================================
        public ChartNhomnganhTangtruong()
            : base(null)
        {
            makeCustomRender(true);

            mContext = Context.getInstance();
        }

        public void setGroup(xVector groups)
        {
            //--------------------
            mGroups = groups;
            for (int i = 0; i < mGroups.size(); i++)
            {
                stShareGroup g = (stShareGroup)mGroups.elementAt(i);
                //calcIndexOfGroup(g);
            }

            doCalcChanged();
        }

        void doCalcChanged()
        {
            isProcessing = true;
            invalidate();
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
             {
                 calcChanged(mPeriod);

                 isProcessing = false;
                 invalidate();
             }, null);
        }

        override public void render(xGraphics g)
        {
            //=========================
            if (viewState == VIEWSTATE_1_NHOMNGANH && chartG != null)
            {
                chartG.render(g);
                return;
            }

            g.setColor(C.COLOR_BLACK);
            g.clear();

            if (isProcessing)
            {
                g.setColor(C.COLOR_ORANGE);
                g.drawStringInRect(mContext.getFontSmallB(), "Đang xử lý", 0, 0, getW(), getH(), xGraphics.HCENTER | xGraphics.VCENTER);
                return;
            }
            
            //  title
            //      [Hom nay] [1 Tuan] [1 Thang] [3 thang] [1 nam]
            //          Tang          |          Giam          |
            //  VN30: +15%; GTDG: 30.1ti    .Chart
            //  
            if (mChanges.size() == 0){
                calcChanged(mPeriod);
            }
            if (mChanges.size() == 0){
                return;
            }

            
            float x, y;
            int buttonW = 80;
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

            int columnH = ItemH;
            double maxPercent = 0;
            for (int i = 0; i < mChanges.size(); i++)
            {
                GroupChanges gc = (GroupChanges)mChanges.elementAt(i);
                if (Math.Abs(gc.changedPercent) > maxPercent)
                {
                    maxPercent = Math.Abs(gc.changedPercent);
                }
            }
            //--------------------
            int buttonY = 30;
            int drawH = getH() - buttonY;
            ItemH = getItemH(getH() - 40, mChanges.size());

            float cellW = getW() / 2;
            float cellH = ItemH;
            maxPercent *= 1.15;
            //  Left side
            int j = 0;

            y = buttonY + 4;

            int itemPerColumn = mChanges.size() / 2;

            for (int i = 0; i < itemPerColumn; i++)
            {
                GroupChanges gc = (GroupChanges)mChanges.elementAt(i);
                x = 2;
                //if (gc.changedPercent >= 0)
                {
                    y = buttonY + 4 + j * cellH;
                    cellW = (float)((Math.Abs(gc.changedPercent) / maxPercent) * getW()/2);
                    g.setColor(gc.changedPercent>0?C.COLOR_GREEN_DARK:C.COLOR_RED);
                    g.fillRectF(x, y, cellW, cellH - 2);

                    //  text
                    g.setColor(C.COLOR_WHITE);
                    if (gc.changedPercent >= 0)
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
                    gc.w = (int)getW()/2-10;
                    gc.h = (int)cellH;
                }
            }
            g.setColor(C.COLOR_GRAY_DARK);
            g.drawVerticalLine(getW() / 2, buttonY, getH());
            //  Right side
            j = 0;
            for (int i = itemPerColumn; i < mChanges.size(); i++)
            {
                GroupChanges gc = (GroupChanges)mChanges.elementAt(i);
                x = getW()/2 + 2;
                //if (gc.changedPercent < 0)
                {
                    y = buttonY + 4 + j * cellH;
                    cellW = (float)((Math.Abs(gc.changedPercent) / maxPercent) * getW()/2);
                    g.setColor(gc.changedPercent > 0 ? C.COLOR_GREEN_DARK : C.COLOR_RED);
                    g.fillRectF(x, y, cellW, cellH - 2);

                    //  text
                    g.setColor(C.COLOR_WHITE);
                    if (gc.changedPercent >= 0)
                    {
                        string s = String.Format("{0}: +{1:F2} %", gc.code, gc.changedPercent);
                        g.drawString(mContext.getFontText(), s, (int)x, (int)(y + cellH / 2), xGraphics.VCENTER);
                    }
                    else
                    {
                        string s = String.Format("{0}: {1:F2} %", gc.code, gc.changedPercent);
                        g.drawString(mContext.getFontText(), s, (int)x, (int)(y + cellH / 2), xGraphics.VCENTER);
                    }
                    j++;

                    gc.x = (int)x;
                    gc.y = (int)y;
                    gc.w = (int)getW() / 2 - 10;
                    gc.h = (int)cellH;
                }
            }
        }

        public int estimateH(int containerH, int items)
        {
            int itemH = getItemH(containerH, items);
            int h = 0;
            if (mChanges.size() % 2 == 0)
            {
                h = 36 + (itemH + 6) * items / 2;
            }
            else
            {
                h = 36 + (itemH + 6) * (items / 2 + 1);
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

        int ItemH = 20;

        void calcChanged(int period)
        {
            stCandle c0 = new stCandle();
            stCandle c1 = new stCandle();
            mChanges.removeAllElements();

            Utils.trace("----------------------");
            bool isNhomIndex = false;

            for (int i = 0; i < mGroups.size(); i++)
            {
                stShareGroup g = (stShareGroup)mGroups.elementAt(i);        
                GroupChanges gc = new GroupChanges();
                gc.group = g;
                mChanges.addElement(gc);
                gc.code = g.getName();

                Utils.trace(g.getName());

                double firstGTGD = 0;
                double lastGTGD = 0;

                double totalGTGD0 = 0;
                double totalGTGD1 = 0;

                int today = 0;

                isNhomIndex = false;

                if (g.getType() == stShareGroup.ID_GROUP_MARKET_OVERVIEW
                    && g.getTotal() == 1)
                {
                    isNhomIndex = true;
                    String code = g.getCodeAt(0);
                    Share share = mContext.mShareManager.getShare(code);
                    if (share == null){
                        continue;
                    }
                    
                    share.loadShareFromFile(false);
                    share.appendTodayCandle2();

                    int last = share.getCandleCnt()-1;
                    int first = (last > period)?(last-period):0;
                    if (last < 0 || first < 0)
                    {
                        continue;
                    }
           
                    c0 = share.getCandle(last, c0);
                    c1 = share.getCandle(first, c1);
                    
                    if (c1.close > 0){
                        double changed = 100 * (c0.close - c1.close) / c1.close;

                        gc.changedPercent += changed;
                    }
                    
                    continue;
                }

                for (int j = 0; j < g.getTotal(); j++)
                {
                    string code = g.getCodeAt(j);
                    Share share = Context.getInstance().mShareManager.getShare(code);
                    int shareID = Context.getInstance().mShareManager.getShareID(code);
                    stCompanyInfo info = Context.getInstance().mShareManager.getCompanyInfo(shareID);
                    if (share != null && info != null)
                    {
                        share.loadShareFromCommonData(true);
                        int last = share.getCandleCnt()-1;
                        int first = (last > period)?(last-period):0;
                        if (last < 0 || first < 0)
                        {
                            continue;
                        }

                        share.getCandle(last, c0);
                        share.getCandle(first, c1);

                        double changed;
                        if (period == 1 && c0.date < today)
                        {
                            //  khong co giao dich hom nay
                            changed = 0;
                        }
                        else
                        {
                            double r = (info.volume * c1.close);
                            changed = 0;// (c0.close - c1.close) * r;

                            changed = 100 * (c0.close - c1.close) / c1.close;
                            changed *= r;
                        }

                        totalGTGD0 += (c0.close * info.volume);
                        totalGTGD1 += (c1.close * info.volume);

                        gc.changedPercent += changed;
                        //String s = String.Format("Code: {0}, Price: Close0/Close1={1}/{2}, r={3} changed/totalChanged: {4}/{5}", 
                            //share.getCode(), c0.close, c1.close, r, changed, gc.changedPercent);
                        //Utils.trace(s);
                    }

                    gc.changedPercent = 100 * (totalGTGD0 - totalGTGD1);
                    gc.changedPercent /= totalGTGD1;
                }

                //Utils.trace(String.Format("Total changed: {0}", gc.changedPercent));
            }

            //  sort
            if (false)//isNhomIndex)
            {
                //  khong sort
            }
            else
            {
                xVector marketOverView = new xVector();
                for (int i = 0; i < mChanges.size() - 1; i++)
                {
                    GroupChanges g = (GroupChanges)mChanges.elementAt(i);
                    if (g.group.getType() == stShareGroup.ID_GROUP_MARKET_OVERVIEW || g.code.IndexOf('#') == 0)
                    {
                        marketOverView.addElement(g);
                    }
                }

                for (int i = 0; i < marketOverView.size(); i++)
                {
                    mChanges.removeElement(marketOverView.elementAt(i));
                }

                //================================

                for (int i = 0; i < mChanges.size() - 1; i++)
                {
                    GroupChanges biggest = (GroupChanges)mChanges.elementAt(i);
                    int biggestIdx = i;
                    for (int j = i + 1; j < mChanges.size(); j++)
                    {
                        GroupChanges gc = (GroupChanges)mChanges.elementAt(j);
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
                //========================
                for (int i = 0; i < mChanges.size(); i++)
                {
                    marketOverView.addElement(mChanges.elementAt(i));
                }
                mChanges.removeAllElements();
                for (int i = 0; i < marketOverView.size(); i++)
                {
                    mChanges.addElement(marketOverView.elementAt(i));
                }
            }
        }

        public override void onMouseUp(int x, int y)
        {
            if (viewState == VIEWSTATE_1_NHOMNGANH && chartG != null)
            {
                chartG.onMouseUp(x, y);
                return;
            }
            for (int i = 0; i < mButtonPositions.size(); i++)
            {
                Rectangle rc = (Rectangle)mButtonPositions.elementAt(i);
                if (rc.Contains(x, y))
                {
                    mPeriod = mPeriods.elementAt(i);
                    
                    doCalcChanged();
                    return;
                }
            }

            for (int i = 0; i < mChanges.size(); i++)
            {
                GroupChanges gc = (GroupChanges)mChanges.elementAt(i);
                if (x >= gc.x && x <= gc.x + gc.w
                    && y >= gc.y && y <= gc.y + gc.h)
                {
                    stShareGroup g = gc.group;
                    showDetailOfGroup(g);
                }
            }
        }
    }
}
