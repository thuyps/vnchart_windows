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
    class RowGainloss : RowNormalShare
    {
        public RowGainloss(xIEventListener listener, int _id, int w, int h)
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

                //  code/ref | Khop | +/- | GiaMua/KL | Gainloss%/Money | date
                Font[] font = { f, f, f, f, f, f, f, f };
                float[] percents = {11, 6, 11, 10,
	                15,	//	gia mua  
	                14,	//	tien von	                
	                15,	//	gain-loss
	                18,
	                -1};
                uint[] colors = { BG_GRAY, BG0, BG0, BG_GRAY, BG_GRAY, BG_GRAY, BG0, 0 };
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
                setCellValue(0, "Mã/TC", C.COLOR_GRAY);

                setCellValue(2, "Giá", C.COLOR_GRAY);

                setCellValue(3, "+/-", C.COLOR_GRAY);

                setCellValue(4, "GiáMua/KL", C.COLOR_GRAY);

                setCellValue(5, "Vốn (tr)", C.COLOR_WHITE);

                setCellValue(6, "Lãi  % / vnd", C.COLOR_GRAY);

                setCellValue(7, "Ngày mua", C.COLOR_WHITE);
            }

            if (getID() == 0)
                return;

            stGainloss gainloss = (stGainloss)getUserData();
            if (gainloss == null)
                return;
            String code = gainloss.code;
            stPriceboardState item = ctx.mPriceboard.getPriceboard(code);

            if (item == null)
                return;

            String s;

            //  code
            uint color;
            setCellValue(0, item.getCode(), C.COLOR_WHITE);
            addCellValue1(0, String.Format("{0:F2}", item.getRef()), C.COLOR_YELLOW);
            float reference = item.getRef();
            float price = item.getCurrentPrice();
            if (price == 0)
                price = reference;

            int j = 0;
            //  price
            color = ctx.valToColorF(item.getCurrentPrice(), item.getCe(), item.getRef(), item.getFloor());
            setCellValue(2, String.Format("{0:F2}", item.getCurrentPrice()), color);
            //	vol
            //s = volumeToString(item.getCurrentVolume());	    
            //addCellValue1(1, s);

            //  change +/-
            float v = price - reference;
            s = String.Format("{0:F2}", (float)v);
            if (item.getCurrentPrice() == 0)
                s = "-";
            setCellValue(3, s, color);

            //  GiaMua
            s = String.Format("{0:F2}", gainloss.price);
            setCellValue(4, s, C.COLOR_WHITE);
            //  KLMua
            s = String.Format("{0:D}", gainloss.volume);
            addCellValue1(4, s);

            //  Tien von
            double capital = gainloss.price * gainloss.volume / 1000.0f;
            s = String.Format("{0:F} tr", capital);
            setCellValue(5, s, C.COLOR_WHITE);

            //========================
            //  %GainLoss
            stPriceboardState ps = item;
            float currentPrice = ps.getCurrentPrice();
            if (currentPrice == 0)
                currentPrice = ps.getRef();

            price = gainloss.price;
            float percent = 0;
            if (price > 0)
            {
                percent = (float)(100 * (currentPrice - price)) / price;
            }
            if (percent > 0)
                s = String.Format("+{0:F2} %", percent);
            else
                s = String.Format("{0:F2} %", percent);
            if (percent > 0)
                color = C.COLOR_GREEN;
            else if (percent == 0)
                color = C.COLOR_YELLOW;
            else
                color = C.COLOR_RED;

            setCellValue(6, s, color);
            //  gainloss money
            double oValue = price * gainloss.volume;
            double cValue = currentPrice * gainloss.volume;
            double benefit = (cValue - oValue) / 1000;

            if (benefit > 0)
                s = String.Format("+{0:F2} tr", benefit);
            else
                s = String.Format("{0:F2} tr", benefit);

            addCellValue1(6, s);
            //  date
            //s = Utils.dateIntToString4(gainloss.date);
            //setCellValue(7, s, C.COLOR_WHITE);
        }

        override public String getCode()
        {
            stGainloss g = (stGainloss)getUserData();
            if (g != null)
                return g.code;

            return null;
        }

        override public stPriceboardState getPriceboard()
        {
            stGainloss g = (stGainloss)getUserData();
            if (g != null)
            {
                return Context.getInstance().mPriceboard.getPriceboard(g.code);
            }

            return null;
        }

        public override void render(xGraphics g)
        {
            base.render(g);

            renderSnapshot2(g);
        }

        void renderSnapshot2(xGraphics g)
        {
            stPriceboardState item = getPriceboard();
            if (item != null)
            {
                stCell cell = getCellAt(7);

                //  snapshot
                if (rcSnapshot == null)
                {
                    rcSnapshot = new Rectangle();
                }
                rcSnapshot.X = (int)cell.x;
                rcSnapshot.Y = 0;
                rcSnapshot.Width = (int)cell.w;
                rcSnapshot.Height = getH();

                sharethumb.DrawAChartDelegator.renderToView(item.code, g, rcSnapshot);

                //  date
                stGainloss gainloss = (stGainloss)getUserData();
                String s = Utils.dateIntToStringDDMM(gainloss.date);

                g.setColor(C.COLOR_WHITE);
                g.drawStringInRect(cell.f, s, cell.x, getH()-17, cell.w, 17, xGraphics.LEFT);
            }
        }
    }
}
