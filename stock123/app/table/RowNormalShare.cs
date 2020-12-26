using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using xlib.framework;
using xlib.ui;
using xlib.utils;

using stock123.app.data;

namespace stock123.app.table
{
    class RowNormalShare: xBaseControl
    {
        protected TablePriceboard mParent;
        protected xVector mCells = new xVector(10);
        //long mHoldingTimeStart;
        protected bool mIsSelected = false;
        //bool mIsRejectTouchUp = false;
        //bool mAllowDelete = false;
        public int mCandleColumeIdx = 1;

        protected const int COLOR_NONE = 1;

        public RowNormalShare(xIEventListener listener, int _id, int w, int h)
            :base(listener)
        {
            makeCustomRender(true);

            setID(_id);

            setSize(w, h);

            createRow(_id, w, h);
        }

        public void setParent(TablePriceboard parent)
        {
            mParent = parent;
        }

        virtual protected void createRow(int _id, int w, int h)
        {
            //==================================
            Context ctx = Context.getInstance();
            Font f = ctx.getFontSmallB();

            if (_id >= 0)
            {
                uint BG_GRAY = 0x90606060;
                uint BG0 = COLOR_NONE;
                if (_id >= 0 && (_id % 2) == 0)
                {
                    BG_GRAY += 0x0050505;
                    BG0 = 0x90202020;
                }

                f = Context.getInstance().getFontSmallB();// getFontTextB();
                Font fsmallB = Context.getInstance().getFontSmallB();
                Font fsmall = Context.getInstance().getFontSmall();
                Font fnormal = Context.getInstance().getFontText();

                Font fSymbol = new Font(new FontFamily("Arial"), 9.0f, FontStyle.Bold);
                //  code/_ref | Mua1 | Mua2 | Mua3 | Khop | +/- | Ban1 | Ban2 | Ban3 | Cao/thap | Cung/Cau TongKL
                Font[] font = {fsmallB, fsmallB,
		            				fnormal, fnormal, fnormal, 
		            				fsmallB, fsmallB, 
		            				fnormal, fnormal, fnormal, 
		            				fsmallB,        //  cao thap
		            				fsmall, 
                                    fsmallB,        /// volume
                                    f};
                float[] percents = {6.5f, 3.7f,          //  11
		                8, 8, 8.5f,    //  Du mua      =   25
		                9, 6.5f,    //  khop / +-   =   15.5   
		                8.5f, 8, 8,    //  Du ban      =   25
		                6.5f,       //  cao/thap    =   6.5
                        7.5f,       //  Du cung cau =   7.0
                        10, 2, -1};    //  Volume      =   10.5
                uint[] colors = { BG_GRAY, BG0, BG0, BG0, BG0, BG_GRAY, BG_GRAY, BG0, BG0, BG0, BG_GRAY, BG_GRAY, BG_GRAY, COLOR_NONE };
                init(w, h, percents, font, colors);
                if (_id <= 0)
                {
                }
                else
                {
                    //enableClicked();
                }
            }

            setID(_id);
        }

        protected void init(int w, int h, float[] columnPercents, Font[] f, uint[] bgcolors)
        {
            //	    mFont = f;

            setSize(w, h);

            int columnCnt = 0;
            int i = 0;
            while (columnPercents[i] != -1)
            {
                columnCnt++;
                i++;
            }

            for (i = 0; i < columnCnt; i++)
            {
                stCell c = new stCell();
                mCells.addElement(c);
            }
            //--------------------------------
            i = 0;
            short x = 0;
            short columnW = 0;
            while (columnPercents[i] != -1)
            {
                columnW = (short)(w * columnPercents[i] / 100);
                stCell c = getCellAt(i);
                c.text = "";
                c.bgColor = bgcolors[i];
                c.x = x;
                c.w = columnW;
                c.f = f[i];
                x += columnW;

                i++;
            }
        }

        protected stCell getCellAt(int idx)
        {
            if (idx < mCells.size())
                return (stCell)mCells.elementAt(idx);
            return null;
        }

        protected void setCellValue(int idx, String text, uint textColor)
        {
            stCell c = getCellAt(idx);
            if (c != null)
            {
                c.text = text;
                c.textColor = textColor;
            }
        }

        protected void setCellValue(int idx, String text, String text2, uint textColor)
        {
            stCell c = getCellAt(idx);
            if (c != null)
            {
                c.text = text;
                c.text2 = text2;
                c.textColor = textColor;
                c.textColor2 = textColor;
            }
        }

        protected void setCellBackgroundColor(int idx, uint color)
        {
            stCell c = getCellAt(idx);
            if (c != null)
            {
                c.bgColor = color;
            }
        }

        protected void setCellValue1(int idx, int v, int dev, uint textColor)
        {
            String s;

            if (v > 0)
            {
                if (dev > 0)
                {
                    float t = (float)v / dev;
                    int n = v % dev;
                    //  12334

                    s = String.Format("{0:F}", (float)v / dev);
                }
                else
                    s = String.Format("{0}", v);
            }
            else
            {
                s = "-";
            }

            //Utils.trace("--setCellValue: " + s);

            setCellValue(idx, s, textColor);
        }

        protected void addCellValue1(int idx, int v, int dev, uint color)
        {
            String s;

            if (v != 0)
            {
                if (dev > 0)
                {
                    s = String.Format("{0:F}", (float)v / dev);
                }
                else
                    s = String.Format("{0}", v);
            }
            else
            {
                s = "-";
            }

            stCell c = getCellAt(idx);
            if (c != null)
            {
                c.text2 = s;
                c.textColor2 = color;
            }
        }

        protected void addCellValue1(int idx, String s, uint color)
        {
            if (s == null) s = "-";
            stCell c = getCellAt(idx);
            if (c != null)
            {
                c.text2 = s;
                c.textColor2 = color;
            }
        }

        protected void addCellValue1(int idx, String s)
        {
            stCell c = getCellAt(idx);
            if (c != null)
            {
                c.text2 = s;
                c.textColor2 = c.textColor;
            }
        }

        override public void render(xGraphics g)
        {
            if (getID() < 0)
                return;

            int x = 0;
            int y = 2;
            int h = getH();

            if (mCells.size() == 0)
                return;

            if (mIsSelected && getID() > 0)
            {
                g.setColor(C.COLOR_BUTTON_SEL);
                g.fillRect(0, 0, getW(), getH());
            }

            int i;

            x = 0;
            for (i = 0; i < mCells.size(); i++)
            {
                stCell c = (stCell)mCells.elementAt(i);
                uint color = c.bgColor;

                int w = c.w;
                x = c.x;
                if (color != COLOR_NONE)
                {
                    g.setColor(color);
                    g.fillRect(x, 0, w, h);
                }
            }
            //---------text
            //	    printf("\n==========================================");
            for (i = 0; i < mCells.size(); i++)
            {
                stCell c = (stCell)mCells.elementAt(i);

                if (i == mCandleColumeIdx && getID() != 0)
                {
                    renderCandle(g, c);
                    continue;
                }

                if (c.text2 == null)
                {
                    g.setColor(c.textColor);
                    if (c.text != null)
                        g.drawStringInRect(c.f, c.text, c.x, 0, c.w, h, xGraphics.HCENTER | xGraphics.VCENTER);
                }
                else
                {
                    g.setColor(c.textColor);
                    g.drawStringInRect(c.f, c.text, c.x, (int)(h / 2 - c.f.GetHeight())+3, c.w, h / 2, xGraphics.HCENTER | xGraphics.VCENTER);

                    g.setColor(c.textColor2);
                    g.drawStringInRect(c.f, c.text2, c.x, h / 2+1, c.w, (int)(c.f.GetHeight()), xGraphics.HCENTER | xGraphics.VCENTER);
                }
                //Utils.trace("here 5:" + c.text + " x=" + c.x + " w=" + c.w + " h=" + h);
            }

            //	grid
            g.setColor(0xff808080);
            for (i = 1; i < mCells.size(); i++)
            {
                stCell c = (stCell)mCells.elementAt(i);
                x = c.x;
                g.drawLine(x, y, x, y + h);
            }
            g.drawHorizontalLine(0, 0, getW());

            /*
            //	history chart button
            if (getID() > 0)
            {
                g.setColor(C.COLOR_ORANGE);
                int tmp = 6;
                y = getH() / 2;
                x = getW() - tmp - 4;
                g.drawLine(x, y - tmp, x + tmp, y);
                g.drawLine(x, y + tmp, x + tmp, y);
            }
             */
        }

        //  code | _ref | KL1 | G1 |= GiaKhop | KLKhop | TongKL =| KL1 | G1 |=== TB | Cao | Thap
        virtual protected void updateQuote()
        {
            if (getID() < 0)
                return;

            Context ctx = Context.getInstance();
            stCell c;
            if (getID() == 0)
            {
                //  code
                setCellValue(0, "Mã/TC", C.COLOR_GRAY);

                setCellValue(2, "M3/KL", C.COLOR_GRAY);
                setCellValue(3, "M2/KL", C.COLOR_GRAY);
                setCellValue(4, "M1/KL", C.COLOR_GRAY);

                setCellValue(5, "Khớp", C.COLOR_GRAY);
                setCellValue(6, "+/-", C.COLOR_GRAY);

                setCellValue(7, "B1/KL", C.COLOR_GRAY);
                setCellValue(8, "B2/KL", C.COLOR_GRAY);
                setCellValue(9, "B3/KL", C.COLOR_GRAY);

                setCellValue(10, "H/L", C.COLOR_GRAY);

                setCellValue(11, "Vol Dư\nM/B", C.COLOR_GRAY);
                /*
                c = getCellAt(11);
                if (c != null)
                {
                    c.text2 = "DưB";
                    c.textColor2 = C.COLOR_GRAY;
                }
                 */

                setCellValue(12, "TổngKL", C.COLOR_GRAY);
            }

            if (getID() == 0)
                return;

            String code = (String)getUserData();
            if (code == null)
                return;
            stPriceboardState item = ctx.mPriceboard.getPriceboard(code);
            stPriceboardState ps = item;

            if (item == null)
                return;

            String s;

            //  code
            uint color;
            setCellValue(0, code, C.COLOR_WHITE);
            addCellValue1(0, String.Format("{0:F2}", item.getRef()), C.COLOR_YELLOW);
            float _ref = item.getRef();

            int j = 0;
            float v;
            int[] rbc = { 2, 3, 4 };	//	remain buy col
            float[] rb = { ps.getRemainBuyPrice2(), ps.getRemainBuyPrice1(), ps.getRemainBuyPrice0() };
            int[] rbv = { ps.getRemainBuyVolume2(), ps.getRemainBuyVolume1(), ps.getRemainBuyVolume0() };

            int[] rsc = { 7, 8, 9 };
            float[] rs = { ps.getRemainSellPrice0(), ps.getRemainSellPrice1(), ps.getRemainSellPrice2() };
            int[] rsv = { ps.getRemainSellVolume0(), ps.getRemainSellVolume1(), ps.getRemainSellVolume2() };
            //	mua1, mua2, mua3

            for (j = 0; j < 3; j++)
            {
                //	price
                color = ctx.valToColorF(rb[j], item.getCe(), item.getRef(), item.getFloor());
                setCellValue(rbc[j], String.Format("{0:F2}", rb[j]), color);
                //	vol
                s = volumeToString(rbv[j]);

                addCellValue1(rbc[j], s);
            }

            //  Khop
            float currentPrice = item.getCurrentPrice();
            int currentVol = item.getCurrentVolume();
            color = ctx.valToColorF(currentPrice, item.getCe(), item.getRef(), item.getFloor());
            setCellValue(5, String.Format("{0:F2}", currentPrice), color);

            s = volumeToString(currentVol);

            addCellValue1(5, s);

            //  change +/-
            v = currentPrice - item.getRef();
            s = String.Format("{0:F2}", (float)v);
            if (currentPrice == 0)
                s = "-";
            setCellValue(6, s, color);

            //	sell1, sell2, sell3
            for (j = 0; j < 3; j++)
            {
                //	price
                color = ctx.valToColorF(rs[j], item.getCe(), item.getRef(), item.getFloor());
                setCellValue(rsc[j], String.Format("{0:F2}", rs[j]), color);
                //	vol
                s = volumeToString(rsv[j]);

                addCellValue1(rsc[j], s);
            }

            //  cao - thap
            setCellValue(10, String.Format("{0:F2}", item.getMax()), ctx.valToColorF(item.getMax(), item.getCe(), item.getRef(), item.getFloor()));
            addCellValue1(10, String.Format("{0:F2}", item.getMin()), ctx.valToColorF(item.getMin(), item.getCe(), item.getRef(), item.getFloor()));
            //  cung - cau
            int buy = item.getRemainBuyVolume0() + item.getRemainBuyVolume1() + item.getRemainBuyVolume2();
            int sell = item.getRemainSellVolume0() + item.getRemainSellVolume1() + item.getRemainSellVolume2();
            String sbuy = volumeToString(buy);
            String ssell = volumeToString(sell);
            setCellValue(11, sbuy, C.COLOR_ORANGE);
            c = getCellAt(11);
            if (c != null)
            {
                c.text2 = ssell;
                c.textColor2 = C.COLOR_ORANGE;
            }
            
            //  total volume
            s = volumeToString(item.getTotalVolume());
            setCellValue(12, s, C.COLOR_WHITE);
        }

        public static String volumeToString(int v)
        {
            float vf = (float)(v/ 1000.0f);
            String s;
            if (vf > 0)
            {
                if (v > 10)
                    s = String.Format("{0:F1}", vf);
                else
                    s = String.Format("{0:F2}", vf);
            }
            else
                s = "-";

            return s;
        }

        override public void invalidate()
        {
            updateQuote();
            base.invalidate();

            //	    Utils.trace("-----updateQUOTE");
        }

        virtual public String getCode()
        {
            return (String)getUserData();
        }

        protected void renderCandle(xGraphics g, stCell c)
        {
            int x, y;

            x = c.x;
            y = 0;

            uint color;

            object o = getUserData();
            String code;
            if (o is String)
            {
                code = (String)o;
            }
            else
            {
                stGainloss gainloss = (stGainloss)o;
                code = gainloss.code;
            }

            if (code == null)
                return;
            stPriceboardState ps = Context.getInstance().mPriceboard.getPriceboard(code);

            if (ps == null)
                return;

            Context ctx = Context.getInstance();

            float price = ps.getCurrentPrice();
            float open = ctx.mPriceboard.getOpen(ps.getID());

            if (open == 0)
                open = price;

            float hi = ps.getMax();
            float lo = ps.getMin();
            //  check hi/lo valid
           
            if ((hi == 0 || lo == 0))
            {
                TradeHistory trade = Context.getInstance().getTradeHistory(ps.getID());
                float[] hl = new float[2];
                if (trade != null && trade.getHiLo(hl))
                {
                    if (hi == 0) hi = hl[0];
                    if (lo == 0) lo = hl[1];
                }
            }

            if (hi == lo)
            {
                hi = price;
            }

            if (hi == 0) hi = open > price ? open : price;
            if (lo == 0) lo = open < price ? open : price;
            if (lo == 0) lo = hi;
            //---------------------------------------------

            float priceLen = (float)(hi - lo);

            int y0 = 0;

            float min = ps.getRef() - (ps.getRef() / 13);	//	+-7% (7*13==100)
            float max = ps.getRef() + (ps.getRef() / 13);

            if (ps.getMarketID() == 1)
            {
                min = ps.getRef() - (ps.getRef() / 19);	//	+-5%
                max = ps.getRef() + (ps.getRef() / 19);
            }

            if (min > lo && lo > 0) min = (float)lo;
            if (max < hi) max = (float)hi;

            float totalPrice = (max - min);  //(10%);
            if (totalPrice < priceLen)
                totalPrice = priceLen;

            if (totalPrice == 0)
                return;

            float ry = (float)(getH() - 2 * y0) / totalPrice;

            int totalH = (int)(ry * totalPrice);
            int bodyW = c.w / 2;

            //================frame=============================
            //  line _ref
            g.setColor(0x70ffff00);
            y = (int)(y0 + totalH - (ps.getRef() - min) * ry);
            g.drawLineDotHorizontal(c.x + 1, y, c.w - 2);
            //===================================================
            if (price == 0)
                return;	//	khong co giao dich        
            color = price < open ? C.COLOR_RED : C.COLOR_GREEN;
            if (price == open)
                color = C.COLOR_WHITE;

            //  draw shadow
            g.setColor(C.COLOR_WHITE);
            x = c.x + c.w / 2;

            if (lo > 0 && hi > 0)
            {
                int minY = (int)(y0 + totalH - (lo - min) * ry);
                int maxY = (int)(y0 + totalH - (hi - min) * ry);

                g.drawLine(x, maxY, x, minY);
            }
            int centerX = x + bodyW / 2;
            //  candle's body
            int oY = (int)(y0 + totalH - (open - min) * ry);
            int cY = (int)(y0 + totalH - (price - min) * ry);
            y = oY < cY ? oY : cY;
            int bodyH = Utils.ABS_INT(cY - oY);
            if (bodyH < 2)
                bodyH = 2;
            g.setColor(color);
            g.fillRect(x - bodyW / 2, y, bodyW, bodyH);
        }

        public override void onMouseDown(int x, int y)
        {
            mDoubleClicked = false;

            showQuickInfo(x + 20, y + 10);

            base.onMouseDown(x, y);

           
        }

        void showQuickInfo(int x, int y)
        {
            try
            {
                if (mToolTip == null)
                {
                    mToolTip = new ToolTip();
                }
                //mToolTip.SetToolTip(getControl(), "Abc asdad asd asd sada");
                Object o = getUserData();
                String code = null;
                if (o.GetType() == typeof(stGainloss))
                {
                    code = ((stGainloss)o).code;
                }
                else if (o.GetType() == typeof(String))
                {
                    code = (String)o;
                }
                if (code == null)
                    return;
                stPriceboardState item = Context.getInstance().mPriceboard.getPriceboard(code);

                if (item == null)
                    return;
                stCompanyInfo inf = Context.getInstance().mShareManager.getCompanyInfo(item.id);
                if (inf != null)
                {
                    StringBuilder sb = Utils.sb2;
                    sb.Length = 0;

                    sb.Append(inf.company_name);
                    sb.Append("\n");
                    sb.AppendFormat("EPS={0:F1} K; P/E={1:F1}", ((float)inf.EPS / 1000), ((float)inf.PE / 100));

                    sb.AppendFormat("; KLCP={0:F2} tr\n", (inf.volume / 1000.0f));

                    float price = 0;
                    double vonhoa = 0;
                    price = item.getCurrentPrice();
                    if (price == 0)
                        price = item.getRef();
                    vonhoa = price;
                    vonhoa = vonhoa * inf.volume;
                    vonhoa = vonhoa / 1000;

                    sb.AppendFormat("Vốn hóa tt={0:F2} tỉ", vonhoa);

                    mToolTip.Show(sb.ToString(), getControl(), x, y, 3000);
                }
            }
            catch (Exception e)
            {

            }
        }

        public override void onMouseMove(int x, int y)
        {
            base.onMouseMove(x, y);

           
        }

        static ToolTip mToolTip = null;

        public override void onMouseUp(int x, int y)
        {
            if (mDoubleClicked)
                return;
            base.onMouseUp(x, y);
           
            if (getID() <= 0)
                return;

            mParent.selectRow(this);

            //=========================
            if (mIsSelected)
            {
                //Context.getInstance().setCurrentShare(getCode());
                Share share = Context.getInstance().mShareManager.getShare(getCode());

                mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, C.ID_SELECT_SHARE_CANDLE, share);
            }
            else
            {
                mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, C.ID_SELECT_SHARE_CANDLE, null);
            }
            
            invalidate();
        }

        bool mDoubleClicked = false;
        public override void onMouseDoubleClick()
        {
            base.onMouseDoubleClick();

            if (Context.getInstance().setCurrentShare(getCode()))
            {
                mDoubleClicked = true;
                Share share = Context.getInstance().mShareManager.getShare(getCode());
                mListener.onEvent(this, xBaseControl.EVT_ON_MOUSE_DOUBLE_CLICK, C.ID_SELECT_SHARE_CANDLE, share);
            }
        }

        public void select()
        {
            mIsSelected = true;
            invalidate();
        }

        public void deselect()
        {
            mIsSelected = false;
            invalidate();
        }
    }
}
