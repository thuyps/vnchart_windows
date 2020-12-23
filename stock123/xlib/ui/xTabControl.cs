using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using xlib.framework;

namespace xlib.ui
{
    public class xTabControl: xBaseControl
    {
        TabControl mTab;
        public xTabControl()
            : base(null)
        {
            mTab = new TabControl();
            setControl(mTab);
        }

        public static xTabControl createTabControl()
        {
            xTabControl c = new xTabControl();
            return c;
        }

        public void addPage(xTabPage page)
        {
            TabPage tp = (TabPage)page.getControl();

            mTab.TabPages.Add(tp);
            //tp.Parent = mTab;
        }

        public void removePage(xTabPage page)
        {
            TabPage tp = (TabPage)page.getControl();
            mTab.TabPages.Remove(tp);
        }

        public xTabPage getPageAtIndex(int pageIndex)
        {
            TabPage page = mTab.TabPages[pageIndex];

            xTabPage xPage = (xTabPage)page.Tag;

            return xPage;
        }

        public void removeAllPages()
        {
            mTab.TabPages.Clear();
        }

        public int getPageCount()
        {
            return ((TabControl)getControl()).TabPages.Count;
        }
        
        public void selectPage(int pageIndex)
        {
            TabControl tabControl = (TabControl)this.getControl();

            if (pageIndex < getPageCount())
            {
                tabControl.SelectTab(pageIndex);
            }
        }

        public void selectLastPage(){
            TabControl tabControl = (TabControl)this.getControl();
            tabControl.SelectTab(getPageCount()-1);
        }
    }
}
