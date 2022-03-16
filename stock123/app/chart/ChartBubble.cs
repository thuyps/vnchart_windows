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
    public class stBubbleBlock
    {
        public int x, y;
        public int w, h;
        public string code;
        public uint color;
        public int point;    //  for sort
        public string inf;
        public string changed;
        public double inf2;
    }

    public class ChartBubble: xBaseControl
    {        
        static public int BUBBLE_TRADE_MONEY = 0;
        static public int BUBBLE_VOLUME_WEIGHT = 1;
        static public int BUBBLE_INDEX_WEIGHT_RATIO_INC = 2;
        static public int BUBBLE_INDEX_WEIGHT_RATIO_DEC = 3;

        Context mContext;
        const int mMapW = 70;
        const int mMapH = 70;
        short[] mMap = new short[mMapW*mMapH];


        int mMarketID;

        xVector mFreeBlocks = new xVector();    //  <stBubbleBlock*>
        xVector mUsedBlocks = new xVector();    //  <stBubbleBlock*>

        float rx;
        float ry;
        int mChartType;
        //==========================================
        public ChartBubble(int marketID, xIEventListener listener)
            : base(listener)
        {
            makeCustomRender(true);

            mContext = Context.getInstance();
            mMarketID = marketID;

            for (int i = 0; i < 1000; i++)
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
            String title = "";
            if (mChartType == BUBBLE_TRADE_MONEY)
            {
                title = "Đồ thị minh họa luồng tiền (Mã / Giá trị GD / Thay đổi giá)";
            }
            else if (mChartType == BUBBLE_VOLUME_WEIGHT)
            {
                title = "Đồ thị minh họa mức độ ảnh hưởng (Mã / Trọng số / Thay đổi giá)";
            }
            else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC)
            {
                title = "Tốp mã làm tăng thị trường (Mã / Thay đổi index / Thay đổi giá)";
            }
            else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
            {
                title = "Tốp mã làm giảm thị trường (Mã / Thay đổi index / Thay đổi giá)";
            }
            Font f = mContext.getFontText();
            g.setColor(C.COLOR_WHITE);
            g.drawString(f, title, 10, 2, xGraphics.TOP | xGraphics.LEFT);

            for (int i = 0; i < mUsedBlocks.size(); i++)
            {
                stBubbleBlock b = (stBubbleBlock)mUsedBlocks.elementAt(i);
                drawBlock(g, b);
            }
        }

        public void refreshChart(int martketID)
        {
            rx = (float)(getW() - 10) / mMapW;
            ry = (float)(getH() - 15) / mMapH;
            //  block data
            int total = 1000;
            int i = 0;

            for (i = 0; i < mUsedBlocks.size(); i++)
            {
                stBubbleBlock b = (stBubbleBlock)mUsedBlocks.elementAt(i);
                b.x = 0;
                b.y = 0;
                mFreeBlocks.addElement(b);
            }
            //  reset
            mUsedBlocks.removeAllElements();
            for (i = 0; i < mMapH; i++)
            {
                for (int j = 0; j < mMapW; j++)
                {
                    mMap[map(i,j)] = 0;
                }
            }

            //--------------------------------------
            //xVector v = mContext.mShareManager.getCompanyInfos();
            double totalEquity = 0.0f;
            double totalIndexInc = 0;
            double totalIndexDec = 0;


            stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(martketID);

            int shareCount = mContext.mShareManager.getTotalShareIDCount();
            //  calc totalEquity
            int[] market = { 0 };
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

            double squareOfCell = 1.0 / (mMapW * mMapH);
            double tmp = 0;
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
                        if (code != null && (code.CompareTo("MSN") == 0 || code.CompareTo("MBB") == 0))
                        {
                            Utils.trace("aaa");
                        }
                        double equity = 0.0f;
                        double ff = priceboard.getCurrentPrice();  //  avoiding of overstack
                        float threshold = 0.003f;
                        if (mChartType == BUBBLE_TRADE_MONEY)
                        {
                            equity = ff * priceboard.getTotalVolume();
                        }
                        else if (mChartType == BUBBLE_VOLUME_WEIGHT){
                            equity = ((double)inf.volume * ff);
                        }
                        else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC)
                        {
                            double indexPointOfShare = pi.current_point * ((inf.volume * ff) / totalEquity);
                            double indexChanged = priceboard.change * indexPointOfShare / priceboard.current_price_1;
                            equity = indexChanged;
                            threshold = 0.00001f;
                        }
                        else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
                        {
                            double indexPointOfShare = pi.current_point * ((inf.volume * ff) / totalEquity);
                            double indexChanged = priceboard.change * indexPointOfShare / priceboard.current_price_1;
                            equity = -indexChanged;
                            threshold = 0.00001f;
                        }
                        float percent = (float)(equity / totalEquity);   //  <= 1
                        if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC
                            || mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
                        {
                            percent = (float)(equity/(double)pi.current_point);
                            //percent = (float)(equity/totalEquityOfUsedBlock);
                        }
                        if (percent >= threshold && mFreeBlocks.size() > 0)
                        {
                            //  big enough to care
                            float percentR = percent;
                            if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_INC)
                            {
                                percentR = (float)(equity / totalIndexInc);
                            }
                            else if (mChartType == BUBBLE_INDEX_WEIGHT_RATIO_DEC)
                            {
                                percentR = (float)(equity / totalIndexDec);
                            }

                            float square = percentR * (mMapH * mMapW);

                            float cells = square;
                            if (cells < 1) cells = 1;

                            stBubbleBlock block = (stBubbleBlock)mFreeBlocks.pop();
                            block.code = mContext.mShareManager.getShareCode(inf.shareID);;
                            //  w & h
                            block.w = (int)Math.Sqrt(cells);
                            block.h = block.w;
                            if (block.h * block.w < cells) block.w++;

                            if (block.h >= 50)
                            {
                                int k = 0;
                            }

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
                            block.point = (int)(percent * 1000);//(priceboard.current_price_1 - priceboard.ref)*1000/priceboard.ref;
                            //  x & y
                            //setBlockPosition(block);

                            //  extra inf
                            StringBuilder sb = Utils.sb;
                            sb.Length = 0;
                            if (mChartType == BUBBLE_TRADE_MONEY)
                            {
                                sb.AppendFormat("{0:F1}tỉ", (equity / 1000000));   //  money unit = 100 vnd
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
                            if (priceboard.getCode() == "ACB")
                            {
                                int k = 0;
                            }
                            //  changed
                            sb.Length = 0;
                            if (reference != 0)
                            {
                                float changed = price - reference;
                                if (changed > 0)
                                    sb.AppendFormat("+{0:F2}%", (float)((changed*100) / reference));
                                else
                                    sb.AppendFormat("{0:F2}%", (float)((changed*100) / reference));
                            }
                            block.changed = sb.ToString();

                            mUsedBlocks.addElement(block);
                        }
                    }
                }
            }// end of for
            
            sortBlocks();
        }

        void setBlockPosition(stBubbleBlock block)
        {
            int[] pos = { 0, 0 };    //  hang | cot
            for (int i = 0; i < mMapH; i++)
            {
                if (i == 2)
                {
                    int k = 0;
                }
                for (int j = 0; j < mMapW; j++)
                {
                    if (canPlaceBlockAt(block, i, j))
                    {
                        if (i >= pos[0])
                        {
                            if (i == pos[0])
                            {
                                if (j < pos[1])
                                {
                                    pos[0] = i;
                                    pos[1] = j;
                                }
                            }
                            else
                            {
                                pos[0] = i;
                                pos[1] = j;
                            }
                        }
                    }
                }
            }
            block.x = (int)pos[1];
            block.y = (int)pos[0];

            for (int i = 0; i < block.h; i++)
            {
                for (int j = 0; j < block.w; j++)
                {
                    if (block.y + i >= mMapH || block.x + j >= mMapW)
                    {
                        int k = 0;
                        return;
                    }
                    mMap[map(block.y + i, block.x + j)] = 1;
                }
            }
        }

        bool canPlaceBlockAt(stBubbleBlock block, int r, int c)
        {
            if (r + block.h < mMapH && c + block.w < mMapW)
            {
                bool hasObj = false;
                for (int i = r; i < r + block.h; i++)
                {
                    for (int j = c; j < c + block.w; j++)
                    {
                        if (mMap[map(i,j)] == 1)
                        {
                            hasObj = true;
                            break;
                        }
                    }
                }
                return hasObj == false;
            }

            return false;
        }

        int map(int i, int j)
        {
            return i * mMapW + j;
        }

        Rectangle getBlockRect(stBubbleBlock block)
        {
            int x = (int)(rx * block.x + 5);
            int y = (int)(ry * block.y + 12);
            int w = (int)(rx * block.w);
            int h = (int)(ry * block.h);

            Rectangle rc = new Rectangle(x, y, w, h);

            return rc;
        }

        void drawBlock(xGraphics g, stBubbleBlock block)
        {
            Rectangle rc = getBlockRect(block);
            int x = rc.X;
            int y = rc.Y;
            int w = rc.Width;
            int h = rc.Height;

            uint alp = 0xd0ffffff;

            g.setColor(block.color & alp);
            g.fillRect(x, y, w - 1, h - 1);

            Font f = mContext.getFontSmallest();
            g.setColor(C.COLOR_BLACK);

            int itemH = (int)f.GetHeight();

            if (h > 35)
            {
                g.drawStringInRect(f, block.code, x, y + h / 2 - 20, w, itemH, xGraphics.HCENTER | xGraphics.TOP);
                g.drawStringInRect(f, block.inf, x, y + h / 2 - 8, w, itemH, xGraphics.HCENTER | xGraphics.TOP);
                g.drawStringInRect(f, block.changed, x, y + h / 2 + 4, w, itemH, xGraphics.HCENTER | xGraphics.TOP);
            }
            else if (h > 20)
            {
                g.drawStringInRect(f, block.code, x, y + h / 2 - 12, w, itemH, xGraphics.HCENTER | xGraphics.TOP);
                g.drawStringInRect(f, block.inf, x, y + h / 2 - 2, w, itemH, xGraphics.HCENTER | xGraphics.TOP);
            }
            else
            {
                g.drawStringInRect(f, block.code, x, y, w, h, xGraphics.HCENTER | xGraphics.TOP);
                if (h > 18)
                    g.drawStringInRect(f, block.inf, x, y + 11, w, itemH, xGraphics.HCENTER | xGraphics.TOP);
            }
        }

        void sortBlocks()
        {
            xVector v = new xVector(mUsedBlocks.size());
            if (mUsedBlocks.size() < 3)
                return;
            int i = 0;
            for (i = 0; i < mUsedBlocks.size(); i++)
            {
                v.addElement(mUsedBlocks.elementAt(i));
            }
            mUsedBlocks.removeAllElements();
            int smallest = -10000;
            for (i = 0; i < v.size() - 1; i++)
            {
                stBubbleBlock b0 = (stBubbleBlock)v.elementAt(i);
                stBubbleBlock b1;
                smallest = i;
                for (int j = i + 1; j < v.size(); j++)
                {
                    b1 = (stBubbleBlock)v.elementAt(j);
                    if (b1.point < b0.point)
                    {
                        smallest = j;
                        b0 = b1;
                    }
                }
                //  swap i <=> smallest
                v.swap(i, smallest);
            }

            for (i = v.size() - 1; i >= 0; i--)
            {
                stBubbleBlock b = (stBubbleBlock)v.elementAt(i);

                //printf("\n===========POINT: %d", b.point);

                setBlockPosition(b);
                mUsedBlocks.addElement(b);
            }
        }

        public void setChartType(int type)
        {
            mChartType = type;

            refreshChart(mMarketID);
        }

        override public void invalidate()
        {
            refreshChart(mMarketID);

            base.invalidate();
        }

        override public void onMouseDoubleClick(int x, int y)
        {
            for (int i = 0; i < mUsedBlocks.size(); i++)
            {
                stBubbleBlock b = (stBubbleBlock)mUsedBlocks.elementAt(i);

                Rectangle rc = getBlockRect(b);
                if (rc.X < x && x < rc.X + rc.Width
                    && rc.Y < y && rc.Y < rc.Y + rc.Height)
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
