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


namespace stock123.app.sharethumb
{
    public class ShareThumbView:xBaseControl
    {
        public const int TAG_SHARE_THUMB_VIEW = 1155;
        string mCode;
        public ShareThumbView():base(null)
        {
            makeCustomRender(true);
        }
        public void setShare(String code)
        {
            mCode = code;
        }
        public override void render(xGraphics g)
        {
            DrawAChartDelegator.renderToView(mCode, g, this);
        }
    }
}
