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
    public class stStatisticsBlock
    {
        public float x, y;
        public float w, h;
        public string code;
        public uint color;
        public float point;    //  for sort
        public string inf;
        public string changed;
        public double inf2;
    }

    public class ChartStatistics: xBaseControl
    {        
        static public int BUBBLE_TRADE_MONEY = 0;
        static public int BUBBLE_VOLUME_WEIGHT = 1;
        static public int BUBBLE_INDEX_WEIGHT_RATIO_INC = 2;
        static public int BUBBLE_INDEX_WEIGHT_RATIO_DEC = 3;

        Context mContext;

        int mMarketID;

        xVector mFreeBlocks = new xVector();    //  <stStatisticsBlock*>
        xVector mUsedBlocks = new xVector();    //  <stStatisticsBlock*>

        float rx;
        float ry;
        int mChartType;
        //==========================================
        public ChartStatistics(int marketID, xIEventListener listener)
            : base(listener)
        {
            makeCustomRender(true);

            mContext = Context.getInstance();
            mMarketID = marketID;

            for (int i = 0; i < 1000; i++)
            {
                stStatisticsBlock b = new stStatisticsBlock();
                mFreeBlocks.addElement(b);
            }
        }

        bool isProcessing = false;
        public void doCalcChanged()
        {
            if (isProcessing)
            {
                return;
            }
            isProcessing = true;
            invalidate();

            /*
            utils.AsyncUtils.DelayCall(250, () =>
            {
                refreshChart(mMarketID);

                isProcessing = false;
            });
             */
            
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
             {
                 System.Threading.Thread.Sleep(100);

                 refreshChart(mMarketID);

                 isProcessing = false;
                 invalidate();
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
            //  title
            String title = "";
            String prefix = mMarketID == 1 ? "[VNX] " : "[HNX] ";
            if (mChartType == BUBBLE_TRADE_MONEY)
            {
                title = prefix + "Luồng tiền (Mã; Thay đổi giá; Giá trị GD)";
            }
            else if (mChartType == BUBBLE_VOLUME_WEIGHT)
            {
                title = prefix + "Mức độ ảnh hưởng (Mã; Thay đổi giá; Trọng số)";
            }
            else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC)
            {
                title = prefix + "Tăng Index (Mã; Thay đổi giá; Thay đổi điểm)";
            }
            else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
            {
                title = prefix + "Giảm Index (Mã; Thay đổi giá; Thay đổi điểm)";
            }
            Font f = mContext.getFontText();
            g.setColor(C.COLOR_WHITE);
            g.drawString(f, title, 10, 2, xGraphics.TOP | xGraphics.LEFT);

            for (int i = 0; i < mUsedBlocks.size(); i++)
            {
                stStatisticsBlock b = (stStatisticsBlock)mUsedBlocks.elementAt(i);
                if (b.w > 0)
                {
                    drawBlock(g, b);
                }
            }
        }

        private void refreshChart(int martketID)
        {
            //  block data
            int total = 1000;
            int i = 0;

            if (getW() == 0 || getH() == 0)
            {
                return;
            }

            for (i = 0; i < mUsedBlocks.size(); i++)
            {
                stStatisticsBlock b = (stStatisticsBlock)mUsedBlocks.elementAt(i);
                mFreeBlocks.addElement(b);
            }
            //  reset
            mUsedBlocks.removeAllElements();
            //--------------------------------------
            int shareCount = mContext.mShareManager.getTotalShareIDCount();
            int[] market = { 0 };

            double totalEquity = 0.0f;
            double totalIndexInc = 0;
            double totalIndexDec = 0;


            stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(martketID);

            //  calc totalEquity
            for (i = 0; i < shareCount; i++)
            {
                int shareID = mContext.mShareManager.getShareIDAt(i, market);
                stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(shareID);
                if (inf != null && inf.floor == martketID)
                {
                    stPriceboardState priceboard = mContext.mPriceboard.getPriceboard(inf.shareID);
                    if (priceboard != null)
                    {
                        double ff = priceboard.getCurrentPrice();  //  avoiding of overstack
                        if (mChartType == BUBBLE_TRADE_MONEY)
                        {
                            totalEquity += ff * priceboard.getTotalVolume();
                        }
                        else if (mChartType == BUBBLE_VOLUME_WEIGHT
                            || mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC
                            || mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
                        {
                            totalEquity += ((double)inf.volume * ff);
                        }
                    }
                }
            }
            //  extra
            for (i = 0; i < shareCount; i++)
            {
                int shareID = mContext.mShareManager.getShareIDAt(i, market);
                stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(shareID);
                if (inf != null && inf.floor == martketID)
                {
                    stPriceboardState priceboard = mContext.mPriceboard.getPriceboard(inf.shareID);
                    if (priceboard != null)
                    {
                        double ff = priceboard.getCurrentPrice();  //  avoiding of overstack
                        if (mChartType == BUBBLE_TRADE_MONEY)
                        {   
                        }
                        else if (mChartType == BUBBLE_VOLUME_WEIGHT
                            || mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC
                            || mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
                        {
                            if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC && priceboard.change > 0)
                            {
                                double indexPointOfShare = pi.current_point * ((inf.volume * ff) / totalEquity);
                                double indexChanged = priceboard.change * indexPointOfShare / priceboard.current_price_1;
                                totalIndexInc += 1.2f*indexChanged;
                            }
                            else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC && priceboard.change < 0)
                            {
                                double indexPointOfShare = pi.current_point * ((inf.volume * ff) / totalEquity);
                                double indexChanged = priceboard.change * indexPointOfShare / priceboard.current_price_1;
                                totalIndexDec += -1.2f*indexChanged;
                            }
                        }
                    }
                }
            }
            if (totalEquity == 0.0)
                return;

            double tmp = 0;
            StringBuilder sb = new StringBuilder();
            //  now calc blocks
            for (i = 0; i < shareCount; i++)
            {
                int shareID = mContext.mShareManager.getShareIDAt(i, market);
                stCompanyInfo inf = mContext.mShareManager.getCompanyInfo(shareID);
                if (inf != null && inf.floor == martketID)
                {
                    stPriceboardState priceboard = mContext.mPriceboard.getPriceboard(inf.shareID);//martketID, share.mShareID);            
                    if (priceboard != null)
                    {
                        String code = mContext.mShareManager.getShareCode(inf.shareID);
                        //if (code != null && (code.CompareTo("MSN") == 0 || code.CompareTo("AAM") == 0))
                        //{
                            //Utils.trace("aaa");
                        //}
                        double modifierValue = 0.0f;
                        double ff = priceboard.getCurrentPrice();  //  avoiding of overstack

                        //String log = String.Format("{0}, {1}, {2}, {3}", code, priceboard.current_price_1, inf.volume, totalEquity);
                        //Utils.trace(log);

                        float threshold = 0.003f;
                        if (mChartType == BUBBLE_TRADE_MONEY)
                        {
                            modifierValue = ff * priceboard.getTotalVolume();
                        }
                        else if (mChartType == BUBBLE_VOLUME_WEIGHT){
                            modifierValue = ((double)inf.volume * ff);
                        }
                        else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC)
                        {
                            double indexPointOfShare = pi.current_point * ((inf.volume * ff) / totalEquity);
                            double indexChanged = priceboard.change * indexPointOfShare / priceboard.current_price_1;
                            modifierValue = indexChanged;
                            threshold = 0.000001f;
                        }
                        else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
                        {
                            double indexPointOfShare = pi.current_point * ((inf.volume * ff) / totalEquity);
                            double indexChanged = priceboard.change * indexPointOfShare / priceboard.current_price_1;
                            modifierValue = -indexChanged;
                            threshold = 0.000001f;
                        }
                        float percent = (float)(modifierValue / totalEquity);   //  <= 1
                        if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC
                            || mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
                        {
                            percent = (float)(modifierValue/(double)pi.current_point);
                        }

                        //String log = String.Format("{0}, {1}, {2}, {3}, {4}, {5}", code, priceboard.current_price_1, inf.volume
                            //, totalEquity, modifierValue, pi.current_point);
                        //Utils.trace(log);
                        if (percent >= threshold && mFreeBlocks.size() > 0)
                        {
                            //  big enough to care
                            float value = percent;
                            if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC)
                            {
                                percent *= 100;// (float)(equity / totalIndexInc);
                            }
                            else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
                            {
                                percent *= 100;// (float)(equity / totalIndexDec);
                            }

                            stStatisticsBlock block = (stStatisticsBlock)mFreeBlocks.pop();
                            block.code = mContext.mShareManager.getShareCode(inf.shareID);;
                            //  w & h
                            //block.w = (short)Math.Sqrt(cells);
                            //block.h = block.w;
                            //if (block.h * block.w < cells) block.w++;

                            //if (block.h >= 50)
                            //{
                                //int k = 0;
                            //}

                            //  color
                            uint color = C.COLOR_YELLOW;
                            float price = priceboard.getCurrentPrice();
                            float reference = priceboard.getRef();
                            if (price == reference) color = C.COLOR_YELLOW;
                            else if (price == priceboard.getCe()) color = C.COLOR_MAGENTA;
                            else if (price == priceboard.getFloor()) color = C.COLOR_CYAN;
                            else if (price < reference)
                            {
                                int r = (int)((reference - price) * 100 / reference);    //  percent
                                if (r < 8)
                                    r = (0xff - 150) + r * 30;
                                else r = 0xff;
                                if (r > 0xff) r = 0xff;
                                //---------------
                                r = 0xff;

                                color = (uint)((0xff << 24) | (r << 16));
                            }
                            else if (price > reference)
                            {
                                int g = (int)((price - reference) * 100 / reference);    //  percent
                                if (g < 8)
                                    g = (0xff - 150) + g * 30;
                                else g = 0xff;
                                if (g > 0xff) g = 0xff;
                                //----------------
                                g = 0xff;
                                color = (uint)((0xff << 24) | (g << 8));
                            }
                            block.color = color;
                            //  point
                            block.point = (float)(percent * 1000);//(priceboard.current_price_1 - priceboard.ref)*1000/priceboard.ref;
                            //  x & y
                            //setBlockPosition(block);

                            //  extra inf

                            sb.Length = 0;
                            if (mChartType == BUBBLE_TRADE_MONEY)
                            {
                                sb.AppendFormat("{0:F1}tỉ", (modifierValue / 1000000));   //  money unit = 100 vnd
                            }
                            else if (mChartType == BUBBLE_VOLUME_WEIGHT)
                            {
                                sb.AppendFormat("{0:F1}%", (percent * 100));
                            }
                            else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC)
                            {
                                double r = (inf.volume * price) / totalEquity;
                                //  totalE = pi.point
                                //  vonhoa = ?


                                //  index-point of code: (r*pi.point);
                                //  price <> (r*pi.point)
                                //  changed <> ?
                                double indexChanged = (priceboard.change * r * pi.current_point) / priceboard.current_price_1;
                                tmp += indexChanged;
                                sb.AppendFormat("{0:F4}", (indexChanged));
                            }
                            else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
                            {
                                double r = (inf.volume * price) / totalEquity;
                                double indexChanged = (priceboard.change * r * pi.current_point) / priceboard.current_price_1;

                                sb.AppendFormat("{0:F4}", (indexChanged));
                            }
                            block.inf = sb.ToString();
                            if (priceboard.getCode() == "GAS")
                            {
                                int k = 0;
                            }
                            //  changed
                            sb.Length = 0;
                            if (reference != 0)
                            {
                                float changed = price - reference;
                                if (changed > 0)
                                    sb.AppendFormat("({0:F2}; +{1:F2}%)", price, (float)((changed*100) / reference));
                                else
                                    sb.AppendFormat("({0:F2}; {1:F2}%)", price, (float)((changed*100) / reference));
                            }
                            block.changed = sb.ToString();

                            //String log = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", code, priceboard.current_price_1, inf.volume
                            //, totalEquity, modifierValue, pi.current_point, mUsedBlocks.size());
                            //Utils.trace(log);
                            mUsedBlocks.addElement(block);
                        }
                    }
                }
            }// end of for
            
            sortBlocks();
        }

       
        public void setChartType(int type)
        {
            mChartType = type;

            //refreshChart(mMarketID);
        }

        override public void invalidate()
        {
            //doCalcChanged();

            base.invalidate();
        }

        void sortBlocks()
        {
            //  sort blocks
            for (int i = 0; i < mUsedBlocks.size()-1; i++)
            {
                stStatisticsBlock b1 = (stStatisticsBlock)mUsedBlocks.elementAt(i);
                int biggestIdx = i;
                for (int j = i + 1; j < mUsedBlocks.size(); j++)
                {
                    stStatisticsBlock b2 = (stStatisticsBlock)mUsedBlocks.elementAt(j);
                    if (b1.point < b2.point)
                    {
                        b1 = b2;
                        biggestIdx = j;
                    }
                }

                if (i != biggestIdx)
                {
                    mUsedBlocks.swap(i, biggestIdx);
                }
            }
            //----------------------

            float w = (getW() - 8)/2;
            float h = getH();

            short y0 = 22;

            float itemH = 20;
            int totalItem = (int)((h - y0)/ itemH);
            if (totalItem < mUsedBlocks.size())
            {
                itemH = 17;
                totalItem = (int)((h - y0) / itemH);
            }

            //  biggest item
            float biggestValue = 0.0f;
            for (int i = 0; i < mUsedBlocks.size() - 1; i++)
            {
                stStatisticsBlock b = (stStatisticsBlock)mUsedBlocks.elementAt(i);
                if (b.point > biggestValue) biggestValue = b.point;
            }

            if (biggestValue == 0)
            {
                return;
            }

            int x1 = 4;
            int k = 0;
            for (int i = 0; i < mUsedBlocks.size() - 1; i++)
            {
                if ((y0 + (k+1) * itemH) > getH())
                {
                    if (x1 > getW() / 2)
                    {
                        break;
                    }
                    x1 = 4 + getW() / 2;
                    k = 0;
                }

                stStatisticsBlock b = (stStatisticsBlock)mUsedBlocks.elementAt(i);
                b.x = (float)x1;
                b.y = (float)(y0 + (k * itemH));
                b.w = (float)(w*b.point) / biggestValue;
                b.h = itemH;

                k++;
            }

            base.invalidate();
        }

        void drawBlock(xGraphics g, stStatisticsBlock block)
        {
            int viewW = getW() - 4;

            uint alp = 0xa0ffffff;

            g.setColor(block.color & alp);
            g.fillRect((int)block.x, (int)block.y, (int)block.w, (int)block.h - 1);
            if (block.x + block.w < getW() - 4)
            {
                g.setColor(0xff202020);
                int x1 = (int)(block.x + block.w);
                g.fillRect(x1, (int)block.y, (int)(getW()-4-x1),  (int)block.h - 1);
            }

            Font f = mContext.getFontSmallest();
            g.setColor(C.COLOR_WHITE);

            int itemH = (int)f.GetHeight();

            String s = String.Format("{0}; {1}; {2}", block.code, block.changed, block.inf);

            g.drawStringInRect(f, s, (int)block.x + 4, (int)block.y + 3, getW()/2, (int)block.h, xGraphics.VCENTER);
            //g.drawStringInRect(f, block.inf, x, y + h / 2 - 8, w, itemH, xGraphics.HCENTER | xGraphics.TOP);
            //g.drawStringInRect(f, block.changed, x, y + h / 2 + 4, w, itemH, xGraphics.HCENTER | xGraphics.TOP);
        }

        Rectangle getBlockRect(stStatisticsBlock block)
        {
            float x = block.x;
            float y = block.y;
            float w = (int)block.w;
            float h = (int)block.h;

            Rectangle rc = new Rectangle((int)x, (int)y, (int)w, (int)h);

            return rc;
        }

        override public void onMouseDoubleClick(int x, int y)
        {
            for (int i = 0; i < mUsedBlocks.size(); i++)
            {
                stStatisticsBlock b = (stStatisticsBlock)mUsedBlocks.elementAt(i);

                Rectangle rc = getBlockRect(b);
                if (rc.X < x && x < rc.X + rc.Width
                    && rc.Y < y && y < rc.Y + rc.Height)
                {
                    Share share = mContext.mShareManager.getShare(b.code);
                    if (share != null)
                    {
                        mListener.onEvent(this, xBaseControl.EVT_ON_MOUSE_DOUBLE_CLICK, C.ID_SELECT_SHARE_CANDLE, share);
                        break;
                    }
                }
            }
        }
    }
}
