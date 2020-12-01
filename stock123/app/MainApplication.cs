using System;
using System.Collections.Generic;
using System.Text;
using xlib.framework;
using System.Windows.Forms;
using System.Drawing;

using stock123.app.ui;

namespace stock123.app
{
    class MainApplication: xMainApplication
    {
        public const int SCREEN_HOME = 0;
        public const int SCREEN_HELP = 1;
        public const int SCREEN_SEARCH = 2;
        //public const int SCREEN_MINIMIZED = 3;

        public bool mShouldSaveWindowState = true;

        static public bool mMiniMode = false;

        static MainApplication mInstance;
        Context mContext;
        public MainApplication(int w, int h): base(w, h)
        {
            mInstance = this;
            mContext = Context.getInstance();

            //this.Resize += new EventHandler(MainApplication_Resize);

            //this.MaximizeBox = false;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            /*
            MessageBoxButtons yn = MessageBoxButtons.YesNo;
            if (MessageBox.Show("Thoát vnChart?", "", yn) == DialogResult.No)
            {
                e.Cancel = true;
            }
             */
        }
        /*
        void MainApplication_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                ScreenBase current = (ScreenBase)getScreen(getCurrentScreenID());
                if (current.canTranslateToMinize())
                {
                    ScreenBase scr = (ScreenBase)getScreen(SCREEN_MINIMIZED);
                    scr.mPreviousScreen = this.getCurrentScreenID();
                    setScreen(SCREEN_MINIMIZED);
                }
            }
        }
         */

        override public void createScreens()
        {
            mScreens = new xScreen[20];
            mScreens[SCREEN_HOME] = new ScreenHome();
            mScreens[SCREEN_HELP] = new ScreenHelp();
            mScreens[SCREEN_SEARCH] = new ScreenSearch();
            //mScreens[SCREEN_MINIMIZED] = new ScreenMinimized();
        }

        DlgContactingServer mStartupDialog;

        delegate void hideContactingDialog();

        void _initApplication()
        {
            createScreens();
            setScreen(SCREEN_HOME);

            mStartupDialog.Close();
        }

        override public void initApplication()
        {
            //  show dialog
            mStartupDialog = new DlgContactingServer();
            mStartupDialog.setMsg2("...");
            mStartupDialog.Show();

            System.Threading.ThreadPool.QueueUserWorkItem(delegate
             {
                 System.Threading.Thread.Sleep(250);

                 mStartupDialog.Invoke((hideContactingDialog)_initApplication);
             }, null);
            //createScreens();
            //if (mMiniMode)
                //setScreen(SCREEN_MINIMIZED);
            //else
                setScreen(SCREEN_HOME);
//            addStatusBar();
        }

        static public MainApplication getInstance()
        {
            return mInstance;
        }

        Color mOldBackColor;
        Size mOldSize;
        Rectangle mRestoreRec;
        public void minimizeApplicationAsString(int w, int h)
        {
            Rectangle rc = this.RestoreBounds;
            this.mRestoreRec = new Rectangle(rc.X, rc.Y, rc.Width, rc.Height);

            this.FormBorderStyle = FormBorderStyle.None;
            mOldSize = this.Size;
            //this.Size = new Size(w, h);

            //this.Location = new Point((MainApplication.getDesktopW()-w)/2, 0);

            this.StartPosition = FormStartPosition.Manual;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = false;

            //this.TopMost = true;
        }

        public void restoreApplicationAdNormal()
        {
            if (mOldSize == null)
                return;
            this.ShowInTaskbar = true;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Size = mOldSize;

            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
            this.WindowState = FormWindowState.Maximized;

            this.BackColor = mOldBackColor;

            this.TopMost = false;

            this.DesktopBounds = mRestoreRec;
        }

        public void moveWindow(int dx, int dy)
        {
            int newX = this.Location.X + dx;
            int newY = this.Location.Y + dy;
            this.Location = new Point(newX, newY);
        }

        public void setLocation(int x, int y)
        {
            this.Location = new Point(x, y);
        }

        public void setSize(int w, int h)
        {
            this.Size = new Size(w, h);
        }

        override public void onExit()
        {
            mContext.exit();
        }
    }
}
