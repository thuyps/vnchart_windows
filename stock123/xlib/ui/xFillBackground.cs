using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using xlib.framework;
using xlib.ui;

namespace xlib.ui
{
    public class xFillBackground: xBaseControl
    {
        uint mColor;
        public xFillBackground(uint color)
            : base(null)
        {
            makeCustomRender(true);

            mColor = color;
        }

        public static xFillBackground createFill(int w, int h, uint color)
        {
            xFillBackground fill = new xFillBackground(color);
            fill.setSize(w, h);
            return fill;
        }

        public override void render(xGraphics g)
        {
            g.setColor(mColor);
            g.fillRect(0, 0, getW(), getH());
        }
    }
}
