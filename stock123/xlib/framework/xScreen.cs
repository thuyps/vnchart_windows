using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using xlib.ui;

namespace xlib.framework
{
    public class xScreen : ContainerControl
    {
        //System.Windows.Forms.ContainerControl
        public xScreen()
        {
            this.Size = new Size(10, 10);
            //this.FormBorderStyle = FormBorderStyle.None;
        }

        protected void StopButtonClick(object sender, EventArgs arg)
        {
            MessageBox.Show("Hello sir");
        }

        virtual public void onActivate() { }
        virtual public void onDeactivate() { }
        virtual public void onTick() { }

        public void addControl(xBaseControl c)
        {
            Controls.Add(c.getControl());
        }

        public void addControl(xContainer c)
        {
            Controls.Add(c.getControl());
        }

        public void addControl(Control c)
        {
            Controls.Add(c);
        }

        public void removeControl(xBaseControl c)
        {
            Controls.Remove(c.getControl());
        }

        virtual public void removeAllControls()
        {
            Controls.Clear();
        }

        virtual public void onSizeChanged()
        {
        }

        public void setSize(int w, int h)
        {
            this.Size = new Size(w, h);
        }

        virtual public int getH()
        {
            return this.ClientSize.Height;
        }

        virtual public int getW()
        {
            return this.ClientSize.Width;
        }

        public void setTitle(String title)
        {
            xMainApplication.getxMainApplication().Text = title;
        }

        virtual public void onToolbarEvent(int buttonID)
        {
        }
        //---------toolbar
        ToolBar mainToolbar;
        public ToolBar getToolbar()
        {
            return mainToolbar;
        }
        public void addToolbar(ImageList imgList)
        {
            //MainApplication.getInstance().addToolbar(imgPNG);
            try
            {
                //removeToolbar();
                mainToolbar = new ToolBar();

                mainToolbar.BorderStyle = BorderStyle.FixedSingle;

                mainToolbar.ImageList = imgList;
                mainToolbar.ButtonClick += new ToolBarButtonClickEventHandler(this.toolbarClick);

                Controls.Add(mainToolbar);
            }
            catch (Exception e)
            {
            }
        }
        public void removeToolbar()
        {
            Controls.Remove(mainToolbar);
        }

        public void clearToolbarButtons()
        {
            mainToolbar.Buttons.Clear();
        }

        public ToolBarButton addToolbarButton(int buttonID, int imgIndex, String text)
        {
            ToolBarButton tb = new ToolBarButton();
            tb.Text = text;
            tb.Tag = (Int32)buttonID;
            tb.ImageIndex = imgIndex;

            mainToolbar.Buttons.Add(tb);

            return tb;
        }

        public ToolBarButton addToolbarButton(int buttonID, int imgIndex, String text, ToolBarButtonStyle style, bool pushed)
        {
            ToolBarButton tb = new ToolBarButton();
            tb.Text = text;
            tb.Tag = (Int32)buttonID;
            tb.ImageIndex = imgIndex;
            tb.Style = style;
            tb.Pushed = pushed;

            mainToolbar.Buttons.Add(tb);

            return tb;
        }

        public void addToolbarSeparator()
        {
            ToolBarButton tb = new ToolBarButton();
            tb.Style = ToolBarButtonStyle.Separator;
            //tb.Text = "|";

            mainToolbar.Buttons.Add(tb);
        }

        public int getToolbarButtonsRight()
        {
            return mainToolbar.Buttons[mainToolbar.Buttons.Count - 1].Rectangle.X + mainToolbar.Buttons[mainToolbar.Buttons.Count - 1].Rectangle.Width;
        }

        void toolbarClick(Object sender, ToolBarButtonClickEventArgs e)
        {
            try
            {
                ToolBarButton bt = e.Button;

                if (bt != null)
                {
                    int buttonID = (Int32)bt.Tag;
                    this.onToolbarEvent(buttonID);
                }
            }
            catch (Exception exception)
            {
                utils.Utils.trace(exception.StackTrace);
                Console.WriteLine(exception.ToString());
            }
        }

        //----------------------status bar
        StatusBar mainStatusBar;
        StatusBarPanel statusPanel;
        StatusBarPanel statusPanel1;
        StatusBarPanel datetimePanel;
        public void addStatusBar()
        {
            if (mainStatusBar != null)
                Controls.Remove(mainStatusBar);

            mainStatusBar = new StatusBar();
            statusPanel = new StatusBarPanel();
            statusPanel1 = new StatusBarPanel();
            datetimePanel = new StatusBarPanel();

            // Set first panel properties and add to StatusBar
            statusPanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel.Text = " - ";
            //statusPanel.ToolTipText = "Last Activity";
            statusPanel.AutoSize = StatusBarPanelAutoSize.Spring;
            mainStatusBar.Panels.Add(statusPanel);
            //  ==============the second=============
            statusPanel1.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel1.Text = " -";
            //statusPanel.ToolTipText = "Last Activity";
            statusPanel1.AutoSize = StatusBarPanelAutoSize.Spring;
            mainStatusBar.Panels.Add(statusPanel1);

            //  ==============the third=============
            // Set second panel properties and add to StatusBar
            datetimePanel.BorderStyle = StatusBarPanelBorderStyle.Raised;
            //datetimePanel.ToolTipText = "Ngày: " + System.DateTime.Today.ToString();
            datetimePanel.Text = System.DateTime.Today.ToLongDateString();
            datetimePanel.AutoSize = StatusBarPanelAutoSize.Contents;
            mainStatusBar.Panels.Add(datetimePanel);

            mainStatusBar.ShowPanels = true;
            Controls.Add(mainStatusBar);
        }

        public void removeStatusbar()
        {
            Controls.Remove(mainStatusBar);
        }

        public void setStatusMsg(String text)
        {
            statusPanel.Text = text;
        }

        public void setStatusMsg1(String text)
        {
            statusPanel1.Text = text;
        }

        virtual public int getToolbarH()
        {
            return 0;
        }

        public int getStatusbarH()
        {
            return 0;
        }

        virtual public int getWorkingH()
        {
            return getH() - getToolbarH() - getStatusbarH();
        }
    }
}