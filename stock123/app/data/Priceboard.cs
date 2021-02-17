/*
 * To change this template, choose Tools | Templates
 * and open the template di the editor.
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

namespace stock123.app.data
{
    public class Priceboard
    {
        //==============for share===============

        public const int PB_MAX = 0;
        public const int PB_MMIN = 1;

        public const int PB_REMAIN_BUY_1 = 2;
        public const int PB_REMAIN_BUY_VOL_1 = 3;
        public const int PB_REMAIN_BUY_2 = 4;
        public const int PB_REMAIN_BUY_VOL_2 = 5;
        public const int PB_REMAIN_BUY_3 = 6;
        public const int PB_REMAIN_BUY_VOL_3 = 7;

        public const int PB_CURRENT_PRICE = 8;
        public const int PB_CURRENT_VOLUME = 9;
        public const int PB_CHANGE = 10;

        public const int PB_REMAIN_SELL_1 = 11;
        public const int PB_REMAIN_SELL_VOL_1 = 12;
        public const int PB_REMAIN_SELL_2 = 13;
        public const int PB_REMAIN_SELL_VOL_2 = 14;
        public const int PB_REMAIN_SELL_3 = 15;
        public const int PB_REMAIN_SELL_VOL_3 = 16;

        public const int PB_TOTAL_VOLUME = 17;
        public const int PB_REF = 18;
        //==============for index===============
        public const int PB_IDX_CURRENT_POINT = 0;
        public const int PB_IDX_CHANGED_POINT = 1;
        public const int PB_IDX_CHANGED_PERCENT = 2;
        public const int PB_IDX_TOTAL_VOLUME = 3;
        public const int PB_IDX_INC_NUM = 4;
        public const int PB_IDX_CE_NUM = 5;
        public const int PB_IDX_DEC_NUM = 6;
        public const int PB_IDX_FLOOR_NUM = 7;
        public const int PB_IDX_REF_NUM = 8;
        public const int PB_IDX_GTDG = 9;
        //======================================
        public const int MARKET_STATUS_UNKNOW = -1;
        public const int MARKET_STATUS_BEFORE_OPEN = 0;
        public const int MARKET_STATUS_ATO = 1;
        public const int MARKET_STATUS_OPENNING = 2;
        public const int MARKET_STATUS_ATC = 3;
        public const int MARKET_STATUS_GTTT = 4;
        public const int MARKET_STATUS_CLOSED = 5;

        //const int PRICEBOARD_FILE_VERSION = 1;
        public const String PRICEBOARD_FILE = "data\\priceboard.dat";

        public xVector mIndices = new xVector();      //  array of stPriceboardStateIndex

        public int mDate;
        public int mTime;
        public bool mHasRefvalue;
        public long mLocalTime = Utils.currentTimeMillis();
        Context mContext;
        public String[] mNextfile = { "", "", "", "", "", "", "", "" };
        //public String[] mNextfile = { "", ""};

        public float[] mOpens = null;
        public int mOpensCount = 0;

        Dictionary<int, stPriceboardState> mPriceboardMap = new Dictionary<int, stPriceboardState>();

        public Priceboard()
        {
            mContext = Context.getInstance();
        }

        //	dd/mm/yyyy hh:mm:ss
        public int parseDate(String date)
        {
            if (date == null || date.Length < 10)
            {
                return 0;
            }

            int dd = (date[0] - '0') * 10 + date[1] - '0';
            int mm = (date[3] - '0') * 10 + date[4] - '0';
            int yyyy = (date[6] - '0') * 1000 + (date[7] - '0') * 100 + (date[8] - '0') * 10 + (date[9] - '0');

            return (yyyy << 16) | (mm << 8) | dd;
        }
        //	dd/mm/yyyy hh:mm:ss

        public int parseTime(String date)
        {
            if (date == null || date.Length < 19)
            {
                return 0;
            }

            int hh = (date[11] - '0') * 10 + date[12] - '0';
            int mm = (date[14] - '0') * 10 + date[15] - '0';
            int ss = (date[17] - '0') * 10 + date[18] - '0';

            return ss + 60 * mm + 3600 * hh;
        }
        /*
            public void setRefPrice(xDataInput di) {
                if (true)   //  invalid method and not used anymore
                    return;
                int floor = di.readInt();
                int cnt = di.readInt();
                if (cnt < 0) {
                    return;
                }
                if (cnt > 5000) {
                    return;
                }

                if (floor < 1 || floor > 2) {
                    return;
                }

                stPriceboardState.alloc_buffer(floor, cnt);

                mHasRefvalue = true;
                int j = 0;
                //====================================================================
                byte[] data = di.getBytes();
                int off = di.getCurrentOffset();

                for (int i = 0; i < cnt; i++) {
                    j = 0;
                    stPriceboardState p = stPriceboardState.seekPriceboard(floor, data, off);
                    //	code
                    p.setRef(data, off);
                    off += 8 + 3 * 4;
                }
            }
         * 
         */
        /*
        public void setZeroPriceboard(xDataInput di)
        {
            String nextfile = di.readUTF();
            //nextfile = "0"; //  CHEAT

            String date = di.readUTF();
            mDate = parseDate(date);

            int floor = di.readInt();
            int max = 10;
            if (floor < 1 || floor > max)
            {
                return;
            }

            mNextfile[floor - 1] = nextfile;

            //=====================index===================
            stPriceboardStateIndex index = getPriceboardIndexOfMarket(floor);
            int oldStatus = index.market_status;
            index.current_point = di.readInt()/100.0f;
            index.changed_point = di.readInt()/100.0f;
            index.changed_percent = di.readInt();
            index.total_volume = di.readInt();
            index.inc_cnt = di.readInt();
            index.ce_cnt = di.readInt();
            index.dec_cnt = di.readInt();
            index.floor_cnt = di.readInt();
            index.ref_num = di.readInt();
            index.totalGTGD = di.readInt() / 10;			//	dv 1tr
            index.market_status = di.readInt();
            index.update_time = date;
            index.reference = index.current_point - index.changed_point;

            index.status_changed = oldStatus != index.market_status ? true : false;
            //=====================end of index===================

            int cnt = di.readInt();
            if (cnt < 0 || cnt > 5000)
            {
                return;
            }

            byte[] data = di.getBytes();

            int j = 0;
            for (int i = 0; i < cnt; i++)
            {
                stPriceboardState p = getPriceboard(floor, data, di.getCurrentOffset());

                //Utils.trace("=====" + c0 + c1 + c2);

                if (p == null)
                {
                    di.skip(Share.SHARE_CODE_LENGTH + 54);
                    continue;
                }
                di.skip(Share.SHARE_CODE_LENGTH);

                p.setZero(di);
            }
        }
         */

        public void setZeroPriceboard2016(xDataInput di)
        {
            String nextfile = di.readUTF();
            //nextfile = "0"; //  CHEAT

            String date = di.readUTF();
            mDate = parseDate(date);

            int floor = di.readInt();
            int max = 10;

            if (floor < 1 || floor > max)
            {
                return;
            }

            mNextfile[floor - 1] = nextfile;

            //=====================index===================
            stPriceboardStateIndex index = getPriceboardIndexOfMarket(floor);
            int oldStatus = index.market_status;
            index.current_point = di.readInt()/100.0f;
            index.changed_point = di.readInt()/100.0f;
            index.changed_percent = di.readInt();
            index.total_volume = di.readInt();
            index.inc_cnt = di.readInt();
            index.ce_cnt = di.readInt();
            index.dec_cnt = di.readInt();
            index.floor_cnt = di.readInt();
            index.ref_num = di.readInt();
            index.totalGTGD = di.readInt() / 10;			//	dv 1tr
            index.market_status = di.readInt();
            index.update_time = date;
            index.reference = index.current_point - index.changed_point;

            index.status_changed = oldStatus != index.market_status ? true : false;

            //=====================end of index===================

            int cnt = di.readInt();
            if (cnt < 0 || cnt > 5000)
            {
                return;
            }

            byte[] data = di.getBytes();

            int j = 0;
            for (int i = 0; i < cnt; i++)
            {
                //stPriceboardState p = getPriceboard(floor, data, di.getCurrentOffset());

                String scode = Utils.bytesNullTerminatedToString(data, di.getCurrentOffset(), 20);
                stPriceboardState p = getPriceboard(scode);

                if (p == null)
                {
                    di.skip(Share.SHARE_CODE_LENGTH + 72);
                    continue;
                }

                p.code = scode;

                //Utils.trace("=====" + c0 + c1 + c2);


                di.skip(Share.SHARE_CODE_LENGTH);

                p.setZero(di);
            }
        }

        public stPriceboardState createNewPriceboardState(int shareID)
        {
            if (mPriceboardMap.ContainsKey(shareID))
            {
                return mPriceboardMap[shareID];
            }

            stPriceboardState ps = new stPriceboardState();
            ps.id = shareID;
            mPriceboardMap.Add(shareID, ps);

            return ps;
        }

        public stPriceboardState getPriceboard(int id)
        {
            if (mPriceboardMap.ContainsKey(id))
            {
                return mPriceboardMap[id];
            }

            return new stPriceboardState();
        }

        public stPriceboardState getPriceboard(String code)
        {
            int id = mContext.mShareManager.getShareID(code);
            return getPriceboard(id);
        }

        public int getIndicesCount()
        {
            return mIndices.size();
        }

        public stPriceboardStateIndex getPriceboardIndexAt(int idx)
        {
            if (idx >= 0 && idx < mIndices.size())
            {
                return (stPriceboardStateIndex)mIndices.elementAt(idx);
            }

            return null;
        }

        public stPriceboardStateIndex getIndexByCode(String code)
        {
            stPriceboardStateIndex idx = null;
            for (int i = 0; i < mIndices.size(); i++)
            {
                idx = (stPriceboardStateIndex)mIndices.elementAt(i);
                if (idx != null && idx.code != null && idx.code.Equals(code))
                {
                    return idx;
                }
            }

            return null;
        }

        public stPriceboardStateIndex getPriceboardIndexOfMarket(int floor)
        {
            if (floor == 4)
            {
                int t = 0;
                t++;
            }
            stPriceboardStateIndex idx = null;
            for (int i = 0; i < mIndices.size(); i++)
            {
                idx = (stPriceboardStateIndex)mIndices.elementAt(i);
                if (idx.marketID == floor)
                {
                    return idx;
                }
            }

            if (true)//createIfNotExist)
            {
                idx = new stPriceboardStateIndex();

                idx.marketID = floor;
                mIndices.addElement(idx);
            }

            return idx;
        }

        public bool hasRefValues()
        {
            return mHasRefvalue;
        }

        public void resetBoard(int floor)
        {
        }

        public bool isMarketClosed()
        {
            for (int i = 1; i <= 2; i++)
            {
                stPriceboardStateIndex pb = getPriceboardIndexOfMarket(i);
                if (pb != null && pb.market_status <= MARKET_STATUS_ATC)
                {
                    return false;
                }
            }

            return true;
        }


        public void loadPriceboard()
        {
            if (true)
                return;
            if (!Context.mInvalidSavedFile)
                return;
            xDataInput di = xFileManager.readFile(PRICEBOARD_FILE, false);
            if (di != null)
            {
                int ver = di.readInt();
                if (ver < Context.FILE_VERSION_10)
                    return;

                int floor_count = di.readInt();

                int i = 0;
                //  indices
                mIndices.removeAllElements();
                for (i = 0; i < floor_count; i++)
                {
                    stPriceboardStateIndex idx = new stPriceboardStateIndex();
                    mIndices.addElement(idx);

                    idx.id = di.readShort();
                    idx.code = di.readUTF();
                    idx.marketID = di.readInt();
                    idx.current_point = di.readFloat();	//	*100
                    idx.changed_point = di.readFloat();	//	*100
                    idx.changed_percent = di.readInt();	//	*100
                    idx.total_volume = di.readInt();
                    idx.inc_cnt = di.readInt();
                    idx.ce_cnt = di.readInt();
                    idx.dec_cnt = di.readInt();
                    idx.floor_cnt = di.readInt();
                    idx.ref_num = di.readInt();
                    idx.totalGTGD = di.readInt();		//	*10000
                    idx.market_status = di.readInt();
                    idx.reference = di.readFloat();

                    idx.mDate = di.readUTF();
                }

                //  shares
                for (i = 0; i < floor_count; i++)
                {
                    stPriceboardState.load(di);
                }
            }
        }

        public void savePriceboard()
        {
            xDataOutput o = new xDataOutput(10000);
            o.writeInt(Context.FILE_VERSION);
            //  indices
            o.writeInt(mIndices.size());
            int i = 0;
            for (i = 0; i < mIndices.size(); i++)
            {
                stPriceboardStateIndex idx = (stPriceboardStateIndex)mIndices.elementAt(i);
                o.writeShort(idx.id);
                o.writeUTF(idx.code);
                o.writeInt(idx.marketID);
                o.writeFloat(idx.current_point);	//	*100
                o.writeFloat(idx.changed_point);	//	*100
                o.writeInt(idx.changed_percent);	//	*100
                o.writeInt((int)idx.total_volume);
                o.writeInt(idx.inc_cnt);
                o.writeInt(idx.ce_cnt);
                o.writeInt(idx.dec_cnt);
                o.writeInt(idx.floor_cnt);
                o.writeInt(idx.ref_num);
                o.writeInt(idx.totalGTGD);		//	*10000
                o.writeInt(idx.market_status);
                o.writeFloat(idx.reference);

                o.writeUTF(idx.mDate);
            }
            //  shares
            for (i = 0; i < mIndices.size(); i++)
            {
                stPriceboardStateIndex idx = (stPriceboardStateIndex)mIndices.elementAt(i);
                stPriceboardState.flush(o, idx.marketID);
            }

            xFileManager.saveFile(o, PRICEBOARD_FILE);
        }

        public void setOnlineIndexData(xDataInput di)
        {
            String date = di.readUTF();
            mDate = parseDate(date);
            mTime = parseTime(date);

            int floor = di.readInt();
            int max = 10;
            if (floor < 1 || floor > max)
            {
                return;
            }

            //=====================index===================
            stPriceboardStateIndex index = getPriceboardIndexOfMarket(floor);

            if (di.available() < 44)
            {
                mContext.err++;
                return;
            }
            if (index != null)
            {
                index.mDate = date;

                index.current_point = di.readInt()/100.0f;
                index.changed_point = di.readInt()/100.0f;
                index.changed_percent = di.readInt();
                index.total_volume = di.readInt();

                index.inc_cnt = di.readInt();
                index.ce_cnt = di.readInt();
                index.dec_cnt = di.readInt();
                index.floor_cnt = di.readInt();

                index.ref_num = di.readInt();
                index.totalGTGD = di.readInt() / 10;			//	dv 1tr

                index.market_status = di.readInt();

                index.update_time = date;

                index.reference = index.current_point - index.changed_point;
            }

            // set timing clock
            if (index != null && index.market_status == MARKET_STATUS_OPENNING)
            {
                //mContext.mClock.reset(mTime * 1000);
            }
            else
            {
                //mContext.mClock.reset();
            }
        }

        public static String getMartketStatus(int status)
        {
            String s = "";
            switch (status)
            {
                case MARKET_STATUS_ATC:
                    s = "ATC";
                    break;
                case MARKET_STATUS_ATO:
                    s = "ATO";
                    break;
                case MARKET_STATUS_BEFORE_OPEN:
                    s = "-";
                    break;
                case MARKET_STATUS_CLOSED:
                    s = "CLOSE";
                    break;
                case MARKET_STATUS_GTTT:
                    s = "GDTT";
                    break;
                case MARKET_STATUS_OPENNING:
                    s = "Open";
                    break;
                case MARKET_STATUS_UNKNOW:
                    break;
            }
            return s;
        }

        xVector mTradeDetails = null;
        public const String PRICEBOARD_FILE_TRADE_DETAIL = "tdetails";

        public bool hasNextFile(int floorIdx)
        {
            if (floorIdx <= 2)
            {
                return mNextfile[floorIdx - 1].Length > 0;
            }

            return false;
        }

        public bool isShareIndex(int shareID)
        {
            for (int i = 0; i < getIndicesCount(); i++)
            {
                stPriceboardStateIndex pi = getPriceboardIndexAt(i);
                if (pi.id == shareID)
                    return true;
            }

            return false;
        }

        public int getDate()
        {
            return mDate;
        }

        public void updateOpens(xDataInput di)
        {
            int cnt = di.readInt();

            mContext.mNeedRefreshOpens = false;

            if (cnt > 0)
            {
                if (mOpens == null || mOpens.Length < cnt)
                {
                    mOpens = new float[2 * (cnt + 200)];
                }
                mOpensCount = cnt;
                int id;
                float open;
                for (int i = 0; i < cnt; i++)
                {
                    id = di.readInt();
                    if (id == 578)
                    {
                        open = 0;
                        Utils.trace("578");
                    }
                    open = di.readInt()/1000.0f;
                    mOpens[2 * i] = id;
                    mOpens[2 * i + 1] = open;
                }
            }
        }

        public void setOpen(int shareID, float v)
        {
            if (mOpens == null)
            {
                return;
            }
            bool setOK = false;
            for (int i = 0; i < mOpensCount; i++)
            {
                if (mOpens[2 * i] == shareID)
                {
                    setOK = true;
                    mOpens[2 * i + 1] = v;
                    break;
                }
            }

            if (setOK == false)
            {
                mOpens[2 * mOpensCount] = shareID;
                mOpens[2 * mOpensCount + 1] = v;
                mOpensCount++;
            }
        }

        public float getOpen(int shareID)
        {
            if (mOpens == null)
                return 0;
            for (int i = 0; i < mOpensCount; i++)
            {
                if (mOpens[2 * i] == shareID)
                    return mOpens[2 * i + 1];
            }

            return 0;
        }

        public void setPriceboardDataOfGroup(xDataInput di)
        {
            int cnt = di.readInt();
            if (cnt < 0 || cnt > 5000)
            {
                mHasRefvalue = false;
                return;
            }
            
            //printf("==c================================\n");
            for (int i = 0; i < cnt; i++)
            {
                int shareID = di.readInt();
                stPriceboardState p = getPriceboard(shareID);
                
                if (p == null)
                {
                    break;
                }
                if (p.code.CompareTo("AAA") == 0)
                {
                    Utils.trace("----");
                }
                //==================================
                
                p.max = di.readInt()/1000.0f;
                p.min = di.readInt()/1000.0f;
                for (int k = 0; k < 3; k++)
                {
                    p.remain_buy_price[k] = di.readInt()/1000.0f;
                    p.remain_buy_volume[k] = di.readInt();
                }
                for (int k = 0; k < 3; k++)
                {
                    p.remain_sell_price[k] = di.readInt()/1000.0f;
                    p.remain_sell_volume[k] = di.readInt();
                }
                p.current_price_1		= di.readInt()/1000.0f;
                p.current_volume_1		= di.readInt();
                p.change				= di.readInt()/1000.0f;
                p.changePercent        = (int)((p.change*100)/p.reference);
                p.total_volume			= di.readInt();
            
                p.state = 0x0fffffff;
            }
        }
    }
}

