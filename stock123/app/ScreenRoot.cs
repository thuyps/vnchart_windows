﻿using System;
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
            },
            (int tabIndex) =>
            {
                if (tabIndex > 0)
                {
                    mScreens.removeElementAt(tabIndex);
                }
            }
            );

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

        xTabPage getTabPageOfShare(Share share)
        {
            for (int i = 0; i < mTab.getPageCount(); i++)
            {
                xTabPage page = mTab.getPageAtIndex(i);
                if (page.userData != null && page.userData == share)
                {
                    ViewHistoryChart his = (ViewHistoryChart)page.userData2;
                    //if (his.mScreenType == ViewHistoryChart.TYPE_CHART)
                    {
                        return page;
                    }
                }
            }

            return null;
        }

        public void createNewHistory(Share oriShare)
        {
            if (mScreens.size() > 30)
            {
                return;
            }

            xTabPage exist = getTabPageOfShare(oriShare);
            ViewHistoryChart his;
            if (exist != null)
            {
                mTab.selectPage(exist);
                return;
            }
            //--------------------
            Share share = oriShare;
            if (share != null && share.getShareID() > 0)
            {
                share = new Share(Share.MAX_CANDLE_CHART_COUNT);
                share.setCode(oriShare.getCode(), 0);
                share.setID(oriShare.getShareID());
                share.setDataType(oriShare.getDataType());
                share.mIsGroupIndex = oriShare.mIsGroupIndex;
            }

            his = new ViewHistoryChart(share);
            his.setSize(mTab.getW(), mTab.getH());
            //his.onActivate();
            mScreens.addElement(his);

            String title = "Lọc mã";
            if (share != null)
            {
                if (share.getCode() != null & share.getCode().Length > 0)
                {
                    title = share.getCode();
                }
            }
            xTabPage page = new xTabPage(title);
            page.userData = oriShare;
            page.userData2 = his;

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

        public void showDetailRealtimeChart(TradeHistory trade)
        {
            Share share = trade.saveToShare();

            createNewHistory(share);
        }
    }
}
