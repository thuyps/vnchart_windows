using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using stock123.app.data;
using xlib.framework;
using xlib.utils;
using xlib.ui;

using System.Runtime.Serialization.Formatters;

using stock123.app.chart;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using xlib.utils;

using stock123.app.net;
//using c;

namespace stock123.app
{
    public class Context
    {
        public const String URL_INTERNATIONAL_CHART = "https://www.tradingview.com/chart/?symbol=NASDAQ%3AGOOG";
        public const String URL_DATA_CONFIG = "https://s3.amazonaws.com/vnchartpc/vnchart.txt";

        public const int VERSION = ((7<<8)|15);
        public static string getVersionStr()
        {
            int major = (VERSION>>8)&0xff;
            int manor = VERSION&0xff;
            return string.Format("{0}.{1:D2}", major, manor);
        }

        public const int FILE_VERSION_SORT = 4;
        public const int FILE_VERSION_FAVORITE = 1;
        public const int FILE_VERSION_10 = 2;
        public const int FILE_VERSION_11 = 3;
        public const int FILE_VERSION_16 = 16;
        public const int FILE_VERSION_17 = 17;
        public const int FILE_VERSION_20 = 20;
        public const int FILE_VERSION_21 = 21;
        public const int FILE_VERSION_22 = 23;
        public const int FILE_VERSION_30 = 30;
        public const int FILE_VERSION_31 = 31;
        public const int FILE_VERSION_32 = 32;
        public const int FILE_VERSION_33 = 34;
        public const int FILE_VERSION_34 = 34;
        public const int FILE_VERSION_35 = 35;
        public const int FILE_VERSION_36 = 36;

        public const int FILE_VERSION = 37;//FILE_VERSION_36;
        public const int FILE_VERSION_LATEST = 38;//36;

        public const int FILE_VERSION_OPTIONS = 38;

        public const int FILE_VERSION_HOME = 1;


        public const int FILE_SHARE_DATA_VERSION = 31;

        public const string RESOURCE_FILE = "res.dat";
        public const String FILE_REALTIME_CHART = "data\\realtimechart.dat";
        public const String FILE_OPTIONS = "data\\options.dat";
        public const String FILE_COMMON_SHARE_GROUP = "data\\sharegroup.dat";
        public const String FILE_FAVORITE = "data\\favorite.dat";
        public const String FILE_LOGIN = "data\\profile.dat";
        public const String FILE_STORE_NAME = "not_use_anymore";
        public const string FILE_TRADE_HISTORIES = "data\\trades.dat";
        public const string FILE_DRAWING = "data\\draw_";
        public const string FILE_GLOBAL_GROUP = "data\\global.dat";
        public const string FILE_TECHNICAL_SORT = "data\\sorts.dat";
        public const string FILE_ALARM = "data\\alarm.dat";

        public const int FILE_ONLINE_USERDATA_VER = 1;
        Font mFontBig;
        Font mFontText;
        Font mFontTextI;
        Font mFontTextB;
        Font mFontTitle;
        Font mFontSlogan;
        Font mFontSmall;
        Font mFontSmallest;
        Font mFontSmallB;
        Font mFontSmaller;
        //=============================
        class stImageList { public ImageList imgs; public String name;}
        xVector mImageList = new xVector(10);
        //=============================

        static Context mInstance = null;

        public int mLastUpdateAllShare;
        public int mCompanyUpdateTime;

        public String mEmail = "";//"thuyps@yahoo.com";
        public String mPassword = "";//"7288288";
        public String mSessionID = "";
        public bool mIsPurchaseAcc;
        public int mDaysLeft = 0;
        public bool mIsTrialAcc = true;
        public bool mIsShowWhatNew = false;

        public bool mShouldReloadAllData = false;
                
        public int mLatestClientVersion = -1;
        public string mLatestClientVersionURL;
        public bool mLastestClientVersionAsk = false;

        //public NetProtocol mNetProtocol;
        public bool mIsFavorGroupChanged = false;
        public xVector mFavorGroups = new xVector(10);  //  stShareGroup
        public xVector mShareGroups = new xVector(10);  //  stShareGroup
        public xVector mShareGroupsSpecial = new xVector(10);  //  stShareGroup
        public stShareGroup vnIndicesGroup = null;
        stShareGroup mCurrentShareGroup;

        public int mLastDayOfShareUpdate = 0;
        //==================================================
	    public float mOptBBPeriod;
	    public float mOptBBD;
	    public float mOptMACDFast;
	    public float mOptMACDSlow;
	    public float mOptMACDSignal;
	    public float mOptPSAR_alpha;
	    public float mOptPSAR_alpha_max;
        public float[] mOptMFIPeriod = { 0, 0};
        public float[] mOptRSIPeriod = { 0, 0};
        public float mOptStochRSIPeriod = 14;
        public float mOptStochRSISMAPeriod = 5;
	    public float mOptADXPeriod;
        public float mOptADXPeriodDMI;

        public float[] mOptChaikin0 = { 0, 0};
        public float[] mOptChaikin1 = { 0, 0};

        public float[] mOptADL_SMA = {3, 10};

        public float[] mOptROC = { 0, 0};
        public float[] mOptTRIX = { 15, 9 };

        public float mOptEnvelopPeriod = 14;  //  period, type: {1: 3 lines, 2: 5 lines}
        public bool[] mOptEnvelopLine0 = {true};
        public bool[] mOptEnvelopLine1 = { true };
        public bool[] mOptEnvelopLine2 = { true };

        public float mOptRSI_EMA = 5;
        public bool[] mOptRSI_EMA_ON = { true};

        public float mOptMFI_EMA = 5;
        public bool[] mOptMFI_EMA_ON = { true};

        public float mOptROC_EMA = 5;
        public bool[] mOptROC_EMA_ON = { true};

        public float[] mOptNVI_EMA = { 5, 100};
        public bool[] mOptNVI_EMA_ON1 = { true};
        public bool[] mOptNVI_EMA_ON2 = { false};

        public float[] mOptPVI_EMA = { 5, 100};
        public bool[] mOptPVI_EMA_ON1 = { true };
        public bool[] mOptPVI_EMA_ON2 = { false };

        public float mOptChaikinOscillatorEMA = 5;
        public bool[] mOptChaikinOscillatorEMA_ON = { true};

        public float mOptOBV_EMA = 5;
        public bool[] mOptOBV_EMA_ON = { true};
        //  new
        public float mOptPVT = 3;
        public bool[] mOptPVT_EMA_ON = { true };

        public float mOptCCIPeriod;
        public float mOptCCIConstant;

        public int mOptMassIndexDifferent;
        public int mOptMassIndexSum;

	    public float mOptBBPeriodDefault;
	    public float mOptBBDDefault;
	    public float mOptMACDFastDefault;
	    public float mOptMACDSlowDefault;
	    public float mOptMACDSignalDefault;
	    public float mOptPSAR_alphaDefault;
	    public float mOptPSAR_alpha_maxDefault;
	    public float mOptMFIPeriodDefault;
	    public float mOptRSIPeriodDefault;
	    public float mOptADXPeriodDefault;

        //  ichimoku
        public float mOptIchimokuTime1;
        public float mOptIchimokuTime2;
        public float mOptIchimokuTime3;
        //  stochastic
        public float mOptStochasticFastSMA;
        public float mOptStochasticFastKPeriod;

        public float mOptStochasticSlowKPeriod;    //  look back period
        public float mOptStochasticSlowKSmoothK;  // smoothed of %K ( standard is 3)
        public float mOptStochasticSlowSMA;    //  x-day SMA of smoothed %K (standard is 3)

        public float mOptZigzagPercent = 6.0f;
        public float mOptSMAVolume = 9;

        public float mOptWilliamRPeriod;
        public float mOptWR_EMA = 3;
        public bool[] mOptWR_EMA_ON = { true };

        public float mOptATRLoopback = 14;
        public float mOptAroonPeriod = 25;
        public float mOptCFMPeriod = 20;

        public float mOptVSTOP_ATR_Loopback = 20.0f;
        public float mOptVSTOP_MULT = 2.0f;

        public int mOptHistoryChartTimeFrame = Share.SCOPE_1YEAR;

        //==============other options==========
        public bool mIsViewSplitted = false;

        //==========================
        public bool[] mIsSMA = { true, true};
        //  [SET_IDX][LINE] ste 3 not used
        //  3 set of SMA, moi set co 3 duong (gio tang len 5 duong)
        //  bo so 3 hien not used
        //  [0][3] = mOptSMA2[0][0]
        //  [1][4] = mOptSMA2[1][1]
        public float[,] mOptSMA = { { 9, 11, 22 }, { 11, 22, 44 }, { 0, 0, 0} };    //  days custom SMA
        public float[,] mOptSMAThickness = { { 1.0f, 1.0f, 1.0f }, { 1.0f, 1.0f, 1.0f }, { 0, 0, 0} };
        public bool[,] mOptSMAUse = { { true, true, false }, { true, true, false }, { false, false, false} };
        public uint[,] mOptSMAColor = { { 0xffff0000, 0xff00ff00, 0xff0000ff }, { 0xffff0000, 0xff00ff00, 0xff0000ff } };

        public bool[] mIsSMA2 = { true, true };
        public float[,] mOptSMA2 = { { 9, 11, 22 }, { 11, 22, 44 }, { 0, 0, 0 } };    //  days custom SMA
        public float[,] mOptSMAThickness2 = { { 1.0f, 1.0f, 1.0f }, { 1.0f, 1.0f, 1.0f }, { 0, 0, 0 } };
        public bool[,] mOptSMAUse2 = { { false, false, false }, { false, false, false }, { false, false, false } };
        public uint[,] mOptSMAColor2 = { { 0xffff0000, 0xff00ff00, 0xff0000ff }, { 0xffff0000, 0xff00ff00, 0xff0000ff } };

        public bool[] mOptChartVisible = new bool[C.ID_NUMS];
        public int[] mOptSubChart = new int[3];

        public bool mOptAutologin = true;
        //public bool mOptAutorunAtStartup = true;
        public bool mOptFilterKLTB30Use = true;
        public int mOptFilterGTGD = 100;
        public bool mOptFilterHiPriceUse = false;
        public int mOptFilterHiPrice = 50000;
        public bool mOptFilterLowPriceUse = true;
        public int mOptFilterLowPrice = 6000;

        public bool mOptFilterVNIndex = true;
        public bool mOptFilterHNX = true;

        public uint mOptFiboTrendColor = C.BLUE_LINE_COLOR;
        public float mOptFiboTrendThinkness = 1.5f;

        public bool mOptDontUseGainloss = false;
        public int mOptFilterSMA1 = 5;
        public int mOptFilterSMA2 = 12;

        public bool mHasPreviewChart = true;

        public int mMasterChartType;
        //===================================================

        //public int mChartDrawingStart = 0;

        private Share mCurrentShare;
        public Share mRealtimeShare = new Share(10000);
        public ShareManager mShareManager;
        public Priceboard mPriceboard;

        public xVector mGlobalIndices = new xVector(15);    //  stGlobalQuote
        public xVector mGlobalShares = new xVector(15);    //  Share

        public static bool mInvalidSavedFile = true;
        public int err = 0;

        public xVector mTradeHistoryManager = new xVector(200);    //  array of TradeHistory
        public bool mTradeHistoryIsSave = true;
        public TradeHistory mCurrentTradeHistory;

        //private String mCurrentShareGroup;

        public String[] mNextFile = { null, null};
        public int mPriceboardNextfileError;

        xResourceManager mResource;

        public string mSloganText = "~~~YOUR SLOGAN~~~";
        public string mSloganFont = "Arial";
        public float mSloganFontSizeInPoint = 28.0f;
        public bool mSloganFontStrikeout;
        public FontStyle mSloganFontStyle;  //  bold, italic

        public uint mSloganColor = 0xffff8000;
        public uint mSloganColorBG = 0xff0080a0;
        //==================================================
        public Share mSelectedGlobalQuote;
        //==================================================
        public xVectorInt[] mSortTechnicalParams = {null, null};
        public xVector[] mSortTechnicalName = { null, null };

        public bool mIsFirstTime = false;
        //==================================================
        public GainLossManager mGainLossManager;
        public string mDeviceID = "";
        //========================================
        public AlarmManager mAlarmManager;

        //public String mComparingShareCode;

        public bool mNeedRefreshOpens = true;
        public int mViewTypeOfPriceboard = 1;

        public int mFontAdjust = 3;

        public bool noupdate = false;
        String ServerURL;

        public Context()
        {
            mInstance = this;
            mResource = new xResourceManager(RESOURCE_FILE);

            if (xFileManager.readFile("noupdate", true) != null)
            {
                noupdate = true;
            }

            VTDictionary.test();

            init();
        }

        static public void destroy()
        {
            mInstance = null;
        }

        static public Context getInstance()
        {
            if (mInstance == null)
            {
                mInstance = new Context();
            }
            return mInstance;
        }

        public Font getBigFont()
        {
            if (mFontBig == null)
            {
                Font f = new Font(new FontFamily("Arial"), 28.0f, FontStyle.Bold);
                mFontBig = f;
            }

            return mFontBig;
        }

        public Font getFontText2()
        {
            if (mFontText == null)
            {
                Font f = new Font(new FontFamily("Arial"), 9.0f, FontStyle.Regular);
                mFontText = f;
            }

            return mFontText;
        }

        public Font getFontText()
        {
            if (mFontText == null)
            {
                Font f = new Font(new FontFamily("Arial"), 10.0f, FontStyle.Regular);
                mFontText = f;
            }

            return mFontText;
        }
        public Font getFontTextItalic()
        {
            if (mFontTextI == null)
            {
                Font f = new Font(new FontFamily("Arial"), 10.0f, FontStyle.Italic);
                mFontTextI = f;
            }

            return mFontTextI;
        }

        public Font getFontTextB()
        {
            if (mFontTextB == null)
            {
                Font f = new Font(new FontFamily("Arial"), 10.0f, FontStyle.Bold);
                mFontTextB = f;
            }

            return mFontTextB;
        }

        public Font getFontSmall()
        {
            if (mFontSmall == null)
            {
                Font f = new Font(new FontFamily("Arial"), 9.0f + getAjustFont());
                mFontSmall = f;
            }

            return mFontSmall;
        }

        public Font getFontSmallB()
        {
            if (mFontSmallB == null)
            {
                //Font f = new Font(new FontFamily("Arial"), 9.0f + getAjustFont(), FontStyle.Bold);
                Font f = new Font(new FontFamily("Arial"), 9.0f + getAjustFont(), FontStyle.Bold);
                mFontSmallB = f;
            }

            return mFontSmallB;
        }

        public Font getFontSmallest()
        {
            if (mFontSmallest == null)
            {
                Font f = new Font(new FontFamily("Arial"), 8.0f);
                mFontSmallest = f;
            }

            return mFontSmallest;
        }

        public Font getFontSmaller()
        {
            if (mFontSmaller == null)
            {
                Font f = new Font(new FontFamily("Arial"), 8.0f);
                mFontSmaller = f;
            }

            return mFontSmaller;
        }

        public Font getFontTitle()
        {
            if (mFontTitle == null)
            {
                Font f = new Font(new FontFamily("Arial"), 15.0f);
                mFontTitle = f;
            }

            return mFontTitle;
        }

        public Font getFontSlogan()
        {
            //if (mFontSlogan == null)
            {
                Font f = new Font(new FontFamily(mSloganFont), mSloganFontSizeInPoint, mSloganFontStyle);
                mFontSlogan = f;
            }

            return mFontSlogan;
        }

        public bool isOnline()
        {
            return mSessionID != null && mSessionID.Length > 0;
        }

        public void setCurrentShareGroup(stShareGroup g){
            mCurrentShareGroup = g;
        }

        /*
        public void setCurrentShareGroup(String gname){
            mCurrentShareGroup = gname;
        }
         */

        public stShareGroup getFavoriteGroup(String group)
        {
            stShareGroup g;
            for (int i = 0; i < mFavorGroups.size(); i++)
            {
                g = (stShareGroup)mFavorGroups.elementAt(i);
                if (g.getName().CompareTo(group) == 0)
                    return g;
            }

            g = new stShareGroup();
            g.setType(stShareGroup.ID_GROUP_FAVOR);
            g.setName(group);

            mFavorGroups.addElement(g);

            return g;
        }

        public stShareGroup getCurrentShareGroup(){
            if (mCurrentShareGroup == null)
                selectDefaultShareGroup();

            if (mCurrentShareGroup == null)
            {
                mCurrentShareGroup = new stShareGroup();
                mCurrentShareGroup.setName("abc");
            }

            return mCurrentShareGroup;
        }

        public int getShareGroupCount(){
            return mShareGroups.size();
        }

        public stShareGroup getShareGroup(int at) {
            if (at >= 0 && at < mShareGroups.size())
                return (stShareGroup)mShareGroups.elementAt(at);

            return null;
        }

        public stShareGroup getShareGroup(String name) {
            if (name == null)
                return null;
            stShareGroup g;
            for (int i = 0; i < mShareGroups.size(); i++) {
                g = (stShareGroup) mShareGroups.elementAt(i);
                if (name == null && g.getTotal() > 0)
                    return g;
                if (g.isName(name)) {
                    return g;
                }
            }

            if (name == null && mShareGroups.size() > 0)
                return (stShareGroup)mShareGroups.elementAt(0);

            /*
            //	find di filter groups
            for (int i = 0; i < mFilterGroup.size(); i++) {
                stShareGroup g = (stShareGroup) mFilterGroup.elementAt(i);
                if (g.isName(name)) {
                    return g;
                }
            }
            */

            //	create a new if cannot find
            g = new stShareGroup();
            //g.setGroupId(getShareGroupCount());
            g.setName(name);

            mShareGroups.addElement(g);

            return g;
        }

        public void addSortedGroups() {
            /*
            //	sorting group
            //const char* filters[2] = {"Giao dich nhieu nhat", "Tang nhieu nhat"};
            int cnt = 3;
            int ids[3] = {S_FILTER_TRADE_VOL, S_FILTER_MOST_INC, S_FILTER_MOST_DEC};
            int type[3] = {stShareGroup::GROUP_TYPE_TOP_VOLUME, stShareGroup::GROUP_TYPE_TOP_INCREASED, stShareGroup::GROUP_TYPE_TOP_DECREASED};
            for (int i = 0; i < cnt; i++)
            {
            stShareGroup* g = new stShareGroup();
            g.setType(type[i]);

            g.setName(new xString2(getString(ids[i])));	//	already
            mFilterGroup.addElement(g);
            }
             * 
             */
        }

        public void setSelectedShare(Share share)
        {
            mCurrentShare = share;
        }

        public Share getSelectedShare()
        {
            return mCurrentShare;
        }

        public TradeHistory getTradeHistory(){
            return mCurrentTradeHistory;
        }

        public TradeHistory findTradeHistory(int shareID){
            if (mTradeHistoryManager == null)
                return null;
            int cnt = mTradeHistoryManager.size();

            for (int i = 0; i < cnt; i++){
                TradeHistory t = (TradeHistory)mTradeHistoryManager.elementAt(i);
                if (t.mShareID == shareID)
                {
                    if (t.mShare == null)
                    {
                        t.mShare = mShareManager.getShare(shareID);
                    }
                    return t;
                }
            }

            return null;
        }

        public void loadTradeHistoryOffline()
        {
            if ((mTradeHistoryManager != null) && (mTradeHistoryManager.size() > 0))
            {
                return;
            }
            xDataInput di = xFileManager.readFile(FILE_REALTIME_CHART, false);

            if (di == null || di.readInt() != FILE_VERSION)
            {
                return;
            }

            int date = di.readInt();

            int count = di.readInt();
            if (count == 0) return;
            mTradeHistoryManager.removeAllElements();

            for (int i = 0; i < count; i++)
            {
                TradeHistory t = new TradeHistory();
                t.load(di);

                mTradeHistoryManager.addElement(t);
            }
        }

        public void saveTradeHistory()
        {
            if (!isOnline())
                return;
            if (mTradeHistoryManager == null)
            {
                return;
            }
            if (mTradeHistoryIsSave)
            {
                return;
            }

            mTradeHistoryIsSave = true;

            xDataOutput o = null;
            o = new xDataOutput(4000);

            o.writeInt(FILE_VERSION);
            o.writeInt(mPriceboard.mDate);

            int cnt = mTradeHistoryManager.size();
            int begin = 0;
            //  only save the last 50 realtime charts
            if (cnt > 50)
            {
                begin = cnt - 50;
                cnt = 50;
            }
            o.writeInt(cnt);

            for (int i = begin; i < mTradeHistoryManager.size(); i++)
            {
                TradeHistory t = (TradeHistory)mTradeHistoryManager.elementAt(i);

                t.save(o);
            }

            xFileManager.saveFile(o, FILE_REALTIME_CHART);
        }

        public void setCurrentTradeHistory(TradeHistory trade)
        {
            if (trade != null)
                mCurrentTradeHistory = trade;
        }

        public void setCurrentTradeHistory(int shareID){
            setCurrentShare(shareID);

            if (mCurrentShare == null)
                return;

            if (mTradeHistoryManager == null){
                mTradeHistoryManager = new xVector();
            }

            TradeHistory t = null;
            int id = mCurrentShare.getID();
            int marketID = mShareManager.getShareMarketID(id);

            t = findTradeHistory(id);

            if (t == null){
                t = new TradeHistory();
                t.setShareID(marketID, id, mCurrentShare.getCode(), mCurrentShare.isIndex());
                mTradeHistoryManager.addElement(t);
            }

            while (mTradeHistoryManager.size() > 50){
                mTradeHistoryManager.removeElementAt(0);
            }

            mCurrentTradeHistory = t;
        }

        public TradeHistory getTradeHistory(int shareID)
        {
            if (mTradeHistoryManager == null)
            {
                mTradeHistoryManager = new xVector();
            }

            TradeHistory t = null;
            int id = shareID;
            int marketID = mShareManager.getShareMarketID(id);

            t = findTradeHistory(id);

            if (t == null)
            {
                t = new TradeHistory();
                String code = mShareManager.getShareCode(id);
                t.setShareID(marketID, id, code, mPriceboard.isShareIndex(id));
                mTradeHistoryManager.addElement(t);
            }

            while (mTradeHistoryManager.size() > 50)
            {
                mTradeHistoryManager.removeElementAt(0);
            }

            return t;
        }
        //  setShare
        public bool setCurrentShare(int id){
            return setCurrentShare(mShareManager.getShare(id));
        }

        public bool setCurrentShare(String code)
        {
            int id = mShareManager.getShareID(code);
            if (id > 0)
            {
                return setCurrentShare(id);
            }

            return false;
        }

        public bool setCurrentShare(Share share)
        {
            String lastComparingShare = null;
            if (mCurrentShare != null && mCurrentShare != share)
            {
                lastComparingShare = mCurrentShare.mCompare2ShareCode;
            }
            //-----------------------------------
            mCurrentShare = share;
            if (mCurrentShare != null)
            {
                mCurrentShare.clearCalculations();

                mCurrentShare.resetCursor();
            }

            if (mCurrentShare != null)
            {
                mCurrentShare.mCompare2ShareCode = lastComparingShare;
            }

            return mCurrentShare != null;
        }

        public void saveProfile(){
            // save login data
            xDataOutput o = new xDataOutput(1024);
            o.writeInt(FILE_VERSION_LATEST);
            
            o.writeUTF(mEmail); // email
            o.writeUTF(mPassword); // password

            //o.writeInt(mLastDayOfShareUpdate);
            o.writeInt(mCompanyUpdateTime);
            //==========================
            //  slogan font
            o.writeUTF(mSloganFont);
            o.writeFloat(mSloganFontSizeInPoint);
            o.writeBoolean(mSloganFontStrikeout);
            o.writeInt((int)mSloganFontStyle);

            //  slogan text
            o.writeUTF(mSloganText);
            o.writeInt((int)mSloganColor);
            o.writeInt((int)mSloganColorBG);

            o.writeBoolean(mIsShowWhatNew);

            o.writeUTF(mDeviceID);

            xFileManager.saveFile(o, Context.FILE_LOGIN);
            o = null;
        }

        public bool loadData(){
            if (loadProfile()){
                mPriceboard.loadPriceboard();

                return true;
            }

            return false;
        }

        public bool loadProfile() {
            xDataInput di = xFileManager.readFile(FILE_LOGIN, false);

            if (di != null){
                int ver = di.readInt();
                if (ver >= Context.FILE_VERSION_10)
                {
                    mInvalidSavedFile = true;
                    mEmail = di.readUTF();
                    mPassword = di.readUTF();

                    //mLastDayOfShareUpdate = di.readInt();
                    mCompanyUpdateTime = di.readInt();

                    if (ver >= FILE_VERSION_21)
                    {
                        mSloganFont = di.readUTF();
                        mSloganFontSizeInPoint = di.readFloat();
                        mSloganFontStrikeout = di.readBoolean();
                        mSloganFontStyle = (FontStyle)di.readInt();

                        //  slogan text
                        mSloganText = di.readUTF();
                        mSloganColor = (uint)di.readInt();
                        mSloganColorBG = (uint)di.readInt();
                    }

                    mIsShowWhatNew = di.readBoolean();
                    if (ver < FILE_VERSION_LATEST)
                        mIsShowWhatNew = false;

                    if (ver >= FILE_VERSION_33)
                        mDeviceID = di.readUTF();

                    return true;
                }
            }else{
                mEmail = "@gmail.com";
                mPassword = "";// "1";
            }

            return true;
        }

        public void saveDefinedShareGroup()
        {
            xDataOutput o = new xDataOutput(5 * 1024);

            o.writeInt(FILE_VERSION);

            int cnt = mShareGroups.size();

            o.writeInt(cnt);

            for (int i = 0; i < cnt; i++)
            {
                stShareGroup p = (stShareGroup)mShareGroups.elementAt(i);

                p.save(o);
            }

            xFileManager.saveFile(o, FILE_COMMON_SHARE_GROUP);
        }

        public void loadDefinedShareGroup()
        {
            mShareGroups.removeAllElements();
            xDataInput di = xFileManager.readFile(FILE_COMMON_SHARE_GROUP, false);
            stShareGroup g;

            if (di != null && di.readInt() == FILE_VERSION)
            {
                int cnt = di.readInt();

                for (int i = 0; i < cnt; i++)
                {
                    g = new stShareGroup();
                    g.load(di);

                    mShareGroups.addElement(g);
                }
            }


            mShareGroupsSpecial.removeAllElements();

            string[] ss = { "Tăng nhiều nhất", "Giảm nhiều nhất", "Giao dịch nhiều nhất", "Vol tăng đột biến" };
            int[] ids = { stShareGroup.ID_GROUP_MOST_INC, stShareGroup.ID_GROUP_MOST_DEC, stShareGroup.ID_GROUP_MOST_VOL, stShareGroup.ID_GROUP_MOST_VOL_INC_PERCENT};
            for (int i = 0; i < ss.Length; i++)
            {
                g = new stShareGroup();
                g.setName(ss[i]);
                g.setGroupType(ids[i]);

                mShareGroupsSpecial.addElement(g);
            }
        }

        public int getLastCandleUpdate(Share share){
            int lastUpdate = share.getLastCandleDate();

            //  if share is belong to bluchip, ... other groups - not other indices neither favorite group
            if (lastUpdate == 0){
                stShareGroup g;
                for (int i = 0; i < mShareGroups.size(); i++){
                    g = (stShareGroup)mShareGroups.elementAt(i);
                    if (g.getGroupType() == stShareGroup.ID_GROUP_FAVOR){
                        if (g.containShare(share.getID())){
                            lastUpdate = Utils.getDateAsInt(365*2);
                        }
                    }
                }
            }

            if (lastUpdate == 0) {
                if (share.isIndex())
                    lastUpdate = Utils.getDateAsInt(365*3);
                else
                    lastUpdate = Utils.getDateAsInt(30*3);
            }

            return lastUpdate;
        }

        public const int FILE_VERSION_OPTIONS2 = 1;
        public const int FILE_VERSION_OPTIONS2_2 = 2;
        public const string FILE_OPTIONS2 = "options2.dat";
        public void loadOptions2()
        {
            xDataInput di = xFileManager.readFile(FILE_OPTIONS2, false);
            if (di != null)
            {
                int ver = di.readInt();
                if (ver >= FILE_VERSION_OPTIONS2)
                {
                    mViewTypeOfPriceboard = di.readInt();

                    mHasPreviewChart = di.readBoolean();

                    mFontAdjust = di.readInt();

                    if (ver >= FILE_VERSION_OPTIONS2_2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                mOptSMAUse2[i, j] = di.readBoolean();
                                mOptSMA2[i, j] = di.readFloat();
                                mOptSMAThickness2[i, j] = di.readFloat();
                                mOptSMAColor2[i, j] = (uint)di.readInt();
                            }
                        }

                        mOptHistoryChartTimeFrame = di.readInt();
                        if (mOptHistoryChartTimeFrame < 5)
                        {
                            mOptHistoryChartTimeFrame = Share.SCOPE_1YEAR;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        public void saveOptions2()
        {
            xDataOutput o = new xDataOutput(2048);

            o.writeInt(FILE_VERSION_OPTIONS2_2);
            o.writeInt(mViewTypeOfPriceboard);
            if (mViewTypeOfPriceboard == 0)
                mViewTypeOfPriceboard = 1;

            o.writeBoolean(mHasPreviewChart);

            o.writeInt(mFontAdjust);
            //  SMA2
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    o.writeBoolean(mOptSMAUse2[i, j]);
                    o.writeFloat(mOptSMA2[i, j]);
                    o.writeFloat(mOptSMAThickness2[i, j]);
                    o.writeInt((int)mOptSMAColor2[i, j]);
                }
            }

            o.writeInt(mOptHistoryChartTimeFrame);

            xFileManager.saveFile(o, FILE_OPTIONS2);
        }

        public void saveOptions()
        {
            xDataOutput o = new xDataOutput(2048);

            o.writeInt(FILE_VERSION_OPTIONS);

            o.writeFloat(mOptBBPeriod);
            o.writeFloat(mOptBBD);
            o.writeFloat(mOptMACDFast);
            o.writeFloat(mOptMACDSlow);
            o.writeFloat(mOptMACDSignal);
            o.writeFloat(mOptPSAR_alpha);
            o.writeFloat(mOptPSAR_alpha_max);
            o.writeFloat(mOptMFIPeriod[0]);
            o.writeFloat(mOptMFIPeriod[1]);
            o.writeFloat(mOptRSIPeriod[0]);
            o.writeFloat(mOptRSIPeriod[1]);

            o.writeFloat(mOptADXPeriod);
            o.writeFloat(mOptADXPeriodDMI);

            o.writeFloat(mOptBBPeriodDefault);
            o.writeFloat(mOptBBDDefault);
            o.writeFloat(mOptMACDFastDefault);
            o.writeFloat(mOptMACDSlowDefault);
            o.writeFloat(mOptMACDSignalDefault);
            o.writeFloat(mOptPSAR_alphaDefault);
            o.writeFloat(mOptPSAR_alpha_maxDefault);
            o.writeFloat(mOptMFIPeriodDefault);
            o.writeFloat(mOptRSIPeriodDefault);
            o.writeFloat(mOptADXPeriodDefault);

            o.writeFloat(mOptIchimokuTime1);
            o.writeFloat(mOptIchimokuTime2);
            o.writeFloat(mOptIchimokuTime3);

            o.writeFloat(mOptStochasticFastSMA);
            o.writeFloat(mOptStochasticFastKPeriod);

            o.writeFloat(mOptStochasticSlowKPeriod);
            o.writeFloat(mOptStochasticSlowKSmoothK);
            o.writeFloat(mOptStochasticSlowSMA);

            o.writeFloat(mOptWilliamRPeriod);
            o.writeFloat(mOptWR_EMA);
            o.writeBoolean(mOptWR_EMA_ON[0]);

            //==========================
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    o.writeBoolean(mOptSMAUse[i, j]);
                    o.writeFloat(mOptSMA[i, j]);
                    o.writeFloat(mOptSMAThickness[i, j]);
                    o.writeInt((int)mOptSMAColor[i, j]);
                }
            }
            for (int i = 0; i < C.ID_NUMS; i++)
            {
                o.writeBoolean(mOptChartVisible[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                o.writeInt(mOptSubChart[i]);
            }
            //================================
            o.writeBoolean(mOptAutologin);

            o.writeBoolean(mOptFilterHiPriceUse);
            o.writeInt(mOptFilterHiPrice);
            o.writeBoolean(mOptFilterLowPriceUse);
            o.writeInt(mOptFilterLowPrice);
            o.writeBoolean(mOptFilterKLTB30Use);
            o.writeInt(mOptFilterGTGD);
            o.writeBoolean(mOptFilterVNIndex);
            o.writeBoolean(mOptFilterHNX);

            o.writeInt(mOptFiboTrendColor);
            o.writeFloat(mOptFiboTrendThinkness);

            //=================================
            o.writeBoolean(false);//mOptAutorunAtStartup);
            Microsoft.Win32.RegistryKey rkApp = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            /*
            if (mOptAutorunAtStartup)
            {
                rkApp.SetValue("vnChart", Application.ExecutablePath.ToString() + " mini");
            }
            else
            {
                rkApp.DeleteValue("MyApp", false);
            }
             */
            //===============================
            o.writeFloat(mOptChaikin0[0]);
            o.writeFloat(mOptChaikin0[1]);

            o.writeFloat(mOptChaikin1[0]);
            o.writeFloat(mOptChaikin1[1]);

            o.writeFloat(mOptROC[0]);
            o.writeFloat(mOptROC[1]);

            o.writeFloat(mOptTRIX[0]);
            o.writeFloat(mOptTRIX[1]);
            //===============================
            o.writeBoolean(mIsViewSplitted);

            o.writeFloat(mOptZigzagPercent);
            o.writeFloat(mOptSMAVolume);

            o.writeFloat(mOptStochRSIPeriod);
            o.writeFloat(mOptStochRSISMAPeriod);

            //================================
            o.writeBoolean(mOptRSI_EMA_ON[0]);
            o.writeFloat(mOptRSI_EMA);

            o.writeBoolean(mOptMFI_EMA_ON[0]);
            o.writeFloat(mOptMFI_EMA);

            o.writeBoolean(mOptROC_EMA_ON[0]);
            o.writeFloat(mOptROC_EMA);

            o.writeBoolean(mOptChaikinOscillatorEMA_ON[0]);
            o.writeFloat(mOptChaikinOscillatorEMA);

            o.writeFloat(mOptADL_SMA[0]);
            o.writeFloat(mOptADL_SMA[1]);

            o.writeBoolean(mOptOBV_EMA_ON[0]);
            o.writeFloat(mOptOBV_EMA);
            //==============envelops==================
            o.writeFloat(mOptEnvelopPeriod);
            o.writeBoolean(mOptEnvelopLine0[0]);
            o.writeBoolean(mOptEnvelopLine1[0]);
            o.writeBoolean(mOptEnvelopLine2[0]);

            //===================================
            o.writeBoolean(mOptDontUseGainloss);
            o.writeInt(mOptFilterSMA1);
            o.writeInt(mOptFilterSMA2);

            o.writeBoolean(mIsSMA[0]);
            o.writeBoolean(mIsSMA[1]);
  
            //  NVI
            o.writeBoolean(mOptNVI_EMA_ON1[0]);    
            o.writeInt((int)mOptNVI_EMA[0]);

            o.writeBoolean(mOptNVI_EMA_ON2[0]);
            o.writeInt((int)mOptNVI_EMA[1]);
            //  PVI
            o.writeBoolean(mOptPVI_EMA_ON1[0]);
            o.writeInt((int)mOptPVI_EMA[0]);

            o.writeBoolean(mOptPVI_EMA_ON2[0]);
            o.writeInt((int)mOptPVI_EMA[1]);

            o.writeInt((int)mOptPVT);
            o.writeBoolean(mOptPVT_EMA_ON[0]);

            o.writeInt((int)mOptCCIPeriod);
            o.writeFloat(mOptCCIConstant);

            o.writeInt(mOptMassIndexDifferent);
            o.writeInt(mOptMassIndexSum);

            o.writeInt((int)mOptATRLoopback);
            o.writeInt((int)mOptAroonPeriod);
            o.writeInt((int)mOptCFMPeriod);

            o.writeInt(mMasterChartType);
            o.writeInt(mOptFilterGTGD);

            xFileManager.saveFile(o, FILE_OPTIONS);
        }

        public void loadOptions()
        {
            xDataInput di = xFileManager.readFile(FILE_OPTIONS, false);

            if (di == null)
                mIsFirstTime = true;

            int ver = 0;
            if (di != null)
                ver = di.readInt();
            if (di == null || ver < FILE_VERSION_OPTIONS)
            {
                mOptDontUseGainloss = false;
                mOptFilterSMA1 = 5;
                mOptFilterSMA2 = 12;
                mOptBBPeriod = mOptBBPeriodDefault = 14.0f;
                mOptBBD = mOptBBDDefault = 2.0f;
                mOptMACDFast = mOptMACDFastDefault = 12;
                mOptMACDSlow = mOptMACDSlowDefault = 26;
                mOptMACDSignal = mOptMACDSignalDefault = 9;
                mOptPSAR_alpha = mOptPSAR_alphaDefault = 0.02f;
                mOptPSAR_alpha_max = mOptPSAR_alpha_maxDefault = 0.2f;
                mOptMFIPeriod[0] = mOptMFIPeriodDefault = 14;
                mOptMFIPeriod[1] = 7;
                mOptRSIPeriod[0] = mOptRSIPeriodDefault = 14;
                mOptRSIPeriod[1] = 7;
                mOptADXPeriod = mOptADXPeriodDefault = 14;
                mOptADXPeriodDMI = 14;

                mOptIchimokuTime1 = 9;
                mOptIchimokuTime2 = 26;
                mOptIchimokuTime3 = 52;

                mOptStochasticFastSMA = 3;
                mOptStochasticFastKPeriod = 14;

                mOptStochasticSlowSMA = 3;
                mOptStochasticSlowKPeriod = 14;
                mOptStochasticSlowKSmoothK = 3;

                mOptWilliamRPeriod = 14;
                mOptWR_EMA = 3;
                mOptWR_EMA_ON[0] = false;

                mOptChartVisible[C.ID_LINE] = true;
                mOptChartVisible[C.ID_CANDLE] = false;
                mOptChartVisible[C.ID_BB] = true;
                mOptChartVisible[C.ID_ICHIMOKU] = false;
                mOptChartVisible[C.ID_PSAR] = true;
                mOptChartVisible[C.ID_CUSTOM1] = false;
                mOptChartVisible[C.ID_CUSTOM2] = false;

                mOptSubChart[0] = ChartBase.CHART_RSI;
                mOptSubChart[1] = ChartBase.CHART_MACD;
                mOptSubChart[2] = ChartBase.CHART_VOLUME;

                mOptAutologin = true;
                //mOptAutorunAtStartup = true;

                mOptChaikin0[0] = 3;
                mOptChaikin0[1] = 10;
                mOptChaikin1[0] = 7;
                mOptChaikin1[1] = 20;

                mOptROC[0] = 12;
                mOptROC[1] = 6;

                mOptTRIX[0] = 15;
                mOptTRIX[1] = 9;

                mIsSMA[0] = true;
                mIsSMA[1] = true;

                mOptPVT = 3;
                mOptPVT_EMA_ON[0] = false;

                mOptCCIPeriod = 20;
                mOptCCIConstant = 0.015f;

                mOptMassIndexDifferent = 9;
                mOptMassIndexSum = 25;

                mOptAroonPeriod = 25;
                mOptCFMPeriod = 20;
                mOptATRLoopback = 14;

                mMasterChartType = ChartBase.CHART_LINE;

                saveOptions();
            }
            else
            {
                mOptBBPeriod = di.readFloat();
                mOptBBD = di.readFloat();
                mOptMACDFast = di.readFloat();
                mOptMACDSlow = di.readFloat();
                mOptMACDSignal = di.readFloat();
                mOptPSAR_alpha = di.readFloat();
                mOptPSAR_alpha_max = di.readFloat();
                mOptMFIPeriod[0] = di.readFloat();
                if (ver >= FILE_VERSION_16)
                    mOptMFIPeriod[1] = di.readFloat();
                
                mOptRSIPeriod[0] = di.readFloat();
                if (ver >= FILE_VERSION_16)
                    mOptRSIPeriod[1] = di.readFloat();
                
                mOptADXPeriod = di.readFloat();
                mOptADXPeriodDMI = di.readFloat();

                mOptBBPeriodDefault = di.readFloat();
                mOptBBDDefault = di.readFloat();
                mOptMACDFastDefault = di.readFloat();
                mOptMACDSlowDefault = di.readFloat();
                mOptMACDSignalDefault = di.readFloat();
                mOptPSAR_alphaDefault = di.readFloat();
                mOptPSAR_alpha_maxDefault = di.readFloat();
                mOptMFIPeriodDefault = di.readFloat();
                mOptRSIPeriodDefault = di.readFloat();
                mOptADXPeriodDefault = di.readFloat();

                mOptIchimokuTime1 = di.readFloat();
                mOptIchimokuTime2 = di.readFloat();
                mOptIchimokuTime3 = di.readFloat();

                mOptStochasticFastSMA = di.readFloat();
                mOptStochasticFastKPeriod = di.readFloat();

                mOptStochasticSlowKPeriod = di.readFloat();
                mOptStochasticSlowKSmoothK = di.readFloat();
                mOptStochasticSlowSMA = di.readFloat();

                mOptWilliamRPeriod = di.readFloat();
                mOptWR_EMA = di.readFloat();
                mOptWR_EMA_ON[0] = di.readBoolean();
                //==========================
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        mOptSMAUse[i, j] = di.readBoolean();
                        mOptSMA[i, j] = di.readFloat();
                        mOptSMAThickness[i, j] = di.readFloat();
                        mOptSMAColor[i, j] = (uint)di.readInt();
                    }
                }
                for (int i = 0; i < C.ID_NUMS; i++)
                {
                    mOptChartVisible[i] = di.readBoolean();
                }
                for (int i = 0; i < 3; i++)
                {
                    mOptSubChart[i] = di.readInt();
                }
                //=========================
                mOptAutologin = di.readBoolean();

                mOptFilterHiPriceUse = di.readBoolean();
                mOptFilterHiPrice = di.readInt();
                mOptFilterLowPriceUse = di.readBoolean();
                mOptFilterLowPrice = di.readInt();
                mOptFilterKLTB30Use = di.readBoolean();
                mOptFilterGTGD = di.readInt();
                mOptFilterVNIndex = di.readBoolean();
                mOptFilterHNX = di.readBoolean();

                mOptFiboTrendColor = (uint)di.readInt();
                mOptFiboTrendThinkness = di.readFloat();

                /*mOptAutorunAtStartup = */di.readBoolean();

                //==================================
                mOptChaikin0[0] = di.readFloat();
                mOptChaikin0[1] = di.readFloat();

                mOptChaikin1[0] = di.readFloat();    
                mOptChaikin1[1] = di.readFloat();

                mOptROC[0] = di.readFloat();                    
                mOptROC[1] = di.readFloat();
   
                mOptTRIX[0] = di.readFloat();
                mOptTRIX[1] = di.readFloat();

                //=====================
                mIsViewSplitted = di.readBoolean();

                mOptZigzagPercent = di.readFloat();
                if (mOptZigzagPercent <= 0.0f)
                    mOptZigzagPercent = 6.0f;
                mOptSMAVolume = di.readFloat();

                mOptStochRSIPeriod = di.readFloat();
                mOptStochRSISMAPeriod = di.readFloat();

                mOptRSI_EMA_ON[0] = di.readBoolean();
                mOptRSI_EMA = di.readFloat();

                mOptMFI_EMA_ON[0] = di.readBoolean();
                mOptMFI_EMA = di.readFloat();

                mOptROC_EMA_ON[0] = di.readBoolean();
                mOptROC_EMA = di.readFloat();

                mOptChaikinOscillatorEMA_ON[0] = di.readBoolean();
                mOptChaikinOscillatorEMA = di.readFloat();

                mOptADL_SMA[0] = di.readFloat();
                mOptADL_SMA[1] = di.readFloat();

                mOptOBV_EMA_ON[0] = di.readBoolean();
                mOptOBV_EMA = di.readFloat();

                mOptEnvelopPeriod = di.readFloat();
                mOptEnvelopLine0[0] = di.readBoolean();
                mOptEnvelopLine1[0] = di.readBoolean();
                mOptEnvelopLine2[0] = di.readBoolean();

                mOptDontUseGainloss = di.readBoolean();
                mOptFilterSMA1 = di.readInt();
                mOptFilterSMA2 = di.readInt();

                mIsSMA[0] = di.readBoolean();
                mIsSMA[1] = di.readBoolean();

                mOptNVI_EMA_ON1[0] = di.readBoolean();
                mOptNVI_EMA[0] = di.readInt();

                mOptNVI_EMA_ON2[0] = di.readBoolean();
                mOptNVI_EMA[1] = di.readInt();
                //  PVI
                mOptPVI_EMA_ON1[0] = di.readBoolean();
                mOptPVI_EMA[0] = di.readInt();

                mOptPVI_EMA_ON2[0] = di.readBoolean();
                mOptPVI_EMA[1] = di.readInt();

                mOptPVT = di.readInt();
                mOptPVT_EMA_ON[0] = di.readBoolean();

                mOptCCIPeriod = di.readInt();
                mOptCCIConstant = di.readFloat();

                if (mOptPVT == 0)
                    mOptPVT = 3;
                if (mOptCCIPeriod == 0)
                {
                    mOptCCIPeriod = 20;
                    mOptCCIConstant = 0.015f;
                }

                mOptMassIndexDifferent = di.readInt();
                mOptMassIndexSum = di.readInt();
                if (mOptMassIndexDifferent == 0)
                {
                    mOptMassIndexDifferent = 9;
                    mOptMassIndexSum = 25;
                }

                mOptATRLoopback = di.readInt();
                mOptAroonPeriod = di.readInt();
                mOptCFMPeriod = di.readInt();
                if (mOptATRLoopback == 0)
                    mOptATRLoopback = 14;
                if (mOptAroonPeriod == 0)
                    mOptAroonPeriod = 25;
                if (mOptCFMPeriod == 0)
                    mOptCFMPeriod = 20;

                mMasterChartType = di.readInt();
                if (mMasterChartType == 0)
                {
                    mMasterChartType = ChartBase.CHART_LINE;
                }
                //-------------
                mOptFilterGTGD = di.readInt();
                if (mOptFilterGTGD == 0)
                {
                    mOptFilterGTGD = 100;
                }
            }
        }

        public void clearAllSavedData(){
            //xFileManager.removeFile(FILE_OPTIONS);
            xFileManager.removeFile(Priceboard.PRICEBOARD_FILE);
            xFileManager.removeFile(Priceboard.PRICEBOARD_FILE_TRADE_DETAIL);
            xFileManager.removeFile(FILE_REALTIME_CHART);
            xFileManager.removeFile(FILE_FAVORITE);
            xFileManager.removeFile(FILE_GLOBAL_GROUP);
            xFileManager.removeFile(FILE_COMMON_SHARE_GROUP);

            AppConfig.deleteConfig();

            deleteSavedServerUrls();

            mLastDayOfShareUpdate = 0;
            mLastUpdateAllShare = 0;
            mCompanyUpdateTime = 0;

            //saveProfile();
            xFileManager.removeFile("data\\^VNINDEX");
            xFileManager.removeFile("data\\^HASTC");
            xFileManager.removeFile("data\\^UPCOM");

            for (int i = 0; i < mFavorGroups.size(); i++)
            {
                stShareGroup g = (stShareGroup)mFavorGroups.elementAt(i);
                for (int j = 0; j < g.getTotal(); j++)
                {
                    xFileManager.removeFile("data\\" + g.getCodeAt(j));
                }
            }

            mShareManager.removeAllShareFiles();
        }

        public bool isValidEmailPassword()
        {
            return mEmail != null && mEmail.Length > 0 && mPassword != null && mPassword.Length > 0;
        }

        public void logout()
        {
            mSessionID = "";
        }

        //  default in offline mode
        public void loadFavorGroup()
        {
            xDataInput di = xFileManager.readFile(FILE_FAVORITE, false);
            stShareGroup gainloss = null;
            if (di != null && di.readInt() == FILE_VERSION_FAVORITE)
            {
                int cnt = di.readInt();

                for (int i = 0; i < cnt; i++)
                {
                    stShareGroup g = new stShareGroup();

                    g.load(di);
                    if (g.getGroupType() == stShareGroup.ID_GROUP_GAINLOSS)
                        gainloss = g;

                    mFavorGroups.addElement(g);
                }
            }
            if (!mOptDontUseGainloss && gainloss == null)
            {
                gainloss = new stShareGroup();
                gainloss.setGroupType(stShareGroup.ID_GROUP_GAINLOSS);
                gainloss.setName("_ Lãi / Lỗ _");
                mFavorGroups.addElement(gainloss);
            }
        }

        //  replace offline favor by online favor
        public void loadOnlineUserData(xDataInput di)
        {
            clearFavoriteGroups();
            stShareGroup gainloss = null;
            if (!mOptDontUseGainloss)
            {
                gainloss = new stShareGroup();
                gainloss.setGroupType(stShareGroup.ID_GROUP_GAINLOSS);
                gainloss.setName("_ Lãi / Lỗ _");
                mFavorGroups.addElement(gainloss);
            }

            if (di != null && di.readInt() == FILE_ONLINE_USERDATA_VER)
            {
                int cnt = di.readInt();

                for (int i = 0; i < cnt; i++)
                {
                    stShareGroup g = new stShareGroup();

                    g.setType(stShareGroup.ID_GROUP_FAVOR);
                    g.load(di);

                    Utils.trace("\n" + g.getName());

                    g.setType(stShareGroup.ID_GROUP_FAVOR);

                    mFavorGroups.addElement(g);
                    if (mCurrentShareGroup != null
                        && mCurrentShareGroup.getName().CompareTo(g.getName()) == 0)
                    {
                        mCurrentShareGroup = g;
                    }
                }
                saveFavorGroup();
                //======gain loss
                if (di.available() > 0)
                {
                    int ver = di.readInt();
                    if (ver == GainLossManager.FILE_GAINLOSS_SIGNAL)
                    {
                        cnt = di.readInt();
                        mGainLossManager.clearAll();
                        for (int i = 0; i < cnt; i++)
                        {
                            string code = di.readUTF();
                            int date = di.readInt();
                            float price = di.readInt()/1000.0f;
                            int vol = di.readInt();

                            mGainLossManager.addGainLoss(code, date, price, vol);
                        }
                        mGainLossManager.sortList();
                        mGainLossManager.save();
                    }
                }
                //  alarm
                if (di.available() > 0)
                {
                    int ver = di.readInt();
                    if (ver == AlarmManager.FILE_ALARM_SIGNAL)
                    {
                        cnt = di.readInt();
                        mAlarmManager.clearAll();
                        for (int i = 0; i < cnt; i++)
                        {
                            stAlarm a = new stAlarm();
                            a.code = di.readUTF();
                            a.comment = di.readUTF();
                            //a.date
                            a.lowerPrice = di.readInt();
                            a.upperPrice = di.readInt();

                            mAlarmManager.addAlarm(a);
                        }

                        mAlarmManager.saveAlarms();
                    }
                }
            }
        }

        public void saveFavorGroup()
        {
            xDataOutput o = new xDataOutput(5 * 1024);

            o.writeInt(FILE_VERSION_FAVORITE);
            int cnt = mFavorGroups.size();

            o.writeInt(cnt);

            for (int i = 0; i < cnt; i++)
            {
                stShareGroup p = (stShareGroup)mFavorGroups.elementAt(i);

                p.save(o);
            }

            xFileManager.saveFile(o, FILE_FAVORITE);
        }

        public void clearFavoriteGroups()
        {
            mFavorGroups.removeAllElements();
            mGainLossManager.clearAll();
            mAlarmManager.clearAll();
        }

        NetProtocol netUserData;
        public void uploadUserData()
        {
            xDataOutput o = this.getUserDataAsStream();

            netUserData = createNetProtocol();
            netUserData.requestSaveUserData(o);

            netUserData.onDoneDelegate = (sender, ok) =>
                {
                    Utils.trace("user data saved");
                };

            netUserData.flushRequest();
        }

        public xDataOutput getUserDataAsStream()
        {
            xDataOutput o = new xDataOutput(5 * 1024);

            o.writeInt(FILE_ONLINE_USERDATA_VER);
            int cnt = 0;
            int i;
            for (i = 0; i < mFavorGroups.size(); i++)
            {
                stShareGroup p = (stShareGroup)mFavorGroups.elementAt(i);

                if (p.getGroupType() == stShareGroup.ID_GROUP_GAINLOSS)
                    continue;
                cnt++;
            }
            o.writeInt(cnt);

            for (i = 0; i < mFavorGroups.size(); i++)
            {
                stShareGroup p = (stShareGroup)mFavorGroups.elementAt(i);

                if (p.getGroupType() == stShareGroup.ID_GROUP_GAINLOSS)
                    continue;

                p.save(o);
            }

            //  gainloss data
            o.writeInt(GainLossManager.FILE_GAINLOSS_SIGNAL);
            o.writeInt(mGainLossManager.getTotal());
            for (i = 0; i < mGainLossManager.getTotal(); i++)
            {
                stGainloss g = mGainLossManager.getGainLossAt(i);

                o.writeUTF(g.code);
                o.writeInt(g.date);
                o.writeInt((int)(1000*g.price));
                o.writeInt(g.volume);
            }
            //  alarm
            o.writeInt(AlarmManager.FILE_ALARM_SIGNAL);
            o.writeInt(mAlarmManager.getAlarmCount());
            for (i = 0; i < mAlarmManager.getAlarmCount(); i++)
            {
                stAlarm a = mAlarmManager.getAlarmAt(i);
                o.writeUTF(a.code);
                o.writeUTF(a.comment);
                //o.writeInt(a.date);
                o.writeInt(a.lowerPrice);
                o.writeInt(a.upperPrice);
            }

            return o;
        }

        public TradeHistory getTradeHistory(Share share)
        {
            if (share == null)
                return null;
            int shareID = share.getID();
            return getTradeHistory(shareID);
        }

        public void clearShareGroup()
        {
            mShareGroups.removeAllElements();
        }

        public String getSession()
        {
            return mSessionID;
        }

        public void init()
        {
            parseConfig();
            AppConfig.loadAppConfig();

            GlobalData.loadData();

            loadProfile();
            loadOptions();
            loadOptions2();

            loadFavorGroup();
            loadDefinedShareGroup();
            loadGlobalGroup();

            mAlarmManager = new AlarmManager();
            mAlarmManager.loadAlarms();

            mShareManager = new ShareManager();
            mShareManager.loadCompanyInfo();
            mShareManager.loadShareIDs();
            mShareManager.loadCommonData();

            mPriceboard = new Priceboard();
            mPriceboard.loadPriceboard();

            loadTradeHistoryOffline();

            if (!loadTechnicalSort())
                loadTechnicalSortDefault();

            mGainLossManager = new GainLossManager();
            mGainLossManager.load();

            //  get the best URL
            string[] urls = {"http://soft123.com.vn:8080/SmaSrv/SSTK", 
                "http://soft123.com.vn:80/SmaSrv/SSTK", 
                "http://soft123.com.vn:8888/SmaSrv/SSTK"};
            string validURL = null;
            /*
            for (int i = 0; i < urls.Length; i++)
            {
                bool ok = xHttp.pingAddress(urls[i]);
                if (ok)
                {
                    validURL = urls[i];
                    break;
                }
            }
            */

            checkServerSoft123();
//            if (validURL == null)
//            {
//                validURL = urls[0];
//            }
            //validURL = "http://27.0.12.182:8080/SmaSrv/SSTK";
            //mNetProtocol = new NetProtocol();
            //mNetProtocol.setServerURL(validURL);
        }

        //=========================
        class InternalListener : xIEventListener
        {
            public bool isSoft123;
            public InternalListener()
            {
                isSoft123 = false;
            }
            public void onEvent(object sender, int evt, int aIntParameter, object aParameter)
            {
                if (evt == xBaseControl.EVT_NET_DONE)
                {
                    Context.getInstance().processServerInfo(isSoft123, aIntParameter, ((byte[])aParameter));
                }
            }
        }

        xVector loadServerUrls()
        {
            xDataInput di = xFileManager.readFile("serverurls.dat", false);
            if (di != null && di.readInt() == 1)
            {
                int cnt = di.readInt();
                xVector v = new xVector();
                for (int i = 0; i < cnt; i++)
                {
                    String s = di.readUTF();
                    v.addElement(s);
                }

                return v;
            }

            return null;
        }

        void saveServerUrls(xVector v)
        {
            xDataOutput o = new xDataOutput(2048);
            o.writeInt(1);
            o.writeInt(v.size());
            for (int i = 0; i < v.size(); i++)
            {
                o.writeUTF((String)v.elementAt(i));
            }

            xFileManager.saveFile(o, "serverurls.dat");
        }

        public void deleteSavedServerUrls()
        {
            xFileManager.removeFile("serverurls.dat");
        }

        public void processServerInfo(bool isSoft123, int len, byte[] data)
        {
            try
            {
                String s = System.Text.Encoding.UTF8.GetString(data, 0, len);
                //  server count
                String tmp = null;
                int b = 0;
                int e = 0;

                b = s.IndexOf("address_count");
                int fieldLen  = 14;
                xVector v = new xVector();
                if (b >= 0)
                {
                    b = s.IndexOf( '=', b);
                    e = s.IndexOf('\n', b);
                }

                if (e > b)
                {
                    tmp = s.Substring(b + 1, e-1-b);
                    tmp = tmp.Trim();
                    int count = Int32.Parse(tmp);
                    for (int i = 0; i < count; i++)
                    {
                        String field = String.Format("address_{0:D}", (i + 1));
                        b = s.IndexOf(field);
                        if (b > 0)
                        {
                            b = s.IndexOf('=', b) + 1;
                            e = s.IndexOf('\n', b);
                            if (e == -1)
                            {
                                e = s.Length;
                            }

                            if (e > b)
                            {
                                String addr = s.Substring(b, e - b);
                                addr = addr.Trim();
                                v.addElement(addr);
                            }
                        }
                    }
                }

                //======================
                if (v.size() > 0)
                {
                    Random r = new Random();
                    int t = r.Next(v.size());
                    String selected = (String)v.elementAt(t);
                    ServerURL = selected;
                    //NetProtocol net = new NetProtocol();
                    //net.setServerURL(selected);
                    //mNetProtocol = net;

                    saveServerUrls(v);
                }
                else
                {
                    if (isSoft123)
                    {
                        checkServerS3();
                    }
                    else
                    {
                        //NetProtocol net = new NetProtocol();
                        //net.setServerURL("http://soft123.com.vn:8080/SmaSrv/SSTK");
                        //mNetProtocol = net;
                        ServerURL = "http://soft123.com.vn:8080/SmaSrv/SSTK";
                    }
                }
            }
            catch (Exception e)
            {
                Utils.trace(e.Message);

                if (isSoft123)
                {
                    checkServerS3();
                }
                else
                {
                    ServerURL = "http://soft123.com.vn:8080/SmaSrv/SSTK";
                    //NetProtocol net = new NetProtocol();
                    //net.setServerURL("http://soft123.com.vn:8080/SmaSrv/SSTK");
                    //mNetProtocol = net;
                }
            }
        }
        //=========================
  
        public String getServerURL()
        {
            return ServerURL;
        }
        void checkServerSoft123()
        {
            xVector v = loadServerUrls();
            if (v == null || v.size() == 0)
            {
                InternalListener listener = new InternalListener();
                listener.isSoft123 = true;
                xHttp http = new xHttp(listener);
                http.get("http://soft123.com.vn/web/vnchart_server.txt", null);
            }
            else
            {
                Random r = new Random();
                int t = r.Next(v.size());
                String selected = (String)v.elementAt(t);
                ServerURL = selected;
                //NetProtocol net = new NetProtocol();
                //net.setServerURL(selected);
                //mNetProtocol = net;
            }
        }
        void checkServerS3()
        {
            InternalListener listener = new InternalListener();
            listener.isSoft123 = false;
            xHttp http = new xHttp(listener);
            http.get("http://s3.amazonaws.com/vnchartpc/vnchart_server.txt", null);
        }

        public void exit()
        {
            saveOptions();
            //saveProfile();
            saveTradeHistory();
            mPriceboard.savePriceboard();
        }

        public void selectDefaultShareGroup()
        {
            if (mFavorGroups.size() > 0)
            {
                stShareGroup g = (stShareGroup)mFavorGroups.elementAt(0);   //  blue chip
                mCurrentShareGroup = g;
            }
            else if (mShareGroups.size() > 4)
            {
                stShareGroup g = (stShareGroup)mShareGroups.elementAt(1);   //  blue chip
                mCurrentShareGroup = g;
            }
        }

        public void selectDefaultShare()
        {
            setCurrentShare("^VNINDEX");
        }

        public void removeShare(stShareGroup g)
        {
            for (int i = 0; i < mFavorGroups.size(); i++)
            {
                stShareGroup gg = (stShareGroup)mFavorGroups.elementAt(i);
                if (gg == g)
                {
                    mFavorGroups.removeElementAt(i);
                    saveFavorGroup();

                    break;
                }
            }
        }

        public uint valToColor(int v, int c, int r, int f)
        {
            if (v == 0) return C.COLOR_WHITE;
            if (v == c) return C.COLOR_MAGENTA;
            if (v == r) return C.COLOR_YELLOW;
            if (v == f) return C.COLOR_CYAN;
            if (v > r) return C.COLOR_GREEN;
            if (v < r) return C.COLOR_RED;

            return C.COLOR_BLACK;
        }

        public uint valToColorF(float v, float c, float r, float f)
        {
            if (v == 0) return C.COLOR_WHITE;
            if (v == c) return C.COLOR_MAGENTA;
            if (v == r) return C.COLOR_YELLOW;
            if (v == f) return C.COLOR_CYAN;
            if (v > r) return C.COLOR_GREEN;
            if (v < r) return C.COLOR_RED;

            return C.COLOR_BLACK;
        }

        public Image getImage(string png)
        {
            xDataInput di = mResource.getResourceAsStream(png);
            Image img = Utils.createImageFromBytes(di.getBytes(), 0, di.size());

            return img;
        }
        //  
        public ImageList getImageList(String png, int frmW, int frmH)
        {
            stImageList imgs;
            for (int i = 0; i < mImageList.size(); i++)
            {
                imgs = (stImageList)mImageList.elementAt(i);
                if (imgs.name.CompareTo(png) == 0)
                {
                    if (frmW > 0 && frmH > 0)
                    {
                        imgs.imgs.ImageSize = new Size(frmW, frmH);
                    }
                    return imgs.imgs;
                }
            }

            Image img = getImage(png);

            imgs = new stImageList();    
            imgs.imgs = Utils.createImageList(img, frmW, frmH);
            imgs.name = png;

            mImageList.addElement(imgs);

            return imgs.imgs;
        }

        public void addGlobalQuote(string symbol, double point, double change, double changePercent, string name)
        {
            stGlobalQuote item = null;
            bool ok = false;
            for (int i = 0; i < mGlobalIndices.size(); i++)
            {
                item = (stGlobalQuote)mGlobalIndices.elementAt(i);
                if (item.symbol.CompareTo(symbol) == 0)
                {
                    ok = true;
                    break;
                }
            }
            if (!ok)
            {
                item = new stGlobalQuote();
                item.symbol = symbol;
                mGlobalIndices.addElement(item);
            }

            item.point = point;
            item.change = change;
            item.changePercent = changePercent;
            item.name = name;
        }

        public void saveGlobalGroup()
        {
            xDataOutput o = new xDataOutput(2000);
            o.writeInt(FILE_VERSION);
            o.writeInt(mGlobalIndices.size());
            for (int i = 0; i < mGlobalIndices.size(); i++)
            {
                stGlobalQuote quote = (stGlobalQuote)mGlobalIndices.elementAt(i);
                o.writeUTF(quote.name);
                o.writeUTF(quote.symbol);
                o.writeFloat((float)quote.point);
                o.writeFloat((float)quote.change);
                o.writeFloat((float)quote.changePercent);
            }
            xFileManager.saveFile(o, FILE_GLOBAL_GROUP);
        }

        void loadGlobalGroup()
        {
            xDataInput di = xFileManager.readFile(FILE_GLOBAL_GROUP, false);
            if (di != null && di.readInt() == FILE_VERSION)
            {
                int cnt = di.readInt();
                string name, symbol;
                float point, change, changePercent;
                for (int i = 0; i < cnt; i++)
                {
                    name = di.readUTF();
                    symbol = di.readUTF();
                    point = di.readFloat();
                    change = di.readFloat();
                    changePercent = di.readFloat();

                    addGlobalQuote(symbol, point, change, changePercent, name);
                }
            }
        }

        public bool isQuoteFavorite(Share share)
        {
            stShareGroup g;
            for (int i = 0; i < mFavorGroups.size(); i++)
            {
                g = (stShareGroup)mFavorGroups.elementAt(i);
                if (g.containShare(share.mID))
                    return true;
            }

            String s1 = share.mCode.ToLower();
            for (int i = 0; i < mGainLossManager.getTotal(); i++)
            {
                stGainloss gl = mGainLossManager.getGainLossAt(i);

                String s0 = gl.code.ToLower();
                if (s0.CompareTo(s1) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        bool loadTechnicalSort()
        {
            xDataInput di = xFileManager.readFile(FILE_TECHNICAL_SORT, false);
            if (di == null)
                return false;

            int ver = di.readInt();
            if (ver == FILE_VERSION_SORT)
            {
                int cnt = di.readInt();
                for (int i = 0; i < cnt; i++)
                {
                    int items = di.readInt();
                    mSortTechnicalName[i] = new xVector(30);
                    mSortTechnicalParams[i] = new xVectorInt(30);

                    if (i == 1 && items < 9) items = 7;
                    for (int j = 0; j < items; j++)
                    {
                        string s = di.readUTF();
                        int sort = di.readInt();

                        mSortTechnicalName[i].addElement(s);
                        mSortTechnicalParams[i].addElement(sort);
                    }

                    if (i == 1 && mSortTechnicalName[i].size() == 7)
                    {
                        //  SMA
                        mSortTechnicalName[i].addElement("SMA(9) cắt đường giá");
                        mSortTechnicalParams[i].addElement(C._SORT_SMA_PRICE_9);
                        mSortTechnicalName[i].addElement("SMA(26) cắt đường giá");
                        mSortTechnicalParams[i].addElement(C._SORT_SMA_PRICE_26);
                    }
                }

                return true;
            }

            return false;
        }

        public void saveTechnicalSort()
        {
            xDataOutput o = new xDataOutput(2048);
            o.writeInt(FILE_VERSION_SORT);

            o.writeInt(2);
            for (int i = 0; i < 2; i++)
            {
                int items = mSortTechnicalName[i].size();
                o.writeInt(items);
                for (int j = 0; j < items; j++)
                {
                    string s = (string)mSortTechnicalName[i].elementAt(j);
                    int sort = mSortTechnicalParams[i].elementAt(j);

                    o.writeUTF(s);
                    o.writeInt(sort);
                }
            }

            xFileManager.saveFile(o, FILE_TECHNICAL_SORT);
        }

        void loadTechnicalSortDefault()
        {
            xVector sort_names = new xVector(10);
            xVectorInt sort_ints = new xVectorInt(10);

            //========================
            sort_names.addElement("MACD cắt trên đường tín hiệu");
            sort_ints.addElement(C._SORT_MACD_CUT_SIGNAL);

            sort_names.addElement("Stochastic slow: %K cắt trên  %D");
            sort_ints.addElement(C._SORT_SLOW_STOCHASTIC_K_CUT_D);

            sort_names.addElement("RSI && MFI cắt trên SMA");
            sort_ints.addElement(C._SORT_RSI_CUT_SMA | C._SORT_MFI_CUT_SMA);

            sort_names.addElement("ROC && MFI cắt trên SMA");
            sort_ints.addElement(C._SORT_ROC_CUT_SMA | C._SORT_MFI_CUT_SMA);

            sort_names.addElement("RSI cắt trên SMA");
            sort_ints.addElement(C._SORT_RSI_CUT_SMA);

            sort_names.addElement("MFI cắt trên SMA");
            sort_ints.addElement(C._SORT_MFI_CUT_SMA);

            sort_names.addElement("ROC cắt trên SMA");
            sort_ints.addElement(C._SORT_ROC_CUT_SMA);

            sort_names.addElement("Giao cắt 2 đường SMA");
            sort_ints.addElement(C._SORT_SMA1_CUT_SMA2);

            sort_names.addElement("ADX: DMI+ cắt DMI-");
            sort_ints.addElement(C._SORT_ADX_CUT_DMIs);

            mSortTechnicalName[0] = sort_names;
            mSortTechnicalParams[0] = sort_ints;
            //========================
            sort_names = new xVector(10);
            sort_ints = new xVectorInt(10);

            sort_names.addElement("RSI lớn hơn");
            sort_ints.addElement(C._SORT_RSI_HIGHER);

            sort_names.addElement("MFI lớn hơn");
            sort_ints.addElement(C._SORT_MFI_HIGHER);

            sort_names.addElement("ROC lớn hơn");
            sort_ints.addElement(C._SORT_ROC_HIGHER);

            sort_names.addElement("Khối lượng tăng dần");
            sort_ints.addElement(C._SORT_VOLUME_IS_UP);

            sort_names.addElement("Cổ phiếu tích lũy");
            sort_ints.addElement(C._SORT_ACCUMULATION);

            sort_names.addElement("MACD hội tụ về Zero");
            sort_ints.addElement(C._SORT_MACD_CONVERGENCY);

            sort_names.addElement("NVI Bullish");
            sort_ints.addElement(C._SORT_BULLISH_NVI);

            //  SMA
            sort_names.addElement("SMA(9) cắt đường giá");
            sort_ints.addElement(C._SORT_SMA_PRICE_9);
            sort_names.addElement("SMA(26) cắt đường giá");
            sort_ints.addElement(C._SORT_SMA_PRICE_26);

            mSortTechnicalName[1] = sort_names;
            mSortTechnicalParams[1] = sort_ints;
        }

        public bool isExpiredAccount()
        {
            if (mIsTrialAcc == false && mIsPurchaseAcc == false)
                return true;

            return false;
        }

        float getAjustFont()
        {
            float t = mFontAdjust;
            t /= 2.0f;

            return t;
        }

        xVectorInt mMarketControlTab0 = new xVectorInt();
        xVectorInt mMarketControlTab1 = new xVectorInt();
        public void setMarketControlTab(int marketID, int tabIdx)
        {
            for (int i = 0; i < mMarketControlTab0.size(); i++)
            {
                int mid = mMarketControlTab0.elementAt(i);
                if (mid == marketID)
                {
                    mMarketControlTab1.setElementAt(i, tabIdx);
                    return;
                }
            }

            mMarketControlTab0.addElement(marketID);
            mMarketControlTab1.addElement(tabIdx);
        }

        public int getMarketControlTab(int marketID)
        {
            for (int i = 0; i < mMarketControlTab0.size(); i++)
            {
                int mid = mMarketControlTab0.elementAt(i);
                if (mid == marketID)
                    return mMarketControlTab1.elementAt(i);
            }

            return 0;
        }

        //==================================
        public void getGainLossValue(double[] values)
        {
            double tong = 0;
            double giatri = 0;
            double loinhuan;
            for (int i = 0; i < mGainLossManager.getTotal(); i++)
            {
                stGainloss g = mGainLossManager.getGainLossAt(i);

                tong += g.price * g.volume;

                stPriceboardState item = this.mPriceboard.getPriceboard(g.code);
                if (item != null)
                {
                    giatri += (item.getCurrentPrice() * g.volume);
                }
                else
                {

                }
            }

            loinhuan = giatri - tong;

            values[0] = tong;
            values[1] = giatri;
            values[2] = loinhuan;
        }
        
        //==================================

        public class ConfigJson
        {
            public int version;
            public string url_all_share2;
        };

        public ConfigJson configJson = new ConfigJson();

        void parseConfig()
        {
            try
            {
                xDataInput di = xFileManager.readFile("config.cfg", false);
                if (di != null)
                {
                    String s = di.readUTF();

                    try
                    {
                        configJson = JsonConvert.DeserializeObject<ConfigJson>(s);
                    }
                    catch (Exception e1)
                    {
                        Utils.trace(e1.Message);
                    }
                    //--------------------------------
                }

                bool shouldDownloadAgain = true;
                if (configJson == null || configJson.version == 0)
                {
                    WebClient webClient = new WebClient();
                    String s = webClient.DownloadString(URL_DATA_CONFIG);
                    if (s != null && s.Length > 100)
                    {
                        xDataOutput o = new xDataOutput(s.Length * 2);
                        o.writeUTF(s);
                        xFileManager.saveFile(o, "config.cfg");

                        shouldDownloadAgain = false;
                    }

                    configJson = JsonConvert.DeserializeObject<ConfigJson>(s);
                }
                else
                {
                }

                if (shouldDownloadAgain)
                {
                    //  download for next run
                    System.Threading.ThreadPool.QueueUserWorkItem(delegate {
                        try
                        {
                            WebClient webClient = new WebClient();
                            String s = webClient.DownloadString(URL_DATA_CONFIG);
                            if (s != null && s.Length > 100)
                            {
                                xDataOutput o = new xDataOutput(s.Length * 2);
                                o.writeUTF(s);
                                xFileManager.saveFile(o, "config.cfg");
                            }
                        }
                        catch (Exception e2)
                        {
                            Utils.trace(e2.Message);
                        }
                    }, null);
                }

                Utils.trace("config: version=" + configJson.version + " url=" + configJson.url_all_share2);
            }
            catch (Exception e)
            {
                Utils.trace(e.Message);
            }
        }

        stShareGroup getShareGroupLike(String name)
        {
	        for (int i = 0; i < mShareGroups.size(); i++)
	        {
		        stShareGroup g = (stShareGroup)mShareGroups.elementAt(i);

                String gn = g.getName();
                if (gn !=  null && gn.IndexOf(name) != -1)
                {
                    return g;
                }
	        }

	        return null;
        }
        xVector mMainMarketGroups = new xVector();
        public xVector getMainMarketGroups(bool clearOriginal)
        {
            if (clearOriginal)
            {
                mMainMarketGroups.removeAllElements();
            }

            if (mMainMarketGroups.size() == 0)
            {
                stShareGroup g;
                
                //  vnindex
                g = new stShareGroup();
                g.setType(stShareGroup.ID_GROUP_MARKET_OVERVIEW);
                g.setName("^VNINDEX");
                g.addCode("^VNINDEX");
                
                mMainMarketGroups.addElement(g);
                
                //  hastc
                g = new stShareGroup();
                g.setType(stShareGroup.ID_GROUP_MARKET_OVERVIEW);
                g.setName("^HASTC");
                g.addCode("^HASTC");
                
                mMainMarketGroups.addElement(g);
                
                //  upcom
                g = new stShareGroup();
                g.setType(stShareGroup.ID_GROUP_MARKET_OVERVIEW);
                g.setName("^UPCOM");
                
                //-------------------------
                /*
                stShareGroup g30 = getShareGroupLike("VN30");
                if (g30 != null){
                    for (int j = 0; j < g30.getTotal(); j++){
                        String code = g30.getCodeAt(j);
                        g.addCode(code);
                    }
                }*/
                //-------------------------
                
                mMainMarketGroups.addElement(g);
                
                //  hnx30
                /*
                 g = new stShareGroup();
                 g.setName("^HNX30");
                 g.addCode("^HNX30");
                 
                 mMainMarketGroups.addElement(g);
                 */
                int largeCap = 10000;//[GlobalData getValueAsInt:@"large_cap" defaultV:10000];
                int lowCap = 1000;//[GlobalData getValueAsInt:@"low_cap" defaultV:1000];
                
                stShareGroup gLowCap = new stShareGroup();
                gLowCap.setType(stShareGroup.ID_GROUP_MARKET_OVERVIEW);
                gLowCap.setName("LowCap");
                mMainMarketGroups.addElement(gLowCap);
                
                stShareGroup gMidCap = new stShareGroup();
                gMidCap.setType(stShareGroup.ID_GROUP_MARKET_OVERVIEW);
                gMidCap.setName("MidCap");
                mMainMarketGroups.addElement(gMidCap);
                
                stShareGroup gLargeCap = new stShareGroup();
                gLargeCap.setType(stShareGroup.ID_GROUP_MARKET_OVERVIEW);
                gLargeCap.setName("LargeCap");
                mMainMarketGroups.addElement(gLargeCap);
                
                //  Share
                int cnt = Context.getInstance().mShareManager.getTotalShareIDCount();// .getShareCount();
                int[] market = { 0 };
                for (int i = 0; i < cnt; i++)
                {
                    market[0] = 0;

                    int shareID = Context.getInstance().mShareManager.getShareIDAt(i, market);

                    Share share = Context.getInstance().mShareManager.getShare(shareID);

                    if (share == null || share.getCode() == null || share.getShareID() <= 0){
                        continue;
                    }

                    stPriceboardState ps = mPriceboard.getPriceboard(shareID);
                    stCompanyInfo inf = mShareManager.getCompanyInfo(shareID);

                    //if (share.getCode() == "HNF")
                    //{
                        //Utils.trace("");
                    //}
                    
                    if (ps != null && inf != null && ps.getMarketID() == 1)
                    {
                        float price = ps.current_price_1;
                        if (price == 0){
                            share.loadShareFromCommonData(true);
                            price = share.getClose(share.getCandleCnt()-1);
                        }
                        
                    
                        //NSString *s = [NSString stringWithFormat:@"code=%s; price=%.2f; vol=%d", share.mCode, price, inf.volume];
                        //NSLog(@"--%@", s);
                        
                        if (price > 0)
                        {
                            double cap = inf.volume*price;
                            cap /= 1000;    //  translate to billion VND
                            
                            if (cap <= lowCap){
                                gLowCap.addCode(share.getCode());
                            }
                            else if (cap >= largeCap){
                                gLargeCap.addCode(share.getCode());
                            }
                            else{
                                gMidCap.addCode(share.getCode());
                            }
                        }
                    }
                }// for
            }
            return mMainMarketGroups;
        }

        public NetProtocol createNetProtocol()
        {
            ServerURL = "http://soft123.com.vn:8080/SmaSrv/SSTK";
            if (ServerURL == null)
            {
                return null;
            }

            NetProtocol net = new NetProtocol();
            net.setServerURL(ServerURL);

            //  for test only
            //net.setServerURL("http://192.168.1.30:8080/srv/");

            return net;
        }

    }

}
