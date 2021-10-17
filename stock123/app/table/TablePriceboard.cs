using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using xlib.ui;
using xlib.framework;
using xlib.utils;
using stock123.app.data;
using System.Windows.Forms;
using stock123.app.utils;
using stock123.app.ui;

namespace stock123.app.table
{
    class TablePriceboard: xContainer
    {
        public const int TABLE_NORMAL_PRICEBOARD = 0;
        public const int TABLE_GAINLOSS = 1;
        protected int mTableType = TABLE_NORMAL_PRICEBOARD;

        xVector vRows = new xVector(50);

        stShareGroup _shareGroup;

        int rowW;
        int rowH;



        public TablePriceboard(xIEventListener listener, stShareGroup g, int w, int rowH)
            :base(listener)
        {
            //makeCustomRender(true);
            setBackgroundColor(C.COLOR_BLACK);


            rowW = w;
            this.rowH = rowH;

            setShareGroup(g, ShareSortUtils.SORT_DUMUA_DUBAN);
        }

        public void onKeyPress(int key)
        {
            if (key == (int)Keys.Up)
            {
                doMoveNextItem(-1);
            }
            else if (key == (int)Keys.Down)
            {
                doMoveNextItem(1);
            }
        }

        void doMoveNextItem(int dir)
        {
            int newItem = mSelectedIndex;
            newItem += dir;
            if (newItem < 1)
            {
                newItem = 1;
            }
            if (newItem >= vRows.size())
            {
                newItem = vRows.size() - 1;
            }

            RowNormalShare row = (RowNormalShare)vRows.elementAt(newItem);
            String code = row.getCode();
            if (code != null && code.Length > 0)
            {
                Share share = Context.getInstance().mShareManager.getShare(code);

                if (share != null)
                {
                    mSelectedIndex = newItem;
                    mListener.onEvent(this, xBaseControl.EVT_BUTTON_CLICKED, C.ID_SELECT_SHARE_CANDLE, share);
                }
            }
        }

        int mSelectedIndex = 0;
        public void onSelectItem(xBaseControl c)
        {
            for (int i = 0; i < vRows.size(); i++)
            {
                xBaseControl vo = (xBaseControl)vRows.elementAt(i);
                if (vo == c)
                {
                    mSelectedIndex = i;
                    break;
                }
            }
        }

        void setShareGroup(stShareGroup g, int sortType)
        {
            if (mTableType == TABLE_GAINLOSS)
            {
                setShareGroupAsFilterResult(g, sortType);
                return;
            }
            //=========================
            this.removeAllControls();

            mTableType = TABLE_NORMAL_PRICEBOARD;

            if (g == null)
            {
                return;
            }

            _shareGroup = g;

            int cnt = g.getTotal();

            int y = 0;

            Font f = Context.getInstance().getFontText();
            xLabel l = xLabel.createSingleLabel(g.getName(), f, 300);
            l.setBackgroundColor(C.COLOR_ORANGE);
            l.setSize(rowW, (int)f.GetHeight() + 4);
            l.setPosition(0, 0);
            l.setTextColor(C.COLOR_WHITE);

            y = (int)l.getBottom() + 1;

            addControl(l);
            bool deletable = g.getType() == stShareGroup.ID_GROUP_FAVOR;
            if (deletable)
                Utils.trace("++++++++++===============row is detelable");
            for (int i = 0; i <= cnt; i++)
            {
                int idx = i - 1;
                int rH = i == 0? 36: rowH;

                RowNormalShare r;
                if (g.getType() == stShareGroup.ID_GROUP_INDICES)
                {
                    r = new RowIndice(mListener, i, rowW, rH);
                    if (idx >= 0)
                    {
                        String code = g.getCodeAt(idx);

                        if (code != null && code.Length > 0)
                        {
                            r.setData(code);
                        }
                        else
                        {
                            continue;
                        }
                        //r.mAllowDelete = deletable;
                    }
                }
                else
                {
                    r = new RowNormalShare(mListener, i, rowW, rH);
                    r.onShowSortMenu += delegateShowSortMenu;
                    r.onSortABC += delegateSortABC;
                    if (idx >= 0)
                    {
                        int shareID = g.getIDAt(idx);
                        String code = g.getCodeAt(idx);

                        if (code != null && code.Length > 0 && shareID >= 0)
                        {
                            r.setData(code);
                        }
                        else
                        {
                            continue;
                        }
                        //r.mAllowDelete = deletable;
                    }
                }
                r.setParent(this);
                r.setSortType(sortType);
                
                r.setPosition(0, y);
                r.invalidate();
                addControl(r);
                vRows.addElement(r);

                y += rH + 1;
            }

            if (g.getType() == stShareGroup.ID_GROUP_FAVOR)
            {
                xFillBackground bottom = xFillBackground.createFill(rowW, 1, C.COLOR_GRAY_LIGHT);
                bottom.setPosition(0, y);
                addControl(bottom);
                y += 1;

                l = xLabel.createSingleLabel("Click phím phải chuột để thêm/xóa mã hoặc nhóm");
                l.setTextColor(C.COLOR_WHITE);
                l.setPosition(l.getW() - 4, y+3);
                addControl(l);
                int tmp = l.getX();

                //	xoa nhom
                y = (int)l.getBottom() + 4;
                /*
                bt = xUIButton.createButton(listener, C.ID_BUTTON_REMOVE_GROUP, " - ", bw);
                bt.setSize(bw, xApplication.point2Pixels(40));
                bt.setPosition(w - bw - 2, y+1);
                addControl(bt);
			
                l = xLabel.createSingleLabel("Xóa nhóm ->", AppContext.getInstance().getFontText());
                l.setTextColor(C.COLOR_RED);
                l.setPosition(bt.getX() - l.getW() - 4, y + (bt.getH()-l.getH())/2);
                addControl(l);

                y += bt.getH() + 2;
                */
            }

            xFillBackground fillBottom = xFillBackground.createFill(rowW, 2, C.COLOR_GRAY_LIGHT);
            fillBottom.setPosition(0, y);
            addControl(fillBottom);

            invalidate();
        }

        public void setShareGroupAsFilterResult(stShareGroup g, int filterType)
        {
            mTableType = TABLE_GAINLOSS;

            this.removeAllControls();

            _shareGroup = g;

            int cnt = g.getTotal();

            int y = 0;

            Font f = Context.getInstance().getFontText();
            xLabel l = xLabel.createSingleLabel(g.getName(), f, 300);
            l.setBackgroundColor(C.COLOR_ORANGE);
            l.setSize(rowW, (int)f.GetHeight() + 4);
            l.setPosition(0, 0);
            l.setTextColor(C.COLOR_WHITE);

            y = (int)l.getBottom() + 1;

            addControl(l);

            for (int i = 0; i <= cnt; i++)
            {
                int idx = i - 1;
                int rH = i == 0 ? 36 : rowH;

                RowNormalShare r = new RowFilterResult(mListener, i, rowW, rH);
                r.onShowSortMenu += delegateShowSortMenu;
                r.setSortType(filterType);

                if (idx >= 0)
                {
                    String code = g.getCodeAt(idx);

                    if (code != null && code.Length > 0)
                    {
                        r.setData(code);
                    }
                    else
                    {
                        continue;
                    }
                    //r.mAllowDelete = deletable;
                }

                r.setParent(this);

                r.setPosition(0, y);
                r.invalidate();
                addControl(r);
                vRows.addElement(r);

                y += rH + 1;
            }

            invalidate();
        }

        public TablePriceboard(xIEventListener listener, GainLossManager gainlosses, int w, int rowH)
            :base(listener)
        {
            setBackgroundColor(C.COLOR_BLACK);

            int cnt = gainlosses.getTotal();

            int y = 0;

            Font f = Context.getInstance().getFontText();
            xLabel l = xLabel.createSingleLabel("Lãi lỗ");
            l.setBackgroundColor(C.COLOR_ORANGE);
            l.setSize(w, (int)f.GetHeight() + 4);
            l.setPosition(0, 0);
            l.setTextColor(C.COLOR_GREEN);
            y = l.getBottom() + 1;

            addControl(l);

            for (int i = 0; i <= cnt; i++)
            {
                int idx = i - 1;
                int rH = rowH;
                if (idx == -1)
                    rH = 20;

                RowGainloss r = new RowGainloss(listener, i, w, rH);

                if (idx >= 0)
                {
                    stGainloss g = (stGainloss)gainlosses.getGainLossAt(idx);

                    r.setData(g);
                }
                r.setParent(this);
                r.setPosition(0, y);

                addControl(r);
                vRows.addElement(r);

                y += rH + 1;
            }

            //	thong ke
		    {
                GainLossSumary sumary = new GainLossSumary(w);
                sumary.setPosition(0, y);
                addControl(sumary);

                y += sumary.getH();
		    }
            //  Help
            {
                xFillBackground bottom = xFillBackground.createFill(w, 1, C.COLOR_GRAY_LIGHT);
                bottom.setPosition(0, y);
                addControl(bottom);
                y += 1;

                l = xLabel.createSingleLabel("Click phím phải chuột để thêm/xóa mã hoặc nhóm");
                l.setTextColor(C.COLOR_WHITE);
                l.setPosition(l.getW() - 4, y + 3);
                addControl(l);

                y += l.getH() + 4;
            }

            xFillBackground fillBottom = xFillBackground.createFill(w, 2, C.COLOR_GRAY_LIGHT);
            fillBottom.setPosition(0, y);
            addControl(fillBottom);

            invalidate();
        }

        RowNormalShare mLastSelectedRow = null;
        public void selectRow(RowNormalShare row)
        {
            if (row == mLastSelectedRow)
            {
                row.deselect();
                mLastSelectedRow = null;
            }
            else
            {
                if (mLastSelectedRow != null)
                {
                    mLastSelectedRow.deselect();
                }

                row.select();
                mLastSelectedRow = row;
            }
        }

        public RowNormalShare getSelectedItem()
        {
            return mLastSelectedRow;
        }

        xVector shareGroupToVector()
        {
            if (_shareGroup == null){
                return new xVector();
            }
            xVector v = new xVector();
            for (int i = 0; i < _shareGroup.getTotal(); i++)
            {
                Share share = Context.getInstance().mShareManager.getShare(_shareGroup.getCodeAt(i));
                if (share != null){
                    v.addElement(share);
                }
            }

            return v;
        }

        String sortedColumn = "";
        int _sortType;
        void delegateShowSortMenu(Control senderControl)
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
                (sender, item) =>
                {
                    if (item.ClickedItem.Text.CompareTo("Xuất danh sách ra file excel(csv)") == 0)
                    {
                        xVector filteredShares = shareGroupToVector();

                        ShareSortUtils.exportGroupToCSV(filteredShares, sortedColumn);
                        return;
                    }

                    float[] columnPercents = { 30, 28, 34, 8 }; //  code, price, value

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

                        showGetDaysForRSRankingDialog();
                        return;
                    }
                    sortedColumn = columnTexts[2];
                    _sortType = sortType;

                    xVector sorted = shareGroupToVector();
                    ShareSortUtils.doSort(sorted, sortType, 0);

                    _shareGroup.clear();
                    for (int i = 0; i < sorted.size(); i++)
                    {
                        Share share = (Share)sorted.elementAt(i);
                        _shareGroup.addCode(share.mCode);
                    }

                    columnTexts[2] = "▼ " + columnTexts[2];

                    //==================
                    if (mTableType == TABLE_GAINLOSS)
                    {
                        setShareGroupAsFilterResult(_shareGroup, sortType);
                    }
                    else
                    {
                        setShareGroup(_shareGroup, sortType);
                    }
                });

            int posX = (int)((100-7.5f)*rowW);
            posX /= 100;
            cm.Show(senderControl.PointToScreen(new Point(posX, 15)));
        }

        void delegateSortABC(Control senderControl)
        {
            _shareGroup.sort();
            if (mTableType == TABLE_GAINLOSS)
            {
                setShareGroupAsFilterResult(_shareGroup, _sortType);
            }
            else
            {
                setShareGroup(_shareGroup, _sortType);
            }
        }

        void showGetDaysForRSRankingDialog()
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

                float[] columnPercents = { 30, 28, 34, 8 }; //  code, price, value

                String[] columnTexts = { "Mã CP", "Giá", "Điểm RS/VNIndex", "" };
                int sortType = ShareSortUtils.SORT_RS_RANKING;
                sortedColumn = columnTexts[2];

                _sortType = sortType;

                xVector sorted = shareGroupToVector();
                ShareSortUtils.doSort(sorted, sortType, days);

                _shareGroup.clear();
                for (int i = 0; i < sorted.size(); i++)
                {
                    Share share = (Share)sorted.elementAt(i);
                    _shareGroup.addCode(share.mCode);
                }

                columnTexts[2] = "▼ " + columnTexts[2];

                //==================
                setShareGroup(_shareGroup, sortType);
            }
        }
    }
}
