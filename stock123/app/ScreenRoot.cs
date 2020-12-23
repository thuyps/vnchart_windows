using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.framework;
using xlib.ui;

using System.Threading;

using stock123.app.data;

namespace stock123.app
{
    class ScreenRoot: ScreenBase
    {
        xVector mScreens;
        ScreenHome screenHome;
        ViewHistoryChart screenHistory0;
        xTabControl mTab;
        xTabPage mHomePage;

        static ScreenRoot _instance;

        public ScreenRoot()
            : base()
        {
            _instance = this;
        }

        static public ScreenRoot instance()
        {
            return _instance;
        }

        public override void onActivate()
        {
            base.onActivate();

            mTab = new xTabControl();
            mTab.setSize(this.Size.Width, this.Size.Height);

            addControl(mTab);

            mScreens = new xVector();
            screenHome = new ScreenHome();
            mScreens.addElement(screenHome);

            mHomePage = new xTabPage("Bảng giá");
            mHomePage.setSize(mTab.getW(), mTab.getH());
            mHomePage.addControl(screenHome);
            /*
            xFillBackground v = new xFillBackground(0xffff0000);
            v.setSize(mTab.getW(), mTab.getH());
            mHomePage.addControl(v);
            */
            mTab.addPage(mHomePage);

            mTab.showClosePageButton(true, (int tabIndex) =>
            {
                if (tabIndex == 0)
                {
                    return false;
                }
                return true;
            }, null);

            screenHome.setSize(mHomePage.getW(), mHomePage.getH());
            screenHome.onActivate();
        }

        public void onApplicationSizeChanged()
        {
            mTab.setSize(this.Size.Width, this.Size.Height);
            for (int i = 0; i < mScreens.size(); i++)
            {
                ScreenBase sc = (ScreenBase)mScreens.elementAt(i);
                sc.setSize(mHomePage.getW(), mHomePage.getH());
            }
        }

        public override void onTick()
        {
            base.onTick();

            for (int i = 0; i < mScreens.size(); i++)
            {
                ScreenBase sc = (ScreenBase)mScreens.elementAt(i);
                sc.onTick();
            }
        }

        public void createNewHistory(Share oriShare)
        {
            if (mScreens.size() > 20)
            {
                return;
            }

            Share share = oriShare;
            if (share.getShareID() > 0)
            {
                share = new Share(Share.MAX_CANDLE_CHART_COUNT);
                share.setCode(oriShare.getCode(), 0);
                share.setID(oriShare.getShareID());
            }

            ViewHistoryChart his = new ViewHistoryChart(share);
            his.setSize(mTab.getW(), mTab.getH());
            //his.onActivate();
            mScreens.addElement(his);

            xTabPage page = new xTabPage(share.getCode());
            his.Tag = page;

            page.addControl(his);
            mTab.addPage(page);

            mTab.selectLastPage();
            
            utils.AsyncUtils.DelayCall(500, () =>
            {
                his.onActivate();
            });
        }

        public void removeHistory(ViewHistoryChart his)
        {
            mTab.removePage((xTabPage)his.Tag);
            mScreens.removeElement(his);
        }

        public void addViewAsTab(String title, xBaseControl content)
        {
            xTabPage page = new xTabPage(title);

            content.setSize(mTab.getW(), mTab.getH());

            page.addControl(content);
            mTab.addPage(page);

            mTab.selectLastPage();
        }
    }
}
