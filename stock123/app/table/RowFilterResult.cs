using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using stock123.app.data;
using xlib.framework;
using xlib.ui;
using xlib.utils;

namespace stock123.app.table
{
    class RowFilterResult : RowNormalShare
    {
        public RowFilterResult(xIEventListener listener, int _id, int w, int h)
            : base(listener, _id, w, h)
        {
            //mCandleColumeIdx = -1;
        }

        override protected void createRow(int _id, int w, int h)
        {
            //==================================
            Context ctx = Context.getInstance();
            Font f = ctx.getFontSmallB();

            if (_id >= 0)
            {
                uint BG_GRAY = 0x90606060;
                uint BG0 = 0;
                if (_id >= 0 && (_id % 2) == 0)
                {
                    BG_GRAY += 0x0050505;
                    BG0 = 0x90202020;
                }

                //  code/ref | +/- | Volume
                Font[] font = { f, f, f, f, f};
                float[] percents = {20, 7, 20, 53,
	                -1};
                uint[] colors = { BG_GRAY, BG0, BG0, BG0, BG_GRAY };
                init(w, h, percents, font, colors);
            }

            setID(_id);
        }

        //  code | ref | KL1 | G1 |= GiaKhop | KLKhop | TongKL =| KL1 | G1 |=== TB | Cao | Thap
        override protected void updateQuote()
        {
            if (getID() < 0)
                return;
            Context ctx = Context.getInstance();
            if (getID() == 0)
            {
                //  code
                setCellValue(0, "Mã/Giá", C.COLOR_GRAY);

                setCellValue(2, "+/-", C.COLOR_GRAY);

                setCellValue(3, "Khối lượng", C.COLOR_GRAY);
            }

            if (getID() == 0)
            {
                return;
            }

            String code = (String)this.getUserData();
            if (code == null)
            {
                return;
            }
            stPriceboardState item = ctx.mPriceboard.getPriceboard(code);

            if (item == null)
                return;

            String s;

            //  code
            uint color;
            setCellValue(0, item.getCode(), C.COLOR_WHITE);

            float reference = item.getRef();
            float price = item.getCurrentPrice();
            if (price == 0)
            {
                price = reference;
            }
            color = ctx.valToColorF(item.getCurrentPrice(), item.getCe(), item.getRef(), item.getFloor());
            addCellValue1(0, String.Format("{0:F2}", item.getCurrentPrice()), color);

            int j = 0;
            //  change +/-
            float v = price - reference;
            s = String.Format("{0:F2}", (float)v);
            if (item.getCurrentPrice() == 0)
                s = "-";
            setCellValue(2, s, color);

            //  change percent
            if (reference > 0){
                s = String.Format("{0:F1%}", (v*100)/reference);
                addCellValue1(2, s, color);
            }

            //  volume
            s = volumeToString(item.getTotalVolume());
            setCellValue(3, s, C.COLOR_WHITE);

            //Share share = (Share)mFilteredShares.elementAt(row);
            if (rcOfView == null)
            {
                rcOfView = new Rectangle();
            }

        }
        System.Drawing.Rectangle rcOfView;

        override public String getCode()
        {
            String code = (String)getUserData();

            return code;
        }

        public override void render(xGraphics g)
        {
            if (getID() < 0)
                return;

            int x = 0;
            int y = 2;
            int h = getH();

            if (mCells.size() == 0)
            {
                return;
            }

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
                    g.drawStringInRect(c.f, c.text, c.x, (int)(h / 2 - c.f.GetHeight()) + 3, c.w, h / 2, xGraphics.HCENTER | xGraphics.VCENTER);

                    g.setColor(c.textColor2);
                    g.drawStringInRect(c.f, c.text2, c.x, h / 2 + 1, c.w, (int)(c.f.GetHeight()), xGraphics.HCENTER | xGraphics.VCENTER);
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

            if (getID() > 0)
            {
                String code = getCode();
                if (code != null)
                {
                    stCell cell = getCellAt(2);
                    rcOfView.X = cell.x;
                    rcOfView.Y = 0;
                    rcOfView.Width = cell.w;
                    rcOfView.Height = getH();

                    sharethumb.DrawAChartDelegator.renderToView(code, g, rcOfView);
                }
            }
            
        }
    }
}
