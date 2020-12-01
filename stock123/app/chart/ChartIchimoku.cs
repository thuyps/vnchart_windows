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
    public class ChartIchimoku:ChartBase
    {
        short[] mLineTenkansen;
        short[] mLineKijunsen;
        short[] mLineChikou;
        short[] mLineSpanA;
        short[] mLineSpanB;
        int mLineTenkansenSize;
        int mLineKijunsenSize;
        int mLineSpanASize;
        int mLineSpanBSize;
        int mLineChikouSize;

        int mChikouLineLength;

        xVector mKumoCloud = new xVector(40); //  short[]
        xVectorInt mKumoCloudItemLength = new xVectorInt(100);
        xVectorInt mKumoCloudColor = new xVectorInt(100);
        //======================================
        public ChartIchimoku(Font f):base(f)
        {
            mShouldDrawValue = true;
            mChartType = CHART_ICHIMOKU;
            CHART_BORDER_SPACING_Y = CHART_BORDER_SPACING_Y_MASTER;
        }

        public override void render(xGraphics g)
        {
            int mH = getH();
            int mW = getW();

            if (isHiding())
                return;
            Share share = mContext.getSelectedDrawableShare(3);

            if (share == null)
                return;
            if (detectShareCursorChanged())
            {
                share.calcIchimoku();

                int newSize = mChartLineLength * 2;
                mLineTenkansen = allocMem(mLineTenkansen, newSize);
                mLineKijunsen = allocMem(mLineKijunsen, newSize);
                mLineChikou = allocMem(mLineChikou, newSize);
                mLineSpanA = allocMem(mLineSpanA, newSize + 1000);
                mLineSpanB = allocMem(mLineSpanB, newSize + 1000);

                int idx = share.mBeginIdx;
                pricesToYs(Share.pTenkansen, idx, mLineTenkansen, mChartLineLength, false);
                pricesToYs(Share.pKijunsen, idx, mLineKijunsen, mChartLineLength, false);
                pricesToYs(Share.pChikouSpan, idx, mLineChikou, mChartLineLength, false);
                //  correct the chikou length
                int chikouCnt = share.getCandleCount() - (int)mContext.mOptIchimokuTime2;
                mChikouLineLength = mChartLineLength;
                if (idx + mChikouLineLength > chikouCnt)
                    mChikouLineLength = chikouCnt - idx;

                int kumoLen = mChartLineLength + (int)mContext.mOptIchimokuTime2;    //  shift 26 days ahead
                pricesToYs(Share.pSpanA, idx, mLineSpanA, kumoLen, false);
                pricesToYs(Share.pSpanB, idx, mLineSpanB, kumoLen, false);

                //  kumo cloud
                uint[] colors = { 0x40f06070, 0x40f0f0f0 };   //  bronw/white
                int t = mLineSpanA[1] > mLineSpanB[1] ? 0 : 1;
                xVectorInt vinters = new xVectorInt(100);
                xVectorInt vcolors = new xVectorInt(100);
                vinters.addElement(0);
                vcolors.addElement((int)colors[t]);

                for (int i = 0; i < kumoLen; i++)
                {
                    int j = 2 * i + 1;
                    if (t == 0)
                    {
                        if (mLineSpanA[j] < mLineSpanB[j])
                        {
                            t = 1 - t;
                            vinters.addElement(i);
                            vcolors.addElement((int)colors[t]);
                        }
                    }
                    else if (t == 1)
                    {
                        if (mLineSpanA[j] > mLineSpanB[j])
                        {
                            t = 1 - t;
                            vinters.addElement(i);
                            vcolors.addElement((int)colors[t]);
                        }
                    }
                }
                //===========================
                vinters.addElement(kumoLen - 1);
                vcolors.addElement(0);  //  dont care this time
                clearKumoCloud();
                //  create kumo cloud
                for (int i = 1; i < vinters.size(); i++)
                {
                    int b = vinters.elementAt(i - 1);
                    int e = vinters.elementAt(i);
                    short[] cloud = new short[((e - b + 10) * 4)];

                    int j = 0;
                    for (int k = b; k <= e; k++)
                    {
                        cloud[j++] = mLineSpanA[2 * k];
                        cloud[j++] = mLineSpanA[2 * k + 1];
                    }
                    cloud[j++] = mLineSpanB[2 * e];
                    cloud[j++] = mLineSpanB[2 * e + 1];
                    for (int k = e; k >= b; k--)
                    {
                        cloud[j++] = mLineSpanB[2 * k];
                        cloud[j++] = mLineSpanB[2 * k + 1];
                    }

                    mKumoCloud.addElement(cloud);
                    mKumoCloudItemLength.addElement(j / 2);
                    mKumoCloudColor.addElement(vcolors.elementAt(i - 1));
                }
            }

            if (mChartLineLength == 0)
                return;

            //===============================================
            //  tenkansen: pink
            g.setColor(0xc0ff00ff);
            g.drawLines(mLineTenkansen, mChartLineLength);

            //  kijunsen: 
            g.setColor(0xc00050ff);
            g.drawLines(mLineKijunsen, mChartLineLength);

            //  chikou
            if (mChikouLineLength > 0)
            {
                g.setColor(0xc0907070);
                g.drawLines(mLineChikou, mChikouLineLength);
            }
            //  spanA
            g.setColor(0xb000a050);
            g.drawLines(mLineSpanA, mChartLineLength + (int)mContext.mOptIchimokuTime2);
            //  spanB
            g.setColor(0xb0ff0000);
            g.drawLines(mLineSpanB, mChartLineLength + (int)mContext.mOptIchimokuTime2);

            //  kumo cloud
            for (int i = 0; i < mKumoCloud.size(); i++)
            {
                short[] p = (short[])mKumoCloud.elementAt(i);
                if (p != null)
                {
                    g.setColor(mKumoCloudColor.elementAt(i));
                    g.fillShapes(p, mKumoCloudItemLength.elementAt(i));
                }
            }

            //  info
            String sz;
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            int cur = share.getCursor();
            int y = 24;
            if (mShouldDrawValue && cur >= 0 && cur < share.getCandleCount())
            {
                g.setColor(0x60404040);
                if (cur - mContext.mOptIchimokuTime2 > 0)
                {
                    int x0 = candleToX(cur - mContext.mOptIchimokuTime2);
                    g.drawVerticalLine(x0, 0, getH());
                }
                int x1 = candleToX(cur + mContext.mOptIchimokuTime2);
                g.drawVerticalLine(x1, 0, getH());

                //==========draw 2 shadow vertical
                g.setColor(0xffffa000);
                sb.AppendFormat("Ichi({0},{1},{2})", (int)mContext.mOptIchimokuTime1, (int)mContext.mOptIchimokuTime2, (int)mContext.mOptIchimokuTime3);
                sz = sb.ToString();
                g.drawString(mFont, sz, 130, y, 0);
                int x = 150 + g.getStringWidth(mFont, sz) + 10;

                String sz1;
                sz1 = formatPrice(Share.pTenkansen[cur]);
                //  tenkansen
                g.setColor(0xc0ff00ff);
                sb.Length = 0;
                sb.AppendFormat("Tenkan: {0}", sz1);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);
                x += g.getStringWidth(mFont, sz) + 10;
                //  kijun
                g.setColor(0xc00050ff);
                sz1 = formatPrice(Share.pKijunsen[cur]);
                sb.Length = 0;
                sb.AppendFormat("Kijun: {0}", sz1);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);
                x += g.getStringWidth(mFont, sz) + 10;
                //  chikou
                g.setColor(0xc0a0a0a0);
                float chikou = 0;
                if (cur < share.getCandleCount() - mContext.mOptIchimokuTime2)
                    chikou = Share.pChikouSpan[cur];
                sz1 = formatPrice(chikou);
                sb.Length = 0;
                sb.AppendFormat("Chikou: {0}", sz1);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);
                x += g.getStringWidth(mFont, sz) + 10;
                //  spanA
                g.setColor(0xff00a050);
                sz1 = formatPrice(Share.pSpanA[cur]);
                sb.Length = 0;
                sb.AppendFormat("SpanA: {0}", sz1);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);
                x += g.getStringWidth(mFont, sz) + 10;
                //  spanB
                g.setColor(0xb0ff0000);
                sz1 = formatPrice(Share.pSpanB[cur]);
                sb.Length = 0;
                sb.AppendFormat("SpanB: {0}", sz1);
                sz = sb.ToString();
                g.drawString(mFont, sz, x, y, 0);
            }
        }
        void clearKumoCloud()
        {
            mKumoCloud.removeAllElements();
            mKumoCloudColor.removeAllElements();
            mKumoCloudItemLength.removeAllElements();
        }
    }
}
