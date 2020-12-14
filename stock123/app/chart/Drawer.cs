using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using stock123.app;
using stock123.app.chart;
using stock123.app.data;

using stock123.app.ui;

using xlib.framework;
using xlib.ui;
using xlib.utils;

namespace stock123.app.chart
{
    public class stTrendLine
    {
        public int type;
        public int[] x = { 0, 0, 0 };
        public int[] y = { 0, 0, 0 };
        public uint color = C.COLOR_BLUE;
        public float thickness = 1.0f;

        public float[] candleIdx = { 0, 0, 0 };
        public float[] price = { 0, 0, 0};
        public object data;
    }

    public class Drawer
    {
        uint[] STANDARD_SELECTION_COLORS = { 0xff00B8B8, 0xffC70000, 0xff0000BD, 0xffCC5A00, 0xff00C200, 0xffD6D600, 0xff7A0094,
        0xffB20036, 0xff3472E8, 0xff006400, 0xffE300D1};

        public const int DRAW_TREND = 0;
        public const int DRAW_FIBO_RETRACEMENT = 1;
        public const int DRAW_FIBO_PROJECTION = 2;
        public const int DRAW_FIBO_TIME = 3;
        public const int DRAW_FIBO_FAN = 4;
        public const int DRAW_FIBO_ARC = 5;
        public const int DRAW_RECTANGLE = 6;
        public const int DRAW_TRIANGLE = 7;
        public const int DRAW_ANDREWS_PITCHFORK = 8;
        public const int DRAW_ABC = 9;
        public const int DRAW_OVAL = 10;
        public const int DRAW_TREND_ARROW = 11;
        public const int DRAW_CNT = 12;

        const int TOP_Y_FIBO_TIME = 42;
        //======================================
        xVector mTrends = new xVector(10);
        ChartBase mChart;
        Context mContext;
        int mSelVertex = -1;
        stTrendLine mSelectedTrend = null;
        bool mShow = false;
        int mLastX, mLastY;
        bool mDraging;
        Font mFont;
        int mX, mY;
        int gap = 7;
        Share mShare;
        //======================================
        static float mLastThickness = 1.0f;
        static uint mLastColor = C.COLOR_YELLOW;
        //======================================
        public Drawer()
        {
            mContext = Context.getInstance();
            mFont = mContext.getFontSmall();
            show(true);
        }

        public void initFibonaccie(ChartBase mainChart, Font f, Share share)
        {
            setChart(mainChart);
            mShare = share;
            mFont = f;

            mChart.clearModifyKey();

            loadFibonaccie();
        }
        
        public void setFont(Font f) 
        { 
            mFont = f; 
        }
        //======================================
        public void render(xGraphics g)
        {
            if (!mShow)
                return;

            Share share = getShare(3);
            if (share == null)
                return;

            int b = share.mBeginIdx;
            int e = share.mEndIdx;
            int selSquareSide = 0;
            uint selSquareColor = 0xff00ff00;

            for (int i = 0; i < mTrends.size(); i++)
            {
                stTrendLine t = (stTrendLine)mTrends.elementAt(i);
                g.setColor(C.COLOR_ORANGE);	//	grey
                if (t.type != DRAW_TREND)
                    g.setColor(C.COLOR_FIBO_DOT_LINE2);

                if (t.type == DRAW_TREND)
                    g.setColor(t.color);

                //if (mSelectedTrend == t)
                //{
                    //g.setColor(C.COLOR_RED);
                //}
                if (t != mSelectedTrend
                    && (t.type == DRAW_RECTANGLE
                    || t.type == DRAW_TRIANGLE
                    || t.type == DRAW_ANDREWS_PITCHFORK
                    || t.type == DRAW_ABC
                    || t.type == DRAW_OVAL))
                {
                }
                else
                {
                    g.setColor(t.color);
                    g.drawLineDot(t.x[0], t.y[0], t.x[1], t.y[1], t.thickness);
                }

                if (t.type == DRAW_FIBO_PROJECTION)
                {
                    g.drawLine(t.x[1], t.y[1], t.x[2], t.y[2]);
                }

                int pointRadiu = 2;
                if (mSelectedTrend == t)
                {
                    pointRadiu = 4;
                    selSquareSide = 60;
                }

                //	draw the point
                if (t.x[0] >= mX)
                {
                    g.setColor(0xff00ff00);
                    g.fillRect(t.x[0] - pointRadiu, t.y[0] - pointRadiu, 2 * pointRadiu, 2 * pointRadiu);
                }

                if (t.x[1] < mX + getW())
                {
                    g.setColor(0xffff5522);
                    g.fillRect(t.x[1] - pointRadiu, t.y[1] - pointRadiu, 2 * pointRadiu, 2 * pointRadiu);
                }

                if (t.type == DRAW_FIBO_PROJECTION || t.type == DRAW_TRIANGLE || t.type == DRAW_ANDREWS_PITCHFORK)
                {
                    if (t.x[2] < mX + getW())
                    {
                        g.setColor(0xffffff00);
                        g.fillRect(t.x[2] - pointRadiu, t.y[2] - pointRadiu, 2 * pointRadiu, 2 * pointRadiu);
                    }
                }
                //  draw the dragging point
                if (mSelectedTrend == t)
                {
                    g.setColor(C.COLOR_MAGENTA);
                    int r = 4;
                    int cx = (t.x[0] + t.x[1]) / 2;
                    int cy = (t.y[0] + t.y[1]) / 2;
                    g.drawRect(cx - 2, cy - 2, 4, 4);
                    g.drawRect(cx - 5, cy - 5, 10, 10);
                }

                //	draw fibo lines
                if (t.type == DRAW_TREND)
                {
                    drawTrend(g, t);
                }
                if (t.type == DRAW_TREND_ARROW)
                {
                    drawTrendArrow(g, t);
                }
                else if (t.type == DRAW_FIBO_RETRACEMENT)
                {
                    drawFiboRetracementLines(g, t);
                }
                else if (t.type == DRAW_FIBO_PROJECTION)
                {
                    drawFiboProjection(g, t);
                }
                else if (t.type == DRAW_FIBO_TIME)
                {
                    drawFiboTime(g, t);
                }
                else if (t.type == DRAW_FIBO_FAN)
                {
                    drawFiboFan(g, t);
                }
                else if (t.type == DRAW_FIBO_ARC)
                {
                    drawFiboArc(g, t);
                }
                else if (t.type == DRAW_RECTANGLE)
                {
                    drawRectangle(g, t);
                }
                else if (t.type == DRAW_TRIANGLE)
                {
                    drawTriangle(g, t);
                }
                else if (t.type == DRAW_OVAL)
                {
                    drawOval(g, t);
                }
                else if (t.type == DRAW_ABC)
                {
                    drawAbc(g, t);
                }
                else if (t.type == DRAW_ANDREWS_PITCHFORK)
                {
                    drawAndrewsPitchFork(g, t);
                }

                if (mSelectedTrend != null)
                {
                    drawControls(g);
                }
            }

            //==================draw finger square
            if (mSelectedTrend != null && mSelVertex != -1)
            {
                int x = mSelectedTrend.x[0];
                int y = mSelectedTrend.y[0];
                if (mSelVertex == 1) { x = mSelectedTrend.x[1]; y = mSelectedTrend.y[1]; }
                else if (mSelVertex == 2) { x = mSelectedTrend.x[2]; y = mSelectedTrend.y[2]; }

                selSquareSide = 20;
                g.setColor(C.COLOR_YELLOW);

                //g.drawRect(x - selSquareSide / 2, y - selSquareSide / 2, selSquareSide, selSquareSide);
                //x += selSquareSide / 4;
                //y += selSquareSide / 4;
                //g.drawRect(x, y, selSquareSide / 2, selSquareSide / 2);

                if (mSelectedTrend.type != DRAW_FIBO_TIME && mFont != null)
                {
                    //sprintf(sz, "%d", mSelVertex+1); 
                    //g.setColor(C.COLOR_WHITE);
                    //g.drawString(mFont, "" + (mSelVertex + 1), x + 3, y - (int)mFont.GetHeight(), 0);
                }
            }
        }

        bool isBetween(int x, int pointX, int radiu)
        {
            int d = pointX - x;
            if (d < 0) d = -d;

            return (d <= radiu);
        }

        bool isOnA(stTrendLine t, int x, int y)
        {
            if (t != null)
            {
                return (isBetween(x, t.x[0], gap)
                        && isBetween(y, t.y[0], gap));
            }
            return false;
        }

        bool isOnB(stTrendLine t, int x, int y)
        {
            if (t != null)
            {
                return (isBetween(x, t.x[1], gap)
                        && isBetween(y, t.y[1], gap));
            }
            return false;
        }

        bool isOnC(stTrendLine t, int x, int y)
        {
            if (t != null)
            {
                return (isBetween(x, t.x[2], gap)
                        && isBetween(y, t.y[2], 
                        gap));
            }
            return false;
        }

        bool isOnMiddleAB(stTrendLine t, int x, int y)
        {
            if (t != null)
            {
                int cx = t.x[0] + (t.x[1] - t.x[0]) / 2;
                int cy = t.y[0] + (t.y[1] - t.y[0]) / 2;
                return (isBetween(x, cx, gap)
                        && isBetween(y, cy, gap));
            }
            return false;
        }

        public void deselectVertex()
        {
            mSelVertex = -1;
        }

        public void deselect()
        {
            mSelVertex = -1;
            mSelectedTrend = null;
        }

        stTrendLine getTrendAt(int x, int y, int[] vertex)
        {
            for (int i = 0; i < mTrends.size(); i++)
            {
                stTrendLine t = (stTrendLine)mTrends.elementAt(i);
                if (isOnA(t, x, y) && t.type != DRAW_ABC)
                {
                    vertex[0] = 0;
                    return t;
                }
                else if (isOnB(t, x, y) && t.type != DRAW_ABC)
                {
                    vertex[0] = 1;
                    return t;
                }
                else if (/*t.type == DRAW_TREND && */isOnMiddleAB(t, x, y))
                {
                    if (isKeyPressing(System.Windows.Forms.Keys.Control))
                    {
                        vertex[0] = 9;
                        stTrendLine t1 = addTrends(t.type, 0, 0);
                        t1.x[0] = t.x[0];
                        t1.y[0] = t.y[0];
                        t1.x[1] = t.x[1];
                        t1.y[1] = t.y[1];

                        t1.color = t.color;
                        t1.thickness = t.thickness;

                        return t1;
                    }
                    else
                    {
                        vertex[0] = 9;
                        return t;
                    }
                }
                else if ((t.type == DRAW_FIBO_PROJECTION
                    || t.type == DRAW_TRIANGLE
                    || t.type == DRAW_ANDREWS_PITCHFORK)
                    && isOnC(t, x, y))
                {
                    vertex[0] = 2;
                    return t;
                }
            }

            return null;
        }

        bool isKeyPressing(System.Windows.Forms.Keys key)
        {
            return ((System.Windows.Forms.Control.ModifierKeys & key) != 0);
        }

        public bool onMouseDoubleClick(int x, int y)
        {
            int[] vertex = {0, 0};
            stTrendLine t = getTrendAt(x, y, vertex);
            if (t != null && t.type == DRAW_ABC)
            {
                DlgAddText dlg = new DlgAddText();
                dlg.setText((string)t.data);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string text = dlg.getText();
                    t.data = text;
                    return true;
                }
            }
            if (t != null && t.type == DRAW_TREND)
            {
                if (vertex[0] == 0)
                {
                    t.y[0] = t.y[1];
                }
                else if (vertex[0] == 1)
                {
                    t.y[1] = t.y[0];
                }
            }
            else
            {
                if (isKeyPressing(System.Windows.Forms.Keys.Shift))
                {
                    t = addTrends(DRAW_TREND, 0, 0);
                    t.color = C.COLOR_BLUE;
                    t.thickness = 0.75f;
                    t.x[0] = 30;
                    t.y[0] = y;
                    t.x[1] = getW() - 60;
                    t.y[1] = y;

                    mSelectedTrend = t;
                    mSelVertex = 1;
                }
            }

            return false;
        }

        public bool onMouseDown(int x, int y)
        {
            mDraging = false;
            if (!mShow)
                return false;

            mLastX = -1;
            mLastX = -1;
            stTrendLine t;

            int[] vertex = { 0 };

            t = getTrendAt(x, y, vertex);

            if (t != null)
            {
                mSelectedTrend = t;
                mSelVertex = vertex[0];
            }
            else
            {
                if (isKeyPressing(System.Windows.Forms.Keys.Control))
                {
                    t = new stTrendLine();
                    t.color = (uint)mLastColor;
                    t.thickness = mLastThickness;
                    t.type = DRAW_TREND;
                    mTrends.addElement(t);
                    mSelectedTrend = t;

                    t.x[0] = x;
                    t.y[0] = y;
                    t.x[1] = x;
                    t.y[1] = y;
                    mSelVertex = 1;

                    translateScreenToStockPosition(t);

                    return true;
                }
            }

            mButtonSel = getButtonSel(x, y);
            if (mButtonSel != -1)
                return true;

            return mSelectedTrend != null;
        }

        public bool onMouseMove(int x, int y)
        {
            if (!mShow)
                return false;

            //if (!Utils.isInRect(x, y, mX, mY, getW(), getH()))
                //return false;

            if (mSelectedTrend == null)
                return false;

            //if (!Utils.isInRect(x, y, mX, mY, getW(), getH()))
                //return false;

            if (mLastX == -1)
            {
                mLastX = x;
                mLastY = y;
            }

            int deltaX = x - mLastX;
            int deltaY = y - mLastY;

            //printf("\n=====deltaX: %d", deltaX);
            mLastX = x;
            mLastY = y;

            mDraging = true;

            switch (mSelVertex)
            {
                case -1:
                    return false;
                case 0:
                    mSelectedTrend.x[0] += deltaX;
                    mSelectedTrend.y[0] += deltaY;
                    break;
                case 1:
                    mSelectedTrend.x[1] += deltaX;
                    mSelectedTrend.y[1] += deltaY;
                    break;
                case 2:
                    mSelectedTrend.x[2] += deltaX;
                    mSelectedTrend.y[2] += deltaY;
                    break;
                case 9:
                    mSelectedTrend.x[0] += deltaX;
                    mSelectedTrend.y[0] += deltaY;

                    mSelectedTrend.x[1] += deltaX;
                    mSelectedTrend.y[1] += deltaY;

                    mSelectedTrend.x[2] += deltaX;
                    mSelectedTrend.y[2] += deltaY;
                    break;
            }

            if (mSelectedTrend.type == DRAW_FIBO_TIME)
            {
                mSelectedTrend.y[0] = mSelectedTrend.y[1] = mSelectedTrend.y[2] = TOP_Y_FIBO_TIME;
            }

            translateScreenToStockPosition(mSelectedTrend);

            return true;
        }

        public bool onMouseUp(int x, int y)
        {
            if (!mShow)
                return false;

            //if (!Utils.isInRect(x, y, mX, mY, getW(), getH()))
                //return false;

            if (mSelectedTrend == null)
                return false;

            if (mButtonSel != -1)
            {
                if (mButtonSel == DEL_BUTTON)
                {
                    removeSelectedTrend();
                }
                if (mButtonSel >= 0 && mButtonSel < colors.Length+1)
                {
                    mSelectedTrend.color = colors[mButtonSel];
                    mLastColor = colors[mButtonSel];
                }

                mButtonSel = -1;
                return true;
            }
            

            mLastX = -1;
            mLastY = -1;

            if (mSelectedTrend != null)
            {
                translateScreenToStockPosition(mSelectedTrend);
            }

            saveFibonaccie();

            if (mDraging == false)	//	just tab somewhere
            {
                int[] vertex = { -1 };
                stTrendLine t = getTrendAt(x, y, vertex);
                if (t == null)
                {
                    mSelectedTrend = null;
                    mSelVertex = -1;
                }
            }

            return true;
        }

        void drawFiboRetracementLines(xGraphics g, stTrendLine t)
        {
            uint color = t.color;// Constants.COLOR_FIBO_DOT_LINE2;	//	blue

            g.setColor(color);

            int minX = t.x[0] < t.x[1] ? t.x[0] : t.x[1];
            int maxX = mX + getW() - 40;

            int y0 = t.y[0];
            int maxY = mY + getH();

            float[] percent = { 0, 23.6f, 38.2f, 50.0f, 61.8f, 100f, 161.8f };
            String[] txt = { "0", "23.6", "38.2", "50.0", "61.8", "100", "161.8" };

            int deltaY = t.y[1] - t.y[0];
            if (deltaY < 0) deltaY = -deltaY;
            int up = 1;
            y0 = t.y[0];
            up = t.y[1] < t.y[0] ? -1 : 1;

            if (t.x[0] < t.x[1])
            {
                y0 = t.y[1];
                if (t.y[1] < t.y[0]) up = 1;
                else up = -1;
            }

            //	int devidedNum = 1000;
            String s;
            for (int i = 0; i < 7; i++)
            {
                int y = (int)(y0 + up * (percent[i] * deltaY) / 100);
                if (y > maxY || y < 0)
                    continue;

                //g.drawLine(minX - 30, y, maxX, y);
                g.setColor(color);
                if (i == 5)
                {
                    g.drawLine(minX - 30, y, maxX, y);
                }
                else
                {
                    g.drawLineDotHorizontal(minX - 30, y, maxX, y);
                }

                if (i == 0 || i == 3 || i == 5)
                {
                    s = Utils.formatDecimalNumber((int)mChart.yToPrice(y), 10, 1);
                    g.setColor(C.COLOR_WHITE);
                    g.drawString(mFont, s, maxX, y, xGraphics.VCENTER | xGraphics.TOP);
                    g.drawString(mFont, txt[i], minX - 30, y, xGraphics.VCENTER | xGraphics.TOP);
                }
            }
        }

        public void drawFiboProjection(xGraphics g, stTrendLine t)
        {
            uint color = t.color;// Constants.COLOR_FIBO_DOT_LINE2;	//	blue

            g.setColor(color);

            float[] percent = { 0f, 23.6f, 38.2f, 50.0f, 61.8f, 100f, 161.8f };
            String[] txt = { "0", "23.6", "38.2", "50.0", "61.8", "100", "161.8" };

            int minX = t.x[0] < t.x[1] ? t.x[0] : t.x[1];
            minX = minX < t.x[2] ? minX : t.x[2];
            int maxX = t.x[0] > t.x[1] ? t.x[0] : t.x[1] + 30;
            maxX = mX + getW() - 35;
            int maxY = mY + getH() - 20;

            int deltaY = t.y[1] - t.y[0];
            if (deltaY < 0) deltaY = -deltaY;

            int up = 1;

            int y0 = t.y[2];

            if (t.x[0] < t.x[1] && t.y[0] < t.y[1]) up = 1;
            if (t.x[0] < t.x[1] && t.y[0] > t.y[1]) up = -1;
            if (t.x[0] > t.x[1] && t.y[0] > t.y[1]) up = 1;
            if (t.x[0] > t.x[1] && t.y[0] < t.y[1]) up = -1;

            String s;
            for (int i = 0; i < 7; i++)
            {
                int y = (int)(y0 + up * (percent[i] * deltaY) / 100);
                if (y > maxY || y < 0)
                    continue;

                g.setColor(color);
                if (i == 5)
                    g.drawLine(minX - 30, y, maxX + 30, y);
                else
                    g.drawLineDotHorizontal(minX - 30, y, maxX + 30, y);

                //=====================
                if (i == 0 || i == 3 || i == 5)
                {
                    s = Utils.formatDecimalNumber((int)mChart.yToPrice(y), 10, 1);

                    g.setColor(C.COLOR_WHITE);
                    g.drawString(mFont, s, maxX, y, xGraphics.VCENTER | xGraphics.TOP);
                    g.drawString(mFont, txt[i], minX - 30, y, xGraphics.VCENTER | xGraphics.TOP);
                }
            }
        }

        void drawFiboTime(xGraphics g, stTrendLine t)
        {
            uint color = t.color;	//	blue

            g.setColor(color);

            float[] percent = { 0, 23.6f, 38.2f, 50.0f, 61.8f, 100f, 161.8f };
            String[] txt = { "0", "23.6", "38.2", "50", "61.8", "100", "161.8" };

            int toRight = 1;
            if (t.x[0] > t.x[1])
            {
                toRight = -1;
            }
            int x0 = mX + t.x[0];
            int deltaX = Utils.ABS_INT((t.x[1] - t.x[0]));
            int maxX = mX + getW();

            //	int devidedNum = 1000;
            //	char sz[25];

            for (int i = 0; i < 7; i++)
            {
                int x = (int)(x0 + toRight * (percent[i] * deltaX) / 100);
                if (x > maxX || x < 0)
                    continue;

                g.setColor(color);
                if (i == 5)
                {
                    g.drawLine(x, t.y[0], x, mY + getH() - TOP_Y_FIBO_TIME);
                }
                else
                {
                    g.drawLineDotHorizontal(x, t.y[0], x, mY + getH() - TOP_Y_FIBO_TIME);
                }

                if (i == 0 || i == 3 || i == 5 || i == 6)
                {
                    g.setColor(C.COLOR_WHITE);
                    g.drawString(mFont, txt[i], x, t.y[0], 0);//xGraphics::HCENTER|xGraphics::BOTTOM);
                }
            }
        }

        void drawFiboArc(xGraphics g, stTrendLine t)
        {
            uint color = t.color;// Constants.COLOR_FIBO_DOT_LINE2;	//	blue

            g.setColor(color);

            float[] percent = { 23.6f, 38.2f, 50.0f, 61.8f, 100f, 161.8f};
            String[] txt = { "23.6", "38.2", "50", "61.8", "100", "161.8" };
            int y = 0;

            double R = (t.x[0] - t.x[1]) * (t.x[0] - t.x[1]) + (t.y[0] - t.y[1]) * (t.y[0] - t.y[1]);
            R = Math.Sqrt(R);

            int r;
            g.setColor(color);

            for (int i = 0; i < percent.Length; i++)
            {
                r = (int)(R * percent[i])/100;

                if (i == 4)
                {
                    g.drawArc(t.x[0], t.y[0], r, 0f, 360);
                }
                else
                {
                    g.drawArcDot(t.x[0], t.y[0], r, 0f, 360);    
                }
                
                g.drawString(mFont, txt[i], t.x[0] - r, t.y[0]);
            }
        }

        void drawFiboFan(xGraphics g, stTrendLine t)
        {
            uint color = t.color;	//	blue

            g.setColor(color);

            float[] percent = { 23.6f, 38.2f, 50.0f, 61.8f};//, 100f};
            String[] txt = { "23.6", "38.2", "50", "61.8", "100", "161.8" };
            int y = 0;
            int deltaY = t.y[0] - t.y[1];
            int dx = t.x[1] - t.x[0];
            int dy = t.y[1] - t.y[0];

            for (int i = 0; i < percent.Length; i++)
            {
                y = t.y[0] - (int)(deltaY * percent[i]/100);
                int x2 = t.x[1] + 2*dx;
                int dx2 = x2 - t.x[0];
                dy = y - t.y[0];    //  => y = dy + t.y[0]

                //  dx/dx2 = dy/dy2
                int dy2 = 0;
                if (dx != 0)
                {
                    dy2 = dx2 * dy / dx;
                    y = t.y[0] + dy2;
                }

                g.drawLineDotHorizontal(t.x[0], t.y[0], x2, y);
                g.drawString(mFont, txt[i], x2 - 30, y);
            }
        }

        void drawRectangle(xGraphics g, stTrendLine t)
        {
            uint color = t.color;

            g.setColor(color);

            int w = t.x[1] - t.x[0];
            int h = t.y[1] - t.y[0];
            
            g.drawRect(t.x[0], t.y[0], w, h);
        }
            
        void drawTriangle(xGraphics g, stTrendLine t)
        {
            uint color = t.color;

            g.setColor(color);

            g.drawTriangleDot(t.x[0], t.y[0], t.x[1], t.y[1], t.x[2], t.y[2]);
        }

        void drawOval(xGraphics g, stTrendLine t)
        {
            uint color = t.color;

            g.setColor(color);
            int w = t.x[1] - t.x[0];
            int h = t.y[1] - t.y[0];
            g.drawEclipse(t.x[0], t.y[0], w, h);
        }

        void drawAbc(xGraphics g, stTrendLine t)
        {
            string text = (string)t.data;
            if (text != null)
            {
                g.setColor(t.color);
                g.drawString(mFont, text, t.x[0], t.y[0]);
            }
        }

        void drawAndrewsPitchFork(xGraphics g, stTrendLine t)
        {
            //
            //     -----1
            // 0---
            //     -----2
            g.setColor(t.color);
            g.drawTriangleDot(t.x[0], t.y[0], t.x[1], t.y[1], t.x[2], t.y[2]);

            float cX = (t.x[1] + t.x[2]) / 2;
            float cY = (t.y[1] + t.y[2]) / 2;

            if (cX == t.x[0])
            {
                cX = t.x[0] + 1;
            }

            //  PT for center line: y = ax + b
            float a = (float)(cY - t.y[0]) / (cX - t.x[0]);
            float b = t.y[0] - a * t.x[0];
            //  find the PT 0 - top line // center line
            float b1 = t.y[1] - a * t.x[1];
            //  find the bottom line
            float b2 = t.y[2] - a * t.x[2];

            int l = (int)(getW() - cX);    //  do dai du kien
            int xx = (int)(cX + l);

            int[] yy = { 0, 0, 0 };

            //  center point: y0 = a*x0 + b
            yy[0] = (int)(a * xx + b);
            yy[1] = (int)(a * xx + b1);
            yy[2] = (int)(a * xx + b2);

            //  now draw 3 // lines
            g.drawLineDot(t.x[1], t.y[1], xx, yy[1], 1);
            g.drawLineDot(t.x[0], t.y[0], xx, yy[0], 1);
            g.drawLineDot(t.x[2], t.y[2], xx, yy[2], 1);
        }

        public void show(bool show)
        {
            mShow = show;
        }

        public void setChart(ChartBase chart)
        {
            mChart = chart;
        }

        public stTrendLine addTrends(int type, int color, float thickness)
        {
            stTrendLine t;
            //--------------------
            t = new stTrendLine();
            if (color == 0)
            {
                uint c = mLastColor;
                color = (int)c;
            }
            if (thickness == 0.0f)
                thickness = 1.0f;
            t.color = (uint)color;
            t.thickness = thickness;
            t.type = type;

            int w = getW() / 5;
            int h = getH() / 4;

            int[] xx = { w, 2 * w, 3 * w };
            int[] yy = { 2 * h, 1 * h, h + h / 2 };

            if (type == DRAW_FIBO_TIME)
            {
                yy[0] = yy[1] = TOP_Y_FIBO_TIME;
            }

            for (int i = 0; i < 3; i++)
            {
                t.x[i] = xx[i];
                t.y[i] = yy[i];
            }

            mTrends.addElement(t);
            mSelectedTrend = t;
            mSelVertex = 0;

            translateScreenToStockPosition(t);

            return t;
        }

        public stTrendLine addText(string text)
        {
            stTrendLine t;
            //--------------------
            t = new stTrendLine();

            t.color = (uint)mLastColor;
            t.thickness = 1.0f;
            t.type = DRAW_ABC;
            t.data = text;

            t.x[0] = getW() / 2;
            t.y[0] = getH() / 2;
            t.x[1] = t.x[0];
            t.y[1] = t.y[0];

            mTrends.addElement(t);
            mSelectedTrend = t;
            mSelVertex = 0;

            translateScreenToStockPosition(t);

            return t;
        }

        public void removeSelectedTrend()
        {
            if (mSelectedTrend != null)
            {
                mTrends.removeElement(mSelectedTrend);

                mSelectedTrend = null;
            }

            if (mTrends.size() > 0)
            {
                mSelectedTrend = (stTrendLine)mTrends.lastElement();
            }

            saveFibonaccie();
        }

        void drawTrend(xGraphics g, stTrendLine t)
        {
        }

        void drawTrendArrow(xGraphics g, stTrendLine t)
        {
            double h = (t.x[1] - t.x[0]) * (t.x[1] - t.x[0]) + (t.y[1] - t.y[0]) * (t.y[1] - t.y[0]);
            h = Math.Sqrt(h);

            double sina = (float)(t.y[1] - t.y[0]) / h;
            double cosa = (float)(t.x[1] - t.x[0]) / h;

            int x01 = - 14;
            int y01 = - 7;
            int x02 = - 14;
            int y02 = + 7;

            int x1 = (int)(x01 * cosa - y01 * sina) + t.x[1];
            int y1 = (int)(x01 * sina + y01 * cosa) + t.y[1];

            int x2 = (int)(x02 * cosa - y02 * sina) + t.x[1];
            int y2 = (int)(x02 * sina + y02 * cosa) + t.y[1];

            short[] xy = { (short)x1, (short)y1, (short)x2, (short)y2, (short)t.x[1], (short)t.y[1] };

            g.setColor(t.color);
            g.fillShapes(xy, 3);

            //g.drawTriangle(x1, y1, t.x[1], t.y[1], x2, y2);
        }

        public bool isShow()
        {
            return mShow;
        }

        void translateScreenToStockPosition(stTrendLine t)
        {
            for (int i = 0; i < 2; i++)
            {
                t.candleIdx[i] = mChart.xToCandleIdx(t.x[i]);
                t.price[i] = mChart.yToPrice(t.y[i]);
            }
            if (t.type == DRAW_FIBO_PROJECTION
                || t.type == DRAW_TRIANGLE
                || t.type == DRAW_ANDREWS_PITCHFORK)
            {
                t.candleIdx[2] = mChart.xToCandleIdx(t.x[2]);
                t.price[2] = mChart.yToPrice(t.y[2]);
            }
        }

        public void recalcPosition()
        {
            for (int i = 0; i < mTrends.size(); i++)
            {
                stTrendLine t = (stTrendLine)mTrends.elementAt(i);
                _recalcPosition(t);
            }
        }

        void _recalcPosition(stTrendLine t)
        {
            for (int i = 0; i < 2; i++)
            {
                t.x[i] = mChart.candleToX(t.candleIdx[i]);
                t.y[i] = mChart.priceToY(t.price[i]);
            }
            if (t.type == DRAW_FIBO_PROJECTION
                || t.type == DRAW_TRIANGLE
                || t.type == DRAW_ANDREWS_PITCHFORK)
            {
                t.x[2] = mChart.candleToX(t.candleIdx[2]);
                t.y[2] = mChart.priceToY(t.price[2]);
            }
            if (t.type == DRAW_FIBO_TIME)
            {
                t.y[0] = t.y[1] = t.y[2] = TOP_Y_FIBO_TIME;
            }
        }

        int getW()
        {
            return mChart.getW();
        }

        int getH()
        {
            return mChart.getH();
        }

        void drawColorSelectionBoxes(xGraphics g)
        {

        }

        string getFilename()
        {
            if (mShare == null)
                return "";
            Utils.sb.Length = 0;
            Utils.sb.AppendFormat("{0}{1}", Context.FILE_DRAWING, mShare.mCode);
            string filename = Utils.sb.ToString();
            return filename;
        }

        bool mShouldSaveFile = true;
        public void enableSaveFile(bool enable)
        {
            mShouldSaveFile = enable;
        }

        void saveFibonaccie()
        {
            if (!mShouldSaveFile)
                return;
            Share share = mShare;
            if (share == null)
                return;

            string filename = getFilename();

            if (mTrends.size() == 0)
            {
                xFileManager.removeFile(filename);
                return;
            }
            xDataOutput o = new xDataOutput(1000);
            o.writeInt(Context.FILE_VERSION);
            o.writeInt(mTrends.size());
            for (int i = 0; i < mTrends.size(); i++)
            {
                stTrendLine t = (stTrendLine)mTrends.elementAt(i);
                o.writeByte(t.type);
                o.writeInt(t.color);
                o.writeFloat(t.thickness);
                for (int j = 0; j < 3; j++)
                {
                    o.writeFloat(t.candleIdx[j]);
                    //Utils.trace("=====savefile: candle=" + t.candleIdx[j]);
                    o.writeFloat(t.price[j]);
                }

                if (t.type == DRAW_ABC)
                {
                    o.writeUTF((string)t.data);
                }
            }

            //Utils.trace("=====savefile");
            xFileManager.saveFile(o, filename);
        }

        void loadFibonaccie()
        {
            if (!mShouldSaveFile)
                return;
            Share share = mShare;
            if (share == null)
                return;

            string sz = getFilename();

            xDataInput di = xFileManager.readFile(sz, false);
            mTrends.removeAllElements();
            mSelectedTrend = null;
            if (di != null)
            {
                if (di.readInt() == Context.FILE_VERSION)
                {
                    int cnt = di.readInt();
                    for (int i = 0; i < cnt; i++)
                    {
                        stTrendLine t = new stTrendLine();
                        t.type = di.readByte();
                        t.color = (uint)di.readInt();
                        t.thickness = di.readFloat();
                        for (int j = 0; j < 3; j++)
                        {
                            t.candleIdx[j] = di.readFloat();
                            Utils.trace("=====loadfile: candle=" + t.candleIdx[j]);
                            t.price[j] = di.readFloat();
                        }

                        if (t.type == DRAW_ABC)
                        {
                            t.data = di.readUTF();
                        }

                        mTrends.addElement(t);
                    }

                    recalcPosition();
                }
            }
            Utils.trace("=====loadfile");
        }

        void removeSaved(string code)
        {
            if (code == null)
                return;
            Utils.sb.Length = 0;
            Utils.sb.AppendFormat("{0}{1}", Context.FILE_DRAWING, code);
            string filename = Utils.sb.ToString();

            xFileManager.removeFile(filename);
        }

        public bool onKeyUp(int key)
        {
            if (key == 27)
            {
                mSelectedTrend = null;
                mSelVertex = -1;
                return true;
            }
            if (key == xMainApplication.KEY_DELETE)
            {
                removeSelectedTrend();
                return true;
            }
            if (mSelectedTrend != null)
            {
                if (key == 'c' || key == 'C')
                {
                    uint[] colors = {C.COLOR_RED, C.COLOR_ORANGE, C.COLOR_MAGENTA, C.COLOR_YELLOW,
                        C.COLOR_GREEN, C.COLOR_BLUE, C.COLOR_CYAN, C.COLOR_GRAY_LIGHT, C.COLOR_GRAY, C.COLOR_WHITE};

                    int current = 0;
                    int i = 0;
                    for (i = 0; i < colors.Length; i++)
                    {
                        if (colors[i] == mSelectedTrend.color)
                        {
                            current = i;
                            break;
                        }
                    }
                    //if (key == (int)System.Windows.Forms.Keys.Right)
                    {
                        current++;
                    }
                    //else if (key == (int)System.Windows.Forms.Keys.Left)
                    //{
                        //current--;
                    //}
                    if (current < 0) current = colors.Length - 1;
                    if (current >= colors.Length) current = 0;
                    mSelectedTrend.color = colors[current];
                    mLastColor = mSelectedTrend.color;
                }
                if ((key == (int)System.Windows.Forms.Keys.Oemplus) || (key == (int)System.Windows.Forms.Keys.OemMinus))
                {
                    if (key == (int)System.Windows.Forms.Keys.Oemplus)
                        mSelectedTrend.thickness += 0.7f;
                    else
                        mSelectedTrend.thickness -= 0.7f;
                    if (mSelectedTrend.thickness < 0.7f) mSelectedTrend.thickness = 0.7f;
                    if (mSelectedTrend.thickness > 5.0f) mSelectedTrend.thickness = 5.0f;

                    mLastThickness = mSelectedTrend.thickness;
                }

                return true;
            }
            return false;
        }

        int mButtonSel = -1;
        const int DEL_BUTTON = 10;
        uint[] colors = { 0xffff0000, 0xffff8000, 0xff00ff00, 0xffff00ff, 0xff00ffff, 0xff0080ff, 0xff0000ff, 0xffffffff };
        int bw = 16;
        int bh = 16;
        int getButtonSel(int x, int y)
        {
            int r = getW() - 40;
            int l = r - colors.Length*bw - bw;
            int b = 24;

            if (y < b || y > b + bh)
                return -1;
            if (x < l || x > r)
                return -1;

            //  delete
            int dx = x - l;
            if (dx > 0)
            {
                int idx = dx / bw;
                if (idx >= colors.Length)
                    idx = DEL_BUTTON;
                return idx;
            }

            return -1;
        }

        void drawControls(xGraphics g)
        {
            int x = 0;
            int y = 24;
            int fw = bw;
            int fh = bh;
            int sx = 0;

            x = getW() - 40 - fw;

            //  delete
            g.setColor(C.COLOR_GRAY_LIGHT);
            Image img = mContext.getImage(C.IMG_DRAWER_BUTTONS);
            if (mButtonSel == 0)
                sx = fw;
    
            g.drawImage(img, x, y, fw, fh, sx, 0);

            //  colors: RED/ORANGE/GREEN/PINK/WHITE
            x -= colors.Length*bw;
            sx += 2 * bw;
            for (int i = 0; i < colors.Length; i++)
            {
                g.setColor(colors[i]);
                g.fillRect(x, y, bw, bh);
                g.drawImage(img, x, y, fw, fh, sx, 0);

                x += bw;
            }
        }

        public void clearAll()
        {
            this.mSelectedTrend = null;
            this.mSelVertex = -1;
            this.mTrends.removeAllElements();
            this.mShouldSaveFile = false;

            if (mShare != null)
                removeSaved(mShare.mCode);
        }

        Share getShare(int days)
        {
            return getShare();
        }

        Share getShare()
        {
            return mShare;
        }
    }

}
