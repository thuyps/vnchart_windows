using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using xlib.framework;
using xlib.utils;
using xlib.ui;
using stock123.app.data;

namespace stock123.app
{
    public class MiniFormMoving: xPicture
    {
        Context mContext;
        int mLastX;
        int mLastY;
        bool mIsMouseDown;
        public MiniFormMoving(xIEventListener listener, ImageList imglist)
            : base(imglist, 5)
        {
            mContext = Context.getInstance();
    
            mImgIndex = mContext.isOnline()?4:5;
            mLastX = -1;
            mIsMouseDown = false;

            mListener = listener;
        }

        public override void onMouseDown(int x, int y)
        {
            mLastX = x;
            mLastY = y;
            mIsMouseDown = true;
        }
        public override void onMouseMove(int x, int y)
        {
            if (!mIsMouseDown)
                return;
            short dX = (short)(x - mLastX);
            short dY = (short)(y - mLastY);

            int param = (dY << 16) | dX;
            mListener.onEvent(this, xBaseControl.EVT_ON_MOUSE_MOVE, param, null);
        }

        public override void onMouseUp(int x, int y)
        {
            mLastX = -1;
            mIsMouseDown = false;
        }

        public void updateStatus()
        {
            if (mContext.isOnline())
            {
                this.mImgIndex = 4;
            }
            else
            {
                this.mImgIndex = 5;
            }
            invalidate();
        }
    }
}
