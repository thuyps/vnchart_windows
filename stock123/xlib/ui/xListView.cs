using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using xlib.framework;

namespace xlib.ui
{
    public class xListView: xBaseControl
    {
        protected ColumnClickEventHandler _columnClickHandler;
        public xListView(xIEventListener listener, ImageList imglist)
            : base(listener)
        {
            ListView lv = new ListView();
            lv.Dock = DockStyle.Fill;
            lv.GridLines = true;
            lv.View = View.Details;
            lv.ColumnClick += new ColumnClickEventHandler(columnClick);
            lv.DoubleClick += new EventHandler(lv_DoubleClick);
            lv.SelectedIndexChanged += new EventHandler(selectChanged); 
            lv.FullRowSelect = true;

            //lv.LostFocus += new EventHandler(lv_LostFocus);

            if (imglist != null)
            {
                lv.SmallImageList = imglist;
            }

            lv.Alignment = ListViewAlignment.Default;
            lv.BorderStyle = BorderStyle.Fixed3D;
            //lv.Alignment = ListViewAlignment.Top;

            //--------------------------------------
            setControl(lv);
        }

        public void hideHeader()
        {
            ListView lv = (ListView)getControl();
            if (lv != null)
                lv.HeaderStyle = ColumnHeaderStyle.None;
        }

        void lv_LostFocus(object sender, EventArgs e)
        {
            ListView lv = (ListView)getControl();
            if (lv != null && lv.SelectedItems.Count > 0)
            {
                xListViewItem item = (xListViewItem)lv.SelectedItems[0].Tag;
                item.onListViewLostFocus();
            }
        }

        public xListView(xIEventListener listener, ImageList imglist, bool sortable)
            : base(listener)
        {
            ListView lv = new ListView();
            lv.Dock = DockStyle.Fill;
            lv.GridLines = true;
            lv.View = View.Details;
            if (sortable)
                lv.ColumnClick += new ColumnClickEventHandler(columnClick);
            lv.DoubleClick += new EventHandler(lv_DoubleClick);
            lv.SelectedIndexChanged += new EventHandler(selectChanged);
            lv.FullRowSelect = true;

            if (imglist != null)
            {
                lv.SmallImageList = imglist;
            }

            lv.Alignment = ListViewAlignment.Default;
            lv.BorderStyle = BorderStyle.Fixed3D;
            //lv.Alignment = ListViewAlignment.Top;

            //--------------------------------------
            setControl(lv);
        }

        public void setColumnHeaders(String[] columnHeaders, float[] columnPercents)
        {
            ListView lv = (ListView)getControl();
            lv.Columns.Clear();

            int w = getW();
            float tmp = 0;
            for (int i = 0; i < columnPercents.Length - 1; i++)
            {
                columnPercents[i] = (int)((columnPercents[i] * w) / 100);
                tmp += columnPercents[i];
            }
            columnPercents[columnPercents.Length - 1] = w - tmp - 7;

            for (int i = 0; i < columnHeaders.Length; i++)
            {
                ColumnHeader col = lv.Columns.Add(columnHeaders[i], (int)columnPercents[i], HorizontalAlignment.Center);
            }
            //lv.Columns[lv.Columns.Count - 1].Width = -2;
        }

        void setColumnHeaders(int[] columnWs)
        {
            ListView lv = (ListView)getControl();
            for (int i = 0; i < columnWs.Length; i++)
            {
                lv.Columns.Add("", columnWs[i], HorizontalAlignment.Center);
            }
            //lv.Columns[lv.Columns.Count - 1].Width = -2;
        }

        public static xListView createListView(xIEventListener listener, String[] columnHeaders, float[] columnPercents, int w, int h, ImageList imglist)
        {
            xListView lv = new xListView(listener, imglist);
            lv.setSize(w, h);
            lv.setColumnHeaders(columnHeaders, columnPercents);

            return lv;
        }

        public static xListView createListView(xIEventListener listener, String[] columnHeaders, float[] columnPercents, int w, int h, ImageList imglist, bool sortable)
        {
            xListView lv = new xListView(listener, imglist, sortable);
            lv.setSize(w, h);
            lv.setColumnHeaders(columnHeaders, columnPercents);

            return lv;
        }

        public void addRow(xListViewItem item)
        {
            ListView lv = (ListView)getControl();
            lv.Items.Add(item.getItem());
        }

        public void addRowAtTop(xListViewItem item)
        {
            ListView lv = (ListView)getControl();
            lv.Items.Insert(0, item.getItem());
        }

        public void clear()
        {
            ListView lv = (ListView)getControl();
            lv.Items.Clear();
        }
        //-----------------------------------
        void selectChanged(object sender, EventArgs e)
        {
            ListView lv = (ListView)getControl();
            if (lv.SelectedItems.Count > 0)
            {
                xListViewItem xitem = (xListViewItem)lv.SelectedItems[0].Tag;
                if (mListener != null)
                    mListener.onEvent(this, xBaseControl.EVT_ON_ROW_SELECTED, getID(), xitem);
            }
        }

        void lv_DoubleClick(object sender, EventArgs e)
        {
            ListView lv = (ListView)getControl();
            if (lv.SelectedItems.Count > 0)
            {
                xListViewItem xitem = (xListViewItem)lv.SelectedItems[0].Tag;
                if (mListener != null)
                    mListener.onEvent(this, xBaseControl.EVT_ON_ROW_DOUBLE_CLICK, getID(), xitem);
            }
        }

        public void setColumnClickHandler(ColumnClickEventHandler columnClickHandler)
        {
            _columnClickHandler = columnClickHandler;
        }

        protected void columnClick(object sender, ColumnClickEventArgs e)
        {
            if (_columnClickHandler != null)
            {
                _columnClickHandler(sender, e);
                return;
            }

            ListView lv = (ListView)getControl();
            ListViewItemComparer sorter = lv.ListViewItemSorter as ListViewItemComparer;

            if (sorter == null)
            {
                sorter = new ListViewItemComparer(e.Column);
                sorter.Numeric = false;

                lv.ListViewItemSorter = sorter;
            }
            else
            {
                sorter.Column = e.Column;
            }

            lv.Sort();
        }

        public xListViewItem getSelectedItem()
        {
            ListView lv = (ListView)getControl();
            if (lv.SelectedItems.Count > 0)
            {
                ListViewItem item = lv.SelectedItems[0];
                xListViewItem xitem = (xListViewItem)item.Tag;

                return xitem;
            }

            return null;
        }

        override public void invalidate()
        {
            ListView lv = (ListView)getControl();
            int cnt = lv.Items.Count;
            for (int i = 0; i < cnt; i++)
            {
                ListViewItem item = lv.Items[i];
                xListViewItem xitem = (xListViewItem)item.Tag;
                xitem.invalidate();
            }
        }
    }
    //==========================================================
    public class ListViewItemComparer : IComparer
    {
        private int column;
        private bool numeric = false;

        public bool LowToHigher = true;

        public int Column
        {
            get { return column; }
            set { column = value; }
        }

        public bool Numeric
        {
            get { return numeric; }
            set { numeric = value; }
        }

        public ListViewItemComparer(int columnIndex)
        {
            Column = columnIndex;
        }

        public int Compare(object x, object y)
        {
            ListViewItem itemX = x as ListViewItem;
            ListViewItem itemY = y as ListViewItem;

            if (itemX == null && itemY == null)
                return 0;
            else if (itemX == null)
                return -1;
            else if (itemY == null)
                return 1;

            if (itemX == itemY) return 0;

            if (Numeric)
            {
                decimal itemXVal, itemYVal;

                if (!Decimal.TryParse(itemX.SubItems[Column].Text, out itemXVal))
                {
                    itemXVal = 0;
                }
                if (!Decimal.TryParse(itemY.SubItems[Column].Text, out itemYVal))
                {
                    itemYVal = 0;
                }

                if (LowToHigher)
                {
                    return Decimal.Compare(itemXVal, itemYVal);
                }
                else
                {
                    return Decimal.Compare(itemYVal, itemXVal);
                }

                
            }
            else
            {
                string itemXText = itemX.SubItems[Column].Text;
                string itemYText = itemY.SubItems[Column].Text;

                return String.Compare(itemXText, itemYText);
            }
        }
    }
}
