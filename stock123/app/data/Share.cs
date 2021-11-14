/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
/**
 *
 * @author ThuyPham
 */
using System;
using System.Collections.Generic;
using System.Text;
using xlib.framework;
using xlib.utils;
using stock123.app.chart;

namespace stock123.app.data
{
    public class Share
    {
        public static int SCOPE_1WEEKS = 5;
        public static int SCOPE_1MONTH = 26;
        public static int SCOPE_3MONTHS = 78;
        public static int SCOPE_6MONTHS = 156;
        public static int SCOPE_1YEAR = 260;
        public static int SCOPE_2YEAR = 520;
        public static int SCOPE_5YEAR = 1200;
        public static int SCOPE_ALL = 10000;

        public static int CANDLE_1MIN = 1;
        public static int CANDLE_5MINS = 5;
        public static int CANDLE_10MINS = 10;
        public static int CANDLE_15MINS = 15;
        public static int CANDLE_30MINS = 30;
        public static int CANDLE_60MINS = 60;

        public static int CANDLE_DAILY = 0;
        public static int CANDLE_WEEKLY = 1;
        public static int CANDLE_MONTHLY = 2;

        public static int SHARE_CODE_LENGTH = 8;
        //static  int FILE_VERSION = 0x02;

        static int CANDLE_SIZE = 28;
        static int CANDLE_SIZE_NETWORK = 28;
        //===============for drawing chart: 52K===============
        public static int MAX_CANDLE_CHART_COUNT = 10000;
        public float[] pSMA1;
        public float[] pSMA2;
        public float[] pSMA3;
        public float[] pSMA4;
        public float[] pSMA5;

        public float[] pPastPrice1;
        public float[] pPastPrice2;

        public float[] pEMA;
        public float[] pRSI;
        public float[] pWilliamR;
        public float[] pRSISecond;
        //=====for macd=====
        public float[] pMACD;
        public float[] pMACDSignal9;
        public float[] pMACDHistogram;
        public float[] pMFI;
        public float[] pMFISecond;
        public float[] pPSAR;
        public bool[] pSAR_SignalUp;

        public float[] pVSTOP;
        public bool[] pVSTOP_SignalUp;

        public float[] pEMAIndicator;
        public float[] pTMP;
        public float[] pTMP1;
        public float[] pTMP2;
        public float[] pTMP3;

        public double[] pDouble1;
        public double[] pDouble2;

        public float[] pBBUpper;
        public float[] pBBLower;

        public float[] pADX;
        public float[] pPLUS_DI;
        public float[] pMINUS_DI;

        public float[] pTrix;
        public float[] pTrixEMA;

        public float[] pSMA_Envelop;

        //========ichimoku=========
        public float[] pTenkansen;
        public float[] pKijunsen;
        public float[] pChikouSpan;
        public float[] pSpanA;
        public float[] pSpanB;
        //=========stochastic========
        public float[] pStochasticFastK;
        public float[] pStochasticFastD;
        public float[] pStochasticSlowK;
        public float[] pStochasticSlowD;

        public float[] pChaikinOscillator;
        public float[] pChaikinOscillatorSecond;

        public float[] pADL;
        public float[] pADL_SMA0;
        public float[] pADL_SMA1;

        public float[] pROC;
        public float[] pROCSecond;

        public float[] pSMAVolume;
        //  thuyps
        public float[] pSMAThuyPS;
        public float[] pSMAThuyPS1;

        public float[] pOBV;

        public float[] pNVI;
        public float[] pNVI_EMA1;
        public float[] pNVI_EMA2;

        public float[] pPVI;
        public float[] pPVI_EMA1;
        public float[] pPVI_EMA2;
        //  new
        public float[] pPVT;
        public float[] pPVT_EMA1;

        public float[] pCCI;

        //public static float[] pVSTOP = new float[MAX_CANDLE_CHART_COUNT];
        //public static bool[] pVSTOP_SignalUp = new bool[MAX_CANDLE_CHART_COUNT];

        public float[] pMassIndex;
        public float[] pATR;
        
        public float[] pComparingPrice;
        public float[] pCRS;
        public float[] pCRS_MA1;
        public float[] pCRS_MA2;

        public float[] pCRS_Percent;
        public float[] pCRS_MA1_Percent;
        public float[] pCRS_MA2_Percent;
        //===============================================
        //  shared memory
        static public int[] mSharedCVolume;
        static float[] mSharedCOpen;
        static public float[] mSharedCClose;
        static float[] mSharedCHighest;
        static float[] mSharedCLowest;
        static float[] mSharedCRef;
        static int[] mSharedCDate;
        static int[] mSharedNNMua;    //  volume
        static int[] mSharedNNBan;
        bool mIsUsingSharedMemory;

        static public float[] pStaticTMP;
        static public float[] pStaticTMP1;
        static public float[] pStaticTMP2;

        //================================================

        //  candles
        public String mCode;
        public String mName;
        public int mMarketID;
        public int mCandleCnt;
        public int[] mCVolume;

        //----------------------------------
        public int[] mCDate;

        public float[] mCOpen;
        public float[] mCClose;
        public float[] mCHighest;
        public float[] mCLowest;
        public float[] mCRef;

        public int[] mNNMua;    //  volume
        public int[] mNNBan;
        //=======================
        public int mBeginIdx;
        public int mEndIdx;
        int mCursor;
        int mHighestCandle;
        int mLowestCandle;
        public int mSelectedCandle;
        public int mModifiedKey;
        public int mModifiedKey2;
        float mHiPrice;
        float mLowPrice;

        int mLastScope;
        int mLastBegin;
        int mLastEnd;

        public int mCandleType;

        //========for sorting
        public double mSortParam;
        public String mCompareText;
        //=============flags=============
        public bool mIsCalcSMA;
        public bool mIsCalcRSI;
        public bool mIsCalcStochRSI;
        public bool mIsCalcOBV;
        public bool mIsCalcADL;
        public bool mIsCalcChaikin;
        public bool mIsCalcROC;
        public bool mIsCalcNVI;
        public bool mIsCalcPVI;
        public bool mIsCalcMACD;
        public bool mIsCalcBollinger;
        public bool mIsCalcPSAR;
        public bool mIsCalcMFI;
        public bool mIsCalcADX;
        public bool mIsCalcIchimoku;
        public bool mIsCalcStochastic;
        public bool mIsCalcSMAVolume;
        public bool mIsCalcTrix;
        public bool mIsCalcEnvelop;
        public bool mIsCalcWilliamR;

        public bool mIsCalcPVT;
        public bool mIsCalcCCI;

        public bool mIsCalcComparingShare;
        //===============================

        bool mShouldSaveData = false;

        public int mID = -1;
        public bool mIsRealtime = false;
        public bool mIsGroupIndex = false;

        public bool mIs1YearChartOn = false;
        public bool mIs2YearChartOn = false;

        public float m1YearChartHigh = 0;
        public float m1YearChartLow = 0;
        public float m2YearChartHigh = 0;
        public float m2YearChartLow = 0;

        public bool mIsComparingChart = false;
        public float mCompare2ShareChartHigh = 0;
        public float mCompare2ShareChartLo = 0;
        //public String mCompare2ShareCode;

        public int mVolumeDivided = 1;
        //=====================================================================

        public Share()
        {

        }

        public Share(int candleCnt)
        {
            allocPrivateMemory(candleCnt);
        }

        public bool isRealtime()
        {
            return mIsRealtime;
        }

        public void setCode(String code, int marketID)
        {
            mCode = code;
            if (marketID == -1)
            {
                Context ctx = Context.getInstance();
                int id = ctx.mShareManager.getShareID(code);
                marketID = ctx.mShareManager.getShareMarketID(id);
            }
            mMarketID = (byte)marketID;
        }

        public void setCode(byte[] code, int off, int martketID)
        {
            mCode = Utils.bytesNullTerminatedToString(code, off, SHARE_CODE_LENGTH);
            mMarketID = (byte)martketID;
        }

        public void setID(int id)
        {
            mID = id;
        }

        public int getID()
        {
            return mID;
        }
        int mLastCandleDate = 0;
        public int getTrueLastCandleDate()
        {
            return mLastCandleDate;
        }

        public void appendTodayCandle2()
        {
            stCandle c = getTodayCandle(true);

            if (c != null && c.open > 0 && getLastCandleDate() < c.date)
            {
                int last = getCandleCnt();
                mCClose[last] = c.close;
                mCOpen[last] = c.open;
                mCHighest[last] = c.highest;
                mCLowest[last] = c.lowest;
                mCDate[last] = c.date;
                mCVolume[last] = c.volume;
                mCRef[last] = c.open;

                mCandleCnt++;
            }
            //===================================
            mEndIdx = getCandleCnt() - 1;
            if (mEndIdx < 0)
                mEndIdx = 0;
            setCursorScope(mCurrentScope);
        }

        public void appendTodayCandle()
        {
            if (getCandleCnt() < 3){
                return;
            }
            stCandle c = getTodayCandle(false);
            if (c != null)
            {
                if (getLastCandleDate() == c.date)
                {
                    int last = getCandleCnt() - 1;
                    if (mCVolume[last] < c.volume){
                        mCClose[last] = c.close;
                        mCOpen[last] = c.open;
                        mCHighest[last] = c.highest;
                        mCLowest[last] = c.lowest;
                        mCDate[last] = c.date;
                        mCVolume[last] = c.volume;
                        mCRef[last] = c.open;
                    }
                }
                else if (getLastCandleDate() < c.date)
                {
                    int last = getCandleCnt();
                    mCClose[last] = c.close;
                    mCOpen[last] = c.open;
                    mCHighest[last] = c.highest;
                    mCLowest[last] = c.lowest;
                    mCDate[last] = c.date;
                    mCVolume[last] = c.volume;
                    mCRef[last] = c.open;

                    mCandleCnt++;
                }
            }

            mEndIdx = getCandleCnt() - 1;
            if (mEndIdx < 0)
                mEndIdx = 0;
            setCursorScope(mCurrentScope);
        }

        static stCandle todayCandle = new stCandle();

        public stCandle getTodayCandle(bool useLastPriceIfNotTrade)
        {
            todayCandle.close = 0;
            todayCandle.date = 0;
            todayCandle.highest = 0;
            todayCandle.lowest = 0;
            todayCandle.volume = 0;
            //=======================
            Context ctx = Context.getInstance();

            if (isIndex())
            {
                stPriceboardStateIndex pi = ctx.mPriceboard.getPriceboardIndexOfMarket(mMarketID);
                if (pi != null && getLastCandleDate() < ctx.mPriceboard.mDate && getCandleCount() > 0)
                {
                    //mLastCandleDate = ctx.mPriceboard.mDate;
                    int last = getCandleCount();
                    float division = 1;
                    float price = pi.current_point / division;

                    if (price == 0 || pi.reference == 0)
                    {
                        return null;
                    }
                    TradeHistory trade = Context.getInstance().getTradeHistory(pi.id);
                    if (trade.mOpen > 0 && trade.mHighest > 0 && trade.mLowest > 0)
                    {
                        todayCandle.close = price;
                        todayCandle.open = trade.mOpen / division;
                        todayCandle.highest = trade.mHighest / division;
                        todayCandle.lowest = trade.mLowest / division;
                    }
                    else
                    {
                        todayCandle.close = price;
                        todayCandle.close = mCClose[last];
                        //mCRef[last] = pi.reference / division;
                        todayCandle.highest = pi.reference / division;
                        todayCandle.lowest = pi.reference / division;
                    }

                    todayCandle.volume = (int)pi.total_volume;
                    todayCandle.date = ctx.mPriceboard.mDate;
                }
                else
                {
                    int last = getCandleCnt() - 1;
                    if (last >= 0)
                    {
                        todayCandle.close = mCClose[last];
                        todayCandle.open = mCOpen[last];
                        todayCandle.highest = mCHighest[last];
                        todayCandle.lowest = mCLowest[last];

                        todayCandle.volume = mCVolume[last];
                        todayCandle.date = mCDate[last];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                stPriceboardState ps = ctx.mPriceboard.getPriceboard(mID);
                if (ps != null && getLastCandleDate() < ctx.mPriceboard.getDate())
                {
                    int last = getCandleCount();
                    float division = 1;
                    float price = ps.getCurrentPrice();
                    float open = price;
                    float hi = ps.getMax();
                    float lo = ps.getMin();

                    int volume = ps.getTotalVolume();

                    //  check hi/lo valid
                    if ((hi == 0 || lo == 0))
                    {
                        TradeHistory trade = Context.getInstance().getTradeHistory(mID);
                        float[] hl = new float[2];
                        if (trade != null && trade.getHiLo(hl))
                        {
                            if (hi == 0) hi = hl[0];
                            if (lo == 0) lo = hl[1];
                            open = trade.getPrice(0);
                        }
                    }

                    if (price == 0)
                    {
                        if (useLastPriceIfNotTrade)
                        {
                            price = getClose();
                            open = price;
                            hi = price;
                            lo = price;
                            volume = 0;
                        }
                        else
                        {
                            return null;
                        }
                    }

                    open = Context.getInstance().mPriceboard.getOpen(mID);
                    if (open == 0)
                    {
                        open = price;
                    }
                    if (hi == 0) hi = open > price ? open : price;
                    if (lo == 0) lo = open < price ? open : price;
                    if (lo == 0) lo = hi;
                    //---------------------------------------------

                    if (price == 0 || hi == 0 || lo == 0)
                        return null;

                    ctx.mPriceboard.mDate = ctx.mPriceboard.getDate();

                    if (last >= 9999 || mCClose == null)
                    {
                        last = last + 1;
                        last--;
                    }

                    todayCandle.close = price / division;
                    todayCandle.open = open / division;
                    todayCandle.highest = hi / division;
                    todayCandle.lowest = lo / division;

                    todayCandle.volume = volume;
                    todayCandle.date = ctx.mPriceboard.getDate();
                }
                else
                {
                    int last = getCandleCnt()-1;
                    if (last >= 0)
                    {
                        todayCandle.close = mCClose[last];
                        todayCandle.open = mCOpen[last];
                        todayCandle.highest = mCHighest[last];
                        todayCandle.lowest = mCLowest[last];

                        todayCandle.volume = mCVolume[last];
                        todayCandle.date = mCDate[last];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return todayCandle;
        }

        public void loadShareFromCommonData(bool appendToday)
        {
            if (isIndex())
            {
                return;
            }

            clearCalculations();

            mLastScope = -1;
            mCandleType = CANDLE_DAILY;

            //appendToday = true;
            removeAllCandles();
            Context ctx = Context.getInstance();
            ctx.mShareManager.loadShareFromCommon(this, -1, false);

            if (appendToday)
            {
                appendTodayCandle();
            }

            this.setCursorScope(Context.getInstance().mOptHistoryChartTimeFrame);
        }

        public void loadShareFromCommonData(bool useSharedMemory, bool appendToday)
        {
            clearCalculations();
            allocMemoryUsingShared(useSharedMemory);

            mLastScope = -1;
            mCandleType = CANDLE_DAILY;

            //appendToday = true;
            removeAllCandles();
            Context ctx = Context.getInstance();
            ctx.mShareManager.loadShareFromCommon(this, -1, false);

            if (appendToday)
            {
                appendTodayCandle();
            }

            this.setCursorScope(Context.getInstance().mOptHistoryChartTimeFrame);
        }

        public bool loadShareFromFile(bool appendToday)
        {
            mVolumeDivided = 1;
            if (isRealtime())
            {
                appendToday = false;
            }

            clearCalculations();
            mCandleType = CANDLE_DAILY;
            mLastScope = -1;
            //appendToday = true;
            if (mCode == null || mCode.Length == 0)
                return false;
            mLastCandleDate = 0;
            removeAllCandles();
            try
            {
                xDataInput di = xFileManager.readFile("data\\" + getShareName(), false);
                if (di != null)
                {
                    int ver = di.readShort();
                    if (ver != Context.FILE_SHARE_DATA_VERSION)
                    {
                        return false;
                    }
                    mCode = di.readUTFString();
                    mMarketID = (byte)di.readByte();
                    mCandleCnt = di.readInt();

                    if (mCandleCnt == 0 || mCandleCnt > MAX_CANDLE_CHART_COUNT)
                    {
                        return false;
                    }

                    loadShare(di);
                    //=====Thay cac data voi common data============
                    if (isIndex() == false && getID() != 100000 && !mIsRealtime)
                    {
                        Context.getInstance().mShareManager.replace1ShareDataToCommon(this);
                    }
                    if (mEndIdx >= mCandleCnt)
                    {
                        mEndIdx = mCandleCnt - 1;
                    }
                    //===========================

                    mLastCandleDate = 0;
                    if (mCandleCnt > 0)
                        mLastCandleDate = mCDate[mCandleCnt - 1];

                    if (appendToday)
                    {
                        appendTodayCandle();
                        mEndIdx = mCandleCnt - 1;
                        setCursorScope(Context.getInstance().mOptHistoryChartTimeFrame);
                    }

                    return true;
                }
            }catch(Exception e)
            {
                Utils.trace(e.Message);
            }
            return false;
        }


        public void unloadShare()
        {
            mCDate = null;

            mCOpen = null;
            mCClose = null;
            mCHighest = null;
            mCLowest = null;
            mCRef = null;

            mNNMua = null;
            mNNBan = null;

            mCVolume = null;
        }

        //  load share from saved file
        bool loadShare()
        {
            mVolumeDivided = 1;
            if (isIndex())
            {
                loadShareFromFile(false);
                return true;
            }
            //===========normal quote===============
            if (loadShareFromFile(false))
            {

            }
            else
            {
                //  no separated saved file
                //  if is OFFLINE_MODE, try to load from common saved file
                if (true)//!Context.getInstance().isOnline())
                {
                    Context.getInstance().mShareManager.loadShareFromCommon(this, -1, false);
                }
            }
            if (isRealtime())
            {
                setCursorScope(SCOPE_ALL);
            }
            else
            {
                setCursorScope(Context.getInstance().mOptHistoryChartTimeFrame);
            }
            //============some calculations for sorting==============
            calcAvgVolume(10, 0);
            //==========================================

            return mCandleCnt > 0;
        }
                //public static float[] pVSTOP = new float[MAX_CANDLE_CHART_COUNT];
        //public static bool[] pVSTOP_SignalUp = new bool[MAX_CANDLE_CHART_COUNT];

        void initIndicatorMemory(int indicator)
        {
            bool realloc = false;
            int maxCandle = MAX_CANDLE_CHART_COUNT;
            if (pTMP != null && pTMP.Length < maxCandle)
            {
                realloc = true;
            }
            switch (indicator)
            {
                case ChartBase.CHART_ADL:
                    if (pADL == null || realloc)
                    {
                        pADL = new float[maxCandle];
                        pADL_SMA0 = new float[maxCandle];
                        pADL_SMA1 = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_ADX:
                    if (pADX == null || realloc){
                        pADX = new float[maxCandle];
                        pPLUS_DI = new float[maxCandle];
                        pMINUS_DI = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_AROON:
                case ChartBase.CHART_AROON_OSCILLATOR:
                    break;
                case ChartBase.CHART_ATR:
                    if (realloc || pATR == null)
                    {
                        pATR = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_BOLLINGER:
                    if (realloc || pBBUpper == null)
                    {
                        pBBUpper = new float[maxCandle];
                        pBBLower = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_CANDLE:
                case ChartBase.CHART_CANDLE_HEIKEN:
                    break;
                case ChartBase.CHART_CCI:
                    if (realloc || pCCI == null)
                    {
                        pCCI = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_CFM:
                    break;
                case ChartBase.CHART_CHAIKIN:
                    if (realloc || pChaikinOscillator == null)
                    {
                        pChaikinOscillator = new float[maxCandle];
                        pChaikinOscillatorSecond = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_ENVELOP:
                    if (realloc || pSMA_Envelop == null)
                    {
                        pSMA_Envelop = new float[maxCandle];
                    }
                    break;

                case ChartBase.CHART_ICHIMOKU:
                    if (realloc || pTenkansen == null)
                    {
                        pTenkansen = new float[maxCandle];
                        pKijunsen = new float[maxCandle];
                        pChikouSpan = new float[maxCandle];
                        pSpanA = new float[maxCandle];
                        pSpanB = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_MACD:
                    if (realloc || pMACD == null)
                    {
                        pMACD = new float[maxCandle];
                        pMACDSignal9 = new float[maxCandle];
                        pMACDHistogram = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_MAE:
                    break;
                case ChartBase.CHART_MASSINDEX:
                    if (realloc || pMassIndex == null)
                    {
                        pMassIndex = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_MFI:
                    if (realloc || pMFI == null)
                    {
                        pMFI = new float[maxCandle];
                        pMFISecond = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_NVI:
                    if (realloc || pNVI == null)
                    {
                        pNVI = new float[maxCandle];
                        pNVI_EMA1 = new float[maxCandle];
                        pNVI_EMA2 = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_OBV:
                    if (realloc || pOBV == null)
                    {
                        pOBV = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_OHLC:
                    break;
                case ChartBase.CHART_COMPARING_SECOND_SHARE:
                    if (realloc || pComparingPrice == null)
                    {
                        pComparingPrice = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_CRS_RATIO:
                    if (realloc || pCRS == null)
                    {
                        pCRS = new float[maxCandle];
                        pCRS_MA1 = new float[maxCandle];
                        pCRS_MA2 = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_CRS_PERCENT:
                    if (realloc || pCRS_Percent == null)
                    {
                        pCRS_Percent = new float[maxCandle];
                        pCRS_MA1_Percent = new float[maxCandle];
                        pCRS_MA2_Percent = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_PAST_1_YEAR:
                    if (realloc || pPastPrice1 == null)
                    {
                        pComparingPrice = new float[maxCandle];
                        pPastPrice1 = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_PAST_2_YEARS:
                    if (realloc || pPastPrice2 == null)
                    {
                        pComparingPrice = new float[maxCandle];
                        pPastPrice2 = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_PSAR:
                    if (realloc || pPSAR == null)
                    {
                        pPSAR = new float[maxCandle];
                        pSAR_SignalUp = new bool[maxCandle];
                    }
                    break;
                case ChartBase.CHART_PVI:
                    if (realloc || pPVI == null)
                    {
                        pPVI = new float[maxCandle];
                        pPVI_EMA1 = new float[maxCandle];
                        pPVI_EMA2 = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_PVO:
                    break;
                case ChartBase.CHART_PVT:
                    if (realloc || pPVT == null)
                    {
                        pPVT = new float[maxCandle];
                        pPVT_EMA1 = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_ROC:
                    if (realloc || pROC == null)
                    {
                        pROC = new float[maxCandle];
                        pROCSecond = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_RSI:
                    if (realloc || pRSI == null)
                    {
                        pRSI = new float[maxCandle];
                        pRSISecond = new float[maxCandle];
                    }

                    break;
                case ChartBase.CHART_SMA:
                    if (realloc || pSMA1 == null)
                    {
                        pSMA1 = new float[maxCandle];
                        pSMA2 = new float[maxCandle];
                        pSMA3 = new float[maxCandle];
                        pSMA4 = new float[maxCandle];
                        pSMA5 = new float[maxCandle];

                    }
                    break;
                case ChartBase.CHART_STOCHASTIC_FAST:
                case ChartBase.CHART_STOCHASTIC_SLOW:
                    if (realloc || pStochasticFastK == null)
                    {
                        pStochasticFastK = new float[maxCandle];
                        pStochasticFastD = new float[maxCandle];
                        pStochasticSlowK = new float[maxCandle];
                        pStochasticSlowD = new float[maxCandle];
                    }
                    if (realloc || pRSI == null)
                    {
                        pRSI = new float[maxCandle];
                        pRSISecond = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_TRIX:
                    if (realloc || pTrix == null)
                    {
                        pTrix = new float[maxCandle];
                        pTrixEMA = new float[maxCandle];
                    }
                    break;

                case ChartBase.CHART_VSTOP:
                    if (realloc || pVSTOP == null)
                    {
                        pVSTOP = new float[maxCandle];
                        pVSTOP_SignalUp = new bool[maxCandle];
                    }
                    break;
                case ChartBase.CHART_WILLIAMR:
                    if (realloc || pWilliamR == null)
                    {
                        pWilliamR = new float[maxCandle];
                    }
                    break;
                case ChartBase.CHART_ZIGZAG:
                    break;
            }

            if (pTMP == null || realloc)
            {
                pTMP = new float[maxCandle];
                pTMP1 = new float[maxCandle];
                pTMP2 = new float[maxCandle];

                pTMP3 = new float[maxCandle];
                pDouble1 = new double[maxCandle];
                pDouble2 = new double[maxCandle];

                pSMAVolume = new float[maxCandle];

                pEMA = new float[maxCandle];
                pEMAIndicator = new float[maxCandle];
            }

        }

        public bool isUsingShareMemory()
        {
            return mCClose == mSharedCClose;
        }

        public void allocMemoryUsingShared(bool useSharedMemory)
        {
            if (isIndex())
            {
                useSharedMemory = false;
            }
            if (useSharedMemory)
            {
                if (mSharedCClose == null)
                {
                    mSharedCVolume = new int[MAX_CANDLE_CHART_COUNT];
                    mSharedCDate = new int[MAX_CANDLE_CHART_COUNT];
                    mSharedCOpen = new float[MAX_CANDLE_CHART_COUNT];
                    mSharedCClose = new float[MAX_CANDLE_CHART_COUNT];
                    mSharedCHighest = new float[MAX_CANDLE_CHART_COUNT];
                    mSharedCLowest = new float[MAX_CANDLE_CHART_COUNT];
                    mSharedCRef = new float[MAX_CANDLE_CHART_COUNT];
                    mSharedNNMua = new int[MAX_CANDLE_CHART_COUNT];
                    mSharedNNBan = new int[MAX_CANDLE_CHART_COUNT];

                    pStaticTMP = new float[MAX_CANDLE_CHART_COUNT];
                    pStaticTMP1 = new float[MAX_CANDLE_CHART_COUNT];
                    pStaticTMP2 = new float[MAX_CANDLE_CHART_COUNT];
                }
                mCVolume = mSharedCVolume;
                mCDate = mSharedCDate;
                mCOpen = mSharedCOpen;
                mCClose = mSharedCClose;
                mCHighest = mSharedCHighest;
                mCLowest = mSharedCLowest;
                mCRef = mSharedCRef;
                mNNMua = mSharedNNMua;
                mNNBan = mSharedNNBan;
                mIsUsingSharedMemory = true;
            }
            else
            {
                if (mCVolume == null)
                {
                    allocPrivateMemory(MAX_CANDLE_CHART_COUNT);

                }
            }
        }

        void allocPrivateMemory(int maxCandle)
        {
            mCVolume = new int[maxCandle];
            mCDate = new int[maxCandle];
            mCOpen = new float[maxCandle];
            mCClose = new float[maxCandle];
            mCHighest = new float[maxCandle];
            mCLowest = new float[maxCandle];
            mCRef = new float[maxCandle];
            mNNMua = new int[maxCandle];
            mNNBan = new int[maxCandle];

            mIsUsingSharedMemory = false;
        }

        public void loadShare(xDataInput di)
        {
            float tmp;
            for (int i = 0; i < mCandleCnt; i++)
            {
                mCOpen[i] = di.readFloat();
                mCClose[i] = di.readFloat();
                mCHighest[i] = di.readFloat();
                mCLowest[i] = di.readFloat();
                mCRef[i] = di.readFloat();
                //di.readInt();   //  skip ce
                mCVolume[i] = di.readInt();
                mNNMua[i] = di.readInt();
                mNNBan[i] = di.readInt();
                mCDate[i] = di.readInt();

                if (mCVolume[i] == 0 && i > 1)
                {
                    mCOpen[i] = mCClose[i] = mCHighest[i] = mCLowest[i] = mCClose[i - 1];
                }

                //==correct data
                tmp = 0;
                if (mCOpen[i] > 0) tmp = mCOpen[i];
                if (tmp == 0 && mCClose[i] > 0) tmp = mCClose[i];
                if (tmp == 0 && mCHighest[i] > 0) tmp = mCHighest[i];
                if (tmp == 0 && mCRef[i] > 0) tmp = mCRef[i];

                if (mCOpen[i] <= 0) mCOpen[i] = tmp;
                if (mCClose[i] <= 0) mCClose[i] = tmp;
                if (mCHighest[i] <= 0) mCHighest[i] = tmp;
                if (mCLowest[i] <= 0) mCLowest[i] = tmp;
                if (mCRef[i] <= 0) mCRef[i] = tmp;
            }

            if (isRealtime())
            {
                setCursorScope(SCOPE_ALL);
            }
            else
            {
                setCursorScope(Context.getInstance().mOptHistoryChartTimeFrame);
            }
            /*
            if (mCode != null)
                Context.getInstance().mShareManager.addSavedShare(mCode);
             */
        }

        //static  int NET_CANDLE_SIZE = 32;
        public void processNetData(int cnt, xDataInput di)
        {
            //int cnt = di.available()/NET_CANDLE_SIZE;

            //if (cnt == 0)
            //    return;
            //  save to file
            //==============
            // and reload
            addMoreData(cnt, di);
        }

        int[] realloc(int[] p, int newLength)
        {
            if (p != null)
            {
                if (p.Length >= newLength)
                {
                    return p;
                }
            }

            int[] newP = new int[newLength];
            if (p != null)
            {
                Buffer.BlockCopy(p, 0, newP, 0, p.Length*sizeof(int));
            }

            return newP;
        }

        float[] reallocf(float[] p, int newLength)
        {
            if (p != null)
            {
                if (p.Length >= newLength)
                {
                    return p;
                }
            }

            float[] newP = new float[newLength];
            if (p != null)
            {
                Buffer.BlockCopy(p, 0, newP, 0, p.Length * sizeof(float));
            }

            return newP;
        }

        void makeSureMemoryCapacity(int count)
        {
            bool ok = false;
            if (mCVolume != null && mCVolume.Length > count)
                ok = true;

            if (!ok)
            {
                int newRoom = count + 100;
                mCVolume = realloc(mCVolume, newRoom);
                mCDate = realloc(mCDate, newRoom);
                mCOpen = reallocf(mCOpen, newRoom);
                mCClose = reallocf(mCClose, newRoom);
                mCHighest = reallocf(mCHighest, newRoom);
                mCLowest = reallocf(mCLowest, newRoom);
                mCRef = reallocf(mCRef, newRoom);
                mNNMua = realloc(mNNMua, newRoom);
                mNNBan = realloc(mNNBan, newRoom);
            }
        }

        void addMoreData(int cnt, xDataInput di)
        {
            int addedCount = cnt;//di.available()/NET_CANDLE_SIZE;
            if (addedCount > 0)
            {
                mShouldSaveData = true;
            }
            else
                return;

            makeSureMemoryCapacity(mCandleCnt + addedCount);
            int lastDate = 0;
            if (mCandleCnt > 0)
            {
                lastDate = getTrueLastCandleDate();// mCDate[mCandleCnt - 1];
            }

            float o, c, h, l, r;
            int v, d;
            int nnmua, nnban;

            int j = mCandleCnt;
            float tmp;
            float max, min;

            float devision = 1000.0f;
            if (isIndex())
            {
                devision = 100.0f;
            }

            int checkDate = (2018 << 16) | (1 << 8) | 22;

            for (int i = mCandleCnt; i < mCandleCnt + addedCount; i++)
            {
                //if (i == addedCount - 2)
                //{
                  //  int kk = 0;
                //}
                o = di.readInt() / devision;
                c = di.readInt() / devision;
                h = di.readInt() / devision;
                l = di.readInt() / devision;
                r = di.readInt() / devision;
                //di.readInt();   //  skip ce
                v = di.readInt();

                nnmua = di.readInt();
                nnban = di.readInt();

                d = di.readInt();

                //==correct data
                tmp = 0;
                max = o < c ? c : o;
                min = o < c ? o : c;

                if (o > 0) tmp = o;
                if (tmp == 0 && c > 0) tmp = c;
                if (tmp == 0 && h > 0) tmp = h;
                if (tmp == 0 && r > 0) tmp = r;

                if (max <= 0) max = tmp;
                if (min <= 0) min = tmp;

                if (o <= 0) o = tmp;
                if (c <= 0) c = tmp;
                if (h <= 0) h = max;
                if (l <= 0) l = min;
                if (r <= 0) r = tmp;
                //---------------------------------------

                int xxx = (2011 << 16) | (12 << 8) | 14;
                if (d == xxx)
                {
                    d = xxx;
                }

                if (d <= lastDate)
                {
                    for (int k = mCandleCnt - 1; k > 0; k--)
                    {
                        if (mCDate[k] <= d)
                        {
                            if (mCDate[k] == d)
                            {
                                mCOpen[k] = o;
                                mCClose[k] = c;
                                mCHighest[k] = h;
                                mCLowest[k] = l;
                                mCRef[k] = r;
                                //di.readInt();   //  skip ce
                                mCVolume[k] = v;
                                mCDate[k] = d;

                                mNNMua[k] = nnmua;
                                mNNBan[k] = nnban;
                            }
                            break;
                        }
                    }
                    continue;
                }
                //---------------------

                lastDate = d;

                mCOpen[j] = o;
                mCClose[j] = c;
                mCHighest[j] = h;
                mCLowest[j] = l;
                mCRef[j] = r;
                //di.readInt();   //  skip ce
                mCVolume[j] = v;
                mCDate[j] = d;

                mNNMua[j] = nnmua;
                mNNBan[j] = nnban;
                j++;
            }

            mCandleCnt = j;

            setCursorScope(Context.getInstance().mOptHistoryChartTimeFrame);
        }

        public void saveShare()
        {
            if (mShouldSaveData == false)
            {
                return;
            }

            if (!isRealtime() && mCandleType != CANDLE_DAILY)
            {
                return;
            }

            mShouldSaveData = false;
            if (mCandleCnt == 0)
            {
                return;
            }

            //==================for managing saved shares============
            //Context.getInstance().mShareManager.addSavedShare(mCode);
            //==============================

            xDataOutput o = new xDataOutput(64 + mCandleCnt * CANDLE_SIZE);
            o.writeShort(Context.FILE_SHARE_DATA_VERSION);
            o.writeUTF(mCode);
            o.writeByte(mMarketID);
            o.writeInt(mCandleCnt);
            for (int i = 0; i < mCandleCnt; i++)
            {
                o.writeFloat(mCOpen[i]);
                o.writeFloat(mCClose[i]);
                o.writeFloat(mCHighest[i]);
                o.writeFloat(mCLowest[i]);
                o.writeFloat(mCRef[i]);
                //o.writeInt(0);  //  ce value, not used
                o.writeInt(mCVolume[i]);
                o.writeInt(mNNMua[i]);
                o.writeInt(mNNBan[i]);
                o.writeInt(mCDate[i]);
            }

            xFileManager.saveFile(o, "data\\" + getShareName());
        }

        String getShareName()
        {
            String md5 = Utils.MD5String(mCode);
            if (isRealtime())
            {
                return String.Format("{0}.rt", md5);
            }
            else
            {
                return md5;
            }
        }

        static public void deleteSavedFile(string code)
        {
            xFileManager.removeFile("data\\" + code);
            xFileManager.removeFile("data\\draw_" + code);
        }
        /*
            public void addMoreCandle(byte[] data, int offset, int len) {
                xDataInput in = new xDataInput(data, offset, len);
                int cnt = len / CANDLE_SIZE;
                int skip = 0;

                int lastDate = getLastCandleDate();
                for (int i = 0; i < cnt; i++) {
                    int d = di.readInt();
                    if (d > lastDate) {
                        break;
                    }

                    skip++;
                }

                int newCandleCnt = cnt - skip;
                if (newCandleCnt == 0) {
                    return;
                }
                int total = getCandleCount() + newCandleCnt;

                int removedShares = 0;
                int keptShares = mCandleCnt;
                if (total > MAX_CANDLE_COUNT) {
                    removedShares = total - MAX_CANDLE_COUNT;
                    keptShares -= removedShares;
                    total = MAX_CANDLE_COUNT;
                }

                int[] date = new int[total];
                int[] volumes = new int[total];
                int[] opens = new int[total];
                int[] closes = new int[total];
                int[] hi = new int[total];
                int[] lo = new int[total];
                int[] ref = new int[total];

                if (keptShares > 0) {
                    System.arraycopy(mCDate, removedShares, date, 0, keptShares);
                    System.arraycopy(mCVolume, removedShares, volumes, 0, keptShares);
                    System.arraycopy(mCOpen, removedShares, opens, 0, keptShares);
                    System.arraycopy(mCClose, removedShares, closes, 0, keptShares);
                    System.arraycopy(mCHighest, removedShares, hi, 0, keptShares);
                    System.arraycopy(mCLowest, removedShares, lo, 0, keptShares);
                    System.arraycopy(mCRef, removedShares, ref, 0, keptShares);
                }

                mCDate = date;
                mCVolume = volumes;
                mCOpen = opens;
                mCClose = closes;
                mCHighest = hi;
                mCLowest = lo;
                mCRef = ref;

                di.resetCursor();
                for (int i = 0; i < cnt; i++) {
                    if (i < skip) {
                        di.skip(CANDLE_SIZE_NETWORK);
                    } else {
                        mCDate[mCandleCnt] = di.readInt();
                        mCVolume[mCandleCnt] = di.readInt();
                        mCOpen[mCandleCnt] = di.readInt();
                        mCClose[mCandleCnt] = di.readInt();
                        mCHighest[mCandleCnt] = di.readInt();
                        mCLowest[mCandleCnt] = di.readInt();
                        mCRef[mCandleCnt] = di.readInt();
                        mCandleCnt++;
                    }
                }
            }
        */
        public bool canMove(int step)
        {
            if (step < 0)
            {
                if (mBeginIdx > 0)
                    return true;
            }
            else
            {
                if (mEndIdx < getCandleCount()-1)
                    return true;
            }
            return false;
        }
        public int moveCursor(int step)
        {
            int res = 0;
            if (step < 0)
            {
                while (mBeginIdx > 0 && step != 0)
                {
                    mEndIdx--;
                    mBeginIdx--;
                    step++;
                    res++;
                }
            }
            else
            {
                while ((mEndIdx < getCandleCount() - 1) && step != 0)
                {
                    mEndIdx++;
                    mBeginIdx++;
                    step--;
                    res--;
                }
            }
            updateHiloCandle();

            /*
            if (mLowestCandle != -1 && mHighestCandle != -1)
            {
                setLowPrice(mLowestCandle.lowest);
                setHiPrice(mHighestCandle.highest);
            }
             */

            return res;
        }

        public void setCursor(int offset)
        {
            if (offset == -1)
            {
                offset = mCandleCnt - 1;
            }
            if (offset >= 0)
            {
                mSelectedCandle = offset;
            }
        }

        public int getLastCandleDate()
        {
            if (mCandleCnt == 0)
            {
                return 0;
            }

            return mCDate[mCandleCnt - 1];
        }

        public int getCandleCnt()
        {
            return getCandleCount();
        }

        public int getCandleCount()
        {
            return mCandleCnt;
        }

        public int getMarketID()
        {
            return mMarketID;
        }

        public int getNNMua()
        {
            return mNNMua[mSelectedCandle];
        }

        public int getNNBan()
        {
            return mNNBan[mSelectedCandle];
        }

        public String getCode()
        {
            return mCode;
        }

        public int getShareID()
        {
            return getID();
        }
        //  only call this if getCandleCount() > 0. I do not verify mCursor because of speed optimization

        public int getDate()
        {
            return mCDate[mSelectedCandle];
        }

        public int getVolume()
        {
            if (getCandleCnt() == 0)
            {
                return 0;
            }
            return mCVolume[mSelectedCandle];
        }

        public float getOpen()
        {
            if (getCandleCnt() == 0)
            {
                return 0;
            }
            return mCOpen[mSelectedCandle];
        }

        public float getClose()
        {
            if (getCandleCnt() == 0)
            {
                return 0;
            }
            if (getCandleCnt() == 0)
            {
                return 0;
            }
            return mCClose[mSelectedCandle];
        }

        public float getHighest()
        {
            if (getCandleCnt() == 0)
            {
                return 0;
            }
            return mCHighest[mSelectedCandle];
        }

        public float getLowest()
        {
            if (getCandleCnt() == 0)
            {
                return 0;
            }
            return mCLowest[mSelectedCandle];
        }

        public float getRef()
        {
            if (getCandleCnt() == 0)
            {
                return 0;
            }
            float reference = 0;
            try
            {
                reference = mCRef[mSelectedCandle];
            }
            catch (Exception e)
            {
            }

            if (reference == 0)
            {
                Share share = Context.getInstance().mShareManager.getShare(mCode);
                if (share != null)
                {
                    if (share.getCandleCnt() == 0)
                    {
                        share.loadShareFromCommonData(false);
                        if (share.getCandleCnt() > 0)
                        {
                            reference = share.getClose(share.getCandleCnt() - 1);
                        }
                    }
                }
            }
            return reference;
        }

        public void replaceCandle(int i, stCandle c)
        {
            if (i >= 0 && i < getCandleCnt())
            {
                mCClose[i] = c.close;
                mCOpen[i] = c.open;
                mCHighest[i] = c.highest;
                mCLowest[i] = c.lowest;
                mCVolume[i] = c.volume;
            }
        }

        public stCandle getCandle(int i, stCandle c)
        {
            if (i < getCandleCount())
            {
                if (c == null)
                {
                    c = new stCandle();
                }
                c.open = getOpen(i);
                c.close = getClose(i);
                c.highest = getHighest(i);
                c.lowest = getLowest(i);
                c.date = getDate(i);
                c.volume = getVolume(i);
            }

            return c;
        }

        public stCandle getCandleByDate(int date, int minDate, stCandle c)
        {
            stCandle c1 = null;
            for (int i = getCandleCnt()-1; i >= 0; i--)
            {
                if (mCDate[i] < minDate)
                {
                    return c1;
                }
                if (mCDate[i] <= date)
                {
                    if (c == null)
                    {
                        c1 = new stCandle();
                    }
                    else
                    {
                        c1 = c;
                    }

                    c1.open = getOpen(i);
                    c1.close = getClose(i);
                    c1.highest = getHighest(i);
                    c1.lowest = getLowest(i);
                    c1.date = getDate(i);
                    c1.volume = getVolume(i);

                    return c1;
                }
            }

            return c1;
        }
        //=======================================

        public int getDate(int t)
        {
            return mCDate[t];
        }

        public int getVolume(int t)
        {
            return mCVolume[t];
        }

        public float getOpen(int t)
        {
            return mCOpen[t];
        }

        public float getClose(int t)
        {
            if (t < mCandleCnt && t >= 0)
            {
                return mCClose[t];
            }
            else
            {
                return 0;
            }
        }

        public float getHighest(int t)
        {
            return mCHighest[t];
        }

        public float getLowest(int t)
        {
            return mCLowest[t];
        }

        public float getRef(int t)
        {
            return mCRef[t];
        }

        public int getNNMua(int t)
        {
            return mNNMua[t];
        }

        public int getNNBan(int t)
        {
            return mNNBan[t];
        }
        //====================================loa======

        public float getHighestPriceIn(int begin, int end)
        {
            float highest = 0;
            for (int i = begin; i <= end; i++)
            {
                if (mCClose[i] > highest)
                {
                    highest = mCClose[i];
                }
            }

            return highest;
        }

        public float getHighestPrice()
        {
            float v = 0;
            {
                float h = mCHighest[mHighestCandle];
                float l = mCLowest[mLowestCandle];

                v = h + (h - l) / 5;
            }

            if (mIs1YearChartOn)
            {
                if (v < m1YearChartHigh)
                    v = m1YearChartHigh;
            }
            if (mIs2YearChartOn)
            {
                if (v < m2YearChartHigh)
                    v = m2YearChartHigh;
            }

            return v;
            //return mCHighest[mHighestCandle] + (mCHighest[mHighestCandle] * 5) / 100;
        }

        public float getLowestPrice()
        {
            float v = 0;
            //if (isRealtime())
            {
                float h = mCHighest[mHighestCandle];
                float l = mCLowest[mLowestCandle];

                v = l - (h - l) / 5;
            }

            if (mIs1YearChartOn)
            {
                if (v > m1YearChartLow)
                {
                    v = m1YearChartLow;
                }
            }
            if (mIs2YearChartOn)
            {
                if (v > m2YearChartLow)
                {
                    v = m2YearChartLow;
                }
            }

            return v;
            //return mCLowest[mLowestCandle] - (mCLowest[mLowestCandle] * 5) / 100;
        }

        public void selectCandle(int candleIdx)
        {
            if (candleIdx == -1)
            {
                int cnt = getCandleCount();
                if (cnt > 0)
                {
                    mSelectedCandle = cnt - 1;
                    if (mSelectedCandle < mBeginIdx || mSelectedCandle > mEndIdx)
                    {
                        setEndDate(mSelectedCandle);
                    }
                }

                return;
            }

            mSelectedCandle = candleIdx;
        }

        int mCurrentScope = SCOPE_3MONTHS;
        int mIntradayTimeFrame = CANDLE_1MIN;
        public void setCursorScope(int _scope)
        {
            mCurrentScope = _scope;

            int scope = _scope;
            if (_scope == SCOPE_1WEEKS)
            {
                scope = SCOPE_1WEEKS;
            }
            else if (_scope >= SCOPE_1MONTH)
            {
                if (mCandleType == CANDLE_WEEKLY)
                    scope = _scope / 5;
                else if (mCandleType == CANDLE_MONTHLY)
                    scope = _scope / 22;
                else
                    scope = _scope;
            }


            if (scope < 5)
                scope = 5;

            setStartEndIndex(getCandleCnt() - scope, getCandleCnt() - 1);

            mSelectedCandle = mEndIdx;
        }

        public void setStartEndIndex(int start, int end)
        {
            if (start < 0)
            {
                start = 0;
            }
            if (end >= getCandleCnt()){
                end = getCandleCnt()-1;
            }
            if (end > start)
            {
                mBeginIdx = start;
                mEndIdx = end;
            }

            updateHiloCandle();
            mModifiedKey = 0;
            mModifiedKey2 = (int)Utils.currentTimeMillis() % 1000;
        }

        public int getCursorScope()
        {
            return mCurrentScope;
        }

        //  for intraday
        public int getTimeFrame()
        {
            return mIntradayTimeFrame;
        }

        void updateHiloCandle()
        {
            mHighestCandle = getHighestCandleIdx();
            mLowestCandle = getLowestCandleIdx();

            mHiPrice = getHighest(mHighestCandle);
            mLowPrice = getLowest(mLowestCandle);
        }

        public int getHighestCandleIdx()
        {
            if (mBeginIdx < 0)
            {
                mBeginIdx = 0;
            }
            if (mEndIdx >= mCandleCnt)
            {
                mEndIdx = mCandleCnt - 1;
            }

            float v = -1;
            int h = mBeginIdx;

            for (int i = mBeginIdx; i <= mEndIdx; i++)
            {
                if (getHighest(i) > v)
                {
                    v = getHighest(i);
                    h = i;
                }
            }

            return h;
        }

        public int getLowestCandleIdx()
        {
            if (mBeginIdx < 0)
            {
                mBeginIdx = 0;
            }
            if (mEndIdx >= mCandleCnt)
            {
                mEndIdx = mCandleCnt - 1;
            }

            float v = 0xfffffff;
            int l = mBeginIdx;
            for (int i = mBeginIdx; i <= mEndIdx; i++)
            {
                if (getLowest(i) < v)
                {
                    v = getLowest(i);
                    l = i;
                }
            }

            return l;
        }

        public int moveChartView(int step)
        {
            int res = 0;
            if (step < 0)
            {
                while (mBeginIdx > 0 && step != 0)
                {
                    mEndIdx--;
                    mBeginIdx--;
                    step++;
                    res++;
                }
            }
            else
            {
                while ((mEndIdx < mCandleCnt - 1) && step != 0)
                {
                    mEndIdx++;
                    mBeginIdx++;
                    step--;
                    res--;
                }
            }
            updateHiloCandle();

            mModifiedKey++;
            mModifiedKey2++;

            return res;
        }


        public void moveCursor(bool back)
        {
            if (mSelectedCandle < mBeginIdx)
                mSelectedCandle = mBeginIdx;
            if (mSelectedCandle > mEndIdx)
                mSelectedCandle = mEndIdx;
            if (back)
                mSelectedCandle--;
            else mSelectedCandle++;

            if (mSelectedCandle < 0)
                mSelectedCandle = 0;
            if (mSelectedCandle >= getCandleCount())
                mSelectedCandle = getCandleCount() - 1;
            if (mSelectedCandle < mBeginIdx)
                moveChartView(-2);
            if (mSelectedCandle > mEndIdx)
                moveChartView(2);
        }

        public int getCursor()
        {
            return mSelectedCandle;
        }

        public void resetCursor()
        {
            if (mEndIdx == 0)
                mEndIdx = mCandleCnt - 1;
            mSelectedCandle = mEndIdx;
        }

        public void invalideModifiedKey()
        {
            mModifiedKey2 = (int)Utils.currentTimeMillis();
        }

        public int getModifiedKey()
        {
            int k1 = (mBeginIdx << 16) | (mEndIdx);
            int k2 = ((int)(mHiPrice*100) << 24) 
                | ((int)(mHiPrice*100) << 16) 
                | ((int)(100*mLowPrice) << 8) 
                | (int)(100*mLowPrice);

            mModifiedKey = (k1 | k2) + mModifiedKey2;

            return mModifiedKey;
        }

        static public int[] SMA(int[] val, int val_off, int cnt, int period, int[] o)
        {
            if (cnt < 3)
            {
                return null;
            }

            int[] ema = o;
            if (ema == null)
            {
                ema = new int[cnt];
            }

            int pre_total = 0;
            int period_cnt = 0;
            int first = 0;

            for (int i = 0; i < cnt; i++)
            {
                int total = pre_total + val[val_off + i];
                if (period_cnt < period)
                {
                    period_cnt++;
                }

                ema[i] = total / period_cnt;

                if (period_cnt == period)
                {
                    first = val[val_off + i - period + 1];
                }
                pre_total = total - first;
            }

            return ema;
        }

        static float[] SMA(float[] val, int cnt, int period, float[] o)
        {
            if (cnt < 3)
                return null;

            float[] ema = o;
            if (ema == null)
                ema = new float[cnt];

            float pre_total = 0;
            float period_cnt = 0;
            float first = 0;

            for (int i = 0; i < cnt; i++)
            {
                if (i == cnt - 4)
                {
                    int k = 0;
                }

                float total = pre_total + val[i];
                if (period_cnt < period)
                    period_cnt++;

                ema[i] = total / period_cnt;

                if (period_cnt == period)
                    first = val[i - period + 1];
                pre_total = total - first;
            }

            return ema;
        }

        static public float[] SMA(int[] val, int val_off, int cnt, int period, float[] o)
        {
            if (cnt < 3)
            {
                return null;
            }

            float[] ema = o;
            if (ema == null)
            {
                ema = new float[cnt];
            }

            double pre_total = 0;
            int period_cnt = 0;
            float first = 0;

            for (int i = 0; i < cnt; i++)
            {
                double total = pre_total + val[val_off + i];
                if (period_cnt < period)
                {
                    period_cnt++;
                }

                ema[i] = (float)(total / period_cnt);

                if (period_cnt == period)
                {
                    first = val[val_off + i - period + 1];
                }
                pre_total = total - first;
            }

            return ema;
        }

        static public float[] SMA(float[] val, int val_off, int cnt, int period, float[] o)
        {
            if (cnt < 3)
            {
                return null;
            }

            float[] ema = o;
            if (ema == null)
            {
                ema = new float[cnt];
            }

            float pre_total = 0;
            int period_cnt = 0;
            float first = 0;

            for (int i = 0; i < cnt; i++)
            {
                float total = pre_total + val[val_off + i];
                if (period_cnt < period)
                {
                    period_cnt++;
                }

                ema[i] = total / period_cnt;

                if (period_cnt == period)
                {
                    first = val[val_off + i - period + 1];
                }
                pre_total = total - first;
            }

            return ema;
        }

        public void calcSMA(int lineNo)
        {
            //if (mIsCalcSMA) {
            //return;
            //}
            mIsCalcSMA = true;

            int len = getCandleCount();
            for (int i = 0; i < len; i++)
            {
                pTMP[i] = getClose(i);
            }
            /*
            if (lineNo == 0)
                pSMA1 = SMA(pTMP, 0, len, Context.getInstance().mSMAPeriod[lineNo], pSMA1);
            else
                pSMA2 = SMA(pTMP, 0, len, Context.getInstance().mSMAPeriod[lineNo], pSMA2);
             */
        }

        public void calcSMA(int period1, int period2, int period3, int period4, int period5)
        {
            initIndicatorMemory(ChartBase.CHART_SMA);

            if (mIsCalcSMA)
                return;
            mIsCalcSMA = true;

            int len = getCandleCount();
            for (int i = 0; i < len; i++)
            {
                pTMP[i] = mCClose[i];
            }

            pSMA1[0] = -1;
            pSMA2[0] = -1;
            pSMA3[0] = -1;
            pSMA4[0] = -1;
            pSMA5[0] = -1;

            if (period1 >= 1)
                pSMA1 = SMA(pTMP, len, period1, pSMA1);
            if (period2 >= 1)
                pSMA2 = SMA(pTMP, len, period2, pSMA2);
            if (period3 >= 1)
                pSMA3 = SMA(pTMP, len, period3, pSMA3);
            if (period4 >= 1)
                pSMA4 = SMA(pTMP, len, period4, pSMA4);
            if (period5 >= 1)
                pSMA5 = SMA(pTMP, len, period5, pSMA5);
        }

        public void calcEMA(int period1, int period2, int period3, int period4, int period5)
        {
            if (mIsCalcSMA)
                return;

            initIndicatorMemory(ChartBase.CHART_SMA);

            mIsCalcSMA = true;

            int len = getCandleCount();
            for (int i = 0; i < len; i++)
            {
                pTMP[i] = mCClose[i];
            }

            pSMA1[0] = -1;
            pSMA2[0] = -1;
            pSMA3[0] = -1;
            pSMA4[0] = -1;
            pSMA5[0] = -1;

            if (period1 >= 1)
                pSMA1 = EMA(pTMP, len, period1, pSMA1);
            if (period2 >= 1)
                pSMA2 = EMA(pTMP, len, period2, pSMA2);
            if (period3 >= 1)
                pSMA3 = EMA(pTMP, len, period3, pSMA3);
            if (period4 >= 1)
                pSMA4 = EMA(pTMP, len, period4, pSMA4);
            if (period5 >= 1)
                pSMA5 = EMA(pTMP, len, period5, pSMA5);
        }

        public bool isPSARReverseGreen()
        {
            int cnt = getCandleCount();
            if (cnt <= 15)
                return false;

            if (pSAR_SignalUp[cnt - 1])
            {
                for (int i = cnt - 1; i >= cnt - 15; i--)
                {
                    if (!pSAR_SignalUp[i])
                        return true;
                }
            }

            //  last check: accepted if the price is higher than pSAR
            if (!pSAR_SignalUp[cnt - 1])
            {
                if (mCClose[cnt - 1] >= pPSAR[cnt - 1])
                    return true;
            }

            return false;
        }

        public bool isPSARGreen()
        {
            int cnt = getCandleCount();
            if (cnt <= 0)
                return false;

            return pSAR_SignalUp[cnt - 1];
        }

        public bool isPSARRed()
        {
            return !isPSARGreen();
        }

        int getLineValue(int[] line)
        {
            int cnt = getCandleCount();
            if (cnt <= 0)
                return 0;

            if (mSelectedCandle >= 0 && mSelectedCandle < cnt)
                return line[mSelectedCandle];

            return 0;
        }

        public void calcRSI(int rsiIDX)
        {
            int period = 14;
            if (rsiIDX == 0 || rsiIDX == 1)
            {
                period = (int)Context.getInstance().mOptRSIPeriod[rsiIDX];
            }
            else if (rsiIDX == 2)
            {
                period = (int)Context.getInstance().mOptStochRSIPeriod;
            }
            calcRSI(rsiIDX, period);
        }

        public void calcRSIWithPeriod(int period, float[] outRSI)
        {
            int cnt = getCandleCount();
            if (cnt < 4)
            {
                return;
            }

            //if (mIsCalcRSI)
            //return;
            //mIsCalcRSI = true;

            float[] av_up = { 0, 0 };
            float[] av_do = { 0, 0 };
            float[] rs = new float[cnt];
            float currgain;
            float currlost;

            int j = 0;
            av_up[0] = 0;
            av_do[0] = 0;

            //int period = 0;
            //if (rsiIDX == 0 || rsiIDX == 1)
            //period = (int)Context.getInstance().mOptRSIPeriod[rsiIDX];
            //else if (rsiIDX == 2)
            //period = (int)Context.getInstance().mOptStochRSIPeriod;

            for (int i = 0; i < cnt; i++)
            {
                //		if (i == 53)
                //			int k = 0;
                if (i <= 0)
                {
                    rs[j++] = 1.0f;
                    continue;
                }
                currgain = mCClose[i] - mCClose[i - 1];
                currlost = mCClose[i] - mCClose[i - 1];

                if (currgain < 0)
                {
                    currgain = 0;
                    currlost = -currlost;
                }
                if (currgain > 0)
                {
                    currlost = 0;
                }

                av_up[1] = (av_up[0] * (period - 1) + currgain) / period;
                av_do[1] = (av_do[0] * (period - 1) + currlost) / period;

                if (av_do[1] == 0)
                    av_do[1] = av_up[1];

                if (av_do[1] == 0)
                    rs[j] = 1.0f;
                else
                    rs[j] = av_up[1] / av_do[1];
                av_up[0] = av_up[1];
                av_do[0] = av_do[1];
                j++;
            }

            for (int i = 0; i < cnt; i++)
            {
                outRSI[i] = 100 - (100 / (1 + rs[i]));
            }
        }

        public void calcRSI(int rsiIDX, int period)
        {
            initIndicatorMemory(ChartBase.CHART_RSI);

            float[] outRSI = pRSI;
            if (rsiIDX == 0)
            {
                outRSI = pRSI;
            }
            else if (rsiIDX == 1)
            {
                outRSI = pRSISecond;
            }
            else if (rsiIDX == 2)
            {
                outRSI = pTMP;
            }

            calcRSIWithPeriod(period, outRSI);
        }

        public void calcRSICustom(float[] price, int period, float[] outRSI)
        {
            initIndicatorMemory(ChartBase.CHART_RSI);

            int cnt = getCandleCount();
            if (cnt < 4)
                return;

            float[] av_up = { 0, 0 };
            float[] av_do = { 0, 0 };
            float[] rs = new float[cnt];
            float currgain;
            float currlost;

            int j = 0;
            av_up[0] = 0;
            av_do[0] = 0;

            for (int i = 0; i < cnt; i++)
            {
                //		if (i == 53)
                //			int k = 0;
                if (i <= 0)
                {
                    rs[j++] = 1.0f;
                    continue;
                }
                currgain = price[i] - price[i - 1];
                currlost = price[i] - price[i - 1];

                if (currgain < 0)
                {
                    currgain = 0;
                    currlost = -currlost;
                }
                if (currgain > 0)
                {
                    currlost = 0;
                }

                av_up[1] = (av_up[0] * (period - 1) + currgain) / period;
                av_do[1] = (av_do[0] * (period - 1) + currlost) / period;

                if (av_do[1] == 0)
                    av_do[1] = av_up[1];

                if (av_do[1] == 0)
                    rs[j] = 1.0f;
                else
                    rs[j] = av_up[1] / av_do[1];
                av_up[0] = av_up[1];
                av_do[0] = av_do[1];
                j++;
            }

            for (int i = 0; i < cnt; i++)
            {
                outRSI[i] = 100 - (100 / (1 + rs[i]));
            }
        }

        public void calcStochRSI(int periodRSI, int periodStoch, int smoothK, int smoothD,
            float[] pStochRSI, float[] pStochRSISMA)
        {
            if (periodRSI <= 0) periodRSI = 14;
            if (periodStoch <= 0) periodStoch = 14;
            if (smoothK <= 0) smoothK = 3;
            if (smoothD <= 0) smoothD = 3;

            int cnt = getCandleCount();
            if (cnt < 4)
            {
                return;
            }

            float[] rsi = pStaticTMP;
            calcRSIWithPeriod(periodRSI, rsi);

            //  StochRSI = (RSI - Lowest Low RSI) / (Highest High RSI - Lowest Low RSI)
            //int period = (int)Context.getInstance().mOptStochRSIPeriod;
            float max;
            float min;

            int testDate = (2021<<16)|(4 << 8)|6;

            for (int i = 0; i < cnt; i++)
            {
                int b = i - periodStoch + 1;
                if (b < 0) b = 0;
                max = -2000;
                min = 2000;

                //if (getDate(i) == testDate){
                    //testDate++;
                //}
                if (i == 248)
                {
                    testDate = 0;
                }
                //  find min/max
                for (int j = i; j >= b; j--)
                {
                    if (rsi[j] > max) max = rsi[j];
                    if (rsi[j] < min) min = rsi[j];
                }

                if (max != min)
                {
                    pStochRSI[i] = (rsi[i] - min) / (max - min);
                }
                else
                {
                    pStochRSI[i] = 0.5f;
                }
            }
            //  scale 0 - 100
            for (int i = 0; i < cnt; i++)
            {
                pStochRSI[i] *= 100;
            }

            //  smoothK
            SMA(pStochRSI, cnt, smoothK, pStaticTMP2);
            for (int i = 0; i < cnt; i++)
            {
                pStochRSI[i] = pStaticTMP2[i];
            }
            //==========smoothD===============
            if (pStochRSISMA != null)
            {
                //EMA(pStochRSI, cnt, (int)Context.getInstance().mOptStochRSISMAPeriod, pStochRSISMA);
                SMA(pStochRSI, cnt, smoothD, pStochRSISMA);
            }
        }

        public void calcOBV()
        {
            initIndicatorMemory(ChartBase.CHART_OBV);

            if (mIsCalcOBV)
                return;

            mIsCalcOBV = false;

            int cnt = getCandleCount();
            if (cnt < 3)
                return;

            pOBV[0] = 0;
            int tmp;
            if (mCVolume[cnt - 1] > 1000000)
                tmp = 10000;
            else if (mCVolume[cnt - 1] > 100000)
                tmp = 1000;
            else
                tmp = 100;
            int v;
            for (int i = 1; i < cnt; i++)
            {
                if (mCClose[i] > mCRef[i])// mCClose[i - 1])
                    pOBV[i] = pOBV[i - 1] + (float)(mCVolume[i]) / tmp;
                else if (mCClose[i] < mCRef[i])// mCClose[i - 1])
                    pOBV[i] = pOBV[i - 1] - (float)(mCVolume[i]) / tmp;
                else
                    pOBV[i] = pOBV[i-1];
            }
        }

        static public float[] EMA(float[] val, int cnt, int period, float[] o)
        {
            if (cnt < 3)
                return null;

            float[] ema = null;

            if (o != null)
                ema = o;
            else
                ema = new float[cnt];

            ema[0] = val[0];
            float exp = (float)2 / (period + 1);
            for (int i = 1; i < cnt; i++)
            {
                //ema[i] = (float)val[i] * exp + ema[i - 1] * (1 - exp);
                ema[i] = (val[i] - ema[i - 1]) * exp + ema[i - 1];
            }

            return ema;
        }

        static public float[] EMA(int[] val, int cnt, int period, float[] o)
        {
            if (cnt < 3)
                return null;

            float[] ema = null;

            if (o != null)
                ema = o;
            else
                ema = new float[cnt];

            ema[0] = val[0];
            float exp = (float)2 / (period + 1);
            for (int i = 1; i < cnt; i++)
            {
                ema[i] = (float)val[i] * exp + ema[i - 1] * (1 - exp);
                //EMA = Price(t) * k + EMA(y) * (1 – k)
                //ema[i] = (val[i] - ema[i - 1]) * exp + ema[i - 1];
            }

            return ema;
        }

        static public float[] EMA(int[] val, int val_off, int cnt, int period)
        {
            if (cnt < 3)
            {
                return null;
            }

            float[] ema = new float[cnt];

            ema[0] = val[val_off];
            float exp = (float)2 / (period + 1);
            for (int i = 1; i < cnt; i++)
            {
                ema[i] = (float)val[val_off + i] * exp + ema[i - 1] * (1 - exp);
            }

            return ema;
        }

        static public float[] EMA(int[] val, int val_off, int cnt, int period, float[] o)
        {
            if (cnt < 3)
            {
                return null;
            }

            float[] ema = o;
            if (ema == null)
            {
                ema = new float[cnt];
            }

            ema[0] = val[val_off];
            float exp = (float)2 / (period + 1);
            for (int i = 1; i < cnt; i++)
            {
                ema[i] = (float)val[val_off + i] * exp + ema[i - 1] * (1 - exp);
            }

            return ema;
        }

        static public float[] EMA(float[] val, int val_off, int cnt, int period, float[] o)
        {
            if (cnt < 3)
            {
                return null;
            }

            float[] ema = o;
            if (ema == null)
            {
                ema = new float[cnt];
            }

            ema[0] = val[val_off];
            float exp = (float)2 / (period + 1);
            for (int i = 1; i < cnt; i++)
            {
                ema[i] = val[val_off + i] * exp + ema[i - 1] * (1 - exp);
            }

            return ema;
        }

        public float getMACD()
        {
            if (mSelectedCandle >= 0 && mSelectedCandle < getCandleCount())
            {
                return pMACD[mSelectedCandle];
            }

            return 0;
        }

        public float getMACDSignal()
        {
            if (mSelectedCandle >= 0 && mSelectedCandle < getCandleCount())
            {
                return pMACDSignal9[mSelectedCandle];
            }

            return 0;
        }
        public bool isMACDCutSignalAbove(int dayBacks)
        {
            if (hasIntersesionAbove(pMACD, pMACDSignal9, dayBacks))
            {
                //int last = getCandleCount() - 1;
                //if (pMACD[last] >= pMACDSignal9[last])
                    return true;
            }

            return false;
        }

        public bool isStochasticKCutDAbove(int dayBacks)
        {
            if (hasIntersesionAbove(pStochasticSlowK, pStochasticSlowD, dayBacks))
            {
                return true;
            }

            return false;
        }

        public bool priceEnterKumoUp()
        {
            if (getCandleCount() < 30)
                return false;

            int idx = getCandleCount()-1;
            //  price must be going up
            if (isUpTrend())
            {
                float distance1, distance2;
                float threahold = mCClose[idx]*7/100;
                distance1 = (float)mCClose[idx] - pSpanA[idx];
                distance2 = (float)mCClose[idx] - pSpanB[idx];
                if (distance1*distance2 < 0)
                    return true;

                if (distance1 > 0 && distance2 > 0) //  above
                    return false;

                //  comming close
                if (Utils.ABS_FLOAT(distance1) < threahold
                    || Utils.ABS_FLOAT(distance2) < threahold)
                {
                    return true;
                }
            }

            return false;
        }

        public bool priceUpAboveKumo()
        {
            if (getCandleCount() < 30)
                return false;

//            if (mCode.CompareTo("FPT") == 0)
//            {
//                int k = 0;
//            }

            int idx = getCandleCount() - 1;
            //  price must be going up
            if (isUpTrend())
            {
                float distance1, distance2;

                distance1 = (float)mCClose[idx] - pSpanA[idx];
                distance2 = (float)mCClose[idx] - pSpanB[idx];
                if (distance1 >= 0
                    && distance2 >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isTenkanCutKijunAbove(int dayBacks)
        {
            if (hasIntersesionAbove(pTenkansen, pKijunsen, dayBacks))
            {
                return true;
            }

            return false;
        }

        public bool isUpTrend(float[] trend)
        {
            int cnt = getCandleCount();
            if (cnt < 10)
                return false;

            SMA(trend, cnt - 10, 10, 4, pStaticTMP2);

            return trend[cnt - 1] > pStaticTMP2[9];
        }

        public bool isUpTrend(float[] trend, int cnt)
        {
            if (cnt < 10)
                return false;

            SMA(trend, cnt - 10, 10, 3, pStaticTMP2);

            return pStaticTMP2[9] > pStaticTMP2[8];
            /*
            for (int i = 9; i > 6; i--)
            {
                if (pTMP[i] < pTMP[i - 1])
                    return false;
            }
            */
        }

        public bool isUpTrend(int[] trend, int cnt)
        {
            if (cnt < 10)
            {
                return false;
            }
            int j = 0;
            int begin = cnt - 30;
            if (begin < 0) begin = 0;
            for (int i = begin; i < cnt; i++)
            {
                pStaticTMP2[j++] = trend[i] / 1000.0f;
            }

            return isUpTrend(pStaticTMP2, j);
        }

        public bool isDownTrend(int[] trend, int cnt)
        {
            if (cnt < 5)
            {
                return false;
            }
            int j = 0;
            int begin = cnt - 30;
            if (begin < 0) begin = 0;
            for (int i = cnt - 10; i < cnt; i++)
            {
                pStaticTMP2[j++] = trend[i] / 1000.0f;
            }

            return isDowntrend(pStaticTMP2, j);
        }

        public float volumeRate()
        {
            if (getCandleCount() < 5)
            {
                return 1;
            }
            int last = getCandleCount() - 1;

            double avg3 = calcAvgVolume(3);
            double avg10_1 = calcAvgVolume(15, 4);

            if (avg3 == 0)
            {
                return 0;
            }

            avg3 = Math.Max(avg3, avg10_1);

            float vol = mCVolume[last];

            double r1 = vol / avg3;
            double r2 = mCVolume[last - 1] / avg3;

            return (float)Math.Min(r1, r2);
        }

        //  1: increase
        //  -1: decrease
        //  0: both
        //  return in percent
        public float getChangedInPercent(int increase, int days)
        {
            int cnt = getCandleCount();
            cnt = cnt < days ? cnt : days;

            float maxValue = 0;
            float minValue = 0;
            int maxPos = 0;
            int minPos = 0;

            for (int i = 0; i < cnt; i++)
            {
                int last = getCandleCount() - 1 - i;
                float c = getClose(last);

                if (maxValue == 0)
                {
                    maxValue = c;
                    maxPos = 0;
                }
                if (minValue == 0)
                {
                    minValue = c;
                    minPos = 0;
                }

                if (c > maxValue)
                {
                    maxValue = c;
                    maxPos = i;
                }
                if (c < minValue)
                {
                    minValue = c;
                    minPos = i;
                }
            }

            float c1 = getClose(getCandleCount() - 1);

            float percent = 0;

            if (increase == 1)
            {
                if (minValue > 0/* && minPos < maxPos*/)
                {
                    percent = (c1 - minValue) * 100 / minValue;
                }
            }
            else if (increase == -1)
            {
                if (maxValue > 0/* && minPos > maxPos*/)
                {
                    percent = (maxValue - c1) * 100 / maxValue;
                }
            }
            else
            {
                //  bien dong
                float percent1 = 0;
                float percent2 = 0;
                if (minValue > 0)
                {
                    percent1 = (c1 - minValue) * 100 / minValue;
                }
                if (maxValue > 0)
                {
                    percent2 = (maxValue - c1) * 100 / maxValue;
                }

                return Math.Max(percent1, percent2);
            }

            return percent;
        }

        public bool isUpTrend1(int[] trend, int daysback)
        {
            int cnt = getCandleCount();
            if (cnt < 3)
                return false;
            if (cnt - daysback < 0)
                daysback = cnt;
            if (trend[cnt - 1] > trend[cnt - daysback])
                return true;
            return false;
        }

        public bool isDowntrend(float[] prices)
        {
            int cnt = getCandleCount();
            if (cnt < 7)
                return false;

            return (prices[cnt - 1] < prices[cnt - 4] && prices[cnt - 2] < prices[cnt - 5]);
        }

        public bool isDowntrend(float[] prices, int length)
        {
            if (length < 7)
                return false;

            SMA(prices, length - 10, 10, 3, pTMP);
            /*
            for (int i = 7; i < 10; i++)
            {
                if (pTMP[i] > pTMP[i - 1])
                    return false;
            }
            */
            return prices[length - 1] < pTMP[9];

            //return true;
        }

        public bool isLower(float[] trend1, float[] trend2)
        {
            int cnt = getCandleCount();
            if (cnt < 3)
                return false;

            if (trend1[cnt - 1] < trend2[cnt - 1])
                return true;

            return false;
        }

        public bool isHigher(float[] trend1, float[] trend2)
        {
            int cnt = getCandleCount();
            if (cnt < 3)
                return false;

            if (trend1[cnt - 1] > trend2[cnt - 1])
                return true;

            return false;
        }

        public bool isHigher(int[] trend1, int[] trend2)
        {
            int cnt = getCandleCount();
            if (cnt < 3)
                return false;

            if (trend1[cnt - 1] > trend2[cnt - 1])
                return true;

            return false;
        }

        public bool isMACDCutSignalUnder(int dayBacks)
        {
            if (hasIntersesion(pMACD, pMACDSignal9, dayBacks))
            {
                int last = getCandleCount() - 1;
                if (pMACD[last] <= pMACDSignal9[last])
                {
                    return true;
                }
            }

            return false;
        }

        public bool hasIntersesion(float[] line1, float[] line2, int dayBacks)
        {
            int cnt = getCandleCount();
            if (cnt < 3)
                return false;

            int b = cnt - dayBacks - 1;
            int e = cnt;

            if (b < 0)
                b = 0;

            float delta = line1[b] - line2[b];
            float signal = 1;

            for (int i = b + 1; i < e; i++)
            {
                signal = (line1[i] - line2[i]) * delta;
                delta = (line1[i] - line2[i]);
                if (signal < 0)
                {
                    //  cut
                    return true;
                }
            }
            //===========going to cut?==============
            delta = line1[b] - line2[b];
            float delta2 = line1[e-1] - line2[e-1];
            if (Utils.ABS_FLOAT(delta2) < Utils.ABS_FLOAT(delta) / 2)
                return true;

            return false;
        }
        bool hasIntersesionAbove(float[] line1, float[] line2, int dayBacks, int candleCnt)
        {
            int cnt = candleCnt;
            if (cnt < 3)
                return false;

            //  line1 must be going up
            if (line1[cnt - 1] < line1[cnt - 2])
                return false;

            int b = cnt - dayBacks - 1;
            int e = cnt;

            if (b < 0)
                b = 0;

            float delta = line1[b] - line2[b];

            //===========going to cut?==============
            delta = line1[b] - line2[b];
            float delta2 = line1[e - 1] - line2[e - 1];
            //  line1 totally is under line2 
            if (delta < 0 && delta2 < 0)
            {
                //  is line1 closing to line2 ?
                if (Utils.ABS_FLOAT(delta2) < Utils.ABS_FLOAT(delta) / 2)
                {
                    //  line 1 must be going up
                    if (line1[e - 1] > line1[b])
                        return true;
                }
            }
            //  line 1 firstly was under line2 but laterly go above
            if (delta < 0 && delta2 > 0)
            {
                return true;
            }

            return false;
        }

        //  line1 cut above line2 or going to cut above
        public bool hasIntersesionAbove(float[] line1, float[] line2, int dayBacks)
        {
            int cnt = getCandleCount();
            if (cnt < 3)
                return false;

            //  line1 must be going up
            if (line1[cnt - 1] < line1[cnt - 2])
                return false;

            int b = cnt - dayBacks - 1;
            int e = cnt;

            if (b < 0)
                b = 0;

            if (line2 == null)
            {
                line2 = pTMP2;
                for (int i = 0; i < 200; i++) { line2[i] = 0; }
            }

            float delta = line1[b] - line2[b];
            //===========going to cut?==============
            delta = line1[b] - line2[b];
            float delta2 = line1[e - 1] - line2[e - 1];
            //  line1 totally is under line2 
            if (delta < 0 && delta2 < 0)
            {
                //  is line1 closing to line2 ?
                if (Utils.ABS_FLOAT(delta2) < Utils.ABS_FLOAT(delta) / 2)
                {
                    //  line 1 must be going up but close enough?
                    if (line1[e - 1] > line1[b] && 1.5*delta2 > delta)  //  delta1: close, delta2: close
                    {
                        return true;
                    }
                }
            }
            //  line 1 firstly was under line2 but laterly go above
            if (delta < 0 && delta2 > 0)
            {
                return true;
            }

            return false;
        }

        public bool hasIntersesion(int[] line1, int[] line2, int dayBacks)
        {
            int cnt = getCandleCount();
            if (cnt < 3)
                return false;

            int b = cnt - dayBacks - 1;
            int e = cnt;

            if (b < 0)
                b = 0;

            int delta = line1[b] - line2[b];
            int signal = 1;

            for (int i = b + 1; i < e; i++)
            {
                signal = (line1[i] - line2[i]) * delta;
                delta = line1[i] - line2[i];
                if (signal < 0)
                {
                    //  cut
                    return true;
                }
            }

            return false;
        }

        public void calcMACD()
        {
            initIndicatorMemory(ChartBase.CHART_MACD);

            int cnt = getCandleCount();
            if (cnt < 10)
            {
                return;
            }

            if (mIsCalcMACD)
            {
                return;
            }
            mIsCalcMACD = true;

            float[] val = pTMP;
            for (int i = 0; i < cnt; i++)
            {
                val[i] = getClose(i);
            }

            float[] ema12 = EMA(val, 0, cnt, (int)Context.getInstance().mOptMACDFast, null);
            float[] ema26 = EMA(val, 0, cnt, (int)Context.getInstance().mOptMACDSlow, null);

            float[] macd = pMACD;
            for (int i = 0; i < cnt; i++)
            {
                macd[i] = ema12[i] - ema26[i];
            }

            //======signal======EMA9 of MACD
            float[] sig = pMACDSignal9;
            sig[0] = macd[0];
            float exp = (float)2 / (Context.getInstance().mOptMACDSignal + 1);
            for (int i = 1; i < cnt; i++)
            {
                sig[i] = (float)macd[i] * exp + (float)sig[i - 1] * (1 - exp);
            }

            //=======histogram===============
            float[] his = pMACDHistogram;
            for (int i = 0; i < cnt; i++)
            {
                his[i] = macd[i] - sig[i];
            }
            //====================================
            ema12 = null;
            ema26 = null;
        }
        /*
        //=========================
        S = EMA_Integer[(X – EMA_Integer(X))2] = EMA_Integer(X2) – [EMA_Integer(X)]2
        d = sqrt(S);
        //=============input: period, d============
        MA = SMA_Integer(price, period);
        DELTA_i = (PRICE_i - MA_i)2
        UPPPER_i = MA_i+d*sqrt(SMA_Integer(DELTA));
        LOWWER_i = MA_i-d*sqrt(SMA_Integer(DELTA));
         */

        public static void calcBollinger(float[] prices, int cnt, int period, float d, float[] lower, float[] upper)
        {
            //	int start = 0;
            if (cnt < period)
            {
                for (int i = 0; i < cnt; i++)
                {
                    upper[i] = 0;
                    lower[i] = 0;
                }
                return;
            }

            float[] sma = Share.SMA(prices, 0, cnt, period, null);

            if (sma == null)
            {
                return;
            }

            float[] deltaSma2 = new float[cnt];
            for (int i = 0; i < period; i++)
            {
                deltaSma2[i] = 0;
            }

            float tmp = 0;
            for (int i = period; i < cnt; i++)
            {
                tmp = 0;
                for (int j = 0; j < period; j++)
                {
                    tmp += (prices[i - j] - sma[i]) * (prices[i - j] - sma[i]);		//	square
                }
                deltaSma2[i] = (float)Math.Sqrt(tmp / period);
            }

            for (int i = 0; i < cnt; i++)
            {
                upper[i] = sma[i] + (d * (deltaSma2[i]));
                lower[i] = sma[i] - (d * (deltaSma2[i]));
            }

            sma = null;
            deltaSma2 = null;
        }

        public void calcBollinger()
        {
            initIndicatorMemory(ChartBase.CHART_BOLLINGER);
            if (mIsCalcBollinger)
            	return;

            mIsCalcBollinger = true;

            int cnt = getCandleCount();

            int period = (int)Context.getInstance().mOptBBPeriod;
            float d = Context.getInstance().mOptBBD;
            //	int start = 0;

            if (cnt < period)
            {
                for (int i = 0; i < cnt; i++)
                {
                    pBBUpper[i] = 0;
                    pBBLower[i] = 0;
                }
                return;
            }

            float[] price = new float[cnt];
            for (int i = 0; i < cnt; i++)
            {
                price[i] = getClose(i);
            }

            Share.calcBollinger(price, cnt, period, d, pBBLower, pBBUpper);
            /*
            float[] sma = SMA(price, 0, cnt, period, null);

            if (sma == null)
            {
                return;
            }

            float[] deltaSma2 = new float[cnt];
            for (int i = 0; i < period; i++)
            {
                deltaSma2[i] = 0;
            }

            float tmp = 0;
            for (int i = period; i < cnt; i++)
            {
                tmp = 0;
                for (int j = 0; j < period; j++)
                {
                    tmp += (price[i - j] - sma[i]) * (price[i - j] - sma[i]);		//	square
                }
                deltaSma2[i] = (float)Math.Sqrt(tmp / period);
            }
            float[] upper = pBBUpper;
            float[] lower = pBBLower;
            for (int i = 0; i < cnt; i++)
            {
                upper[i] = sma[i] + (int)(d * (deltaSma2[i]));
                lower[i] = sma[i] - (int)(d * (deltaSma2[i]));
            }

            price = null;
            sma = null;
            deltaSma2 = null;
             */
        }

        /*
        //	http://en.wikipedia.org/wiki/Parabolic_SAR
        pSAR[n+1] = pSAR[n] + alpha*(EP-pSAR[n])
        - initial: alpha = 0.2f
        - alpha is increased by 0.02 each time a new EP is recorded.
        - maximum value for the acceleration factor is normally set at 0.20,
         */
        public void calcPSAR(float a, float aMax)
        {
            initIndicatorMemory(ChartBase.CHART_PSAR);

            int cnt = getCandleCount();
            if (cnt < 10)
                return;

            if (mIsCalcPSAR)
                return;
            mIsCalcPSAR = true;

            float alpha0 = Context.getInstance().mOptPSAR_alpha;// 0.02f;
            float alpha = a > 0?a:alpha0;
            float lastAlpha = alpha;
            float alpha_max = aMax > 0?aMax:Context.getInstance().mOptPSAR_alpha_max;//0.2f;

            float psar_next;
            float psar_last;

            //	PSAR[n+1] = PSAR[n] + alpha(EP - PSAR[n]);
            pPSAR[0] = mCLowest[0] - 2;
            psar_last = pPSAR[0];
    	
            float EP = mCHighest[0];
            float lastEP = EP;
    	    bool uptrend = true;
    	    bool newtrend = true;
            
    	    float[] lowest2days = {0, 0};
    	    float[] highest2days = {0, 0};
    	    lowest2days[0] = lowest2days[1] = mCLowest[0];
    	    highest2days[0] = highest2days[1] = mCHighest[0];
    	    float highest2daysVal = lowest2days[0];
    	    float lowest2daysVal = highest2days[0];
            
    	    for (int i = 1; i < cnt; i++)
    	    {           
    		    float highest = mCHighest[i];
    		    float close = mCClose[i];
    		    float lowest = mCLowest[i];
    //            		if (ddd == c->date)
    //            		{
    //            			int k = 0;
    //            		}
    		    //	hit new EP?
    		    if (uptrend)
    		    {
    			    if (highest > EP)    //  new extreme point recorded -> increase alpha
    			    {
    				    EP = highest;
    				    if (alpha < alpha_max)
    					    alpha += alpha0;
                        else
                            alpha = alpha_max;
    			    }
                    psar_next = psar_last + lastAlpha*(Utils.ABS_FLOAT(EP - psar_last));
    		    }
    		    else
    		    {
    			    if (lowest < EP) //  new extreme point recorded -> increase alpha
    			    {
    				    EP = lowest;
    				    if (alpha < alpha_max)
    					    alpha += alpha0;
                        else
                            alpha = alpha_max;
    			    }
                    psar_next = psar_last - lastAlpha*(Utils.ABS_FLOAT(EP - psar_last));
    		    }
                lastAlpha = alpha;

                //========================
    		    //If tomorrow's SAR value lies within (or beyond) today's or yesterday's price range, the SAR must
    		    //be set to the closest price bound. For example, if in an uptrend, the new SAR value is calculated
    		    //and it results to be greater than today's or yesterday's lowest price, the SAR must be set equal to that lower boundary.
    		    if (uptrend)
    		    {
    			    if (psar_next > lowest2daysVal) psar_next = lowest2daysVal;
    		    }
    		    else
    		    {
    			    if (psar_next < highest2daysVal) psar_next = highest2daysVal;
    		    }
                //============================
                //  detect new trend
    		    if (uptrend)
    		    {
    			    if (psar_next > lowest)  //  reverse trend to down
    			    {
    				    uptrend = false;
                        newtrend = true;
    				    alpha = alpha0;
                                        
                        EP = lowest;
    			    }
    		    }
    		    else
    		    {
    			    if (psar_next < highest) //  reverse to up trend
    			    {
    				    uptrend = true;
                        newtrend = true;

                        EP = highest;
    			    }
    		    }
                
                if (newtrend)
                {
                    alpha = alpha0;
                    lastAlpha = alpha;
                    
                    lowest2daysVal = lowest;
                    highest2daysVal = highest;
                    
                    lowest2days[0] = lowest;
                    highest2days[0] = highest;
                    
                    psar_next = lastEP;
                    psar_last = psar_next;
                    
                    newtrend = false;
                }
                
    		    //============set value now
    		    pPSAR[i] = psar_next;//+0.5f;
    		    pSAR_SignalUp[i] = uptrend;
                
                lastEP = EP;
                
    		    //	update lowest/highest in past 2 days
    		    lowest2days[1] = lowest2days[0];
    		    lowest2days[0] = lowest;
    		    highest2days[1] = highest2days[0];
    		    highest2days[0] = highest;
                
    		    lowest2daysVal = lowest2days[0] < lowest2days[1]?lowest2days[0]:lowest2days[1];
    		    highest2daysVal = highest2days[0] > highest2days[1]?highest2days[0]:highest2days[1];
                
    		    psar_last = psar_next;
    	    }
        }

        public void calcVSTOP(int period, float mult)
        {
            initIndicatorMemory(ChartBase.CHART_VSTOP);

            int periodTR = period > 0?period: (int)Context.getInstance().mOptVSTOP_ATR_Loopback;  //  20
            float MULT = mult > 0?mult: Context.getInstance().mOptVSTOP_MULT;// 2.0f;
            
            if (periodTR == 0){
                periodTR = 20;
                MULT = 2.0f;
            }
            
            int cnt = getCandleCnt();
            if (cnt < periodTR+1)
                return;
            
            float SIC = mCClose[0];
            pVSTOP[0] = SIC;
            bool uptrend = true;
            
            int ddd = (2013<<16)|(7<<8)|19;
            float[] ATR = pTMP;
            TRUERANGE_Average(periodTR, ATR);
            if (ATR == null)
                return;

            stCandle c = new stCandle();
            for (int i = 1; i < cnt; i++)
            {
                c = getCandle(i, c);
                
                if (ddd == c.date)
                {
                    ddd = 0;
                }
                
                if (uptrend)
                {
                    if (c.close > SIC)
                    {
                        SIC = c.close;
                    }
                }
                else
                {
                    if (c.close < SIC)
                    {
                        SIC = c.close;
                    }
                }
                
                float vstop = 0;
                if (uptrend)
                {
                    vstop = SIC - MULT*ATR[i];
                    if (vstop < pVSTOP[i-1])
                        vstop = pVSTOP[i-1];
                }
                else
                {
                    vstop = SIC + MULT*ATR[i];
                    if (vstop > pVSTOP[i-1])
                        vstop = pVSTOP[i-1];
                }
                
                //======== new trend============
                //If tomorrow's VSTOP value lies within (or beyond) tomorrow's price range, a new trend direction is
                //then signaled, and the VSTOP must "switch sides."
                if (uptrend)
                {
                    if (vstop > c.close)
                    {
                        uptrend = false;
                        
                        SIC = c.close;
                        vstop = SIC + MULT*ATR[i];
                    }
                }
                else
                {
                    if (vstop < c.close)
                    {
                        uptrend = true;
                        
                        SIC = c.close;
                        vstop = SIC - MULT*ATR[i];
                    }
                }
                
                //============set value now
                pVSTOP[i] = vstop;
                pVSTOP_SignalUp[i] = uptrend;
            }
        }

        /*
        Typical Price = ( (Day High + Day Low + Day Close) / 3)
        Raw Money Flow = (Typical Price) x (Volume)
        Positive Money Flow = Sum of Raw Money Flow for the specified number of periods where Typical Price increased
        Negative Money Flow = Sum of Raw Money Flow for the specified number of periods where Typical Price decreased
        Money Ratio = (Positive Money Flow / Negative Money Flow)
        ly, the MFI can be calculated directly from the Money Ratio:

        Money Flow Index = 100 - (100 / (1 + Money Ratio))
         */
        public void calcMFI(int mfiIDX)
        {
            initIndicatorMemory(ChartBase.CHART_MFI);

            int cnt = getCandleCount();

            if (mIsCalcMFI)
            {
                return;
            }
            mIsCalcMFI = true;

            float[] rawMF = new float[cnt];
            bool[] typicalPriceInc = new bool[cnt];
            //	int rawCnt = 0;

            float[] positiveMF = new float[cnt];
            float[] negativeMF = new float[cnt];
            float[] MFI = pMFI;
            if (mfiIDX == 1)
                MFI = pMFISecond;

            int period = (int)Context.getInstance().mOptMFIPeriod[mfiIDX];
            if (cnt < period)
            {
                return;
            }

            float lastTypicalPrice = 0;

            float hi, lo, c, v;
            int d = (2012 << 16) | (1 << 8) | 10;
            for (int i = 0; i < cnt; i++)
            {
                if (mCDate[i] == d)
                {
                    int k = 0;
                }
                hi = getHighest(i);
                lo = getLowest(i);
                c = getClose(i);
                v = getVolume(i);

                float typicalPrice = (float)(hi + lo + c) / 3;

                rawMF[i] = typicalPrice * ((float)v / 10000);
                typicalPriceInc[i] = (typicalPrice > lastTypicalPrice);

                int tmp = i - period + 1;
                if (tmp < 0)
                {
                    tmp = 0;
                }
                double sumRawMFInc = 0;
                double sumRawMFDec = 0;
                for (int j = tmp; j <= i; j++)
                {
                    if (typicalPriceInc[j])
                    {
                        sumRawMFInc += rawMF[j];
                    }
                    else
                    {
                        sumRawMFDec += rawMF[j];
                    }
                }
                float moneyRatio = 1000;
                if (sumRawMFDec != 0)
                {
                    moneyRatio = (float)(sumRawMFInc / sumRawMFDec);
                }
                MFI[i] = 100 - (int)(100 / (1 + moneyRatio));
                lastTypicalPrice = typicalPrice;
            }

            rawMF = null;
            positiveMF = null;
            negativeMF = null;
            typicalPriceInc = null;
        }

        public void clearCalculations()
        {
            mModifiedKey = 0;
            mModifiedKey2 = (int)Utils.currentTimeMillis()%1000;

            mIsCalcComparingShare = false;
            mIsCalcPVT = false;
            mIsCalcCCI = false;

            mIsCalcWilliamR = false;
            mIsCalcPSAR = false;
            mIsCalcBollinger = false;
            mIsCalcSMA = false;
            mIsCalcRSI = false;
            mIsCalcStochRSI = false;
            mIsCalcOBV = false;
            mIsCalcChaikin = false;
            mIsCalcADL = false;
            mIsCalcROC = false;
            mIsCalcNVI = false;
            mIsCalcPVI = false;
            //mIsCalcEMA = false;
            mIsCalcMACD = false;
            mIsCalcMFI = false;
            mIsCalcADX = false;
            mIsCalcIchimoku = false;
            mIsCalcStochastic = false;
            mIsCalcSMAVolume = false;
            mIsCalcTrix = false;

            mIsCalcEnvelop = false;

            int len = MAX_CANDLE_CHART_COUNT;
            /*
            for (int i = 0; i < len; i++)
            {
                pSMA1[i] = 0;
                pSMA2[i] = 0;
                pEMA[i] = 0;
                pRSI[i] = 0;
                pMACD[i] = 0;
                pMACDSignal9[i] = 0;
                pMACDHistogram[i] = 0;
                pMFI[i] = 0;
                pPSAR[i] = 0;
                pBBUpper[i] = 0;
                pBBLower[i] = 0;
            }
            */
        }

        public float getCurrentValueBBHi()
        {
            if (mSelectedCandle >= 0 && mSelectedCandle < getCandleCount())
            {
                return pBBUpper[mSelectedCandle];
            }

            return 0;
        }

        public float getCurrentValueBBLo()
        {
            if (mSelectedCandle >= 0 && mSelectedCandle < getCandleCount())
            {
                return pBBLower[mSelectedCandle];
            }

            return 0;
        }

        public float getCurrentValueParabolic()
        {
            if (mSelectedCandle >= 0 && mSelectedCandle < getCandleCount())
            {
                return pPSAR[mSelectedCandle];
            }

            return 0;
        }

        public float getCurrentValueSMA()
        {
            if (mSelectedCandle >= 0 && mSelectedCandle < getCandleCount())
            {
                return pSMA1[mSelectedCandle];
            }

            return 0;
        }

        int mIsIndex = -1;
        public bool isIndex()
        {
            if (mCode != null && mCode.Length > 0 && (mCode[0] == '^' 
                || mCode.IndexOf("HNX30") == 0 
                || mCode.IndexOf("HNX30") == 0
                || mCode.IndexOf("VN30") == 0
                || mIsGroupIndex
                )
                )
            {
                mIsIndex = 1;
                return true;
            }
            mIsIndex = 0;

            return false;
            /*
            //  is index?
            if (mIsIndex == -1)
            {
                if (Context.getInstance().mPriceboard.isShareIndex(mID))
                    mIsIndex = 1;
                else
                    mIsIndex = 0;
            }

            return mIsIndex == 1;
             * */
        }

        public void removeAllCandles()
        {
            mCandleCnt = 0;
            mSelectedCandle = 0;
            mBeginIdx = 0;
            mEndIdx = 0;
            if (mCVolume != null)
            {
                System.Buffer.SetByte(mCVolume, 0, 0);
            }
        }

        public void addMoreCandle(float open, float close, float reference, float hi, float lo, int vol, int date)
        {
            makeSureMemoryCapacity(mCandleCnt + 1);

            if (mCandleCnt > 0)
            {
                if (mCDate[mCandleCnt - 1] >= date)
                    return;
            }

            //  ThuyPS: special case
            if (vol == 0 && mCandleCnt > 2)
            {
                open = close = hi = lo = mCOpen[mCandleCnt - 1];
            }

            //==============
            float tmp = 0;
            if (open > 0) tmp = open;
            if (tmp == 0 &&close > 0) tmp = close;
            if (tmp == 0 && hi > 0) tmp = hi;
            if (tmp == 0 && lo > 0) tmp = lo;
            if (tmp == 0 && reference > 0) tmp = reference;

            if (open <= 0) open = tmp;
            if (close <= 0) close = tmp;
            if (hi <= 0) hi = tmp;
            if (lo <= 0) lo = tmp;
            if (reference <= 0) reference = tmp;
            //==============


            mCDate[mCandleCnt] = date;
            mCVolume[mCandleCnt] = vol;
            mCOpen[mCandleCnt] = open;
            mCClose[mCandleCnt] = close;
            mCHighest[mCandleCnt] = hi;
            mCLowest[mCandleCnt] = lo;
            mCRef[mCandleCnt] = reference;
            mCandleCnt++;
            mShouldSaveData = true;
        }

        //  formular: http://www.dinosaurtech.com/2007/average-directional-movement-index-adx-formula-in-c-2/
        public void calcADX()
        {
            initIndicatorMemory(ChartBase.CHART_ADX);
            if (getCandleCount() < 3)
                return;

            if (mIsCalcADX)
                return;

            int period = (int)Context.getInstance().mOptADXPeriod;

            mIsCalcADX = true;
            int cnt = getCandleCount();
            //===========================================
            //===========================================
            float dH = 0;
            float dL = 0;

            float[] pDM = new float[cnt];   //  +DM
            float[] mDM = new float[cnt];   //  -DM

            pDM[0] = 0;
            mDM[0] = 0;

            int i = 0;
            //=======================tinh DM+ && DM-
            for (i = 1; i < cnt; i++)
            {
                dH = mCHighest[i] - mCHighest[i - 1];
                dL = mCLowest[i - 1] - mCLowest[i];

                if ((dH < 0 && dL < 0) || (dH == dL))
                {
                    pDM[i] = 0;
                    mDM[i] = 0;
                }
                else if (dH > dL)
                {
                    pDM[i] = dH;
                    mDM[i] = 0;
                }
                else if (dH < dL)
                {
                    pDM[i] = 0;
                    mDM[i] = dL;
                }
            }
            //=======tinh ADM==========================    
            //  true range
            float[] TR = new float[cnt];
            float t0, t1, t2;
            TR[0] = 0;

            for (i = 1; i < cnt; i++)
            {
                t0 = Utils.ABS_FLOAT(mCHighest[i] - mCLowest[i]);
                t1 = Utils.ABS_FLOAT(mCHighest[i] - mCClose[i - 1]);
                t2 = Utils.ABS_FLOAT(mCClose[i - 1] - mCLowest[i]);

                if (t0 < t1) t0 = t1;
                TR[i] = t0 < t2 ? t2 : t0;
            }
            int dmiPeriod = (int)Context.getInstance().mOptADXPeriodDMI;
            aveSum14(TR, cnt, dmiPeriod);
            aveSum14(pDM, cnt, dmiPeriod);
            aveSum14(mDM, cnt, dmiPeriod);
            /*
            //==============+DM14 && -DM14=====
            for (i = 0; i < cnt; i++)
            {
                if (i < period)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        pDM[i] += pDM[j];
                        mDM[i] += mDM[j];
                    }
                }
                else
                {
                    pDM[i] = pDM[i - 1] - pDM[i - 1] / period + pDM[i];
                    mDM[i] = mDM[i - 1] - mDM[i - 1] / period + mDM[i];
                }
            }
             */
            //==============+DI14 && -DI14==========
            for (i = 0; i < cnt; i++)
            {
                if (TR[i] != 0)
                    pPLUS_DI[i] = 100 * (pDM[i] / TR[i]);
                else
                    pPLUS_DI[i] = 0;

                if (TR[i] != 0)
                    pMINUS_DI[i] = 100 * (mDM[i] / TR[i]);
                else
                    pMINUS_DI[i] = 0;
            }
            //==========directional movement index DX=======
            float[] pDX = new float[cnt];
            for (i = 0; i < cnt; i++)
            {
                if (pPLUS_DI[i] + pMINUS_DI[i] != 0)
                    pDX[i] = 100 * Utils.ABS_FLOAT(pPLUS_DI[i] - pMINUS_DI[i]) / (pPLUS_DI[i] + pMINUS_DI[i]);
                else pDX[i] = 0;
            }
            for (i = 0; i < cnt; i++)
            {
                if (i < period)
                {
                    float total = 0;
                    for (int j = 0; j <= i; j++)
                    {
                        total += pDX[j];
                    }
                    pADX[i] = total / (i + 1);
                }
                else
                {
                    pADX[i] = (pADX[i - 1] * (period - 1) + pDX[i]) / period;
                }
            }
            //==================ADX===============  
            pDM = null;
            mDM = null;

            pDX = null;

            TR = null;
        }

        public bool isPDICutMDI(bool cutAbove, int dayBacks)
        {
            if (cutAbove)
            {
                return hasIntersesionAbove(pPLUS_DI, pMINUS_DI, dayBacks);
            }
            else
            {
                return false;
            }
            /*
            bool inter = hasIntersesion(pPLUS_DI, pMINUS_DI, dayBacks);
            if (inter)
            {
                int last = getCandleCount() - 1;
                if (last > 0)
                {
                    inter = false;
                    if (cutAbove && (pPLUS_DI[last] >= pMINUS_DI[last]))
                        inter = true;
                    if (!cutAbove && (pPLUS_DI[last] <= pMINUS_DI[last]))
                        inter = true;
                }
            }

            return inter;
             * */
        }

        public float getADXValue()
        {
            int cnt = getCandleCount();
            if (cnt > 3 && mSelectedCandle >= 0 && mSelectedCandle < cnt)
            {
                return pADX[mSelectedCandle];
            }

            return 0;
        }

        public bool isMACDConvergency()
        {
            int cnt = getCandleCount();
            if (cnt < 10)
                return false;
            mIsCalcMACD = false;
            calcMACD();

            if (pMACDHistogram[cnt - 1] > pMACDHistogram[cnt - 2] && pMACDHistogram[cnt - 1] < 0)
            {
                return true;
            }

            return false;
        }

        public bool isMACDBuySignal()
        {
            int cnt = getCandleCount();
            if (cnt < 10)
                return false;
            mIsCalcMACD = false;
            calcMACD();

            if (isMACDCutSignalAbove(15) /*&& isUpTrend(pMACD) */&& pMACD[cnt - 1] < 0)
            {
                return true;
            }

            return false;
        }

        public bool isRSICutSMA()
        {
            calcRSI(0);
            EMA(pRSI, getCandleCount(), (int)Context.getInstance().mOptRSI_EMA, pTMP);

            if (hasIntersesionAbove(pRSI, pTMP, 15))
            {
                return true;
            }
            return false;
        }

        public bool isMFICutSMA()
        {
            calcMFI(0);
            EMA(pMFI, getCandleCount(), (int)Context.getInstance().mOptMFI_EMA, pTMP);

            if (hasIntersesionAbove(pMFI, pTMP, 15))
            {
                return true;
            }
            return false;
        }

        public bool isROCCutSMA()
        {
            calcROC(0);
            EMA(pROC, getCandleCount(), (int)Context.getInstance().mOptROC_EMA, pTMP);

            if (hasIntersesionAbove(pROC, pTMP, 15))
            {
                return true;
            }
            return false;
        }

        public bool isADLCutSMA()
        {
            calcADL();
            EMA(pADL, getCandleCount(), (int)5, pTMP);

            if (hasIntersesionAbove(pADL, pTMP, 15))
            {
                return true;
            }
            return false;
        }

        public bool isMACDSellSignal()
        {
            int cnt = getCandleCount();
            if (cnt < 10)
                return false;

            if (isMACDCutSignalUnder(3) && isDowntrend(pMACD) && pMACD[cnt - 1] > 0)
            {
                return true;
            }

            return false;
        }

        public bool isUpTrend()
        {
            int cnt = getCandleCount();
            if (cnt > 10)
            {
                SMA(mCClose, cnt - 10, 10, 4, pTMP);

                for (int i = 6; i < 10; i++)
                {
                    if (pTMP[i] < pTMP[i - 1])
                        return false;
                }

                return true;
            }
            return false;
        }

        public bool isDownTrend2()
        {
            int cnt = getCandleCount();
            if (cnt > 10)
            {
                SMA(mCClose, cnt - 10, 10, 4, pTMP);

                for (int i = 4; i < 10; i++)
                {
                    if (pTMP[i] > pTMP[i - 1])
                        return false;
                }

                return true;
            }
            return false;
        }

        public bool isPriceCrossBBUpper(bool isUpper)
        {
            calcBollinger();

            int cnt = getCandleCount();
            int i = cnt - 1;
            float price = getClose(i);
            if (cnt > 5)
            {
                if (isUpper && pBBUpper[i] <= price)
                    return true;
                if (!isUpper && pBBLower[i] >= price)
                {
                    return true;
                }
            }

            return false;
        }
        public bool isPriceAlmostHitBBUpper(bool isUpper)
        {
            calcBollinger();

            int cnt = getCandleCount();
            int i = cnt - 1;
            float price = getClose(i);
            if (price == 0)
                return false;
            if (cnt > 5)
            {
                if (isUpper)
                {
                    float f = (float)(100 * (pBBUpper[i] - price)) / price;
                    if (f < 1)
                        return true;
                }
                else
                {
                    float f = (float)(100 * (price - pBBLower[i])) / price;
                    if (f < 1)
                        return true;
                }
            }

            return false;
        }
        //==========================    candle stick
        bool isCandleBodyLonger(int t, float percent)
        {
            if (getClose(t) == 0)
                return false;
            float body = (float)getCandleBody(t);
            float percentBody = body * 100 / getClose(t);

            return percentBody >= percent;
        }
        bool isCandleBodyShorter(int t, float percent)
        {
            if (getClose(t) == 0)
                return false;
            float body = (float)getCandleBody(t);
            float percentBody = body * 100 / getClose(t);

            return percentBody <= percent;
        }
        float getCandleBody(int t)
        {
            float body = 0;
            if (t >= 0 && t < getCandleCount())
            {
                body = Utils.ABS_FLOAT(getClose(t) - getOpen(t));
            }
            return body;
        }
        float getCandleLength(int t)
        {
            float len = 0;
            if (t >= 0 && t < getCandleCount())
            {
                len = Utils.ABS_FLOAT(getHighest(t) - getLowest(t));
            }
            return len;
        }
        float getCandleShadowUp(int t)
        {
            float len = 0;
            if (t >= 0 && t < getCandleCount())
            {
                if (isCandleWhite(t))
                    len = Utils.ABS_FLOAT(getHighest(t) - getClose(t));
                else
                    len = Utils.ABS_FLOAT(getHighest(t) - getOpen(t));
            }
            return len;
        }
        float getCandleShadowDown(int t)
        {
            float len = 0;
            if (t >= 0 && t < getCandleCount())
            {
                if (isCandleWhite(t))
                    len = Utils.ABS_FLOAT(getOpen(t) - getLowest(t));
                else
                    len = Utils.ABS_FLOAT(getClose(t) - getLowest(t));
            }
            return len;
        }
        bool isCandleBlack(int t)
        {
            return getOpen(t) > getClose(t);
        }
        bool isCandleWhite(int t)
        {
            return getOpen(t) < getClose(t);
        }
        int isCandleDeltaLonger(float deltaPrice, float percent)
        {
            float percentToPrice = (float)(percent * getClose()) / 100;
            if (deltaPrice > percentToPrice)
                return 1;
            if (deltaPrice < percentToPrice)
                return -1;

            return 0;
        }
        bool isCandleStar(int t)
        {
            float body = getCandleBody(t);

            if (isCandleDeltaLonger(body, 2) == -1)
                return true;

            return false;
        }
        bool isCandleShoutingStar(int t)
        {
            if (isCandleStar(t))
            {
                float lowerShadow = getCandleShadowDown(t);
                float upperShadow = getCandleShadowUp(t);
                if (upperShadow > lowerShadow && isCandleDeltaLonger(upperShadow, 2) == 1)
                    return true;
            }
            return false;
        }
        bool isCandleHammer(int t)
        {
            if (t < 3)
                return false;
            if (isCandleStar(t))
            {
                float lowerShadow = getCandleShadowDown(t);
                float upperShadow = getCandleShadowUp(t);

                //  upper shadow should be very short
                //  lower shadow should be long
                //  body should be short
                //  close&open must lower than prior prices

                if ((lowerShadow > upperShadow)
                    && (isCandleDeltaLonger(lowerShadow, 2) == 1) && ((float)lowerShadow >= (float)upperShadow*1.5f))
                {
                        return true;
                }
            }

            return false;
        }
        bool isBullishEngulfing(int t)
        {
            //  downtrend, today's Candle is white and yesterday's was black
            //  today's close is higher than yesterday's
            //  today's open is lower than yesterday's
            if (isDownTrend2() && isCandleWhite(t) && isCandleBlack(t - 1))
            {
                if (getClose(t) > getClose(t - 1))
                {
                    if (getOpen(t) < getClose(t - 1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool isPercing(int t)
        {
            if (isDownTrend2() && isCandleWhite(t) && isCandleBlack(t - 1))
            {
                if (getClose(t) < getOpen(t - 1))
                {
                    if (getOpen(t) < getClose(t - 1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool isMorningStar(int t)
        {
            if (isCandleStar(t - 1))
            {
                if (isCandleBlack(t - 2) && isCandleWhite(t))
                {
                    if (getClose(t) > getClose(t - 1))
                        return true;
                }
            }
            return false;
        }
        bool isBearishEngulfing(int t)
        {
            if (isUpTrend() && isCandleBlack(t))
            {
                float body0 = getCandleBody(t - 1);
                float body = getCandleBody(t);
                if (body > body0)
                {
                    if (getClose(t) <= getOpen(t - 1) && getOpen(t) > getClose(t - 1))
                        return true;
                }
            }
            return false;
        }

        bool isDarkcloudCover(int t)
        {
            if (isUpTrend() && isCandleBlack(t))
            {
                if (getOpen(t) > getClose(t - 1) && getClose(t) < getClose(t - 1))
                    return true;
            }
            return false;
        }
        bool isEveningStar(int t)
        {
            if (isCandleStar(t - 1) && isCandleBlack(t))
            {
                if (getClose(t) < getOpen(t - 1))
                {
                    float body = getCandleBody(t);
                    if (isCandleDeltaLonger(body, 2) == 1)
                        return true;
                }
            }
            return false;
        }

        public static String readTerminatedString(byte[] p, int off)
        {
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            int end = off + Share.SHARE_CODE_LENGTH;
            if (end > p.Length)
                end = p.Length;
            for (int i = off; i < end; i++)
            {
                if (p[i] == 0)
                {
                    break;
                }
                sb.Append((char)p[i]);
            }
            return sb.ToString();
        }

        public void calcIchimoku()
        {
            initIndicatorMemory(ChartBase.CHART_ICHIMOKU);

            if (mIsCalcIchimoku)
                return;
            mIsCalcIchimoku = true;

            Context ctx = Context.getInstance();

            calcTenkansen(pTenkansen, (int)ctx.mOptIchimokuTime1);
            calcTenkansen(pKijunsen, (int)ctx.mOptIchimokuTime2);
            calcChikou();
            calcSpanA();
            calcSpanB();
        }

        void calcTenkansen(float[] p, int period)
        {
            int cnt = getCandleCount();
            if (cnt == 0)
                return;
            float h;
            float l;
            int d = (2009 << 16) | (9 << 8) | 15;
            for (int i = 0; i < cnt; i++)
            {
                if (mCDate[i] == d)
                {
                    int k = 0;
                }
                //  highest high
                h = getHighestPriceBetween(i, period);

                //  lowest low
                l = getLowestPriceBetween(i, period);

                //=================
                p[i] = (h + l) / 2;
            }
        }

        void calcChikou()
        {
            int period = (int)Context.getInstance().mOptIchimokuTime2;
            int cnt = getCandleCount();
            if (cnt < 0)
                return;
            for (int i = 0; i < cnt; i++)
            {
                if (i - period >= 0)
                    pChikouSpan[i - period] = mCClose[i]; ;
            }
        }
        //  Senkou Span A (Leading Span A): (tenkansen + kijunsen)/2)) 
        //  shift 26 days ahead
        void calcSpanA()
        {
            int cnt = getCandleCount();
            if (cnt == 0)
                return;
            int period2 = (int)Context.getInstance().mOptIchimokuTime2;
            for (int i = 0; i < cnt; i++)
            {
                pSpanA[i + period2] = (pTenkansen[i] + pKijunsen[i]) / 2;
            }
        }

        //  Senkou Span B (Leading Span B): (52-period high + 52-period low)/2)) 
        // shift 26 days ahead
        void calcSpanB()
        {
            int cnt = getCandleCount();
            if (cnt == 0)
                return;

            int period2 = (int)Context.getInstance().mOptIchimokuTime2;
            int period3 = (int)Context.getInstance().mOptIchimokuTime3;

            float h;
            float l;

            for (int i = 0; i < cnt; i++)
            {
                h = getHighestPriceBetween(i, period3);
                l = getLowestPriceBetween(i, period3);

                pSpanB[i + period2] = (h + l) / 2;
            }
        }
        float getLowestPriceBetween(int from, int period)
        {
            int begin = from - period + 1;
            if (begin < 0) begin = 0;

            float lo = 999999;
            for (int i = begin; i <= from; i++)
            {
                if (mCLowest[i] < lo)
                    lo = mCLowest[i];
            }

            return lo;
        }
        float getHighestPriceBetween(int from, int period)
        {
            int begin = from - period + 1;
            if (begin < 0) begin = 0;

            float hi = -999999;
            for (int i = begin; i <= from; i++)
            {
                if (mCHighest[i] > hi)
                    hi = mCHighest[i];
            }

            return hi;
        }

        public void calcStochastic()
        {
            initIndicatorMemory(ChartBase.CHART_STOCHASTIC_FAST);

            //if (mIsCalcStochastic)
                //return;
            mIsCalcStochastic = true;

            //	K = 100*(recentClose-lowestClose)/(highestHi-lowestLow);
            //	D = 3-period of%K
            float recentClose;
            float lowestLow;
            float highestHi;

            Context ctx = Context.getInstance();

            int k = (int)ctx.mOptStochasticFastKPeriod;
            int d = (int)ctx.mOptStochasticFastSMA;

            int cnt = getCandleCount();

            for (int i = 0; i < cnt; i++)
            {
                recentClose = mCClose[i];
                lowestLow = getLowestPriceBetween(i, k);
                highestHi = getHighestPriceBetween(i, k);

                if (highestHi != lowestLow)
                {
                    pStochasticFastK[i] = 100 * (float)(recentClose - lowestLow) / (highestHi - lowestLow);
                }
                else
                {
                    pStochasticFastK[i] = 50;  //  50 or 100?
                }
            }
            pStochasticFastD = SMA(pStochasticFastK, cnt, d, pStochasticFastD);

            //  full
            calcStochasticFull();
        }

        public void calcStochasticFull()
        {
            initIndicatorMemory(ChartBase.CHART_STOCHASTIC_FAST);

            //if (mIsCalcStochastic)
                //return;
            //mIsCalcStochastic = true;

            //	K = 100*(recentClose-lowestClose)/(highestHi-lowestLow);
            //	D = 3-period of%K
            float recentClose;
            float lowestLow;
            float highestHi;

            Context ctx = Context.getInstance();

            int loopback = (int)ctx.mOptStochasticSlowKPeriod;
            int smooth = (int)ctx.mOptStochasticSlowKSmoothK;

            int cnt = getCandleCount();

            for (int i = 0; i < cnt; i++)
            {
                recentClose = mCClose[i];
                lowestLow = getLowestPriceBetween(i, loopback);
                highestHi = getHighestPriceBetween(i, loopback);

                if (highestHi != lowestLow)
                {
                    pTMP[i] = 100 * (float)(recentClose - lowestLow) / (highestHi - lowestLow);
                }
                else
                {
                    pTMP[i] = 50;  //  50 or 100?
                }
            }
            pStochasticSlowK = SMA(pTMP, cnt, smooth, pStochasticSlowK);
            pStochasticSlowD = SMA(pStochasticSlowK, cnt, (int)ctx.mOptStochasticSlowSMA, pStochasticSlowD);
        }

        public void calcWilliamR()
        {
            initIndicatorMemory(ChartBase.CHART_WILLIAMR);

            float recentClose;
            float lowestLow;
            float highestHi;

            Context ctx = Context.getInstance();

            int lookback = (int)ctx.mOptWilliamRPeriod;

            int cnt = getCandleCount();

            for (int i = 0; i < cnt; i++)
            {
                recentClose = mCClose[i];
                lowestLow = getLowestPriceBetween(i, lookback);
                highestHi = getHighestPriceBetween(i, lookback);

                float delta = highestHi - recentClose;
                float distance = highestHi - lowestLow;
                if (distance != 0)
                {
                    pWilliamR[i] = delta * (-100) / distance;
                }
                else
                {
                    pWilliamR[i] = -50;  //  50 or 100?
                }
            } 
        }

        public float getRefFromPriceboard()
        {
            Context ctx = Context.getInstance();
            int floor = getMarketID();
            float reference = 0;
            if (!isIndex())
            {
                stPriceboardState pb = ctx.mPriceboard.getPriceboard(mCode);

                if (pb != null)
                {
                    reference = pb.getRef();
                }
            }
            else
            {
                stPriceboardStateIndex p = ctx.mPriceboard.getPriceboardIndexOfMarket(floor);
                if (p != null)
                {
                    reference = p.reference;
                }
                else
                {
                    reference = getRef() * 10;
                }
            }

            if (reference == 0)
            {
                reference = getRef();
            }

            return reference;
        }

        public double getIncreasedPercentInDays(int days)
        {
            //  reload share data
            Context.getInstance().mShareManager.loadShareFromCommon(this, -1, false);

            int e = getCandleCount() - 1;
            int b = getCandleCount() - 1 - days;
            if (b < 0) b = 0;
            if (e < 0) e = 0;
            if (e == 0)
                return 0;
            double percent = 0;

            if (getClose(b) != 0)
            {
                float tmp = (getClose(e) - getClose(b)) * 100 / getClose(b);
                percent = tmp;
            }
            return percent;
        }

        public int calcAvgVolume(int days)
        {
            return calcAvgVolume(days, 0);
        }

        public int calcAvgVolume(int days, int stepbacks)
        {
            int cnt = getCandleCount();
            if (stepbacks > 0){
                cnt -= stepbacks;
                if (cnt < 0)
                {
                    return 0;
                }
            }

            if (days > cnt) days = cnt;
            int begin = cnt - days;
            if (days == 0)
            {
                return 0;
            }
            double total = 0;
            for (int i = begin; i < cnt; i++)
            {
                total += getVolume(i);
            }
            int avg = (int)(total / days);

            return avg;
        }

        public bool isCandleBullishEngulfing()
        {
            if (mCode.CompareTo("SSI") == 0)
            {
                int k = 0;
            }
            int cnt = getCandleCount();
            if (cnt < 10)
                return false;
            for (int i = cnt - 2; i >= cnt - 3; i--)
            {
                //  strong downtrend
                //  first is a black candle
                //  second is white candle
                //  second's body is takeover first'
                if (isDowntrend(mCClose, i))
                {
                    if (isCandleBlack(i) && isCandleWhite(i + 1))
                    {
                        if (getClose(i + 1) >= getOpen(i)
                            && getOpen(i + 1) <= getClose(i) + (getOpen(i)-getClose(i))/3)
                        {
                            return true;
                        }
                        break;
                    }
                }
            }
            return false;
        }

        public bool isCandleBullishPearcing()
        {
            int cnt = getCandleCount();
            if (cnt < 10)
                return false;
            for (int i = cnt - 2; i >= cnt - 3; i--)
            {
                if (mCode.CompareTo("BVS") == 0)
                {
                    int k = 0;
                }
                if (isDowntrend(mCClose, i))
                {
                    //  1st day is a long black body.
                    //  2nd day is a white body which opens below the low of the 1st day.
                    //  2nd day closes within, but above the midpoint of the 1st day's body.
                    if (isCandleBlack(i) && isCandleWhite(i + 1)
                        && isCandleBodyLonger(i, 1.5f))
                    {
                        if (getClose(i + 1) > getClose(i) && getClose(i + 1) < getOpen(i)
                            && getOpen(i + 1) <= getClose(i))
                            return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return false;
        }

        public bool isCandleMorningStar()
        {
            //if (mCode.CompareTo("SSI") == 0)
            //{
                //int k = 0;
            //}
            int cnt = getCandleCount();
            if (cnt < 10)
                return false;
            for (int i = cnt - 2; i >= cnt - 4; i--)
            {
                if (isDowntrend(mCClose, i))
                {
                    // (i+1) is a morning star?

                    //  strongly downtrend.
                    //  star's body is short
                    //  star's body must not touch prior body.
                    //  next candle is strong up bar eating into major of first bar
                    if (isCandleBlack(i) && isCandleBodyShorter(i+1, 1.0f))
                    {
                        if (getClose(i + 1) <= getClose(i))
                        {
                            if (isCandleWhite(i + 2) && getClose(i + 1) <= getOpen(i + 2))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return false;
        }

        public bool isCandleHammer()
        {
            int cnt = getCandleCount();
            if (cnt < 10)
                return false;
            for (int i = cnt - 1; i >= cnt - 3; i--)
            {
                if (isDowntrend(mCClose, i) && (isCandleBlack(i-1) || isCandleBlack(i-2)))
                {
                    // (i+1) is a morning star?

                    //  strongly downtrend.
                    if (isCandleHammer(i))
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return false;
        }

        public bool isCandleHarami()
        {
            int cnt = getCandleCount();
            if (cnt < 10)
                return false;
            for (int i = cnt - 3; i >= cnt - 5; i--)
            {
                //  i + 1 & i + 2 forms a Harami
                if (isDowntrend(mCClose, i))
                {
                    //  first is a long black
                    //  second is a short white, engufed by the first
                    if (isCandleBlack(i + 1) && isCandleBodyLonger(i + 1, 2.0f))
                    {
                        if (isCandleWhite(i + 2))
                        {
                            float body1 = getCandleBody(i + 1);
                            float body2 = getCandleBody(i + 2);
                            if (body1 >= body2)
                            {
                                //  second is engufed by the first
                                if (getClose(i + 1) <= getOpen(i + 2)
                                    && getOpen(i + 1) >= getClose(i + 2))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    //break;
                }
            }
            return false;
        }

        public bool isBullishDiverRSI()
        {
            int[] v0 = { 0, 0};
            int[] v1 = { 0, 0};
            int off = getCandleCount() - 1;
            int l = 0;
            l = getDowntrendLowerLowLength(mCClose, off, 40, v0);
            if (l > 0)
            {
                if (off - v0[1] > 4)
                    return false;
                l = getUptrendLowerLowLength(pRSI, off, 40, v1);
                if (l > 0)
                {
                    if (pRSI[v1[1]] > 30)
                        return true;
                }
            }

            return false;
        }

        public bool isBullishDiverROC()
        {
            int[] v0 = { 0, 0 };
            int[] v1 = { 0, 0 };
            int off = getCandleCount() - 1;
            int l = 0;
            l = getDowntrendLowerLowLength(mCClose, off, 40, v0);
            if (l > 0)
            {
                if (off - v0[1] > 4)
                    return false;
                l = getUptrendLowerLowLength(pROC, off, 40, v1);
                if (l > 0)
                {
                    //if (pRSI[v1[1]] > 30)
                        return true;
                }
            }

            return false;
        }

        public bool isRSIUptrendLowerLow()
        {
            int[] v1 = { 0, 0 };
            int off = getCandleCount() - 1;
            int l = 0;
            l = getUptrendLowerLowLength(pRSI, off, 20, v1);
            if (l > 0)
            {
                if (pRSI[v1[1]] > 30)
                    return true;
            }

            return false;
        }

        public bool isBullishDiverMFI()
        {
            int[] v0 = { 0, 0 };
            int[] v1 = { 0, 0 };
            int off = getCandleCount() - 1;
            int l = 0;
            l = getDowntrendLowerLowLength(mCClose, off, 40, v0);
            if (l > 0)
            {
                if (off - v0[1] > 4)
                    return false;
                l = getUptrendLowerLowLength(pMFI, off, 40, v1);
                if (l > 0)
                {
                    if (pMFI[v1[1]] > 30)
                        return true;
                    return true;
                }
            }

            return false;
        }

        int getDowntrendLowerLowLength(float[] prices, int off, int days, int[] vertexs)
        {
            if (off <= 0)
                return 0;
            int begin = off - days;
            if (begin < 0)
                begin = 0;

            xVectorInt lowers = new xVectorInt(15);
            float lower = -1;
            int pos = -1;
            int downtrendRecorded = off;
            int e = off;
            while (e > 1 && prices[e] < prices[e - 1])
                e--;
            float threahold = prices[off]*10/100;

            for (int i = e; i > begin; i--)
            {
                if (pos == -1)
                {
                    pos = i;
                    lower = prices[i];
                }
                else
                {
                    if (prices[i] <= lower)
                    {
                        pos = i;
                        lower = prices[i];
                    }
                    else
                    {
                        //  this is lower low
                        lowers.addElement(pos);
                        int j = 0;
                        //  skip until reach higher high
                        for (j = i - 1; j > begin; j--)
                        {
                            if (prices[j] < lower)
                                break;
                            else
                                lower = prices[j];
                        }

                        i = j;
                        if (j > begin)
                        {
                            pos = i;
                            lower = prices[i];
                        }
                    }
                }
            }   //  end of for
            //  
            if (lowers.size() > 1)
            {
                int last = lowers.firstElement();
                lower = prices[last];

                if ((prices[off] - lower) > threahold)
                    return 0;
                downtrendRecorded = last;
                int cnt = 0;
                vertexs[1] = last;
                int retry = 0;
                for (int i = 1; i < lowers.size(); i++)    //  steps backward
                {
                    pos = lowers.elementAt(i);
                    if (prices[pos] > lower)
                    {
                        downtrendRecorded = pos;
                        cnt++;
                        vertexs[0] = pos;
                    }
                    else
                    {
                        if (lower - prices[pos] < threahold && retry < 2)
                        {
                            retry++;
                            lower = prices[pos];
                        }else
                            break;
                    }
                }
                if (cnt == 0)
                {
                    //  not a downtrend
                    downtrendRecorded = off;
                }
            }

            return off - downtrendRecorded;
        }

        int getDowntrendLowerLowLength(int[] prices, int off, int days, int[] vertexs)
        {
            if (off <= 0)
                return 0;
            int begin = off - days;
            if (begin < 0)
                begin = 0;

            xVectorInt lowers = new xVectorInt(15);
            int lower = -1;
            int pos = -1;
            int downtrendRecorded = off;

            int e = off;
            while (e > 1 && prices[e] < prices[e - 1])
                e--;
            float threahold = prices[off] * 5 / 100;

            for (int i = e; i > begin; i--)
            {
                if (pos == -1)
                {
                    pos = i;
                    lower = prices[i];
                }
                else
                {
                    if (prices[i] <= lower)
                    {
                        pos = i;
                        lower = prices[i];
                    }
                    else
                    {
                        //  this is lower low
                        lowers.addElement(pos);
                        //Utils.trace(lower + "");
                        int j = 0;
                        //  skip until reach higher high
                        for (j = i - 1; j > begin; j--)
                        {
                            if (prices[j] < lower)
                                break;
                            else
                                lower = prices[j];
                        }

                        i = j;
                        if (j > begin)
                        {
                            pos = i;
                            lower = prices[i];
                        }
                    }
                }
            }   //  end of for
            //  
            if (lowers.size() > 1)
            {
                int last = lowers.firstElement();
                lower = prices[last];
                if ((prices[off] - lower) > threahold)
                    return 0;

                downtrendRecorded = last;
                int cnt = 0;
                vertexs[1] = last;
                int retry = 0;
                for (int i = 1; i < lowers.size(); i++)    //  steps backward
                {
                    pos = lowers.elementAt(i);
                    if (prices[pos] > lower)
                    {
                        downtrendRecorded = pos;
                        cnt++;
                        vertexs[0] = pos;
                    }
                    else
                    {
                        if (lower - prices[pos] < threahold && retry < 2)
                        {
                            retry++;
                            lower = prices[pos];
                        }
                        else
                        {
                            //  previous Lower is too high => not a downtrend
                            break;
                        }
                    }
                }
                if (cnt == 0)
                {
                    //  not a downtrend
                    downtrendRecorded = off;
                }
            }

            return off - downtrendRecorded;
        }

        int getUptrendLowerLowLength(float[] prices, int off, int days, int[] vertexs)
        {
            if (off <= 0)
                return 0;
            int begin = off - days;
            if (begin < 0)
                begin = 0;

            xVectorInt lowers = new xVectorInt(15);
            float lower = -1;
            int pos = -1;
            int downtrendRecorded = off;
            float threahold = prices[off]*5/100;

            //int e = off;
            //while (e > 1 && prices[e] < prices[e - 1])
                //e--;

            for (int i = off; i > begin; i--)
            {
                if (pos == -1)
                {
                    pos = i;
                    lower = prices[i];
                }
                else
                {
                    if (prices[i] <= lower)
                    {
                        pos = i;
                        lower = prices[i];
                    }
                    else
                    {
                        //  this is lower low
                        lowers.addElement(pos);
                        //Utils.trace(lower + "\\");
                        int j = 0;
                        //  skip until reach higher high
                        for (j = i - 1; j > begin; j--)
                        {
                            if (prices[j] < lower)
                                break;
                            else
                                lower = prices[j];
                        }

                        i = j;
                        if (j > begin)
                        {
                            pos = i;
                            lower = prices[i];
                        }
                    }
                }
            }   //  end of for
            //  
            if (lowers.size() > 1)
            {
                int last = lowers.firstElement();
                float higher = prices[last];

                downtrendRecorded = last;
                int cnt = 0;
                vertexs[1] = last;
                int retry = 0;
                for (int i = 1; i < lowers.size(); i++)    //  steps backward
                {
                    pos = lowers.elementAt(i);
                    if (prices[pos] < higher)
                    {
                        downtrendRecorded = pos;
                        cnt++;
                        vertexs[0] = pos;
                    }
                    else
                    {
                        if (prices[pos] - higher <= threahold && retry < 2)
                        {
                            higher = prices[pos];
                        }else
                            break;
                    }
                }
                if (cnt == 0)
                {
                    //  not a downtrend
                    downtrendRecorded = off;
                }
            }

            return off - downtrendRecorded;
        }

        public void calcChaikinOscillator(int chaikinIDX)
        {
            initIndicatorMemory(ChartBase.CHART_CHAIKIN);

            int cnt = getCandleCount();
            if (cnt < 4)
                return;

            if (mIsCalcChaikin)
                return;
            int i = 0;
            mIsCalcChaikin = true;

            float[] ad = new float[cnt];
            ad[0] = 0;

            int period0 = (int)Context.getInstance().mOptChaikin0[chaikinIDX];
            int period1 = (int)Context.getInstance().mOptChaikin1[chaikinIDX];

            //  A/D(i) =((CLOSE(i) - LOW(i)) - (HIGH(i) - CLOSE(i)) * VOLUME(i) / (HIGH(i) - LOW(i)) + A/D(i-1)
            /*
              1. Money Flow Multiplier = [(Close  -  Low) - (High - Close)] /(High - Low) 
              2. Money Flow Volume = Money Flow Multiplier x Volume for the Period
              3. ADL = Previous ADL + Current Period's Money Flow Volume
              4. Chaikin Oscillator = (3-day EMA of ADL)  -  (10-day EMA of ADL)
             */
            double mfm;
            double mfv;

            //  if volume too big?
            double tmp = 0;
            for (i = 0; i < cnt; i++)
            {
                if (i > 10)
                    break;
                tmp += mCVolume[i];
            }
            tmp /= i;
            int devFactor = 1;
            if (tmp > 10000000)
                devFactor = 100000;
            else if (tmp > 1000000)
                devFactor = 10000;
            else if (tmp > 100000)
                devFactor = 1000;

            for (i = 1; i < cnt; i++)
            {
                if (i == 2000)
                {
                    int k = 0;
                }
                if (mCHighest[i] - mCLowest[i] == 0)
                {
                    ad[i] = ad[i - 1];
                }
                else
                {
                    //  money flow multiplier
                    //  position of close / distance_hi_lo
                    mfm = 2*mCClose[i] - (mCLowest[i] + mCHighest[i]);
                    mfm /= (mCHighest[i] - mCLowest[i]);
                    //  money flow volume
                    mfv = mfm * (mCVolume[i]/devFactor);
                    ad[i] = ad[i - 1] + (float)mfv;
                }
            }
            float[] sma3 = EMA(ad, cnt, period0, null);
            float[] sma10 = EMA(ad, cnt, period1, null);

            if (chaikinIDX == 0)
            {
                for (i = 0; i < cnt; i++)
                {
                    pChaikinOscillator[i] = sma3[i] - sma10[i];
                }
            }
            else
            {
                for (i = 0; i < cnt; i++)
                {
                    pChaikinOscillatorSecond[i] = sma3[i] - sma10[i];
                }
            }
        }

        public void calcADL()
        {
            initIndicatorMemory(ChartBase.CHART_ADL);

            int cnt = getCandleCount();
            if (cnt < 4)
                return;

            if (mIsCalcADL)
                return;
            int i = 0;
            mIsCalcADL = true;

            float[] ad = pADL;
            ad[0] = 0;

            //  A/D(i) =((CLOSE(i) - LOW(i)) - (HIGH(i) - CLOSE(i)) * VOLUME(i) / (HIGH(i) - LOW(i)) + A/D(i-1)
            /*
              1. Money Flow Multiplier = [(Close  -  Low) - (High - Close)] /(High - Low) 
              2. Money Flow Volume = Money Flow Multiplier x Volume for the Period
              3. ADL = Previous ADL + Current Period's Money Flow Volume
              4. Chaikin Oscillator = (3-day EMA of ADL)  -  (10-day EMA of ADL)
             */
            double mfm;
            double mfv;

            //  if volume too big?
            double devFactor = mCVolume[cnt-1];
            devFactor /= 100;
            if (devFactor <= 0)
                devFactor = 1;

            for (i = 1; i < cnt; i++)
            {
                if (mCHighest[i] - mCLowest[i] == 0)
                {
                    ad[i] = ad[i - 1];
                }
                else
                {
                    //  money flow multiplier
                    //  position of close / distance_hi_lo
                    mfm = 2 * mCClose[i] - (mCLowest[i] + mCHighest[i]);
                    mfm /= (mCHighest[i] - mCLowest[i]);
                    //  money flow volume
                    mfv = mfm * ((float)mCVolume[i] / devFactor);
                    ad[i] = ad[i - 1] + (float)mfv;
                }
            }

            EMA(ad, cnt, (int)Context.getInstance().mOptADL_SMA[0], pADL_SMA0);
            EMA(ad, cnt, (int)Context.getInstance().mOptADL_SMA[1], pADL_SMA1);
        }

        public void calcROC(int rocIDX)
        {
            initIndicatorMemory(ChartBase.CHART_ROC);

            int cnt = getCandleCount();
            
            int period = (int)Context.getInstance().mOptROC[rocIDX];
            if (cnt < period)
                return;

            if (mIsCalcROC)
                return;
            int i = 0;
            mIsCalcROC = true;

            //  ROC = [(Close - Close n periods ago) / (Close n periods ago)] * 100
            float[] roc = pROC;
            if (rocIDX == 1)
                roc = pROCSecond;

            for (i = 0; i < period; i++)
                roc[i] = 0;

            float tmp;
            for (i = period; i < cnt; i++)
            {
                if (mCClose[i - period] == 0)
                {
                    roc[i] = roc[i-1];
                }
                else
                {
                    tmp = (float)(mCClose[i] - mCClose[i - period])/mCClose[i - period];
                    tmp *= 100;
                    roc[i] = tmp;
                }
            }

            //SMA(roc, 0, cnt, 3, roc);
        }

        public void calcNVI()
        {
            initIndicatorMemory(ChartBase.CHART_NVI);

            int cnt = getCandleCount();

            if (mIsCalcNVI || cnt == 0)
                return;

            int i = 0;
            mIsCalcNVI = true;
            //  if (V < Vi)
            //      NVI = NVIi(1 + (PRICE[i] - PRICE[i-1])/PRICE[i-1])
            //  else NVI = NVI[i-1]
            pNVI[0] = 100;
            float tmp;
            for (i = 1; i < cnt; i++)
            {
                if (mCVolume[i] < mCVolume[i - 1] && mCClose[i - 1] != 0)
                {
                    tmp = mCClose[i] - mCClose[i - 1];
                    tmp /= mCClose[i - 1];

                    pNVI[i] = pNVI[i - 1] * (1 + tmp);
                }
                else
                {
                    pNVI[i] = pNVI[i - 1];
                }
            }

            int period1 = (int)Context.getInstance().mOptNVI_EMA[0];
            int period2 = (int)Context.getInstance().mOptNVI_EMA[1];
            Context ctx = Context.getInstance();
            if (ctx.mOptNVI_EMA_ON1[0])
            {
                EMA(pNVI, cnt, period1, pNVI_EMA1);
            }
            else
            {
                SMA(pNVI, 0, cnt, period1, pNVI_EMA1);
            }
            if (ctx.mOptNVI_EMA_ON2[0])
            {
                EMA(pNVI, cnt, period2, pNVI_EMA2);
            }
            else
            {
                SMA(pNVI, 0, cnt, period2, pNVI_EMA2);
            }
        }

        public void calcPVI()
        {
            initIndicatorMemory(ChartBase.CHART_PVI);

            int cnt = getCandleCount();

            if (mIsCalcPVI || cnt == 0)
                return;

            int i = 0;
            mIsCalcPVI = true;
            //  if (V > Vi)
            //      PVI = PVIi(1 + (PRICE[i] - PRICE[i-1])/PRICE[i-1])
            //  else PVI = PVI[i-1]
            pPVI[0] = 100;
            float tmp;
            //int XX = (2011 << 16) | (10 << 8) | 5;
            for (i = 1; i < cnt; i++)
            {
                //if (mCDate[i] == XX)
                //{
                    //pPVI[i] = 100;
                    //continue;
                //}
                if (mCVolume[i] > mCVolume[i - 1] && mCClose[i - 1] != 0)
                {
                    tmp = mCClose[i] - mCClose[i - 1];
                    tmp /= mCClose[i - 1];

                    pPVI[i] = pPVI[i - 1] * (1 + tmp);
                }
                else
                {
                    pPVI[i] = pPVI[i - 1];
                }
            }

            Context ctx = Context.getInstance();
            int period1 = (int)ctx.mOptPVI_EMA[0];
            int period2 = (int)ctx.mOptPVI_EMA[1];
            if (ctx.mOptPVI_EMA_ON1[0])
            {
                EMA(pPVI, cnt, period1, pPVI_EMA1);
            }
            else
            {
                SMA(pPVI, 0, cnt, period1, pPVI_EMA1);
            }
            if (ctx.mOptPVI_EMA_ON2[0])
            {
                EMA(pPVI, cnt, period2, pPVI_EMA2);
            }
            else
            {
                SMA(pPVI, 0, cnt, period2, pPVI_EMA2);
            }
        }

        //  ignoreMovePercent: 0-100
        public xVectorInt calcZigzag()
        {
            float[] price = mCClose;
            int off = 0;// 1800;
            int cnt = getCandleCount();
            float ignoreMovePercent = Context.getInstance().mOptZigzagPercent;

            int dedicateVertex = off;
            float length = 0;
            int direction = 0;
            float moveLength = ignoreMovePercent*price[off]/100;
            xVectorInt v = new xVectorInt(20);
            int curDir, lastDir = 1;
            int i = 0;
            for (i = off+1; i < cnt; i++)
            {
                //  always recalc moveLength
                moveLength = ignoreMovePercent * price[i] / 100;

                //if (price[i] == 3791)
                //{
                    //int k = 0;
                //}
                float tmp = price[i] - price[i-1];
                if (tmp == 0)
                    curDir = lastDir;
                else{
                    curDir = tmp > 0?1:-1;
                }
                lastDir = curDir;
                //========================
                if (direction == 0) //  first time
                {
                    direction = curDir;
                }
                //========================
                //if ((dedicateVertex == -1) && (direction * curDir == -1))
                //{
                    //dedicateVertex = i - 1;
                //}
                //if (dedicateVertex == -1)
                //{
                    //continue;
                //}
                //===================================
                if (i > 0)
                {
                    length = price[i - 1] - price[dedicateVertex];
                }
                else
                {
                    length = 0;
                }
                //========================
                if (direction == 1 && curDir == -1)  //  reverse point
                {
                    if (length >= moveLength)
                    {
                        v.addElement(dedicateVertex);   //  record LOWER
                        //  
                        dedicateVertex = i - 1; //  might be HIGHEST
                        direction = -1;         //  down
                    }
                    else
                    {
                        //  reject dedicateVertex
                        //dedicateVertex = -1;
                        //direction = -1;
                        if (price[i] < price[dedicateVertex])
                        {
                            dedicateVertex = i;
                        }
                    }
                }
                else if (direction == -1 && curDir == 1) //  reverse point
                {
                    if (Utils.ABS_FLOAT(length) >= moveLength)
                    {
                        v.addElement(dedicateVertex);
                        dedicateVertex = i - 1;
                        direction = 1;
                    }
                    else
                    {
                        //  reject dedicateVertex
                        //dedicateVertex = -1;
                        //direction = 1;
                        if (price[i] > price[dedicateVertex])
                        {
                            dedicateVertex = i;
                        }
                    }
                }
            }

            if (v.size() > 0 && dedicateVertex != -1)
            {
                if (v.lastElement() != dedicateVertex)
                {
                    v.addElement(dedicateVertex);
                    if (v.lastElement() != cnt - 1)
                    {
                        v.addElement(cnt - 1);
                    }
                }
            }

            return v;
        }

        public float[] calcSMAVolume(int off, int end)
        {
            initIndicatorMemory(ChartBase.CHART_VOLUME);

            if (mIsCalcSMAVolume)
                return pSMAVolume;
            off -= (int)Context.getInstance().mOptSMAVolume;
            if (off < 0) off = 0;

            SMA(mCVolume, off, end - off, (int)Context.getInstance().mOptSMAVolume, pSMAVolume);

            mIsCalcSMAVolume = true;

            return pSMAVolume;
        }

        public bool isSMAInter()
        {    
            int cnt = getCandleCount();
            mSortParam = -1;
            if (cnt < 7)
            {
                return false;
            }

            int period0 = Context.getInstance().mOptFilterSMA1;
            int period1 = Context.getInstance().mOptFilterSMA2;
    
            int max = period1 > period0?period1:period0;

            int b = cnt - max - 50;
            int e = cnt-1;

            if (b < 0) b = 0;
            int j = 0;

            for (int i = b; i <= e; i++)
            {
                pTMP[j++] = mCClose[i];
            }

            cnt = e - b + 1;

            SMA(pTMP, cnt, period0, pTMP2);
            SMA(pTMP, cnt, period1, pTMP3);
    
            return hasIntersesionAbove(pTMP2, pTMP3, 15, cnt);
        }

        public bool isSMACutPrice(int period)
        {
            int cnt = getCandleCount();
            mSortParam = -1;
            if (cnt < 7)
            {
                return false;
            }

            int b = cnt - period - 50;
            int e = cnt - 1;

            if (b < 0) b = 0;
            int j = 0;

            for (int i = b; i <= e; i++)
            {
                pTMP[j++] = mCClose[i];
            }

            cnt = e - b + 1;

            SMA(pTMP, cnt, period, pTMP1);

            return pTMP1[cnt - 1] > mCClose[cnt - 1];
        }

        public bool isNVIBullish()
        {
            return hasIntersesionAbove(pNVI_EMA1, pNVI_EMA2, 15);
        }

        public bool isAccumulation()
        {
            int cnt = getCandleCount();

            if (cnt < 15)
                return false;

            //if (mCode.CompareTo("OCH") == 0)
            //{
                //int k = 0;
            //}

            int e = cnt-1;
            int b = cnt - 10;
            float sma = (mCClose[e] + mCClose[b]) / 2;
            float threhold = 3.0f;
            float threhold2 = 6.0f;
            float min = 10000000;
            float max = -1;
            float percent;

            for (int i = b; i <= e; i++)
            {
                //percent = ((float)(mCClose[i] - sma) * 100) / sma;
                //if (Utils.ABS_FLOAT(percent) > threhold)
                    //return false;

                if (mCClose[i] > max) max = mCClose[i];
                if (mCClose[i] < min) min = mCClose[i];
            }
            if (min == 0)
                return false;

            percent = ((float)(max - min) * 100) / min;
            if (percent > threhold2)
                return false;

            return true;
        }

        public void calcVolumeUpValue()
        {
            int cnt = getCandleCount();
            mSortParam = -1;
            if (cnt < 7)
            {
                return;
            }
            if (mCode == "SD2")
            {
                int k = 0;
            }
            if (SMA(mCVolume, 0, getCandleCount(), 3, pTMP2) != null)
            {
                if (isUpTrend(pTMP2) && pTMP2[cnt - 2] > 0)
                {
                    mSortParam = (((pTMP2[cnt - 1]) / pTMP2[cnt - 2]));
                }
            }

            return ;
        }

        public void calcSMAThuyPS()
        {
            //if (mIsCalcSMAVolume)
                //return pSMAVolume;

            int cnt = getCandleCount();
            if (cnt < 5)
                return;
            float[] p = pTMP;
            float[] p4 = pTMP1;
            p[0] = 0;
            int i = 0;
            float[] t = { 0, 0, 0, 0, 0};
            int period = 5;

            float ac = 0;
            float ac4 = 0;
            int tmp = mCVolume[cnt - 1];
            if (tmp > 1000000) tmp = 10000;
            else if (tmp > 100000) tmp = 1000;
            else if (tmp > 10000) tmp = 100;
            else if (tmp > 1000) tmp = 10;
            else tmp = 1;
            for (i = 0; i < 4; i++)
            {
                p[i] = 0;
                p4[i] = 0;
            }
            //  line 1
            float v;
            for (i = 4; i < cnt; i++)
            {
                if (mCClose[i-1] == 0 || mCVolume[i-1] == 0)
                    p[i] = p[i-1];
                else
                {
                    v = (float)(mCVolume[i]) / tmp;
                    p[i] = p[i-1] + ((float)(mCClose[i] - mCClose[i-1])*v);
                }
            }
            /*
            for (i = 1; i < cnt; i++)
            {
                if (p[i - 1] == 0)
                    p[i] = 1;
                else
                    p[i] = p[i-1] + (p[i] - p[i - 1])/p[i-1];
            }
             */
            //  line 2
            period = 3;
            for (i = 4; i < cnt; i++)
            {
                if (mCClose[i - period] == 0 || mCVolume[i-period] == 0)
                    p4[i] = p4[i - period];
                else
                {
                    v = (float)(mCVolume[i]) / tmp;
                    p4[i] = p4[i-1] + ((float)(mCClose[i] - mCClose[i - period]) / mCClose[i - period]) * v;
                }
            }
            /*
            pTMP[0] = 0;
            float percent = 0.0f;
            for (i = period; i < cnt; i++)
            {
                pSMAThuyPS[i] = p[i];
                if (p4[i-1] == 0)
                    percent = 0;
                else
                    percent = p4[i];// (p4[i] - p4[i - period]) / p4[i - 1];
                pSMAThuyPS1[i] = pSMAThuyPS[i] + percent * 5 * pSMAThuyPS[i];
            }
             */

            //calcRSICustom(p, 14, pSMAThuyPS);
            //calcRSICustom(p4, 14, pSMAThuyPS1);

            calcMFI(0);

            SMA(pMFI, cnt, 1, pSMAThuyPS);
            SMA(pMFI, cnt, 7, pSMAThuyPS1);
        }

        public void calcTrix()
        {
            initIndicatorMemory(ChartBase.CHART_TRIX);

            int cnt = getCandleCount();
            if (cnt < 5)
                return;

            if (mIsCalcTrix)
                return;
    
            mIsCalcTrix = false;
            int period = (int)Context.getInstance().mOptTRIX[0];
            EMA(mCClose, cnt, period, pTMP);
            EMA(pTMP, cnt, period, pTMP1);
            EMA(pTMP1, cnt, period, pTMP2);

            pTrix[0] = 0;
            for (int i = 1; i < cnt; i++)
            {
                if (pTMP2[i - 1] != 0)
                    pTrix[i] = (pTMP2[i] - pTMP2[i - 1]) / pTMP2[i - 1];
                else
                    pTrix[i] = pTrix[i - 1];

                pTrix[i] *= 100;
            }

            //  EMA
            period = (int)Context.getInstance().mOptTRIX[1];
            EMA(pTrix, cnt, period, pTrixEMA);
        }

        public void calcEnvelop()
        {
            initIndicatorMemory(ChartBase.CHART_ENVELOP);

            int cnt = getCandleCount();
            if (cnt < 5)
                return;

            if (mIsCalcEnvelop)
                return;

            mIsCalcEnvelop = false;
            int period = (int)Context.getInstance().mOptEnvelopPeriod;
            EMA(mCClose, cnt, period, pSMA_Envelop);
        }

        public bool focusAtSelected()
        {
            if (mCurrentScope > SCOPE_6MONTHS)
            {
                mLastBegin = mBeginIdx;
                mLastEnd = mEndIdx;
                mLastScope = mCurrentScope;
                //-----------------------------------
                int half = SCOPE_6MONTHS/2;
                mBeginIdx = mSelectedCandle - half;
                if (mBeginIdx < 0)
                    mBeginIdx = 0;
                mEndIdx = mBeginIdx + SCOPE_6MONTHS;
                if (mEndIdx >= mCandleCnt)
                    mEndIdx = mCandleCnt - 1;

                mCurrentScope = SCOPE_6MONTHS;
                updateHiloCandle();

                return true;
            }
            else if (mCurrentScope == SCOPE_6MONTHS)
            {
                //  zooming out
                if (mLastScope != -1)
                {
                    mBeginIdx = mLastBegin;
                    mEndIdx = mLastEnd;
                    mCurrentScope = mLastScope;

                    mLastScope = -1;

                    updateHiloCandle();

                }

                return true;
            }

            return false;
        }
        //---------------------------------------------------
        public void toWeekly()
        {
            int cnt = getCandleCount();

            if (cnt < 4)
            {
                return;
            }
            int last = 100;

            int D = 0;
            long V = 0;
            float C = 0;
            float H = 0;
            float L = 0;
            float O = 0;
            int j = 0;

            int vol = getAveVolumeInDays(5);
            if (vol > 10000000)
            {
                mVolumeDivided = 100;
            }

            for (int i = 0; i < cnt; i++)
            {
                int date = mCDate[i];

                int day = Utils.dayOfWeek(Utils.EXTRACT_YEAR(date),
                                      Utils.EXTRACT_MONTH(date),
                                      Utils.EXTRACT_DAY(date));

                if (day < last)
                {
                    //  end of the week
                    if (last != 100)
                    {
                        mCClose[j] = C;
                        mCDate[j] = D;
                        mCHighest[j] = H;
                        mCLowest[j] = L;
                        mCOpen[j] = O;
                        mCVolume[j] = (int)(V/mVolumeDivided);

                        if (j == 503)
                        {
                            j = 503;
                        }

                        j++;
                    }

                    //  beginning of week
                    last = day;

                    O = mCOpen[i];
                    H = mCHighest[i];
                    L = mCLowest[i];
                    C = mCClose[i];
                    D = mCDate[i];
                    V = mCVolume[i];
                }
                else
                {
                    V += mCVolume[i];
                    D = mCDate[i];
                    C = mCClose[i];
                    if (mCLowest[i] < L) L = mCLowest[i];
                    if (mCHighest[i] > H) H = mCHighest[i];
                    last = day;
                }
            }// end of for
            //--------------
            if (j < cnt)
            {
                mCClose[j] = C;
                mCDate[j] = D;
                mCHighest[j] = H;
                mCLowest[j] = L;
                mCOpen[j] = O;
                mCVolume[j] = (int)(V/mVolumeDivided);

                j++;
            }
            //--------------
            mCandleCnt = j;

            mCandleType = CANDLE_WEEKLY;
        }

        public void toMonthly()
        {
            int cnt = getCandleCount();
            if (cnt < 20)
                return;
            int last = 100;

            int vol = getAveVolumeInDays(5);
            if (vol > 10000000)
            {
                mVolumeDivided = 100;
            }

            int D = 0;
            long V = 0;
            float C = 0;
            float H = 0;
            float L = 0;
            float O = 0;
            int j = 0;
            for (int i = 0; i < cnt; i++)
            {
                int date = mCDate[i];

                int day = Utils.EXTRACT_DAY(date);

                if (day < last)
                {
                    //  end of the week
                    if (last != 100)
                    {
                        mCClose[j] = C;
                        mCDate[j] = D;
                        mCHighest[j] = H;
                        mCLowest[j] = L;
                        mCOpen[j] = O;
                        mCVolume[j] = (int)(V/mVolumeDivided);

                        j++;
                    }

                    //  beginning of month
                    last = day;

                    O = mCOpen[i];
                    H = mCHighest[i];
                    L = mCLowest[i];
                    C = mCClose[i];
                    D = mCDate[i];
                    V = mCVolume[i];
                }
                else
                {
                    V += mCVolume[i];
                    D = mCDate[i];
                    C = mCClose[i];

                    if (D == ((2015 << 16) | (7 << 8) | 31))
                    {
                        D = ((2015 << 16) | (7 << 8) | 31);
                    }
                    if (mCLowest[i] < L) L = mCLowest[i];
                    if (mCHighest[i] > H) H = mCHighest[i];
                    last = day;
                }
            }// end of for
            //--------------
            if (j < cnt)
            {
                mCClose[j] = C;
                mCDate[j] = D;

                mCHighest[j] = H;
                mCLowest[j] = L;
                mCOpen[j] = O;
                mCVolume[j] = (int)(V/mVolumeDivided);

                j++;
            }
            //--------------    
            mCandleCnt = j;

            mCandleType = CANDLE_MONTHLY;
        }
        //---------------------------
        public int getEndDate()
        {
            if (mEndIdx < getCandleCount())
                return mCDate[mEndIdx];

            return 0;
        }

        public void setEndDate(int date)
        {
            int cnt = getCandleCount();
            mEndIdx = cnt - 1;

            if (date == -1)
                return;

            for (int i = 0; i < cnt; i++)
            {
                int d = mCDate[i];

                if (d > date)
                {
                    mEndIdx = i - 1;
                    break;
                }
            }

            if (mEndIdx < 0)
                mEndIdx = 0;

            mSelectedCandle = mEndIdx;
        }

        public void aveSum14(float[] t, int cnt, int period)
        {
            int k = cnt < period ? cnt : period;
            int i = 0;
            for (i = 0; i < k; i++)
            {
                for (int j = k - i - 1; j > 0; j--)
                {
                    t[k - i - 1] += t[j];
                }
            }
            //=============TR14=============
            for (i = period; i < cnt; i++)
            {   
                t[i] = t[i - 1] - t[i - 1] / period + t[i];
            }
        }

        public int getAveVolumeInDays(int days)
        {
            return calcAvgVolume(days);
        }

        public int getAveVolumeInDays(int days, int stepbacks)
        {
            return calcAvgVolume(days, stepbacks);
        }

        /*
     * 
CCI = (Typical Price  -  20-period SMA of TP) / (.015 x Mean Deviation)

Typical Price (TP) = (High + Low + Close)/3

Constant = .015

There are four steps to calculating the Mean Deviation. First, subtract 
the most recent 20-period average of the typical price from each period's 
typical price. Second, take the absolute values of these numbers. Third, 
sum the absolute values. Fourth, divide by the total number of periods (20). 
*/
        public void calcCCI()
        {
            initIndicatorMemory(ChartBase.CHART_CCI);

    	    int cnt = getCandleCount();
            if (cnt < 3)
                return;
            
            if (mIsCalcCCI)
        	    return;
            
            mIsCalcCCI = true;
            
            int loopback = (int)Context.getInstance().mOptCCIPeriod;
            float k = (float)Context.getInstance().mOptCCIConstant;
            
            //	
            
            float[] pTP = pTMP;
            float[] pMD = pTMP1;
            float[] pSMA_TP = pTMP2;
            
            //	calc pTP & pMD
            for (int i = 0; i < cnt; i++)
            {
        	    pTP[i] = (mCHighest[i] + mCLowest[i] + mCClose[i])/3;
            }
            
            SMA(pTP, cnt, loopback, pSMA_TP);
            for (int i = 0; i < cnt; i++)
            {
        	    if (i == 100)
        	    {
        		    loopback = 20;
        	    }
        	    pMD[i] = _calcMeanDeviation(i, loopback, pTP, pSMA_TP);
            }        
            
            //	calc CCI
            pCCI[0] = 0;
            for (int i = 1; i < cnt; i++)
            {
        	    if (pMD[i] > 0)
        		    pCCI[i] = (pTP[i] - pSMA_TP[i])/(k*pMD[i]);
        	    else
        		    pCCI[i] = pCCI[i-1];
            }
        }

        public void calcPVT()
        {
            initIndicatorMemory(ChartBase.CHART_PVT);

            /*
             * Calculate the Percentage Change in closing price: 
               ( Closing Price [today] - Closing Price [yesterday] ) / Closing Price [yesterday]
    Multiply the Percentage Change by Volume: 
               Percentage Change * Volume [today]
    Add to yesterday's cumulative total: 
               Percentage Change * Volume [today] + PVT [yesterday]
             * */

            if (mIsCalcPVT)
                return;

            mIsCalcPVT = true;

            int cnt = getCandleCount();
            if (cnt < 3)
                return;

            pPVT[0] = 0;
            int tmp;

            if (mCVolume[cnt - 1] > 1000000)
                tmp = 10000;
            else if (mCVolume[cnt - 1] > 100000)
                tmp = 1000;
            else if (mCVolume[cnt - 1] > 10000)
                tmp = 100;
            else
                tmp = 1;

            float k = 0;
            for (int i = 1; i < cnt; i++)
            {
                if (mCClose[i - 1] != 0)
                {
                    k = (mCClose[i] - mCClose[i - 1]);
                    k /= mCClose[i - 1];
                }
                else
                    k = 1;

                pPVT[i] = pPVT[i - 1] + mCVolume[i] * k;
            }

            int smaPeriod = (int)Context.getInstance().mOptPVT;
            SMA(pPVT, cnt, smaPeriod, pPVT_EMA1);
        }

        float _calcMeanDeviation(int offset, int loopback, float[] pTP, float[] pSMA_TP)
        {
            int b = offset - loopback + 1;
            if (b < 0)
                b = 0;
            int cnt = 0;
            float totalTP = 0;
            for (int i = offset; i >= b; i--)
            {
                float t = pTP[i] - pSMA_TP[offset];
                if (t < 0) t = -t;

                totalTP += t;
                cnt++;
            }

            if (cnt > 0)
                return totalTP / cnt;

            return 0;
        }

        float[] TRUERANGE(float[] TR)
        {
            int cnt = getCandleCount();

            if (cnt <= 0)
                return null;

            float t0, t1, t2;

            TR[0] = mCHighest[0] = mCLowest[0];

            for (int i = 1; i < cnt; i++)
            {
                t0 = /*xUtils::absF*/(mCHighest[i] - mCLowest[i]);
                t1 = /*xUtils::absF*/(mCHighest[i] - mCClose[i - 1]);
                t2 = /*xUtils::absF*/(mCClose[i - 1] - mCLowest[i]);

                t0 = Math.Max(t0, t1);
                t0 = Math.Max(t0, t2);

                TR[i] = t0;

                //            if (t0 < t1) t0 = t1;
                //            TR[i] = t0 < t2?t2:t0;
            }

            return TR;
        }

        float[] TRUERANGE_Average(int period, float[] pATR)
        {
            int cnt = getCandleCount();

            if (cnt <= period || period <= 0)
                return null;

            TRUERANGE(pTMP2);

            int period0 = period - 1;
            pATR[period0] = 0;
            for (int i = 0; i < period; i++)
            {
                pATR[i] = 0;
                pATR[period0] += pTMP2[i];
            }
            pATR[period0] /= period;
            //================
            for (int i = period; i < cnt; i++)
            {
                pATR[i] = (pATR[i - 1] * period0 + pTMP2[i]) / period;
            }

            return pATR;
        }

        public void calcMassIndex()
        {
            initIndicatorMemory(ChartBase.CHART_MASSINDEX);

            pMassIndex[0] = 0;
            int periodOfDifferent = (int)Context.getInstance().mOptMassIndexDifferent;
            int periodOfSum = (int)Context.getInstance().mOptMassIndexSum;

            int cnt = getCandleCount();
            if (cnt < periodOfSum)
                return;
            
            int len = getCandleCount();
            for (int i = 0; i < len; i++)
            {
                pTMP[i] = Utils.ABS_FLOAT(mCHighest[i] - mCLowest[i]);
            }
            EMA(pTMP, cnt, periodOfDifferent, pTMP1);
            EMA(pTMP1, cnt, periodOfDifferent, pTMP2);

            for (int i = 0; i < cnt; i++)
            {
                if (pTMP2[i] != 0)
                {
                    pTMP[i] = pTMP1[i] / pTMP2[i];
                }
                else
                {
                    pTMP[i] = 0;
                }
            }
            //============================
            /*
            pMassIndex[0] = pTMP[0];
            for (int i = 0; i < periodOfSum; i++)
            {
                pMassIndex[i] = pMassIndex[i-1] + pTMP[i];
            }
             */
            //============================
            //        int d = (2013 << 16) | (12 << 8) | 18;

            for (int i = 0; i < cnt; i++)
            {
                //pMassIndex[i] = pTMP[i] + pMassIndex[i-1] - pTMP[i-periodOfSum];
                pMassIndex[i] = 0;

                for (int j = 0; j < periodOfSum; j++)
                {
                    if ((i - j) >= 0)
                        pMassIndex[i] += pTMP[i - j];
                }
            }
        }

        public void changeTimeFrameOfIntraday(int candleType)
        {
            mIntradayTimeFrame = candleType;
            loadShareFromFile(false);
            //==================================
            int cnt = getCandleCount();
            if (cnt == 0)
                return;
            if (candleType == 0)
                candleType = CANDLE_1MIN;
            
            mCandleType = candleType;
            int mins = mCandleType;
            if (mins == 1)
            {
                selectCandle(getCandleCount()-1);
                setCursorScope(0xfffff);    //  show all
                updateHiloCandle();

                mSelectedCandle = getCandleCount() - 1;
                mBeginIdx = 0;
                mEndIdx = getCandleCount() - 1;
                
                if (mSelectedCandle < 0)
                    mSelectedCandle = 0;
                
                return; //  default
            }

            int j = 0;
            int last = -1;
            int mark = -1;
            int[] mark5mins = {5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 0};
            int[] mark10mins = {10, 20, 30, 40, 50, 60, 0};
            int[] mark15mins = {15, 30, 45, 60, 0};
            int[] mark30mins = {30, 60, 0};
            int[] mark60mins = {60, 0};
            int lastMM = -1;

            int ccDate = 0;
            int ccVolume = 0;
            float ccOpen = 0;
            float ccClose = 0;
            float ccHi = 0;
            float ccLow = 0;
            if (cnt == 0)
                return;

            for (int i = 0; i < cnt; i++)
            {
                if (j == 44)
                {
                    j = 44;
                }
                if (i == 0){
                    ccDate = mCDate[i];
                    ccVolume = mCVolume[i];
                    ccOpen = mCOpen[i];
                    ccClose = mCClose[i];
                    ccHi = mCHighest[i];
                    ccLow = mCLowest[i];
                }
                //=====================
                int date = mCDate[i]; //  m:d:hh:mm

                int hh = (date >> 16) & 0xff;
                int mm = (date >> 8) & 0xff;
                if (lastMM == -1)
                    lastMM = mm;
                
                //  determine the end mark
                if (mark == -1)
                {
                    int[] mks = mark5mins;
                    if (mins == 5) mks = mark5mins;
                    else if (mins == 10) mks = mark10mins;
                    else if (mins == 15) mks = mark15mins;
                    else if (mins == 30) mks = mark30mins;
                    else if (mins == 60) mks = mark60mins;
                    
                    int t = 0;
                    while (mks[t] != 0)
                    {
                        if (mm < mks[t])
                        {
                            mark = mks[t];
                            break;
                        }

                        t++;
                    }
                }
                //=================================
                
                if (mm > mark || mm < lastMM)   //  mm < lastMM -> next hour
                {
                    //  owrite cc to the candle
                    mCDate[j] = ccDate;
                    mCVolume[j] = ccVolume;
                    mCClose[j] = ccClose;
                    mCOpen[j] = ccOpen;
                    mCHighest[j] = ccHi;
                    mCLowest[j] = ccLow;

                    j++;
                    
                    //  first minute
                    ccDate = mCDate[i];
                    ccVolume = mCVolume[i];
                    ccOpen = mCOpen[i];
                    ccClose = mCClose[i];
                    ccHi = mCHighest[i];
                    ccLow = mCLowest[i];

                    mark = -1;
                }
                else{
                    ccVolume += mCVolume[i];
                    ccDate = mCDate[i];
                    ccClose = mCClose[i];
                    if (mCLowest[i] < ccLow) ccLow = mCLowest[i];
                    if (mCHighest[i] > ccHi) ccHi = mCHighest[i];
                }
                
                lastMM = mm;
            }
            //==================================================
            if (j < cnt)
            {
                mCDate[j] = ccDate;
                mCVolume[j] = ccVolume;
                mCClose[j] = ccClose;
                mCOpen[j] = ccOpen;
                mCHighest[j] = ccHi;
                mCLowest[j] = ccLow;

                j++;
            }
            mCandleCnt = j;

            selectCandle(getCandleCount() - 1);
            setCursorScope(0xfffff);    //  show all
            //===============================
            updateHiloCandle();
            
            mSelectedCandle = getCandleCount() - 1;
            mBeginIdx = 0;
            mEndIdx = getCandleCount() - 1;
            
            if (mSelectedCandle < 0)
                mSelectedCandle = 0;
        }

        public void calcATR()
        {
            initIndicatorMemory(ChartBase.CHART_ATR);

            int period = (int)Context.getInstance().mOptATRLoopback;
            if (period <= 0 || period > 100)
                period = 14;

            TRUERANGE_Average(period, pATR);
        }

        public void calcRSPrice(Share baseShare, int ma1, int ma2)
        {
            if (baseShare == null)
            {
                return;
            }

            baseShare.loadShareFromFile(true);

            initIndicatorMemory(ChartBase.CHART_CRS_RATIO);

            int cnt = getCandleCount();
            int baseCnt = baseShare.getCandleCount();

            int j = 0;

            float[] pClose = pTMP;
            float[] pBase = pTMP1;
            int k = baseCnt - 1;

            for (j = cnt - 1; j >= 0; j--)
            {
                pClose[j] = getClose(j);

                if (k >= 0)
                {
                    pBase[j] = baseShare.getClose(k);
                }
                else
                {
                    pBase[j] = baseShare.getClose(0);
                }
                k--;
            }
            //---------------------------------
            float zoom = pBase[cnt-1]/pClose[cnt-1];
            if (zoom < 10) zoom = 1;
            else if (zoom < 100) zoom = 10;
            else if (zoom < 1000) zoom = 100;
            else zoom = 1000;
            for (j = 0; j < cnt; j++)
            {
                if (pClose[j] > 0 && pBase[j] > 0)
                {
                    pCRS[j] = pClose[j]*zoom/pBase[j];
                }
                else
                {
                    pCRS[j] = 1;
                }
            }

            SMA(pCRS, cnt, ma1, pCRS_MA1);
            SMA(pCRS, cnt, ma2, pCRS_MA2);
        }

        public void calcRSPerformance(Share baseShare, int period, int ma1, int ma2)
        {
            if (baseShare == null)
            {
                return;
            }

            baseShare.loadShareFromFile(true);

            initIndicatorMemory(ChartBase.CHART_CRS_PERCENT);

            int cnt = getCandleCount();
            int baseCnt = baseShare.getCandleCount();

            int j = 0;
            
            float[] pClose = pTMP;
            float[] pBase = pTMP1;
            int k = baseCnt - 1;

            int date = (2020 << 16) | (3 << 8) | 30;

            for (j = cnt -1; j >= 0; j--)
            {
                pClose[j] = getClose(j);

                if (k >= 0)
                {
                    pBase[j] = baseShare.getClose(k);
                }
                else
                {
                    pBase[j] = baseShare.getClose(0);
                }
                k--;
            }
            //---------------------------------
            //  RS = ((close / sma(close, length) / (base / sma(base, length)))-1.0)*100
            SMA(pClose, cnt, 3, pTMP2);
            SMA(pBase, cnt, 3, pTMP3);
            for (j = 0; j < cnt; j++)
            {
                float m1;
                float m2;

                if (getDate(j) == date)
                {
                    j = j;
                }

                if (j >= period)
                {
                    m1 = pTMP2[j - period];
                    m2 = pTMP3[j - period];
                }
                else
                {
                    m1 = pTMP2[0];
                    m2 = pTMP3[0];
                }
                if (m1 > 0 && m2 > 0)
                {
                    float c = pClose[j]/m1;
                    float b = pBase[j]/m2;
                    
                    pCRS_Percent[j] = (c / b - 1.0f) * 100;
                }
                else
                {
                    if (j > 0)
                    {
                        pCRS_Percent[j] = pCRS_Percent[j - 1];
                    }
                    else
                    {
                        pCRS_Percent[j] = 0;
                    }
                }
            }

            SMA(pCRS_Percent, cnt, ma1, pCRS_MA1_Percent);
            SMA(pCRS_Percent, cnt, ma2, pCRS_MA2_Percent);
        }

        public void calcComparingShare(String comparingCode)
        {
            initIndicatorMemory(ChartBase.CHART_COMPARING_SECOND_SHARE);

            if (mIsCalcComparingShare){
                return;
            }
            int cnt = getCandleCount();
            for (int i = 0; i < cnt; i++){
                pComparingPrice[i] = 0;
            }
            //-------------------------------

            Share second = Context.getInstance().mShareManager.getShare(comparingCode);// new Share();
            second.loadShare();

            int j = second.getCandleCount()-1;
            for (int i = cnt - 1; j >= 0 && i >= 0; i--,j--)
            {
                pComparingPrice[i] = second.mCClose[j];
            }
        }

        public void calcYearOfPast(int yearOfPast)
        {
            initIndicatorMemory(ChartBase.CHART_PAST_1_YEAR);
            initIndicatorMemory(ChartBase.CHART_PAST_2_YEARS);

            if (yearOfPast == 1)
                mIs1YearChartOn = true;
            if (yearOfPast == 2)
                mIs2YearChartOn = true;
            float[] p = null;
            if (yearOfPast == 1)
            {
                pPastPrice1[0] = -1;
                p = pPastPrice1;
            }
            else if (yearOfPast == 2)
            {
                pPastPrice2[0] = -1;
                p = pPastPrice2;
            }
            if (p == null)
            {
                return;
            }
            if (getCandleCount() <= 10)
                return;
            int today = getDate(getCandleCount() - 1);
            int y = (today>>16)&0xffff;
            y -= yearOfPast;

            int pastDay = (y << 16) | (today & 0xffff);
            int len = 0;
            for (int i = 0; i < mCandleCnt; i++)
            {
                int d = mCDate[i];
                if (d < pastDay)
                {
                    pTMP[len++] = mCClose[i];
                }
                else
                {
                    break;
                }
            }
            //========================
            int distance = mCandleCnt - len;
            int j = 0;
            if (distance > 0)
            {
                for (int i = 0; i < distance; i++)
                {
                    p[j++] = 0;
                }
            }
            for (int i = 0; i < len; i++)
            {
                p[j++] = pTMP[i];
            }

            //  hi/lo
            float lo = 9000000;
            float hi = -9000000;
            for (int i = mBeginIdx; i <= mEndIdx; i++)
            {
                if (p[i] > hi) hi = p[i];
                if (p[i] < lo) lo = p[i];
            }

            if (yearOfPast == 1){
                m1YearChartLow = lo;
                m1YearChartHigh = hi;
            }
            else{
                m2YearChartLow = lo;
                m2YearChartHigh = hi;
            }

            mModifiedKey = (int)Utils.currentTimeMillis() % 10000;
            mModifiedKey2 = (int)Utils.currentTimeMillis() % 1000 + 123456;
        }

        public bool calcARoon(float[] up, float[] down, float[] aroonOscillator, int period)
        {
            int cnt = getCandleCount();

            if (cnt < period || period < 0)
                return false;

            for (int i = 0; i < period; i++)
            {
                up[i] = 0;
                down[i] = 0;
                aroonOscillator[i] = 0;
            }

            for (int i = period; i < cnt; i++)
            {
                if (i == cnt - 4)
                {
                    int k = 0;
                    i++;
                    i = i - 1;
                }
                int hh = i;
                int ll = i;

                for (int j = i - 1; j > i - period; j--)
                {
                    if (mCHighest[j] > mCHighest[hh])
                        hh = j;
                    if (mCLowest[j] < mCLowest[ll])
                        ll = j;
                }

                hh = i - hh;
                ll = i - ll;

                //===================
                float v = period - hh;
                v /= period;
                v *= 100;
                up[i] = v;
                //==================
                v = period - ll;
                v /= period;
                v *= 100;
                down[i] = v;
            }

            for (int i = 0; i < cnt; i++)
            {
                aroonOscillator[i] = up[i] - down[i];
            }

            return true;
        }
    
        //1. Money Flow Multiplier = [(Close  -  Low) - (High - Close)] /(High - Low) 
        //2. Money Flow Volume = Money Flow Multiplier x Volume for the Period
        //3. 20-period CMF = 20-period Sum of Money Flow Volume / 20 period Sum of Volume 
        				
        public void calcCFM(int period, float[] cfm)
        {
            initIndicatorMemory(ChartBase.CHART_CFM);

    	    if (period == 0)
    		    period = 20;
    	    int cnt = getCandleCount();
    	    float mfm;
    	    float[] mfv = pTMP;
    	    //	1 & 2
    	    for (int i = 0; i < cnt; i++)
    	    {
    		    mfm = (mCClose[i] - mCLowest[i]) - (mCHighest[i] - mCClose[i]);    		
    		    float t = mCHighest[i] - mCLowest[i];
    		    if (t > 0){
    			    mfm /= t;
    			    mfv[i] = mfm*mCVolume[i];
    		    }
    		    else
    		    {
    			    if (i > 1)
    				    mfv[i] = mfv[i-1];
    			    else
    				    mfv[i] = 0;
    		    }
    	    }
    	    //	3
    	    double[] sumMFV = pDouble1;
    	    double[] sumV = pDouble2;
        	
    	    for (int i = 0; i < cnt; i++)
    	    {
    		    sumMFV[i] = 0;
    		    sumV[i] = 0;
        		
    		    int b = i - period;
    		    if (b < 0) b = 0;
    		    for (int j = i; j > b; j--)
    		    {
    			    sumMFV[i] += mfv[j];
    			    sumV[i] += mCVolume[j]; 
    		    }
    	    }
    	    //==================
    	    double d = 0;
    	    for (int i = 0; i < cnt; i++)
    	    {
    		    if (sumV[i] > 0)
    		    {
    			    d = sumMFV[i]/sumV[i];
    			    cfm[i] = (float)d;
    		    }
    		    else
    			    cfm[i] = 0;
    	    }
        }

        public bool hitBuySignal()
        {
            return false;
        }

        public int calcHeiken(int maxLength, float[] candleHs)
        {
            int cnt = getCandleCount();
            int begin = cnt - maxLength;

            if (begin < 0) begin = 0;
            if (cnt < 10)
            {
                return -1;
            }

            float o, c, h, l;
            float ho, hc, hh, hl;
            float last_ho = -1;
            float last_hc = -1;
            float delta;
            float candleBodyH = 0;
            int j = 0;

            for (int i = begin; i < cnt; i++)
            {
                o = this.getOpen(i);
                c = this.getClose(i);
                h = this.getHighest(i);
                l = this.getLowest(i);
                //-----------------------------------
                if (last_hc == -1)
                {
                    last_hc = (o + c + h + l) / 4;
                    last_ho = (o + c + h + l) / 4;
                }

                hc = (o + h + l + c) / 4;
                ho = (last_ho + last_hc) / 2;
                hh = h > ho ? h : ho;
                hl = l < ho ? l : ho;

                last_ho = ho;
                last_hc = hc;
                //-------------------------
                candleBodyH = hc - ho;

                candleHs[j++] = candleBodyH;
            }

            return j;
        }
    }
}