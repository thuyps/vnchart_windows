using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using xlib.ui;
using xlib.framework;
using xlib.utils;
using stock123.app.data;


namespace stock123.app.ui
{
    public partial class FilterSetDialog : Form, xIEventListener
    {
        xVector vFilters;
        FilterSet filterSet;
        public FilterSetDialog()
        {
            InitializeComponent();
        }

        public void setFilterSet(FilterSet filterSet)
        {
            this.filterSet = filterSet;

            if (filterSet.name != null)
            {
                this.textBox1.Text = filterSet.name;
            }

            xVector v = FilterManager.getAvailableSignalItemsAndFilterSet(filterSet);
            vFilters = v;

            int w = 340;
            int h = 500;
            int itemH = 30;
            int itemW = w;

            xScrollView scroll = new xScrollView(null, w, h);
            scroll.setSize(w, h);

            w -= 26;
            itemW = w;

            panel1.Controls.Add(scroll.getControl());

            xContainer container = new xContainer();
            //container.setBackgroundColor(0xff00ff00);
            container.setSize(w, v.size()*itemH);
            //scroll.setControl(container.getControl());

            scroll.addControl(container);

            Context ctx = Context.getInstance();
            ImageList imgs = ctx.getImageList(C.IMG_SUB_BUTTONS, 16, 15);
            for (int i = 0; i < v.size(); i++)
            {
                FilterItem item = (FilterItem)v.elementAt(i);
                xContainer c = new xContainer();
                c.setPosition(0, i * itemH);
                c.setSize(itemW, itemH);

                //  title
                //xLabel l = xLabel.createSingleLabel("asdas ad sad asd");//item.getTitle());

                string s = item.getTitle();
                Utils.trace(s);

                xCheckbox l = xCheckbox.createCheckbox(item.getTitle(), item.selected, null, itemW - 60);

                xFillBackground sep = new xFillBackground(0x80808080);
                sep.setPosition(0, itemH - 1);
                sep.setSize(w, 1);

                xButton setting = null;

                if (item.hasSetting)
                {
                    setting = xButton.createImageButton(0, this, imgs, 0);
                    setting.setPosition(0, (itemH - setting.getH()) / 2);
                    setting.setID(i);

                    l.setPosition(30, (itemH - l.getH()) / 2);
                }
                else
                {
                    l.setPosition(30, (itemH - l.getH()) / 2);
                }

                if (setting != null)
                {
                    c.addControl(setting);
                }

                c.addControl(l);
                c.addControl(sep);

                container.addControl(c);
                //xSco
            }
        }

        public void onEvent(object sender, int evt, int aIntParameter, object aParameter)
        {
            xBaseControl c = (xBaseControl)sender;
            int id = c.getID();
            if (id >= 0 && id < vFilters.size())
            {
                FilterItem item = (FilterItem)vFilters.elementAt(id);;
                for (int i = 0; i < filterSet.getFilterItemCnt(); i++)
                {
                    FilterItem used = filterSet.getFilterItemAt(i);
                    if (used.type == item.type)
                    {
                        item.param1 = used.param1;
                        item.param2 = used.param2;
                        item.param3 = used.param3;
                        break;
                    }
                }
                
                DlgFilterParamSetting dlg = new DlgFilterParamSetting();
                dlg.setFilterItem(item);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //FilterManager.getInstance().saveFilterSets();
                }
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            filterSet.name = this.textBox1.Text;

            if (filterSet.name.Length == 0)
            {
                MessageBox.Show("Lỗi, tên bộ lọc không hợp lệ");
                this.DialogResult = DialogResult.None;
            }

            filterSet.clear();
            for (int i = 0; i < vFilters.size(); i++)
            {
                FilterItem item = (FilterItem)vFilters.elementAt(i);
                if (item.selected[0])
                {
                    filterSet.addFilterItem(item);
                }
            }
            Context.userDataManager().flushUserData();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            FilterManager.getInstance().removeFilterSet(filterSet);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}