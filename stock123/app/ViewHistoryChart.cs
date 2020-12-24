using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.ui;
using xlib.framework;
using xlib.utils;
using stock123.app.data;
using stock123.app.chart;
using stock123.app.ui;
using stock123.app.net;

namespace stock123.app
{
    public class ViewHistoryChart: ScreenBase
    {
        public const int TYPE_SEARCH = 0;
        public const int TYPE_CHART = 1;
        public int mScreenType = TYPE_CHART;

        const int SUB_CHART_H_DEFAULT = 150;
        const int SUB_CHART_H_MIN = 100;

        const int NETSTATE_NORMAL = 0;
        const int NETSTATE_GET_QUOTE_DATA_PREPARING = 1;
        const int NETSTATE_GET_QUOTE_DATA = 2;

        int mNetState;

        bool[] mUseVNIndex = { true };
        bool[] mUseHASTC = { true };

        bool[] mIsFilterKLGD = { true };
        bool[] mIsFilterLowPrice = { true };
        bool[] mIsFilterHiPrice = { true };
        float[] mFilterKLGD = { 10000 };
        float[] mFilterLowPrice = { 1000 };
        float[] mFilterHiPrice = { 30000 };

        xContainer mLeftPanel;  //  quote list & search control
        xContainer mRightPanel;
        xVector mPanels = new xVector(2);

        xTextField mQuickCode;

        xVector mFilteredShares = new xVector(1000);
        xContainer mTableList;

        int[] mAcceptedMarkets = { 1, 0, 0, 0};

        bool mSortTopToBottom;
        int mCurrentSort;

        DlgContactingServer mNetworkContacting;
        HistoryChartControl mMainHistoryChartControl;

        ToolBarButton mAlarmButton;
        int mAlarmAnimationIDX = 0;
        xTimer mAlarmAnimation = new xTimer(500);

        NetProtocol mNetProtocol;

        public ViewHistoryChart(Share share)
            : base()
        {
            mScreenType = share != null?TYPE_CHART:TYPE_SEARCH;
            mShare = share;
//            List<Share> list;
//            IComparer<Share>
        }

        public override int getToolbarH()
        {
            return 39;
        }

        public override void onActivate()
        {
            mCurrentSort = -1;
            mSortTopToBottom = true;

            Share share = mShare;
            mNetState = NETSTATE_NORMAL;

            mNetProtocol = mContext.createNetProtocol();
            mNetProtocol.setListener(this);

            if (mScreenType == TYPE_SEARCH)
            {
                mShare = mContext.mShareManager.getVnindexShareAt(0);
                try
                {
                    mContext.mShareManager.loadAllShares();
                    doFilterQuick();
                }
                catch (Exception e)
                {
                    Utils.trace(e.StackTrace);
                    Utils.trace(e.Message);
                }
            }
            else
            {
                if (share.getID() > 0)
                {
                    share.mIsComparingChart = false;
                    if (share.isRealtime())
                    {
                        mNetState = NETSTATE_NORMAL;
                    }
                    else
                    {
                        reloadShare(share, false);
                        if (true)//mContext.isQuoteFavorite(share))
                        {
                            mNetState = NETSTATE_GET_QUOTE_DATA_PREPARING;
                        }

                        if (!mContext.isOnline())
                        {
                            mNetState = NETSTATE_NORMAL;
                        }
                    }
                }
            }

            updateUI();
        }

        public override void onDeactivate()
        {
            mScreenType = TYPE_SEARCH;

            mShare.mIsComparingChart = false;

            removeAllControls();

            mLeftPanel = null;
            mRightPanel = null;

            base.onDeactivate();
        }

        override public void onTick()
        {
            //===========do animation on alarm============
            if (mAlarmAnimation.isExpired())
            {
                mAlarmAnimation.reset();
                if (mContext.mAlarmManager.hasAlarm())
                {
                    mAlarmAnimationIDX = 1 - mAlarmAnimationIDX;
                    mAlarmButton.ImageIndex = 10 + mAlarmAnimationIDX;
                }
            }
            //============================================

            int date = 0;
            
            if (mNetState == NETSTATE_GET_QUOTE_DATA_PREPARING)
            {
                mNetState = NETSTATE_GET_QUOTE_DATA;
                mNetProtocol.resetRequest();

                date = mShare.getTrueLastCandleDate();
                if (date == 0)
                {
                    date = C.DATE_BEGIN;
                }
                else
                {
                    long l = Utils.dateToNumber(date);
                    date = Utils.dateFromNumber(l - 5);
                }

                mNetProtocol.requestGet1ShareData(mShare.mID, date);

                mNetProtocol.flushRequest();

                if (mShare.getCandleCount() == 0)
                {
                    mNetworkContacting = new DlgContactingServer();
                    mNetworkContacting.Show();
                }
            }
        }

        void updateUI()
        {
            //--------------------------------------
            removeAllControls();
            //mTableList = null;
            mRightPanel = null;
            //--------------------------------------
            createToolbar();
            createLeftPanel();
            createRightPanel(); //  main chart & sub charts
        }

        void createToolbar()
        {
            addToolbar(mContext.getImageList(C.IMG_MAIN_ICONS, 30, 26));
            ToolBar tb = getToolbar();
            tb.ButtonSize = new System.Drawing.Size(60, 30);
            tb.Size = new System.Drawing.Size(300, 30);
            tb.TextAlign = ToolBarTextAlign.Right;

            //addToolbarButton(C.ID_GOTO_HOME_SCREEN, 0, "Bảng giá");
            addToolbarButton(C.ID_SEARCH_ON, 3, "Lọc");
            //addToolbarSeparator();
            //addToolbarSeparator();
            //addToolbarSeparator();
            addToolbarSeparator();
            addToolbarButton(C.ID_SETUP_INDICATOR_PARAMETER, 2, "Cài đặt");

            addToolbarSeparator();
            addToolbarButton(C.ID_SHOW_FILTER_PARAMETER_FORM, 7, "Đ/k lọc");

            addToolbarSeparator();
            /*
            for (int i = 0; i < 10; i++)
            {
                addToolbarSeparator();
            }
             */

            bool viewPushed = mContext.mIsViewSplitted;
            addToolbarButton(C.ID_TOOGLE_DRAWING_TOOL, 9, "Vẽ", ToolBarButtonStyle.ToggleButton, false);
            //addToolbarSeparator();
            addToolbarSeparator();
            addToolbarButton(C.ID_SPLIT_VIEW, 8, "-View-", ToolBarButtonStyle.ToggleButton, viewPushed);
            //addToolbarButton(C.ID_GOTO_MINI_SCREEN, 4, "Minimize");
            //addToolbarSeparator();
            addToolbarSeparator();
            mAlarmButton = addToolbarButton(C.ID_ALARM_MANAGER, 10, "Cảnh báo");
            //addToolbarSeparator();
            addToolbarSeparator();
            addToolbarButton(C.ID_GOTO_HELP, 1, "Help");
            //addToolbarButton(C.ID_ADD_SUB_CHART, 2, "+Sub chart");
            //addToolbarButton(C.ID_ADD_MASTER_CHART, 2, "+Master chart");
            //=================================================
            /*
            xBaseControl dropdown = null;
            dropdown = createShareGroupDroplist(C.ID_DROPDOWN_COMMON_GROUP, "Nhóm mặc định", mContext.mShareGroups);
            dropdown.setPosition(getW() - dropdown.getW()-100, 0);
            tb.Controls.Add(dropdown.getControl());

            int x = dropdown.getX() - 10 - dropdown.getW();
            dropdown = createShareGroupDroplist(C.ID_DROPDOWN_FAVOR_GROUP, "Nhóm yêu thích", mContext.mFavorGroups);
            dropdown.setPosition(x, 0);
            tb.Controls.Add(dropdown.getControl());
            */
            //  quick look
            xLabel l = xLabel.createSingleLabel("Mã cổ phiếu");
            xTextField tf = xTextField.createTextField(100);
            mQuickCode = tf;
            int x = getW() - tf.getW() - 20;

            l.setSize(tf.getW(), l.getH());
            l.setPosition(x, 2);
            tb.Controls.Add(l.getControl());

            tf.setPosition(x, l.getBottom());//, dropdown.getY() + l.getH() + 4);
            tb.Controls.Add(tf.getControl());
            tf.setButtonEvent(C.ID_BUTTON_QUOTE, this);

            //  scroll
            stShareGroup g = mContext.getCurrentShareGroup();
            if (g != null && g.getTotal() > 1)
            {
                int itemW = 40;
                int itemH = 13;
                xContainer symbolContainer = new xContainer();
                int cw = itemW*(g.getTotal()/2 + 1);
                int ch = 2 * itemH + 1;
                symbolContainer.setSize(cw, ch);

                //symbolContainer.setBackgroundColor(0x80808000);
                for (int i = 0; i < g.getTotal(); i+=2)
                {
                    //  row 1
                    l = xLabel.createSingleLabel(g.getCodeAt(i));
                    l.setSize(itemW, itemH);
                    l.setPosition((i/2) * itemW, 0);
                    //l.setListener(this);
                    l.setTextColor(0xffff0000);
                    l.enableClick(C.ID_SYMBOL_CLICK_START + i, this);

                    symbolContainer.addControl(l);

                    //  row 2
                    if (i + 1 >= g.getTotal())
                    {
                        break;
                    }
                    l = xLabel.createSingleLabel(g.getCodeAt(i+1));
                    l.setSize(itemW, itemH);
                    l.setPosition((i / 2) * itemW, itemH);
                    //l.setListener(this);
                    l.setTextColor(0xffff0000);
                    l.enableClick(C.ID_SYMBOL_CLICK_START + i+1, this);

                    symbolContainer.addControl(l);
                }
                xScrollView scroll = new xScrollView(null, 16 * itemW, ch+ 17);
                scroll.addControl(symbolContainer);
                scroll.setPosition(getW() - 166 - scroll.getW(), 0);
                tb.Controls.Add(scroll.getControl());
            }
            
            //==============================================================
            addStatusBar();
            setStatusMsg("{Chọn vùng: shift+bấm&rê chuột}, {Vẽ đường: ctrl+bấm&rê chuột}, {Clone trend: ctrl+center point}");
            setStatusMsg1("{Zoom chart: nhấp đúp chuột}, {Thickness: -/+}");
        }

        void createLeftPanel()
        {
            int leftW = 240;
            if (mLeftPanel == null)
            {
                mLeftPanel = new xContainer();
                mLeftPanel.setSize(leftW, getWorkingH());
                mLeftPanel.setPosition(getW()-leftW, getToolbarH());

                xBaseControl search = recreateSearchControl();
                search.setPosition(0, mLeftPanel.getH() - search.getH());
                mLeftPanel.addControl(search);

                //  share list
                mTableList = new xContainer();
                mTableList.setSize(mLeftPanel.getW(), mLeftPanel.getH() - search.getH());
                mLeftPanel.addControl(mTableList);
                //===================
            }
            if (mScreenType == TYPE_CHART)
                leftW = 0;  //  hide it
            mLeftPanel.setPosition(getW() - leftW, getToolbarH());
            mLeftPanel.setSize(leftW, getWorkingH());
            addControl(mLeftPanel);

            if (mScreenType == TYPE_SEARCH)
            {
                recreateTableList();
            }
        }

        void recreateTableList()
        {
            mTableList.removeAllControls();
            xListView list = createFilteredList(mTableList.getW(), mTableList.getH());
            list.setPosition(0, 0);

            mTableList.addControl(list);

            if (mFilteredShares.size() > 0)
            {
                mContext.setCurrentShare((Share)mFilteredShares.elementAt(0));
            }
            else
            {
                mContext.selectDefaultShare();
            }
            reloadShare(mShare, true);
        }

        public void createRightPanel()
        {
            Share share = mShare;
            if (share == null)
                return;

            //============================
            int y0 = getToolbarH();

            if (mRightPanel == null)
            {
                mRightPanel = new xContainer();
                addControl(mRightPanel);
            }
            else
            {
                mRightPanel.removeAllControls();
            }
            mRightPanel.setSize(getW() - mLeftPanel.getW(), mLeftPanel.getH());
            //mRightPanel.setPosition(mLeftPanel.getRight(), y0);
            mRightPanel.setPosition(0, y0);
            

            int w = mRightPanel.getW();
            mPanels.removeAllElements();

            bool showDrawingTool = false;
            if (mMainHistoryChartControl != null)
                showDrawingTool = mMainHistoryChartControl.hasDrawing();
            //==========================
            if (mContext.mIsViewSplitted)
            {
                float tmp = (float)w/5;
                int[] ws = { (int)tmp * 3, (int)tmp * 2 };

                xBaseControl[] panels = { null, null };
                xSplitter splitter = xSplitter.createSplitter(false, mRightPanel.getW(), mRightPanel.getH(), ws[0], 120, 120);
                for (int i = 0; i < 2; i++)
                {
                    HistoryChartControl his = new HistoryChartControl(mShare, "panel" + (i+1), ws[i], mRightPanel.getH(), i==1);
                    his.setListener(this);
                    his.setPosition(0, 0);
                    panels[i] = his;
                }

                mMainHistoryChartControl = (HistoryChartControl)panels[0];

                splitter.setPanels(panels[0], panels[1]);
                mPanels.addElement(panels[0]);
                mPanels.addElement(panels[1]);

                mRightPanel.addControl(splitter);
            }
            else
            {
                HistoryChartControl his = new HistoryChartControl(mShare, "pannel0", w, mRightPanel.getH(), false);
                his.setListener(this);
                his.setPosition(0, 0);

                mMainHistoryChartControl = his;

                mPanels.addElement(his);
                mRightPanel.addControl(his);
            }

            if ((showDrawingTool && !mMainHistoryChartControl.hasDrawing())
                || (!showDrawingTool && mMainHistoryChartControl.hasDrawing()))
                mMainHistoryChartControl.toogleDrawingTool();
        }

        xListView createFilteredList(int w, int h)
        {
            xVector shares = mFilteredShares;
            int cnt = shares.size();

            float[] columnPercents = { 40, 30, 30 }; //  code, price, value
            String[] columnTexts = { "Mã CP", "Giá", "---" };

            xListView list = xListView.createListView(this, columnTexts, columnPercents, w-20, h, mContext.getImageList(C.IMG_MARKET_ICONS, 20, 21));
            list.setID(C.ID_PRICEBOARD_TABLE);
            list.setBackgroundColor(C.COLOR_GRAY);
            //  always add vnindex & hastc
            int i = 0;
            for (i = 0; i < mContext.mShareManager.getVnindexCnt(); i++)
            {
                Share share = mContext.mShareManager.getVnindexShareAt(i);
                if (share != null && share.getCode() != null && share.getCode().Length > 0)
                {
                    RowQuoteList r = RowQuoteList.createRowQuoteList(share, this);
                    list.addRow(r);
                    r.update();
                }
            }
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                if (!mContext.mPriceboard.isShareIndex(share.mID))
                {
                    RowQuoteList r = RowQuoteList.createRowQuoteList(share, this);
                    list.addRow(r);
                    r.update();
                }
            }
            return list;
        }

        public override void onToolbarEvent(int buttonID)
        {
            if (buttonID == C.ID_ALARM_MANAGER)
            {
                showAlarmManager();
            }
            /*
            if (buttonID == C.ID_GOTO_MINI_SCREEN)
            {
                ScreenBase scr = (ScreenBase)MainApplication.getInstance().getScreen(MainApplication.SCREEN_MINIMIZED);
                scr.mPreviousScreen = MainApplication.SCREEN_SEARCH;
                goNextScreen(MainApplication.SCREEN_MINIMIZED);
            }
             */
            if (buttonID == C.ID_TOOGLE_DRAWING_TOOL)
            {
                mMainHistoryChartControl.toogleDrawingTool();
            }
            if (buttonID == C.ID_SEARCH_ON)
            {
                mScreenType = 1 - mScreenType;

                if (mContext.mShareManager.getShareCount() < 100)
                {
                    mContext.mShareManager.loadAllShares();
                    doFilter();
                }

                updateUI();
            }
            if (buttonID == C.ID_SETUP_INDICATOR_PARAMETER)
            {
                FormSettingParameters dlg = new FormSettingParameters(mShare, this);
                dlg.ShowDialog();
            }
            if (buttonID == C.ID_SPLIT_VIEW)
            {
                mContext.mIsViewSplitted = !mContext.mIsViewSplitted;
                mContext.saveOptions();
                //updateUI();
                createRightPanel();
            }
            if (buttonID == C.ID_SHOW_FILTER_PARAMETER_FORM)
            {
                DlgFilterConfig dlg = new DlgFilterConfig();
                dlg.ShowDialog();

                doFilter();
                recreateTableList();
            }
            if (buttonID == C.ID_GOTO_HOME_SCREEN)
            {
                goNextScreen(MainApplication.SCREEN_HOME);
            }
            /*
            if (buttonID == C.ID_ADD_SUB_CHART)
            {
                //SubchartsContainer chart = new SubchartsContainer(Constants.SUBCHART_VOLUME, this);
                xBaseControl c = createSubchart(SubchartsContainer.SUBCHART_VOLUME);
                mSubCharts.addElement(c);
                mSubContainer.addControl(c);

                adjustChartsSize();
            }
             */
            if (buttonID == C.ID_ADD_MASTER_CHART)
            {
            }
            if (buttonID == C.ID_GOTO_HELP)
            {
                showHelp();
            }
        }

        xContainer createFilterBasicControls(int w, int h)
        {
            int i = 0;
            xLabel l;
            int x, y;
            x = 10;
            y = 10;
            xButton bt;
            xContainer c = new xContainer();

            x = 0;
            //  separator should be here
            y = 0;// bt.getBottom() - 4;

            //========buttons


            String[] bts = {
                                "Alphabet",
                                "Cổ phiếu tích luỹ",
                                "Vol tăng đột biến",
                                "Tăng giá (theo %)",
                                "Giảm giá (theo %)",
                                "KL lớn nhất trong ngày",
                                "CP rẻ/đắt nhất",
                                "KLCP niêm yết",
                                "Vốn hóa thị trường",
                                "Heiken"
                           };
            int[] ids = {   
                            C.ID_SORT_BASE + C.SORT_ABC,
                            C.ID_SORT_BASE + C.SORT_TICHLUY,
                            C.ID_SORT_BASE + C.SORT_VOL_DOTBIEN,
                            C.ID_SORT_BASE + C.SORT_MOST_INCREASE,
                            C.ID_SORT_BASE + C.SORT_MOST_DECREASE, 
                            C.ID_SORT_BASE + C.SORT_TODAY_BIGGEST_VOLUME,
                            C.ID_SORT_BASE + C.SORT_LOWEST_PRICE,
                            C.ID_SORT_BASE + C.SORT_ON_MARKET_VOLUME,
                            C.ID_SORT_BASE + C.SORT_VONHOATT,                           
                            C.ID_SORT_BASE + C.SORT_HEIKEN
                            };

            int bth = 25;
            y += 6;
            for (i = 0; i < bts.Length; i++)
            {
                bt = xButton.createStandardButton(ids[i], this, bts[i], w);
                bt.setSize(w, bth);
                bt.setPosition(x, y);
                c.addControl(bt);

                y =
                    bt.getBottom();// +2;
            }

            c.setSize(w, y);

            return c;
        }

        xContainer createFilterBasicControls2(int w, int h)
        {
            int i = 0;
            xLabel l;
            int x, y;
            x = 10;
            y = 10;
            xButton bt;
            xContainer c = new xContainer();

            x = 0;
            //  separator should be here
            y = 0;// bt.getBottom() - 4;

            //========buttons


            String[] bts = {
                                "EPS cao nhất",
                                "P/E thấp nhất",
                                "Beta",
                                "ROA",
                                "ROE"
                                };
            int[] ids = {   
                            C.ID_SORT_BASE + C.SORT_EPS,
                            C.ID_SORT_BASE + C.SORT_PE,
                            C.ID_SORT_BASE + C.SORT_BETA,
                            C.ID_SORT_BASE + C.SORT_ROA,
                            C.ID_SORT_BASE + C.SORT_ROE
            };

            int bth = 25;
            y += 6;
            for (i = 0; i < bts.Length; i++)
            {
                bt = xButton.createStandardButton(ids[i], this, bts[i], w);
                bt.setSize(w, bth);
                bt.setPosition(x, y);
                c.addControl(bt);

                y =
                    bt.getBottom();// +2;
            }

            c.setSize(w, y);

            return c;
        }

        //  Technical filter 1
        xContainer createFilterTAControls(int w, int h)
        {
            int i = 0;
            xLabel l;
            int x, y;
            x = 10;
            y = 10;
            xButton bt;
            xContainer c = new xContainer();
            x = 0;
            y = 0;
            int bth = 25;
            y += 6;
            //========buttons
            int cnt = 0;
            //if (mContext.mSortTechnicalName[0] != null)
                //cnt = mContext.mSortTechnicalName[0].size();

            ImageList imgs = mContext.getImageList(C.IMG_SUB_BUTTONS, 16, 15);    

            int bw = w - 18;

            cnt = 9;
            FilterManager fm = FilterManager.getInstance();
            fm.loadFilterSets();

            int createdFilters = fm.getFilterSetCnt();
            cnt = createdFilters < 9?createdFilters:9;

            for (i = 0; i < cnt; i++)
            {
                FilterSet item = fm.getFilterSetAt(i);
                string name = item.name;// (string)mContext.mSortTechnicalName[0].elementAt(i);
                //int sort_params = mContext.mSortTechnicalParams[0].elementAt(i);

                bt = xButton.createStandardButton(C.ID_SORT_TECHNICAL, this, name, w);
                bt.setSize(bw, bth);
                bt.setPosition(x, y);
                //bt.setDataInt(i);   //  (0 << 16) | i
                bt.setData(item);
                c.addControl(bt);
                //  edit button
                xButton edit = xButton.createImageButton(C.ID_SORT_TECHNICAL_EDIT, this, imgs, 0);
                //edit.setDataInt(i);
                edit.setData(item);
                edit.setPosition(w - edit.getW(), y + (bt.getH() - edit.getH()) / 2);
                c.addControl(edit);

                y = bt.getBottom() +2;
            }

            if (i < 9)
            {
                bt = xButton.createStandardButton(C.ID_SORT_TECHNICAL, this, "TẠO BỘ LỌC", w);
                bt.setSize(bw, bth);
                bt.setTextColor(0xffff0000);
                bt.setPosition(x, y);
                //bt.setDataInt(i);   //  (0 << 16) | i
                bt.setData(null);

                c.addControl(bt);

                y = bt.getBottom() + 2;
            }

            c.setSize(w, y);

            return c;
        }

        xContainer createFilterTA2Controls(int w, int h)
        {
            int i = 0;
            xLabel l;
            int x, y;
            x = 10;
            y = 10;
            xButton bt;
            xContainer c = new xContainer();
            x = 0;
            y = 0;
            int bth = 25;
            y += 6;
            //========buttons
            int cnt = 0;
            //if (mContext.mSortTechnicalName[1] != null)
                //cnt = mContext.mSortTechnicalName[1].size();

            FilterManager fm = FilterManager.getInstance();
            int remains = fm.getFilterSetCnt() - 9;
            cnt = remains > 0 ? remains : 0;

            ImageList imgs = mContext.getImageList(C.IMG_SUB_BUTTONS, 16, 15);

            int bw = w - 18;

            for (i = 0; i < cnt; i++)
            {
                FilterSet item = fm.getFilterSetAt(i + 9);
                string name = item.name;// (string)mContext.mSortTechnicalName[1].elementAt(i);
                int sort_params = mContext.mSortTechnicalParams[1].elementAt(i);

                bt = xButton.createStandardButton(C.ID_SORT_TECHNICAL2, this, name, w);
                bt.setSize(bw, bth);
                bt.setPosition(x, y);
                //bt.setDataInt((1<<16)|i);
                bt.setData(item);
                c.addControl(bt);
                //  edit button
                xButton edit = xButton.createImageButton(C.ID_SORT_TECHNICAL_EDIT, this, imgs, 0);
                edit.setData(item); //sua loi 16/03/2020 null);
                //edit.setData(bt);
                //edit.setDataInt((1<<16)|i);
                edit.setPosition(w - edit.getW(), y + (bt.getH() - edit.getH()) / 2);
                c.addControl(edit);

                y = bt.getBottom() + 2;
            }

            if (i < 9)
            {
                bt = xButton.createStandardButton(C.ID_SORT_TECHNICAL2, this, "TẠO BỘ LỌC", w);
                bt.setTextColor(0xffff0000);
                bt.setSize(bw, bth);
                bt.setPosition(x, y);
                //bt.setDataInt((1<<16)|i);
                bt.setData(null);
                c.addControl(bt);

                y = bt.getBottom() + 2;
            }

            c.setSize(w, y);

            return c;
        }

        xContainer createFilterCandleControls(int w, int h)
        {
            int i = 0;
            xLabel l;
            int x, y;
            x = 10;
            y = 10;
            xButton bt;
            xContainer c = new xContainer();
            x = 0;
            y = 0;
            //========buttons

            String[] bts = {
                                "Bullish Engufing",
                                "Bullish Pearcing",
                                "Morning star",
                                "Hammer",
                                "Bullish Harami",
                                "--",
                                "--",
                                "--",
                                "--"};
            int[] ids = { 
                            C.ID_SORT_CANDLE_BASE + C.SORT_BULLISH_ENGULFING,
                            C.ID_SORT_CANDLE_BASE + C.SORT_BULLISH_PEARCING,
                            C.ID_SORT_CANDLE_BASE + C.SORT_MORNING_STAR,
                            C.ID_SORT_CANDLE_BASE + C.SORT_HAMMER,
                            C.ID_SORT_CANDLE_BASE + C.SORT_BULLISH_HARAMI,
                            0,
                            0,
                            0,
                            0};

            int bth = 25;
            y += 6;
            for (i = 0; i < bts.Length; i++)
            {
                bt = xButton.createStandardButton(ids[i], this, bts[i], w);
                bt.setSize(w, bth);
                bt.setPosition(x, y);
                c.addControl(bt);

                y = bt.getBottom();// +2;
            }

            c.setSize(w, y);

            return c;
        }

        public override void onEvent(object sender, int evt, int aIntParameter, object aParameter)
        {
            base.onEvent(sender, evt, aIntParameter, aParameter);
            if (evt == xBaseControl.EVT_NET_DATA_AVAILABLE)
            {
                if (mNetworkContacting != null)
                {
                    mNetworkContacting.setMsg2((aIntParameter / 1024) + " KB");
                }
            }
            //================================
         
            Share share = mShare;

            if (evt == xBaseControl.EVT_ON_CONTEXT_MENU)
            {
                processMenuContext(aIntParameter);
            }
            if (evt == C.EVT_REPAINT_CHARTS)
            {
                refreshCharts();
            }
            if (evt == C.EVT_SHOW_TUTORIAL)
            {
                showHelp(aIntParameter);
            }
            if (evt == xBaseControl.EVT_BUTTON_CLICKED)
            {
                if (aIntParameter >= C.ID_SYMBOL_CLICK_START && aIntParameter < C.ID_SYMBOL_CLICK_END)
                {
                    int idx = aIntParameter - C.ID_SYMBOL_CLICK_START;
                    stShareGroup g = mContext.getCurrentShareGroup();
                    if (idx >= 0 && idx < g.getTotal())
                    {
                        string code = g.getCodeAt(idx);
                        share = mContext.mShareManager.getShare(code);

                        if (share != null)
                        {
                            mShare = share;
                            reloadShare(mShare, true);

                            mMainHistoryChartControl.setShare(mShare);

                            mNetState = NETSTATE_GET_QUOTE_DATA_PREPARING;

                            refreshCharts();
                        }
                    }
                }
                if (aIntParameter == C.ID_ALARM_MANAGER)
                {
                    showAlarmManager();
                }
                if (aIntParameter == C.ID_ALARM_MODIFY)
                {
                    settingAlarm((stAlarm)aParameter);

                    mAlarmContainer.invalidate();
                    updateAlarmListUI(mAlarmContainer);
                }
                //==========================
                if (aIntParameter == C.ID_SORT_TECHNICAL || aIntParameter == C.ID_SORT_TECHNICAL2)
                {
                    if (mContext.isExpiredAccount())
                    {
                        showDialogOK("Tài khoản của bạn đã hết hạn, hãy gia hạn tài khoản để sử dụng chức năng này.");
                        return;
                    }
                    xBaseControl c = (xBaseControl)sender;
                    FilterSet filterSet = (FilterSet)c.getData();
                    if (filterSet == null)
                    {
                        createAFilter();
                    }
                    else
                    {
                        sortTechnical(filterSet);
                    }
                }

                if (aIntParameter == C.ID_SORT_TECHNICAL_EDIT || aIntParameter == C.ID_SORT_TECHNICAL_EDIT2)
                {
                    //int n = ((xBaseControl)sender).getDataInt();
                    xBaseControl c = (xBaseControl)sender;
                    if (c.getData() != null)
                    {
                        showSortTechnicalConfig((xButton)sender, (FilterSet)c.getData());
                    }
                    
                }
                if (aIntParameter == C.ID_BUTTON_QUOTE)
                {
                    string code = mQuickCode.getText();
                    share = mContext.mShareManager.getShare(code);
                    if (share != null)
                    {
                        if (share.isIndex()
                            || mContext.isQuoteFavorite(share))
                        {
                            share.loadShareFromFile(true);
                            if (mContext.isQuoteFavorite(share))
                                mNetState = NETSTATE_GET_QUOTE_DATA_PREPARING;
                        }
                        else
                        {
                            share.loadShareFromCommonData(true);
                        }

                        mContext.setCurrentShare(share);

                        refreshCharts();
                    }
                    else
                    {
                        showDialogOK("Không tìm thấy cổ phiếu " + code);
                    }
                }
                if (aIntParameter >= C.ID_SORT_BASE && aIntParameter <= C.ID_SORT_END)
                {
                    if (mContext.isExpiredAccount())
                    {
                        showDialogOK("Tài khoản của bạn đã hết hạn, hãy gia hạn tài khoản để sử dụng chức năng này.");
                        return;
                    }
                    doSortBasic(aIntParameter - C.ID_SORT_BASE);
                }
                /*
                if (aIntParameter >= C.ID_SORT_TA_BASE && aIntParameter <= C.ID_SORT_TA_END)
                {
                    doSortTA(aIntParameter - C.ID_SORT_TA_BASE);
                }
                 */
                if (aIntParameter >= C.ID_SORT_CANDLE_BASE && aIntParameter <= C.ID_SORT_CANDLE_END)
                {
                    if (mContext.isExpiredAccount())
                    {
                        showDialogOK("Tài khoản của bạn đã hết hạn, hãy gia hạn tài khoản để sử dụng chức năng này.");
                        return;
                    }
                    doSortCandle(aIntParameter - C.ID_SORT_CANDLE_BASE);
                }
                if (aIntParameter == C.ID_SHOW_FILTER_PARAMETER_FORM)
                {
                    DlgFilterConfig dlg = new DlgFilterConfig();
                    dlg.ShowDialog();

                    doFilter();
                    recreateTableList();
                }
            }
            if (evt == xBaseControl.EVT_ON_ROW_SELECTED)
            {
                if (aIntParameter == C.ID_PRICEBOARD_TABLE)
                {
                    RowQuoteList r = (RowQuoteList)aParameter;
                    share = r.mShare;

                    if (share != null)
                    {
                        if (share.isIndex() || mContext.isQuoteFavorite(share))
                        {
                            share.loadShareFromFile(false);
                            if (mContext.isQuoteFavorite(share))
                                mNetState = NETSTATE_GET_QUOTE_DATA_PREPARING;
                        }
                        else
                        {
                            share.loadShareFromCommonData(true);
                        }

                        mContext.setCurrentShare(share);

                        //refreshCharts();
                        onChangedQuote();
                        //AAA
                    }
                }
            }
            if (evt == xBaseControl.EVT_ON_SPLITTER_SIZE_CHANGED)
            {
                
            }
        }

        void doSortBasic(int sort)
        {
            if (sort == mCurrentSort)
            {
                mSortTopToBottom = !mSortTopToBottom;
            }
            else
            {
                mSortTopToBottom = true;
            }

            mCurrentSort = sort;

            //bool mSortTopToBottom = true;
            StringBuilder sb = Utils.sb;
            sb.Length = 0;

            doFilter();
            switch (sort)
            {
                case C.SORT_HEIKEN:
                    mContext.mShareManager.sortHeiken(mFilteredShares);
                    setTitle("Heiken");
                    break;
                case C.SORT_ABC:
                    mContext.mShareManager.sortAlphabet(mFilteredShares);
                    setTitle("Alphabet");
                    break;
                case C.SORT_TICHLUY:
                    onClickSortTichluy();
                    break;
                case C.SORT_VOL_DOTBIEN:
                    mContext.mShareManager.sortVolDotbien(mFilteredShares);
                    setTitle("Vol đột biến");
                    break;
                case C.SORT_ON_MARKET_VOLUME:
                    setTitle("KLCP niêm yết");
                    mContext.mShareManager.sortOnMarketVolume(mFilteredShares);
                    break;

                case C.SORT_MOST_DECREASE:
                case C.SORT_MOST_INCREASE:
                    {
                        setTitle("Giảm/Tăng giá nhiều nhất");

                        if (sort == C.SORT_MOST_INCREASE)
                        {
                            sb.Append("Tăng nhiều nhất trong số ngày:");
                            mSortTopToBottom = true;
                        }
                        else
                        {
                            sb.Append("Giảm nhiều nhất trong số ngày:");
                            mSortTopToBottom = false;
                        }
                        DlgEnterDay dlg = DlgEnterDay.createDialog(this, sb.ToString());
                        dlg.ShowDialog();
                        if (dlg.getResultID() == C.ID_DLG_BUTTON_OK)
                        {
                            String sd = dlg.getText();
                            try
                            {
                                int days = Int32.Parse(sd);
                                mContext.mShareManager.sortMostIncreased(mFilteredShares, days);
                            }catch(Exception e)
                            {
                                showDialogOK(e.ToString());
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    break;
                case C.SORT_TODAY_BIGGEST_VOLUME:
                    mContext.mShareManager.sortTodayBiggestVolume(mFilteredShares);
                    break;
                case C.SORT_EPS:
                    setTitle("EPS cao nhất");
                    mContext.mShareManager.sortHighestEPS(mFilteredShares);
                    break;
                case C.SORT_PE:
                    setTitle("P/E thấp nhất");
                    mSortTopToBottom = false;
                    mContext.mShareManager.sortLowestPE(mFilteredShares);
                    break;
                case C.SORT_BETA:
                    setTitle("Beta");
                    mContext.mShareManager.sortLowestBeta(mFilteredShares);
                    break;
                case C.SORT_ROA:
                    setTitle("ROA");
                    mContext.mShareManager.sortLowestROA(mFilteredShares);
                    break;
                case C.SORT_ROE:
                    setTitle("ROE");
                    mContext.mShareManager.sortLowestROE(mFilteredShares);
                    break;
//                case C.SORT_BOOK_VALUE:
                    //mContext.mShareManager.sortAlphabet(mFilteredShares);
                    //break;
                case C.SORT_LOWEST_PRICE:
                    setTitle("CP rẻ nhất");
                    mContext.mShareManager.sortCheapestPrice(mFilteredShares);
                    break;
                case C.SORT_VONHOATT:
                    setTitle("Vốn hóa thị trường");
                    mContext.mShareManager.sortOnMarketValue(mFilteredShares);
                    break;
            }

            reorderFinal(mSortTopToBottom);

            recreateTableList();
            refreshCharts();
        }
        /*
        void doSortTA(int sort)
        {
            if (sort == mCurrentSort)
            {
                mSortTopToBottom = !mSortTopToBottom;
            }
            else
                mSortTopToBottom = true;

            mCurrentSort = sort;

            //bool mSortTopToBottom = true;
            StringBuilder sb = Utils.sb;
            sb.Length = 0;

            //-------------------------
            doFilter();
            //-------------------------
            switch (sort)
            {
                case C.SORT_TA_MACD_CUT_SIGNAL:
                    mContext.mShareManager.sortMacdCutSignal(mFilteredShares);
                    setSubchartType(SubchartsContainer.SUBCHART_MACD);
                    break;
                case C.SORT_STOCHASTIC:
                    mContext.mShareManager.sortStochastic(mFilteredShares);
                    setSubchartType(SubchartsContainer.SUBCHART_STOCHASTIC_SLOW);
                    break;
                case C.SORT_RSI_OVERSOLD:
                    mContext.mShareManager.sortRSIOversold(mFilteredShares);
                    setSubchartType(SubchartsContainer.SUBCHART_RSI);
                    break;
                case C.SORT_MFI_OVERSOLD:
                    mContext.mShareManager.sortMFIOversold(mFilteredShares);
                    setSubchartType(SubchartsContainer.SUBCHART_MFI);
                    break;
                case C.SORT_PSAR_REVERSE_UP:
                    mContext.mShareManager.sortPSARReverseUp(mFilteredShares);
                    //mMainChart.showAttachChart(ChartBase.CHART_PSAR);
                    break;
                case C.SORT_TENKAN_CUT_KIJUN:
                    mContext.mShareManager.sortTenkanCutKijunAbove(mFilteredShares);
                    //mMainChart.hideAttachChart(ChartBase.CHART_BOLLINGER);
                    //mMainChart.showAttachChart(ChartBase.CHART_ICHIMOKU);
                    break;
                case C.SORT_PRICE_ENTERS_KUMO:
                    mContext.mShareManager.sortPriceEnterKumo(mFilteredShares);
                    //mMainChart.hideAttachChart(ChartBase.CHART_BOLLINGER);
                    //mMainChart.showAttachChart(ChartBase.CHART_ICHIMOKU);
                    break;
                case C.SORT_PRICE_UP_ABOVE_KUMO:
                    mContext.mShareManager.sortPriceAboveKumo(mFilteredShares);
                    break;
                    /*
                case C.SORT_BULLISH_MACD_RSI_MFI:
                    mContext.mShareManager.sortBullishDiverRSI(mFilteredShares);
                    setSubchartType(SubchartsContainer.SUBCHART_RSI);
                    break;
                case C.SORT_BULLISH_DIVER_ROC:
                    mContext.mShareManager.sortBullishDiverROC(mFilteredShares);
                    setSubchartType(SubchartsContainer.SUBCHART_ROC);
                    break;
                case C.SORT_BULLISH_STOCK_OBV_ROC:
                    mContext.mShareManager.sortBullishDiverMFI(mFilteredShares);
                    setSubchartType(SubchartsContainer.SUBCHART_MFI);
                    break;
                     * /
                case C.SORT_VOLUME_UP:
                    mSortTopToBottom = true;
                    mContext.mShareManager.sortBullishVolume(mFilteredShares);
                    break;
                default:
                    break;
            }
            //-------------------------
            reorderFinal(mSortTopToBottom);
            //-------------------------
            recreateTableList();
        }
         */

        void doSortCandle(int sort)
        {
            //mMainChart.showAttachChart(ChartBase.CHART_BOLLINGER);

            if (sort == mCurrentSort)
            {
                mSortTopToBottom = !mSortTopToBottom;
            }
            else
                mSortTopToBottom = true;

            mCurrentSort = sort;

            //mShowingInfo = true;
            //bool mSortTopToBottom = true;
            StringBuilder sb = Utils.sb;
            sb.Length = 0;

            //-------------------------
            doFilter();
            //-------------------------
            switch (sort)
            {
                case C.SORT_BULLISH_ENGULFING:
                    mContext.mShareManager.sortCandleBullishEngulfing(mFilteredShares);
                    break;
                case C.SORT_BULLISH_PEARCING:
                    mContext.mShareManager.sortCandleBullishPearcing(mFilteredShares);
                    break;
                case C.SORT_MORNING_STAR:
                    mContext.mShareManager.sortCandleMorningStar(mFilteredShares);
                    break;
                case C.SORT_HAMMER:
                    mContext.mShareManager.sortCandleHammer(mFilteredShares);
                    break;
                case C.SORT_BULLISH_HARAMI:
                    mContext.mShareManager.sortCandleHarami(mFilteredShares);
                    break;
                default:
                    break;
            }
            //-------------------------
            reorderFinal(mSortTopToBottom);
            //-------------------------
            recreateTableList();
        }

        void reorderFinal(bool sortTopToBottom)
        {
            if (!sortTopToBottom)
            {
                mFilteredShares.makeReverse();
            }
        }

        void doFilterQuick()
        {
            bool saved = mContext.mOptFilterKLTB30Use;
            mContext.mOptFilterKLTB30Use = false;
            doFilter();
            mContext.mOptFilterKLTB30Use = saved;
        }

        void doFilter()
        {
            mFilteredShares.removeAllElements();

            int i = 0;
            Share share;
            xVector v0 = new xVector(2000);
            //  fill the list
            int shareCnt = mContext.mShareManager.getTotalShareIDCount();

            {
                int[] market = {0};
                for (i = 0; i < shareCnt; i++)
                {
                    int shareID = mContext.mShareManager.getShareIDAt(i, market);
                    share = mContext.mShareManager.getShare(shareID);

                    if (!share.isIndex())
                    {
                        v0.addElement(share);
                    }
                }
            }
            // loc theo market
            mAcceptedMarkets[0] = 0;
            mAcceptedMarkets[1] = 0;
            mAcceptedMarkets[2] = 0;
            if (mContext.mOptFilterVNIndex)
                mAcceptedMarkets[0] = 1;
            if (mContext.mOptFilterHNX)
                mAcceptedMarkets[1] = 2;

            //  by market ID
            int cnt = v0.size();
            xVector v1 = new xVector(v0.size());

            for (i = 0; i < v0.size(); i++)
            {
                share = (Share)v0.elementAt(i);
                v1.addElement(share);
                /*
                int marketID = share.mMarketID;
                for (int j = 0; j < mAcceptedMarkets.Length; j++)
                {
                    if (marketID == mAcceptedMarkets[j])
                    {
                        v1.addElement(share);
                    }
                }
                */
            }
            //====================//
            xVector tmp = v0;
            v0 = v1;
            v1 = tmp;
            v1.removeAllElements();
            //====================//

            //  by KLTB 10 phien
            if (mContext.mOptFilterKLTB30Use)
            {
                for (i = 0; i < v0.size(); i++)
                {
                    share = (Share)v0.elementAt(i);


                    //if (share.mCode.CompareTo("DVH") == 0)
                    //{
                        //i = i;
                    //}

                    share.loadShareFromCommonData(true);

                    int vol10 = share.getTotalVolume(5);
                    if (vol10 < 0)
                    {
                        vol10 = share.calcTotalVolume(5);
                    }

                    int giatri = (int)((vol10 * share.getClose())/1000);//  convert to trieu
                    if (giatri >= mContext.mOptFilterGTGD)
                    {
                        v1.addElement(share);
                    }
                }
            }
            else
            {
                tmp = v0;
                v0 = v1;
                v1 = tmp;
            }
            //====================//
            tmp = v0;
            v0 = v1;
            v1 = tmp;
            v1.removeAllElements();
            //====================//
            //=================hi price & low price===================
            for (i = 0; i < v0.size(); i++)
            {
                share = (Share)v0.elementAt(i);
                stPriceboardState ps = mContext.mPriceboard.getPriceboard(share.mID);
                if (ps != null)
                {
                    float price = ps.getRef()*1000;

                    //price *= 100;

                    if (mContext.mOptFilterHiPriceUse && mContext.mOptFilterLowPriceUse)
                    {
                        if (price >= mContext.mOptFilterLowPrice && price <= mContext.mOptFilterHiPrice)
                            v1.addElement(share);
                    }
                    else if (mContext.mOptFilterHiPriceUse)
                    {
                        if (price <= mContext.mOptFilterHiPrice)
                            v1.addElement(share);
                    }
                    else if (mContext.mOptFilterLowPriceUse)
                    {
                        if (price >= mContext.mOptFilterLowPrice)
                            v1.addElement(share);
                    }
                    else
                    {
                        v1.addElement(share);
                    }
                }
            }
            //====================//
            tmp = v0;
            v0 = v1;
            v1 = tmp;
            v1.removeAllElements();
            //====================//

            for (i = 0; i < v0.size(); i++)
            {
                object o = v0.elementAt(i);
                share = (Share)o;
                //System.Console.WriteLine(share.mCode + "/" + share.mSortParam);
                mFilteredShares.addElement(o);
            }
        }

        override public void onShareGroupSelected(stShareGroup g)
        {
            mContext.setCurrentShareGroup(g);
            int shareID;

            mFilteredShares.removeAllElements();
            for (int i = 0; i < g.getTotal(); i++)
            {
                shareID = g.getIDAt(i);
                Share share = mContext.mShareManager.getShare(shareID);
                mFilteredShares.addElement(share);
            }
            //============now recreate the list
            recreateTableList();
        }

        xTabControl mSearchControl = null;
        xBaseControl recreateSearchControl()
        {
            xTabControl tab = mSearchControl;

            if (mSearchControl == null)
            {
                mSearchControl = new xTabControl();
            }
            tab = mSearchControl;
            tab.removeAllPages();

            int w = mLeftPanel.getW();
            int h = mLeftPanel.getH()*2/5;
            tab.setSize(w, h);
            
            xContainer controls;
            xTabPage page;

            //==========ky thuat=========
            page = new xTabPage("Kỹ thuật1");
            controls = createFilterTAControls(w - 20, h - 30);
            page.setSize(controls);
            page.addControl(controls);
            tab.addPage(page);
            
            page = new xTabPage("Kỹ thuật2");
            controls = createFilterTA2Controls(w - 20, h - 30);
            page.setSize(controls);
            page.addControl(controls);
            tab.addPage(page);

            page = new xTabPage("Cơ bản1");
            controls = createFilterBasicControls(w - 20, h - 30);
            page.setSize(controls);
            page.addControl(controls);
            tab.addPage(page);

            page = new xTabPage("Cơ bản2");
            controls = createFilterBasicControls2(w - 20, h - 30);
            page.setSize(controls);
            page.addControl(controls);
            tab.addPage(page);
            //==========candle=========
            /*
            page = new xTabPage("Candle");
            controls = createFilterCandleControls(w - 20, h - 30);
            page.setSize(controls);
            page.addControl(controls);
            tab.addPage(page);
             */

            return tab;
        }

        void setSubchartType(int type)
        {
            /*
            for (int i = 0; i < mSubCharts.size(); i++)
            {
                SubchartsContainer c = (SubchartsContainer)mSubCharts.elementAt(i);
                if (c.getSubchartID() == type)
                    return;
            }

            if (mSubCharts.size() > 0)
            {
                SubchartsContainer c = (SubchartsContainer)mSubCharts.elementAt(0);
                c.setChart(type);
            }
             */
        }

        override public void onNetworkCompleted(bool success)
        {
            if (NETSTATE_GET_QUOTE_DATA == mNetState)
            {
                reloadShare(mShare, true);
            }

            mNetState = NETSTATE_NORMAL;

            if (mNetworkContacting != null)
            {
                mNetworkContacting.Hide();
                mNetworkContacting = null;
            }

            //createRightPanel();
            if (mShare != null)
            {
                mShare.clearCalculations();
            }
            refreshCharts();
            setStatusMsg("{Chọn vùng: shift+bấm&rê chuột}, {Vẽ đường: ctrl+bấm&rê chuột}, {Clone trend: ctrl+center point}");
            setStatusMsg1("{Zoom chart: nhấp đúp chuột}, {Thickness: -/+}");
            //updateUI();
        }

        void onChangedQuote()
        {
            for (int i = 0; i < mPanels.size(); i++)
            {
                HistoryChartControl his = (HistoryChartControl)mPanels.elementAt(i);
                his.onChangedQuote();
                //his.invalidateCharts();
            }
        }

        void refreshCharts()
        {
            for (int i = 0; i < mPanels.size(); i++)
            {
                HistoryChartControl his = (HistoryChartControl)mPanels.elementAt(i);
                //his.onChangedQuote();
                his.invalidateCharts();
            }
        }

        void reloadShare(Share share, bool applyTodayCandle)
        {
            if (share != null)
            {
                if (mContext.isQuoteFavorite(share)
                    || share.isIndex())
                {
                    if (!share.loadShareFromFile(applyTodayCandle))
                    {
                        if (!mContext.isOnline())
                        {
                            //  offline mode: use from common
                            share.loadShareFromCommonData(false);
                        }
                    }
                }
                else
                {
                    share.loadShareFromCommonData(true);
                }
            }
        }

        void processMenuContext(int idx)
        {
            int chart = -1;
            if (idx == C.ID_EDIT_BOLLINGER)
            {
                chart = ChartBase.CHART_BOLLINGER;
            }
            if (idx == C.ID_EDIT_ENVELOP)
            {
                chart = ChartBase.CHART_ENVELOP;
            }
            else if (idx == C.ID_EDIT_ICHIMOKU)
            {
                chart = ChartBase.CHART_ICHIMOKU;
            }
            else if (idx == C.ID_EDIT_PSAR)
            {
                chart = ChartBase.CHART_PSAR;
            }
            else if (idx == C.ID_EDIT_ZIGZAG)
            {
                chart = ChartBase.CHART_ZIGZAG;
            }
            else if (idx == C.ID_CAPTURE_IMAGE)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "PNG Image | *.png";

                DialogResult r = dlg.ShowDialog();
                if (r != DialogResult.Cancel)
                {
                    Utils.captureViewAsImage(mRightPanel, dlg.FileName);
                }
            }
            if (chart != -1)
            {
                FormSettingParameters dlg = new FormSettingParameters(mShare, this);
                dlg.setCurrentChart(chart);
                dlg.ShowDialog();
            }
            else
            {
                
            }
        }

        void createAFilter()
        {
            FilterSet filterSet = new FilterSet();
            FilterSetDialog dlg = new FilterSetDialog();
            dlg.setFilterSet(filterSet);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FilterManager.getInstance().addFilterSet(filterSet);
                FilterManager.getInstance().saveFilterSets();
                recreateSearchControl();
            }
        }

        bool isProcessing = false;
        DlgProcessing dlgProcessing;
        delegate void onDoFilterDone();
        void _onDoFilterDone()
        {
            if (dlgProcessing != null)
            {
                try
                {
                    dlgProcessing.Close();
                    dlgProcessing = null;
                }
                catch (Exception e)
                {
                }
            }

            recreateTableList();
            refreshCharts();
        }

        void showBusyDialog()
        {
            if (dlgProcessing == null)
            {
                dlgProcessing = new DlgProcessing();
                dlgProcessing.Show();
            }
        }


        void sortTechnical(FilterSet filterSet)
        {
            if (isProcessing)
            {
                return;
            }
            //int flags = mContext.mSortTechnicalParams[type].elementAt(idx);
            //string s = (string)mContext.mSortTechnicalName[type].elementAt(idx);
            setTitle(filterSet.name);

            showBusyDialog();
            isProcessing = true;
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
             {
                 //===========================
                 try
                 {
                     doFilter();

                     xVector vFired = new xVector();
                     for (int i = 0; i < mFilteredShares.size(); i++)
                     {
                         Share share = (Share)mFilteredShares.elementAt(i);
                         mContext.mShareManager.loadShareFromCommon(share, 250, true);
                         bool fired = true;
                         for (int j = 0; j < filterSet.getFilterItemCnt(); j++)
                         {
                             FilterItem filterItem = filterSet.getFilterItemAt(j);
                             bool fired1 = FilterManager.getInstance().hasSignal(filterItem, share);
                             if (fired1 == false)
                             {
                                 fired = false;
                                 break;
                             }
                         }
                         if (fired)
                         {
                             vFired.addElement(share);
                         }
                     }

                     mFilteredShares.removeAllElements();
                     for (int i = 0; i < vFired.size(); i++)
                     {
                         Share share = (Share)vFired.elementAt(i);
                         mFilteredShares.addElement(share);
                     }

                     mContext.mShareManager.sortABC(mFilteredShares);
                 }
                 catch (Exception e)
                 {
                     Utils.trace(e.Message);
                 }
                 isProcessing = false;
                 //===========================
                 this.Invoke((onDoFilterDone)_onDoFilterDone);
             }, null);
        }

        

        void showSortTechnicalConfig(xButton button, FilterSet filterSet)
        {
            /*
            DlgFilterDefine dlg = new DlgFilterDefine();
            dlg.setFilter(type, idx);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //recreateSearchControl();
                string name = (string)mContext.mSortTechnicalName[type].elementAt(idx);
                xButton bt = (xButton)button.getData();
                bt.setText(name);

                mContext.saveTechnicalSort();
            }
            */
            FilterSetDialog dlg = new FilterSetDialog();
            dlg.setFilterSet(filterSet);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FilterManager.getInstance().saveFilterSets();
                recreateSearchControl();
            }
        }

        public override void onSizeChanged()
        {
            /*
            if (MainApplication.getInstance().getDeviceW() < 600
                || MainApplication.getInstance().getDeviceH() < 400)
                return;
             */

            base.onSizeChanged();
            if (this.Size.Width == 0 || this.Size.Height == 0)
                return;
            if (getW() > 0 && getH() > 0)
                updateUI();
        }

        public void onClickSortTichluy()
        {
            FilterItem filterItem = new FilterItem();
            filterItem.param1 = GlobalData.getData().getValueInt("tichluy_daodong");
            filterItem.param2 = GlobalData.getData().getValueInt("tichluy_ngay");
            filterItem.type = FilterManager.SIGNAL_TICHLUY;

            if (filterItem.param1 < 1 || filterItem.param1 > 50)
            {
                filterItem.param1 = 7;
            }
            if (filterItem.param2 < 1 || filterItem.param2 > 50)
            {
                filterItem.param2 = 10;
            }

            DlgFilterParamSetting dlg = new DlgFilterParamSetting();
            dlg.setFilterItem(filterItem);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (filterItem.param1 < 1 || filterItem.param1 > 50)
                {
                    filterItem.param1 = 7;
                }
                if (filterItem.param2 < 1 || filterItem.param2 > 50)
                {
                    filterItem.param2 = 10;
                }
                GlobalData.getData().setValueInt("tichluy_daodong", (int)filterItem.param1);
                GlobalData.getData().setValueInt("tichluy_ngay", (int)filterItem.param2);
                GlobalData.saveData();

                doFilter();
                mContext.mShareManager.sortTichluy((int)filterItem.param1, (int)filterItem.param2, mFilteredShares);
                
                recreateSearchControl();
            }
        }
    }
}
