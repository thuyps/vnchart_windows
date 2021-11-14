using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.ui;
using xlib.framework;
using xlib.utils;
using stock123.app.chart;
using stock123.app.data;
using stock123.app.ui;
using stock123.app.net;

namespace stock123.app
{
    public class HistoryChartControl: xContainer, xIEventListener
    {
        int CHART_W = 0;
        int CHART_X0 = 33;
        const int SUB_CHART_H_DEFAULT = 150;

        Context mContext;
        xSplitter mSplitter;
        xContainer mMainContainer;
        xContainer mSubContainer;
        ChartMaster mChartMaster;

        xContainer mTimingRange;
        xContainer mInfo;
        bool mShowingInfo;
        bool mIsSecondPanel;
        string mName;

        Drawer mDrawer;
        Share mShare;

        xVector mSubCharts = new xVector(10);
        xBaseControl mZoomingChart = null;

        int mChartType;

        public HistoryChartControl(Share share, string name, int w, int h, bool isSecondPanel)
            : base()
        {
            mChartType = Share.CANDLE_DAILY;
            setSize(w, h);
            mIsSecondPanel = isSecondPanel;

            setShare(share);

            mContext = Context.getInstance();

            mDrawer = new Drawer();

            int x = 0;

            if (isSecondPanel)
            {
                CHART_X0 = 0;
                CHART_W = getW() - CHART_X0;
            }
            else
            {
                ToolStrip ts = createToolStrip();
                addControl(ts);

                CHART_X0 = ts.Size.Width;
                CHART_W = getW() - ts.Size.Width;
            }

            int mainH = getH()/5;
            //===================================
            int subCnt = 4;
            int[] subs = new int[20];
            bool[] subs_grid = new bool[20];
            mName = name;

            //============================================
            mMainContainer = new xContainer();
            mMainContainer.setSize(CHART_W, mainH);
            mMainContainer.setPosition(0, 0);

            //  drawing tool
            if (!isSecondPanel)
            {
                xBaseControl tool = createDrawingToolStrip();
                tool.setPosition(140, 0);
                mDrawingToolStrip = tool;
                addControl(tool);

                tool.show(false);
            }
            //=================MASTER chart=====================
            if (!isSecondPanel)
            {
                //====company info
                mInfo = new xContainer();
                mInfo.setSize(150, 0);
                mInfo.setPosition(mMainContainer.getW() - mInfo.getW() - 50, 0);
                mMainContainer.addControl(mInfo);

                if (mTimingRange == null)
                    mTimingRange = new xContainer();
                if (share != null)
                {   
                    createChartRangeControls(share, mTimingRange);
                }
                mTimingRange.setPosition((CHART_W - mTimingRange.getW()) / 2, mMainContainer.getH() - mTimingRange.getH() - 16);
                mMainContainer.addControl(mTimingRange);
            }

            //=========================================================
            ChartMaster cl = createMasterChart(ChartBase.CHART_LINE, CHART_W, mMainContainer.getH());
            if (isSecondPanel)
            {
                cl.mRenderOHLCInfo = false;
                cl.mShouldDrawDateSeparator = false;
                cl.mHasFibonacci = false;
            }
            else
            {
            }
            cl.setShare(mShare);
            cl.setIsMasterChart(true);//isSecondPanel == false);
            mMainContainer.addControl(cl);

            mChartMaster = cl;
            cl.toggleAttachChart(ChartBase.CHART_BOLLINGER);
            //mChartMaster.loadOption();

            //=========================================================
            xDataInput di = xFileManager.readFile("data\\" + name, false);
            if (di != null && di.readInt() == Context.FILE_VERSION)
            {
                mainH = di.readInt();
                mChartMaster.load(di);
                //  correct mainH
                mMainContainer.setSize(mMainContainer.getW(), mainH);
                mChartMaster.setSize(mChartMaster.getW(), mainH);
                //=========================
                subCnt = di.readInt();
                if (subCnt > 10) subCnt = 10;
                for (int j = 0; j < subCnt; j++)
                {
                    subs[j] = di.readInt();
                    subs_grid[j] = di.readBoolean();
                }
            }
            else
            {
                //  default value
                if (isSecondPanel)
                {
                    subCnt = 5;
                    subs[0] = ChartBase.CHART_MACD;
                    subs[1] = ChartBase.CHART_RSI;
                    subs[2] = ChartBase.CHART_MFI;
                    subs[3] = ChartBase.CHART_STOCHASTIC_SLOW;
                    subs[4] = ChartBase.CHART_ROC;

                    subs_grid[0] = false;
                    subs_grid[1] = false;
                    subs_grid[2] = false;
                    subs_grid[3] = false;
                    subs_grid[4] = false;

                    mainH = 40;
                }
                else
                {
                    subCnt = 1;
                    subs[0] = ChartBase.CHART_VOLUME;

                    mainH = getH()*3/4;
                }
            }

            mChartMaster.setHasDrawer(true);
            //====================SUB container
            mSubContainer = new xContainer();

            for (int i = 0; i < subCnt; i++)
            {
                SubchartsContainer sub = createSubchart(subs[i]);
                sub.setHasGrid(subs_grid[i]);
                mSubCharts.addElement(sub);
                mSubContainer.addControl(sub);
            }

            if (mMainContainer == null || mSubContainer == null)
                return;

            //  finally, add main/subs to the splitter
            xSplitter splitter = xSplitter.createSplitter(true, CHART_W, getH(), mainH, 30, 20);
            splitter.setPosition(CHART_X0, 0);
            splitter.setListener(this);

            splitter.setPanels(mMainContainer, mSubContainer);
            x += CHART_W;

            mSplitter = splitter;

            addControl(splitter);

            //============================
            int[] ids = {
                C.ID_TS_CHARTLINE,
                C.ID_TS_CHARTCANDLE,
                C.ID_TS_CHARTCANDLE_HEIKEN,
                C.ID_TS_CHARTHLC,
                C.ID_TS_CHARTOHLC, -1,
                C.ID_EDIT_BOLLINGER, C.ID_EDIT_ENVELOP, C.ID_EDIT_ICHIMOKU, C.ID_EDIT_PSAR, C.ID_EDIT_ZIGZAG, -1, 
                C.ID_CAPTURE_IMAGE,
                -1,
                C.ID_RELOAD_DATA_OF_SYMBOL

                        };
            string[] texts = {
                                "Kiểu đồ thị Line",
                "Kiểu đồ thị Nến",
                "Kiểu đồ thị Nến Heiken",
                "Kiểu đồ thị HLC",
                "Kiểu đồ thị OHLC",
                "",
                                "Thông số Bollinger",
                                "Thông số Envelops",
                                "Thông số Ichimoku Kynko Hyo",
                                "Thông số Parabollic (PSAR)",
                                "Thông số Zigzag",
                                "",
                                "Lưu file ảnh",
                                "",
                                "Tải lại dữ liệu của mã"
                             };
            setMenuContext(ids, texts, ids.Length);
        }

        xBaseControl createDrawingToolStrip()
        {
            int frameW = 23;
            int frameH = 20;
            //  line | candle | OHDC |*| 
            //  BB | PSAR | Ichi | SMA1 | SMA2 | SMA3 | * | 
            //  Trend/Retrace/Fan/Arc/Projection/Time | * | 
            //  + SubChart 
            int[] icons = {
                            27, 21, 22, 24, 23, 25, 26, 
                            -1, 
                            10, 11, 12, 13, 14,
                            -1,
                            28};
            int[] ids = {
                C.ID_TS_DRAW_ABC, C.ID_TS_DRAW_TREND, C.ID_TS_DRAW_TREND_ARROW, C.ID_TS_DRAW_TRIANGLE, C.ID_TS_DRAW_RECTANGLE, C.ID_TS_DRAW_ECLIPSE, C.ID_TS_DRAW_ANDREWS_PITCHFORK,
                -1,
                C.ID_TS_DRAW_RETRACE, C.ID_TS_DRAW_PROJECT, C.ID_TS_DRAW_TIME, C.ID_TS_DRAW_ARC, C.ID_TS_DRAW_FAN,
                -1,
                C.ID_TS_DRAW_CLEAR_ALL
            };
            string[] tips = {"Xóa tất cả các hình vẽ",
                                "",
                                "Text",
                                "Vẽ đường xu hướng",
                                "Vẽ đường mũi tên",
                                "Vẽ tam giác",
                                "Vẽ hình chữ nhật",
                                "Vẽ hình vòm",
                                "Vẽ Andrews's Pitchfork",
                                "",
                                "Vẽ Fibonacci Retracement",
                                "Vẽ Fibonacci Projection",
                                "Vẽ Fibonacci Time",
                                "Vẽ Fibonacci Arc",
                                "Vẽ Fibonacciv Fan"};

            ToolStripItem[] items = new ToolStripItem[icons.Length];
            int j = 0;
            xContainer tool = new xContainer();
            xButton bt;
            int bh = frameH;

            ImageList imglist = mContext.getImageList(C.IMG_TOOLSTRIPS, frameW, frameH);
            int x = 0;

            for (int i = 0; i < icons.Length; i++)
            {
                if (icons[i] == -1)
                {
                    //items[j ] = new ToolStripSeparator();
                }
                else
                {
                    bt = xButton.createImageButton(ids[i], this, imglist, icons[i]);
                    bt.setSize(30, 23);
                    bt.setPosition(x, 0);
                    tool.addControl(bt);

                    x = bt.getRight();
                    bh = bt.getH();
                    /*
                    items[j] = new ToolStripButton();
                    items[j].ImageIndex = icons[i];
                    items[j].Tag = (Int32)ids[i];
                    items[j].ToolTipText = tips[i];
                     */
                }

                j++;
            }
            tool.setSize(x, bh);
            /*
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
            */
            return tool;
        }

        ToolStrip createToolStrip()
        {
            int frameW = 23;
            int frameH = 20;
            //  line | candle | OHDC |*| 
            //  BB | PSAR | Ichi | SMA1 | SMA2 | SMA3 | * | 
            //  Trend/Retrace/Fan/Arc/Projection/Time | * | 
            //  + SubChart 
            int[] icons = { 16, -1, 
                            //0, 1, 29, 2, 18, -1, 
                            3, 30, 5, 4, 19, 17, 20, 6, 7,-1,
                            8, 31, 32, -1,
                            //9, 10, 11, 12, 13, 14, -1, 
                            15};
            int[] ids = { C.ID_TS_INFO, 0,
                            //C.ID_TS_CHARTLINE, C.ID_TS_CHARTCANDLE, C.ID_TS_CHARTCANDLE_HEIKEN, C.ID_TS_CHARTOHLC, C.ID_TS_CHARTHLC, 0,
                            C.ID_TS_BOLLINGER, C.ID_TS_ENVELOP, C.ID_TS_ICHIMOKU, C.ID_TS_PSAR, C.ID_TS_VSTOP, C.ID_TS_ZIGZAG, C.ID_TS_VOLUMEBYPRICE, C.ID_TS_SMA1, C.ID_TS_SMA2, 0,
                            C.ID_TS_COMPARE_2_SHARES, C.ID_TS_COMPARE_YEAR1, C.ID_TS_COMPARE_YEAR2, 0,
                            
                            //C.ID_TS_DRAW_TREND, C.ID_TS_DRAW_RETRACE, C.ID_TS_DRAW_PROJECT, C.ID_TS_DRAW_TIME, C.ID_TS_DRAW_ARC, C.ID_TS_DRAW_FAN,0,
                            C.ID_TS_ADD_SUBCHART};
            string[] tips = { "Thông tin", "",
                            //"Đồ thị line", "Đồ thị nến Nhật", "Đồ thị nến Heiken Ashi", "Đồ thị OHLC", "Đồ thị HLC", "", 
                            "Bollinger", "Envelops", "Ichimoku Kynko Hyo", "Parabolic SAR (Stop and Reverse)", "Volatility Stop Indicator", "Zigzaz", "Volume by Price", "Vẽ đường trung bình", "Vẽ đường trung bình", "",
                            "So sánh với cổ phiếu khác", "So sánh giá 1 năm trước", "So sánh giá 1 năm trước", "",
                            //"Vẽ trend", "Fibonacci Retracement", "Fibonacci Projection", "Fibonacci Time", "Fibonacci Arc", "Fibonacci Fan", "",
                            "Thêm chỉ báo"};

            ToolStripItem[] items = new ToolStripItem[icons.Length + 3];    //  2 sections
            int j = 0;
            for (int i = 0; i < icons.Length; i++)
            {
                if (icons[i] == -1)
                {
                    items[j] = new ToolStripButton();
                    items[j].ImageIndex = 0;
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

        virtual public void onEvent(object sender, int evt, int aIntParameter, object aParameter)
        {
            if (evt == xBaseControl.EVT_ON_MOUSE_DOUBLE_CLICK && sender != mChartMaster)
            {
                if (mZoomingChart == sender)
                {
                    mZoomingChart = null;
                }
                else
                    mZoomingChart = (xBaseControl)sender;

                adjustChartsSize();
            }

            if (evt == C.EVT_SUB_CHART_CONTAINER_CHANGED)
            {
                saveToFile();
            }

            if (evt == C.EVT_SHOW_TUTORIAL)
            {
                mListener.onEvent(sender, evt, aIntParameter, aParameter);
            }

            if (evt == C.EVT_FOCUS_AT_CURSOR)
            {
                int[] scopes = { Share.SCOPE_1WEEKS, Share.SCOPE_1MONTH, Share.SCOPE_3MONTHS, Share.SCOPE_6MONTHS, Share.SCOPE_1YEAR, Share.SCOPE_2YEAR, Share.SCOPE_5YEAR, Share.SCOPE_ALL };
                Share share = mShare;
                if (share != null)
                {
                    createChartRangeControls(share, mTimingRange);
                }
            }


            if (evt == xBaseControl.EVT_BUTTON_CLICKED)
            {
                Share share = mShare;
                //  daily/weekly/monthly
                if (aIntParameter == C.ID_WEEKLY_CHART && share != null)
                {
                    mChartType++;
                    if (mChartType > Share.CANDLE_MONTHLY)
                        mChartType = Share.CANDLE_DAILY;

                    reloadShare(share, true);
                    
                }
                //------------------------------

                if (aIntParameter >= C.ID_TS_DRAW_TREND_ARROW && aIntParameter <= C.ID_TS_DRAW_CLEAR_ALL)
                {
                    drawTrend(aIntParameter);
                }
                if (aIntParameter == C.ID_ADD_SUB_CHART)
                {
                    int idx = (Int32)aParameter;
                    addSubchart(null, idx);
                }
                if (aIntParameter == C.ID_REMOVE_SUB_CHART)
                {
                    SubchartsContainer rem = (SubchartsContainer)sender;
                    if (mSubCharts.size() > 1)
                    {
                        for (int i = 0; i < mSubCharts.size(); i++)
                        {
                            SubchartsContainer c = (SubchartsContainer)mSubCharts.elementAt(i);
                            if (c == rem)
                            {
                                mSubCharts.removeElement(c);
                                mSubContainer.removeControl(rem);
                                adjustChartsSize();
                                break;
                            }
                        }
                    }
                }
            }
            if (evt == xBaseControl.EVT_ON_SPLITTER_SIZE_CHANGED)
            {
                adjustChartsSize();
            }

            if (evt == C.EVT_REFRESH_SHARE_DATA)
            {
                refreshShareData();
            }

            if (evt == C.EVT_REPAINT_CHARTS)
            {
                mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
            }
            if (aIntParameter >= C.ID_CHART_RANGE && aIntParameter < C.ID_CHART_RANGE_END)
            {
                Share share = mShare;
                if (share != null)
                {
                    int idx = aIntParameter - C.ID_CHART_RANGE;
                    int scope = 0;
                    if (share.isRealtime())
                    {
                        int[] scopes = {Share.CANDLE_1MIN, Share.CANDLE_5MINS, 
                                            Share.CANDLE_10MINS, Share.CANDLE_15MINS, 
                                            Share.CANDLE_30MINS, Share.CANDLE_60MINS};
                        if (idx >= 0 && idx < scopes.Length)
                        {
                            scope = scopes[idx];
                            share.changeTimeFrameOfIntraday(scope);
                        }
                    }
                    else
                    {
                        int[] scopes = { Share.SCOPE_1WEEKS, Share.SCOPE_1MONTH, Share.SCOPE_3MONTHS, Share.SCOPE_6MONTHS, Share.SCOPE_1YEAR, Share.SCOPE_2YEAR, Share.SCOPE_5YEAR, Share.SCOPE_ALL };
                        if (idx >= 0 && idx < scopes.Length)
                        {
                            scope = scopes[idx];
                            if (mChartType == Share.CANDLE_DAILY)
                            {
                                mContext.mOptHistoryChartTimeFrame = scope;
                                mContext.saveOptions2();
                            }
                            share.setCursorScope(scope);
                        }
                    }
                    createChartRangeControls(share, mTimingRange);
                    mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
                }
            }
        }

        NetProtocol netRefreshShareData;
        void refreshShareData()
        {
            if (mShare == null || mShare.getID() == 0)
            {
                return;
            }

            GlobalData.vars().setValueInt("share_refreshing", 1);

            netRefreshShareData = mContext.createNetProtocol();
            netRefreshShareData.setListener(this);

            xVector ids = new xVector();
            ids.addElement(mShare.getID());
            netRefreshShareData.requestGetPriceboard(ids);

            netRefreshShareData.onDoneDelegate = (sender, ok) =>
            {
                GlobalData.vars().setValueInt("share_refreshing", 0);

                reloadShare(mShare, true);
            };

            netRefreshShareData.flushRequest();
        }

        public void reloadShare(Share share, bool applyTodayCandle)
        {
            int scope = share.getCursorScope();
            int endDate = share.getEndDate();

            /*
            share.loadShareFromFile(true);
            if (share.getCandleCount() == 0)
                share.loadShareFromCommonData(true);
            */

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

            if (mChartType == Share.CANDLE_WEEKLY)
                share.toWeekly();
            else if (mChartType == Share.CANDLE_MONTHLY)
                share.toMonthly();

            share.setEndDate(endDate);
            share.setCursorScope(scope);

            createChartRangeControls(share, mTimingRange);

            mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, null);
        }

        void adjustChartsSize()
        {
            if (mZoomingChart != null)
            {
                bool hasZooming = false;
                for (int i = 0; i < mSubCharts.size(); i++)
                {
                    if (mSubCharts.elementAt(i) == mZoomingChart)
                    {
                        hasZooming = true;
                        break;
                    }
                }
                if (!hasZooming)
                    mZoomingChart = null;
            }
            if (mSubCharts.size() > 0)
            {
                Share share = mShare;
    
                //  adjust master chart position first
                if (mTimingRange != null)
                    mTimingRange.setPosition((CHART_W - mTimingRange.getW()) / 2, mMainContainer.getH() - mTimingRange.getH() - 16);
                mChartMaster.setSize(mMainContainer);

                //======================================================
                //  now adjust sub charts
                int subHTotal = getH() - mMainContainer.getH() - 2;

                int items = mSubCharts.size();
                int zoomTimes = 3;
                if (mZoomingChart != null)
                    items += (zoomTimes-1);
                int h = subHTotal / items;
                h -= 1;

                int y = 0;// mChartMaster.getBottom() + 2;
                for (int i = 0; i < mSubCharts.size(); i++)
                {
                    xBaseControl c = (xBaseControl)mSubCharts.elementAt(i);
                    if (c == mZoomingChart)
                    {
                        c.setSize(c.getW(), zoomTimes * h);
                    }
                    else
                    {
                        c.setSize(c.getW(), h);
                    }
                    c.setPosition(0, y);
                    y = c.getBottom()+1;
                }

                //  resize sub container
                mSubContainer.setSize(CHART_W, subHTotal);
                mSubContainer.setPosition(0, 0);
                mSubContainer.invalidate();

                saveToFile();
            }
            else
            {
                //mSplitter.setSplitterDistance(mRightPanel.getH());
            }
        }

        protected xContainer createChartRangeControls(Share share, xContainer c)
        {
            int currentRange = share.getCursorScope();

            if (share.isRealtime())
            {
                return createChartRangeControlsRT(share.mCandleType, c);
            }

            c.removeAllControls();

            xLabel l;
            //  5 days, 1 month, 3 month, 6 month, 1 year, 2 year
            String[] ss = { "5d", "1m", "3m", "6m", "1y", "2y", "5y", "-", null };
            int[] scopes = { Share.SCOPE_1WEEKS, Share.SCOPE_1MONTH, Share.SCOPE_3MONTHS, Share.SCOPE_6MONTHS, Share.SCOPE_1YEAR, Share.SCOPE_2YEAR, Share.SCOPE_5YEAR, Share.SCOPE_ALL };
            int i = 0;

            int w = 10;
            int h = 20;
            int x = 0;
            int sel = 0;
            //  update sel
            if (currentRange > 0)
            {
                for (i = 0; i < scopes.Length; i++)
                {
                    if (currentRange >= scopes[i])
                    {
                        sel = i;
                    }
                    else
                        break;
                }
            }
            if (currentRange > Share.SCOPE_5YEAR)
                sel = scopes.Length - 1;
            //==========================
            i = 0;
            while (ss[i] != null)
            {
                l = xLabel.createSingleLabel(ss[i], mContext.getFontSmallest(), 25);

                l.setPosition(x, 0);
                l.setAlign(xGraphics.HCENTER);
                l.enableClick(C.ID_CHART_RANGE + i, this);
                c.addControl(l);
                if (i == sel)
                {
                    l.setBackgroundColor(C.COLOR_GRAY_DARK);
                    l.setTextColor(C.COLOR_WHITE);
                }
                else
                {
                    l.setBackgroundColor(C.COLOR_GRAY_LIGHT);
                    l.setTextColor(C.COLOR_BLACK);
                }

                x = l.getRight() + 1;
                h = l.getH();

                i++;
            }
            //=======weekly button=======
            string charttype = "daily";
            if (mChartType == Share.CANDLE_DAILY)
                charttype = "daily";
            else if (mChartType == Share.CANDLE_WEEKLY)
                charttype = "weekly";
            else
                charttype = "monthly";
            l = xLabel.createSingleLabel(charttype, mContext.getFontSmallest(), 60);
            l.setPosition(x, 0);
            l.setAlign(xGraphics.HCENTER);
            l.enableClick(C.ID_WEEKLY_CHART, this);

            x += l.getW();
            l.setBackgroundColor(C.COLOR_GRAY_DARK);
            l.setTextColor(C.COLOR_ORANGE);
            c.addControl(l);
            //===========================
            w = x;
            c.setSize(w, h);
            c.setOpaque(0.7f);

            return c;
        }

        protected xContainer createChartRangeControlsRT(int candleType, xContainer c)
        {
            Share share = mShare;
            c.removeAllControls();

            xLabel l;
            //  1 min, 5 mins, 10 mins, 15 mins, 30 mins, 60mins
            String[] ss = { "1 min", "5m", "10m", "15m", "30m", "60m", null };
            int[] scopes = { Share.CANDLE_1MIN, Share.CANDLE_5MINS, 
                Share.CANDLE_10MINS, Share.CANDLE_15MINS, 
                Share.CANDLE_30MINS, Share.CANDLE_60MINS};
            int i = 0;

            int w = 10;
            int h = 20;
            int x = 0;
            int sel = 0;
            //  update sel
            if (candleType > 0)
            {
                for (i = 0; i < scopes.Length; i++)
                {
                    if (candleType >= scopes[i])
                    {
                        sel = i;
                    }
                    else
                        break;
                }
            }
            if (candleType > Share.CANDLE_60MINS)
                sel = 0;
            //==========================
            i = 0;
            while (ss[i] != null)
            {
                l = xLabel.createSingleLabel(ss[i], mContext.getFontSmallest(), 40);

                l.setPosition(x, 0);
                l.setAlign(xGraphics.HCENTER);
                l.enableClick(C.ID_CHART_RANGE + i, this);
                c.addControl(l);
                if (i == sel)
                {
                    l.setBackgroundColor(C.COLOR_GRAY_DARK);
                    l.setTextColor(C.COLOR_WHITE);
                }
                else
                {
                    l.setBackgroundColor(C.COLOR_GRAY_LIGHT);
                    l.setTextColor(C.COLOR_BLACK);
                }

                x = l.getRight() + 1;
                h = l.getH();

                i++;
            }
            //===========================
            w = x;
            c.setSize(w, h);
            c.setOpaque(0.7f);

            return c;
        }

        public ChartMaster createMasterChart(int type, int w, int h)
        {
            ChartMaster cl = new ChartMaster(mContext.getFontSmall());
            cl.setIsMasterChart(true);
            cl.setSize(w, h);
            cl.setPosition(0, 0);

            cl.mShouldDrawCursor = true;
            cl.mShouldDrawDateSeparator = true;
            cl.mShouldDrawPriceLine = true;
            cl.mShouldDrawValue = true;

            cl.mHasFibonacci = true;

            cl.allowRepositionCursor();
            cl.setListener(this);
            if (type == ChartBase.CHART_LINE
                || type == ChartBase.CHART_CANDLE
                || type == ChartBase.CHART_OHLC
                || type == ChartBase.CHART_HLC
                || type == ChartBase.CHART_CANDLE_HEIKEN)
            {
            }
            else
            {
                type = ChartBase.CHART_LINE;
            }
           
            cl.setChartType(type);

            return cl;
        }

        Share mCurrentShare = null;
        public void invalidateCharts()
        {
            if (mCurrentShare != mShare && mTimingRange != null)
            {
                mCurrentShare = mShare;
                if (mChartType == Share.CANDLE_MONTHLY)
                {
                    createChartRangeControls(mShare, mTimingRange);
                }
                else
                {
                    createChartRangeControls(mShare, mTimingRange);
                }
            }
            mChartMaster.invalidate();

            for (int i = 0; i < mSubCharts.size(); i++)
            {
                xBaseControl c = (xBaseControl)mSubCharts.elementAt(i);
                c.invalidate();
            }
        }

        void showInfo(Share share)
        {
            mInfo.removeAllControls();
            mInfo.show(true);
            if (share == null || share.isIndex())
            {
                mInfo.setSize(mInfo.getW(), 0);
                return;
            }

            CompanyInfo inf = new CompanyInfo(share.mID, false);
            inf.dontShowName();
            inf.setFont(mContext.getFontSmall());
            //inf.setFontColor(C.COLOR_ORANGE);
            inf.setSize(mInfo);
            inf.init();
            mInfo.setSize(inf);
            mInfo.addControl(inf);
            mShowingInfo = true;
        }

        void tool_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                ToolStripItem item = e.ClickedItem;

                int idx = (Int32)item.Tag;

                processToolStripEvent(e, idx);
                saveToFile();
            }
            catch (Exception ex)
            {
            }
        }

        void processToolStripEvent(ToolStripItemClickedEventArgs e, int idx)
        {
            Share share = mShare;
            if (share == null)
                return;
            if (idx == C.ID_TS_SMA1 || idx == C.ID_TS_SMA2)
            {
                int sma = idx - C.ID_TS_SMA1;
                if (mChartMaster.isSMAOn())
                {
                    mChartMaster.toggleSMA(sma);
                }
                else
                {
                    DlgSMA dlg = new DlgSMA(sma);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (mShare != null)
                            mShare.mIsCalcSMA = false;
                        mContext.saveOptions();
                        mContext.saveOptions2();
                        mChartMaster.toggleSMA(sma);
                    }
                }
            }
            if (idx == C.ID_TS_INFO)
            {
                if (mShowingInfo)
                {
                    mShowingInfo = false;
                    mInfo.show(false);
                }
                else
                {
                    showInfo(share);
                }
            }
            if (idx == C.ID_TS_CHARTLINE)
            {
                mChartMaster.setChartType(ChartLine.CHART_LINE);
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();

                saveToFile();
            }
            else if (idx == C.ID_TS_CHARTCANDLE)
            {
                mChartMaster.setChartType(ChartLine.CHART_CANDLE);
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();

                saveToFile();
            }
            else if (idx == C.ID_TS_CHARTCANDLE_HEIKEN)
            {
                mChartMaster.setChartType(ChartLine.CHART_CANDLE_HEIKEN);
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();

                saveToFile();
            }
            else if (idx == C.ID_TS_CHARTOHLC)
            {
                mChartMaster.setChartType(ChartLine.CHART_OHLC);
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();

                saveToFile();
            }
            else if (idx == C.ID_TS_CHARTHLC)
            {
                mChartMaster.setChartType(ChartLine.CHART_HLC);
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();

                saveToFile();
            }
            //=============================================
            else if (idx == C.ID_TS_BOLLINGER)
            {
                mChartMaster.toggleAttachChart(ChartBase.CHART_BOLLINGER);
            }
            else if (idx == C.ID_TS_ENVELOP)
            {
                mChartMaster.toggleAttachChart(ChartBase.CHART_ENVELOP);
            }
            else if (idx == C.ID_TS_PSAR)
            {
                mChartMaster.toggleAttachChart(ChartBase.CHART_PSAR);
            }
            else if (idx == C.ID_TS_VSTOP)
            {
                mChartMaster.toggleAttachChart(ChartBase.CHART_VSTOP);
            }
            else if (idx == C.ID_TS_ICHIMOKU)
            {
                mChartMaster.toggleAttachChart(ChartBase.CHART_ICHIMOKU);
            }
            else if (idx == C.ID_TS_ZIGZAG)
            {
                mChartMaster.toggleAttachChart(ChartBase.CHART_ZIGZAG);
            }
            else if (idx == C.ID_TS_VOLUMEBYPRICE)
            {
                mChartMaster.toggleAttachChart(ChartBase.CHART_VOLUMEBYPRICE);
            }
            else if (idx == C.ID_TS_COMPARE_YEAR1 || idx == C.ID_TS_COMPARE_YEAR2)
            {
                int type = ChartBase.CHART_PAST_1_YEAR;
                if (idx == C.ID_TS_COMPARE_YEAR2)
                    type = ChartBase.CHART_PAST_2_YEARS;
                mChartMaster.toggleAttachChart(type);
                if (type == ChartBase.CHART_PAST_1_YEAR)
                {
                    mShare.mIs1YearChartOn = mChartMaster.isAttachedOn(type);
                }
                else
                {
                    mShare.mIs2YearChartOn = mChartMaster.isAttachedOn(type);
                }
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();
                
            }
            else if (idx == C.ID_TS_COMPARE_2_SHARES)
            {
                if (mChartMaster.isAttachedOn(ChartBase.CHART_COMPARING_SECOND_SHARE))
                {
                    mChartMaster.hideAttachChart(ChartBase.CHART_COMPARING_SECOND_SHARE);

                    

                    mChartMaster.clearModifyKey();
                    mChartMaster.invalidate();
                }
                else
                {
                    showCompareWithShareDialog();
                }
            }
            else if (idx == C.ID_TS_ADD_SUBCHART)
            {
                //addSubchartUnder(null
                if (mContext.isExpiredAccount())
                {
                    int[] ids = {
                                    -1000,  //ChartBase.CHART_ADL,
                            -1000,  //ChartBase.CHART_ADX,
                            -1000,
                            -1000,  //ChartBase.CHART_CHAIKIN,
                            -1000,
                            ChartBase.CHART_MFI,
                            ChartBase.CHART_MACD,
                            -1000,  //ChartBase.CHART_NVI,
                            -1000,  //ChartBase.CHART_PVI,
                            -1000,  //ChartBase.CHART_OBV,
                            -1000,  //ChartBase.CHART_ROC,
                            -1000,  //  PVT
                            ChartBase.CHART_RSI,
                                            ChartBase.CHART_CRS_RATIO,
                //ChartBase.CHART_CRS_PERCENT,
                            -1000,  //ChartBase.CHART_STOCHASTIC_FAST,
                            -1000,  //ChartBase.CHART_STOCHASTIC_SLOW,
                            -1000,  //ChartBase.CHART_STOCHRSI,
                            -1000,  //ChartBase.CHART_TRIX,
                            ChartBase.CHART_VOLUME
                };

                    string[] ss = {
                            "******Accumulation Distribution Line (ADL)",
                            "******Average Directional Index (ADX)",
                            "******Commodity Channel Index (CCI)",
                            "******Chaikin Oscillator",
                            "******Mass Index (Mass)",
                            "Money Flow Index (MFI)",
                            "******Moving Average Convergence-Divergence (MACD)",
                            "******Negative Volume Index (NVI)",
                            "******Positive Volume Index (PVI)",
                            "******On Balance Volume (OBV)",
                            "******Rate of Change (ROC)",
                            "******Price Volume Trend",
                            "Relative Strength Index (RSI)",
                                                        "Relative Strength (RS=A/B) Comparative",
                            //"Relative Price performance (RS=[A/A(period)]/[B/B(period)])",
                            "******Stochastic Fast",
                            "******Stochastic Slow",
                            "******StochRSI",
                            "******Triple Exponential Average (TRIX)",
                            "Volume"
                };
                    ContextMenu menu = createMenuContext(ids, ss, ids.Length, onAddSubChartMenu);
                    int x = e.ClickedItem.Bounds.X + e.ClickedItem.Bounds.Width + 3;
                    int y = e.ClickedItem.Bounds.Y;
                    menu.Show(getControl(), new Point(x, y));
                }
                else
                {
                    int[] ids = {
                    ChartBase.CHART_AROON,
                    ChartBase.CHART_AROON_OSCILLATOR,
                    ChartBase.CHART_ATR,
                ChartBase.CHART_ADL,
                ChartBase.CHART_ADX,
                ChartBase.CHART_CCI,
                    ChartBase.CHART_CFM,
                ChartBase.CHART_CHAIKIN,
                ChartBase.CHART_MASSINDEX,
                ChartBase.CHART_MFI,
                ChartBase.CHART_MACD,
                ChartBase.CHART_NVI,
                ChartBase.CHART_PVI,
                ChartBase.CHART_OBV,
                ChartBase.CHART_ROC,
                ChartBase.CHART_PVT,
                ChartBase.CHART_RSI,
                                ChartBase.CHART_CRS_RATIO,
                //ChartBase.CHART_CRS_PERCENT,
                ChartBase.CHART_STOCHASTIC_FAST,
                ChartBase.CHART_STOCHASTIC_SLOW,
                ChartBase.CHART_STOCHRSI,
                ChartBase.CHART_TRIX,
                    ChartBase.CHART_WILLIAMR,
                ChartBase.CHART_VOLUME
                };

                    string[] ss = {
                            "Aroon",
                            "Aroon Oscillator",
                            "Average true range (ATR)",
                            "Accumulation Distribution Line (ADL)",
                            "Average Directional Index (ADX)",
                            "Commodity Channel Index (CCI)",
                            "Chaikin Money Flow (CMF)",
                            "Chaikin Oscillator",
                            "Mass Index (Mass)",
                            "Money Flow Index (MFI)",
                            "Moving Average Convergence-Divergence (MACD)",
                            "Negative Volume Index (NVI)",
                            "Positive Volume Index (PVI)",
                            "On Balance Volume (OBV)",
                            "Rate of Change (ROC)",
                            "Price Volume Trend (PVT)",
                            "Relative Strength Index (RSI)",
                                                        "Relative Strength Comparative (RS=A/B)",
                            //"Relative Price performance (RS=[A/A(period)]/[B/B(period)])",
                            "Stochastic Fast",
                            "Stochastic Full",
                            "StochRSI",
                            "Triple Exponential Average (TRIX)",
                            "William %R",
                            "Volume"
                };
                    ContextMenu menu = createMenuContext(ids, ss, ids.Length, onAddSubChartMenu);
                    int x = e.ClickedItem.Bounds.X + e.ClickedItem.Bounds.Width + 3;
                    int y = e.ClickedItem.Bounds.Y;
                    menu.Show(getControl(), new Point(x, y));
                }
            }
        }

        void drawTrend(int trend)
        {
            int idx = trend;

            Drawer drawer = mChartMaster.getDrawer();

            //=============================================
            if (idx == C.ID_TS_DRAW_CLEAR_ALL)
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                if (MessageBox.Show("Xóa tất cả các hình vẽ trên đồ thị?", "Xóa hình vẽ", buttons)
                    == DialogResult.Yes)
                {
                    mChartMaster.getDrawer().clearAll();
                }
                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_TREND)
            {
                /*
                uint a = C.BLUE_LINE_COLOR;
                DlgLineColorPicker dlg = new DlgLineColorPicker((int)mContext.mOptFiboTrendColor, mContext.mOptFiboTrendThinkness);
                dlg.ShowDialog();

                mContext.mOptFiboTrendColor = (uint)dlg.getColor();
                mContext.mOptFiboTrendThinkness = dlg.getThickness();
                mDrawer.addTrends(Drawer.DRAW_TREND, dlg.getColor(), dlg.getThickness());
                 */
                drawer.addTrends(Drawer.DRAW_TREND, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_TREND_ARROW)
            {
                drawer.addTrends(Drawer.DRAW_TREND_ARROW, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_RETRACE)
            {
                drawer.addTrends(Drawer.DRAW_FIBO_RETRACEMENT, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_PROJECT)
            {
                drawer.addTrends(Drawer.DRAW_FIBO_PROJECTION, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_TIME)
            {
                drawer.addTrends(Drawer.DRAW_FIBO_TIME, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_FAN)
            {
                drawer.addTrends(Drawer.DRAW_FIBO_FAN, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_ARC)
            {
                drawer.addTrends(Drawer.DRAW_FIBO_ARC, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_TRIANGLE)
            {
                drawer.addTrends(Drawer.DRAW_TRIANGLE, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_RECTANGLE)
            {
                drawer.addTrends(Drawer.DRAW_RECTANGLE, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_ANDREWS_PITCHFORK)
            {
                drawer.addTrends(Drawer.DRAW_ANDREWS_PITCHFORK, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_ABC)
            {
                DlgAddText dlg = new DlgAddText();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string text = dlg.getText();
                    if (text != null && text.Length > 0)
                    {
                        drawer.addText(text);
                        drawer.show(true);

                        mChartMaster.invalidate();
                    }
                }
            }
            else if (idx == C.ID_TS_DRAW_ECLIPSE)
            {
                drawer.addTrends(Drawer.DRAW_OVAL, 0, 0);
                drawer.show(true);

                mChartMaster.invalidate();
            }
        }
        /*
        void onComparePriceWithPastYears(object sender, EventArgs e)
        {
            try
            {
                MenuItem item = (MenuItem)sender;

                int type = (int)item.Tag;
                mChartMaster.toggleAttachChart(type);
            }
            catch (Exception ex)
            {
            }
        }
         */

        void onAddSubChartMenu(object sender, EventArgs e)
        {
            try
            {
                MenuItem item = (MenuItem)sender;

                addSubchart(null, (int)item.Tag);

                saveToFile();
            }
            catch (Exception ex)
            {
            }
        }

        void addSubchart(xBaseControl sender, int chartID)
        {
            if (chartID == -1000)
            {
                MessageBox.Show("Tài khoản của bạn đã hết hạn, hãy gia hạn tài khoản để sử dụng chức năng này.");
                return;
            }

            if (chartID == -1)
                chartID = ChartBase.CHART_VOLUME;

            xBaseControl c;

            c = createSubchart(chartID);
            mSubCharts.addElement(c);
            mSubContainer.addControl(c);
            adjustChartsSize();
        }
        SubchartsContainer createSubchart(int subchart)
        {
            SubchartsContainer sub = new SubchartsContainer(0, mShare, this, true);
            sub.setChartMaster(mChartMaster);
            sub.setSize(CHART_W, 10);
            sub.setPosition(0, sub.getBottom() + 2);
            sub.setChart(subchart);

            return sub;
        }

        public override void setSize(int w, int h)
        {
            base.setSize(w, h);

            if (mMainContainer == null || mSubContainer == null)
                return;

            CHART_W = getW() - CHART_X0;
            mSplitter.setSize(CHART_W, h);
            mMainContainer.setSize(CHART_W, mMainContainer.getH());
            mChartMaster.setSize(mMainContainer);

            if (mTimingRange != null)
                mTimingRange.setPosition((CHART_W - mTimingRange.getW()) / 2, mMainContainer.getH() - mTimingRange.getH() - 16);

            mSubContainer.setSize(w, mSubContainer.getH());
            for (int i = 0; i < mSubCharts.size(); i++)
            {
                xBaseControl c = (xBaseControl)mSubCharts.elementAt(i);
                c.setSize(CHART_W, c.getH());
            }

            invalidateCharts();

            saveToFile();
        }

        public void saveToFile()
        {
            xDataOutput o = new xDataOutput(200);
            o.writeInt(Context.FILE_VERSION);
            
            o.writeInt(mMainContainer.getH());
            mChartMaster.flush(o);

            o.writeInt(mSubCharts.size());
            for (int i = 0; i < mSubCharts.size(); i++)
            {
                SubchartsContainer c = (SubchartsContainer)mSubCharts.elementAt(i);
                o.writeInt(c.getSubchartID());
                o.writeBoolean(c.hasGrid());
            }

            xFileManager.saveFile(o, "data\\" + mName);
        }

        public override bool onMenuEvent(MenuItem item)
        {
            int idx = (int)item.Tag;

            if (idx == C.ID_TS_CHARTLINE)
            {
                mChartMaster.setChartType(ChartLine.CHART_LINE);
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();

                saveToFile();
            }
            else if (idx == C.ID_TS_CHARTCANDLE)
            {
                mChartMaster.setChartType(ChartLine.CHART_CANDLE);
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();

                saveToFile();
            }
            else if (idx == C.ID_TS_CHARTCANDLE_HEIKEN)
            {
                mChartMaster.setChartType(ChartLine.CHART_CANDLE_HEIKEN);
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();

                saveToFile();
            }
            else if (idx == C.ID_TS_CHARTOHLC)
            {
                mChartMaster.setChartType(ChartLine.CHART_OHLC);
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();

                saveToFile();
            }
            else if (idx == C.ID_TS_CHARTHLC)
            {
                mChartMaster.setChartType(ChartLine.CHART_HLC);
                mChartMaster.clearModifyKey();
                mChartMaster.invalidate();

                saveToFile();
            } 
            else if (idx == C.ID_TS_DRAW_RETRACE)
            {
                mDrawer.addTrends(Drawer.DRAW_FIBO_RETRACEMENT, 0, 0);
                mDrawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_PROJECT)
            {
                mDrawer.addTrends(Drawer.DRAW_FIBO_PROJECTION, 0, 0);
                mDrawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_TIME)
            {
                mDrawer.addTrends(Drawer.DRAW_FIBO_TIME, 0, 0);
                mDrawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_FAN)
            {
                mDrawer.addTrends(Drawer.DRAW_FIBO_FAN, 0, 0);
                mDrawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_TREND)
            {
                MessageBox.Show("Giữ phím Ctrl + Bấm và rê phím chuột trái");
            }
            else if (idx == C.ID_TS_DRAW_ARC)
            {
                mDrawer.addTrends(Drawer.DRAW_FIBO_ARC, 0, 0);
                mDrawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_TRIANGLE)
            {
                mDrawer.addTrends(Drawer.DRAW_TRIANGLE, 0, 0);
                mDrawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_RECTANGLE)
            {
                mDrawer.addTrends(Drawer.DRAW_RECTANGLE, 0, 0);
                mDrawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_ANDREWS_PITCHFORK)
            {
                mDrawer.addTrends(Drawer.DRAW_ANDREWS_PITCHFORK, 0, 0);
                mDrawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_ABC)
            {
                mDrawer.addTrends(Drawer.DRAW_ABC, 0, 0);
                mDrawer.show(true);

                mChartMaster.invalidate();
            }
            else if (idx == C.ID_TS_DRAW_ECLIPSE)
            {
                mDrawer.addTrends(Drawer.DRAW_OVAL, 0, 0);
                mDrawer.show(true);

                mChartMaster.invalidate();
            }
            else{
                return base.onMenuEvent(item);
            }

            return false;
        }

        xBaseControl mDrawingToolStrip;
        public void toogleDrawingTool()
        {
            if (mDrawingToolStrip.isShow())// getCotrol().Visible)
            {
                mDrawingToolStrip.show(false);
            }
            else
            {
                mDrawingToolStrip.show(true);
            }
        }

        public bool hasDrawing()
        {
            if (mDrawingToolStrip != null)
                return mDrawingToolStrip.isShow();

            return false;
        }

        public ChartMaster getMasterChart()
        {
            return mChartMaster;
        }

        public void onChangedQuote()
        {
            if (mShowingInfo)
                showInfo(mShare);

            invalidateCharts();
        }

        void showCompareWithShareDialog()
        {
            DlgCompareToShare dlg = new DlgCompareToShare();
            dlg.setShareCode("");
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                String code = dlg.getShareCode();

                if (mContext.mShareManager.getShareID(code) <= 0)
                {
                    MessageBox.Show("Mã cổ phiếu không tồn tại");
                }
                else
                {
                    //mContext.mComparingShareCode = code.ToUpper();
                    String mCompare2ShareCode = code.ToUpper();
                    mShare.clearCalculations();

                    //mChartMaster.showAttachChart(ChartBase.CHART_COMPARING_SECOND_SHARE);
                    mChartMaster.compareToChart(mCompare2ShareCode);

                    mChartMaster.clearModifyKey();
                    mChartMaster.invalidate();
                }
            }
        }

        public void setShare(Share share)
        {
            mShare = share;

            if (mChartMaster == null)
            {
                return;
            }
            //  
            for (int i = 0; i < mSubCharts.size(); i++)
            {
                SubchartsContainer c = (SubchartsContainer)mSubCharts.elementAt(i);
                c.setShare(share);
                c.invalidate();
            }

            mChartMaster.setShare(share);
            mChartMaster.invalidate();
        }


    }
}
