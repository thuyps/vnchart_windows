using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.IO;

using xlib.ui;
using xlib.framework;
using xlib.utils;
using stock123.app.data;
using stock123.app.chart;
using stock123.app.ui;

namespace stock123.app
{
    public class ScreenBase: xScreen, xIEventListener
    {
        public Context mContext;
        public xContainer mClientContainer;
        public xContainer mStatusBar;
        public int mPreviousScreen;
        protected Share mShare;
        public ScreenBase()
            : base()
        {
            mContext = Context.getInstance();
        }

        public void goNextScreen(int scrID)
        {
            MainApplication.getInstance().setScreen(scrID);
        }

        virtual public void onEvent(object sender, int evt, int aIntParameter, object aParameter)
        {
            if (evt == xBaseControl.EVT_NET_DONE)
            {
                onNetworkCompleted(true);
            }
            else if (evt == xBaseControl.EVT_NET_ERROR)
            {
                onNetworkCompleted(false);
            }
            else if (evt == xBaseControl.EVT_NET_DATA_AVAILABLE)
            {
                setStatusMsg("Received: " + aIntParameter/1024 + " KB");
            }
        }

        virtual public void onNetworkCompleted(bool success)
        {
        }

        public override void onSizeChanged()
        {
            base.onSizeChanged();

            this.Size = new Size(MainApplication.getInstance().getDeviceW(),
                MainApplication.getInstance().getDeviceH());

            if (mStatusBar != null)
            {
                mStatusBar.setSize(getW(), mStatusBar.getH());
                mStatusBar.setPosition(0, getH() - mStatusBar.getH());
            }
        }

        public void showDialogOK(String msg)
        {
            System.Windows.Forms.MessageBox.Show(msg);
        }

        public bool showDialogYesNo(String msg)
        {   
            DialogResult r = System.Windows.Forms.MessageBox.Show(msg, "", MessageBoxButtons.YesNo);
            return r == DialogResult.OK || r == DialogResult.Yes;
        }

        public void showDialogCancel(int dialogID, String title, String msg)
        {
        }

        protected xContainer createChartRangeControls(int sel, xContainer c)
        {
            Share share = mContext.getSelectedShare();
            int currentRange = 0;
            if (share != null)
            {
                currentRange = share.getCursorScope();
            }
            c.removeAllControls();
            //  5 days, 1 month, 3 month, 6 month, 1 year, 2 year
            String[] ss = { "5d", "1m", "3m", "6m", "1y", "2y", "-", null };
            int[] scopes = { Share.SCOPE_1WEEKS, Share.SCOPE_1MONTH, Share.SCOPE_3MONTHS, Share.SCOPE_6MONTHS, Share.SCOPE_1YEAR, Share.SCOPE_2YEAR, Share.SCOPE_ALL};
            int i = 0;

            int w = 10;
            int h = 20;
            int x = 0;

            //  update sel
            if (currentRange > 0)
            {
                for (i = 0; i < scopes.Length; i++)
                {
                    if (currentRange >= scopes[i])
                    {
                        sel = i;
                    } else
                        break;
                }
            }
            if (currentRange > Share.SCOPE_2YEAR)
                sel = scopes.Length - 1;
            //==========================
            i = 0;
            while (ss[i] != null)
            {
                xLabel l = xLabel.createSingleLabel(ss[i], mContext.getFontSmallest(), 25);

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

            cl.mHasFibonacci = false;

            cl.allowRepositionCursor();
            cl.setListener(this);

            cl.setChartType(type);

            return cl;
        }

        protected void showHelp()
        {
            DialogAbout dlg = new DialogAbout();
            dlg.Show();

            //FilterSetDialog dlg = new FilterSetDialog();
            //dlg.Show();

            //HelpDlg dlg = new HelpDlg();
            //dlg.ShowDialog();
        }

        protected void showWeb(string url)
        {
            HelpDlg dlg = new HelpDlg(url);

            dlg.ShowDialog();
        }

        protected xBaseControl createShareGroupDroplist(int id, String title, xVector groups)
        {
            xContainer c = new xContainer();
            xLabel l = xLabel.createSingleLabel(title, mContext.getFontTextB(), 150);
            l.setTextColor(C.COLOR_BLUE);
            l.setSize(l.getW(), 16);
            int w = 150;
            l.setPosition(0, 0);
            c.addControl(l);

            ComboBox cb = new ComboBox();
            c.addControl(cb);
            cb.Size = new Size(w, cb.Size.Height);
            cb.Left = 0;
            cb.Top = l.getBottom();
            cb.DropDownStyle = ComboBoxStyle.DropDownList;

            stShareGroup current = mContext.getCurrentShareGroup();

            for (int i = 0; i < groups.size(); i++)
            {
                stShareGroup favor = (stShareGroup)groups.elementAt(i);
                cb.Items.Add(favor.getName());

                if (current != null && favor.getName().CompareTo(current.getName()) == 0)
                {
                    cb.SelectedIndex = i;
                    mContext.setCurrentShareGroup(favor);
                }
            }

            cb.Tag = (Int32)id;
            cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged);

            c.setSize(w, 40);

            return c;
        }

        protected xBaseControl createShareGroupList(bool editable, int id, String title, xVector groups, int w, int h)
        {
            xContainer c = new xContainer();
            c.setSize(w, h);

            Font f = Context.getInstance().getFontText();
            xLabel l = xLabel.createSingleLabel(title, f, w);
            l.setBackgroundColor(C.COLOR_ORANGE);
            l.setTextColor(C.COLOR_WHITE);

            c.addControl(l);

            int titleH = l.getH();

            xContainer c1 = new xContainer();
            if (editable)
                c1.setSize(w, h - 30 - titleH);
            else
                c1.setSize(w, h - 6 - titleH);
            c1.setPosition(0, l.getH());
            c.addControl(c1);

            stShareGroup current = mContext.getCurrentShareGroup();
            string[] ss = {title};
            float[] cols = {100.0f};
            xListView list = xListView.createListView(this, ss, cols, w, c1.getH(), Context.getInstance().getImageList(C.IMG_BLANK_ROW_ICON, 1, 21));
            list.hideHeader();
            list.setID(id);
            ((ListView)list.getControl()).HideSelection = false;
            ((ListView)list.getControl()).Scrollable = true;
            list.setBackgroundColor(C.COLOR_GRAY_DARK);
            int y = 0;
            for (int i = 0; i < groups.size(); i++)
            {
                stShareGroup favor = (stShareGroup)groups.elementAt(i);

                xListViewItem item = xListViewItem.createListViewItem(this, new string[]{favor.getName()});
                uint color = C.COLOR_WHITE;
                //if (favor.getGroupType() == stShareGroup.ID_GROUP_MARKET_OVERVIEW)
                //{
                    //color = C.COLOR_ORANGE;
                //}
                if (favor.getGroupType() == stShareGroup.ID_GROUP_GAINLOSS)
                {
                    color = C.COLOR_ORANGE;
                }
                item.getItem().SubItems[0].Font = mContext.getFontText2();
                item.getItem().SubItems[0].ForeColor = Color.FromArgb((int)color);
                item.setData(favor);
                list.addRow(item);
            }

            c1.addControl(list);
            //====================
            if (editable)
            {
                xButton bt;
                bt = xButton.createStandardButton(C.ID_ADD_GROUP, this, "Thêm nhóm", w / 2 - 2);
                bt.setPosition(1, c1.getBottom() + 3);
                c.addControl(bt);

                bt = xButton.createStandardButton(C.ID_REMOVE_GROUP, this, "Xóa nhóm", w / 2 - 2);
                bt.setPosition(w / 2 + 1, c1.getBottom() + 3);
                c.addControl(bt);
            }

            return c;
        }

        void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox combo = (ComboBox)sender;
                int id = (Int32)combo.Tag;
                stShareGroup g = null;
                String name = (String)combo.SelectedItem;
                if (id == C.ID_DROPDOWN_FAVOR_GROUP)
                {
                    g = mContext.getFavoriteGroup(name);
                }
                if (id == C.ID_DROPDOWN_COMMON_GROUP)
                {
                    g = mContext.getShareGroup(name);
                }
                onShareGroupSelected(g);
            }
            catch (Exception ex)
            {
            }
        }

        virtual public void onShareGroupSelected(stShareGroup g)
        {
        }

        virtual public bool canTranslateToMinize()
        {
            return true;
        }

        public void showHelp(int chart)
        {
            string url = null;
            if (chart == 0) // how to use
            {
                url = "howto.htm";
            }
            if (chart == -1) //  what new
            {
                url = "release.htm";
            }
            if (chart == ChartBase.CHART_BOLLINGER)
            {
            }
            if (chart == ChartBase.CHART_PSAR)
            {
            }
            if (chart == ChartBase.CHART_ICHIMOKU)
            {
                url = "Ichimoku.htm";
            }
            if (chart == ChartBase.CHART_MACD)
            {
                url = "MACD.htm";
            }
            if (chart == ChartBase.CHART_STOCHASTIC_FAST)
            {
                url = "stochastic.htm";
            }
            if (chart == ChartBase.CHART_STOCHASTIC_SLOW)
            {
                url = "stochastic.htm";
            }
            if (chart == ChartBase.CHART_VOLUME)
            {
            }
            if (chart == ChartBase.CHART_ADX)
            {
                url = "adx.htm";
            }
            if (chart == ChartBase.CHART_RSI)
            {
                url = "rsi.htm";
            }
            if (chart == ChartBase.CHART_NVI)
            {
                url = "nvi.html";
            }
            if (chart == ChartBase.CHART_PVI)
            {
                url = "pvi.html";
            }
            if (chart == ChartBase.CHART_MFI)
            {
                url = "mfi.htm";
            }
            if (chart == ChartBase.CHART_MAE)
            {
            }
            if (chart == ChartBase.CHART_CHAIKIN)
            {
                url = "ChaikinOscillator.htm";
            }
            if (chart == ChartBase.CHART_ROC)
            {
                url = "roc.html";
            }
            if (chart == ChartBase.CHART_ZIGZAG)
            {
            }
            if (chart == ChartBase.CHART_THUYPS)
            {
            }
            if (chart == ChartBase.CHART_WILLIAMR)
            {
                url = "williamr.htm";
            }
            if (chart == ChartBase.CHART_STOCHRSI)
            {
                url = "srsi.htm";
            }
            if (chart == ChartBase.CHART_HLC)
            {
            }
            if (chart == ChartBase.CHART_VOLUMEBYPRICE)
            {
                url = "vbp.htm";
            }
            if (chart == ChartBase.CHART_OBV)
            {
                url = "obv.htm";
            }
            if (chart == ChartBase.CHART_TRIX)
            {
                url = "TRIX.htm";
            }
            if (chart == ChartBase.CHART_ADL)
            {
                url = "adl.htm";
            }
            else if (chart == ChartBase.CHART_CCI)
            {
                url = "cci.html";
            }
            else if (chart == ChartBase.CHART_MASSINDEX)
            {
                url = "mass.htm";
            }
            else if (chart == ChartBase.CHART_PVT)
            {
                url = "pvt.html";
            }

            if (url != null)
            {
                string path = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().GetModules()
                    [0].FullyQualifiedName) + "\\help\\" + url;

                HelpDlg dlg = new HelpDlg(path);
                dlg.Show();
            }
        }

        override public int getH()
        {
            if (this.ClientSize.Height < 300)
                return 600;

            return this.ClientSize.Height;
        }

        override public int getW()
        {
            if (this.ClientSize.Width < 500)
                return 500;

            return this.ClientSize.Width;
        }

        //==========================================
        /*
        public xContainer mAlarmContainer = null;
        bool mIsAlarmShowing = false;
        public void showAlarmManager()
        {
            if (mAlarmContainer != null)
                return;

            int cnt = mContext.mAlarmManager.getAlarmCount();
            if (cnt == 0)
            {
                showDialogOK("Bạn chưa đặt Cảnh báo nào. Để cài đặt Cảnh báo, bấm chuột phải vào cổ phiếu và chọn <Cài đặ cảnh báo>");
                return;
            }

            xContainer c = new xContainer();
            mAlarmContainer = c;
            updateAlarmListUI(mAlarmContainer);
            //  int 
            int firedCnt = 0;
            for (int i = 0; i < cnt; i++)
            {
                stAlarm a = mContext.mAlarmManager.getAlarmAt(i);
                if (a.hasAlarm() != 0)
                {
                    firedCnt++;
                }
            }

            AlarmDialog dlg = new AlarmDialog(this, firedCnt, c);
            if (dlg.Size.Height > c.getH() - 66)
            {
                dlg.setSizeH(c.getH() + 80);
            }

            dlg.Show();
        }

        public void updateAlarmListUI(xContainer c)
        {
            c.removeAllControls();

            int itemH = 80;
            int y = 0;
            int itemW = 520;

            int cnt = mContext.mAlarmManager.getAlarmCount();
            c.setSize(itemW + 4, itemH * cnt);
            //  fired alarm are first
            for (int i = 0; i < cnt; i++)
            {
                stAlarm a = mContext.mAlarmManager.getAlarmAt(i);
                if (a.hasAlarm() != 0)
                {
                    xBaseControl item = createAlarmItem(a, 4, y, itemW, itemH);
                    y += itemH;

                    c.addControl(item);
                }
            }
            //  no fired alarm
            for (int i = 0; i < cnt; i++)
            {
                stAlarm a = mContext.mAlarmManager.getAlarmAt(i);
                if (a.hasAlarm() == 0)
                {
                    xBaseControl item = createAlarmItem(a, 4, y, itemW, itemH);
                    y += itemH;

                    c.addControl(item);
                }
            }
        }

        public xBaseControl createAlarmItem(stAlarm a, int x, int y, int w, int h)
        {
            AlarmItem item = new AlarmItem(a);
            item.setSize(w, h);
            item.setPosition(x, y);
            item.setListener(this);

            return item;
        }

        public void settingAlarm(stAlarm a)
        {
            DlgSetAlarm dlg = new DlgSetAlarm(mShare, a);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                a.date = Utils.getDateAsInt();
                a.lowerPrice = dlg.getLowerPrice();
                a.upperPrice = dlg.getUpperPrice();

                mContext.mAlarmManager.saveAlarms();

                mContext.uploadUserData();
            }
        }

        public void onCloseAlarmDialog()
        {
            mAlarmContainer = null;
        }
         */
    }
}
