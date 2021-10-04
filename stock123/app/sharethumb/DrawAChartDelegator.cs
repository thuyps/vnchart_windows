using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using xlib.framework;
using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;
using stock123.xlib.ui;

using System.Drawing;


namespace stock123.app.sharethumb
{
    public class DrawAChartDelegator
    {
        float[] closes;
        float[] opens;
        float[] highs;
        float[] lows;
        int[] volumes;
        int[] dates;
        int candleCount;
        protected float mPriceDistance;
        protected int mChartLineLength;
        protected short[] mChartLineXY;

        float priceMax;
        float priceMin;

        int beginIdx;
        int endIdx;
        String mCode;
        int daysRange;
        //xBaseControl view;
        xGraphics graphics;
        Rectangle rcView;

        public DrawAChartDelegator()
        {
            init();
        }

        static public void renderToView(String code, xGraphics g, xBaseControl view)
        {
            DrawAChartDelegator sharedInstance = (DrawAChartDelegator)GlobalData.vars().getValue("DrawAChartDelegator");

            if (sharedInstance == null)
            {
                sharedInstance = new DrawAChartDelegator();
                GlobalData.vars().setValue("DrawAChartDelegator", sharedInstance);
            }

            sharedInstance._renderToView(code, g, view);
        }

        static public void renderToView(String code, xGraphics g, System.Drawing.Rectangle rc)
        {
            DrawAChartDelegator sharedInstance = (DrawAChartDelegator)GlobalData.vars().getValue("DrawAChartDelegator");

            if (sharedInstance == null)
            {
                sharedInstance = new DrawAChartDelegator();
                GlobalData.vars().setValue("DrawAChartDelegator", sharedInstance);
            }

            sharedInstance._renderToView(code, g, rc);
        }

        void _renderToView(String code, xGraphics g, xBaseControl view)
        {
            mCode = code;
            graphics = g;

            Rectangle rc = new Rectangle(view.getX(), view.getY(), view.getW(), view.getH());
            _renderToView(code, g, rc);
        }

        void _renderToView(String code, xGraphics g, Rectangle rc)
        {
            daysRange = 3 * 26;
            mChartLineLength = daysRange + 1;

            rcView = rc;

            //===================================
            candleCount = Context.getInstance().mShareManager.loadShareFromCommon(code, true, closes, opens, highs, lows, dates, volumes, daysRange);
            if (candleCount < 3)
            {
                return;
            }

            calcHiLo(closes, candleCount);

            endIdx = candleCount - 1;
            beginIdx = endIdx - daysRange;
            if (beginIdx < 0)
            {
                beginIdx = 0;
            }
            //-------------------
            float lineThink = 1.5f;

            mChartLineLength = endIdx - beginIdx + 1;

            //  render volume
            renderVolume(g);

            //  color index
            g.setColor(C.COLOR_GRAY_LIGHT);

            pricesToYs(closes, beginIdx, mChartLineXY, mChartLineLength, priceMin, priceMax);

            g.drawLines(mChartLineXY, mChartLineLength, lineThink);

        }

        //=========================================

        public void init()
        {
            int MAX_CANDLEs = 1000;
            closes = new float[MAX_CANDLEs];
            highs = new float[MAX_CANDLEs];
            lows = new float[MAX_CANDLEs];
            opens = new float[MAX_CANDLEs];
            volumes = new int[MAX_CANDLEs];
            dates = new int[MAX_CANDLEs];
            mChartLineXY = new short[5000];
        }

        void calcHiLo(float[] close, int candleCount)
        {
            priceMin = 10000;
            priceMax = 0;
            /*
            candleCount = Context.getInstance().mShareManager.loadShareFromCommon(mCode, true, closes, opens, highs, lows, dates, volumes, daysRange);
            if (candleCount < 3)
            {
                return;
            }
             */
            endIdx = candleCount - 1;
            beginIdx = endIdx - 3 * 26;
            if (beginIdx < 0)
            {
                beginIdx = 0;
            }

            //  detect min/max
            for (int j = 0; j < candleCount; j++)
            {
                if (closes[j] < priceMin)
                {
                    priceMin = closes[j] - closes[j] * 5 / 100;
                }
                if (closes[j] > priceMax)
                {
                    priceMax = closes[j] + closes[j] * 5 / 100;
                }
            }
        }

        public int priceToY(float price)
        {
            float priceDistance = priceMax - priceMin;

            int dy = (int)((price - priceMin) * rcView.Height / priceDistance);

            return rcView.Height - dy;
        }

        protected void pricesToYs(float[] price, int offset, short[] xy, int len, float price_low, float price_hi)
        {
            float priceDistance = priceMax - priceMin;
            float low = priceMin;

            //	priceDistance will fit drawingH
            //	ex: 100k == 120 pixels . 1k = 1.2 pixels
            float rY = (float)rcView.Height / priceDistance;

            int rightPadding = 1;// (int)xUtils.pointToPixels(1);
            float rX = (float)(rcView.Width - rightPadding) / (endIdx - beginIdx + 1);

            //	int begin = share.mBeginIdx;
            for (int i = 0; i < len; i++)
            {
                xy[2 * i] = (short)(rcView.X + (short)(i * rX));
                xy[2 * i + 1] = (short)(rcView.Y + (short)(rcView.Height - (price[i + offset] - low) * rY));
            }
        }

        void renderVolume(xGraphics g)
        {
            int h = (int)(rcView.Height / 2.5f);

            //	get biggest volume
            long biggest = 0;
            long lowest = -1;
            int vol;
            int i, j;
            for (i = beginIdx; i <= endIdx; i++)
            {
                vol = volumes[i];
                if (lowest == -1)
                    lowest = vol;
                if (biggest < vol)
                {
                    biggest = vol;
                }
                if (lowest > vol)
                {
                    lowest = vol;
                }
            }

            if (biggest == 0)
            {
                return;
            }

            int rightPadding = (int)1;// xUtils.pointToPixels(1);
            float w = rcView.Width - rightPadding;

            double ry = (float)h / (biggest - lowest);
            double rw = w / mChartLineLength;
            float mVolumeBarW = ((w / mChartLineLength) * 2.0f / 3);

            float volumeBarWHalf = mVolumeBarW / 2;

            for (i = 0; i < mChartLineLength; i++)
            {
                j = (i + beginIdx);
                mChartLineXY[i * 2] = (short)(rcView.X + (i * rw - volumeBarWHalf));	//	x
                mChartLineXY[i * 2 + 1] = (short)(rcView.Y + (h - (volumes[j] - lowest) * ry)); //  h
            }

            //  render
            g.setColor(0xff00ff00);
            float tmp = rcView.Height;
            int y0 = rcView.Height - h;

            for (i = 0; i < mChartLineLength; i++)
            {
                if (beginIdx + i > 0)
                {
                    if (closes[beginIdx + i] > closes[beginIdx + i - 1])
                        g.setColor(0xff00c000);
                    else if (closes[beginIdx + i] < closes[beginIdx + i - 1])
                        g.setColor(0xffa00000);
                    else
                        g.setColor(0xffa08000);
                }
                g.fillRectF(mChartLineXY[2 * i], y0 + mChartLineXY[2 * i + 1], mVolumeBarW, h - mChartLineXY[2 * i + 1]);
            }
        }
    }
}
