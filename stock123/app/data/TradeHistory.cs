/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
using System;
using System.Collections.Generic;
using System.Text;
using xlib.framework;
using xlib.utils;
/**
 *
 * @author ThuyPham
 */

namespace stock123.app.data
{
    public class TradeHistory
    {
        /*
        public Vector mTradeTransaction = new Vector();
         * 
         */

        public bool mHasNewData;
        public String mCode;
        public int mShareID;
        public Share mShare;
        public int mDate;
        public int mFloorID;
        Context mContext;
        public float mPriceRef;
        public bool mIsIndex;
        public int mTradeTransactionCount;
        public int[] mTradeTransactionBuffer;
        public int mMaxTradeVolume = 1;

        public float mOpen, mClose, mHighest, mLowest;

        public TradeHistory()
        {
            //mDate = Utils.getDateAsInt();
            mTradeTransactionBuffer = new int[FIELD_CNT * 30];
            mContext = Context.getInstance();
        }

        public void setShareID(int marketID, int shareID, String code, bool isIndex)
        {
            mFloorID = marketID;
            mShareID = shareID;
            mShare = mContext.mShareManager.getShare(mShareID);
            mCode = code;
            mIsIndex = isIndex;
            if (isIndex)
            {
                stPriceboardStateIndex p = mContext.mPriceboard.getIndexByCode(code);
                if (p != null)
                    mPriceRef = p.reference;///10;
                //mFloor = p.floor_index;
            }
            else
            {
                stPriceboardState ps = mContext.mPriceboard.getPriceboard(code);
                if (ps != null)
                    mPriceRef = ps.getRef();
                //mFloor = ps.getFloor();
            }
        }

        public int getFloorID()
        {
            return mFloorID;
        }

        public String getCode()
        {
            return mCode;
        }

        public int getCodeID()
        {
            return mShareID;
        }

        public void setData(xDataInput di)
        {
            //int shareID = in.readShort();
            int cnt = di.readInt();
            if (cnt <= 0)
            {
                return;
            }

            int today = Utils.getDateAsInt();
            if (mDate != today)
            {
                clear();

                mDate = today;
            }

            mHasNewData = true;
            mContext.mTradeHistoryIsSave = false;
            //clear();
            float last = mPriceRef;
            /*
            if (mTradeTransaction.size() > 0) {
            stTradeTransaction t = (stTradeTransaction) mTradeTransaction.lastElement();
            last = t.price;
            }
             *
             */
            //=============resize buffer if needed
            int newCap = (mTradeTransactionCount + cnt) * FIELD_CNT;    //  1 item == 5 INTs
            if (newCap > mTradeTransactionBuffer.Length)
            {
                int[] p = new int[newCap + 20 * FIELD_CNT];
                Buffer.BlockCopy(mTradeTransactionBuffer, 0, p, 0, mTradeTransactionBuffer.Length*sizeof(int));
                //System.arraycopy(mTradeTransactionBuffer, 0, p, 0, mTradeTransactionBuffer.length);
                mTradeTransactionBuffer = p;
            }

            if (mTradeTransactionCount > 0)
            {
                last = getPrice(mTradeTransactionCount - 1);
            }

            float price;
            int v;
            float divide = mIsIndex?100.0f:1000.0f;

            //CHEAT if (cnt > 0) cnt = 2;
            for (int i = 0; i < cnt; i++)
            {
                /*
                stTradeTransaction t = new stTradeTransaction();

                t.time = in.readInt();

                t.price = in.readInt();
                if (!mIsIndex) {
                t.price /= 100;
                }

                if (t.price == 0) {
                t.price = last;
                } else {
                last = t.price;
                }

                t.trade_volume = in.readInt();
                t.volume = in.readInt();
                 * 
                 */
                setTime(mTradeTransactionCount, di.readInt());

                price = di.readInt()/divide;
                
                if (price == 0)
                {
                    price = last;
                }
                else
                {
                    last = price;
                }
                setPrice(mTradeTransactionCount, last);

                v = di.readInt();
                setTradeVolume(mTradeTransactionCount, v);
                if (v > mMaxTradeVolume)
                {
                    mMaxTradeVolume = v;
                }
                setVolume(mTradeTransactionCount, di.readInt());

                mTradeTransactionCount++;

                //addTransaction(t);
            }

            if (getTransactionCount() > 0)
            {
                mClose = getPrice(getTransactionCount() - 1);
            }
        }

        /*
        public void addTransaction(stTradeTransaction trans) {
        mTradeTransaction.addElement(trans);
        mHasNewData = true;
        }

        public Vector getTransactions() {
        return mTradeTransaction;
        } 
         *    
         */
        public int getTransactionCount()
        {
            return mTradeTransactionCount;//mTradeTransaction.size();
        }

        public bool hasNewData()
        {
            return mHasNewData;
        }

        public void clear()
        {
            mMaxTradeVolume = 1;
            mTradeTransactionCount = 0;
            //mTradeTransaction.removeAllElements();
        }

        public int getLastTime()
        {
            if (mTradeTransactionCount > 0)
            {
                if (mDate < mContext.mPriceboard.mDate)
                {
                    clear();
                    return 0;
                }
                else
                {
                    return getTime(mTradeTransactionCount - 1);
                }
            }
            return 0;
        }

        public void onTick()
        {
        }

       
        public static int FIELD_CNT = 4; //  4 field each item: time, price, trade_vol, total_vol

        /*
        int getDate(int transIdx){
        if (transIdx < mTradeTransactionCount){
        return mTradeTransactionBuffer[transIdx*FIELD_CNT+0];
        }

        return 0;
        }
         * 
         */
        public int getTime(int transIdx)
        {
            if (transIdx < mTradeTransactionCount)
            {
                return mTradeTransactionBuffer[transIdx * FIELD_CNT + 0];
            }

            return 0;
        }

        void setTime(int transIdx, int v)
        {
            mTradeTransactionBuffer[transIdx * FIELD_CNT + 0] = v;
        }

        public float getPrice(int transIdx)
        {
            if (transIdx < mTradeTransactionCount)
            {
                return mTradeTransactionBuffer[transIdx * FIELD_CNT + 1]/1000.0f;
            }

            return 0;
        }

        void setPrice(int transIdx, float v)
        {
            mTradeTransactionBuffer[transIdx * FIELD_CNT + 1] = (int)(v*1000);

            if (mHighest == 0 || v > mHighest)
            {
                mHighest = v;
            }
            if (mLowest == 0 || v < mLowest)
                mLowest = v;

            if (mOpen == 0)
                mOpen = v;
        }

        public int getTradeVolume(int transIdx)
        {
            if (transIdx < mTradeTransactionCount)
            {
                return mTradeTransactionBuffer[transIdx * FIELD_CNT + 2];
            }

            return 0;
        }

        void setTradeVolume(int transIdx, int v)
        {
            mTradeTransactionBuffer[transIdx * FIELD_CNT + 2] = v;
        }

        public int getVolume(int transIdx)
        {
            if (transIdx < 0)
            {
                transIdx = mTradeTransactionCount - 1;
            }
            if (transIdx >= 0 && transIdx < mTradeTransactionCount)
            {
                return mTradeTransactionBuffer[transIdx * FIELD_CNT + 3];
            }

            return 0;
        }

        void setVolume(int transIdx, int v)
        {
            mTradeTransactionBuffer[transIdx * FIELD_CNT + 3] = v;
        }

        public int getMaxTradeVolume()
        {
            return mMaxTradeVolume;
        }

        public void save(xDataOutput o)
        {
            o.writeInt(mDate);
            o.writeInt(mShareID);
            o.writeUTF(mCode);
            o.writeByte(mFloorID);
            o.writeFloat(mPriceRef);
            o.writeBoolean(mIsIndex);
            o.writeInt(mMaxTradeVolume);

            o.writeInt(mTradeTransactionCount);
            int cnt = mTradeTransactionCount*FIELD_CNT;
            for (int i = 0; i < cnt; i++)
            {
                o.writeInt(mTradeTransactionBuffer[i]);
            }
        }

        public bool load(xDataInput di)
        {
            mDate = di.readInt();

            mShareID = di.readInt();
            mCode = di.readUTF();
            mFloorID = di.readByte();
            mPriceRef = di.readFloat();
            mIsIndex = di.readBoolean();
            mMaxTradeVolume = di.readInt();

            mTradeTransactionCount = di.readInt();
            int cnt = mTradeTransactionCount*FIELD_CNT;
            if (cnt > mTradeTransactionBuffer.Length)
            {
                int[] p = new int[cnt + 20 * FIELD_CNT];
                mTradeTransactionBuffer = p;
            }

            for (int i = 0; i < cnt; i++)
            {
                mTradeTransactionBuffer[i] = di.readInt();
            }

            return true;
        }

        public bool getHiLo(float[] hl)
        {
            if (getTransactionCount() == 0)
            {
                return false;
            }
            float _hi = -1;
            float _lo = 1000000;
            for (int i = 0; i < getTransactionCount(); i++)
            {
                float price = getPrice(i);
                if (price > _hi)
                {
                    _hi = price;
                }
                if (price < _lo)
                {
                    _lo = price;
                }
            }

            hl[0] = _hi;
            hl[1] = _lo;

            return true;
        }

        public Share saveToShare()
        {
            Share share = new Share();
            share.setDataType(Share.DATATYPE_TICK);
            share.allocMemoryUsingShared(false);

            int cnt = getTransactionCount();

            float c = 0;
            int v = 0;
            for (int i = 0; i < cnt; i++)
            {
                int time = getTime(i);

                v = getTradeVolume(i);
                c = getPrice(i);

                share.addMoreCandle(c, c, c, c, c, v, time);
            }

            share.saveShare();

            return share;
        }

        public Share convertToShareData(int timeStamp, Share share)
        {
            int time = 0;
            int lastMinute = 0;

            float o = 0;
            float c = 0;
            float h = 0;
            float l = 0;
            double v = 0;
            int d = 0;

            Share outShare = share;

            if (outShare == null)
            {
                outShare = mShare;
            }
            outShare.setDataType(Share.DATATYPE_TICK);
            outShare.setID(mShare.mID);
            outShare.setCode(mShare.getCode(), getFloorID());

            outShare.removeAllCandles();
            outShare.clearCalculations();
            //mShare.mIsRealtime = true;

            int minute = 0;

            for (int i = 0; i < mTradeTransactionCount; i++)
            {
                time = getTime(i);
                int hour = (time >> 16) & 0xff;
                minute = (time >> 8) & 0xff;
                minute = hour * 60 + minute;

                float price = getPrice(i);

                if (lastMinute == 0)
                {
                    o = price;
                    c = o;
                    h = o;
                    l = o;
                    v = getTradeVolume(i);
                    d = time;
                    lastMinute = minute;
                }
                else if ((minute - lastMinute) >= timeStamp)
                {
                    outShare.addMoreCandle(o, c, o, h, l, (int)v, d);

                    o = price;
                    c = o;
                    h = o;
                    l = o;
                    v = getTradeVolume(i);
                    d = time;
                    lastMinute = minute;
                }
                else
                {
                    if (price > h) h = price;
                    if (price < l) l = price;
                    c = price;

                    v += getTradeVolume(i);
                }
            }

            if (lastMinute > 0 && c > 0 && v > 0)
            {
                outShare.addMoreCandle(o, c, o, h, l, (int)v, d);
            }

            outShare.setCursorScope(Share.SCOPE_ALL);

            return outShare;
        }
    }
}