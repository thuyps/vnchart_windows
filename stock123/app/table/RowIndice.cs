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
    class RowIndice: RowNormalShare
    {
        //TablePriceboard mParent;
        //xVector mCells = new xVector(10);
        //long mHoldingTimeStart;
        //bool mIsSelected = false;
        //bool mIsRejectTouchUp = false;
        //bool mAllowDelete = false;
        //public int mCandleColumeIdx = 1;

        //const int COLOR_NONE = 1;

        public RowIndice(xIEventListener listener, int _id, int w, int h)
            :base(listener, _id, w, h)
        {
            /*
            makeCustomRender(true);

            setID(_id);

            setSize(w, h);

            createRow(_id, w, h);
             */
        }

        override protected void createRow(int _id, int w, int h)
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
                //  code/_ref | Price | +/- | Cao/thap | TongKL
                Font[] font = {f, f,
                    f, f, 
		            				f, f,
		            				fsmallB, fsmallB};
                float[] percents = {25,
                    6,
                    15f,   
		                15, 
                    15,
		                25, -1};
                uint[] colors = { BG_GRAY, BG0, BG0, BG0, BG_GRAY, BG_GRAY, BG0, BG0, BG0, BG_GRAY, BG_GRAY, BG_GRAY, COLOR_NONE };
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

        //  code | _ref | KL1 | G1 |= GiaKhop | KLKhop | TongKL =| KL1 | G1 |=== TB | Cao | Thap
        override protected void updateQuote()
        {
            if (getID() < 0)
                return;

            Context ctx = Context.getInstance();
            stCell c;
            if (getID() == 0)
            {
                //  code
                setCellValue(0, "Mã/TC", C.COLOR_GRAY);

                setCellValue(2, "Giá", C.COLOR_GRAY);
                setCellValue(3, "+/-", C.COLOR_GRAY);

                setCellValue(4, "H/L", C.COLOR_GRAY);

                
                setCellValue(5, "TổngKL", C.COLOR_GRAY);
            }

            if (getID() == 0)
                return;

            String code = (String)getUserData();
            if (code == null)
                return;
            stPriceboardState item = GlobalData.getPriceboardStateOfIndexWithSymbol(code);
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

            //  Khop
            float currentPrice = item.getCurrentPrice();
            int currentVol = item.getCurrentVolume();
            color = ctx.valToColorF(currentPrice, item.getCe(), item.getRef(), item.getFloor());
            setCellValue(2, String.Format("{0:F2}", currentPrice), color);

            s = volumeToString(currentVol);

            addCellValue1(2, s);

            //  change +/-
            v = currentPrice - item.getRef();
            s = String.Format("{0:F2}", (float)v);
            if (currentPrice == 0)
                s = "-";
            setCellValue(3, s, color);

            //  cao - thap
            setCellValue(4, String.Format("{0:F2}", item.getMax()), ctx.valToColorF(item.getMax(), item.getCe(), item.getRef(), item.getFloor()));
            addCellValue1(4, String.Format("{0:F2}", item.getMin()), ctx.valToColorF(item.getMin(), item.getCe(), item.getRef(), item.getFloor()));
            
            
            //  total volume
            s = volumeToString(item.getTotalVolume());
            setCellValue(5, s, C.COLOR_WHITE);
        }

        void showQuickInfo(int x, int y)
        {
            
        }
   }
}
