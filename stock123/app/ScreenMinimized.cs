using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.ui;
using xlib.framework;
using xlib.utils;
using stock123.app.data;
using stock123.app.chart;
using stock123.app.ui;

namespace stock123.app
{
    public class ScreenMinimized: ScreenBase
    {
        const int STATE_NORMAL = 0;
        const int STATE_SHOWING_REALTIME = 1;

        //const int NETSTATE_NORMAL = 0;
        const int NETSTATE_PREPARING_LOGIN = 1;
        const int NETSTATE_LOGGING = 2;
        const int NETSTATE_PREPARING_PRICEBOARD_ZERO = 4;
        const int NETSTATE_GETTING_PRICEBOARD_ZERO = 5;

        const int NETSTATE_REQUEST_PREPARING = 10;
        const int NETSTATE_REQUEST = 11;

        const int MINI_H = 20;
        int mState;
        int mNetState;
        int mCurrentLocationX;
        int mCurrentLocationY;
        xTimer mTimer;
        bool mIsRequestingNetwork;
        MiniFormMoving mMiniFormMoving;

        public ScreenMinimized()
            : base()
        {
            mState = STATE_NORMAL;
            mCurrentLocationX = -1;
        }

        xVector mControls = new xVector(5);
        xVector mRealtimes = new xVector(2);

        public override void onActivate()
        {
            base.onActivate();
            mIsRequestingNetwork = false;
            //---------------------------------------
            if (mTimer == null)
                mTimer = new xTimer(3000);
            mContext.mNetProtocol.cancelNetwork();
            mContext.mNetProtocol.setListener(this);

            mState = STATE_NORMAL;

            MainApplication.getInstance().minimizeApplicationAsString(700, MINI_H);
            MainApplication.getInstance().postMessage(this, this, C.EVT_WINDOW_INITIALIZED, 0, null);
            if (mCurrentLocationX != -1)
            {
                MainApplication.getInstance().setLocation(mCurrentLocationX, mCurrentLocationY);
            }

            setSize(MainApplication.getInstance().Size.Width, MainApplication.getInstance().Size.Height);

            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //this.BackColor = Color.Coral;

            MiniFormMoving controller = new MiniFormMoving(this, mContext.getImageList(C.IMG_MINI, -1, -1));
            controller.updateStatus();
            controller.setPosition(0, 0);
            addControl(controller);
            mMiniFormMoving = controller;

            //============finally indices
            MiniIndex index;
            int x = controller.getRight() + 2;
            for (int i = 0; i < 2; i++)
            {
                stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(i);
                if (pi != null)
                {
                    index = new MiniIndex(pi.marketID);
                    index.setPosition(x, 0);
                    addControl(index);

                    mControls.addElement(index);

                    x = index.getRight();
                }
            }
            //  some buttons
            x = createSystemButtons(x + 2);
            setSize(x, MINI_H);

            MainApplication.getInstance().setSize(x, MINI_H);
            if (mCurrentLocationX == -1)
            {
                mCurrentLocationX = (MainApplication.getDesktopW() - getW()) / 2;
                mCurrentLocationY = 0;
            }

            MainApplication.getInstance().setLocation(mCurrentLocationX, mCurrentLocationY);
            if (!mContext.isOnline())
            {
                mNetState = NETSTATE_PREPARING_LOGIN;
            }
            else
            {
                mNetState = NETSTATE_REQUEST_PREPARING;
            }
        }

        int createSystemButtons(int x)
        {
            xButton bt;

            int bw = 22;
            int bh = MINI_H;

            int[] idxs = { 0, 1, 2 };   //  image index
            int[] ids = { C.ID_MINI_REALTIMECHART, C.ID_MINI_RESTORE, C.ID_MINI_EXIT };

            ImageList imgs = mContext.getImageList(C.IMG_MINI, 16, 16);
            for (int i = 0; i < idxs.Length; i++)
            {
                bt = xButton.createImageButton(ids[i], this, imgs, idxs[i]);
                bt.setSize(bw, bh);
                bt.setPosition(x, 0);
                addControl(bt);

                x = bt.getRight();
            }

            return x;
        }

        public override void onDeactivate()
        {
            base.onDeactivate();

            MainApplication.getInstance().restoreApplicationAdNormal();
        }

        override public void onEvent(object sender, int evt, int aIntParameter, object aParameter)
        {
            //base.onEvent(sender, evt, aIntParameter, aParameter);

            //  network
            if (evt == xBaseControl.EVT_NET_DONE)
            {
                onNetworkCompleted(true);
            }
            else if (evt == xBaseControl.EVT_NET_ERROR)
            {
                onNetworkCompleted(false);
            }
            if (evt == C.EVT_WINDOW_INITIALIZED)
            {
                MainApplication.getInstance().TopMost = true;
            }
            //  moving window
            if (evt == xBaseControl.EVT_ON_MOUSE_MOVE)
            {
                ushort mark = 0xffff;
                short dx = (short)((aIntParameter) & mark);
                short dy = (short)((aIntParameter >> 16) & mark);

                //Utils.trace("===========Move: dx/dy:" + dx + "/" + dy);

                MainApplication.getInstance().moveWindow(dx, dy);
                mCurrentLocationX = MainApplication.getInstance().Location.X;
                mCurrentLocationY = MainApplication.getInstance().Location.Y;
            }

            if (evt == xBaseControl.EVT_BUTTON_CLICKED)
            {
                if (aIntParameter == C.ID_MINI_RESTORE)
                {
                    goNextScreen(mPreviousScreen);
                    MainApplication.getInstance().restoreApplicationAdNormal();
                }
                if (aIntParameter == C.ID_MINI_EXIT)
                {
                    if (showDialogYesNo("Thoát vnChart?"))
                    {
                        MainApplication.getInstance().mShouldSaveWindowState = false;
                        MainApplication.getInstance().exitApplication();
                    }
                }
                if (aIntParameter == C.ID_MINI_REALTIMECHART)
                {
                    showRealtimeChart();
                }
            }
        }

        public override void onTick()
        {
            base.onTick();

            if (mContext.mNetProtocol != null)
                mContext.mNetProtocol.onTick();

            if (mIsRequestingNetwork)
                return;

            if (!mContext.isOnline())
            {
                mNetState = NETSTATE_PREPARING_LOGIN;
            }
            //else
            //{
                //mNetState = NETSTATE_REQUEST_PREPARING;
            //}

            if (!mTimer.isExpired())
                return;
            mTimer.reset();
            //=======================================
            if (mNetState == NETSTATE_PREPARING_LOGIN)
            {
                login();
                mNetState = NETSTATE_LOGGING;
            }
            if (mNetState == NETSTATE_PREPARING_PRICEBOARD_ZERO)
            {
                mContext.mNetProtocol.resetRequest();

                mContext.mNetProtocol.requestPriceboardInitial(-1, null);
                mContext.mNetProtocol.flushRequest();

                mNetState = NETSTATE_GETTING_PRICEBOARD_ZERO;
                return;
            }

            //===============================================
            if (mNetState == NETSTATE_REQUEST_PREPARING)
            {
                if (mState == STATE_NORMAL) //  mini0 mode
                {
                    mContext.mNetProtocol.resetRequest();
                    for (int i = 0; i < mContext.mPriceboard.getIndicesCount(); i++)
                    {
                        stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(i);
                        mContext.mNetProtocol.requestOnlineIndex(pi.marketID);
                    }
                    mContext.mNetProtocol.flushRequest();
                    mIsRequestingNetwork = true;
                    mNetState = NETSTATE_REQUEST;
                }
                else if (mState == STATE_SHOWING_REALTIME)//    mini mode with 2 realtime charts
                {
                    mContext.mNetProtocol.resetRequest();
                    for (int i = 0; i < mContext.mPriceboard.getIndicesCount(); i++)
                    {
                        stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(i);
                        mContext.mNetProtocol.requestOnlineIndex(pi.marketID);
                        if (pi.id != 0)
                        {
                            TradeHistory trade = mContext.getTradeHistory(pi.id);
                            if (trade != null)
                                mContext.mNetProtocol.requestTradeHistory(pi.code, pi.marketID, 0, trade.getLastTime());
                        }
                    }
                    mContext.mNetProtocol.flushRequest();
                    mIsRequestingNetwork = true;
                }
                mNetState = NETSTATE_REQUEST;
            }
        }

        void showRealtimeChart()
        {
            if (STATE_SHOWING_REALTIME == mState)
            {
                for (int i = 0; i < mRealtimes.size(); i++)
                {
                    xBaseControl c = (xBaseControl)mRealtimes.elementAt(i);
                    removeControl(c);
                }
                mRealtimes.removeAllElements();
                Invalidate();

                mState = STATE_NORMAL;
                MainApplication.getInstance().setSize(getW(), MINI_H);
            }
            else
            {
                int rW = getW() / 2;
                int rH = 200;
                int newH = getH() + rH;
                int x = 0;
                int y = MINI_H;

                MainApplication.getInstance().setSize(getW(), newH);

                mState = STATE_SHOWING_REALTIME;
                mTimer.expireTimer();

                RealtimeChart r;
                for (int i = 0; i < 2; i++)
                {
                    stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(i);
                    if (pi != null && pi.id > 0)
                    {
                        r = new RealtimeChart(mContext.getTradeHistory(pi.id), this);
                        r.setPosition(x + 1, y + 1);
                        r.setSize(rW - 2, rH - 2);
                        addControl(r);

                        mRealtimes.addElement(r);

                        x += rW;
                    }
                }
            }
        }

        override public void onNetworkCompleted(bool success)
        {
            mIsRequestingNetwork = false;
            if (success)
            {
                if (mNetState == NETSTATE_LOGGING)
                {
                    mMiniFormMoving.updateStatus();

                    mNetState = NETSTATE_PREPARING_PRICEBOARD_ZERO;
                    mTimer.expireTimer();
                }
                if (mNetState == NETSTATE_GETTING_PRICEBOARD_ZERO)
                {
                    mNetState = NETSTATE_REQUEST_PREPARING;
                }

                int i = 0;
                xBaseControl c;
                for (i = 0; i < mControls.size(); i++)
                {
                    c = (xBaseControl)mControls.elementAt(i);
                    c.invalidate();
                }

                for (i = 0; i < mRealtimes.size(); i++)
                {
                    c = (xBaseControl)mRealtimes.elementAt(i);
                    c.invalidate();
                }

                if (mContext.mPriceboard.isMarketClosed() && mNetState >= NETSTATE_REQUEST_PREPARING)
                {
                    mTimer.setExpiration(30 * 1000);
                }
                else if (mTimer.getExpiration() > 5000)
                {
                    mTimer.setExpiration(10000);
                }
            }
            else
            {
                mTimer.setExpiration(60 * 1000);
                mTimer.reset();
                if (!mContext.isOnline())
                {
                    mNetState = NETSTATE_PREPARING_LOGIN;
                }
            }
            if (mNetState == NETSTATE_REQUEST)
                mNetState = NETSTATE_REQUEST_PREPARING;
        }

        public void login()
        {
            if (!mContext.isValidEmailPassword())
            {
                mNetState = STATE_NORMAL;
                return;
            }
            //-------------------login--------------------
            mContext.mNetProtocol.resetRequest();
            mContext.mNetProtocol.requestLogin(mContext.mEmail, mContext.mPassword, 0);

            if (Utils.getDateAsInt() - mContext.mCompanyUpdateTime >= 2)	//	2days
            {
                mContext.mNetProtocol.requestGetCompanyInfo();
            }

            mContext.mNetProtocol.requestGetShareIDs();
            mContext.mNetProtocol.requestIndicesIDs();

            if (mContext.getShareGroupCount() == 0)
                mContext.mNetProtocol.requestShareGroup();

            

            mContext.mNetProtocol.requestGetUserData();

            //updateLatestData();
            //================================================
            mContext.mNetProtocol.flushRequest();
        }

        override public int getH()
        {
            return this.ClientSize.Height;
        }
        override public int getW()
        {
            return this.ClientSize.Width;
        }
    }
}
