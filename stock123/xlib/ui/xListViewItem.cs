using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.framework;

namespace xlib.ui
{
    public class xListViewItem: xBaseControl
    {
        public ListViewItem mListViewItem;
        public xListViewItem(xIEventListener listener, String[] subItems)
            : base(listener)
        {
            ListViewItem item = new ListViewItem(subItems,1);

            

            mListViewItem = item;
            item.Tag = this;
        }

        public xListViewItem(xIEventListener listener, int columnCnt)
            : base(listener)
        {
            ListViewItem item = new ListViewItem();
            mListViewItem = item;
            item.Tag = this;

            setColumnCnt(columnCnt);
        }

        public static xListViewItem createListViewItem(xIEventListener listener, String[] subItems)
        {
            xListViewItem xi = new xListViewItem(listener, subItems);

            return xi;
        }

        public static xListViewItem createListViewItem(xIEventListener listener, int columnCnt)
        {
            xListViewItem xi = new xListViewItem(listener, columnCnt);

            return xi;
        }

        void setColumnCnt(int cnt)
        {
            for (int i = 0; i < cnt; i++)
            {
                getItem().SubItems.Add("");
            }
        }

        public ListViewItem getItem()
        {
            return mListViewItem;
        }

        public void setTextColor(int cell, uint argb)
        {
            getItem().UseItemStyleForSubItems = false;
            getItem().SubItems[cell].ForeColor = Color.FromArgb((int)argb);
        }

        public void setTextForCell(int cell, String text)
        {
            getItem().SubItems[cell].Text = text;
        }

        public void setTextForCell(int cell, String text, uint color)
        {
            String current = getItem().SubItems[cell].Text;
            if (current != null)
            {
                if (current.CompareTo(text) == 0)
                    return;
            }
            getItem().SubItems[cell].Text = text;
            setTextColor(cell, color);
        }

        public void setTextFont(int cell, Font f)
        {
            getItem().UseItemStyleForSubItems = false;
            getItem().SubItems[cell].Font = f;
        }

        public void setBackgroundColorForCell(int cell, uint color)
        {
            getItem().SubItems[cell].BackColor = Color.FromArgb((int)color);
        }

        public void setImageIndex(int index)
        {
            getItem().ImageIndex = index;
        }

        public void onListViewLostFocus()
        {
            getItem().SubItems[0].BackColor = Color.Red;
        }
    }
}
