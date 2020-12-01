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
    class ChangeIndexItem
    {
        public int shareID;

        public float price;

        public float modifiedPercent;
        public float modifiedValue;

        public bool selected;

        public float x, y, w, h;
    };
    class ChartChangeIndex: ChartBase
    {
        Context mContext;
        int mShareID;
        int mLastTime;

        ChangeIndexItem currentSelected;

        double totalIndexInc;
        double totalIndexDec;

        //  ChangeIndexItem
        xVector vChangeInc;
        xVector vChangeDec;

        int startDate;
        int endDate;

        bool _isProcessing;
        stShareGroup group;
        //========================================
        public ChartChangeIndex(Font f)
            : base(f)
        {
            mContext = Context.getInstance();
            currentSelected = null;

            startDate = 0;
            endDate = 0;

            group = null;

            _isProcessing = false;
        }
        public override void render(xGraphics g)
        {
            g.setColor(0xff002000);
            g.fillRect(0, 0, getW(), getH());

            if (_isProcessing)
            {
                g.setColor(C.COLOR_ORANGE);
                g.drawStringInRect(mContext.getFontSmallB(), "Đang xử lý", 0, 0, getW(), getH(), xGraphics.HCENTER | xGraphics.VCENTER);
                return;
            }

            g.setColor(C.COLOR_ORANGE);
            
            String sz = String.Format("MãCP (giá, thay đổi %): Tăng/Giảm index: {0:F2}/{1:F2}", totalIndexInc, totalIndexDec);
            
            g.drawString(mContext.getFontSmall(), sz, 8, 1, xGraphics.TOP);
            
            if (totalIndexInc == 0 && totalIndexDec == 0){
                return;
            }
                        
            double total = totalIndexInc + Math.Abs(totalIndexDec);
            float incPercent = (float)(totalIndexInc/total);
            float decPercent = (float)(Math.Abs(totalIndexDec)/total);
            
            if (incPercent < 0.25) {incPercent = 0.25f; decPercent = 1.0f - incPercent;}
            if (decPercent < 0.25) {decPercent = 0.25f; incPercent = 1.0f - decPercent;}
            
            float incW = (getW() - 4)*0.5f;//incPercent;
            float decW = (getW() - 4)*0.5f;//decPercent;
            
            int maxItems = 22;
            int incCnt = vChangeInc.size() > maxItems?maxItems:vChangeInc.size();
            float itemH = (getH() - 20)/maxItems;
            int decCnt = vChangeDec.size() >maxItems?maxItems:vChangeDec.size();
            
            int x0 = 1;
            int x1 = (int)(getW() - decW);
            
            float incBiggest = 0;
            float decBiggest = 0;

            ChangeIndexItem first = (ChangeIndexItem)vChangeInc.firstElement();
            if (vChangeInc.size() > 0) incBiggest = 1.1f * first.modifiedValue;

            first = (ChangeIndexItem)vChangeDec.firstElement();
            if (vChangeDec.size() > 0) decBiggest = 1.1f * first.modifiedValue;
            
            float maxBiggest = Math.Max(incBiggest, decBiggest);
            
            int titleH = 20;
            
            //  increase
            for (int i = 0; i < incCnt; i++){
                ChangeIndexItem item = (ChangeIndexItem)vChangeInc.elementAt(i);
                
                g.setColor(C.COLOR_GREEN_DARK);
                float y = titleH + i*itemH;
                float itemW = item.modifiedValue*incW/maxBiggest;
                
                g.fillRectF(x0, y, itemW, itemH - 1);
                
                g.setColor(C.COLOR_WHITE);
                stPriceboardState ps = mContext.mPriceboard.getPriceboard(item.shareID);
                
                sz = String.Format("{0}({1:F1}, {2:F1}%): {3:F3}", ps.code, item.price, item.modifiedPercent, item.modifiedValue);
                g.drawStringF(mContext.getFontSmall(), sz, x0 + 8, y + itemH/2, xGraphics.VCENTER);
                
                item.x = x0;
                item.y = y;
                item.w = incW;
                item.h = itemH;
            }
            
            //  decrease
            for (int i = 0; i < decCnt; i++){
                ChangeIndexItem item = (ChangeIndexItem)vChangeDec.elementAt(i);
                
                g.setColor(C.COLOR_RED);
                float y = titleH + i*itemH;
                float itemW = item.modifiedValue*decW/maxBiggest;
                
                g.fillRectF(x1, y, itemW, itemH - 1);
                
                g.setColor(C.COLOR_WHITE);
                stPriceboardState ps = mContext.mPriceboard.getPriceboard(item.shareID);
                
                sz = String.Format("{0}({1:F1}, {2:F1%}): {3:F3}", ps.code, item.price, item.modifiedPercent, item.modifiedValue);
                g.drawStringF(mContext.getFontSmall(), sz, x1 + 8, y + itemH/2, xGraphics.VCENTER);
                
                item.x = x1;
                item.y = y;
                item.w = decW;
                item.h = itemH;
            }
            
            if (currentSelected != null && currentSelected.selected)
            {
                g.setColor(C.COLOR_ORANGE);
                g.drawRectF(currentSelected.x, currentSelected.y, currentSelected.w, currentSelected.h);
            }
        }

        public void setShareID(int shareID)
        {
            mShareID = shareID;
            mLastTime = 0;

            invalidate();
        }

        void setStartEnd(int startDate, int endDate)
        {
            //  for debug only
            //startDate = (2017 << 16) | (2<<8) | 2;
            //endDate = (2018 << 16) | (4 << 8) | 10;

            this.startDate = startDate;
            this.endDate = endDate;
        }


        //  ChangeIndexItem
        void sortChanges(xVector v)
        {
            ChangeIndexItem biggest;
            int biggestIdx;
            for (int i = 0; i < v.size() - 1; i++)
            {
                biggest = (ChangeIndexItem)v.elementAt(i);
                biggestIdx = i;
                for (int j = i; j < v.size(); j++)
                {
                    ChangeIndexItem mv = (ChangeIndexItem)v.elementAt(j);

                    if (biggest.modifiedValue < mv.modifiedValue)
                    {
                        biggest = mv;
                        biggestIdx = j;
                    }
                }

                v.swap(i, biggestIdx);
            }
        }

        public void refreshChart()
        {
            if (group == null){
                group = new stShareGroup();
                group.setType(stShareGroup.ID_GROUP_DEFAULT);
            }
    
            group.clear();
    
            if (startDate > 0 && endDate > 0 && startDate <= endDate)
            {
                refreshChart2();
                return;
            }
    
            currentSelected = null;
            
            Share index = mContext.mShareManager.getShare(mShareID);
            stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(index.getMarketID());
            if (index == null || pi == null){
                return;
            }

            if (mContext.mShareManager.getVnindexCnt() == 0){
                return;
            }
            
            _isProcessing = true;
            
            index.loadShareFromFile(false);
            index.appendTodayCandle2();
            
            if (index.getCandleCnt() < 10){
                return; //  impossible
            }
            //---------------------------
            
            endDate = index.getLastCandleDate();
            startDate = index.getDate(index.getCandleCnt()-2);
            
            //stCandle cIndex = index.getCan .getCandleByDate(startDate);
            stCandle cIndex = new stCandle();
            index.getCandleByDate(startDate, 0, cIndex);
            //===============================================
            double totalEquity = 0.0;
            totalIndexInc = 0;
            totalIndexDec = 0;

            int cnt = mContext.mShareManager.getVnindexCnt();

            double[] indexOfShares = new double[cnt];
            double[] changedPercents = new double[cnt];
            float[] prices = new float[cnt];

            stCandle _c0 = new stCandle();
            stCandle _c1 = new stCandle();
            //  tinh total equity
            for (int i = 0; i < cnt; i++)
            {
                Share share = mContext.mShareManager.getVnindexShareAt(i);
                if (share.getShareID() == 223){
                    indexOfShares[i] =1;
                }
                indexOfShares[i] = 0;
                
                if (share.getMarketID() == index.getMarketID() && !share.isIndex())
                {
                    stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(share.getShareID());
                    if (inf != null)
                    {
                        share.loadShareFromCommonData(false);
                        share.appendTodayCandle2();
                        
                        stCandle c0 = share.getCandleByDate(startDate, 0, _c0);
                        stCandle c1 = share.getCandleByDate(endDate, startDate, _c1);
                        
                        if (c0 == null || c1 == null){
                            continue;
                        }
                        if (c0.close == 0 || c1.close == 0){
                            continue;
                        }
                        
                        totalEquity += (inf.volume*c0.close);

                        double changedPercent = (c1.close-c0.close)/c0.close;
                        if (changedPercent < 0){
                            //changedPercent = changedPercent;
                        }
                        indexOfShares[i] = cIndex.close * (inf.volume * c0.close)*changedPercent;
                        
                        if (changedPercent > 0){
                            totalIndexInc += changedPercent;
                        }
                        else{
                            totalIndexDec += changedPercent;
                        }
                        
                        //---------
                        changedPercents[i] = (c1.close-c0.close)*100/c0.close;
                        prices[i] = c1.close;
                    }
                }
            }
            
            //  cleanup
            vChangeDec.removeAllElements();
            vChangeInc.removeAllElements();
            //====================================
            totalIndexInc /= totalEquity;
            totalIndexDec /= totalEquity;
            
            totalIndexInc = 0;
            totalIndexDec = 0;
            
            for (int i = 0; i < cnt; i++)
            {
                Share share = mContext.mShareManager.getVnindexShareAt(i);
                
                //if (strcmp(share.mCode, "VIC") == 0){
                //            /i = i;
                //}
                
                double indexChanged = indexOfShares[i] / totalEquity;
                
                ChangeIndexItem item;
                if (indexChanged > 0){
                    item = new ChangeIndexItem();
                    item.modifiedValue = (float)indexChanged;
                    
                    totalIndexInc += item.modifiedValue;
                    
                    item.shareID = share.getShareID();
                    
                    vChangeInc.addElement(item);
                }
                else{
                    item = new ChangeIndexItem();
                    item.modifiedValue = (float)(-indexChanged);
                    
                    totalIndexDec += item.modifiedValue;
                    
                    item.shareID = share.getShareID();
                    
                    vChangeDec.addElement(item);
                    
                }
                item.price = prices[i];
                item.modifiedPercent = (float)changedPercents[i];
            }
            
            //  end of calc
            sortChanges(vChangeInc);
            sortChanges(vChangeDec);
            
            while (vChangeInc.size() > 200){
                vChangeInc.removeElementAt(vChangeInc.size()-1);
            }
            while (vChangeDec.size() > 200){
                vChangeDec.removeElementAt(vChangeDec.size()-1);
            }
            
            //---------------
            //  ChangeIndexItem
            xVector[] vv = {vChangeInc, vChangeDec};
            for (int j = 0; j < 2; j++){
                for (int i = 0; i < vv[j].size(); i++){
                    ChangeIndexItem item = (ChangeIndexItem)vv[j].elementAt(i);
                    Share share = mContext.mShareManager.getShare(item.shareID);
                    if (share != null && share.getCode() != null){
                        group.addCode(share.getCode());
                    }
                }
            }

            invalidate();
        }

        void refreshChart2()
        {
            if (group == null)
            {
                group = new stShareGroup();
                group.setType(stShareGroup.ID_GROUP_DEFAULT);
            }

            group.clear();

            currentSelected = null;

            Share index = mContext.mShareManager.getShare(mShareID);
            stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(index.getMarketID());
            if (index == null || pi == null)
            {
                return;
            }

            if (mContext.mShareManager.getVnindexCnt() == 0)
            {
                return;
            }

            _isProcessing = true;

            index.loadShareFromFile(false);

            stCandle cIndex = new stCandle();
            index.getCandleByDate(startDate, 0, cIndex);
            //===============================================
            double totalEquity = 0.0;
            totalIndexInc = 0;
            totalIndexDec = 0;

            int cnt = mContext.mShareManager.getVnindexCnt();

            double[] indexOfShares = new double[cnt];
            double[] changedPercents = new double[cnt];
            float[] prices = new float[cnt];

            stCandle _c0 = new stCandle();
            stCandle _c1 = new stCandle();
            //  tinh total equity
            for (int i = 0; i < cnt; i++)
            {
                Share share = mContext.mShareManager.getVnindexShareAt(i);
                if (share.getShareID() == 223)
                {
                    indexOfShares[i] = 1;
                }
                indexOfShares[i] = 0;

                if (share.getMarketID() == index.getMarketID() && !share.isIndex())
                {
                    stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(share.getShareID());
                    if (inf != null)
                    {
                        share.loadShareFromCommonData(false);
                        share.appendTodayCandle2(); //  use last price if today is not trade

                        stCandle c0 = share.getCandleByDate(startDate, 0, _c0);
                        stCandle c1 = share.getCandleByDate(endDate, startDate, _c1);

                        if (c0 == null || c1 == null)
                        {
                            continue;
                        }
                        if (c0.close == 0 || c1.close == 0)
                        {
                            continue;
                        }

                        totalEquity += (inf.volume * c0.close);

                        double changedPercent = (c1.close - c0.close) / c0.close;
                        if (changedPercent < 0)
                        {
                            //changedPercent = changedPercent;
                        }
                        indexOfShares[i] = cIndex.close * (inf.volume * c0.close) * changedPercent;

                        if (changedPercent > 0)
                        {
                            totalIndexInc += changedPercent;
                        }
                        else
                        {
                            totalIndexDec += changedPercent;
                        }

                        //---------
                        changedPercents[i] = (c1.close - c0.close) * 100 / c0.close;
                        prices[i] = c1.close;
                    }
                }
            }

            //  cleanup
            vChangeDec.removeAllElements();
            vChangeInc.removeAllElements();
            //====================================
            totalIndexInc /= totalEquity;
            totalIndexDec /= totalEquity;

            totalIndexInc = 0;
            totalIndexDec = 0;

            for (int i = 0; i < cnt; i++)
            {
                Share share = mContext.mShareManager.getVnindexShareAt(i);

                //if (strcmp(share.mCode, "VIC") == 0){
                //            /i = i;
                //}

                double indexChanged = indexOfShares[i] / totalEquity;

                ChangeIndexItem item;
                if (indexChanged > 0)
                {
                    item = new ChangeIndexItem();
                    item.modifiedValue = (float)indexChanged;

                    totalIndexInc += item.modifiedValue;

                    item.shareID = share.getShareID();

                    vChangeInc.addElement(item);
                }
                else
                {
                    item = new ChangeIndexItem();
                    item.modifiedValue = (float)(-indexChanged);

                    totalIndexDec += item.modifiedValue;

                    item.shareID = share.getShareID();

                    vChangeDec.addElement(item);

                }
                item.price = prices[i];
                item.modifiedPercent = (float)changedPercents[i];
            }

            //  end of calc
            sortChanges(vChangeInc);
            sortChanges(vChangeDec);

            while (vChangeInc.size() > 200)
            {
                vChangeInc.removeElementAt(vChangeInc.size() - 1);
            }
            while (vChangeDec.size() > 200)
            {
                vChangeDec.removeElementAt(vChangeDec.size() - 1);
            }

            //---------------
            //  ChangeIndexItem
            xVector[] vv = { vChangeInc, vChangeDec };
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < vv[j].size(); i++)
                {
                    ChangeIndexItem item = (ChangeIndexItem)vv[j].elementAt(i);
                    Share share = mContext.mShareManager.getShare(item.shareID);
                    if (share != null && share.getCode() != null)
                    {
                        group.addCode(share.getCode());
                    }
                }
            }

            invalidate();
        }

        public override void onMouseUp(int x, int y)
        {
            //  ChangeIndexItem**
            xVector[] vv = {vChangeInc, vChangeDec};
            Share share;
    
            for (int j = 0; j < 2; j++)
            {
                xVector v = vv[j];
                
                for (int i = 0; i < v.size(); i++)
                {
                    ChangeIndexItem item = (ChangeIndexItem)v.elementAt(i);
                    if (item.w > 0)
                    {
                        if (Utils.isInRect(x, y, (int)item.x, (int)item.y, (int)item.w, (int)item.h))
                        {
                            if (item.selected)
                            {
                                item.selected = false;
                                share = mContext.mShareManager.getShare(item.shareID);
                                mContext.setCurrentShare(share);
                                
                                /*
                                C2ObjObject *objShare = [[[C2ObjObject alloc]initWithPointer:share]autorelease];
                                C2ObjObject *objGroup = [[C2ObjObject alloc]initWithPointer:group];
                                NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:
                                                      objShare, @"share",
                                                      objGroup, @"group", nil];
                                
                                notifyEvent(EVT_SHOW_SHARE_HISTORY, 0, dict);
                                return;
                                 * */
                            }
                            
                            if (currentSelected != null){
                                currentSelected.selected = false;
                            }
                            
                            item.selected = true;
                            share = mContext.mShareManager.getShare(item.shareID);
                            mContext.setCurrentShare(share);
                            /*
                            notifyEvent(EVT_SELECT_SHARE, 0, share);
                            */
                            currentSelected = item;
                            invalidate();
                            
                            return;
                        }
                    }
                }
            }

        }
    }

    

}
