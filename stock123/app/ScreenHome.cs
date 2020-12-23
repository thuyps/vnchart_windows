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
using stock123.app;
using System.Diagnostics;

using stock123.app.table;

using stock123.app.net;
using System.Runtime.InteropServices;
//using System.Data;

namespace stock123.app
{
    class ScreenHome: ScreenBase
    {
        //  screen state
        const int SCREENSTATE_HOME = 0;
        const int SCREENSTATE_CHANGES_STATISTICS = 1;
        int mScreenState = 0;
        //============================
        const int STATE_LOGIN_REQUIRED = -1;
        const int STATE_NORMAL = 0;
        const int STATE_PREPARING_LOGIN = 5;
        const int STATE_PREPARING_REGISTER = 6;
        const int STATE_LOGGING = 7;
        
        const int STATE_PREPARING_PRICEBOARD_ZERO = 8;
        const int STATE_GETTING_PRICEBOARD_ZERO = 9;
        
        //const int STATE_PREPARING_NEXTFILE = 6;
        //const int STATE_GETTING_NEXTFILE = 7;
        const int STATE_PREPARING_UPDATE_REALTIME = 10;
        const int STATE_UPDATING_REALTIME = 11;
        
        const int STATE_CHANGING_PASSWORD_PREPARING = 15;
        const int STATE_CHANGING_PASSWORD = 16;
        const int STATE_RESETING_PASSWORD_PREPARING = 17;
        const int STATE_RESETING_PASSWORD = 18;

        const int STATE_DOWNLOAD_NEW_VERSION_PREPARING = 30;
        const int STATE_DOWNLOAD_NEW_VERSION = 31;

        const int STATE_DOWNLOAD_ALL_SHARE_PREPARING = 35;
        const int STATE_DOWNLOAD_ALL_SHARE = 36;

        const int W_INDEX_BRIEF = 900;

        const int W_SHARE_GROUP = 150;//210;
        const int X_PRICEBOARD = (W_SHARE_GROUP+3);
        const int W_PRICEBOARD = (W_INDEX_BRIEF - X_PRICEBOARD);
        const int H_PREVIEW_CHART = 260;

        const int X_REALTIME_CHART = 1000;

        const int H_SHAREGROUP_FAVOR = 240;
        const int H_SHAREGROUP_DEFAULT = 290;
        const int H_SHAREGROUP_STATISTIC = 150;
        //---------------------------------------------------------------/
        const int VIEWTYPE_TABLE = 1;
        const int VIEWTYPE_CANDLE = 2;

        //---------------------------------------------------------------/

        int mNetState;
        bool mIsLaunchingActivate = true;
        xTimer mTimer;
        xTimer mTimerRequestOpen;
        xTimer mTimerGlobal;

        xContainer mLeftPanel;
        xContainer mPreviewChart;

        TablePriceboard mPriceboard;
        xBaseControl mPriceboadCandle;
        xListView mGlobalPriceboard;

        xContainer mRightPanel;
        int mRightPanelW = 0;
        bool mShouldDrawMACD = true;
        bool mRightPanelIsIndices;
        //  priceboard container
        int mPriceboardY;
        int mPriceboardH;
        xBaseControl mPriceboardContainer;

        DlgContactingServer mStartupDialog;

        bool mIsLoadIndicesDataAtStartup = false;

        TodayCandle mTodayCandle;
        QuotePoint mQuotePoint;
        RealtimeChart mRealtimeChart;
        ChartMoney mRealtimeMoneyChart;
        RealtimeTradeListDetail mRealtimeTradeList;

        xContainer mTimingRange;

        xVector mControlsShouldInvalideAfterNetDone = new xVector(10);
        xVector mCharts = new xVector(10);

        xContainer mSubContainer0;
        ChartMaster mMasterChart;

        SubchartsContainer mSubContainer1;

        //  current screen view
        xBaseControl mCurrentScreenView = null;

        //  main priceboard view
        xSplitter mSplitterMain = null;
        //  changes
        xTabControl mChangesView = null;

        ToolBarButton mAlarmButton;
        ToolBarButton mSwitchButton;

        int mAlarmAnimationIDX = 0;
        xTimer mAlarmAnimation = new xTimer(500);
        //---------------------------------------------------------------/
        bool mRequestResetPassword = false;
        bool mRequestChangePassword = false;
        string mRequestNewPassword;
        //---------------------------------------------------------------/
        bool mIsGettingServerAddress;

        NetProtocol mNetProtocol;
        int mRealtimeUpdateCnt;
        //---------------------------------------------------------------/

        bool mIsShowAlarmDialog = true;
        public ScreenHome(): base()
        {
        }

        bool doNotRecreateHomeScreen = false;
        public override void onDeactivate()
        {
            if (doNotRecreateHomeScreen)
            {
                return;
            }
            base.onDeactivate();
        }
        public override void onActivate()
        {
            if (doNotRecreateHomeScreen)
            {
                doNotRecreateHomeScreen = false;
                mNetState = STATE_NORMAL;
                return;
            }

            mNetProtocol = mContext.createNetProtocol();
            if (mNetProtocol == null){
                mIsGettingServerAddress = true;
                return;
            }
            mNetProtocol.setListener(this);

            mRealtimeUpdateCnt = 0;

            mTimer = new xTimer(3000);
            mTimerRequestOpen = new xTimer(3*60*1000);
            mTimerGlobal = new xTimer(2*60 * 1000);
            //-----------------------------------------
            Share currentShare = mContext.getSelectedShare();
            if (mContext.getSelectedShare() == null)
            {
                mContext.selectDefaultShare();

                mContext.selectDefaultShareGroup();
            }
            updateUI();

            setTitle("Home Screen");
            setStatusMsg("status bar");

            mNetProtocol.setListener(this);

            if (!mContext.isOnline())
            {
                if (mIsLaunchingActivate)
                {
                    mIsLaunchingActivate = false;
                    if (needDownloadAllShare())
                    {
                        mNetState = STATE_DOWNLOAD_ALL_SHARE_PREPARING;
                        showContactingServerDlg();
                    }
                    else{
                        if (mContext.mOptAutologin && mContext.isValidEmailPassword())
                        {
                            mNetState = STATE_PREPARING_LOGIN;
                        }
                        else{
                            mNetState = STATE_LOGIN_REQUIRED;
                            showLoginDialog("Login hệ thống");
                        }
                    }
                }
                return;
            }
            else
            {
                //if (mIsLaunchingActivate)   //  loggined by Miniscreen, now need to get priceboard
                {
                    mNetState = STATE_PREPARING_PRICEBOARD_ZERO;
                    mIsLaunchingActivate = false;
                }
                //mNetState = STATE_PREPARING_NEXTFILE;
            }
            if (currentShare != null)
                mContext.setCurrentShare(currentShare);
            else
                mContext.selectDefaultShare();

            if (mContext.getSelectedShare() != null)
            {
                if (mContext.getSelectedShare().isRealtime())
                {
                    mContext.setCurrentShare(mContext.getSelectedShare().mID);
                }
                if (mContext.getSelectedShare().isIndex())
                    mContext.getSelectedShare().loadShareFromFile(true);
                else
                    mContext.getSelectedShare().loadShareFromCommonData(true);
            }
        }

        public override void onSizeChanged()
        {
            base.onSizeChanged();

            if (getW() > 0 && getH() > 0)
                updateUI();
        }

        public void updateUI()
        {
            removeAllControls();
            mControlsShouldInvalideAfterNetDone.removeAllElements();
            mRealtimeTradeList = null;
            //---------------------------------
            //---------------------------------
            stShareGroup g = mContext.getCurrentShareGroup();
            int w, x;
            if (g != null)
            {
                setTitle("  -Nhóm: " + g.getName());
            }

            //-tool bar
            createToolbar();
            
            //  status bar
            addStatusBar();
//            return;
            //=======================================
            int workingH = getH() - getToolbarH() - getStatusbarH();
            w = getW();
            if (w < 1300)
                w = 1300;

            //  working area
            xContainer c = new xContainer();
            mLeftPanel = c;
            c.setSize(W_INDEX_BRIEF, workingH);

            int y0 = 0;
            //  add vnindex & hastc briefs - 2 lines on the top
            int indexIDX = 0;
            for (int i = 0; i < mContext.mPriceboard.getIndicesCount(); i++)
            {
                stPriceboardStateIndex pi = (stPriceboardStateIndex)mContext.mPriceboard.getPriceboardIndexAt(i);
                if ((pi.marketID == 1 || pi.marketID == 2 || pi.marketID == 3)&&pi.code != null && pi.code.Length > 0)
                if (pi.supported && pi.code != null && pi.code.Length > 0)
                {
                    IndexBriefLine inf = new IndexBriefLine(pi.marketID);
                    inf.setListener(this);
                    inf.setID(C.ID_MARKET_INDEX);
                    inf.setPosition(0, indexIDX * inf.getH());
                    inf.setSize(W_INDEX_BRIEF, inf.getH());
                    c.addControl(inf);
                    y0 = inf.getBottom();

                    indexIDX++;

                    mControlsShouldInvalideAfterNetDone.addElement(inf);
                }
            }
            mPriceboardY = y0;

            //  main priceboard
            createTableAndPreviewChart(g);
            //-------------------------------
            mRightPanel = new xContainer();
            int ww = getW();
            //if (mRightPanelW == 0)
                mRightPanelW = getW() - W_INDEX_BRIEF - 6;
            mRightPanel.setSize(mRightPanelW, c.getH());

            addRealtimeIndicesControls();
            //--------------------------------
            //======share group=====
            int y = mPriceboardContainer.getY();
            //-----------------------------
            xBaseControl favors = createShareGroupList(false, C.ID_DROPDOWN_FAVOR_GROUP, "Nhóm yêu thích", mContext.mFavorGroups, W_SHARE_GROUP, H_SHAREGROUP_FAVOR);
            favors.setPosition(0, y);
            y += H_SHAREGROUP_FAVOR;

            xBaseControl statistics = createShareGroupList(false, C.ID_GROUP_SPECIAL, "Đột biến", mContext.mShareGroupsSpecial, W_SHARE_GROUP, H_SHAREGROUP_STATISTIC);
            statistics.setPosition(0, y);

            y += H_SHAREGROUP_STATISTIC;
            int tmp = c.getH() - y;
            xBaseControl defaults = createShareGroupList(false, C.ID_DROPDOWN_COMMON_GROUP, "Nhóm mặc định", mContext.mShareGroups, W_SHARE_GROUP, tmp);
            defaults.setPosition(0, y);

            c.addControl(favors);
            c.addControl(defaults);
            c.addControl(statistics);
            //=========================
            mLeftPanel.setSize(mPriceboardContainer.getW(), workingH);
            mLeftPanel.setPosition(0, 0);

            int distanceW = W_INDEX_BRIEF;

            mSplitterMain = xSplitter.createSplitter(false, getW(), workingH, distanceW, 200, 100);//getW()-W_PRICEBOARD);
            mSplitterMain.setPanels(mLeftPanel, mRightPanel);
            mSplitterMain.setPosition(0, getToolbarH() + 2);
            mSplitterMain.setListener(this);
            //this.addControl(mSplitterMain);

            int evt = C.ID_GOTO_HOME_SCREEN;
            if (mScreenState == SCREENSTATE_HOME)
            {
                evt = C.ID_GOTO_HOME_SCREEN;
            }
            else if (mScreenState == SCREENSTATE_CHANGES_STATISTICS)
            {
                evt = C.ID_CHANGES_STATISTICS_VIEW;
            }

            mScreenState = -1;
            onToolbarEvent(evt);
        }

        void createTableAndPreviewChart(stShareGroup g)
        {
            int previewH = H_PREVIEW_CHART;
            if (!mContext.mHasPreviewChart)
                previewH = 0;

            int h = (mLeftPanel.getH() - mPriceboardY) - previewH;// / 2;
            if (h < 250)
                h = 250;
            //  add priceboard
            mPriceboardH = h;

            //if (g != null)
            {
                if (g != null && g.getGroupType() == stShareGroup.ID_GROUP_GAINLOSS)
                    showGainlossTable();
                else
                    recreatePriceboard();
            }
            if (mPreviewChart != null)
            {
                mLeftPanel.removeControl(mPreviewChart);
            }
            //-----------------------------
            if (mContext.mHasPreviewChart)
            {
                xContainer charts = new xContainer();

                int y = mPriceboardContainer.getBottom() + 4;
                charts.setSize(W_PRICEBOARD, H_PREVIEW_CHART);
                charts.setPosition(X_PRICEBOARD, y);
                mLeftPanel.addControl(charts);
                createCharts(charts);

                mPreviewChart = charts;
            }
        }

        void createCharts(xContainer c)
        {
            xDataInput di = loadOptions();
            if (di != null)
            {
                if (di.readInt() != Context.FILE_VERSION_HOME)
                    di = null;
            }
            mCharts.removeAllElements();
            //=========================
            ToolStrip ts = createToolStripForMainChart();
            c.addControl(ts);
            int chartW = c.getW() - ts.Size.Width;
            //-------------------------------
            int h0 = c.getH()*3/4;
            int w = c.getW();

            xSplitter splitter = new xSplitter(true, chartW, c.getH(), h0, 40, 20);
            splitter.setPosition(ts.Size.Width, 0);
            splitter.setListener(this);
            c.addControl(splitter);

            mSubContainer0 = new xContainer();
            mSubContainer0.setPosition(0, 0);
            //==============timing range
            if (mTimingRange == null)
                mTimingRange = new xContainer();
            createChartRangeControls(0, mTimingRange);
            mSubContainer0.addControl(mTimingRange);

            //  master chart
            mMasterChart = createMasterChart(ChartLine.CHART_LINE, chartW, mSubContainer0.getH());
            mSubContainer0.addControl(mMasterChart);
            mCharts.addElement(mMasterChart);
            if (di != null)
            {
                mMasterChart.load(di);
            }
            else
            {
                //  default is bollinger
                mMasterChart.toggleAttachChart(ChartBase.CHART_BOLLINGER);
            }
            //---------------------------------------
            int subChart = ChartBase.CHART_VOLUME;
            if (di != null)
            {
                subChart = di.readInt();
                mRightPanelW = di.readInt();

                if (di.available() > 0)
                    mShouldDrawMACD = di.readBoolean();
                else
                    mShouldDrawMACD = true;
            }
            mSubContainer1 = new SubchartsContainer(C.ID_SUBCHART_CONTAINER_0, mContext.getSelectedShare(), this, false);
            mSubContainer1.setChart(subChart);

            mSubContainer1.hideRemoveButton();
            mCharts.addElement(mSubContainer1);
            //  finally add 2 subcontainers to the splitter

            splitter.setPanels(mSubContainer0, mSubContainer1);
        }

        void createToolbar()
        {
            addToolbar(mContext.getImageList(C.IMG_MAIN_ICONS, 30, 26));

            if (!mContext.isOnline())
                addToolbarButton(C.ID_LOGIN, 5, "Login");
            else
                addToolbarButton(C.ID_LOGOUT, 6, "Logout");
            addToolbarButton(C.ID_GOTO_HOME_SCREEN, 0, "Bảng giá");
            addToolbarButton(C.ID_CHANGES_STATISTICS_VIEW, 15, "Thay đổi");
            addToolbarButton(C.ID_GOTO_SEARCH_SCREEN, 3, "Lọc cổ phiếu");

            addToolbarButton(C.ID_GOTO_SETTING, 2, "Cấu hình");
            addToolbarSeparator();
            mAlarmButton = addToolbarButton(C.ID_ALARM_MANAGER, 10, "Alarm");
            addToolbarSeparator();
            //addToolbarButton(C.ID_GOTO_MINI_SCREEN, 4, "Minimize");
            addToolbarSeparator();
            addToolbarButton(C.ID_PREVIEW_HISTORY_CHART, 13, "Đồ thị lsử");
            mSwitchButton = addToolbarButton(C.ID_SWITCH_VIEW, 12, "-Candle-");
            addToolbarSeparator();
            addToolbarButton(C.ID_GOTO_HELP, 1, "About");
            addToolbarSeparator();
            addToolbarButton(C.ID_REFRESH_DATA, 14, "Refresh");
            //  favorite group
            int x = getToolbarButtonsRight() + 20;
            //return;
            ToolBar tb = getToolbar();

            int sloganW = getW() - x - 30;
            //  slogan
            mSloganContainer = new xContainer();
            mSloganContainer.setSize(sloganW, tb.Size.Height - 8);
            mSloganContainer.setPosition(x + 20, 2);
            recreateSlogan();
            tb.Controls.Add(mSloganContainer.getControl());
        }

        int getClientH()
        {
            return getH() - getToolbarH() - getStatusbarH();
        }

        void traceState()
        {
            String s = "";
            if (mNetState == STATE_LOGIN_REQUIRED) s = "STATE_LOGIN_REQUIRED";
            else if (mNetState == STATE_NORMAL) s = "STATE_NORMAL";
            else if (mNetState == STATE_PREPARING_LOGIN) s = "STATE_PREPARING_LOGIN";
            else if (mNetState == STATE_PREPARING_REGISTER) s = "STATE_PREPARING_REGISTER";
            else if (mNetState == STATE_LOGGING) s = "STATE_LOGGING";

            else if (mNetState == STATE_PREPARING_PRICEBOARD_ZERO) s = "STATE_PREPARING_PRICEBOARD_ZERO";
            else if (mNetState == STATE_GETTING_PRICEBOARD_ZERO) s = "STATE_GETTING_PRICEBOARD_ZERO";

            //const int STATE_PREPARING_NEXTFILE = 6;
            //const int STATE_GETTING_NEXTFILE = 7;
            else if (mNetState == STATE_PREPARING_UPDATE_REALTIME) s = "STATE_PREPARING_UPDATE_REALTIME";
            else if (mNetState == STATE_UPDATING_REALTIME) s = "STATE_UPDATING_REALTIME";

            else if (mNetState == STATE_CHANGING_PASSWORD_PREPARING) s = "STATE_CHANGING_PASSWORD_PREPARING";
            else if (mNetState == STATE_CHANGING_PASSWORD) s = "STATE_CHANGING_PASSWORD";
            else if (mNetState == STATE_RESETING_PASSWORD_PREPARING) s = "STATE_RESETING_PASSWORD_PREPARING";
            else if (mNetState == STATE_RESETING_PASSWORD) s = "STATE_RESETING_PASSWORD";

            else if (mNetState == STATE_DOWNLOAD_NEW_VERSION_PREPARING) s = "STATE_DOWNLOAD_NEW_VERSION_PREPARING";
            else if (mNetState == STATE_DOWNLOAD_NEW_VERSION) s = "STATE_DOWNLOAD_NEW_VERSION"; 
        }

        override public void onTick()
        {
            if (mNetProtocol == null)
            {
                if (mStartupDialog == null)
                {
                    showContactingServerDlg();
                }

                if (mIsGettingServerAddress)
                {
                    mNetProtocol = mContext.createNetProtocol();

                    if (mNetProtocol != null)
                    {
                        mIsGettingServerAddress = false;
                        mStartupDialog.Close();
                        mStartupDialog = null;

                        onActivate();
                    }
                }
                return;
            }
            
            if (mNetState == STATE_DOWNLOAD_NEW_VERSION_PREPARING)
            {
                xHttp http = new xHttp(this);
                http.get(mContext.mLatestClientVersionURL, null);
                mNetState = STATE_DOWNLOAD_NEW_VERSION;
            }
            else if (mNetState == STATE_DOWNLOAD_ALL_SHARE_PREPARING)
            {
                xHttp http = new xHttp(this);
                http.get(mContext.configJson.url_all_share2, null);
                mNetState = STATE_DOWNLOAD_ALL_SHARE;
            }

            if (mNetState == STATE_DOWNLOAD_NEW_VERSION || mNetState == STATE_DOWNLOAD_ALL_SHARE)
            {
                return;
            }

            traceState();
            //==========================================
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

            if (mNetProtocol != null)
                mNetProtocol.onTick();

            if (mContext.mShouldReloadAllData)
            {
                mContext.logout();
                mContext.clearAllSavedData();
                mContext.mShouldReloadAllData = false;
                AppConfig.appConfig.allShareUpdateDate = 0;

                if (needDownloadAllShare())
                {
                    mNetState = STATE_DOWNLOAD_ALL_SHARE_PREPARING;
                }
                else
                {
                    mNetState = STATE_PREPARING_LOGIN;
                }
            }
            if (hasServerNotification())
                return;

            //================
            if (!mIsShowAlarmDialog && mContext.mAlarmManager.hasTriggerredAlarm())
            {
                mIsShowAlarmDialog = true;

                xMainApplication.getxMainApplication().postMessageInUIThread(this, this, xBaseControl.EVT_BUTTON_CLICKED, C.ID_ALARM_MANAGER, null);
            }
            //================
            if (mContext.mIsFavorGroupChanged)
            {
                mTimer.expireTimer();
            }

            if (mNetState == STATE_NORMAL)
            {
                if (mContext.isOnline())
                {
                    if (mTimer.isExpired())
                    {
                        if (mRequestChangePassword)
                        {
                            mRequestChangePassword = false;
                            mNetState = STATE_CHANGING_PASSWORD_PREPARING;
                        }
                        else if (mRequestResetPassword)
                        {
                            mRequestResetPassword = false;
                            mNetState = STATE_RESETING_PASSWORD_PREPARING;
                        }
                        else
                        {
                            mNetState = STATE_PREPARING_UPDATE_REALTIME;
                            mRealtimeUpdateCnt++;
                            if (mRealtimeUpdateCnt >= 9)
                            {
                                mRealtimeUpdateCnt = 0;
                                mNetState = STATE_PREPARING_PRICEBOARD_ZERO;
                            }
                            if (mContext.mPriceboard.isMarketClosed())
                            {
                                //mNetState = STATE_PREPARING_PRICEBOARD_ZERO;
                                mTimer.setExpiration(30 * 1000);
                            }
                            else
                            {
                                mTimer.setExpiration(30 * 1000);
                            }
                        }
                        mTimer.reset();
                    }
                }
            }

           

            //  login
            if (mNetState == STATE_PREPARING_LOGIN)
            {
                login(0);
            }
            if (mNetState == STATE_PREPARING_REGISTER)
            {
                login(1);
            }
            if (mNetState == STATE_CHANGING_PASSWORD_PREPARING)
            {
                mNetProtocol.resetRequest();
                mNetProtocol.requestSetPassword(mRequestNewPassword);
                mNetProtocol.flushRequest();
                mNetState = STATE_CHANGING_PASSWORD;
            }
            if (mNetState == STATE_RESETING_PASSWORD_PREPARING)
            {
                mNetProtocol.resetRequest();
                mNetProtocol.requestResetPassword(mContext.mEmail);
                mNetProtocol.flushRequest();
                mNetState = STATE_RESETING_PASSWORD;

                showContactingServerDlg();
            }
            //  get priceboard reference + zero
            if (mNetState == STATE_PREPARING_PRICEBOARD_ZERO)
            {
                if (!mTimer.isExpired())
                {
                    return;
                }
                mNetProtocol.resetRequest();

                //  indices
                if (!mIsLoadIndicesDataAtStartup)
                {
                    for (int i = 0; i < mContext.mShareManager.getVnindexCnt(); i++)
                    {
                        Share share = mContext.mShareManager.getVnindexShareAt(i);
                        if (share == null)
                            break;
                        share.loadShareFromFile(false);
                        int date = share.getLastCandleDate();
                        if (date == 0)
                        {
                            date = Utils.getDateAsInt(5000);
                        }
                        else
                        {
                            long l = Utils.dateToNumber(date);
                            date = Utils.dateFromNumber(l - 10);
                        }
                        mNetProtocol.requestGet1ShareData(share.mID, date);
                    }
                    //  loadShareFromFile makes mClose,mOpen... incorrect, fix it
                    if (mContext.getSelectedShare() != null)
                    {
                        mContext.getSelectedShare().clearCalculations();
                        if (mContext.getSelectedShare().isIndex())
                            mContext.getSelectedShare().loadShareFromFile(false);
                        else
                            mContext.getSelectedShare().loadShareFromCommonData(false);
                    }
                    //=========================
                }
                //mNetProtocol.requestPriceboardRef(-1);
                mNetProtocol.requestPriceboardInitial(-1, null);

                mNetProtocol.requestOpens();

                //  json indices
                VTDictionary dict = new VTDictionary();
                dict.setValueInt(JSONHandler.kMessageID, JSONHandler.JMSG_LIST_INDICES);
                mNetProtocol.requestJSONMessage(dict.toJson());


                mNetProtocol.flushRequest();

                mNetState = STATE_GETTING_PRICEBOARD_ZERO;
            }
            //  realtime update
            if (mNetState == STATE_PREPARING_UPDATE_REALTIME)
            {
                mNetProtocol.resetRequest();
                //  check if user changed his favorite group
                if (mContext.mIsFavorGroupChanged)
                {
                    xDataOutput o = mContext.getUserDataAsStream();
                    mNetProtocol.requestSaveUserData(o);
                    o = null;
                }
                mContext.mIsFavorGroupChanged = false;
                //  intraday history
                if (mRealtimeChart != null)
                {
                    TradeHistory tradehistory = mRealtimeChart.getTradeHistory();
                    if (tradehistory != null)
                        mNetProtocol.requestTradeHistory(tradehistory.mCode, tradehistory.getFloorID(), 0, tradehistory.getLastTime());
                }
                //  vnindex & hastc
                for (int t = 0; t < mContext.mPriceboard.getIndicesCount(); t++)
                {
                    stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(t);

                    if (pi.id != 0)
                    {
                        TradeHistory trade = mContext.getTradeHistory(pi.id);
                        mNetProtocol.requestTradeHistory(pi.code, pi.marketID, 0, trade.getLastTime());

                        //  online index
                        //mNetProtocol.requestOnlineIndex(pi.marketID);
                    }
                }

                //  current priceboard
                stShareGroup currentGroup = mContext.getCurrentShareGroup();
                xVector v = new xVector();

                if (currentGroup != null)
                {
                    for (int i = 0; i < currentGroup.getTotal(); i++)
                    {
                        String code = currentGroup.getCodeAt(i);
                        int id = mContext.mShareManager.getShareID(code);
                        v.addElement((Int32)id);
                    }
                }
                if (v.size() > 0)
                {
                    mNetProtocol.requestGetPriceboard(v);
                }
                //======opens=========
                if (mTimerRequestOpen.isExpired())
                {
                    Utils.trace("--------timer is expired-----------");
                    mTimerRequestOpen.reset();

                    mNetProtocol.requestOpens();
                }
                //=========================
                mNetProtocol.flushRequest();

                mNetState = STATE_UPDATING_REALTIME;
            }
            /*
            if (mNetState == STATE_NORMAL && mTimer.isExpired())
            {
                mTimer.reset();
                if (mContext.isOnline())
                {

                }
            }
             * */
        }

        override public void onNetworkCompleted(bool success)
        {
            if (mStartupDialog != null)
            {
                mStartupDialog.Close();
                mStartupDialog = null;
            }
            //SubScreenBase::onNetworkCompleted(data, len);
            mTimer.reset();
/*
            if (mDialog && mDialog.getID() == ID_DIALOG_NETWORK_CONTACTING)
            {
                closeDialog();
            }
*/
            //---------------------------    
            if (mNetState == STATE_CHANGING_PASSWORD)
            {
                mNetState = STATE_NORMAL;
            }
            if (mNetState == STATE_RESETING_PASSWORD)
            {
                mNetState = STATE_LOGIN_REQUIRED;
                //  show msg
                hasServerNotification();

                showLoginDialog("Login hệ thống");
            }
            if (mNetState == STATE_GETTING_PRICEBOARD_ZERO)
            {
                if (success)
                    mIsLoadIndicesDataAtStartup = true;

                if (mContext.mEmail.CompareTo("thuyps@gmail.com") == 0 && mContext.mPassword.CompareTo("1") == 0)
                {
                    //  clear default user
                    mContext.mEmail = "";
                    mContext.mPassword = "";
                    mContext.saveProfile();
                    mContext.logout();
                    updateUI();

                    mNetState = STATE_LOGIN_REQUIRED;
                    showLoginDialog("Login hệ thống");
                }

                //  check alarm after getting priceboard zero
                mIsShowAlarmDialog = false;
            }
            //---------------------------
            if (mNetState == STATE_LOGGING)
            {
                if (mContext.isOnline())
                {
                    mContext.saveProfile();

                    updateUI();

                    /*
                    //-----------------------------
                    if (mSubScreen)
                        mSubScreen.recreateControls();
                    */
                    mNetState = STATE_PREPARING_PRICEBOARD_ZERO;
                    mTimer.expireTimer();
                    setStatusMsg("LOGIN success. Starting priceboard ZERO");

                    bool showContactingDlg = true;

                    if (mContext.mLatestClientVersion > C.CLIENT_VERSION)
                    {
                        if (true)//mContext.noupdate)
                        {
                            showContactingDlg = false;
                            showNewVersionDialog();
                        }
                        else if (true)//showDialogYesNo("Đã có phiên bản vnChart mới, bạn có muốn tải về không?"))
                        {
                            //showWeb(mContext.mLatestClientVersionURL);
                            mNetState = STATE_DOWNLOAD_NEW_VERSION_PREPARING;
                        }
                        //mContext.mLatestClientVersion = -1;
                    }

                    if (mContext.mIsFirstTime)
                    {
                        mContext.mIsShowWhatNew = true;
                        //showHelp(0);
                        mContext.saveProfile();
                    }
                    else if (!mContext.mIsShowWhatNew && mContext.mEmail.CompareTo("thuyps@gmail.com") != 0)
                    {
                        //mContext.mIsShowWhatNew = true;
                        mContext.saveProfile();
                        //showHelp(-1);

                        showContactingDlg = false;
                    }

                    if (showContactingDlg)
                    {
                        showContactingServerDlg();
                    }
                }
                else
                {
                    string err = "Lỗi mạng";
                    string status = err;
                    if (mNetProtocol.getLastError() != null && mNetProtocol.getLastError().Length > 0)
                    {
                        err = mNetProtocol.getLastError();
                    }
                    else if (hasServerNotification())
                    {
                        err = null;
                        status = null;
                    }

                    if (err != null)
                        showDialogOK(err);
                    if (status != null)
                        setStatusMsg(status);
                    //mNetState = STATE_NORMAL;
                    mNetState = STATE_LOGIN_REQUIRED;

                    showLoginDialog(err);
                }
            }
            if (mNetState == STATE_GETTING_PRICEBOARD_ZERO)
            {
                mContext.mPriceboard.savePriceboard();

                if (mContext.mPriceboard.hasNextFile(1))
                {
                    mNetState = STATE_PREPARING_UPDATE_REALTIME;
                    setStatusMsg("priceboard ZERO success. Starting realtime update");
                }
                else
                {
                    setStatusMsg("failed to get priceboard-zero");
                    mNetState = STATE_NORMAL;
                }
                updateItemsAfterNetDone();
            }
            if (mNetState == STATE_UPDATING_REALTIME)
            {
                mNetState = STATE_NORMAL;
                mTimer.reset();
                //  refresh: priceboard, 2 indices, realtime charts
                mPriceboard.invalidate();
                updateItemsAfterNetDone();

                if (mContext.mLatestClientVersion > Context.VERSION && !mContext.mLastestClientVersionAsk)
                {
                    showNewVersionDialog();
                }
                /*
                if (mContext.mPriceboardNextfileError > 4)
                {
                    mContext.mPriceboardNextfileError = 0;
                    mNetState = STATE_PREPARING_PRICEBOARD_ZERO;
                    setStatusMsg("restarting from ZERO priceboard");
                }

                updateItemsAfterNetDone();
                */
            }
/*
            if (mSubScreen)
                mSubScreen.invalidate();
*/
            /*
             mUITable.updateTable();
             //mTable.updateTable();
             if (mQueryCnt++ < 6)
             mTimer.expireTimer();
     
             */
            Utils.trace("==========Network DONE.");
        }

        void killvnChart()
        {
            try
            {
                foreach (Process clsProcess in Process.GetProcesses())
                {
                    if (clsProcess.ProcessName.StartsWith("vnChart"))
                    {
                        clsProcess.Kill();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        void downloadEvent(object sender, int aEvent, int aIntParameter, object aParameter)
        {
            if (aEvent == xBaseControl.EVT_NET_DONE)
            {
                int len = aIntParameter;
                byte[] p = (byte[])aParameter;

                if (len > 0)
                {
                    if (mNetState == STATE_DOWNLOAD_NEW_VERSION)
                    {
                        xFileManager.removeFile("setup.exe");
                        xFileManager.saveFile(p, 0, len, "setup.exe");
                        String file = xFileManager.UserDir + "setup.exe";

                        //xMainApplication.getxMainApplication().exitApplication();
                        System.Diagnostics.Process.Start(file);

                        killvnChart();
                    }
                    else if (mNetState == STATE_DOWNLOAD_ALL_SHARE)
                    {
                        decompressAllShare(p, 0, len);

                        mStartupDialog.Close();
                        mStartupDialog = null;

                        mNetState = STATE_PREPARING_LOGIN;
                    }
                }
            }
            else if (aEvent == xBaseControl.EVT_NET_ERROR)
            {
                if (mNetState == STATE_DOWNLOAD_NEW_VERSION)
                {
                    showDialogOK("Could not download setup file!");
                }
                else
                {
                    mNetState = STATE_PREPARING_LOGIN;
                }

                mNetState = STATE_PREPARING_PRICEBOARD_ZERO;
                mTimer.expireTimer();
            }
            else if (aEvent == xBaseControl.EVT_NET_DATA_AVAILABLE)
            {
                if (mStartupDialog != null)
                {
                    mStartupDialog.setMsg2((aIntParameter / 1024) + " KB");
                }
            }
        }

        public override void onEvent(object sender, int evt, int aIntParameter, object aParameter)
        {
            if (mNetState == STATE_DOWNLOAD_NEW_VERSION || mNetState == STATE_DOWNLOAD_ALL_SHARE)
            {
                downloadEvent(sender, evt, aIntParameter, aParameter);
                return;
            }
            //=======================================

            base.onEvent(sender, evt, aIntParameter, aParameter);
            if (evt == xBaseControl.EVT_NET_DATA_AVAILABLE)
            {
                if (mStartupDialog != null)
                {
                    mStartupDialog.setMsg2((aIntParameter / 1024) + " KB");
                }
            }
            //======================================

            if (evt == C.EVT_SUB_CHART_CONTAINER_CHANGED)
            {
                saveOptions();
            }
            if (evt == xBaseControl.EVT_ON_SPLITTER_SIZE_CHANGED)
            {
                if (sender == mSplitterMain)
                {
                    onMainSplitterSizeChanged();
                }
                else
                {
                    mMasterChart.setSize(mSubContainer0);
                    mTimingRange.setPosition((mSubContainer0.getW() - mTimingRange.getW()) / 2, mSubContainer0.getH() - mTimingRange.getH() - 16);
                }
                invalidateCharts();
            }
            if (evt == C.EVT_REPAINT_CHARTS)
            {
                invalidateCharts();
            }

            if (evt == C.EVT_SHOW_TUTORIAL)
            {
                showHelp(aIntParameter);
            }

            if (mNetState == STATE_LOGGING)
                return;
            //====================================================

            Share share = mContext.getSelectedShare();
            if (evt == xBaseControl.EVT_ON_CONTEXT_MENU)
            {
                onContextMenu(aIntParameter);
            }
            //===========================
            if (evt == xBaseControl.EVT_ON_MOUSE_DOUBLE_CLICK)
            {
                if (aIntParameter == C.ID_SELECT_SHARE_CANDLE)
                {
                    share = (Share)aParameter;
                    if (share != null)
                    {
                        //selectShare(share);

                        goChartScreen(share.getShareID());//share.mID);
                        mNetState = STATE_NORMAL;
                    }
                }
            }
            //===========================

            if (evt == xBaseControl.EVT_BUTTON_CLICKED)
            {
                //if (aIntParameter == C.ID_GOTO_SEARCH_SCREEN)
                //{
                    //goNextScreen(MainApplication.SCREEN_SEARCH);
                //}
                if (aIntParameter == C.ID_SELECT_SHARE_CANDLE)
                {
                    share = (Share)aParameter;
                    if (share != null)
                    {
                        selectShare(share.mID);
                    }
                        /*
                    else
                    {
                        mContext.selectDefaultShare();
                        addRealtimeIndicesControls();
                        stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(mContext.getSelectedShare().getMarketID());
                        if (pi != null)
                        {
                            mContext.setCurrentShare(pi.id);
                            if (mContext.getSelectedShare() != null)
                            {
                                mContext.getSelectedShare().loadShareFromFile(true);
                            }
                            invalidateCharts();
                        }
                    }
                         */
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
                //===========================================
                if (aIntParameter == C.ID_ADD_GROUP ||
                    aIntParameter == C.ID_REMOVE_GROUP)
                {
                    onContextMenu(aIntParameter);
                }
                if (aIntParameter == C.ID_BUTTON_EDIT_SLOGAN)
                {
//                    xMainApplication.getxMainApplication().exitApplication();
//                    System.Diagnostics.Process.Start("E:\\projects\\soft123\\projects\\soft123\\client\\cs_stock123\\stock123\\stock123\\setup\\release\\vnChart_install.exe");

                    DlgEditSlogan dlg = new DlgEditSlogan();
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        recreateSlogan();
                    }
                }
                if (aIntParameter == C.ID_HISTORY_CHART)
                {
                    int marketID = (Int32)aParameter;
                    stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(marketID);
                    if (pi != null)
                        goChartScreen(pi.id);
                }
                if (aIntParameter == C.ID_MARKET_INDEX)
                {
                    addRealtimeIndicesControls();
                    IndexBriefLine idx = (IndexBriefLine)sender;

                    stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(idx.getMarketID());
                    if (pi != null)
                    {
                        mContext.setCurrentShare(pi.id);
                        if (mContext.getSelectedShare() != null)
                        {
                            mContext.getSelectedShare().loadShareFromFile(true);
                        }
                        invalidateCharts();
                    }

                    //  show indices group
                    if (Context.getInstance().vnIndicesGroup != null)
                    {
                        Context.getInstance().setCurrentShareGroup(Context.getInstance().vnIndicesGroup);
                        recreatePriceboard();
                    }
                }
                //if (aIntParameter == Constants.ID_BUTTON_INDICES_DETAIL)
                //{
                    //addRealtimeIndicesControls();
                //}
                if (aIntParameter >= C.ID_CHART_RANGE && aIntParameter < C.ID_CHART_RANGE_END)
                {
                    if (share != null)
                    {
                        int idx = aIntParameter - C.ID_CHART_RANGE;
                        int[] scopes = { Share.SCOPE_1WEEKS, Share.SCOPE_1MONTH, Share.SCOPE_3MONTHS, Share.SCOPE_6MONTHS, Share.SCOPE_1YEAR, Share.SCOPE_2YEAR, Share.SCOPE_ALL};
                        if (idx >= 0 && idx < scopes.Length)
                        {
                            share.setCursorScope(scopes[idx]);

                            createChartRangeControls(idx, mTimingRange);

                            invalidateCharts();
                        }
                    }
                }
            }
//            if (evt == xBaseControl.EVT_ON_CONTEXT_MENU)
//            {
//                onContextMenu(sender, aIntParameter);
//            }
            if (evt == xBaseControl.EVT_ON_ROW_DOUBLE_CLICK)
            {
                if (aIntParameter == C.ID_MARKET_INDEX)
                {
                    IndexBriefLine idx = (IndexBriefLine)sender;

                    stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(idx.getMarketID());
                    if (pi != null)
                    {
                        goChartScreen(pi.id);
                    }
                }
                if (aIntParameter == C.ID_GAINLOSS_TABLE)
                {
                    RowPriceboardGainLoss r = (RowPriceboardGainLoss)aParameter;
                    stGainloss a = (stGainloss)r.getData();

                    int shareId = mContext.mShareManager.getShareID(a.code);
                    if (shareId > 0)
                    {
                        goChartScreen(shareId);
                    }
                }
                if (aIntParameter == C.ID_PRICEBOARD_TABLE)
                {
                    
                    {
                        RowPriceboard r = (RowPriceboard)aParameter;
                        int shareId = r.mShareID;
                        if (shareId > 0)
                        {
                            goChartScreen(shareId);
                        }
                    }
                }
            }
            if (evt == xBaseControl.EVT_ON_ROW_SELECTED)
            {
                mTimer.expireTimer();
                if (aIntParameter == C.ID_DROPDOWN_FAVOR_GROUP
                    || aIntParameter == C.ID_DROPDOWN_COMMON_GROUP
                    || aIntParameter == C.ID_GROUP_SPECIAL)
                {
                    xListViewItem item = (xListViewItem)aParameter;

                    onShareGroupSelected((stShareGroup)item.getData());
                }
                if (aIntParameter == C.ID_GAINLOSS_TABLE)
                {                    
                    RowPriceboardGainLoss r = (RowPriceboardGainLoss)aParameter;
                    stGainloss a = (stGainloss)r.getData();

                    int shareID = mContext.mShareManager.getShareID(a.code);
                    selectShare(shareID);
                }
                if (aIntParameter == C.ID_PRICEBOARD_TABLE)
                {
                    RowPriceboard r = (RowPriceboard)aParameter;
                    int shareId = r.mShareID;
                    selectShare(shareId);
                }
            }
        }

        public void login(int createAcc)
        {
            if (!mContext.isValidEmailPassword()){
                mNetState = STATE_NORMAL;
                return;
            }
            mContext.mTradeHistoryManager.removeAllElements();

            //-------------------login--------------------
            mNetProtocol.resetRequest();

            if (createAcc == 1)
            {
                mNetProtocol.requestLogin(mContext.mEmail, mContext.mPassword, createAcc);
            }
            else
            {
                //mNetProtocol.requestLoginNew(mContext.mEmail, mContext.mPassword, createAcc);
                mNetProtocol.requestLoginNew(mContext.mEmail, mContext.mPassword, createAcc);
            }
            mNetProtocol.requestGetShareIDs();
            mNetProtocol.requestIndicesIDs();

            //if (mContext.getShareGroupCount() == 0)
                mNetProtocol.requestShareGroup();

            if (Utils.getDateAsInt() - mContext.mCompanyUpdateTime >= 2
                || mContext.mShareManager.getCompanyInfos().size() == 0)	//	2days
            {
                mNetProtocol.requestGetCompanyInfo();
            }
            
            mNetProtocol.requestGetUserData();

            updateLatestData();
            //================================================
            mNetProtocol.flushRequest();
            mNetState = STATE_LOGGING;

            //  show dialog
            showContactingServerDlg();
        }

        void showContactingServerDlg()
        {
            if (mStartupDialog != null)
            {
                return;
            }
            //  show dialog
            mStartupDialog = new DlgContactingServer();
            mStartupDialog.Show();

            setStatusMsg(C.S_CONTACTING_SERVER);
        }

        public void updateLatestData()
        {
            int date = 0;

            //date = Utils.getDateAsInt(10);
            //=============================    
            if (mContext.mLastDayOfShareUpdate > 0)
            {
                long l = Utils.dateToNumber(mContext.mLastDayOfShareUpdate);
                l -= 6;
                date = Utils.dateFromNumber(l);
            }
            mNetProtocol.requestGetAllShares2(date);

            int devidedDate = mContext.mLastDayOfShareUpdate;
            if (devidedDate == 0)
            {
                devidedDate = Utils.getDateAsInt(500);
            }

        }

        void reloadAllData()
        {
            mNetProtocol.cancelNetwork();
            mContext.logout();

            mContext.clearAllSavedData();
            mContext.deleteSavedServerUrls();

            AppConfig.deleteConfig();

            Environment.Exit(0);

            //killvnChart();
            /*

            Application.Restart();
            Environment.Exit(0);
             * */
            /*
            //=============mContext.init();
            mNetProtocol.setListener(this);

            //=============updateUI();

            mNetState = STATE_PREPARING_LOGIN;
             */
        }

        void showNewVersionDialog()
        {
            if (mContext.mLastestClientVersionAsk == false)
            {
                mContext.mLastestClientVersionAsk = true;
                NewVersionDialog dialog = new NewVersionDialog();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Process.Start(mContext.mLatestClientVersionURL);
                }
            }
        }

        void showLoginDialog(string title)
        {
            DlgLogin dlg = new DlgLogin(title, mContext.mEmail, mContext.mPassword);
            dlg.ShowDialog();
            if (dlg.mDlgResult == DlgLogin.DLG_LOGIN)
            {
                string email = dlg.getEmail();
                string pass = dlg.getPassword();

                mContext.mEmail = email;
                mContext.mPassword = pass;

                mNetState = STATE_PREPARING_LOGIN;
            }
            else if (dlg.mDlgResult == DlgLogin.DLG_OFFLINE)
            {
            }
            else if (dlg.mDlgResult == DlgLogin.DLG_REGISTER)
            {
                DlgRegister reg = new DlgRegister("", "");
                reg.ShowDialog();
                if (reg.mDlgResult == DlgRegister.DLG_REGISTER)
                {
                    string email = reg.getEmail();
                    string pass = reg.getPassword();

                    mContext.mEmail = email;
                    mContext.mPassword = pass;
                    mNetState = STATE_PREPARING_REGISTER;
                }
                else
                {
                    showLoginDialog(title);
                }
            }
            else if (dlg.mDlgResult == DlgLogin.DLG_FORGOT_PASSWORD)
            {
                string email = dlg.getEmail();
                if (Utils.isValidEmail(email))
                {
                    mContext.mEmail = email;
                    mRequestResetPassword = false;
                    mNetState = STATE_RESETING_PASSWORD_PREPARING;
                }
                else
                {
                    showDialogOK("Bạn chưa nhập email!");
                    showLoginDialog(title);
                }
            }
        }

        public override void onToolbarEvent(int buttonID)
        {
            if (mNetState == STATE_LOGGING && mScreenState != -1)
                return;

            if (buttonID == C.ID_PREVIEW_HISTORY_CHART)
            {
                mContext.mHasPreviewChart = !mContext.mHasPreviewChart;
                mContext.saveOptions2();
                createTableAndPreviewChart(mContext.getCurrentShareGroup());
                return;
            }
            else if (buttonID == C.ID_CHANGES_STATISTICS_VIEW)
            {
                if (mScreenState != SCREENSTATE_CHANGES_STATISTICS)
                {
                    if (mCurrentScreenView != null)
                    {
                        this.removeControl(mCurrentScreenView);
                    }

                    if (mChangesView == null)
                    {
                        mChangesView = new xTabControl();
                        
                        xTabPage page1 = new xTabPage("Thay đổi chỉ số");
                        xContainer view0 = createThaydoiMainIndices(mSplitterMain.getW(), mSplitterMain.getH() - 40);
                        //view0.delegateShowHistoryChartOfGroup += showGroupHistory;
                        //view0.delegateShowHistoryChartOfShare += onClickShare;
                        page1.addControl(view0);

                        xTabPage page2 = new xTabPage("Thay đổi ngành");
                        ChartNhomnganhTangtruong view = createThaydoiNhomnganh(mSplitterMain.getW(), mSplitterMain.getH() - 40);
                        view.delegateShowHistoryChartOfGroup += showGroupHistory;
                        view.delegateShowHistoryChartOfShare += onClickShare;
                        page2.addControl(view);

                        mChangesView.addPage(page1);
                        mChangesView.addPage(page2);
                    }
                    mChangesView.setSize(mSplitterMain.getW(), mSplitterMain.getH());
                    mChangesView.setPosition(mSplitterMain.getX(), mSplitterMain.getY());
                    this.addControl(mChangesView);
                }
                mCurrentScreenView = mChangesView;
                mScreenState = SCREENSTATE_CHANGES_STATISTICS;
            }
            else if (buttonID == C.ID_GOTO_HOME_SCREEN)
            {
                if (mScreenState != SCREENSTATE_HOME)
                {
                    if (mCurrentScreenView != null)
                    {
                        this.removeControl(mCurrentScreenView);
                    }
                    this.addControl(mSplitterMain);
                }
                mChangesView = null;
                mCurrentScreenView = mSplitterMain;
                mScreenState = SCREENSTATE_HOME;
            }
            else if (buttonID == C.ID_SWITCH_VIEW)
            {
                stShareGroup g = mContext.getCurrentShareGroup();
                if (g.getGroupType() != stShareGroup.ID_GROUP_GAINLOSS)
                {
                    mTimerRequestOpen.expireTimer();
                    mContext.mViewTypeOfPriceboard++;
                    if (mContext.mViewTypeOfPriceboard > VIEWTYPE_CANDLE) mContext.mViewTypeOfPriceboard = VIEWTYPE_TABLE;
                    mContext.saveOptions2();

                    recreatePriceboard();
                }
            }
            else if (buttonID == C.ID_ALARM_MANAGER)
            {
                showAlarmManager();
            }
            else if (buttonID == C.ID_GOTO_SETTING)
            {
                DlgConfig dlg = new DlgConfig();
                dlg.ShowDialog();

                processSettingDlg(dlg.mDialogResult);
            }
            else if (buttonID == C.ID_GOTO_SEARCH_SCREEN)
            {
                goNextScreen(MainApplication.SCREEN_SEARCH);
            }
            /*
            if (buttonID == C.ID_GOTO_MINI_SCREEN)
            {
                ScreenBase scr = (ScreenBase)MainApplication.getInstance().getScreen(MainApplication.SCREEN_MINIMIZED);
                scr.mPreviousScreen = MainApplication.SCREEN_HOME;
                goNextScreen(MainApplication.SCREEN_MINIMIZED);
            }*/
            else if (buttonID == C.ID_GOTO_HELP)
            {
                showHelp();
            }
            else if (buttonID == C.ID_REFRESH_DATA)
            {
                if (mNetState == STATE_NORMAL)
                {
                    mNetState = STATE_PREPARING_PRICEBOARD_ZERO;
                    //mTimer.expireTimer();
                    mTimer.setExpiration(3 * 1000);
                    setStatusMsg("Refreshing...");
                }
                else
                {
                    setStatusMsg("System is busy :(");
                }
            }
            if (buttonID == C.ID_LOGIN)
            {    
                mNetState = STATE_LOGIN_REQUIRED;    
                showLoginDialog("Login hệ thống");
            }
            if (buttonID == C.ID_LOGOUT)
            {
                try
                {
                    mNetProtocol.cancelNetwork();
                    mContext.logout();
                    mNetState = STATE_NORMAL;
                    updateUI();
                }catch(Exception e)
                {
                }
            }
        }

        xBaseControl createGainlossTable(int w, int h)
        {
            /*
            xContainer c = new xContainer(this);

            c.setSize(w, h);

            stShareGroup g = mContext.getCurrentShareGroup();
            float[] columnPercents = {6, 7, 
                7, 10, 
                8, 10, 8,
                9, 10, 
                8, 7.5f, 9, -1};

            String[] columnTexts = {"Mã CP", "TC", 
                    "Khớp", "KLKhớp", "+/-", "Tổng KL Khớp",
                    "Giá mua", "KL mua",
                    "Tiền vốn (tr)",
                    "Lãi %", "Lãi VNĐ (tr)",
                    "Ngày"};

            xListView l = xListView.createListView(this, columnTexts, columnPercents, w, h, mContext.getImageList(C.IMG_MARKET_ICONS, 20, 21));
            l.setID(C.ID_GAINLOSS_TABLE);
            l.setBackgroundColor(C.COLOR_GRAY);
            mPriceboard = l;

            for (int i = 0; i < mContext.mGainLossManager.getTotal(); i++)
            {
                stGainloss a = mContext.mGainLossManager.getGainLossAt(i);
                if (a != null)
                {
                    RowPriceboardGainLoss row = RowPriceboardGainLoss.createRowPriceboardGainLoss(a, this);
                    l.addRow(row);
                    row.update();
                }
            }

            c.addControl(l);
            //addControl(c);
            */
            int rowH = 34;
            stShareGroup g = mContext.getCurrentShareGroup();
            TablePriceboard priceboard = new TablePriceboard(this, mContext.mGainLossManager, w, rowH);
            int boardH = rowH * mContext.mGainLossManager.getTotal() + 160;
            if (boardH < h)
                boardH = h;
            priceboard.setSize(w, boardH);
            priceboard.invalidate();

            xScrollView scroll = new xScrollView(this, w, h);
            scroll.addControl(priceboard);
            mPriceboard = priceboard;

            //==================context menu=============
            int baseIDX = C.ID_SHARE_GROUP_BASE;
            int groups = mContext.getShareGroupCount();
            if (groups > 0)
            {
                int cnt = 50;   //  Them, xoa
                int[] ids = new int[cnt];
                String[] texts = new String[cnt];
                //==================
                int[] ids0 = { C.ID_BUY_MORE, C.ID_SELL_MORE, -1, C.ID_ADD_SHARE_GAINLOSS, C.ID_REMOVE_SHARE_GAINLOSS, -1, C.ID_ADD_GROUP, C.ID_REMOVE_GROUP, -1, C.ID_SET_ALARM };
                String[] texts0 = { "Sửa: mua thêm cổ phiếu", "Sửa: bán bớt cổ phiếu", "-", "Thêm cổ phiếu (Lãi-Lỗ)", "Xóa CP khỏi d/s (Lãi-Lỗ)", "", "Thêm nhóm yêu thích", "Xóa nhóm", "", "Cài đặt Cảnh báo"};

                int i;
                for (i = 0; i < ids0.Length; i++)
                {
                    ids[i] = ids0[i];
                    texts[i] = texts0[i];
                }

                priceboard.setMenuContext(ids, texts, i);
            }

            return scroll;// c;
        }

        xBaseControl createPriceboard(int w, int h)
        {
            stShareGroup g = mContext.getCurrentShareGroup();
            if (mContext.mViewTypeOfPriceboard == VIEWTYPE_CANDLE 
                && g.getGroupType() != stShareGroup.ID_GROUP_GAINLOSS)
            {
                return _createPriceboardAsCandles(w, h);
            }
            else
            {
                return _createPriceboardAsTable(w, h);
            }
        }

        xContainer _createPriceboardAsCandles(int w, int h)
        {
            xContainer c = new xContainer(this);

            c.setSize(w, h);

            stShareGroup g = mContext.getCurrentShareGroup();

            int candleW = 110;
            int candleH = 110;
            int colsPerRow = (w-10) / candleW;
            candleW = w / colsPerRow;

            int rows = g.getTotal() / colsPerRow;
            int remain = g.getTotal() % colsPerRow;
            if (remain != 0)
                rows++;

            if (rows >= 3)
                candleW -= 3;

            int x = 0;
            int y = 0;

            xContainer board = new xContainer(this);
            board.setSize(colsPerRow*candleW, rows*candleH);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < colsPerRow; j++)
                {
                    int idx = i * colsPerRow + j;
                    if (idx >= g.getTotal())
                        break;

                    string code = g.getCodeAt(idx);
                    Share share = mContext.mShareManager.getShare(code);
                    if (share != null)
                    {
                        TodayCandle candle = new TodayCandle();
                        candle.setShare(share);
                        candle.setSize(candleW, candleH);
                        candle.setHasTitle(true);
                        candle.setDrawRefLabel(false);
                        candle.setListener(this);

                        x = j * candleW;
                        y = i * candleH;
                        candle.setPosition(x, y);

                        board.addControl(candle);
                    }
                }
            }
            mPriceboadCandle = board;

            xScrollView sv = new xScrollView(this, w, h);
            sv.addControl(board);
            c.addControl(sv);
            c.setBackgroundColor(C.COLOR_BLACK);

            //==================context menu=============
            int baseIDX = C.ID_SHARE_GROUP_BASE;
            int groups = mContext.getShareGroupCount();
            if (groups > 0)
            {
                int cnt = 50;   //  Them, xoa
                int[] ids = new int[cnt];
                String[] texts = new String[cnt];
                //==================
                int[] ids0 = { C.ID_ADD_SHARE, C.ID_REMOVE_SHARE, -1, C.ID_ADD_GROUP, C.ID_REMOVE_GROUP, -1, C.ID_SET_ALARM };
                String[] texts0 = { "Thêm cổ phiếu", "Xóa CP khỏi d/s", "", "Thêm nhóm yêu thích", "Xóa nhóm", "", "Cài đặt cảnh báo" };

                int i;
                for (i = 0; i < ids0.Length; i++)
                {
                    ids[i] = ids0[i];
                    texts[i] = texts0[i];
                }
                /*
                //==================
                for (int j = 0; j < groups; j++)
                {
                    g = mContext.getShareGroup(j);
                    if (j == 0) //  CP Yeu thich
                        ids[i] = -1;
                    else
                        ids[i] = baseIDX + j;
                    texts[i] = g.getName();
                    i++;
                }
                 */

                c.setMenuContext(ids, texts, i);
            }

            return c;
        }

        private void priceboard_MouseClick(Object sender, MouseEventArgs e)
        {
            //e.X; e.Y;
            ToolTip t = new ToolTip();
            Control c = (Control)sender;
            t.Show("This is a test", c, 3);
        }

        xBaseControl _createPriceboardAsTable(int w, int h)
        {
            int rowH = 35;
            stShareGroup g = mContext.getCurrentShareGroup();
            TablePriceboard priceboard = new TablePriceboard(this, g, w, rowH);
            int boardH = rowH * g.getTotal() + 160;
            if (boardH < h)
                boardH = h;
            priceboard.setSize(w, boardH);
            priceboard.invalidate();
            mPriceboard = priceboard;

            xScrollView scroll = new xScrollView(this, w, h);
            scroll.addControl(priceboard);

            //==================context menu=============
            int baseIDX = C.ID_SHARE_GROUP_BASE;
            int groups = mContext.getShareGroupCount();
            if (groups > 0)
            {
                int cnt = 50;   //  Them, xoa
                int[] ids = new int[cnt];
                String[] texts = new String[cnt];
                //==================
                int[] ids0 = { C.ID_ADD_SHARE, C.ID_REMOVE_SHARE, -1, C.ID_ADD_GROUP, C.ID_REMOVE_GROUP, -1, C.ID_SHOW_SHARES_CHANGED_IN_GROUP, -1, C.ID_SET_ALARM };
                String[] texts0 = { "Thêm cổ phiếu", "Xóa CP khỏi d/s", "", "Thêm nhóm yêu thích", "Xóa nhóm", "", "Thay đổi cổ phiếu trong nhóm", "", "Cài đặt cảnh báo" };

                int i;
                for (i = 0; i < ids0.Length; i++)
                {
                    ids[i] = ids0[i];
                    texts[i] = texts0[i];
                }

                priceboard.setMenuContext(ids, texts, i);
            }

            return scroll;
            /*
            xContainer c = new xContainer(this);

            c.setSize(w, h);

            c
            float[] columnPercents = { 5.6f, 4.5f, 
                4.4f, 5.0f, 4.4f, 5.2f, 4.6f, 6.0f, 
                4.5f, 5.5f, 4f, 
                4.6f, 6.0f, 4.4f, 5.2f, 4.4f, 5.0f, 
                4.4f, 4.4f, 8 };
            String[] columnTexts = {"Mã CP", "TC", "Mua3", "KL 3", "Mua2", "KL 2", "Mua1", "KL 1",
                    "Khớp", "KLKhớp", "+/-",
                    "Bán1", "KL 1", "Bán2", "KL 2", "Bán3", "KL 3", 
                    "Cao", "Thấp", "Tổng KL"};

            xListView l = xListView.createListView(this, columnTexts, columnPercents, w, h, mContext.getImageList(C.IMG_MARKET_ICONS, 20, 21));

            l.setID(C.ID_PRICEBOARD_TABLE);
            l.setBackgroundColor(C.COLOR_GRAY);
            mPriceboard = l;

            if (g != null)
            {
                for (int i = 0; i < g.getTotal(); i++)
                {
                    int shareID = g.getIDAt(i);
                    if (shareID != -1)
                    {
                        RowPriceboard row = RowPriceboard.createRowPriceboard(shareID, this);
                        l.addRow(row);
                        row.update();
                    }
                }
            }
            c.addControl(l);
            //addControl(c);

            //==================context menu=============
            int baseIDX = C.ID_SHARE_GROUP_BASE;
            int groups = mContext.getShareGroupCount();
            if (groups > 0)
            {
                int cnt = 50;   //  Them, xoa
                int[] ids = new int[cnt];
                String[] texts = new String[cnt];
                //==================
                int[] ids0 = { C.ID_ADD_SHARE, C.ID_REMOVE_SHARE, -1, C.ID_ADD_GROUP, C.ID_REMOVE_GROUP, -1, C.ID_SET_ALARM };
                String[] texts0 = { "Thêm cổ phiếu", "Xóa CP khỏi d/s", "", "Thêm nhóm yêu thích", "Xóa nhóm", "", "Cài đặt cảnh báo"};

                int i;
                for (i = 0; i < ids0.Length; i++)
                {
                    ids[i] = ids0[i];
                    texts[i] = texts0[i];
                }

                l.setMenuContext(ids, texts, i);
            }

            return c;
            */
        }

        xContainer createGlobalIndicesPriceboard(int w, int h)
        {
            xContainer c = new xContainer(this);

            c.setSize(w, h);

            //  name, symbol, price, chage, changePercent
            float[] columnPercents = { 35, 20, 15, 15, 15};
            String[] columnTexts = {"Chỉ số sàn", "Mã", "Điểm", "+/-", "%"};

            xListView l = xListView.createListView(this, columnTexts, columnPercents, w, h, mContext.getImageList(C.IMG_BLANK_ROW_ICON, 1, 21));
            l.setID(C.ID_PRICEBOARD_TABLE);
            l.setBackgroundColor(C.COLOR_GRAY);
            mGlobalPriceboard = l;

            for (int i = 0; i < mContext.mGlobalIndices.size(); i++)
            {
                stGlobalQuote quote = (stGlobalQuote)mContext.mGlobalIndices.elementAt(i);
                RowGlobalQuote row = RowGlobalQuote.createRowQuoteList(quote, this);
                l.addRow(row);
                row.update();
            }
            c.addControl(l);

            return c;
        }

        void removeInvalidateItemFromRightPanel()
        {
            for (int i = 0; i < mRightPanel.getItemCount(); i++)
            {
                xBaseControl c = (xBaseControl)mRightPanel.getControlAt(i);

                for (int j = 0; j < mControlsShouldInvalideAfterNetDone.size(); j++)
                {
                    xBaseControl c0 = (xBaseControl)mControlsShouldInvalideAfterNetDone.elementAt(j);

                    if (c == c0)
                    {
                        mControlsShouldInvalideAfterNetDone.removeElementAt(j);
                        break;
                    }
                }
            }
        }

        void updateItemsAfterNetDone()
        {
            Utils.trace("=====================refresh table");
            mPriceboard.invalidate();
            if (mPriceboardContainer != null)
                mPriceboardContainer.invalidate();

            for (int j = 0; j < mControlsShouldInvalideAfterNetDone.size(); j++)
            {
                xBaseControl c0 = (xBaseControl)mControlsShouldInvalideAfterNetDone.elementAt(j);

                c0.invalidate();
            }

            if (mRealtimeTradeList != null)
                mRealtimeTradeList.updateList();

            if (mTodayCandle != null)
                mTodayCandle.invalidate();
            if (mQuotePoint != null)
                mQuotePoint.invalidate();
        }

        void addRealtimeIndicesControls()
        {
            int y = 0;

            removeInvalidateItemFromRightPanel();
            mRightPanel.removeAllControls();

            int hh = mRightPanel.getH() / 2 - 5;
            //if (mContext.mPriceboard.getIndicesCount() >= 3 && Context.HAS_VNX30)
                //hh = mRightPanel.getH() / 3 - 5;

            for (int i = 0; i < mContext.mPriceboard.getIndicesCount(); i++)
            {
                //if (!Context.HAS_VNX30 && i == 2)
                  //  break;

                stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(i);
                if (pi.code == null)
                    break;
                if (pi.marketID != 1 && pi.marketID != 2)
                    break;

                IndexControl ic = new IndexControl(this, pi.marketID, mRightPanel.getW(), hh);
                ic.setPosition(0, y);
                ic.setSize(mRightPanel.getW(), hh);
                y += ic.getBottom() + 10;

                mControlsShouldInvalideAfterNetDone.addElement(ic);

                mRightPanel.addControl(ic);
            }

            mRightPanelIsIndices = true;
        }

        void addRealtimeCtrlOfCurrentShare()
        {
            mRightPanelIsIndices = false;
            if (mContext.mCurrentTradeHistory == null)
                return;
            removeInvalidateItemFromRightPanel();
            mRightPanel.removeAllControls();

            xTabControl tc = xTabControl.createTabControl();
            tc.setSize(mRightPanel.getW(), mRightPanel.getH() - 10);//mRealtimeMoneyChart.getBottom() + 20);
            mRightPanel.addControl(tc);
            xTabPage page1 = new xTabPage("Đồ thị phiên");
            xTabPage page2 = new xTabPage("Danh sách lệnh");
            //xTabPage page3 = new xTabPage("Thông tin tài chính");
            tc.addPage(page1);
            tc.addPage(page2);
            //tc.addPage(page3);
            //==============================page 1's content================================
            float unitH = (mRightPanel.getH() - 20) / 10;
            float candleH = 2.5f * unitH;
            float realtimeChartH = 4 * unitH;
            float moneyPriceH = 3.5f * unitH;
            //-----------today candle
            mTodayCandle = createTodayCandle(150, (int)candleH);
            mQuotePoint = createQuotePoint(mRightPanel.getW() - mTodayCandle.getW() - 3, (int)candleH);
            mTodayCandle.setPosition(0, 0);
            mQuotePoint.setPosition(mTodayCandle.getRight() + 2, 0);
            int y = 0;
            //  realtime chart
            if (mRealtimeChart != null)
            {
                if (mShouldDrawMACD != mRealtimeChart.mShouldDrawMACD)
                {
                    mShouldDrawMACD = mRealtimeChart.mShouldDrawMACD;
                    saveOptions();
                }
            }

            if (mRealtimeChart == null)
            {
                RealtimeChart rc = new RealtimeChart(mContext.mCurrentTradeHistory, this);

                mRealtimeChart = rc;
            }
            mRealtimeChart.mShouldDrawMACD = mShouldDrawMACD;

            mRealtimeChart.setSize(mRightPanel.getW() - 10, (int)realtimeChartH);
            mRealtimeChart.setPosition(0, mTodayCandle.getBottom() + 4);
            //----------money - price
            int infoW = 210;
            if (mRealtimeMoneyChart == null)
            {
                mRealtimeMoneyChart = new ChartMoney(mContext.getSelectedShare().mID);
                mRealtimeMoneyChart.setPosition(0, mRealtimeChart.getBottom() + 4);
            }
            //mRealtimeMoneyChart.setSize(mRealtimeChart.getW(), (int)moneyPriceH);
            int moneyW = tc.getW() - infoW - 4;
            if (moneyW < 80)
            {
                moneyW = 80;
                infoW = tc.getW() - moneyW - 4;
            }
            if (infoW < 0)
            {
                infoW = 1;
            }
            mRealtimeMoneyChart.setSize(moneyW, (int)moneyPriceH);
            mRealtimeMoneyChart.setShareID(mContext.mCurrentTradeHistory.mShareID);
            //==========================
            mControlsShouldInvalideAfterNetDone.addElement(mTodayCandle);
            mControlsShouldInvalideAfterNetDone.addElement(mRealtimeChart);
            mControlsShouldInvalideAfterNetDone.addElement(mRealtimeMoneyChart);
            //================================
            page1.addControl(mTodayCandle);
            page1.addControl(mQuotePoint);
            page1.addControl(mRealtimeChart);
            page1.addControl(mRealtimeMoneyChart);
            //==============================page 2================================

            TradeHistory trade = mContext.getTradeHistory(mContext.getSelectedShare().mID);
            mRealtimeTradeList = new RealtimeTradeListDetail(trade, tc.getW() - 30, tc.getH() - 10);
            page2.addControl(mRealtimeTradeList.getListCtrl());
            //====================================================================
            y = tc.getBottom() + 4;
            //  info
            CompanyInfo ci = new CompanyInfo(mContext.mCurrentTradeHistory.getCodeID(), true);
            ci.setSize(infoW, 300);
            ci.setPosition(mRealtimeMoneyChart.getRight()+4, mRealtimeMoneyChart.getY());
            ci.init();
            page1.addControl(ci);
            //page3.addControl(ci);
        }

        void invalidateCharts()
        {
            for (int i = 0; i < mCharts.size(); i++)
            {
                xBaseControl cb = (xBaseControl)mCharts.elementAt(i);
                cb.invalidate();
            }
        }

        /*
        void onContextMenu(Object sender, int idx)
        {
            try
            {
                xBaseControl c = (xBaseControl)sender;
                int senderID = c.getID();
                if (senderID == C.ID_PRICEBOARD_TABLE)
                {
                    selectShareGroup(idx - C.ID_SHARE_GROUP_BASE);
                }
            }
            catch (Exception e)
            {
            }
        }

        void selectShareGroup(int groupIDX)
        {
            if (groupIDX >= 0 && groupIDX < mContext.getShareGroupCount())
            {
                stShareGroup g = mContext.getShareGroup(groupIDX);
                mContext.setCurrentShareGroup(g);

                updateUI();
            }
        }
         */

        void onContextMenu(int idx)
        {
            if (idx == C.ID_BUY_MORE)
            {
                RowNormalShare item = mPriceboard.getSelectedItem();

                if (item != null)
                {
                    stGainloss g = (stGainloss)item.getData();

                    DlgBuyMore dlg = new DlgBuyMore(g);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        int price = dlg.getPrice();
                        int vol = dlg.getVolume();

                        if (price > 0 && vol > 0)
                        {
                            double m = g.price * g.volume + price * vol;
                            g.price = (int)(m / (g.volume + vol));
                            g.volume += vol;

                            mContext.mIsFavorGroupChanged = true;
                            showGainlossTable();
                        }
                        else
                        {
                            showDialogOK("Dữ liệu (Giá / Khối lượng) phải lớn hơn 0");
                        }
                    }
                }
                else
                {
                    showDialogOK("Bạn chưa chọn cổ phiếu nào để mua thêm.");
                }
            }
            if (idx == C.ID_SELL_MORE)
            {
                RowNormalShare item = mPriceboard.getSelectedItem();

                if (item != null)
                {
                    stGainloss g = (stGainloss)item.getData();
                    DlgSellStock dlg = new DlgSellStock(g);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        int vol = dlg.getVolume();
                        if (vol > 0 && vol < g.volume)
                        {
                            g.volume -= vol;
                            mContext.mIsFavorGroupChanged = true;

                            showGainlossTable();
                        }
                        else
                        {
                            showDialogOK("Khối lượng bán phải nhỏ hơn " + g.volume + "và lớn hơn 0");
                        }
                    }
                }
                else
                {
                    showDialogOK("Bạn chưa chọn cổ phiếu nào để bán.");
                }
            }
            if (idx == C.ID_ADD_SHARE_GAINLOSS)
            {
                DlgAddShareGainloss dlg = new DlgAddShareGainloss(C.ID_DLG_ADD_SHARE_GAINLOSS, this);
                //dlg.Show();
                dlg.ShowDialog();

                if (dlg.getResultID() == C.ID_DLG_BUTTON_OK)
                {
                    String code = dlg.getText();
                    code = code.ToUpper();
                    float price = dlg.getPrice()/1000.0f;
                    int vol = dlg.getVolume();
                    int date = Utils.getDateAsInt();
                    int shareID = mContext.mShareManager.getShareID(code);
                    if (shareID > 0)
                    {
                        mContext.mGainLossManager.addGainLoss(code, date, price, vol);
        
                        mContext.mIsFavorGroupChanged = true;

                        showGainlossTable();
                    }
                    else
                    {
                        showDialogOK("Mã cổ phiếu không hợp lệ.");
                    }
                }
            }
            if (idx == C.ID_REMOVE_SHARE_GAINLOSS)
            {
                RowNormalShare item = mPriceboard.getSelectedItem();

                if (item != null)
                {
                    stGainloss g = (stGainloss)item.getData();
                    mContext.mGainLossManager.removeGainLoss(g);
                    showGainlossTable();
                    mContext.mIsFavorGroupChanged = true;
                }
                else
                {
                    showDialogOK("Bạn chưa chọn cổ phiếu nào.");
                }
            }
            //================================
            if (idx == C.ID_ADD_SHARE)
            {
                DlgAddShare dlg = new DlgAddShare(C.ID_DLG_ADD_SHARE, this);
                //dlg.Show();
                dlg.ShowDialog();

                if (dlg.getResultID() == C.ID_DLG_BUTTON_OK)
                {
                    String code = dlg.getText();
                    code = code.ToUpper();
                    int shareID = mContext.mShareManager.getShareID(code);
                    if (shareID > 0)
                    {
                        stShareGroup g = mContext.getCurrentShareGroup();
                        if (g != null)
                        {
                            g.addCode(code);
                            if (g.isFavorGroup())
                            {
                                mContext.saveFavorGroup();
                                mContext.mIsFavorGroupChanged = true;
                                mTimer.expireTimer();
                            }
                            else
                                mContext.saveDefinedShareGroup();

                            recreatePriceboard();
                        }
                    }
                    else
                    {
                        showDialogOK("Mã cổ phiếu không hợp lệ.");
                    }
                }
            }
            if (idx == C.ID_REMOVE_SHARE)
            {
                /*
                xListViewItem item = mPriceboard.getSelectedItem();
                if (item != null)
                {
                    RowPriceboard r = (RowPriceboard)item;
                    int shareID = r.mShareID;
                    Share share = mContext.mShareManager.getShare(shareID);
                 */  
                Share share = mContext.getSelectedShare();// mContext.mShareManager.getShare(shareID);    
                if (share != null && !share.isIndex())    
                {
                    stShareGroup g = mContext.getCurrentShareGroup();
                    if (g != null && share != null)
                    {
                        if (g.containShare(share.mID))
                        {
                            MessageBoxButtons bt = MessageBoxButtons.YesNo;
                            if (MessageBox.Show("Xóa cổ phiếu: " + share.mCode + " khỏi danh sách?", "Remove share", bt)
                                == DialogResult.Yes)
                            {

                                g.removeCode(share.getCode());
                                if (g.isFavorGroup())
                                    mContext.saveFavorGroup();
                                else
                                    mContext.saveDefinedShareGroup();

                                mContext.mIsFavorGroupChanged = true;
                                mTimer.expireTimer();

                                recreatePriceboard();
                            }
                        }
                    }
                }
                else
                {
                    showDialogOK("Bạn chưa chọn cổ phiếu nào.");
                }
            }
            if (idx == C.ID_ADD_GROUP)
            {
                if (mContext.mFavorGroups.size() > 20)
                {
                    showDialogOK("Số nhóm tự tạo không quá 20.");
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
                        mContext.saveFavorGroup();

                        mContext.mIsFavorGroupChanged = true;
                        mTimer.expireTimer();

                        updateUI();
                    }
                }
            }
            if (idx == C.ID_REMOVE_GROUP)
            {
                stShareGroup g = mContext.getCurrentShareGroup();
                if (g == null)
                    return;
                if (g.isFavorGroup())
                {
                    StringBuilder sb = Utils.sb;
                    sb.Length = 0;
                    sb.AppendFormat("Xóa nhóm {0}", g.getName());
                    if (showDialogYesNo(sb.ToString()))
                    {
                        mContext.removeShare(g);
                        mContext.selectDefaultShareGroup();
                        updateUI();

                        mContext.mIsFavorGroupChanged = true;
                        mTimer.expireTimer();
                    }
                }
                else
                {
                    showDialogOK("Bạn chỉ xóa được nhóm do bạn tự tạo");
                }
            }
            if (idx == C.ID_SET_ALARM)
            {
                /*
                xListViewItem item = mPriceboard.getSelectedItem();
                if (item != null)
                {
                    int shareID = -1;
                    if (item is RowPriceboard)
                    {
                        RowPriceboard r = (RowPriceboard)item;
                        shareID = r.mShareID;
                    }
                    else
                    {
                        RowPriceboardGainLoss r = (RowPriceboardGainLoss)item;
                        stGainloss gain = (stGainloss)r.getData();

                        shareID = mContext.mShareManager.getShareID(gain.code);
                    }
                    if (shareID == -1)
                        return;
                */
                    
                Share share = mContext.getSelectedShare();// mContext.mShareManager.getShare(shareID);
                if (share != null && !share.isIndex())
                {
                    stAlarm a = mContext.mAlarmManager.getAlarm(share.getCode());
                    if (a == null)
                    {
                        a = new stAlarm();
                        mContext.mAlarmManager.addAlarm(a);
                        a.code = share.getCode();
                    }
                    //---------------------
                    settingAlarm(a);
                }
                else
                {
                    showDialogOK("Bạn chưa chọn cổ phiếu nào.");
                }
            }
            else if (idx == C.ID_SHOW_SHARES_CHANGED_IN_GROUP)
            {
                stShareGroup g = mContext.getCurrentShareGroup();
                if (g != null)
                {
                    //showTangtruongOfGroup(g);
                }
            }
        }

        void recreatePriceboard()
        {
            mPriceboadCandle = null;
            if (mPriceboardContainer != null)
            {
                mLeftPanel.removeControl(mPriceboardContainer);
            }
            xBaseControl list = null;

            list = createPriceboard(W_PRICEBOARD, mPriceboardH);

            list.setPosition(X_PRICEBOARD, mPriceboardY);

            mPriceboardContainer = list;
            
            mLeftPanel.addControl(mPriceboardContainer);
        }

        ChartNhomnganhTangtruong createThaydoiNhomnganh(int w, int h)
        {
            ChartNhomnganhTangtruong list = new ChartNhomnganhTangtruong();
            list.setSize(w, h);
            xVector v = mContext.getMainMarketGroups(true);
            for (int i = 0; i < mContext.mFavorGroups.size(); i++)
            {
                stShareGroup g = (stShareGroup)mContext.mFavorGroups.elementAt(i);
                if (g.getName().IndexOf('#') == 0)
                {
                    v.addElement(g);
                }
            }
            for (int i = 1; i < mContext.mShareGroups.size(); i++)
            {
                v.addElement(mContext.mShareGroups.elementAt(i));
            }

            list.setGroup(v);
            return list;
        }

        xContainer createThaydoiMainIndices(int w, int h)
        {
            xContainer container = new xContainer();
            container.setSize(w, h);

            int itemW = w / 4 - 1;
            int itemH = h / 2 - 1;
            //  Dòng tiền | Trọng số | Tăng Index | Giảm Index

            int[] markets = { 1, 2 };
            for (int i = 0; i < 2; i++)
            {
                stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexOfMarket(markets[i]);

                int[] types = {ChartStatistics.BUBBLE_TRADE_MONEY, ChartStatistics.BUBBLE_VOLUME_WEIGHT, 
                    ChartStatistics.BUBBLE_INDEX_WEIGHT_RATIO_INC, ChartStatistics.BUBBLE_INDEX_WEIGHT_RATIO_DEC};
                int[] xx = {0, w/4, 2*w/4, 3*w/4};

                //for (int j = 0; j < 4; j++)
                for (int j = 0; j < 4; j++)
                {
                    ChartStatistics moneyChart = new ChartStatistics(pi.marketID, this);
                    moneyChart.setChartType(types[j]);
                    moneyChart.setSize(itemW, itemH);
                    moneyChart.setPosition(xx[j], i*h/2);

                    moneyChart.doCalcChanged();

                    container.addControl(moneyChart);
                }
            }
            return container;
        }

        void showGroupHistory(string code)
        {
            Share share = new Share(Share.MAX_CANDLE_CHART_COUNT);
            share.setID(100000);
            share.setCode(code, 1);
            share.loadShareFromFile(false);

            goChartScreen(share);
        }

        void onClickShare(string code)
        {
            int shareID = mContext.mShareManager.getShareID(code);
            if (shareID > 0)
            {
                goChartScreen(shareID);
            }
        }

        void goChartScreen(int shareID)
        {
            /*
            doNotRecreateHomeScreen = true;
            if (shareID != -1)
            {
                mContext.setCurrentShare(shareID);
            }

            ViewHistoryChart scr = (ViewHistoryChart)MainApplication.getInstance().getScreen(MainApplication.SCREEN_SEARCH);
            scr.mScreenType = ViewHistoryChart.TYPE_CHART;
            goNextScreen(MainApplication.SCREEN_SEARCH);
             * */
            Share share = mContext.mShareManager.getShare(shareID);
            ScreenRoot.instance().createNewHistory(share);
        }

        void goChartScreen(Share share)
        {
            ScreenRoot.instance().createNewHistory(share);
        }

        void processSettingDlg(int dlgResult)
        {
            if (dlgResult == DlgConfig.RESULT_OK)
            {
                recreateSlogan();
                updateUI();
            }
            if (dlgResult == DlgConfig.RESULT_RELOAD)
            {
                if (showDialogYesNo("Vui lòng chạy lại vnChart để tải lại dữ liệu."))
                {
                    reloadAllData();
                }
            }
            if (dlgResult == DlgConfig.RESULT_EXPAND_ACCOUNT)
            {
                showWeb(C.URL_EXPAND_ACCOUNT);
            }
            if (dlgResult == DlgConfig.RESULT_CHANGE_PASSWORD)
            {
                if (mContext.isOnline())
                {
                    DlgResetPass dlg = new DlgResetPass();
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        mRequestChangePassword = true;
                        mRequestNewPassword = dlg.getNewPassword();
                        mTimer.expireTimer();
                    }
                }
                else
                {
                    showDialogOK("Bạn cần login trước khi thực hiện chức năng này.");
                }
            }
            if (dlgResult == DlgConfig.RESULT_RESET_PASSWORD)
            {
                if (mContext.isOnline())
                {
                    mRequestResetPassword = true;
                    mTimer.expireTimer();
                }
                else
                {
                    showDialogOK("Bạn cần login trước khi thực hiện chức năng này.");
                }
            }
        }

        bool hasServerNotification()
        {
            int serverNotificationCode = mNetProtocol.getServerNotificationCode();
            if (serverNotificationCode > 0)
            {
                if (NetProtocol.SN_SESSION_TIMEOUT == serverNotificationCode)
                {
                    mNetProtocol.cancelNetwork();
                    mContext.logout();
                    updateUI();
                }
                String msg;
                if ((NetProtocol.SN_ACCOUNT_GOING_EXPIRE == serverNotificationCode)
                    || (NetProtocol.SN_ACCOUNT_EXPIRED == serverNotificationCode))
                {
                    if (NetProtocol.SN_ACCOUNT_GOING_EXPIRE == serverNotificationCode)
                        msg = "Tài khoản của bạn sắp hết hạn, bạn có muốn gia hạn không";
                    else
                        msg = "Tài khoản của bạn đã hết hạn, bạn có muốn gia hạn không";
                    if (showDialogYesNo(msg))
                    {
                        showWeb(C.URL_EXPAND_ACCOUNT);
                    }
                    mNetProtocol.resetServerNotification();
                }
                else
                {
                    msg = mNetProtocol.getServerNotificationMsg();
                    mNetProtocol.resetServerNotification();
                    if (msg != null && msg.Length > 0)
                    {
                        showDialogOK(msg);
                    }
                }

                return true;
            }

            return false;
        }

        void refreshPriceboard()
        {
        }

        void showGainlossTable()
        {
            if (mPriceboardContainer != null)
            {
                mLeftPanel.removeControl(mPriceboardContainer);
            }
            xBaseControl list = null;
   
            list = createGainlossTable(W_PRICEBOARD, mPriceboardH);

            list.setPosition(X_PRICEBOARD, mPriceboardY);

            mPriceboardContainer = list;
            mLeftPanel.addControl(mPriceboardContainer);
        }

        void showMostIncreasedGroup(stShareGroup g)
        {
            if (mContext.mShareManager.mShareIDs == null)
                return;
            int i;

            int cnt = mContext.mShareManager.mShareIDs.Length/ShareManager.SHARE_ID_SIZE;
            int[] ids = new int[cnt];
            int[] v = new int[cnt];
            int k = 0;

            for (i = 0; i < cnt; i++)
            {
                int marketID = (int)mContext.mShareManager.mShareIDs[k];
                int shareID = (mContext.mShareManager.mShareIDs[k+1] << 8) | (mContext.mShareManager.mShareIDs[k+2]);
                ids[i] = shareID;
                stPriceboardState ps = mContext.mPriceboard.getPriceboard(shareID);

                if (ps != null && ps.getRef() > 0 && ps.getCurrentPrice() > 0)
                {
                    int inc;
                    if (g.getGroupType() == stShareGroup.ID_GROUP_MOST_VOL)
                        inc = ps.getTotalVolume();
                    else if (g.getGroupType() == stShareGroup.ID_GROUP_MOST_VOL_INC_PERCENT)
                    {
                        double total = ps.getTotalVolume();
                        inc = -1000;
                        Share share = mContext.mShareManager.getShare(shareID);
                        //if (share.mCode == "BRC")
                        //{
                            //int ttt = 0;
                        //}
                        if (share != null && total > 10000)
                        {
                            mContext.mShareManager.loadShareFromCommon(share, 12, true);
                            int vol10days = share.calcTotalVolume(10);
                            if (vol10days > 0)
                            {
                                inc = (int)(total/vol10days);
                            }
                        }
                    }
                    else
                        inc = (int)((ps.getCurrentPrice() - ps.getRef()) * 1000 / ps.getRef());
                    v[i] = inc;
                }
                else
                {
                    v[i] = 0;
                }

                k += ShareManager.SHARE_ID_SIZE;
            }
            //  sort now
            int smallestIDX;
            int smallestVal;
            for (i = 0; i < cnt-1; i++)
            {
                int a = v[i];
                int a_ID = ids[i];
                smallestVal = a;
                smallestIDX = i;
                for (int j = i+1; j < cnt; j++)
                {
                    int b = v[j];
                    int valB = b;

                    if (valB < smallestVal)
                    {
                        smallestIDX = j;
                        smallestVal = valB;
                    }
                }
                //  swap places
                v[i] = v[smallestIDX];
                v[smallestIDX] = a;

                ids[i] = ids[smallestIDX];
                ids[smallestIDX] = a_ID;
            }

            //===============sort=========
            g.clear();
            int rows = 40;
            if (g.getGroupType() == stShareGroup.ID_GROUP_MOST_DEC)
            {
                for (i = 0; i < rows; i++)
                {
                    int shareID = ids[i];
                    g.addCode(mContext.mShareManager.getShareCode(shareID));
                }
            }
            else// if (g.getGroupType() == stShareGroup.ID_GROUP_MOST_INC)
            {
                for (i = cnt-1; i > cnt-1-rows; i--)
                {
                    int shareID = ids[i];
                    g.addCode(mContext.mShareManager.getShareCode(shareID));
                }
            }
            //========================

            mContext.setCurrentShareGroup(g);

            if (g != null)
            {
                setTitle("  -Nhóm: " + g.getName());
            }

            recreatePriceboard();
        }

        override public void onShareGroupSelected(stShareGroup g)
        {
            if (g.getGroupType() == stShareGroup.ID_GROUP_MOST_INC
                || g.getGroupType() == stShareGroup.ID_GROUP_MOST_DEC
                || g.getGroupType() == stShareGroup.ID_GROUP_MOST_VOL
                || g.getGroupType() == stShareGroup.ID_GROUP_MOST_VOL_INC_PERCENT)
            {
                mContext.setCurrentShareGroup(g);
                showMostIncreasedGroup(g);
                return;
            }
            //if (g.getGroupType() == stShareGroup.ID_GROUP_MARKET_OVERVIEW)
            //{
                //createMarketOverviewChart();
                //return;
            //}
            if (g.getGroupType() == stShareGroup.ID_GROUP_GAINLOSS)
            {
                setTitle("  -Nhóm: " + g.getName());

                mContext.setCurrentShareGroup(g);

                showGainlossTable();
                return;
            }
            //=====================================================
            mContext.setCurrentShareGroup(g);

            if (g != null)
            {
                setTitle("  -Nhóm: " + g.getName());

            }

            recreatePriceboard();

            //updateUI();
            mTimerGlobal.expireTimer();
        }

        void onGlobalNetEvent(int evt, int aIntParameter, object aParameter)
        {
            if (evt == xBaseControl.EVT_NET_DONE)
            {
                
            }
            if (evt == xBaseControl.EVT_NET_ERROR)
            {
                mNetState = STATE_NORMAL;
            }
        }
        ToolStrip createToolStripForMainChart()
        {
            int frameW = 23;
            int frameH = 20;
            //  line | candle | OHDC |*| 
            //  BB | PSAR | Ichi | SMA1 | SMA2 | SMA3 | * | 
            //  Trend/Retrace/Fan/Arc/Projection/Time | * | 
            //  + SubChart 
            int[] icons = {0, 1, 29, 2, 18, -1, 
                            3, 4, 5, 6, 7, -1, 
                            9, 10, 11, 12, 13, 14, -1};
            int[] ids = {
                            C.ID_TS_CHARTLINE, C.ID_TS_CHARTCANDLE, C.ID_TS_CHARTCANDLE_HEIKEN, C.ID_TS_CHARTOHLC, C.ID_TS_CHARTHLC, 0, 
                            C.ID_TS_BOLLINGER, C.ID_TS_PSAR};

            string[] tips = {
                            "Đồ thị line", "Đồ thị nến Nhật", "Đồ thị nến Heiken Ashi", "Đồ thị OHLC", "Đồ thị HLC", "", 
                            "Bollinger", "Parabolic SAR (Stop and Reverse)"};

            ToolStripItem[] items = new ToolStripItem[ids.Length+1];
            int j = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                if (icons[i] == -1)
                {
                    items[j] = new ToolStripButton();
                    items[j].ImageIndex = 19;
                    items[j].Enabled = false;
                    items[j + 1] = new ToolStripSeparator();
                    j += 1;
                }
                else
                {
                    items[j] = new ToolStripButton();
                    items[j].ImageIndex = icons[i];
                    items[j].Tag = (Int32)ids[i];
                    items[j].ToolTipText = tips[i];
                }

                j++;
            }

            ToolStrip tool = new ToolStrip(items);
            tool.ImageList = mContext.getImageList(C.IMG_TOOLSTRIPS, frameW, frameH);
            tool.Dock = DockStyle.Left;
            tool.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            tool.RenderMode = ToolStripRenderMode.System;
            //tool.Size = new Size(40, tool.Size.Height);
            tool.AutoSize = false;
            tool.ImageScalingSize = new Size(frameW, frameW);
            tool.Size = new Size(frameW + 10, tool.Size.Height);

            tool.ItemClicked += new ToolStripItemClickedEventHandler(tool_ItemClicked);

            return tool;
        }

        void tool_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                ToolStripItem item = e.ClickedItem;

                int idx = (Int32)item.Tag;

                if (idx ==  C.ID_TS_CHARTLINE)
                {
                    mMasterChart.setChartType(ChartBase.CHART_LINE);
                }
                if (idx == C.ID_TS_CHARTCANDLE)
                {
                    mMasterChart.setChartType(ChartBase.CHART_CANDLE);
                }
                if (idx == C.ID_TS_CHARTCANDLE_HEIKEN)
                {
                    mMasterChart.setChartType(ChartBase.CHART_CANDLE_HEIKEN);
                }
                if (idx == C.ID_TS_CHARTOHLC)
                {
                    mMasterChart.setChartType(ChartBase.CHART_OHLC);
                }
                if (idx == C.ID_TS_CHARTHLC)
                {
                    mMasterChart.setChartType(ChartBase.CHART_HLC);
                }
                if (idx == C.ID_TS_BOLLINGER)
                {
                    mMasterChart.toggleAttachChart(ChartBase.CHART_BOLLINGER);
                }
                if (idx == C.ID_TS_PSAR)
                {
                    mMasterChart.toggleAttachChart(ChartBase.CHART_PSAR);
                }
                if (idx == C.ID_TS_VSTOP)
                {
                    mMasterChart.toggleAttachChart(ChartBase.CHART_VSTOP);
                }
                if (idx == C.ID_TS_ICHIMOKU)
                {
                    mMasterChart.toggleAttachChart(ChartBase.CHART_ICHIMOKU);
                }
                mMasterChart.invalidate();
                saveOptions();
            }
            catch (Exception ex)
            {
            }
        }

        void saveOptions()
        {
            if (mMasterChart != null)
            {
                xDataOutput o = new xDataOutput(200);
                o.writeInt(Context.FILE_VERSION_HOME);
                mMasterChart.flush(o);
                o.writeInt(mSubContainer1.getSubchartID());

                o.writeInt(mRightPanelW);

                o.writeBoolean(mShouldDrawMACD);

                xFileManager.saveFile(o, "data\\home.dat");
            }
        }

        xDataInput loadOptions()
        {
            return xFileManager.readFile("data\\home.dat", false);
        }

        public override bool canTranslateToMinize()
        {
            //  logging
            if (mNetState >= 1 && mNetState <= 5)
                return false;

            return base.canTranslateToMinize();
        }

        xContainer mSloganContainer;
        public xContainer recreateSlogan()
        {
            mSloganContainer.removeAllControls();

            xContainer c = mSloganContainer;
            c.setBackgroundColor(mContext.mSloganColorBG);

            //c.setBackgroundColor(C.COLOR_GRAY_DARK);

            string slogan = mContext.mSloganText;
            xLabel l = xLabel.createSingleLabel(slogan, mContext.getFontSlogan(), -1);
            l.setTextColor(mContext.mSloganColor);
            l.setSize(c);
            l.setAlign(xGraphics.HCENTER | xGraphics.VCENTER);
            l.enableClick(C.ID_BUTTON_EDIT_SLOGAN, this);

            c.addControl(l);

            return c;
        }

        void onMainSplitterSizeChanged()
        {
            int w = mRightPanel.getW();
            mRightPanelW = w;

            saveOptions();

            if (mRightPanelIsIndices)
            {
                addRealtimeIndicesControls();
            }
            else
            {
                addRealtimeCtrlOfCurrentShare();
            }
        }

        void selectShare(int shareID)
        {
            Share share = mContext.mShareManager.getShare(shareID);
            if (share == null)
            {
                return;
            }
            if (share.isRealtime())
            {
                share.loadShareFromFile(false);
            }
            else
            {
                selectShare(share);
            }
        }

        void selectShare(Share share)
        {
            if (share != null)
            {
                if (share != null)
                {
                    if (share.isIndex())
                        share.loadShareFromFile(true);
                    else
                        share.loadShareFromCommonData(true);
                }
                mContext.setCurrentShare(share);
                /*
                createChartRangeControls(0, mTimingRange);
                */
                TradeHistory trade = mContext.getTradeHistory(share.getShareID());
                if (trade != null)
                {
                    mContext.setCurrentTradeHistory(trade);
                    addRealtimeCtrlOfCurrentShare();
                    ((RealtimeChart)mRealtimeChart).setTrade(trade);
                }

                mTimer.expireTimer();

                //===========================
                if (mTodayCandle != null)
                {
                    mTodayCandle.setShare(share);
                }
                if (mQuotePoint != null)
                {
                    mQuotePoint.setShare(share);
                }

                if (mRealtimeMoneyChart != null)
                {
                    mRealtimeMoneyChart.setShareID(share.getShareID());
                }


                if (mMasterChart != null)
                {
                    mMasterChart.setShare(share);
                }
                if (mSubContainer1 != null)
                {
                    mSubContainer1.setShare(share);
                }

                updateItemsAfterNetDone();
                invalidateCharts();
            }
        }

        TodayCandle createTodayCandle(int w, int h)
        {
            TodayCandle todayCandle = new TodayCandle();
            todayCandle.setSize(w, h);
            todayCandle.setShare(mContext.getSelectedShare());

            return todayCandle;
        }

        QuotePoint createQuotePoint(int w, int h)
        {
            QuotePoint qp = new QuotePoint();
            qp.setShare(mContext.getSelectedShare());
            qp.setSize(w, h);

            return qp;
        }

        bool needDownloadAllShare()
        {
            int today = Utils.getDateAsInt();

            long delta = Utils.dateToNumber(today) - Utils.dateToNumber(AppConfig.appConfig.allShareUpdateDate);
            if (delta > 10)
            {
                String url = Context.getInstance().configJson.url_all_share2;
                if (url != null && url.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }

        void decompressAllShare(byte[] p, int off, int len)
        {
            xFileManager.removeFile(ShareManager.SAVED_COMMON_SHARE_FILE);

            mContext.mLastDayOfShareUpdate = 0;

            NetProtocol n = new NetProtocol();
            n.ignoreSessionId = true;
            n.processPackage(p, off, len);

            if (xFileManager.isFileExist(ShareManager.SAVED_COMMON_SHARE_FILE))
            {
                AppConfig.appConfig.allShareUpdateDate = Utils.getDateAsInt();
                AppConfig.saveAppConfig();
            }

            Utils.trace("Downloaded all share: " + len);
        }
    }
}
