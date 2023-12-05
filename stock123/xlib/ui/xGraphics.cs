using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace xlib.ui
{
    public class xGraphics
    {
		public const int LEFT		= 1 << 0;
		public const int HCENTER		= 1 << 1;
		public const int RIGHT		= 1 << 2;
		public const int TOP			= 1 << 3;
		public const int VCENTER		= 1 << 4;
        public const int BOTTOM = 1 << 5;
        //============================================

        Graphics mGraphics;
        Pen mPen;

        public xGraphics()
        {
            mPen = new Pen(Color.Black);
        }

        public Graphics getGraphics()
        {
            return mGraphics;
        }

        public void setGraphics(Graphics g)
        {
            mGraphics = g;
        }

        public void setColor(int color)
        {
            mPen.Color = Color.FromArgb(color);
        }

        public void setColor(uint color)
        {
            mPen.Color = Color.FromArgb((int)color);
        }

        public void drawLine(float x1, float y1, float x2, float y2, float thickness)
        {
            float oldThick = mPen.Width;
            mPen.Width = thickness;

            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            mGraphics.DrawLine(mPen, x1, y1, x2, y2);

            mPen.Width = oldThick;
            mGraphics.SmoothingMode = oldSmoothMode;
        }

        public void drawLineDot(float x1, float y1, float x2, float y2, float thickness)
        {
            float oldThick = mPen.Width;
            mPen.Width = thickness;

            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            System.Drawing.Drawing2D.DashStyle old = mPen.DashStyle;
            mPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            mPen.DashPattern = new float[] { 5, 5 };

            mGraphics.DrawLine(mPen, x1, y1, x2, y2);

            mPen.Width = oldThick;
            mGraphics.SmoothingMode = oldSmoothMode;

            //  restore old state
            mPen.DashStyle = old;
        }

        public void drawLine(float x1, float y1, float x2, float y2)
        {
            if (y1 < -10000)
                return;
            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            mGraphics.DrawLine(mPen, x1, y1, x2, y2);
            mGraphics.SmoothingMode = oldSmoothMode;
        }

        public void fillRectF(float x, float y, float w, float h)
        {
            if (h < 0)
            {
                mGraphics.FillRectangle(mPen.Brush, x, y + h, w, -h);
            }
            else
            {
                mGraphics.FillRectangle(mPen.Brush, x, y, w, h);
            }
        }
        public void fillRect(float x, float y, float w, float h)
        {
            if (h < 0)
            {
                mGraphics.FillRectangle(mPen.Brush, x, y + h, w, -h);
            }
            else
            {
                mGraphics.FillRectangle(mPen.Brush, x, y, w, h);
            }
        }

        public void drawRectF(float x, float y, float w, float h)
        {
            drawRect((int)x, (int)y, (int)w, (int)h);
        }

        public void drawRect(float x, float y, float w, float h)
        {
            if (h < 0)
            {
                y += h;
                h = -h;
            }
            if (w < 0)
            {
                x += w;
                w = -w;
            }
            mGraphics.DrawRectangle(mPen, x, y, w, h);
        }

        public void fillShapes(float[] xy, int pointCnt)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Winding);

            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (pointCnt > xy.Length / 2)
                pointCnt = xy.Length / 2;

            int i = 0;
            float x0, y0, x, y;
            path.StartFigure();
            x0 = xy[0];
            y0 = xy[1];
            for (i = 1; i < pointCnt; i++)
            {
                x = xy[2 * i];
                y = xy[2 * i + 1];
                path.AddLine(x0, y0, x, y);
                x0 = x;
                y0 = y;
            }
            path.CloseFigure();
            mGraphics.FillPath(mPen.Brush, path);

            mGraphics.SmoothingMode = oldSmoothMode;
        }

        public void drawLines(float[] xy, int offset, int pointCnt, float thickness)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Winding);

            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (pointCnt > xy.Length / 2)
            {
                pointCnt = xy.Length / 2;
            }

            int i = 0;
            float x0, y0, x, y;
            //path.StartFigure();
            int t = offset;
            x0 = xy[t];
            y0 = xy[t+1];
            for (i = 1; i < pointCnt; i++)
            {
                t = offset + (i << 1);

                x = xy[t];
                y = xy[t + 1];
                path.AddLine(x0, y0, x, y);
                x0 = x;
                y0 = y;
            }
            //path.CloseFigure();
            mPen.Width = thickness;
            mGraphics.DrawPath(mPen, path);

            mGraphics.SmoothingMode = oldSmoothMode;
        }

        public void drawLines(float[] xy, int pointCnt, float lineThick)
        {
            float oldThick = mPen.Width;
            mPen.Width = lineThick;

            drawLines(xy, pointCnt);

            mPen.Width = oldThick;
        }

        public void drawLines(float[] xy, int pointCnt)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Winding);
            
            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (pointCnt > xy.Length / 2)
                pointCnt = xy.Length / 2;

            int i = 0;
            float x0, y0, x, y;
            //path.StartFigure();
            x0 = xy[0];
            y0 = xy[1];
            for (i = 1; i < pointCnt; i++)
            {
                x = xy[2 * i];
                y = xy[2 * i + 1];
                path.AddLine(x0, y0, x, y);
                x0 = x;
                y0 = y;
            }
            //path.CloseFigure();
            mGraphics.DrawPath(mPen, path);

            mGraphics.SmoothingMode = oldSmoothMode;
        }

        public void drawLinesDot(float[] xy, int pointCnt)
        {
            System.Drawing.Drawing2D.DashStyle old = mPen.DashStyle;
            mPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            mPen.DashPattern = new float[] { 5, 5 };

            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Winding);

            if (pointCnt > xy.Length / 2)
                pointCnt = xy.Length / 2;

            int i = 0;
            float x0, y0, x, y;
            //path.StartFigure();
            x0 = xy[0];
            y0 = xy[1];
            for (i = 1; i < pointCnt; i++)
            {
                x = xy[2 * i];
                y = xy[2 * i + 1];
                path.AddLine(x0, y0, x, y);
                x0 = x;
                y0 = y;
            }
            //path.CloseFigure();
            mGraphics.DrawPath(mPen, path);

            //  restore old state
            mPen.DashStyle = old;
            mGraphics.SmoothingMode = oldSmoothMode;
        }

        public void drawString(Font f, String s, float x, float y)
        {
            drawString(f, s, x, y, LEFT | TOP);
        }
        public void drawStringF(Font f, String s, float x, float y, int align)
        {
            drawString(f, s, x, y, align);
        }
        public void drawString(Font f, String s, float x, float y, int align)
        {
            if (((align & LEFT) != 0) && ((align & TOP) != 0)
                || align == 0)
            {
                mGraphics.DrawString(s, f, mPen.Brush, x, y);
                return;
            }

            SizeF size = mGraphics.MeasureString(s, f);

            if ((align & RIGHT) != 0)
            {
                x -= (int)size.Width;
            }
            if ((align & BOTTOM) != 0)
            {
                y -= (int)size.Height;
            }
            if ((align & HCENTER) != 0)
            {
                x -= (int)size.Width / 2;
            }
            if ((align & VCENTER) != 0)
            {
                y -= (int)size.Height / 2;
            }
            mGraphics.DrawString(s, f, mPen.Brush, x, y);
        }

        public void drawStringInRect(Font f, String s, float x, float y, float w, float h, int align)
        {
            RectangleF rc = new RectangleF(x, y, w, h);
            SizeF size = mGraphics.MeasureString(s, f);

            StringFormat stringFormat = new StringFormat();
            if ((align & HCENTER) != 0)
            {
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                mGraphics.DrawString(s, f, mPen.Brush, rc, stringFormat);
            }
            else
            {
                mGraphics.DrawString(s, f, mPen.Brush, rc);
            }
        }

        public void drawLineDotHorizontal(float x1, float y1, float x2, float y2)
        {
            System.Drawing.Drawing2D.DashStyle old = mPen.DashStyle;
            mPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            mPen.DashPattern = new float[] { 5, 5};

            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            mGraphics.DrawLine(mPen, x1, y1, x2, y2);

            //  restore old state
            mPen.DashStyle = old;
            mGraphics.SmoothingMode = oldSmoothMode;
        }

        public void drawLineDotHorizontal(float x, float y, float w)
        {
            drawLineDotHorizontal(x, y, x + w, y);
        }

        public void drawVerticalLine(float x, float y, float h)
        {
            drawLine(x, y, x, y + h);
        }

        public void drawHorizontalLine(float x, float y, float w)
        {
            drawLine(x, y, x + w, y);
        }

        public void clear()
        {
            mGraphics.Clear(mPen.Color);
        }

        public int getStringWidth(Font f, String s)
        {
            return (int)mGraphics.MeasureString(s, f).Width;
        }

        public void drawPoint(float x, float y, float radiu)
        {
            x -= radiu/2;
            y -= radiu/2;
            mGraphics.FillEllipse(mPen.Brush, x, y, radiu, radiu);
        }

        public void drawTriangle(float x0, float y0, float x1, float y1, float x2, float y2)
        {
            drawLine(x0, y0, x1, y1);
            drawLine(x1, y1, x2, y2);
            drawLine(x2, y2, x0, y0);
        }

        public void drawTriangleDot(float x0, float y0, float x1, float y1, float x2, float y2)
        {
            drawLineDot(x0, y0, x1, y1, 1);
            drawLineDot(x1, y1, x2, y2, 1);
            drawLineDot(x2, y2, x0, y0, 1);
        }

        public void drawEclipse(float x, float y, float w, float h)
        {
            if (h < 0)
            {
                y += h;
                h = -h;
            }
            if (w < 0)
            {
                x += w;
                w = -w;
            }
            mGraphics.DrawEllipse(mPen, x, y, w, h);
        }

        public void fillCircle(float ox, float oy, float r)
        {
            mGraphics.FillEllipse(mPen.Brush, ox - r, oy - r, 2 * r, 2 * r);
        }

        public void drawArc(float ox, float oy, float r, float startAngle, float sweepAngle)
        {
            float x = ox - r;
            float y = oy - r;
            float w = 2 * r;
            float h = 2 * r;
            mGraphics.DrawArc(mPen, x, y, w, h, startAngle, sweepAngle);
        }

        public void drawArcDot(float ox, float oy, float r, float startAngle, float sweepAngle)
        {
            System.Drawing.Drawing2D.DashStyle old = mPen.DashStyle;
            mPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            mPen.DashPattern = new float[] { 5, 5 };

            //System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            //mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float x = ox - r;
            float y = oy - r;
            float w = 2 * r;
            float h = 2 * r;
            mGraphics.DrawArc(mPen, x, y, w, h, startAngle, sweepAngle);

            //  restore old state
            mPen.DashStyle = old;
            //mGraphics.SmoothingMode = oldSmoothMode;
        }

        public void drawImage(Image img, int x, int y)
        {
            mGraphics.DrawImage(img, x, y);
        }

        public void drawImage(Image img, int dx, int dy, int w, int h, int sx, int sy)
        {
            //  overload 14
            mGraphics.DrawImage(img, dx, dy, new RectangleF(sx, sy, w, h), GraphicsUnit.Pixel);
        }

        public void drawHistogram(float[] xy, int offset, int pointCnt,
                              float yBase, float hisW,
                              uint colorUp1, uint colorUp2,
                              uint colorDown1, uint colorDown2)
        {
            uint color;
            uint preColor = colorUp1;
            for (int i = offset; i < offset + pointCnt; i++)
            {
                int x = 2 * i;
                int y = 2 * i + 1;
                int y_1 = i > 0 ? (2 * (i - 1) + 1) : y;

                if (xy[y] < yBase)
                {
                    color = xy[y] < xy[y_1] ? colorUp1 : colorUp2;
                }
                else if (xy[y] > yBase)
                {
                    color = xy[y] > xy[y_1] ? colorDown1 : colorDown2;
                }
                else
                {
                    color = preColor;
                }
                preColor = color;
                setColor(color);

                setColor(color);

                fillRect(xy[x] - hisW / 2, xy[y], hisW, yBase - xy[y]);
            }
        }

        public void drawLinesUpDown(float[] xy, int offset, int pointCnt,
                                    float[] values,
                                    int valueOffset,
                                    float thick,
                                    uint colorUp, uint colorDown)
        {
            if (pointCnt == 0 || xy == null)
            {
                return;
            }

            int start = offset;
            bool trendUp = true;
            trendUp = values[valueOffset + 1] > values[valueOffset];
            int end = offset + pointCnt;
            for (int i = offset + 1; i < end; i++)
            {
                int vi = valueOffset + i - offset;
                if (trendUp && values[vi] < values[vi - 1])
                {
                    setColor(colorUp);
                    drawLines(xy, 2 * start, i - start, thick);

                    start = i - 1;
                    trendUp = false;
                }
                else if (!trendUp && values[vi] > values[vi - 1])
                {
                    setColor(colorDown);
                    drawLines(xy, 2 * start, i - start, thick);
                    start = i - 1;
                    trendUp = true;
                }
            }

            if (start < end - 1)
            {
                setColor(trendUp ? colorUp : colorDown);
                drawLines(xy, 2 * start, end - start, thick);
            }

        }
    }
}
