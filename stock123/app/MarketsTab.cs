using System;
using System.Collections.Generic;
using System.Text;
using xlib.ui;
using xlib.framework;
using stock123.app.data;

namespace stock123.app
{
    public class MarketsTab: xContainer
    {
        xTabControl mTabControl;
        Context mContext;
        xVector mControlsShouldInvalideAfterNetDone = new xVector();

        public MarketsTab(xIEventListener listener, int w, int h)
            : base(listener)
        {
            mTabControl = new xTabControl();
            this.setSize(w, h);

            mContext = Context.getInstance();

            mTabControl.setSize(w, h);
            addControl(mTabControl);

            //  common
            xTabPage page = new xTabPage("Hose && Hnx");
            xBaseControl c = createCommonTab();
            page.addControl(c);

            mTabControl.addPage(page);

            for (int i = 0; i < mContext.mPriceboard.getIndicesCount(); i++)
            {
                stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(i);
                if (pi == null || pi.code == null)
                    break;

                //  Hose
                page = new xTabPage(pi.code);
                c = createMarketFullControl(pi);
                page.addControl(c);

                mTabControl.addPage(page);
            }
        }

        public void refresh()
        {
        }

        xBaseControl createMarketFullControl(stPriceboardStateIndex pi)
        {
            xContainer c = new xContainer();
            c.setSize(getW() - 6, getH() - 6);

            return c;
        }

        xBaseControl createCommonTab()
        {
            xContainer c = new xContainer();
            c.setSize(getW() - 6, getH() - 6);
            int y = 0;
            int h = c.getH() / 2 - 10;
            mControlsShouldInvalideAfterNetDone.removeAllElements();

            for (int i = 0; i < mContext.mPriceboard.getIndicesCount(); i++)
            {
                stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(i);
                if (pi == null || pi.code == null)
                    break;
                IndexControl ic = new IndexControl(mListener, pi.marketID, c.getW(), h);
                ic.setPosition(0, y);
                ic.setSize(c.getW(), h);
                y += ic.getBottom() + 10;

                mControlsShouldInvalideAfterNetDone.addElement(ic);

                c.addControl(ic);
            }

            return c;
        }
    }
}
