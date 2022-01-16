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
using stock123.app.utils;
using stock123.app.data.userdata;

namespace stock123.app
{
    public class ViewHistoryChart: ScreenBase
    {
        public const int TYPE_SEARCH = 0;
        public const int TYPE_CHART_DEPRECATED = 1;
        public int mScreenType = TYPE_SEARCH;//TYPE_CHART;

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

        xContainer mSymbolContainer;
        xButton btChonNhom;

        String sortedColumn = null;

        xContainer mLeftPanel;  //  quote list & search control
        xContainer mRightPanel;
        xVector mPanels = new xVector(2);

        xTextField mQuickCode;

        xVector mFilteredShares = new xVector(1000);
        String mTitleOfFilter;
        bool[] mFilteredSharesOrderAccend = {true, true, true};
        int mFilteredSharesOrderLastSortType = -1;
        xContainer listFilteredShare;

        xContainer myGroupPage;
        xContainer nhomnganhPage;

        xContainer listMygroupShares;
        xContainer listNhomnganhShares;
        
        xTabControl mTabOfGroups;

        xListView listMyGroups;
        xListView listNhomnganhs;


        int[] mAcceptedMarkets = { 1, 0, 0, 0};

        bool mSortTopToBottom;
        int mCurrentSort;

        DlgContactingServer mNetworkContacting;
        HistoryChartControl mMainHistoryChartControl;

        ToolBarButton mAlarmButton;
        int mAlarmAnimationIDX = 0;
        xTimer mAlarmAnimation = new xTimer(500);

        NetProtocol mNetProtocol;
        long mLastCheckTick;

        public ViewHistoryChart(Share share)
            : base()
        {
            mScreenType = TYPE_SEARCH;// share != null ? TYPE_CHART : TYPE_SEARCH;
            mShare = share;
//            List<Share> list;
//            IComparer<Share>
        }

        public override int getToolbarH()
        {
            return 39;
        }

        override public int getWorkingH()
        {
            return getH() - getToolbarH() - getStatusbarH() - 28;
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
                if (mShare == null)
                {
                    mShare = mContext.mShareManager.getVnindexShareAt(0);
                }
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

                        share.loadShareFromFile(false);
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
            else if (mNetState == NETSTATE_NORMAL && !mShare.isRealtime())
            {
                if (mLastCheckTick == 0) {
                    mLastCheckTick = Utils.currentTimeMillis();
                }
                long now = Utils.currentTimeMillis();
                long delta = now - mLastCheckTick;
                if (Utils.isWorkingTime() && delta > 30000)
                {
                    stCandle c = mShare.getTodayCandle(false);
                    if (c.date == mShare.getLastCandleDate())
                    {
                        if (c.close != mShare.getClose(mShare.getCandleCnt() - 1)
                            || c.volume != mShare.getVolume(mShare.getCandleCnt()-1))
                        {
                            int start = mShare.mBeginIdx;
                            int end = mShare.mEndIdx;
                            int sel = mShare.mSelectedCandle;

                            mShare.appendTodayCandle();

                            mShare.setStartEndIndex(start, end);
                            mShare.mSelectedCandle = sel;

                            refreshCharts();

                            mLastCheckTick = now;
                        }
                    }
                }
            }
        }

        void updateUI()
        {
            logTimeElapsedStart();
            //--------------------------------------
            removeAllControls();
            mRightPanel = null;
            mLeftPanel = null;
            //--------------------------------------
            createToolbar();

            logTimeElapsedStop("updateUI1");

            createLeftPanel();
            logTimeElapsedStop("updateUI2");
            logTimeElapsedStop("updateUI0");
            createRightPanel(); //  main chart & sub charts
            logTimeElapsedStop("updateUI3");
            
            createSymbolList(0, 0);

            logTimeElapsedStop("updateUI4");

        }

        void createToolbar()
        {
            addToolbar(mContext.getImageList(C.IMG_MAIN_ICONS, 30, 26));
            ToolBar tb = getToolbar();
            tb.ButtonSize = new System.Drawing.Size(60, 30);
            tb.Size = new System.Drawing.Size(300, 30);
            tb.TextAlign = ToolBarTextAlign.Right;

            //addToolbarButton(C.ID_GOTO_HOME_SCREEN, 0, "Bảng giá");
            //addToolbarButton(C.ID_SEARCH_ON, 3, "Lọc");
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
            //addToolbarSeparator();
            //addToolbarButton(C.ID_SPLIT_VIEW, 8, "-View-", ToolBarButtonStyle.ToggleButton, viewPushed);
            //addToolbarButton(C.ID_GOTO_MINI_SCREEN, 4, "Minimize");
            //addToolbarSeparator();
            addToolbarSeparator();
            mAlarmButton = addToolbarButton(C.ID_ALARM_MANAGER, 10, "Cảnh báo");
            //addToolbarSeparator();
            addToolbarSeparator();
            //addToolbarButton(C.ID_GOTO_HELP, 1, "Help");
            //addToolbarButton(C.ID_ADD_SUB_CHART, 2, "+Sub chart");
            //addToolbarButton(C.ID_ADD_MASTER_CHART, 2, "+Master chart");
            //=================================================
            
            //  quick look
            xLabel l = xLabel.createSingleLabel("Mã:");
            xTextField tf = xTextField.createTextField(54);
            mQuickCode = tf;
            int x = 500;

            l.setSize(28, l.getH());
            l.setPosition(x, 9);
            tb.Controls.Add(l.getControl());

            tf.setPosition(x+28, 7);//, dropdown.getY() + l.getH() + 4);
            tb.Controls.Add(tf.getControl());
            tf.setButtonEvent(C.ID_BUTTON_QUOTE, this);
  
            //==============================================================
            addStatusBar();
            setStatusMsg("{Chọn vùng: shift+bấm&rê chuột}, {Vẽ đường: ctrl+bấm&rê chuột}, {Clone trend: ctrl+center point}");
            setStatusMsg1("{Zoom chart: nhấp đúp chuột}, {Thickness: -/+}");
        }

        //=========================================
        long timeMark = 0;
        void logTimeElapsedStart()
        {
            timeMark = Utils.currentTimeMillis();
        }
        void logTimeElapsedStop(String tag)
        {
            Utils.trace(String.Format("----time mark TAG: {0}={1}", tag, (Utils.currentTimeMillis() - timeMark)));
            timeMark = Utils.currentTimeMillis();
        }
        long timeMark2 = 0;
        void logTimeElapsedStart2()
        {
            timeMark2 = Utils.currentTimeMillis();
        }
        void logTimeElapsedStop2(String tag)
        {
            Utils.trace(String.Format("----time mark2 TAG: {0}={1}", tag, (Utils.currentTimeMillis() - timeMark2)));
            timeMark2 = Utils.currentTimeMillis();
        }
        //=========================================

        const int LEFT_PANEL_W = 320;
        int heightOfSharelist()
        {
            return mLeftPanel.getH() / 2 + 40;
        }
        void createLeftPanel()
        {
            int leftW = LEFT_PANEL_W;
            long t = Utils.currentTimeMillis();

            logTimeElapsedStart();
            if (mLeftPanel == null)
            {
                mLeftPanel = new xContainer();
                mLeftPanel.setSize(leftW, getWorkingH());
                mLeftPanel.setPosition(0, getToolbarH());

                
                //  tabs
                int h = heightOfSharelist();
                int y = getH() - h;

                int tw = mLeftPanel.getW();
                int th = mLeftPanel.getH();

                //  tabs control
                mTabOfGroups = new xTabControl();
                mTabOfGroups.setSize(tw, th);
                mLeftPanel.addControl(mTabOfGroups);

                xTabPage page;

                
                //  tab my group
                page = new xTabPage("Theo dõi");
                page.setBackgroundColor(C.COLOR_GRAY_DARK);

                myGroupPage = new xContainer();
                myGroupPage.setSize(tw, th);
                listMygroupShares = new xContainer();
                listMygroupShares.setSize(tw, h);
                
                myGroupPage.addControl(listMygroupShares);

                int h2 = mLeftPanel.getH() - h;
                y = h;

                xContainer c = new xContainer();
                c.setSize(tw, th-y);
                c.setPosition(0, y);

                string[] ss = { "NHÓM" };
                float[] cols = {100.0f};
                listMyGroups = xListView.createListView(this, ss, cols, tw, c.getH(), null);
                listMyGroups.setSize(tw, c.getH());

                xListView.OnClickItem onClickItemMyGroup = (xListViewItem item) =>{
                    stShareGroup g = (stShareGroup)item.getData();
                    createShareListControl(listMygroupShares, g);

                    showChartOfGroup(g);
                };
                listMyGroups.onClickItem += onClickItemMyGroup;    //  action
            

                c.addControl(listMyGroups);

                myGroupPage.addControl(c);

                page.addControl(myGroupPage);
                mTabOfGroups.addPage(page);

                updateMyShareGroupList();

                //  context menu
                int[] ids = { C.ID_ADD_GROUP, C.ID_REMOVE_GROUP};
                String[] texts = { "Thêm nhóm theo dõi", "Xóa nhóm"};
                listMyGroups.setMenuContext(ids, texts, 2);

                xListView.OnMenuItemClick onMenuItemClick = (int id, string title) =>
                {
                    if (id == C.ID_ADD_GROUP)
                    {
                        doAddGroup();
                    }
                    else if (id == C.ID_REMOVE_GROUP)
                    {
                        xListViewItem selItem = listMyGroups.getSelectedItem();
                        if (selItem != null)
                        {
                            stShareGroup g = (stShareGroup)selItem.getData();
                            if (g != null && g.getType() == stShareGroup.ID_GROUP_FAVOR)
                            {
                                doRemoveGroup(g);
                            }
                        }
                    }
                };
                listMyGroups.onMenuItemClick = onMenuItemClick;

                //  tab Nhom nganh
                page = new xTabPage("Nhóm ngành");
                nhomnganhPage = new xContainer();
                nhomnganhPage.setSize(tw, th);
                page.setBackgroundColor(C.COLOR_GRAY_DARK);

                listNhomnganhShares = new xContainer();
                listNhomnganhShares.setSize(tw-10, h);
                nhomnganhPage.addControl(listNhomnganhShares);

                y = h;

                c = new xContainer();
                c.setSize(tw, th - y);
                c.setPosition(0, y);
                nhomnganhPage.addControl(c);

                string[] ss2 = { "NGÀNH" };
                listNhomnganhs = xListView.createListView(this, ss2, cols, tw, c.getH(), null);
                listNhomnganhs.setSize(tw-50, th - y);
                listNhomnganhs.setPosition(0, 0);
                xListView.OnClickItem onClickItemNhomnganhGroup = (xListViewItem item) =>
                {
                    stShareGroup g = (stShareGroup)item.getData();
                    createShareListControl(listNhomnganhShares, g);

                    showChartOfGroup(g);
                };
                listNhomnganhs.onClickItem = onClickItemNhomnganhGroup;   //  action

                c.addControl(listNhomnganhs);

                page.addControl(nhomnganhPage);

                updateNhomnganhGroupList();

                mTabOfGroups.addPage(page);

                //  tab filter
                page = new xTabPage("Filter");
                page.setBackgroundColor(C.COLOR_GRAY_DARK);
                xContainer filterContainer = new xContainer();
                filterContainer.setSize(tw, th);

                listFilteredShare = new xContainer();
                listFilteredShare.setSize(mLeftPanel.getW(), h);
                filterContainer.addControl(listFilteredShare);

                y = h;

                xBaseControl search = recreateSearchControl(mLeftPanel.getW(), mLeftPanel.getH() - h);

                search.setPosition(0, y);
                filterContainer.addControl(search);

                page.addControl(filterContainer);
                mTabOfGroups.addPage(page);

                //-------------------------------
                mLeftPanel.addControl(mTabOfGroups);//mTableList);


                //===================
            }
            mLeftPanel.setPosition(0, getToolbarH());
            mLeftPanel.setSize(leftW, getWorkingH());
            addControl(mLeftPanel);

            logTimeElapsedStop("createLeftPanel1");

            if (mScreenType == TYPE_SEARCH)
            {
                logTimeElapsedStop("createLeftPanel2");
                recreateTableList(-1);
                logTimeElapsedStop("createLeftPanel3");
            }
        }

        void createShareListControl(xContainer c, stShareGroup g)
        {
            c.removeAllControls();

            xBaseControl list = createSharelistOfCurrentGroup(g, c.getW(), c.getH());
            c.addControl(list);

        }

        void recreateTableList(int sortType)
        {
            listFilteredShare.removeAllControls();
            //xListView list = createFilteredList(sortType, mTableList.getW(), mTableList.getH());

            logTimeElapsedStart2();
            xBaseControl list = createFilteredList(sortType, listFilteredShare.getW(), listFilteredShare.getH());

            logTimeElapsedStop2("recreateTableList1");

            list.setPosition(0, 0);

            listFilteredShare.addControl(list);

            logTimeElapsedStop2("recreateTableList2");

            if (mFilteredShares.size() > 0)
            {
                mContext.setCurrentShare((Share)mFilteredShares.elementAt(0));
            }
            else
            {
                mContext.selectDefaultShare();
            }
            logTimeElapsedStop2("recreateTableList3");
            reloadShare(mShare, true);

            logTimeElapsedStop2("recreateTableList4");
        }

        public void createRightPanel()
        {
            Share share = mShare;
            if (share == null)
            {
                return;
            }

            //============================
            int y0 = getToolbarH();
            logTimeElapsedStop("createRightPanel1");

            if (mRightPanel == null)
            {
                mRightPanel = new xContainer();
                addControl(mRightPanel);
            }
            else
            {
                mRightPanel.removeAllControls();
            }
            logTimeElapsedStop("createRightPanel2");
            mRightPanel.setSize(getW() - mLeftPanel.getW(), mLeftPanel.getH());
            //mRightPanel.setPosition(mLeftPanel.getRight(), y0);
            mRightPanel.setPosition(mLeftPanel.getW(), y0);
            
            int w = mRightPanel.getW();
            mPanels.removeAllElements();
            logTimeElapsedStop("createRightPanel3");
            bool showDrawingTool = false;
            if (mMainHistoryChartControl != null)
            {
                showDrawingTool = mMainHistoryChartControl.hasDrawing();
            }
            //==========================
            logTimeElapsedStart2();
            HistoryChartControl his = new HistoryChartControl(mShare, "pannel0", w, mRightPanel.getH(), false);
            logTimeElapsedStop2("hhhhhhhhhhhhhhhhhhhhhhhhhh");
            his.setListener(this);
            his.setPosition(0, 0);

            mMainHistoryChartControl = his;

            mPanels.addElement(his);
            mRightPanel.addControl(his);

            if ((showDrawingTool && !mMainHistoryChartControl.hasDrawing())
                || (!showDrawingTool && mMainHistoryChartControl.hasDrawing()))
                mMainHistoryChartControl.toogleDrawingTool();
        }

        xListView createFilteredList_old(int sortType, int w, int h)
        {
            xVector shares = mFilteredShares;
            int cnt = shares.size();

            if (sortType == -1)
            {
                sortType = ShareSortUtils.SORT_TRADE_VALUE;
            }
            ShareSortUtils.doSort(shares, sortType, 0);

            float[] columnPercents = { 30, 28, 34, 8 }; //  code, price, value
            String[] columnTexts = { "Mã CP", "Giá", "▼ GTGD tỉ", "" };
            sortedColumn = "GTGD (tỉ)";
            //-------------------
            xListView list = xListView.createListView(this, columnTexts, columnPercents, w-20, h, mContext.getImageList(C.IMG_MARKET_ICONS, 20, 21));
            list.setID(C.ID_PRICEBOARD_TABLE);
            list.setBackgroundColor(C.COLOR_GRAY);

            int[] ids0 = { 
                                 C.ID_EXPORT_TO_EXCEL};
            String[] texts0 = {
                                      "Xuất danh sách mã ra file excel(csv)"};

            list.setListener(this);
            list.setMenuContext(ids0, texts0, 1);
            //-------------------

            list.setColumnHeaderTextColor(2, C.COLOR_RED);

            ColumnClickEventHandler columnClick = new ColumnClickEventHandler((sender, e) =>
            {
                ListView lv = (ListView)sender;
                if (e.Column == 0 || e.Column == 1)
                {
                    /*
                    //  alphabet
                    ListViewItemComparer sorter = new ListViewItemComparer(e.Column);
                    sorter.Numeric = e.Column == 1?true:false;
                    sorter.LowToHigher = mFilteredSharesOrderAccend[e.Column];
                    mFilteredSharesOrderAccend[e.Column] = !mFilteredSharesOrderAccend[e.Column];

                    lv.ListViewItemSorter = sorter;
                    sorter.Column = e.Column;

                    lv.Sort();
                     */

                    int sort = e.Column == 0?ShareSortUtils.SORT_SYMBOL:ShareSortUtils.SORT_PRICE;
                    ShareSortUtils.doSort(mFilteredShares, sort, 0);

                    mFilteredSharesOrderAccend[e.Column] = !mFilteredSharesOrderAccend[e.Column];
                    if (mFilteredSharesOrderAccend[e.Column])
                    {
                        mFilteredShares.makeReverse();
                    }

                    list.clear();
                    for (int j = 0; j < mFilteredShares.size(); j++)
                    {
                        Share share = (Share)mFilteredShares.elementAt(j);
                        RowFilterResult r = RowFilterResult.createRowQuoteList(share, this);
                        list.addRow(r);
                        r.update();
                    }
                }
                else
                {
                    showSortColumnMenu(lv, e);
                    
                }
            });
            list.setColumnClickHandler(columnClick);


            //  always add vnindex & hastc
            int i = 0;
                        /*
            for (i = 0; i < mContext.mShareManager.getVnindexCnt(); i++)
            {
                Share share = mContext.mShareManager.getVnindexShareAt(i);
                if (share != null && share.getCode() != null && share.getCode().Length > 0)
                {
                    RowFilterResult r = RowFilterResult.createRowQuoteList(share, this);
                    list.addRow(r);
                    r.update();
                }
            }
             */
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                if (!mContext.mPriceboard.isShareIndex(share.mID))
                {
                    RowFilterResult r = RowFilterResult.createRowQuoteList(share, this);
                    list.addRow(r);
                    r.update();
                }
            }
            return list;
        }

        void updateMyShareGroupList()
        {
            xVector v = Context.userDataManager().shareGroups();
            
            updateShareGroupList(v, listMyGroups);
        }

        void updateNhomnganhGroupList()
        {
            xVector v = new xVector();
            
            //--------------------
            //  cat 1
            for (int i = 0; i < mContext.getShareGroupCount(1); i++)
            {
                stShareGroup g = mContext.getShareGroup(1, i);
                v.addElement(g);
            }
            //  cat 2
            for (int i = 0; i < mContext.getShareGroupCount(2); i++)
            {
                stShareGroup g = mContext.getShareGroup(2, i);
                v.addElement(g);
            }

            updateShareGroupList(v, listNhomnganhs);
        }

        void updateShareGroupList(xVector groups, xListView listView)
        {
            ((ListView)listView.getControl()).HideSelection = false;
            ((ListView)listView.getControl()).Scrollable = true;
            listView.setBackgroundColor(C.COLOR_GRAY_DARK);
            listView.clear();
            for (int i = 0; i < groups.size(); i++)
            {
                stShareGroup g = (stShareGroup)groups.elementAt(i);
                xListViewItem item = xListViewItem.createListViewItem(this, 1);// xListViewItem.createListViewItem(this, 1);
                item.getItem().SubItems[0].Font = mContext.getFontText2();
                //item.getItem().SubItems[0].ForeColor = Color.Blue;
                item.setTextColor(0, C.COLOR_WHITE);
                item.setTextForCell(0, g.getName());

                item.setData(g);
                listView.addRow(item);
            }
        }

        xBaseControl createSharelistOfCurrentGroup(stShareGroup g, int w, int h)
        {
            int rowH = 44;
            int tableH = 22 + 37 + (rowH + 1) * (g.getTotal());
            xScrollView tableContainer = new xScrollView(null, w, h);

            table.TablePriceboard table = new table.TablePriceboard(this, (stShareGroup)null, w - 20, rowH);
            table.setSize(w, tableH);
            table.hasTitle = true;
            if (mTitleOfFilter != null)
            {
                g.setName(mTitleOfFilter);
            }
            table.setShareGroupAsFilterResult(g, ShareSortUtils.SORT_DEFAULT);

            tableContainer.addControl(table);

            //  index of the group
            Share indexOfGroup = mContext.mShareManager.calcIndexOfGroup(g);

            if (indexOfGroup != null)
            {
                indexOfGroup.setCursorScope(mContext.mOptHistoryChartTimeFrame);

                showChartOfShare(indexOfGroup);
                onChangedQuote();
            }

            return tableContainer;
        }

        xBaseControl createFilteredList(int sortType, int w, int h)
        {
            xVector shares = mFilteredShares;
            int cnt = shares.size();

            if (sortType == -1)
            {
                sortType = ShareSortUtils.SORT_TRADE_VALUE;
            }
            ShareSortUtils.doSort(shares, sortType, 0);

            stShareGroup g = new stShareGroup();
            /*
            g.setName("Filter");
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                if (!mContext.mPriceboard.isShareIndex(share.mID))
                {
                    g.addCode(share.getCode());
                }
            }

            */

            int rowH = 44;
            int tableH = 22 + 37 + (rowH+1) * (g.getTotal());
            xScrollView tableContainer = new xScrollView(null, w, h);

            table.TablePriceboard table = new table.TablePriceboard(this, (stShareGroup)null, w - 20, rowH);
            table.setSize(w, tableH);
            table.hasTitle = true;
            if (mTitleOfFilter != null)
            {
                g.setName(mTitleOfFilter);
            }

            table.setShareGroupAsFilterResult(g, ShareSortUtils.SORT_DEFAULT);

            tableContainer.addControl(table);

            return tableContainer;
        }

        public override void onToolbarEvent(int buttonID)
        {
            if (buttonID == C.ID_ALARM_MANAGER)
            {
                //showAlarmManager();
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
            if (buttonID == C.ID_SHOW_FILTER_PARAMETER_FORM)
            {
                DlgFilterConfig dlg = new DlgFilterConfig();
                dlg.ShowDialog();

                doFilter();
                recreateTableList(-1);
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
                                //"KL lớn nhất trong ngày",
                                //"CP rẻ/đắt nhất",
                                "KLCP niêm yết",
                                //"Vốn hóa thị trường",
                                "Heiken"
                           };
            int[] ids = {   
                            C.ID_SORT_BASE + C.SORT_ABC,
                            C.ID_SORT_BASE + C.SORT_TICHLUY,
                            C.ID_SORT_BASE + C.SORT_VOL_DOTBIEN,
                            C.ID_SORT_BASE + C.SORT_MOST_INCREASE,
                            C.ID_SORT_BASE + C.SORT_MOST_DECREASE, 
                            //C.ID_SORT_BASE + C.SORT_TODAY_BIGGEST_VOLUME,
                            //C.ID_SORT_BASE + C.SORT_LOWEST_PRICE,
                            C.ID_SORT_BASE + C.SORT_ON_MARKET_VOLUME,
                            //C.ID_SORT_BASE + C.SORT_VONHOATT,                           
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

            int createdFilters = fm.getFilterSetCnt();
            cnt = createdFilters < 9?createdFilters:9;

            for (i = 0; i < cnt; i++)
            {
                FilterSet item = fm.getFilterSetAt(i);
                string name = item.name;// (string)mContext.mSortTechnicalName[0].elementAt(i);
                //int sort_params = mContext.mSortTechnicalParams[0].elementAt(i);
                logTimeElapsedStart2();
                
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
                if (aIntParameter == C.ID_BUTTON_CHON_NHOM_CP || aIntParameter == C.ID_BUTTON_CHON_NHOM_CP_NGANH)
                {
                    showShareGroupListMenu(aIntParameter == C.ID_BUTTON_CHON_NHOM_CP, ((xButton)sender).getControl());
                }
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
                            showChartOfShare(share);
                        }
                    }
                }
                /*
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
                 */
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
                        showChartOfShare(share);
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
                    recreateTableList(-1);
                }
                if (aIntParameter == C.ID_SELECT_SHARE_CANDLE)
                {
                    share = (Share)aParameter;
                    if (share != null)
                    {
                        showChartOfShare(share);
                        //refreshCharts();
                        onChangedQuote();
                        //AAA
                    }
                }
            }
            if (evt == xBaseControl.EVT_ON_ROW_SELECTED)
            {
                if (aIntParameter == C.ID_PRICEBOARD_TABLE)
                {
                    RowFilterResult r = (RowFilterResult)aParameter;
                    share = r.mShare;

                    if (share != null)
                    {
                        showChartOfShare(share);
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
            int sortType = -1;
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
                        int defDays = GlobalData.getData().getValueInt("most_change_days");
                        if (defDays <= 0) defDays = 5;
                        DlgEnterDay dlg = DlgEnterDay.createDialog(sb.ToString(), defDays);
                        dlg.ShowDialog();
                        if (dlg.getResultID() == C.ID_DLG_BUTTON_OK)
                        {
                            String sd = dlg.getText();
                            try
                            {
                                int days = Int32.Parse(sd);
                                if (days > 0)
                                {
                                    GlobalData.getData().setValueInt("most_change_days", days);
                                    GlobalData.saveData();
                                }
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

            recreateTableList(ShareSortUtils.SORT_IGNORE);
            refreshCharts();
        }

        override public void setTitle(String title)
        {
            base.setTitle(title);
            mTitleOfFilter = title;
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
            recreateTableList(ShareSortUtils.SORT_IGNORE);
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
//            mContext.mOptFilterKLTB30Use = false;
            doFilter();
//            mContext.mOptFilterKLTB30Use = saved;
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

                    if (share == null)
                    {
                        shareID = mContext.mShareManager.getShareIDAt(i, market);
                        share = mContext.mShareManager.getShare(shareID);
                    }

                    if (share != null && !share.isIndex() && share.mCode.Length == 3)
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
                    if (share.getCandleCnt() == 0)
                    {
                        continue;
                    }

                    int vol10 = share.calcAvgVolume(5);

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
                if (o != null)
                {
                    mFilteredShares.addElement(o);
                }
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
                if (share != null)
                {
                    mFilteredShares.addElement(share);
                }
            }
            //============now recreate the list
            recreateTableList(ShareSortUtils.SORT_IGNORE);

            btChonNhom.setText(g.getName());
        }

        xTabControl mSearchControl = null;
        xBaseControl recreateSearchControl(int w, int h)
        {
            xTabControl tab = mSearchControl;

            if (mSearchControl == null)
            {
                mSearchControl = new xTabControl();
            }
            tab = mSearchControl;
            tab.removeAllPages();

            tab.setSize(w, h);
            
            xContainer controls;
            xTabPage page;

            //==========ky thuat=========
            logTimeElapsedStart2();

            page = new xTabPage("Kỹ thuật1");
            controls = createFilterTAControls(w - 20, h - 30);
            page.setSize(controls);
            page.addControl(controls);
            tab.addPage(page);

            logTimeElapsedStop2("recreateSearchControl1");
            
            page = new xTabPage("Kỹ thuật2");
            controls = createFilterTA2Controls(w - 20, h - 30);
            page.setSize(controls);
            page.addControl(controls);
            tab.addPage(page);

            logTimeElapsedStop2("recreateSearchControl2");

            page = new xTabPage("Cơ bản");
            controls = createFilterBasicControls(w - 20, h - 30);
            page.setSize(controls);
            page.addControl(controls);
            tab.addPage(page);

            logTimeElapsedStop2("recreateSearchControl3");

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
            /*
            for (int i = 0; i < mPanels.size(); i++)
            {
                HistoryChartControl his = (HistoryChartControl)mPanels.elementAt(i);
                his.onChangedQuote();
                //his.invalidateCharts();
            }
             */
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
            if (share == null){
                return;
            }
            if (mMainHistoryChartControl != null)
            {
                ((HistoryChartControl)mMainHistoryChartControl).reloadShare(share, applyTodayCandle);
            }
            else
            {
                if (mContext.isQuoteFavorite(share)
                    || share.isIndex())
                {
                    if (!share.loadShareFromFile(applyTodayCandle))
                    {

                        share.loadShareFromCommonData(true);
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

            if (idx == C.ID_EXPORT_TO_EXCEL)
            {
                ShareSortUtils.exportGroupToCSV(mFilteredShares, sortedColumn);

                return;
            }

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
            else if (idx == C.ID_ADD_SYMBOL_TO_GROUP)
            {

            }
            else if (idx == C.ID_RELOAD_DATA_OF_SYMBOL)
            {
                Share.deleteSavedFile(mShare.getCode());

                reloadShare(mShare, true);

                refreshCharts();

                if (mNetProtocol != null)
                {
                    mNetProtocol.cancelNetwork();
                }
                mNetProtocol = mContext.createNetProtocol();
                mNetProtocol.setListener(this);
                mNetState = NETSTATE_GET_QUOTE_DATA_PREPARING;
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
                //FilterManager.getInstance().saveFilterSets();
                Context.userDataManager().flushUserData();
                recreateSearchControl(mLeftPanel.getW(), heightOfSharelist());
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

            recreateTableList(ShareSortUtils.SORT_IGNORE);
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
            mTitleOfFilter = filterSet.name;

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
                         //if (share.getCode().CompareTo("STB") == 0)
                         //{
                             //Utils.trace("stb");
                         //}
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
                         if (share != null)
                         {
                             mFilteredShares.addElement(share);
                         }
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
                Context.userDataManager().flushUserData();
                recreateSearchControl(mLeftPanel.getW(), heightOfSharelist());
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
            {
                updateUI();
            }
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
                
                recreateSearchControl(mLeftPanel.getW(), heightOfSharelist());
            }
        }

        void showChartOfShare(Share share)
        {
            if (share != null && share.mIsGroupIndex)
            {
                mMainHistoryChartControl.setShare(share);
                return;
            }
            //------------------------
            if (share != null && share != mShare)
            {
                mShare = new Share();

                mShare.setCode(share.getCode(), share.getMarketID());
                mShare.setID(share.getID());
                mShare.setCursorScope(Context.getInstance().mOptHistoryChartTimeFrame);

                mShare.allocMemoryUsingShared(false);

                reloadShare(mShare, true);

                mMainHistoryChartControl.setShare(mShare);
                
                mNetState = NETSTATE_GET_QUOTE_DATA_PREPARING;

                refreshCharts();
            }
        }

        void showSortColumnMenu(ListView lv, ColumnClickEventArgs e)
        {
            //  show popup menu
            ContextMenuStrip cm = new ContextMenuStrip();
            cm.Items.Add("RSI");
            cm.Items.Add("MFI");
            cm.Items.Add("Vốn hóa");
            cm.Items.Add("Giá trị giao dịch");
            cm.Items.Add("EPS");
            cm.Items.Add("PE");
            cm.Items.Add("Khối lượng");
            cm.Items.Add("Khối lượng thay đổi (TB3/TB15)");
            cm.Items.Add("Điểm RS/VNIndex");
            cm.Items.Add("-");
            cm.Items.Add("Xuất danh sách ra file excel(csv)");

            cm.ItemClicked += new ToolStripItemClickedEventHandler(
                (sender, item) => {
                    if (item.ClickedItem.Text.CompareTo("Xuất danh sách ra file excel(csv)") == 0)
                    {
                        ShareSortUtils.exportGroupToCSV(mFilteredShares, sortedColumn);
                        return;
                    }

                    float[] columnPercents = { 30, 28, 34, 8}; //  code, price, value

                    String[] columnTexts = { "Mã CP", "Giá", "---", "" };
                    int sortType = ShareSortUtils.SORT_RSI;
                    if (item.ClickedItem.Text.CompareTo("RSI") == 0)
                    {
                        sortType = ShareSortUtils.SORT_RSI;
                        columnTexts[2] = "RSI";
                    }
                    else if (item.ClickedItem.Text.CompareTo("MFI") == 0)
                    {
                        sortType = ShareSortUtils.SORT_MFI;
                        columnTexts[2] = "MFI";
                    }
                    else if (item.ClickedItem.Text.CompareTo("EPS") == 0)
                    {
                        sortType = ShareSortUtils.SORT_EPS;
                        columnTexts[2] = "EPS";
                    }
                    else if (item.ClickedItem.Text.CompareTo("PE") == 0)
                    {
                        sortType = ShareSortUtils.SORT_PE;
                        columnTexts[2] = "PE";
                    }
                    else if (item.ClickedItem.Text.CompareTo("Vốn hóa") == 0)
                    {
                        sortType = ShareSortUtils.SORT_VonHoa;
                        columnTexts[2] = "VốnHóa(tỉ)";
                    }
                    else if (item.ClickedItem.Text.CompareTo("Giá trị giao dịch") == 0)
                    {
                        sortType = ShareSortUtils.SORT_TRADE_VALUE;
                        columnTexts[2] = "GTGD (tỉ)";
                    }
                    else if (item.ClickedItem.Text.CompareTo("Khối lượng") == 0)
                    {
                        sortType = ShareSortUtils.SORT_VOLUME;
                        columnTexts[2] = "Khối lượng";
                    }
                    else if (item.ClickedItem.Text.CompareTo("Khối lượng thay đổi (TB3/TB15)") == 0)
                    {
                        sortType = ShareSortUtils.SORT_THAYDOI_VOL;
                        columnTexts[2] = "+/-Vol(%)";
                    }
                    else if (item.ClickedItem.Text.CompareTo("Điểm RS/VNIndex") == 0)
                    {
                        sortType = ShareSortUtils.SORT_RS_RANKING;
                        columnTexts[2] = "Điểm RS/VNIndex";

                        showGetDaysForRSRankingDialog((xListView)lv.Tag);
                        return;
                    }
                    sortedColumn = columnTexts[2];

                    ShareSortUtils.doSort(mFilteredShares, sortType, 0);

                    if (mFilteredSharesOrderLastSortType != -1)
                    {
                        if (mFilteredSharesOrderLastSortType != sortType)
                        {
                            mFilteredSharesOrderLastSortType = sortType;
                            mFilteredSharesOrderAccend[2] = true;
                        }
                        else
                        {
                            mFilteredSharesOrderAccend[2] = false;
                        }
                    }
                    else
                    {
                        mFilteredSharesOrderAccend[2] = true;
                    }
                    mFilteredSharesOrderLastSortType = sortType;
                    if (!mFilteredSharesOrderAccend[2])
                    {
                        mFilteredShares.makeReverse();
                        mFilteredSharesOrderLastSortType = -1;
                    }
                    
                    columnTexts[2] = "▼ " + columnTexts[2];

                    //==================

                    xListView list = (xListView)lv.Tag;
                    list.clear();
                    list.setColumnHeaders(columnTexts, columnPercents);

                    for (int i = 0; i < mFilteredShares.size(); i++)
                    {
                        Share share = (Share)mFilteredShares.elementAt(i);
                        RowFilterResult r = RowFilterResult.createRowQuoteList(share, this);
                        list.addRow(r);
                        r.update();
                    }
                });

            cm.Show(lv.PointToScreen(new Point(50, 15)));
        }

        void showGetDaysForRSRankingDialog(xListView list)
        {
            int defValue = GlobalData.getData().getValueInt("rs_ranking_days");
            if (defValue == 0)
            {
                defValue = 30;
            }
            DlgEnterDay dlg = DlgEnterDay.createDialog("Điểm RS so với VNIndex trong số ngày", defValue);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                int days = dlg.getDays();

                if (days <= 1)
                {
                    days = 30;
                }
                else
                {
                    GlobalData.getData().setValueInt("rs_ranking_days", days);
                    GlobalData.saveData();
                }

                float[] columnPercents = { 30, 28, 34, 8}; //  code, price, value

                String[] columnTexts = { "Mã CP", "Giá", "Điểm RS/VNIndex", "" };
                int sortType = ShareSortUtils.SORT_RS_RANKING;
                sortedColumn = columnTexts[2];

                ShareSortUtils.doSort(mFilteredShares, sortType, days);

                if (mFilteredSharesOrderLastSortType != -1)
                {
                    if (mFilteredSharesOrderLastSortType != sortType)
                    {
                        mFilteredSharesOrderLastSortType = sortType;
                        mFilteredSharesOrderAccend[2] = true;
                    }
                    else
                    {
                        mFilteredSharesOrderAccend[2] = false;
                    }
                }
                else
                {
                    mFilteredSharesOrderAccend[2] = true;
                }
                mFilteredSharesOrderLastSortType = sortType;
                if (!mFilteredSharesOrderAccend[2])
                {
                    mFilteredShares.makeReverse();
                    mFilteredSharesOrderLastSortType = -1;
                }
                    
                columnTexts[2] = "▼ " + columnTexts[2];

                //==================

                list.clear();
                list.setColumnHeaders(columnTexts, columnPercents);

                for (int i = 0; i < mFilteredShares.size(); i++)
                {
                    Share share = (Share)mFilteredShares.elementAt(i);
                    RowFilterResult r = RowFilterResult.createRowQuoteList(share, this);
                    list.addRow(r);
                    r.update();
                }
            }
        }

        void showShareGroupListMenu(bool isMine, Control c)
        {
            //  show popup menu
            ContextMenuStrip cm = new ContextMenuStrip();

            if (isMine)
            {
                xVector v = Context.userDataManager().shareGroups();
                for (int i = 0; i < v.size(); i++)
                {
                    stShareGroup g = (stShareGroup)v.elementAt(i);
                    cm.Items.Add(g.getName());
                }
            }
            else
            {

                //--------------------
                //  cat 1
                for (int i = 0; i < mContext.getShareGroupCount(1); i++)
                {
                    stShareGroup g = mContext.getShareGroup(1, i);
                    cm.Items.Add(g.getName());
                }
                //  cat 2
                for (int i = 0; i < mContext.getShareGroupCount(2); i++)
                {
                    stShareGroup g = mContext.getShareGroup(2, i);
                    cm.Items.Add(g.getName());
                }
                //--------------------
            }

            cm.ItemClicked += new ToolStripItemClickedEventHandler(
                (sender, item) =>
                {
                    String name = item.ClickedItem.Text;

                    stShareGroup selectedGroup = null;
                    xVector v = Context.userDataManager().shareGroups();
                    for (int i = 0; i < v.size(); i++)
                    {
                        stShareGroup g = (stShareGroup)v.elementAt(i);
                        if (g.getName().CompareTo(name) == 0)
                        {
                            selectedGroup = g;
                            break;
                        }
                    }

                    if (selectedGroup == null)
                    {
                        for (int i = 0; i < mContext.getShareGroupCount(1); i++)
                        {
                            stShareGroup g = mContext.getShareGroup(1, i);
                            if (g.getName().CompareTo(name) == 0)
                            {
                                selectedGroup = g;
                                break;
                            }
                        }
                        for (int i = 0; i < mContext.getShareGroupCount(2); i++)
                        {
                            stShareGroup g = mContext.getShareGroup(2, i);
                            if (g.getName().CompareTo(name) == 0)
                            {
                                selectedGroup = g;
                                break;
                            }
                        }
                    }

                    if (selectedGroup != null)
                    {
                        mContext.setCurrentShareGroup(selectedGroup);

                        onShareGroupSelected(selectedGroup);

                        //createSymbolList(35, 14);
                    }
                    
                });

            cm.Show(c.PointToScreen(new Point(50, 15)));
        }

        void createSymbolList(int itemW, int itemH)
        {
            stShareGroup g = mContext.getCurrentShareGroup();
            mFilteredShares.removeAllElements();

            if (g.getType() == stShareGroup.ID_GROUP_GAINLOSS)
            {
                GainLossManager gm = Context.userDataManager().gainLossManager();
                for (int i = 0; i < gm.getTotal(); i++)
                {
                    stGainloss gainloss = gm.getGainLossAt(i);
                    Share share = mContext.mShareManager.getShare(gainloss.code);
                    if (share != null)
                    {
                        mFilteredShares.addElement(share);
                    }
                }
            }
            else
            {
                for (int i = 0; i < g.getTotal(); i++)
                {
                    String code = g.getCodeAt(i);
                    Share share = mContext.mShareManager.getShare(code);
                    if (share != null)
                    {
                        mFilteredShares.addElement(share);
                    }
                }
            }
            recreateTableList(ShareSortUtils.SORT_SYMBOL);
            /*
            //=================

            itemW = 35;
            itemH = 14;

            int itemPerRow = mSymbolContainer.getW() / itemW;
            int itemMax = itemPerRow*2;
            mSymbolContainer.removeAllControls();

            Font f = new Font(new FontFamily("Arial"), 9.0f);

            for (int i = 0; i < g.getTotal(); i++)
            {
                if (i >= itemMax)
                {
                    break;
                }
                int row = i / itemPerRow;
                int col = i % itemPerRow;
                //  row 1
                xLabel l = xLabel.createSingleLabel(g.getCodeAt(i));
                l.setSize(itemW, itemH);
                l.setFont(f);
                l.setPosition(col* itemW, row*itemH);
                //l.setListener(this);
                l.setTextColor(0xff000000);
                l.enableClick(C.ID_SYMBOL_CLICK_START + i, this);

                mSymbolContainer.addControl(l);
            }
             */
        }

        void showChartOfGroup(stShareGroup group)
        {
        }


        void doAddGroup()
        {
            if (mContext.favoriteGroups().size() > stShareGroup.MAX_FAVORITE_GROUPS)
            {
                showDialogOK(String.Format("Số nhóm tự tạo không quá: {0}", stShareGroup.MAX_FAVORITE_GROUPS));
                return;
            }
            DlgAddShareGroup dlg = new DlgAddShareGroup();
            dlg.ShowDialog();
            if (dlg.getResultID() == C.ID_DLG_BUTTON_OK)
            {
                String group = dlg.getText();
                if (group != null)
                {
                    group = group.Trim();
                    if (group.Length == 0)
                        return;

                    stShareGroup g = mContext.getFavoriteGroup(group);
                    mContext.setCurrentShareGroup(g);
                    //mContext.saveFavorGroup();

                    //mContext.uploadUserData();
                    Context.userDataManager().flushUserData();

                    updateMyShareGroupList();
                }

                mMainHistoryChartControl.setupMenuContext();
            }
        }

        void doRemoveGroup(stShareGroup g)
        {
            if (g.isFavorGroup())
            {
                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                sb.AppendFormat("Xóa nhóm {0}", g.getName());
                if (showDialogYesNo(sb.ToString()))
                {
                    mContext.removeShare(g);
                    mContext.selectDefaultShareGroup();

                    Context.userDataManager().flushUserData();

                    updateMyShareGroupList();

                    mMainHistoryChartControl.setupMenuContext();
                }
            }
            else
            {
                showDialogOK("Bạn chỉ xóa được nhóm do bạn tự tạo");
            }
        }
    }
}
