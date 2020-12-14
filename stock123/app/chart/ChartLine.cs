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
    public class ChartLine: ChartBase
    {
        short[] mPricelines = new short[10];
        short[] mChartLineXY3;
        short[] mChartLineXY4;
        short[] mChartLineXY5;
        int mSMAIdx;
        public int mSMAPeriod;
        //==================
        short[] mChartEMA;
        short[] mChartEMA2;
        //=========================================

        public ChartLine(Font f):base(f)
        {
            mChartType = CHART_LINE;
            mVolume = "";
            mOpen = "";
            mClose = "";

            mLineThink = 1.0f;
        }

        override public void render(xGraphics g)
        {
            if (isHiding())
                return;
            //g.setColor(Constants.COLOR_BLACK);
            //g.clear();

            if (getShare(3) == null)
                return;

            if (mChartType == CHART_COMPARING_SECOND_SHARE)
            {
                drawChartOfSecondShare(g);
            }
            else if (mChartType == CHART_PAST_1_YEAR || mChartType == CHART_PAST_2_YEARS)
            {
                drawChartPastOfYears(g);
            }
            else if (mChartType == CHART_SMA)
            {
                if (mSMAIdx == 3)
                {
                    drawChartSMA3(g);
                }
                else
                {
                    drawChartSMA(g);
                }
            }
            else if (mChartType == CHART_NVI)
            {
                drawChartNVI(g);
            }
            else if (mChartType == CHART_PVI)
            {
                drawChartPVI(g);
            }
            else if (mChartType == CHART_RSI)
            {
                drawChartRSI(g);
            }
            else if (mChartType == CHART_MFI)
            {
                drawChartMFI(g);
            }
            else if (mChartType == CHART_ADL)
            {
                drawChartADL(g);
            }
            else if (mChartType == CHART_CHAIKIN)
            {
                drawChartChaikin(g);
            }
            else if (mChartType == CHART_ROC)
            {
                drawChartROC(g);
            }
            else if (mChartType == CHART_THUYPS)
            {
                drawChartThuyPS(g);
            }
            else if (mChartType == CHART_TRIX)
            {
                drawChartTrix(g);
            }
            else if (mChartType == CHART_PVT)
            {
                drawChartPVT(g);
            }
            else if (mChartType == CHART_CCI)
            {
                drawChartCCI(g);
            }
            else if (mChartType == CHART_MASSINDEX)
            {
                drawChartMassIndex(g);
            }
            else if (mChartType == CHART_ATR)
            {
                drawChartATR(g);
            }

            renderDrawer(g);
        }

        override public xVector getTitles()
        {
            xVector v = new xVector(3);
            Share share = getShare(3);
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            if (share != null)
            {
                int idx = share.getCursor();
                if (mChartType == CHART_LINE)
                {
                    return v;
                }
                else if (mChartType == CHART_RSI)
                {
                    sb.AppendFormat("RSI({0})={1:F1}", mContext.mOptRSIPeriod[0], share.pRSI[idx]);
                    if (mHasSecondChart)
                        sb.AppendFormat("    RSI#2({0:F1})={1}", mContext.mOptRSIPeriod[1], share.pRSISecond[idx]);
                    if (mContext.mOptRSI_EMA > 0)
                    {
                        sb.AppendFormat("    MA({0})={1:F1}", mContext.mOptRSI_EMA, share.pEMAIndicator[idx]);
                    }

                    v.addElement(new stTitle(sb.ToString(), C.COLOR_WHITE));
                }
                else if (mChartType == CHART_MFI)
                {
                    sb.AppendFormat("MFI({0})={1:F1}", mContext.mOptMFIPeriod[0], share.pMFI[idx]);
                    if (mHasSecondChart)
                        sb.AppendFormat("    MFI#2({0})={1:F1}", mContext.mOptMFIPeriod[1], share.pMFISecond[idx]);

                    if (mContext.mOptMFI_EMA > 0)
                    {
                        sb.AppendFormat("    MA({0})={1:F1}", mContext.mOptMFI_EMA, share.pEMAIndicator[idx]);
                    }

                    v.addElement(new stTitle(sb.ToString(), C.COLOR_WHITE));
                }
                else if (mChartType == CHART_CHAIKIN)
                {
                    //v.addElement(new stTitle("Chaikin", C.COLOR_WHITE));
                }
                else if (mChartType == CHART_ROC)
                {
                    sb.AppendFormat("ROC({0}): {1:F2}%", mContext.mOptROC[0], share.pROC[idx]);
                    if (mHasSecondChart)
                        sb.AppendFormat("    ROC#2({0}): {1:F2}%", mContext.mOptROC[1], share.pROCSecond[idx]);

                    if (mContext.mOptROC_EMA > 0)
                    {
                        sb.AppendFormat("    SMA({0}): {1:F2}%", mContext.mOptROC_EMA, share.pEMAIndicator[idx]);
                    }

                    v.addElement(new stTitle(sb.ToString(), C.COLOR_WHITE));
                }
                else if (mChartType == CHART_NVI)
                {
                    sb.AppendFormat("NVI={0:F1}", share.pNVI[idx]);
                    v.addElement(new stTitle(sb.ToString(), C.COLOR_WHITE));
                    if (mContext.mOptNVI_EMA[0] > 0)
                    {
                        sb.Length = 0;
                        sb.AppendFormat("     MA({0}): {1:F1}", (int)mContext.mOptNVI_EMA[0], share.pNVI_EMA1[idx]);
                        v.addElement(new stTitle(sb.ToString(), C.COLOR_MAGENTA));
                    }
                    if (mContext.mOptNVI_EMA[1] > 0)
                    {
                        sb.Length = 0;
                        sb.AppendFormat("     MA({0}): {1:F1}", (int)mContext.mOptNVI_EMA[1], share.pNVI_EMA2[idx]);
                        v.addElement(new stTitle(sb.ToString(), C.COLOR_BLUE_LIGHT));
                    }                    
                }
                else if (mChartType == CHART_PVI)
                {
                    sb.AppendFormat("PVI={0:F1}", share.pPVI[idx]);
                    v.addElement(new stTitle(sb.ToString(), C.COLOR_WHITE));
                    if (mContext.mOptPVI_EMA[0] > 0)
                    {
                        sb.Length = 0;
                        sb.AppendFormat("     MA({0}): {1:F1}", (int)mContext.mOptPVI_EMA[0], share.pPVI_EMA1[idx]);
                        v.addElement(new stTitle(sb.ToString(), C.COLOR_MAGENTA));
                    }
                    if (mContext.mOptPVI_EMA[1] > 0)
                    {
                        sb.Length = 0;
                        sb.AppendFormat("     MA({0}): {1:F1}", (int)mContext.mOptPVI_EMA[1], share.pPVI_EMA2[idx]);
                        v.addElement(new stTitle(sb.ToString(), C.COLOR_BLUE_LIGHT));
                    }
                }
                else if (mChartType == CHART_TRIX)
                {
                    sb.AppendFormat("TRIX({0}, {1})", mContext.mOptTRIX[0], mContext.mOptTRIX[1]);
                    v.addElement(new stTitle(sb.ToString(), C.COLOR_WHITE));

                    sb.Length = 0;
                    sb.AppendFormat("{0:F2}", share.pTrix[idx]);
                    v.addElement(new stTitle(sb.ToString(), C.COLOR_GREEN));

                    sb.Length = 0;
                    sb.AppendFormat("{0:F2}", share.pTrixEMA[idx]);
                    v.addElement(new stTitle(sb.ToString(), C.COLOR_MAGENTA));
                }
                else if (mChartType == CHART_ADL)
                {

                }
                else if (mChartType == CHART_PVT)
                {
                    sb.Length = 0;
                    String s = Utils.formatNumber((int)share.pPVT[idx]);
                    sb.Length = 0;
                    sb.AppendFormat("PVT={0}", s);
                    v.addElement(new stTitle(sb.ToString(), C.COLOR_WHITE));
                    if (mContext.mOptPVT > 0)
                    {
                        sb.Length = 0;
                        s = Utils.formatNumber((int)share.pPVT_EMA1[idx]);
                        sb.Length = 0;
                        sb.AppendFormat("     MA({0}): {1:F1}", (int)mContext.mOptPVT, s);
                        v.addElement(new stTitle(sb.ToString(), C.COLOR_MAGENTA));
                    }
                }
                else if (mChartType == CHART_CCI)
                {
                    sb.AppendFormat("CCI (k:{0:F3}, {1}): {2:F2}", mContext.mOptCCIConstant, (int)mContext.mOptCCIPeriod, share.pCCI[idx]);

                    v.addElement(new stTitle(sb.ToString(), C.COLOR_WHITE));
                }
                else if (mChartType == CHART_MASSINDEX)
                {
                    sb.AppendFormat("MASS ({0}, {1}): {2:F1}", mContext.mOptMassIndexDifferent, (int)mContext.mOptMassIndexSum, share.pMassIndex[idx]);

                    v.addElement(new stTitle(sb.ToString(), C.COLOR_WHITE));
                }
                else if (mChartType == CHART_ATR)
                {
                    sb.AppendFormat("ATR ({0}): {1:F2}", mContext.mOptATRLoopback, share.pATR[idx]);

                    v.addElement(new stTitle(sb.ToString(), C.COLOR_WHITE));
                }
            }

            return v;
        }

        protected void drawChartRSI(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            if (detectShareCursorChanged())
            {
                Share s = getShare();
                s.calcRSI(0);

                Share share = s;

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                pricesToYs(share.pRSI, s.mBeginIdx, mChartLineXY, mChartLineLength, 0, 100);
                float[] tmp = { 30, 50, 70 };
                pricesToYs(tmp, 0, mPricelines, 3, 0, 100);

                if (mHasSecondChart)
                {
                    s.mIsCalcRSI = false;
                    s.calcRSI(1);

                    mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);

                    pricesToYs(share.pRSISecond, s.mBeginIdx, mChartLineXY2, mChartLineLength, 0, 100);
                }
                //==============================
                if (mContext.mOptRSI_EMA > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);

                    if (mContext.mOptRSI_EMA_ON[0])
                    {
                        Share.EMA(share.pRSI, s.getCandleCount(), (int)mContext.mOptRSI_EMA, s.pEMAIndicator);
                    }
                    else
                    {
                        Share.SMA(share.pRSI, 0, s.getCandleCount(), (int)mContext.mOptRSI_EMA, s.pEMAIndicator);
                    }
                    pricesToYs(s.pEMAIndicator, s.mBeginIdx, mChartEMA, mChartLineLength, 0, 100);
                }
            }

            if (mShouldDrawGrid)
                drawGrid(g);

            String[] ss = { "30", "50", "70" };
            for (int i = 0; i < 3; i++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, mPricelines[2 * i + 1], getW() - 20, mPricelines[2 * i + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ss[i], getW() - 20, mPricelines[2 * i + 1], xGraphics.VCENTER);
            }

            g.setColor(0xffff7000);
            g.drawLines(mChartLineXY, mChartLineLength, 2.0f);

            if (mHasSecondChart)
            {
                g.setColor(C.COLOR_SECOND_CHART);
                g.drawLines(mChartLineXY2, mChartLineLength, 1.5f);
            }

            if (mContext.mOptRSI_EMA > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                g.drawLines(mChartEMA, mChartLineLength, 1.0f);
            }

            mMouseTitle = "" + (int)yToPrice(mLastY, 0, 100);

            renderCursor(g);
        }

        protected void drawChartMFI(xGraphics g)
        {
            int mX = 0;
            int mY = 0;

            if (detectShareCursorChanged())
            {
                Share s = getShare();
                s.calcMFI(0);

                Share share = s;

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                pricesToYs(share.pMFI, s.mBeginIdx, mChartLineXY, mChartLineLength, 0, 100);
                float[] tmp = { 20, 50, 80 };
                pricesToYs(tmp, 0, mPricelines, 3, 0, 100);

                if (mHasSecondChart)
                {
                    s.mIsCalcMFI = false;
                    s.calcMFI(1);

                    mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);
                    pricesToYs(share.pMFISecond, s.mBeginIdx, mChartLineXY2, mChartLineLength, 0, 100);
                }
                //==============================
                if (mContext.mOptMFI_EMA > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);

                    if (mContext.mOptMFI_EMA_ON[0])
                    {
                        Share.EMA(share.pMFI, s.getCandleCount(), (int)mContext.mOptMFI_EMA, s.pEMAIndicator);
                    }
                    else
                    {
                        Share.SMA(share.pMFI, 0, s.getCandleCount(), (int)mContext.mOptMFI_EMA, s.pEMAIndicator);
                    }
                    pricesToYs(s.pEMAIndicator, s.mBeginIdx, mChartEMA, mChartLineLength, 0, 100);
                }
            }

            if (mShouldDrawGrid)
                drawGrid(g);
            String[] ss = { "20", "50", "80" };

            for (int i = 0; i < 3; i++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, mPricelines[2 * i + 1], getW() - 20, mPricelines[2 * i + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ss[i], getW() - 20, mPricelines[2 * i + 1], xGraphics.VCENTER);
            }

            g.setColor(0xff20dd3d);
            g.drawLines(mChartLineXY, mChartLineLength, 2.0f);
            if (mHasSecondChart)
            {
                g.setColor(C.COLOR_SECOND_CHART);
                g.drawLines(mChartLineXY2, mChartLineLength, 1.5f);
            }
            if (mContext.mOptMFI_EMA > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                g.drawLines(mChartEMA, mChartLineLength, 1.0f);
            }

            mMouseTitle = "" + (int)yToPrice(mLastY, 0, 100);
            renderCursor(g);
        }

        protected void drawChartChaikin(xGraphics g)
        {
            int mX = 0;
            int mY = 0;

            Share s = getShare();
            if (s == null)
                return;

            Share share = s;

            if (detectShareCursorChanged())
            {
                s.calcChaikinOscillator(0);
                if (mHasSecondChart)
                {
                    s.mIsCalcChaikin = false;
                    s.calcChaikinOscillator(1);
                }
                lo = 10000;
                hi = -10000;
                for (int i = 0; i < mChartLineLength; i++)
                {
                    if (share.pChaikinOscillator[i + s.mBeginIdx] > hi) hi = share.pChaikinOscillator[i + s.mBeginIdx];
                    if (share.pChaikinOscillator[i + s.mBeginIdx] < lo) lo = share.pChaikinOscillator[i + s.mBeginIdx];

                    if (share.pChaikinOscillatorSecond[i + s.mBeginIdx] > hi) hi = share.pChaikinOscillatorSecond[i + s.mBeginIdx];
                    if (share.pChaikinOscillatorSecond[i + s.mBeginIdx] < lo) lo = share.pChaikinOscillatorSecond[i + s.mBeginIdx];
                }

                float max = (Utils.ABS_FLOAT(hi) > Utils.ABS_FLOAT(lo)) ? Utils.ABS_FLOAT(hi) : Utils.ABS_FLOAT(lo);
                hi = max;
                lo = -max;

                //============================================
                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                pricesToYs(share.pChaikinOscillator, s.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);

                if (mHasSecondChart)
                {
                    mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);

                    pricesToYs(share.pChaikinOscillatorSecond, s.mBeginIdx, mChartLineXY2, mChartLineLength, lo, hi);
                }
                if (mContext.mOptChaikinOscillatorEMA > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);
                    if (mContext.mOptChaikinOscillatorEMA_ON[0])
                    {
                        Share.EMA(share.pChaikinOscillator, s.getCandleCount(), (int)mContext.mOptChaikinOscillatorEMA, s.pEMAIndicator);
                    }
                    else
                    {
                        Share.SMA(share.pChaikinOscillator, 0, s.getCandleCount(), (int)mContext.mOptChaikinOscillatorEMA, s.pEMAIndicator);
                    }
                    pricesToYs(s.pEMAIndicator, s.mBeginIdx, mChartEMA, mChartLineLength, lo, hi);
                }
            }

            if (mShouldDrawGrid)
                drawGrid(g);

            for (int i = 0; i < mChartLineLength; i++)
            {
                if (share.pChaikinOscillator[i + s.mBeginIdx] > hi) hi = share.pChaikinOscillator[i + s.mBeginIdx];
                if (share.pChaikinOscillator[i + s.mBeginIdx] < lo) lo = share.pChaikinOscillator[i + s.mBeginIdx];

                if (share.pChaikinOscillatorSecond[i + s.mBeginIdx] > hi) hi = share.pChaikinOscillatorSecond[i + s.mBeginIdx];
                if (share.pChaikinOscillatorSecond[i + s.mBeginIdx] < lo) lo = share.pChaikinOscillatorSecond[i + s.mBeginIdx];
            }

            g.setColor(C.COLOR_FADE_YELLOW);

            float[] tmp = {0};
            short[] yy = {0, 0};
            pricesToYs(tmp, 0, yy, 1, lo, hi);
            g.drawLine(0, yy[1], getW() - 20, yy[1]);
            g.setColor(C.COLOR_FADE_YELLOW0);
            g.drawString(mFont, "0", getW() - 20, yy[1], xGraphics.VCENTER);
            //==============================
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("{0:F2}", hi);
            g.drawString(mFont, sb.ToString(), getW() - 2, 0, xGraphics.RIGHT);

            sb.Length = 0;
            sb.AppendFormat("{0:F2}", lo);
            g.drawString(mFont, sb.ToString(), getW() - 2, getH()-mFont.Height, xGraphics.RIGHT);
            //==============================
            g.setColor(C.COLOR_ORANGE);
            sb = Utils.getSB();
            int idx = s.getCursor();
            sb.AppendFormat("Chaikin({0},{1}): {2}", (int)mContext.mOptChaikin0[0], (int)mContext.mOptChaikin1[0], share.pChaikinOscillator[idx]);
            g.drawString(mFont, sb.ToString(), 2, 1, xGraphics.LEFT | xGraphics.TOP);
            g.drawLines(mChartLineXY, mChartLineLength, 2.0f);

            if (mContext.mOptChaikinOscillatorEMA > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                sb = Utils.getSB();
                sb.AppendFormat("    MA({0:F0})", mContext.mOptChaikinOscillatorEMA);
                g.drawString(mFont, sb.ToString(), 150, 1, xGraphics.LEFT | xGraphics.TOP);

                g.drawLines(mChartEMA, mChartLineLength, 1.0f);
            }

            if (mHasSecondChart)
            {
                g.setColor(C.COLOR_SECOND_CHART);
                sb = Utils.getSB();
                sb.AppendFormat("Chaikin#2({0},{1}): {2}", (int)mContext.mOptChaikin0[1], (int)mContext.mOptChaikin1[1], share.pChaikinOscillatorSecond[idx]);
                g.drawString(mFont, sb.ToString(), 200, 1, xGraphics.LEFT | xGraphics.TOP);

                g.drawLines(mChartLineXY2, mChartLineLength, 1.5f);
            }

            mMouseTitle = "";
            renderCursor(g);
        }

        float hi = -0x0fffff;
        float lo = 0xffffff;
        protected void drawChartADL(xGraphics g)
        {
            int mX = 0;
            int mY = 0;

            Share s = getShare();
            if (s == null)
                return;

            Share share = s;

            if (detectShareCursorChanged())
            {
                s.calcADL();

                hi = -10000000;
                lo = 10000000;

                for (int i = 0; i < mChartLineLength; i++)
                {
                    if (share.pADL[i + s.mBeginIdx] > hi) hi = share.pADL[i + s.mBeginIdx];
                    if (share.pADL[i + s.mBeginIdx] < lo) lo = share.pADL[i + s.mBeginIdx];                }
                //============================================
                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);
                mChartLineXY3 = allocMem(mChartLineXY3, mChartLineLength * 2);

                pricesToYs(share.pADL, s.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);
                pricesToYs(share.pADL_SMA0, s.mBeginIdx, mChartLineXY2, mChartLineLength, lo, hi);
                pricesToYs(share.pADL_SMA1, s.mBeginIdx, mChartLineXY3, mChartLineLength, lo, hi);
            }

            if (mShouldDrawGrid)
                drawGrid(g);

            //======================
            g.setColor(C.COLOR_ORANGE);
            g.drawString(mFont, "ADL ", 10, 1, xGraphics.LEFT | xGraphics.TOP);
            g.drawLines(mChartLineXY, mChartLineLength, 2.0f);

            StringBuilder sb = Utils.getSB();
            if (mContext.mOptADL_SMA[0] > 0)
            {
                g.setColor(C.COLOR_MAGENTA);

                sb.AppendFormat("EMA({0:F0})", mContext.mOptADL_SMA[0]);
                g.drawString(mFont, sb.ToString(), 50, 1, xGraphics.LEFT | xGraphics.TOP);

                g.drawLines(mChartLineXY2, mChartLineLength, 1.5f);
            }

            if (mContext.mOptADL_SMA[1] > 0)
            {
                g.setColor(C.COLOR_BLUE_LIGHT);
                sb = Utils.getSB();
                sb.AppendFormat("EMA({0:F0})", mContext.mOptADL_SMA[1]);
                g.drawString(mFont, sb.ToString(), 110, 1, xGraphics.LEFT | xGraphics.TOP);

                g.drawLines(mChartLineXY3, mChartLineLength, 1.0f);
            }
            mTitle = null;
            mMouseTitle = Utils.formatNumber((int)yToPrice(mLastY, lo, hi));
            renderCursor(g);
        }

        protected void drawChartROC(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            int i = 0;
            Share s = getShare();
            if (s == null)
                return;

            Share share = s;
            if (detectShareCursorChanged())
            {
                s.calcROC(0);
                if (mHasSecondChart)
                {
                    s.mIsCalcROC = false;
                    s.calcROC(1);
                }
                //===========================
                lo = -14;
                hi = 14;
                for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                {
                    if (share.pROC[i] < lo) lo = share.pROC[i];
                    if (share.pROC[i] > hi) hi = share.pROC[i];

                    if (share.pROCSecond[i] < lo) lo = share.pROCSecond[i];
                    if (share.pROCSecond[i] > hi) hi = share.pROCSecond[i];
                }

                float max = (Utils.ABS_FLOAT(hi) > Utils.ABS_FLOAT(lo)) ? Utils.ABS_FLOAT(hi) : Utils.ABS_FLOAT(lo);
                hi = max;
                lo = -max;

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                pricesToYs(share.pROC, s.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);
                if (mHasSecondChart)
                {
                    mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);
                    pricesToYs(share.pROCSecond, s.mBeginIdx, mChartLineXY2, mChartLineLength, lo, hi);
                }

                //==============================
                if (mContext.mOptROC_EMA > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);

                    if (mContext.mOptROC_EMA_ON[0])
                    {
                        Share.EMA(share.pROC, s.getCandleCount(), (int)mContext.mOptROC_EMA, s.pEMAIndicator);
                    }
                    else
                    {
                        Share.SMA(share.pROC, 0, s.getCandleCount(), (int)mContext.mOptROC_EMA, s.pEMAIndicator);
                    }
                    pricesToYs(s.pEMAIndicator, s.mBeginIdx, mChartEMA, mChartLineLength, lo, hi);
                }
            }
            /*
            for (i = 0; i < mChartLineLength; i++)
            {
                if (share.pROC[i + s.mBeginIdx] > hi) hi = share.pROC[i + s.mBeginIdx];
                if (share.pROC[i + s.mBeginIdx] < lo) lo = share.pROC[i + s.mBeginIdx];
            }
             */

            float[] tmp = { -10, 0, 10};
            short[] yy = { 0, 0, 0, 0, 0, 0};
            string[] ls = { "-10", "0%", "+10" };
            pricesToYs(tmp, 0, yy, 3, lo, hi);

            int gapLineH = getH() / 5;

            if (mShouldDrawGrid)
                drawGrid(g);

            for (int l = 0; l < 3; l++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, yy[l*2+1], getW() - 33, yy[l*2+1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ls[l], getW() - 33, yy[l*2+1], xGraphics.VCENTER);
            }
            //==============================
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("{0:F2}", hi);
            g.drawString(mFont, sb.ToString(), getW() - 2, 0, xGraphics.RIGHT);

            sb.Length = 0;
            sb.AppendFormat("{0:F2}", lo);
            g.drawString(mFont, sb.ToString(), getW() - 2, getH() - mFont.Height, xGraphics.RIGHT);
            //==============================
            g.setColor(C.COLOR_ORANGE);
            g.drawLines(mChartLineXY, mChartLineLength, 2.0f);
            if (mHasSecondChart)
            {
                g.setColor(C.COLOR_SECOND_CHART);
                g.drawLines(mChartLineXY2, mChartLineLength, 1.5f);
            }

            if (mContext.mOptROC_EMA > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                g.drawLines(mChartEMA, mChartLineLength, 1.0f);
            }

            sb.Length = 0;
            sb.AppendFormat("{0:F2} %", (float)yToPrice(mLastY, lo, hi));
            mMouseTitle = sb.ToString();
            renderCursor(g);
        }

        protected void drawChartNVI(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            int i = 0;
            Share s = getShare();
            if (s == null)
                return;
            Share share = s;
            if (detectShareCursorChanged())
            {
                s.calcNVI();
                //===========================
                lo = 20000;
                hi = -20000;
                for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                {
                    if (share.pNVI[i] < lo) lo = share.pNVI[i];
                    if (share.pNVI[i] > hi) hi = share.pNVI[i];
                }
                if (mContext.mOptNVI_EMA[0] > 0)
                {
                    for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                    {
                        if (share.pNVI_EMA1[i] < lo) lo = share.pNVI_EMA1[i];
                        if (share.pNVI_EMA1[i] > hi) hi = share.pNVI_EMA1[i];
                    }
                }
                if (mContext.mOptNVI_EMA[1] > 0)
                {
                    for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                    {
                        if (share.pNVI_EMA2[i] < lo) lo = share.pNVI_EMA2[i];
                        if (share.pNVI_EMA2[i] > hi) hi = share.pNVI_EMA2[i];
                    }
                }

                //float max = (Utils.ABS_FLOAT(hi) > Utils.ABS_FLOAT(lo)) ? Utils.ABS_FLOAT(hi) : Utils.ABS_FLOAT(lo);
                //hi = max;
                //lo = -max;

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                pricesToYs(share.pNVI, s.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);

                //==============================
                if (mContext.mOptNVI_EMA[0] > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);

                    pricesToYs(share.pNVI_EMA1, s.mBeginIdx, mChartEMA, mChartLineLength, lo, hi);
                }
                if (mContext.mOptNVI_EMA[1] > 0)
                {
                    mChartEMA2 = allocMem(mChartEMA2, mChartLineLength * 2);

                    pricesToYs(share.pNVI_EMA2, s.mBeginIdx, mChartEMA2, mChartLineLength, lo, hi);
                }
            }

            int gapLineH = getH() / 5;

            StringBuilder sb = Utils.getSB();
            //==========================
            float step = (hi - lo)/5;
            float[] tmp = { lo, lo+step, lo+2*step, lo + 3*step, lo+4*step, hi };
            short[] yy = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            pricesToYs(tmp, 0, yy, 5, lo, hi);

            if (mShouldDrawGrid)
                drawGrid(g);

            for (int l = 0; l < 5; l++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, yy[l * 2 + 1], getW() - 33, yy[l * 2 + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                sb.Length = 0;
                sb.AppendFormat("{0:F1}", tmp[l]);

                g.drawString(mFont, sb.ToString(), getW() - 33, yy[l * 2 + 1], xGraphics.VCENTER);
            }

            //==============================
            /*
            sb.AppendFormat("{0:F2}", hi);
            g.drawString(mFont, sb.ToString(), getW() - 2, 0, xGraphics.RIGHT);

            sb.Length = 0;
            sb.AppendFormat("{0:F2}", lo);
            g.drawString(mFont, sb.ToString(), getW() - 2, getH() - mFont.Height, xGraphics.RIGHT);
             */
            //==============================
            g.setColor(C.COLOR_ORANGE);
            g.drawLines(mChartLineXY, mChartLineLength, 1.0f);

            if (mContext.mOptNVI_EMA[0] > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                g.drawLines(mChartEMA, mChartLineLength, 2.0f);
            }

            if (mContext.mOptNVI_EMA[1] > 0)
            {
                g.setColor(C.COLOR_BLUE_LIGHT);
                g.drawLines(mChartEMA2, mChartLineLength, 1.0f);
            }

            sb.Length = 0;
            sb.AppendFormat("{0:F2}", (float)yToPrice(mLastY, lo, hi));
            mMouseTitle = sb.ToString();
            renderCursor(g);
        }

        protected void drawChartPVI(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            int i = 0;
            Share s = getShare();
            if (s == null)
                return;
            Share share = s;
            if (detectShareCursorChanged())
            {
                s.calcPVI();
                //===========================
                lo = 20000;
                hi = -20000;
                for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                {
                    if (share.pPVI[i] < lo) lo = share.pPVI[i];
                    if (share.pPVI[i] > hi) hi = share.pPVI[i];
                }
                if (mContext.mOptPVI_EMA[0] > 0)
                {
                    for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                    {
                        if (share.pPVI_EMA1[i] < lo) lo = share.pPVI_EMA1[i];
                        if (share.pPVI_EMA1[i] > hi) hi = share.pPVI_EMA1[i];
                    }
                }
                if (mContext.mOptPVI_EMA[1] > 0)
                {
                    for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                    {
                        if (share.pPVI_EMA2[i] < lo) lo = share.pPVI_EMA2[i];
                        if (share.pPVI_EMA2[i] > hi) hi = share.pPVI_EMA2[i];
                    }
                }

                //float max = (Utils.ABS_FLOAT(hi) > Utils.ABS_FLOAT(lo)) ? Utils.ABS_FLOAT(hi) : Utils.ABS_FLOAT(lo);
                //hi = max;
                //lo = -max;

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                pricesToYs(share.pPVI, s.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);

                //==============================
                if (mContext.mOptPVI_EMA[0] > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);

                    pricesToYs(share.pPVI_EMA1, s.mBeginIdx, mChartEMA, mChartLineLength, lo, hi);
                }
                if (mContext.mOptPVI_EMA[1] > 0)
                {
                    mChartEMA2 = allocMem(mChartEMA2, mChartLineLength * 2);

                    pricesToYs(share.pPVI_EMA2, s.mBeginIdx, mChartEMA2, mChartLineLength, lo, hi);
                }
            }

            int gapLineH = getH() / 5;

            StringBuilder sb = Utils.getSB();
            //==========================
            float step = (hi - lo) / 5;
            float[] tmp = { lo, lo + step, lo + 2 * step, lo + 3 * step, lo + 4 * step, hi };
            short[] yy = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            pricesToYs(tmp, 0, yy, 5, lo, hi);

            if (mShouldDrawGrid)
                drawGrid(g);

            for (int l = 0; l < 5; l++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, yy[l * 2 + 1], getW() - 33, yy[l * 2 + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                sb.Length = 0;
                sb.AppendFormat("{0:F1}", tmp[l]);

                g.drawString(mFont, sb.ToString(), getW() - 33, yy[l * 2 + 1], xGraphics.VCENTER);
            }

            //==============================
            /*
            sb.AppendFormat("{0:F2}", hi);
            g.drawString(mFont, sb.ToString(), getW() - 2, 0, xGraphics.RIGHT);

            sb.Length = 0;
            sb.AppendFormat("{0:F2}", lo);
            g.drawString(mFont, sb.ToString(), getW() - 2, getH() - mFont.Height, xGraphics.RIGHT);
             */
            //==============================
            g.setColor(C.COLOR_GREEN);
            g.drawLines(mChartLineXY, mChartLineLength, 1.0f);

            if (mContext.mOptPVI_EMA[0] > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                g.drawLines(mChartEMA, mChartLineLength, 2.0f);
            }

            if (mContext.mOptPVI_EMA[1] > 0)
            {
                g.setColor(C.COLOR_BLUE_LIGHT);
                g.drawLines(mChartEMA2, mChartLineLength, 1.0f);
            }

            sb.Length = 0;
            sb.AppendFormat("{0:F2}", (float)yToPrice(mLastY, lo, hi));
            mMouseTitle = sb.ToString();
            renderCursor(g);
        }

        protected void drawChartThuyPS(xGraphics g)
        {
            /*
            int mX = 0;
            int mY = 0;
            int i = 0;
            float min = 10000000;
            float max = 0;
            Share s = getShare();
            if (s == null)
                return;
            if (detectShareCursorChanged())
            {
                s.calcSMAThuyPS();

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);

                min = 10000000;
                max = 0;
                for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                {
                    if (share.pSMAThuyPS[i] > max) max = share.pSMAThuyPS[i];
                    if (share.pSMAThuyPS[i] < min) min = share.pSMAThuyPS[i];

                    if (share.pSMAThuyPS1[i] > max) max = share.pSMAThuyPS1[i];
                    if (share.pSMAThuyPS1[i] < min) min = share.pSMAThuyPS1[i];
                }

                min = 0;
                max = 100;
                pricesToYs(share.pSMAThuyPS, s.mBeginIdx, mChartLineXY, mChartLineLength, min, max);
                pricesToYs(share.pSMAThuyPS1, s.mBeginIdx, mChartLineXY2, mChartLineLength, min, max);
            }

            for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
            {
                if (share.pSMAThuyPS[i] > max) max = share.pSMAThuyPS[i];
                if (share.pSMAThuyPS[i] < min) min = share.pSMAThuyPS[i];

                if (share.pSMAThuyPS1[i] > max) max = share.pSMAThuyPS1[i];
                if (share.pSMAThuyPS1[i] < min) min = share.pSMAThuyPS1[i];
            }
            min = 0;
            max = 100;
            float[] tmp = { 50 };
            short[] yy = { 0, 0 };
            pricesToYs(tmp, 0, yy, 1, min, max);

            int gapLineH = getH() / 5;

            if (mShouldDrawGrid)
                drawGrid(g);

            g.setColor(C.GREY_LINE_COLOR3);
            g.drawLine(0, yy[1], getW() - 33, yy[1]);
            g.drawString(mFont, "%", getW() - 33, yy[1], xGraphics.VCENTER);

            g.setColor(C.COLOR_ORANGE);
            g.drawLines(mChartLineXY, mChartLineLength, 1.5f);

            g.setColor(C.COLOR_CYAN);
            g.drawLines(mChartLineXY2, mChartLineLength, 1.5f);

            renderCursor(g);
             */
        }

        protected void drawChartTrix(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            int i = 0;
            Share s = getShare();
            if (s == null)
                return;
            Share share = s;
            if (detectShareCursorChanged())
            {
                s.calcTrix();

                lo = 100;
                hi = -100;
                for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                {
                    if (share.pTrix[i] < lo) lo = share.pTrix[i];
                    if (share.pTrix[i] > hi) hi = share.pTrix[i];
                }
                float max = (Utils.ABS_FLOAT(hi) > Utils.ABS_FLOAT(lo)) ? Utils.ABS_FLOAT(hi) : Utils.ABS_FLOAT(lo);
                hi = max;
                lo = -max;
                //-------------TRIX-------------------
                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                pricesToYs(share.pTrix, s.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);
                //-------------EMA--------------------
                mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);
                pricesToYs(share.pTrixEMA, s.mBeginIdx, mChartLineXY2, mChartLineLength, lo, hi);

                float[] tmp = { 0};
                pricesToYs(tmp, 0, mPricelines, 1, lo, hi);
            }

            if (s.getCandleCount() < 5)
                return;

            if (mShouldDrawGrid)
                drawGrid(g);

            g.setColor(C.COLOR_FADE_YELLOW);
            g.drawLine(0, mPricelines[1], getW() - 33, mPricelines[1]);
            g.drawString(mFont, "0%", getW() - 33, mPricelines[1], xGraphics.VCENTER);
            
            //  TRIX
            g.setColor(C.COLOR_GREEN);
            g.drawLines(mChartLineXY, mChartLineLength, 1.5f);

            //  EMA
            g.setColor(C.COLOR_MAGENTA);
            g.drawLines(mChartLineXY2, mChartLineLength, 1.5f);

            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            sb.AppendFormat("{0:F2} %", yToPrice(mLastY, lo, hi));
            mMouseTitle = sb.ToString();
            renderCursor(g);
        }

        public override void setChartType(int type)
        {
            mCurrentShare = null;
            base.setChartType(type);
    
            this.CHART_BORDER_SPACING_Y = 5;
        }

        override public void toggleSecondChart()
        {
            if (mChartType == CHART_RSI 
                || mChartType == CHART_MFI
                || mChartType == CHART_CHAIKIN
                || mChartType == CHART_ROC
                )
            {
                mHasSecondChart = !mHasSecondChart;
                if (mHasSecondChart)
                {
                    mCurrentKey = 0;
                }
                invalidate();
            }
        }

        override public bool isSecondChartDisplaying()
        {
            return mHasSecondChart;
        }

        public void setSMAIdx(int sma)
        {
            mSMAIdx = sma;
        }

        public int getSMAIdx()
        {
            return mSMAIdx;
        }

        void drawChartPastOfYears(xGraphics g)
        {
            if (getShare(2) == null)
                return;

            Share share = getShare();

            float[] p = null;
            if (mChartType == CHART_PAST_1_YEAR)
            {
                p = share.pPastPrice1;
            }
            else
            {
                p = share.pPastPrice2;
            }
            if (detectShareCursorChanged())		//	share's cursor has been changed
            {
                if (mChartType == CHART_PAST_1_YEAR)
                {
                    share.calcYearOfPast(1);
                }
                else
                {
                    share.calcYearOfPast(2);    
                }

                lo = -1;
                hi = -1;
                lo = share.getLowestPrice();
                hi = share.getHighestPrice();
                /*
                for (int i = share.mBeginIdx; i <= share.mEndIdx; i++)
                {
                    if (lo == -1) lo = p[i];
                    if (hi == -1) hi = p[i];
                    if (p[i] < lo) lo = p[i];
                    if (p[i] > hi) hi = p[i];
                }
                 */
                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                
                mChartLineXY[0] = 0;
                mChartLineXY[1] = 0;

                int idx = share.mBeginIdx;
                if (p[0] != -1)
                    pricesToYs(p, idx, mChartLineXY, mChartLineLength, lo, hi);    
            }

            //  SMA1
            if ((mChartLineXY[0] != 0 || mChartLineXY[1] != 0))
            {
                int year = 1;
                int y = 0;
                int x = getW() - 160;
                uint color = 0;
                if (mChartType == CHART_PAST_1_YEAR)
                {
                    color = 0x90c0c000;
                    y = 40;
                }
                else
                {
                    year = 2;
                    y = 60;
                    color = 0x9000c0c0;
                }
                g.setColor(color);
                g.drawLines(mChartLineXY, mChartLineLength, mContext.mOptSMAThickness[mSMAIdx, 0]);
                //  title
                //g.setColor(mContext.mOptSMAColor[mSMAIdx, 0]);
                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                sb.AppendFormat("-{0} year price: {1:F2}", year, p[share.getCursor()]);
                String sz = sb.ToString();
                g.setColor((int)((0xff << 24) | color));
                g.drawString(mFont, sz, x, y, 0);
            }
        }

        void drawChartOfSecondShare(xGraphics g)
        {
            if (getShare(2) == null)
                return;

            Share share = getShare();

            if (detectShareCursorChanged())		//	share's cursor has been changed
            {
                share.calcComparingShare();

                lo = 100000;
                hi = -10000;

                for (int i = share.mBeginIdx; i < share.mEndIdx; i++)
                {
                    if (share.pComparingPrice[i] > hi) hi = share.pComparingPrice[i];
                    if (share.pComparingPrice[i] < lo) lo = share.pComparingPrice[i];
                }

                hi = hi + hi / 20.0f;
                lo -= lo / 20.0f;
                
                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                mChartLineXY[0] = 0;
                mChartLineXY[1] = 0;

                int idx = share.mBeginIdx;

                pricesToYs(share.pComparingPrice, idx, mChartLineXY, mChartLineLength, lo, hi);
            }

            if ((mChartLineXY[0] != 0 || mChartLineXY[1] != 0))
            {
                int year = 1;
                int y = 0;
                int x = getW() - 160;
                uint color = 0;
                
                color = 0x9000c000;
                y = 80;
                
                g.setColor(color);
                g.drawLines(mChartLineXY, mChartLineLength, mContext.mOptSMAThickness[mSMAIdx, 0]);
                //  title
                //g.setColor(mContext.mOptSMAColor[mSMAIdx, 0]);
                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                sb.AppendFormat(" {0}: {1:F2}", share.mCompare2ShareCode, share.pComparingPrice[share.getCursor()]);
                String sz = sb.ToString();
                g.setColor((int)((0xff << 24) | color));
                g.drawString(mFont, sz, x, y, 0);
            }
        }

        protected void drawChartSMA3(xGraphics g)
        {
            if (getShare(2) == null)
                return;

            Share share = getShare();
            int d1 = mSMAPeriod;

            if (detectShareCursorChanged())		//	share's cursor has been changed
            {
                share.calcSMA(d1, 0, 0, 0, 0);

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);

                mChartLineXY[0] = 0;
                mChartLineXY[1] = 0;

                int idx = share.mBeginIdx;
                if (share.pSMA1[0] != -1)
                    pricesToYs(share.pSMA1, idx, mChartLineXY, mChartLineLength, share.getLowestPrice(), share.getHighestPrice());
            }

            //  render title
            int x = 4;
            int y = getH() - mFont.Height - 55;
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            String sz;
           
            string ss = "SMA";

            //  SMA1
            if ((mChartLineXY[0] != 0 || mChartLineXY[1] != 0))
            {
                g.setColor(C.COLOR_GREEN);
                
                g.drawLines(mChartLineXY, mChartLineLength, 1.5f);
                //  title
                //g.setColor(mContext.mOptSMAColor[mSMAIdx, 0]);
                sb.AppendFormat("{0} ({1}): {2:F2}", ss, d1, share.pSMA1[share.getCursor()]);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);

                y -= mFont.Height;
            }
        }

        protected void drawChartSMA(xGraphics g)
        {
            if (getShare(2) == null)
                return;

            Share share = getShare();
            int d1 = (int)mContext.mOptSMA[mSMAIdx, 0];
            int d2 = (int)mContext.mOptSMA[mSMAIdx, 1];
            int d3 = (int)mContext.mOptSMA[mSMAIdx, 2];
            int d4 = (int)mContext.mOptSMA2[mSMAIdx, 0];
            int d5 = (int)mContext.mOptSMA2[mSMAIdx, 1];
                    
            if (detectShareCursorChanged())		//	share's cursor has been changed
            {
                /*
                if (mSMAIdx == 2)   //  filter SMA1 cut SMA2
                {
                    share.calcSMA(mContext.mOptFilterSMA1, mContext.mOptFilterSMA2, 0);
                }
                else
                 */
                {
                    if (mContext.mIsSMA[mSMAIdx])
                        share.calcSMA(d1, d2, d3, d4, d5);
                    else
                        share.calcEMA(d1, d2, d3, d4, d5);
                }

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                mChartLineXY2 = allocMem(mChartLineXY2, mChartLineLength * 2);
                mChartLineXY3 = allocMem(mChartLineXY3, mChartLineLength * 2);
                mChartLineXY4 = allocMem(mChartLineXY4, mChartLineLength * 2);
                mChartLineXY5 = allocMem(mChartLineXY5, mChartLineLength * 2);

                mChartLineXY[0] = 0;
                mChartLineXY[1] = 0;

                mChartLineXY2[0] = 0;
                mChartLineXY2[1] = 0;
                mChartLineXY3[0] = 0;
                mChartLineXY3[1] = 0;

                mChartLineXY4[0] = 0;
                mChartLineXY4[1] = 0;
                mChartLineXY5[0] = 0;
                mChartLineXY5[1] = 0;

                int idx = share.mBeginIdx;
                if (share.pSMA1[0] != -1)
                    pricesToYs(share.pSMA1, idx, mChartLineXY, mChartLineLength, share.getLowestPrice(), share.getHighestPrice());
                if (share.pSMA2[0] != -1)
                    pricesToYs(share.pSMA2, idx, mChartLineXY2, mChartLineLength, share.getLowestPrice(), share.getHighestPrice());
                if (share.pSMA3[0] != -1)
                    pricesToYs(share.pSMA3, idx, mChartLineXY3, mChartLineLength, share.getLowestPrice(), share.getHighestPrice());
                if (share.pSMA4[0] != -1)
                    pricesToYs(share.pSMA4, idx, mChartLineXY4, mChartLineLength, share.getLowestPrice(), share.getHighestPrice());
                if (share.pSMA5[0] != -1)
                    pricesToYs(share.pSMA5, idx, mChartLineXY5, mChartLineLength, share.getLowestPrice(), share.getHighestPrice());
            }

            //  render title
            int x = 4;
            int y = getH() - mFont.Height - 55;
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            String sz;

            bool[] sma = { mContext.mOptSMAUse[mSMAIdx, 0], mContext.mOptSMAUse[mSMAIdx, 1], mContext.mOptSMAUse[mSMAIdx, 2],
                            mContext.mOptSMAUse2[mSMAIdx, 0], mContext.mOptSMAUse2[mSMAIdx, 1]};
            int[] smaDays = { d1, d2, d3, d4, d5};
            /*
            if (mSMAIdx == 2)
            {
                sma[0] = mContext.mOptFilterSMA1 > 0;
                sma[1] = mContext.mOptFilterSMA2 > 0;
                sma[2] = false;

                smaDays[0] = mContext.mOptFilterSMA1;
                smaDays[1] = mContext.mOptFilterSMA2;
                smaDays[2] = 0;
            }
            */
            string ss = "SMA";
            if (mSMAIdx < 2)
            {
                if (!mContext.mIsSMA[mSMAIdx]) ss = "EMA";
            }

            //  SMA1
            if (sma[0] && (mChartLineXY[0] != 0 || mChartLineXY[1] != 0))
            {
                if (mSMAIdx == 2)
                    g.setColor(C.COLOR_MAGENTA);
                else
                    g.setColor(mContext.mOptSMAColor[mSMAIdx, 0]);
                g.drawLines(mChartLineXY, mChartLineLength, mContext.mOptSMAThickness[mSMAIdx, 0]);
                //  title
                //g.setColor(mContext.mOptSMAColor[mSMAIdx, 0]);
                sb.AppendFormat("{0} {1}({2}): {3:F2}", ss, 1, smaDays[0], share.pSMA1[share.getCursor()]);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);

                y -= mFont.Height;
            }
            //  SMA2
            if (sma[1] && (mChartLineXY2[0] != 0 || mChartLineXY2[1] != 0))
            {
                if (mSMAIdx == 2)
                    g.setColor(C.COLOR_BLUE_LIGHT);
                else
                    g.setColor(mContext.mOptSMAColor[mSMAIdx, 1]);
                g.drawLines(mChartLineXY2, mChartLineLength, mContext.mOptSMAThickness[mSMAIdx, 1]);
                //  title
                //g.setColor(mContext.mOptSMAColor[mSMAIdx, 1]);
                sb.Length = 0;
                sb.AppendFormat("{0} {1}({2}): {3:F2}", ss, 2, smaDays[1], share.pSMA2[share.getCursor()]);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);

                y -= mFont.Height;
            }
            //  SMA3
            if (sma[2] && (mChartLineXY3[0] != 0 || mChartLineXY3[1] != 0))
            {
                //if (mSMAIdx < 2)
                    g.setColor(mContext.mOptSMAColor[mSMAIdx, 2]);
                g.drawLines(mChartLineXY3, mChartLineLength, mContext.mOptSMAThickness[mSMAIdx, 2]);
                //  title
                //g.setColor(mContext.mOptSMAColor[mSMAIdx, 2]);
                sb.Length = 0;
                sb.AppendFormat("{0} {1}({2}):{3:F2}", ss, 3, smaDays[2], share.pSMA3[share.getCursor()]);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);
                y -= mFont.Height;
            }
            //  SMA4
            if (sma[3] && (mChartLineXY4[0] != 0 || mChartLineXY4[1] != 0))
            {
                //if (mSMAIdx < 2)
                    g.setColor(mContext.mOptSMAColor2[mSMAIdx, 0]);
                g.drawLines(mChartLineXY4, mChartLineLength, mContext.mOptSMAThickness2[mSMAIdx, 0]);
                //  title
                //g.setColor(mContext.mOptSMAColor[mSMAIdx, 2]);
                sb.Length = 0;
                sb.AppendFormat("{0} {1}({2}):{3:F2}", ss, 4, smaDays[3], share.pSMA4[share.getCursor()]);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);
                y -= mFont.Height;
            }

            //  SMA5
            if (sma[4] && (mChartLineXY5[0] != 0 || mChartLineXY5[1] != 0))
            {
                //if (mSMAIdx < 2)
                    g.setColor(mContext.mOptSMAColor2[mSMAIdx, 1]);
                g.drawLines(mChartLineXY5, mChartLineLength, mContext.mOptSMAThickness2[mSMAIdx, 1]);
                //  title
                //g.setColor(mContext.mOptSMAColor[mSMAIdx, 2]);
                sb.Length = 0;
                sb.AppendFormat("{0} {1}({2}):{3:F2}", ss, 5, smaDays[4], share.pSMA5[share.getCursor()]);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);
            }
        }

        public bool isSupportSMA()
        {
            if (mChartType == CHART_RSI
                || mChartType == CHART_MFI
                || mChartType == CHART_CHAIKIN
                || mChartType == CHART_ROC)
                return true;

            return false;
        }

        protected void drawChartPVT(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            int i = 0;
            Share s = getShare();
            if (s == null)
                return;
            Share share = s;
            if (detectShareCursorChanged())
            {
                s.calcPVT();
                //===========================
                lo = -1;
                hi = -1;
                for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                {
                    if (lo == -1) lo = share.pPVT[i];
                    if (hi == -1) hi = share.pPVT[i];
                    if (share.pPVT[i] < lo) lo = share.pPVT[i];
                    if (share.pPVT[i] > hi) hi = share.pPVT[i];
                }
                if (mContext.mOptPVT > 0)
                {
                    for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                    {
                        if (share.pPVT_EMA1[i] < lo) lo = share.pPVT_EMA1[i];
                        if (share.pPVT_EMA1[i] > hi) hi = share.pPVT_EMA1[i];
                    }
                }

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                pricesToYs(share.pPVT, s.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);

                //==============================
                if (mContext.mOptPVT > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);

                    pricesToYs(share.pPVT_EMA1, s.mBeginIdx, mChartEMA, mChartLineLength, lo, hi);
                }
            }

            int gapLineH = getH() / 5;

            StringBuilder sb = Utils.getSB();
            //==========================
            float step = (hi - lo) / 5;
            float[] tmp = { lo, lo + step, lo + 2 * step, lo + 3 * step, lo + 4 * step, hi };
            short[] yy = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            pricesToYs(tmp, 0, yy, 5, lo, hi);

            if (mShouldDrawGrid)
                drawGrid(g);

            for (int l = 0; l < 5; l++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, yy[l * 2 + 1], getW() - 33, yy[l * 2 + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                sb.Length = 0;
                sb.AppendFormat("{0:F1}", tmp[l]);

                g.drawString(mFont, sb.ToString(), getW() - 33, yy[l * 2 + 1], xGraphics.VCENTER);
            }

            //==============================
            /*
            sb.AppendFormat("{0:F2}", hi);
            g.drawString(mFont, sb.ToString(), getW() - 2, 0, xGraphics.RIGHT);

            sb.Length = 0;
            sb.AppendFormat("{0:F2}", lo);
            g.drawString(mFont, sb.ToString(), getW() - 2, getH() - mFont.Height, xGraphics.RIGHT);
             */
            //==============================
            g.setColor(C.COLOR_ORANGE);
            g.drawLines(mChartLineXY, mChartLineLength, 1.0f);

            if (mContext.mOptPVT > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                g.drawLines(mChartEMA, mChartLineLength, 2.0f);
            }

            sb.Length = 0;
            sb.AppendFormat("{0:F2}", (float)yToPrice(mLastY, lo, hi));
            mMouseTitle = sb.ToString();
            renderCursor(g);
        }

        protected void drawChartCCI(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            int i = 0;
            Share s = getShare();
            if (s == null)
                return;
            Share share = s;
            if (detectShareCursorChanged())
            {
                s.calcCCI();
                //===========================
                lo = -1;
                hi = -1;
                for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                {
                    if (lo == -1) lo = share.pCCI[i];
                    if (hi == -1) hi = share.pCCI[i];
                    if (share.pCCI[i] < lo) lo = share.pCCI[i];
                    if (share.pCCI[i] > hi) hi = share.pCCI[i];
                }
                if (hi > 300) hi = 300;
                if (hi < 100) hi = 100;

                if (lo < -300) lo = -300;
                if (lo > -100) lo = -100;

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                pricesToYs(share.pCCI, s.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);
            }

            int gapLineH = getH() / 5;

            StringBuilder sb = Utils.getSB();
            //==========================
            float[] tmp = { 150, 100, 0, -100, -150 };
            short[] yy = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            pricesToYs(tmp, 0, yy, 5, lo, hi);

            if (mShouldDrawGrid)
                drawGrid(g);

            for (int l = 0; l < 5; l++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, yy[l * 2 + 1], getW() - 33, yy[l * 2 + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                sb.Length = 0;
                sb.AppendFormat("{0:F1}", tmp[l]);

                g.drawString(mFont, sb.ToString(), getW() - 33, yy[l * 2 + 1], xGraphics.VCENTER);
            }

            //==============================
            int j = 0;

            int y100 = yy[2 * 1 + 1];
            int y_100 = yy[2 * 3 + 1];
            fillColorGreen(g, mChartLineXY, mChartLineLength, (short)y100);
            fillColorRed(g, mChartLineXY, mChartLineLength, (short)y_100);
            //==============================
            g.setColor(C.COLOR_ORANGE);
            g.drawLines(mChartLineXY, mChartLineLength, 1.0f);

            sb.Length = 0;
            sb.AppendFormat("{0:F2}", (float)yToPrice(mLastY, lo, hi));
            mMouseTitle = sb.ToString();
            renderCursor(g);
        }

        protected void drawChartMassIndex(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            int i = 0;
            Share s = getShare();
            if (s == null)
                return;
            Share share = s;
            if (detectShareCursorChanged())
            {
                s.calcMassIndex();
                //===========================
                lo = -1;
                hi = -1;
                for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                {
                    if (lo == -1) lo = share.pMassIndex[i];
                    if (hi == -1) hi = share.pMassIndex[i];
                    if (share.pMassIndex[i] < lo) lo = share.pMassIndex[i];
                    if (share.pMassIndex[i] > hi) hi = share.pMassIndex[i];
                }
                if (hi < 28)
                    hi = 28;

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                pricesToYs(share.pMassIndex, s.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);
            }

            int gapLineH = getH() / 5;

            StringBuilder sb = Utils.getSB();
            //==========================
            float[] tmp = { 27, 26.5f };
            short[] yy = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            pricesToYs(tmp, 0, yy, 2, lo, hi);

            if (mShouldDrawGrid)
                drawGrid(g);

            for (int l = 0; l < 2; l++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, yy[l * 2 + 1], getW() - 33, yy[l * 2 + 1]);
                g.setColor(C.COLOR_FADE_YELLOW0);
                sb.Length = 0;
                sb.AppendFormat("{0:F1}", tmp[l]);

                g.drawString(mFont, sb.ToString(), getW() - 33, yy[l * 2 + 1], xGraphics.VCENTER);
            }

            //==============================
            g.setColor(C.COLOR_ORANGE);
            g.drawLines(mChartLineXY, mChartLineLength, 1.0f);

//            sb.Length = 0;
//            sb.AppendFormat("{0:F2}", (float)yToPrice(mLastY, lo, hi));
            mMouseTitle = null;// sb.ToString();
            renderCursor(g);
        }

        void drawChartATR(xGraphics g)
        {
            int mX = 0;
            int mY = 0;
            int i = 0;
            Share s = getShare();
            if (s == null)
                return;
            Share share = s;
            if (detectShareCursorChanged())
            {
                s.calcATR();
                //===========================
                lo = 0x0fffffff;
                hi = -0x0fffffff;
                for (i = s.mBeginIdx; i <= s.mEndIdx; i++)
                {
                    if (share.pATR[i] < lo) lo = share.pATR[i];
                    if (share.pATR[i] > hi) hi = share.pATR[i];
                }

                mChartLineXY = allocMem(mChartLineXY, mChartLineLength * 2);
                pricesToYs(share.pATR, s.mBeginIdx, mChartLineXY, mChartLineLength, lo, hi);
/*
                if (mContext.mOptATR_EMA > 0)
                {
                    mChartEMA = allocMem(mChartEMA, mChartLineLength * 2);
                    Share.EMA(share.pATR, s.getCandleCount(), (int)mContext.mOptATR_EMA, share.pTMP);
                    pricesToYs(share.pTMP, s.mBeginIdx, mChartEMA, mChartLineLength, lo, hi);
                }
 */
            }
            //==========================
            //mShouldDrawGrid = true;
            if (mShouldDrawGrid)
                drawGrid(g);

            //==============================
            g.setColor(C.COLOR_ORANGE);
            g.drawLines(mChartLineXY, mChartLineLength, 1.5f);
            drawPriceLines(g, share.pATR);
/*
            if (AppContext.getInstance().mOptATR_EMA > 0)
            {
                g.setColor(C.COLOR_MAGENTA);
                g.drawLines(mChartEMA, mChartLineLength, 1.0f);
            }
            */
            mMouseTitle = null;
            renderCursor(g);
        }
    }
}
