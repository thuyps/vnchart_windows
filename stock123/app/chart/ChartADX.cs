using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;
using xlib.framework;

namespace stock123.app.chart
{
    public class ChartADX: ChartBase
    {
        short[] mLinePDI;
        int mLinePDISize;
        short[] mLineMDI;
        int mLineMDISize;
        int mY20;
        int mY40;
        //===================================
        public ChartADX(Font f)
            : base(f)
        {
            mShouldDrawCursor = true;
            mChartType = CHART_ADX;
            //CHART_BORDER_SPACING_Y = 1;
        }

        public override void render(xGraphics g)
        {
            int mH = getH();
            int mW = getW();

            if (isHiding())
                return;
            Share share = getShare();
            if (share == null)
                return;

            int idx = 0;
            int maxPrice = 80;
            if (detectShareCursorChanged())
            {
                share.calcADX();

                int newSize = mChartLineLength * 2;
                mChartLineXY = allocMem(mChartLineXY, newSize);
                mLineMDI = allocMem(mLineMDI, newSize);
                mLinePDI = allocMem(mLinePDI, newSize);

                idx = share.mBeginIdx;
                pricesToYs(share.pPLUS_DI, idx, mLinePDI, mChartLineLength, 0, maxPrice);
                pricesToYs(share.pMINUS_DI, idx, mLineMDI, mChartLineLength, 0, maxPrice);
                pricesToYs(share.pADX, idx, mChartLineXY, mChartLineLength, 0, maxPrice);

                float ry = (float)getDrawingH() / maxPrice;
                mY20 = (int)(getMarginY() + getDrawingH() - 20 * ry);
                mY40 = (int)(getMarginY() + getDrawingH() - 40 * ry);
            }

            if (mChartLineLength == 0)
                return;

            if (mShouldDrawGrid)
                drawGrid(g);
            //===============================================
            g.setColor(C.COLOR_FADE_YELLOW);
            g.drawHorizontalLine(0, mY20, mW - 34);
            g.drawHorizontalLine(0, mY40, mW - 34);

            g.setColor(C.COLOR_FADE_YELLOW0);
            g.drawString(mFont, "20", 0 + mW - 2, mY20, xGraphics.RIGHT | xGraphics.VCENTER);
            g.drawString(mFont, "40", 0 + mW - 2, mY40, xGraphics.RIGHT | xGraphics.VCENTER);

            g.setColor(0xffffffff);

            g.drawLines(mChartLineXY, mChartLineLength, 2.0f);

            g.setColor(0xff00ff00);
            g.drawLines(mLinePDI, mChartLineLength, 1.0f);

            g.setColor(0xffff0000);
            g.drawLines(mLineMDI, mChartLineLength, 1.0f);
            //==============================
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            sb.AppendFormat("{0:F2}", (float)yToPrice(mLastY, 0, 80));
            mMouseTitle = sb.ToString();

            renderCursor(g);

            //===================
            //	bottom border
            g.setColor(0xffa0a0a0);
            g.drawHorizontalLine(0, 0, getW());

            renderDrawer(g);
        }

        public override string getTitle()
        {
            Share share = getShare();
            if (share != null)
            {
                int idx = share.getCursor();
                int adx = (int)share.pADX[idx];
                int pdi = (int)share.pPLUS_DI[idx];
                int mdi = (int)share.pMINUS_DI[idx];

                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                sb.AppendFormat("ADX={0}; +DI={1}; -DI={2}", adx, pdi, mdi);
                return sb.ToString();
            }

            return "";
        }

        override public xVector getTitles()
        {
            xVector v = new xVector();

            Share share = getShare();
            if (share == null)
                return null;

            int idx = share.getCursor();
            int adx = (int)share.pADX[idx];
            int pdi = (int)share.pPLUS_DI[idx];
            int mdi = (int)share.pMINUS_DI[idx];

            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            sb.AppendFormat("ADX({0})={1}", mContext.mOptADXPeriod, adx);
            v.addElement(new stTitle(sb.ToString(), 0xfff0f0f0));

            //  +di
            sb.Length = 0;
            sb.AppendFormat("DMI({0}): +DI={1}", (int)mContext.mOptADXPeriodDMI, pdi);
            v.addElement(new stTitle(sb.ToString(), 0xff00ff00));

            //  signal 9
            sb.Length = 0;
            sb.AppendFormat("-DI: {0}", mdi);

            v.addElement(new stTitle(sb.ToString(), 0xffff0000));

            return v;
        }
    }
}
