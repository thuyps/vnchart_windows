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
    public class ChartVolumeByPrice: ChartBase
    {
        String mHighestVolume;
        short[] mSMAVolumeXY;
        xVector mBlocks = new xVector(12);
        //=======================================
        class vBlock
        {
            public double positiveVolume;
            public double negativeVolume;
            public int price;
            public int beginPrice;
            public int endPrice;
        };

        public ChartVolumeByPrice(Font f)
            : base(f)
        {
            mChartType = CHART_VOLUMEBYPRICE;
            //CHART_BORDER_SPACING_Y = CHART_BORDER_SPACING_Y_MASTER;
        }

        void calcVolumeByPrice()
        {
            Share share = getShare();
            if (share.getCandleCount() < 5)
                return;

            float highest = share.getHighestPrice();
            float lowest = share.getLowestPrice();
            float step = (float)((highest - lowest) / 12);
            mBlocks.removeAllElements();

            int i = 0;
            for (i = 0; i < 12; i++)
            {
                vBlock v = new vBlock();
                v.beginPrice = (int)(lowest + i * step);
                v.endPrice = (int)(lowest + (i + 1) * step);
                v.price = (int)(v.beginPrice + step / 2);

                mBlocks.addElement(v);
            }

            for (int j = share.mBeginIdx; j <= share.mEndIdx; j++)
            {
                float price = share.getClose(j);
                vBlock v = getvBlock(price);
                if (v != null)
                {
                    int signal = 1;
                    if (j < share.mEndIdx)
                    {
                        signal = (share.getClose(j) < share.getClose(j + 1)) ? 1 : -1;
                    }

                    if (signal > 0)
                        v.positiveVolume += share.getVolume(j);
                    else
                        v.negativeVolume += share.getVolume(j);
                }
            }
            //==========================================
            mCurrentShare = share;
        }

        vBlock getvBlock(float price)
        {
            for (int i = 0; i < mBlocks.size(); i++)
            {
                vBlock v = (vBlock)mBlocks.elementAt(i);
                if (v.beginPrice <= price && price <= v.endPrice)
                    return v;
            }

            return null;
        }

        public override void render(xGraphics g)
        {
            if (isHiding())
                return;
            if (getShare(3) == null)
                return;
            Share share = getShare();
            int mX = 0;
            int mY = 0;
            if (detectShareCursorChanged())
            {
                calcVolumeByPrice();
            }

            int i = 0;
            vBlock v;
            double maxTotalVol = 0;
            //  get max total volume
            for (i = 0; i < mBlocks.size(); i++)
            {
                v = (vBlock)mBlocks.elementAt(i);
                if (v.positiveVolume + v.negativeVolume > maxTotalVol)
                    maxTotalVol = v.positiveVolume + v.negativeVolume;
            }
            if (maxTotalVol == 0)
                return;
            int w = getW()/2;
            float rx = (float)(w/maxTotalVol);

            for (i = 0; i < mBlocks.size(); i++)
            {
                v = (vBlock)mBlocks.elementAt(i);

                float bottomY = priceToY(v.beginPrice);
                float topY = priceToY(v.endPrice);
                //  green block
                g.setColor(0x4000ff00);
                float bw1 = (float)(v.positiveVolume * rx);
                g.fillRect(0, topY+1, bw1, bottomY - topY-2);
                g.setColor(0xa000ff00);
                //g.drawRect(0, topY + 1, bw1, bottomY - topY - 2);

                //  red block
                g.setColor(0x40ff0000);
                float bw2 = (float)(v.negativeVolume * rx);
                g.fillRect(bw1, topY+1, bw2, bottomY - topY-2);
                g.setColor(0xa0ff0000);
                //g.drawRect(bw1, topY + 1, bw2, bottomY - topY - 2);
            }
        }

        public override String getTitle()
        {
            Share share = getShare(3);
            if (share != null)
            {
                int idx = share.getCursor();
                {
                    int v = share.getVolume(idx);
                    String s = Utils.formatNumber(v);
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat("Vol: {0}", s);
                    return Utils.sb.ToString();
                }
            }
            return "0";
        }
    }
}
