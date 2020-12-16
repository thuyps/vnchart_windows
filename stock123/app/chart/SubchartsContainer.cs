using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

using xlib.framework;
using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;
using stock123.app.ui;

namespace stock123.app.chart
{
    public class SubchartsContainer: xContainer, xIEventListener
    {
        //ChartBase mChart;   //  for reference
        ChartBase mCurrentChart;
        bool mHasGrid = false;
        int mChartIdx;
        //xButton mSwitchButton;
        //xButton mAddButton;
        //xButton mRemoveButton;
        xButton mSettingButton;
        xButton mDrawGridButton;
        xButton mSecondChartButton;
        xButton mHelpButton;

        Share mShare;
        //=================================

        public SubchartsContainer(int idx, Share share, xIEventListener listener, bool add_removable)
            : base(listener)
        {
            setID(idx);

            mShare = share;

            mCurrentChart = null;
            //mSwitchButton = null;

            mChartIdx = 0;

            if (Context.getInstance().isExpiredAccount())
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
                -1000,  //ChartBase.CHART_STOCHASTIC_FAST,
                -1000,  //ChartBase.CHART_STOCHASTIC_SLOW,
                -1000,  //ChartBase.CHART_STOCHRSI,
                -1000,  //ChartBase.CHART_TRIX,
                ChartBase.CHART_VOLUME,
                //ChartBase.CHART_THUYPS,
                -1,
                C.ID_REMOVE_SUB_CHART};
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
                            "******Stochastic Fast",
                            "******Stochastic Slow",
                            "******StochRSI",
                            "******Triple Exponential Average (TRIX)",
                            "Volume",
                //"ThuyPS",
                            "",
                            "Đóng đồ thị (Thêm: Ctrl + chọn đồ thị)",};

                int cnt = ids.Length;
                if (!add_removable)
                    cnt -= 2;
                setMenuContext(ids, ss, cnt);
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
                ChartBase.CHART_STOCHASTIC_FAST,
                ChartBase.CHART_STOCHASTIC_SLOW,
                ChartBase.CHART_STOCHRSI,
                ChartBase.CHART_TRIX,
                    ChartBase.CHART_WILLIAMR,
                ChartBase.CHART_VOLUME,
                //ChartBase.CHART_THUYPS,
                -1,
                C.ID_REMOVE_SUB_CHART};
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
                            "Stochastic Fast",
                            "Stochastic Full",
                            "StochRSI",
                            "Triple Exponential Average (TRIX)",
                            "William %R",
                            "Volume",
                //"ThuyPS",
                            "",
                            "Đóng đồ thị (Thêm: Ctrl + chọn đồ thị)",};

                int cnt = ids.Length;
                if (!add_removable)
                    cnt -= 2;
                setMenuContext(ids, ss, cnt);
            }
        }

        override public bool onMenuEvent(MenuItem item)
        {
            int idx = (int)item.Tag;

            if (idx == C.ID_REMOVE_SUB_CHART)
            {
                mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, idx, null);
            }
            else
            {
                //if (idx >= 0 && idx < 20)
                if (idx == -1000)
                {
                    MessageBox.Show("Tài khoản của bạn đã hết hạn, hãy gia hạn tài khoản để sử dụng chức năng này.");
                }
                else{
                    if (isKeyPressing(System.Windows.Forms.Keys.Control))
                    {
                        mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, C.ID_ADD_SUB_CHART, (Int32)idx);
                    }
                    else
                    {
                        setChart(idx);
                    }
                }
            }
            if (mListener != null)
                mListener.onEvent(this, C.EVT_SUB_CHART_CONTAINER_CHANGED, 0, null);
            return true;
        }

        public void setChart(ChartBase c)
        {    
            //removeControl(mSwitchButton);
            c.mShouldDrawGrid = mHasGrid;

            removeAllControls();
            //====================================

            if (mCurrentChart != null)
            {
                removeControl(mCurrentChart);
            }

            //  chart
            //addControl(c);
            mCurrentChart = c;
            mCurrentChart.setListener(this);

            Context ctx = Context.getInstance();    
    
            ImageList imgs = ctx.getImageList(C.IMG_SUB_BUTTONS, 16, 15);
            int x = 0;
            int y = 0;
            //  setting
            if (mSettingButton == null)
            {
                mSettingButton = xButton.createImageButton(C.ID_BUTTON_SETTING_CHART, this, imgs, 0);

                mSettingButton.setPosition(0, (getH() - mSettingButton.getH()) - 1);

                x = mSettingButton.getRight();
            }
            addControl(mSettingButton);
            y = (getH() - mSettingButton.getH()) - 1;
            //  grid
            if (mDrawGridButton == null)
            {
                mDrawGridButton = xButton.createImageButton(C.ID_BUTTON_DRAW_GRID, this, imgs, 4);
                mDrawGridButton.setPosition(x, y);
                x = mDrawGridButton.getRight();
            }
            addControl(mDrawGridButton);
            //  sma
            if (c.isSupportSecondChart())
            {
                if (mSecondChartButton == null)
                {
                    mSecondChartButton = xButton.createImageButton(C.ID_BUTTON_SECOND_CHART, this, imgs, 1);

                    mSecondChartButton.setPosition(x, y);
                    x = mSecondChartButton.getRight();
                }
                addControl(mSecondChartButton);
            }
            //  help
            bool hasHelp = false;
            int chartType = c.getChartType();
            if (chartType == ChartBase.CHART_ADL
                || chartType == ChartBase.CHART_ADX
                || chartType == ChartBase.CHART_CCI
                || chartType == ChartBase.CHART_CHAIKIN
                || chartType == ChartBase.CHART_ICHIMOKU
                || chartType == ChartBase.CHART_MACD
                || chartType == ChartBase.CHART_MFI
                || chartType == ChartBase.CHART_OBV
                || chartType == ChartBase.CHART_ROC
                || chartType == ChartBase.CHART_NVI
                || chartType == ChartBase.CHART_PVI
                || chartType == ChartBase.CHART_PVT
                || chartType == ChartBase.CHART_CCI
                || chartType == ChartBase.CHART_MASSINDEX
                || chartType == ChartBase.CHART_RSI
                || chartType == ChartBase.CHART_STOCHASTIC_FAST
                || chartType == ChartBase.CHART_STOCHASTIC_SLOW
                || chartType == ChartBase.CHART_STOCHRSI
                || chartType == ChartBase.CHART_TRIX
                || chartType == ChartBase.CHART_WILLIAMR
                || chartType == ChartBase.CHART_VOLUMEBYPRICE)
                hasHelp = true;

            if (mHelpButton == null)
            {
                mHelpButton = xButton.createImageButton(C.ID_BUTTON_CONTEXT_HELP, this, imgs, 3);
                mHelpButton.setPosition(0, y - mHelpButton.getH());
            }
            if (hasHelp)
                addControl(mHelpButton);
             /*
            if (mRemoveButton == null)
            {
                //mAddButton = xButton.createStandardButton(Constants.ID_ADD_SUB_CHART, this, " + ", 30);
                //mAddButton.setPosition(mSwitchButton.getRight(), mSwitchButton.getY());
                mRemoveButton = xButton.createStandardButton(C.ID_REMOVE_SUB_CHART, this, " - ", 30);
                mRemoveButton.setPosition(0, getH() - mRemoveButton.getH()-1);
                //mRemoveButton.setPosition(mSwitchButton.getRight(), mSwitchButton.getY());
            }*/

            //addControl(mAddButton);
            //addControl(mRemoveButton);

            addControl(c);
    
            //  context menu here
            /*
            //  help
            img = ctx->getImage("help0.png");
            img1 = ctx->getImage("help1.png");    
            xUIButton *bt = xUIButton::createImageButton(ID_BUTTON_CONTEXT_HELP, this, 0, img, img1);
            bt->setPosition(0, mSwitchButton->getY() - mSwitchButton ->getH());
            addControl(bt);
             */
        }
    
        public void setChart(int chartIdx)
        {
            ChartBase c = createSubChart(chartIdx);
            if (c != null)
            {
                mChartIdx = chartIdx;
                c.setSize(getW(), getH());
                setChart(c);
            }
        }

        override public void invalidate()
        {
            base.invalidate();

            if (mCurrentChart != null)
                mCurrentChart.invalidate();
        }

        virtual public void onEvent(Object sender, int aEvent, int aIntParameter, Object aParameter)
        {
            if (aEvent == C.EVT_REPAINT_CHARTS)
            {
                mListener.onEvent(this, aEvent, aIntParameter, null);
            }
            if (aEvent == xBaseControl.EVT_ON_MOUSE_DOUBLE_CLICK)
            {
                mListener.onEvent(this, xBaseControl.EVT_ON_MOUSE_DOUBLE_CLICK, 0, null);
            }
            if (aEvent == xBaseControl.EVT_BUTTON_CLICKED)
            {
                if (aIntParameter == C.ID_BUTTON_SETTING_CHART)
                {
                    FormSettingParameters dlg = new FormSettingParameters(mShare, this);
                    dlg.setOptionIDX(0);
                    dlg.setCurrentChart(getSubchartID());
                    dlg.ShowDialog();
                }
                if (aIntParameter == C.ID_BUTTON_DRAW_GRID)
                {
                    mCurrentChart.mShouldDrawGrid = !mCurrentChart.mShouldDrawGrid;
                    mHasGrid = mCurrentChart.mShouldDrawGrid;
                    mCurrentChart.invalidate();

                    if (mListener != null)
                        mListener.onEvent(this, C.EVT_SUB_CHART_CONTAINER_CHANGED, 0, null);
                }
                if (aIntParameter == C.ID_BUTTON_SECOND_CHART)
                {
                    if (!mCurrentChart.isSecondChartDisplaying())
                    {
                        FormSettingParameters dlg = new FormSettingParameters(mShare, this);
                        dlg.setOptionIDX(1);
                        dlg.setCurrentChart(getSubchartID());
                        dlg.ShowDialog();                        
                    }
                    mCurrentChart.toggleSecondChart();
                }
                /*
                if (aIntParameter >= C.ID_BUTTON_SWITCH_SUBCHART && aIntParameter < C.ID_BUTTON_SWITCH_SUBCHART + 5)
                {
                    int idx = aIntParameter - C.ID_BUTTON_SWITCH_SUBCHART;
                    mChartIdx++;
                    if (mChartIdx >= ChartBase.CHART_NUMS)
                        mChartIdx = 0;

                    ChartBase c = createSubChart(mChartIdx);
                    if (c != null)
                    {
                        c.setSize(getW(), getH());
                        setChart(c);
                    }
                }
                */
                if (aIntParameter == C.ID_BUTTON_CONTEXT_HELP)
                {
                    mListener.onEvent(this, C.EVT_SHOW_TUTORIAL, mChartIdx, null);
                }
                /*
                if (aIntParameter == C.ID_REMOVE_SUB_CHART)
                {
                    mListener.onEvent(this, aEvent, aIntParameter, null);
                }
                 */
            }
        }
    
        public int getSubchartID()
        {
            return mChartIdx;
        }

        ChartBase createSubChart(int chartID)
        {
            ChartBase c = null;
            Font f = Context.getInstance().getFontSmall();

            if (chartID == ChartBase.CHART_THUYPS)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_THUYPS);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_TRIX)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_TRIX);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_OBV)
            {
                c = new ChartOBV(f);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_MACD)
            {
                c = new ChartMACD(f);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_MFI)
            {
                c = new ChartLine(f);
                c.setChartType(ChartLine.CHART_MFI);    //  ChartLine.CHART_THUYPS);//
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_RSI)
            {
                c = new ChartLine(f);
                c.setChartType(ChartLine.CHART_RSI);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_ADX)
            {
                c = new ChartADX(f);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_STOCHASTIC_FAST)
            {
                c = new ChartStochastic(f, true);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_STOCHASTIC_SLOW)
            {
                c = new ChartStochastic(f, false);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_ADL)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_ADL);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_CHAIKIN)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_CHAIKIN);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_ROC)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_ROC);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_STOCHRSI)
            {
                c = new ChartStochRSI(f);
                c.setChartType(ChartBase.CHART_STOCHRSI);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_WILLIAMR)
            {
                c = new ChartWilliamR(f);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_NVI)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_NVI);
                c.mShouldDrawCursor = true;
            }
            if (chartID == ChartBase.CHART_PVI)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_PVI);
                c.mShouldDrawCursor = true;
            }
            else if (chartID == ChartBase.CHART_PVT)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_PVT);
                c.mShouldDrawCursor = true;
            }
            else if (chartID == ChartBase.CHART_CCI)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_CCI);
                c.mShouldDrawCursor = true;
            }
            else if (chartID == ChartBase.CHART_MASSINDEX)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_MASSINDEX);
                c.mShouldDrawCursor = true;
            }
            else if (chartID == ChartBase.CHART_AROON)
            {
                c = new ChartAroon(f, ChartBase.CHART_AROON);
                c.setChartType(ChartBase.CHART_AROON);
                c.mShouldDrawCursor = true;
            }
            else if (chartID == ChartBase.CHART_AROON_OSCILLATOR)
            {
                c = new ChartAroon(f, ChartBase.CHART_AROON_OSCILLATOR);
                c.setChartType(ChartBase.CHART_AROON_OSCILLATOR);
                c.mShouldDrawCursor = true;
            }
            else if (chartID == ChartBase.CHART_CFM)
            {
                c = new ChartCFM(f);
                c.setChartType(ChartBase.CHART_CFM);
                c.mShouldDrawCursor = true;
            }
            else if (chartID == ChartBase.CHART_ATR)
            {
                c = new ChartLine(f);
                c.setChartType(ChartBase.CHART_ATR);
                c.mShouldDrawCursor = true;
            }
            if (c == null)
            {
                c = new ChartVolume(f);
                c.setListener(this);
                c.mShouldDrawCursor = true;
            }

            c.setHasDrawer(true);

            return c;
        }
        int getChartID(int sub)
        {
            //=============================
            int chartID = 0;
            switch (sub)
            {
                case ChartBase.CHART_VOLUME:
                    chartID = C.ID_VOLUME;
                    break;
                case ChartBase.CHART_MACD:
                    chartID = C.ID_MACD;
                    break;
                case ChartBase.CHART_ADX:
                    chartID = C.ID_ADX;
                    break;
                case ChartBase.CHART_RSI:
                    chartID = C.ID_RSI;
                    break;
                case ChartBase.CHART_MFI:
                    chartID = C.ID_MFI;
                    break;
                case ChartBase.CHART_STOCHASTIC_FAST:
                    chartID = C.ID_STOCHASTIC_FAST;
                    break;
                case ChartBase.CHART_STOCHASTIC_SLOW:
                    chartID = C.ID_STOCHASTIC_SLOW;
                    break;
                case ChartBase.CHART_STOCHRSI:
                    chartID = C.ID_STOCHRSI;
                    break;
                case ChartBase.CHART_WILLIAMR:
                    chartID = C.ID_WILLIAM_R;
                    break;
                default:
                    break;
            }

            return chartID;
        }

        public override void setSize(int w, int h)
        {
            base.setSize(w, h);
            if (mCurrentChart != null)
            {
                mCurrentChart.setPosition(0, 0);
                mCurrentChart.setSize(w, h);
                mCurrentChart.invalidate();
                int x, y;
                if (mSettingButton != null)
                {
                    mSettingButton.setPosition(0, (getH() - mSettingButton.getH()) - 1);
                }

                x = mSettingButton.getRight();
                y = mSettingButton.getY();

                if (mDrawGridButton != null)
                {
                    mDrawGridButton.setPosition(x, y);
                    x = mDrawGridButton.getRight();
                }
                if (mSecondChartButton != null)
                {
                    mSecondChartButton.setPosition(x, y);
                    x = mSecondChartButton.getRight();
                }
                if (mHelpButton != null)
                {
                    mHelpButton.setPosition(0, y - mHelpButton.getH());
                }
                //if (mRemoveButton != null)
                //{
                    //mAddButton.setPosition(mSwitchButton.getRight(), mSwitchButton.getY());
                    //mRemoveButton.setPosition(0, (getH() - mRemoveButton.getH()) - 1);
                //}
            }
        }

        public void hideRemoveButton()
        {
            //if (mRemoveButton != null)
            //{
                //mRemoveButton.show(false);
            //}
        }

        public bool hasGrid()
        {
            return mHasGrid;
        }

        public void setHasGrid(bool grid)
        {
            mHasGrid = grid;
            if (mCurrentChart != null)
                mCurrentChart.mShouldDrawGrid = grid;
        }

        public void setShare(Share share)
        {
            mShare = share;
        }
    }
}
