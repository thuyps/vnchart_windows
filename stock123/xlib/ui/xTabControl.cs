using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using xlib.framework;

namespace xlib.ui
{
    public delegate bool PreRemoveTab(int indx);
    public delegate void DidRemoveTab(int indx);
    public class TabControlEx : TabControl
    {
        public bool mShowClosePageButton = false;
        public TabControlEx()
            : base()
        {
            PreRemoveTabPage = null;
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        public PreRemoveTab PreRemoveTabPage;
        public DidRemoveTab DidRemoveTabPage;

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Brush b = new SolidBrush(Color.Black);
            Pen p = new Pen(b);

            Rectangle r = e.Bounds;
            r = GetTabRect(e.Index);

            if (mShowClosePageButton)
            {
                string titel = this.TabPages[e.Index].Text;
                Font f = this.Font;
                e.Graphics.DrawString(titel, f, b, new PointF(r.X + 3, r.Y+4));

                //--------------------

                bool drawClose = true;
                if (PreRemoveTabPage != null)
                {
                    drawClose = PreRemoveTabPage(e.Index);
                }

                if (drawClose)
                {
                    r.Offset(r.Size.Width - 9, 3);
                    r.Width = 6;
                    r.Height = 6;

                    e.Graphics.DrawLine(p, r.X, r.Y, r.X + r.Width, r.Y + r.Height);
                    e.Graphics.DrawLine(p, r.X + r.Width, r.Y, r.X, r.Y + r.Height);
                }
            }
            else{
                string titel = this.TabPages[e.Index].Text;
                Font f = this.Font;
                e.Graphics.DrawString(titel, f, b, new PointF(r.X, r.Y));
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            Point p = e.Location;
            for (int i = 0; i < TabCount; i++)
            {
                Rectangle r = GetTabRect(i);
                r.Offset(r.Size.Width - 7, 2);
                r.Width = 6;
                r.Height = 6;
                if (r.Contains(p))
                {
                    CloseTab(i);
                }
            }
        }

        private void CloseTab(int i)
        {
            if (PreRemoveTabPage != null)
            {
                bool closeIt = PreRemoveTabPage(i);
                if (!closeIt)
                    return;
            }
            TabPages.Remove(TabPages[i]);

            if (DidRemoveTabPage != null)
            {
                DidRemoveTabPage(i);
            }
        }
    }

    //=============================================

    public class xTabControl: xBaseControl
    {
        TabControlEx mTab;

        public xTabControl()
            : base(null)
        {
            mTab = new TabControlEx();

            setControl(mTab);
        }

        public void showClosePageButton(bool show, PreRemoveTab preRemoveTab, DidRemoveTab didRemoveTab)
        {
            mTab.mShowClosePageButton = show;
            mTab.Padding = new Point(10, 4);
            mTab.PreRemoveTabPage = preRemoveTab;
            mTab.DidRemoveTabPage = didRemoveTab;
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
