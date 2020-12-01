using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.framework;

namespace xlib.ui
{
    public class xSplitter: xBaseControl
    {
        xBaseControl mPanel1;
        xBaseControl mPanel2;
        public xSplitter(bool isHorizontal, int w, int h, int distance, int panel1Min, int panel2Min)
            : base(null)
        {
            if (panel1Min > w - 10)
                panel1Min = w - 10;
            if (panel2Min > w - 10)
                panel2Min = w - 10;
            //  if distance is outsize of pannel1Min <-> pannel2Min
            if (distance < panel1Min || distance > w - panel2Min)
            {
                distance = panel1Min + (w - panel2Min)/2;
            }

            SplitContainer spliter = new SplitContainer();
            setControl(spliter);

            spliter.Orientation = (isHorizontal) ? Orientation.Horizontal : Orientation.Vertical;

            setSize(w, h);

            spliter.Panel1MinSize = panel1Min;
            spliter.Panel2MinSize = panel2Min;
            spliter.SplitterDistance = distance;

            //spliter.Orientation = (isHorizontal) ? Orientation.Horizontal : Orientation.Vertical;

            spliter.SplitterMoved +=new SplitterEventHandler(spliter_SplitterMoved);
        }

        static public xSplitter createSplitter(bool isHorizontal, int w, int h, int distance, int panel1Min, int panel2Min)
        {
            if (panel2Min < 10) //  fix crash
                panel2Min = 500;
            xSplitter splitter = new xSplitter(isHorizontal, w, h, distance, panel1Min, panel2Min);
            return splitter;
        }

        public void setPanels(xBaseControl panel1, xBaseControl panel2)
        {
            mPanel1 = panel1;
            mPanel2 = panel2;

            SplitContainer splitter = (SplitContainer)getControl();
            splitter.Panel1.Controls.Add(mPanel1.getControl());
            splitter.Panel2.Controls.Add(mPanel2.getControl());

            adjustPanelsSize();
        }

        void adjustPanelsSize()
        {
            if (mPanel1 == null || mPanel2 == null)
                return;
            SplitContainer splitter = (SplitContainer)getControl();
            int w, h;
            if (splitter.Orientation == Orientation.Horizontal)
            {
                w = getW();
                h = splitter.SplitterDistance;

                mPanel1.setSize(w, h);

                h = getH() - splitter.SplitterDistance - splitter.SplitterWidth;
                mPanel2.setSize(w, h);
            }
            else
            {
                w = splitter.SplitterDistance;
                h = getH();

                mPanel1.setSize(w, h);

                w = getW() - splitter.SplitterDistance - splitter.SplitterWidth;
                mPanel2.setSize(w, h);
            }

            if (mListener != null)
            {
                mListener.onEvent(this, xBaseControl.EVT_ON_SPLITTER_SIZE_CHANGED, mID, null);
            }
        }

        void spliter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            adjustPanelsSize();
        }

        public void setSplitterDistance(int splitterDistance)
        {
            SplitContainer spliter = (SplitContainer)getControl();
            spliter.SplitterDistance = splitterDistance;

            adjustPanelsSize();
        }
    }
}
