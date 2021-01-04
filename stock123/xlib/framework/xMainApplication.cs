using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

using xlib.ui;

namespace xlib.framework
{
    public class Msg
    {
        public xIEventListener listener;
        public Object sender;
        public int evt;
        public int intParam;
        public Object param;
    }
    public class xMainApplication: Form
    {
        static public int KEY_DELETE = 46;

        protected xScreen[] mScreens = null;
        int mNextScreenID;
        protected int mCurrentScreenID;
        Form mAppForm = null;
        //=================================
        bool mIsRunning = true;
        delegate void Tick();
        Tick mDelegateTick = null;

        static public bool mControlKey = false;

        List<Msg> mQueueMessages;

        delegate void PostMessage(Object sender, xIEventListener listener, int evt, int intParam, Object param);

        bool mIsExcutingTick = false;
        static xMainApplication mInstance;
        //=================================
        public xMainApplication(int scrW, int scrH)
        {
            mAppForm = this;
            mInstance = this;

            mQueueMessages = new List<Msg>();

            mNextScreenID = -1;
            mCurrentScreenID = -1;

            this.Size = new Size(scrW, scrH);
            initApplication();

            mDelegateTick = new Tick(this.onTick);
            //--------------------------

            Thread thread = new Thread(new ThreadStart(this.run));
            thread.Start();

            this.FormClosing += this.closeForm;

//            this.KeyDown += new KeyEventHandler(xMainApplication_KeyDown);
//            this.KeyUp += new KeyEventHandler(xMainApplication_KeyUp);
        }
/*
        void xMainApplication_KeyDown(object sender, KeyEventArgs e)
        {    
            mControlKey = e.Control;
        }

        void xMainApplication_KeyUp(object sender, KeyEventArgs e)
        {
            mControlKey = e.Control;
        }
        */
        private void closeForm(object sender, FormClosingEventArgs e)
        {
            if (mIsRunning)
            {
                onExit();
                mDelegateTick = null;
                mAppForm = null;
                mIsRunning = false;

                e.Cancel = true;
            }
        }

        private void run()
        {
            while (mIsRunning)
            {
                if (!mIsExcutingTick)
                {
                    try
                    {
                        mIsExcutingTick = true;
                        if (mDelegateTick != null && mAppForm != null)
                            mAppForm.Invoke(mDelegateTick);
                    }catch(Exception e)
                    {
                        mIsExcutingTick = false;
                    }
                }

                Thread.Sleep(10);
            }

            Application.Exit();
        }

        public void postMessage(Object sender, xIEventListener listener, int evt, int intParam, Object param)
        {
            lock (mQueueMessages)
            {
                Msg msg = new Msg();

                msg.listener = listener;
                msg.sender = sender;
                msg.evt = evt;
                msg.intParam = intParam;
                msg.param = param;

                mQueueMessages.Add(msg);
            }
        }

        public void postMessageInUIThread(Object sender, xIEventListener listener, int evt, int intParam, Object param)
        {
            try
            {
                mAppForm.Invoke(new PostMessage(this.postMessage), sender, listener, evt, intParam, param);
            }
            catch (Exception e)
            {
            }

        }

        private void processPostMessage()
        {
            if (mQueueMessages.Count == 0)
                return;
            lock (mQueueMessages)
            {
                Msg msg = mQueueMessages[0];
                mQueueMessages.RemoveAt(0);

                msg.listener.onEvent(msg.sender, msg.evt, msg.intParam, msg.param);
            }
        }

        public void exitApplication()
        {
            onExit();
            mIsRunning = false;
        }

        virtual public void onExit()
        {

        }

        public bool isRunning()
        {
            return mIsRunning;
        }

        //  virtual
        virtual public void initApplication() { }
        virtual public void createScreens() { }
        virtual public void setScreen(int idx)
        {
            if (mScreens == null)
                return;
            if (idx >= 0 && idx < mScreens.Length)
            {
                mNextScreenID = idx;
            }
        }

        xVector mTickControls = new xVector();

        virtual public void onTick()
        {
            try
            {
                processPostMessage();

                if (mNextScreenID != -1)
                {
                    System.Console.Out.WriteLine("=========mNextScreenID: " + mNextScreenID);
                    if (mCurrentScreenID != -1)
                    {
                        mScreens[mCurrentScreenID].onDeactivate();
                        Controls.Remove(mScreens[mCurrentScreenID]);
                    }
                    mCurrentScreenID = mNextScreenID;
                    mNextScreenID = -1;
                    //  remove current screen's toolbar
                    mScreens[mCurrentScreenID].setSize(getDeviceW(), getDeviceH());
                    mScreens[mCurrentScreenID].onActivate();

                    Controls.Add(mScreens[mCurrentScreenID]);
                }

                if (mCurrentScreenID != -1)
                {
                    mScreens[mCurrentScreenID].onTick();
                }
                mIsExcutingTick = false;

                for (int i = 0; i < mTickControls.size(); i++)
                {
                    xBaseControl c = (xBaseControl)mTickControls.elementAt(i);
                    c.onTick();
                }
            }
            catch (Exception e)
            {
                mIsExcutingTick = false;
            }
        }

        public void registerTick(xBaseControl c)
        {
            if (!mTickControls.contains(c))
                mTickControls.addElement(c);
        }

        public void unregisterTick(xBaseControl c)
        {
            mTickControls.removeElement(c);
        }

        public int getDeviceW()
        {
            return this.ClientSize.Width;
        }
        public int getDeviceH()
        {
            return this.ClientSize.Height;
        }

        static public int getDesktopW()
        {
            return System.Windows.Forms.SystemInformation.VirtualScreen.Width;
        }

        static public int getDesktopH()
        {
            return System.Windows.Forms.SystemInformation.VirtualScreen.Height;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (mCurrentScreenID != -1)
            {
                mScreens[mCurrentScreenID].onSizeChanged();
            }
        }

        public static xMainApplication getxMainApplication()
        {
            return mInstance;
        }

        public int captionH()
        {
            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);

            int titleHeight = screenRectangle.Top;// -this.Top;

            return titleHeight;
        }

        public xScreen getScreen(int idx)
        {
            if (idx >= 0 && idx < mScreens.Length)
                return (xScreen)mScreens[idx];

            return null;
        }

        public int getCurrentScreenID()
        {
            return mCurrentScreenID;
        }
    }
}
