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

        public void drawLine(int x1, int y1, int x2, int y2, float thickness)
        {
            float oldThick = mPen.Width;
            mPen.Width = thickness;

            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            mGraphics.DrawLine(mPen, x1, y1, x2, y2);

            mPen.Width = oldThick;
            mGraphics.SmoothingMode = oldSmoothMode;
        }

        public void drawLineDot(int x1, int y1, int x2, int y2, float thickness)
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

        public void drawLine(int x1, int y1, int x2, int y2)
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
        public void fillRect(int x, int y, int w, int h)
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

        public void drawRect(int x, int y, int w, int h)
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

        public void fillShapes(short[] xy, int pointCnt)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Winding);

            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (pointCnt > xy.Length / 2)
                pointCnt = xy.Length / 2;

            int i = 0;
            int x0, y0, x, y;
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

        public void drawLines(short[] xy, int pointCnt, float lineThick)
        {
            float oldThick = mPen.Width;
            mPen.Width = lineThick;

            drawLines(xy, pointCnt);

            mPen.Width = oldThick;
        }

        public void drawLines(short[] xy, int pointCnt)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Winding);
            
            System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (pointCnt > xy.Length / 2)
                pointCnt = xy.Length / 2;

            int i = 0;
            int x0, y0, x, y;
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

        public void drawLinesDot(short[] xy, int pointCnt)
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
            int x0, y0, x, y;
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

        public void drawString(Font f, String s, int x, int y)
        {
            drawString(f, s, x, y, LEFT | TOP);
        }
        public void drawStringF(Font f, String s, float x, float y, int align)
        {
            drawString(f, s, (int)x, (int)y, align);
        }
        public void drawString(Font f, String s, int x, int y, int align)
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

        public void drawStringInRect(Font f, String s, int x, int y, int w, int h, int align)
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

        public void drawLineDotHorizontal(int x1, int y1, int x2, int y2)
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

        public void drawLineDotHorizontal(int x, int y, int w)
        {
            drawLineDotHorizontal(x, y, x + w, y);
        }

        public void drawVerticalLine(int x, int y, int h)
        {
            drawLine(x, y, x, y + h);
        }

        public void drawHorizontalLine(int x, int y, int w)
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

        public void drawPoint(int x, int y, int radiu)
        {
            x -= radiu/2;
            y -= radiu/2;
            mGraphics.FillEllipse(mPen.Brush, x, y, radiu, radiu);
        }

        public void drawTriangle(int x0, int y0, int x1, int y1, int x2, int y2)
        {
            drawLine(x0, y0, x1, y1);
            drawLine(x1, y1, x2, y2);
            drawLine(x2, y2, x0, y0);
        }

        public void drawTriangleDot(int x0, int y0, int x1, int y1, int x2, int y2)
        {
            drawLineDot(x0, y0, x1, y1, 1);
            drawLineDot(x1, y1, x2, y2, 1);
            drawLineDot(x2, y2, x0, y0, 1);
        }

        public void drawEclipse(int x, int y, int w, int h)
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

        public void drawArc(int ox, int oy, int r, float startAngle, float sweepAngle)
        {
            int x = ox - r;
            int y = oy - r;
            int w = 2 * r;
            int h = 2 * r;
            mGraphics.DrawArc(mPen, x, y, w, h, startAngle, sweepAngle);
        }

        public void drawArcDot(int ox, int oy, int r, float startAngle, float sweepAngle)
        {
            System.Drawing.Drawing2D.DashStyle old = mPen.DashStyle;
            mPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            mPen.DashPattern = new float[] { 5, 5 };

            //System.Drawing.Drawing2D.SmoothingMode oldSmoothMode = mGraphics.SmoothingMode;
            //mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int x = ox - r;
            int y = oy - r;
            int w = 2 * r;
            int h = 2 * r;
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
    }
}
