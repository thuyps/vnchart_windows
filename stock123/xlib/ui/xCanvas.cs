using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using xlib.framework;


namespace xlib.ui
{
    class xCanvas: Control
    {
        xBaseControl mBaseControl;
        xGraphics mGraphics;

        bool mDoubleBuffer;
        Bitmap offScreenBmp;
        Graphics offScreenDC;
        xGraphics offG;

        public xCanvas(bool doubleBuffer, bool isTransparent)
            : base()
        {
            mDoubleBuffer = doubleBuffer;
            if (isTransparent)
            {
                SetStyle(ControlStyles.UserPaint, false);

                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.BackColor = Color.Transparent;
            }

            this.KeyDown += new KeyEventHandler(xCanvas_KeyDown);
            this.KeyUp +=new KeyEventHandler(xCanvas_KeyUp);
        }

        public xCanvas(bool doubleBuffer)
            : base()
        {
            mDoubleBuffer = doubleBuffer;
            this.KeyUp += new KeyEventHandler(xCanvas_KeyUp);
            this.KeyDown += new KeyEventHandler(xCanvas_KeyDown);
        }

        void xCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            mBaseControl.onKeyDown(e.KeyValue);
        }

        //protected override void OnKeyPress(KeyPressEventArgs e)
        //{
            //mBaseControl.onKeyPress(e.KeyChar);
        //}

        void xCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            mBaseControl.onKeyPress(e.KeyValue);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //  ThuyPS: avoiding flickering technique
            if (mBaseControl != null && mDoubleBuffer)
            {
                if (offScreenBmp == null)
                {
                    offScreenBmp = new Bitmap(mBaseControl.getW(), mBaseControl.getH());
                    offScreenDC = Graphics.FromImage(offScreenBmp);

                    offG = new xGraphics();
                    offG.setGraphics(offScreenDC);
                }
                //  background
                offG.getGraphics().Clear(this.BackColor);

                //  render
                mBaseControl.render(offG);
            }
            else
            {
                base.OnPaintBackground(pevent);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (mBaseControl != null)
            {
                if (mGraphics == null)
                    mGraphics = new xGraphics();
                mGraphics.setGraphics(e.Graphics);

                if (mDoubleBuffer)
                {
                    mGraphics.getGraphics().DrawImage(offScreenBmp, 0, 0);
                }
                else
                {
                    mBaseControl.render(mGraphics);
                }
            }
        }

        public void setxBaseControl(xBaseControl c)
        {
            mBaseControl = c;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (mBaseControl != null && e.Button == MouseButtons.Left)
            {
                mBaseControl.onMouseDown(e.X, e.Y);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mBaseControl != null && e.Button == MouseButtons.Left)
            {
                mBaseControl.onMouseMove(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (mBaseControl != null && e.Button == MouseButtons.Left)
            {
                mBaseControl.onMouseUp(e.X, e.Y);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (mBaseControl != null)
            {
                mBaseControl.onMouseDoubleClick(e.X, e.Y);
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            if (mBaseControl != null)
            {
                mBaseControl.onMouseDoubleClick();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (mBaseControl != null)
            {
                if (offScreenDC != null)
                {
                    offScreenBmp.Dispose();
                    offScreenDC.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public void recalcDoubleBuffer()
        {
            offScreenBmp = null;
            offScreenDC = null;
            offG = null;

            Invalidate();
        }
    }
}
