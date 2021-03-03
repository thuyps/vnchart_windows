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
    public class ShareManager
    {
        public static int SHARE_ID_SIZE = 1 + 2 + Share.SHARE_CODE_LENGTH;
        //public static  int FILE_COMPANY_INFO_VERSION = 0x04;
        public static String FILE_COMPANY_INFO = "data\\company_info.dat";
        public static int MAX_SHAVED_SHARE_COUNT = 25;
        static String SAVED_LIST_FILE = "data\\share_list.dat";         //  to manage invididual shares
        public static String SAVED_COMMON_SHARE_FILE = "data\\database5.dat";
        public static int SAVED_COMMON_SHARE_FILE_VERSION = 13;
        static String SHARE_IDS_FILE = "data\\share_ids.dat";

        public static int RECORDID_FILE_COMPANY_INFO = 1;
        public Dictionary<int, Share> mShares = new Dictionary<int, Share>();           //  Share array
        public Dictionary<int, stCompanyInfo> mCompanyInfos = new Dictionary<int, stCompanyInfo>();     //  stCompany
        int mShareIDCount = 0;
        public byte[] mShareIDs = null;         //  array of [floor:1 | id: 2 | share_code:8]
        xVectorInt vMarketIDs = new xVectorInt(3000);
        xVectorInt vIDs = new xVectorInt(3000);
        xVector vCodes = new xVector(3000);

        Context mContext;
        //===========candle area
        public static int[] volumes;
        //==============================================
        public static int NET_CANDLE_SIZE = 36;
        public static int MAX_COMMON_CANDLES = 600;   //  1 year
        public static int CANDLE_SIZE = 36;
        byte[] mCommonData = null;
        //  item: code:8 + candle_cnt:2 + floor:2 + [CANDLE_SIZE*MAX_COMMON_CANDLES];
        //  entire: share_cnt:4 + [item];

        public ShareManager()
        {
            mContext = Context.getInstance();
        }

        public void processCompanyInfo(xDataInput di)
        {
            if (di == null || di.available() < 100)
            {
                return;
            }

            xDataOutput o = null;
            o = new xDataOutput(di.available() + 20);
            o.writeInt(Context.FILE_VERSION_LATEST);
            o.write(di.getBytes(), di.getCurrentOffset(), di.available());

            xFileManager.saveFile(o, FILE_COMPANY_INFO);

            mContext.mCompanyUpdateTime = Utils.getDateAsInt();

            //	now process
            loadCompanyInfo();
        }

        public void processIndicesIDs(xDataInput di)
        {
            int cnt = di.readByte();// .readInt();
            byte[] p = di.getBytes();
            int off = di.getCurrentOffset();
            stPriceboardStateIndex idx;

            stPriceboardState ps = mContext.mPriceboard.getPriceboard(1483);
            
            for (int i = 0; i < mContext.mPriceboard.getIndicesCount(); i++)
            {
                stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(i);
                /*
                if (pi.marketID == 5)
                {
                    pi.code = ps.code;// "^Upcom";
                }
                 */
            }
            
            for (int i = 0; i < cnt; i++)
            {
                byte floor = p[off++];
                Utils.sb.Length = 0;
                while (p[off] != 0)
                {
                    Utils.sb.Append((char)p[off]);
                    off++;
                }
                String code = Utils.sb.ToString();
                idx = mContext.mPriceboard.getPriceboardIndexOfMarket(floor);
                idx.code = code;
                idx.id = mContext.mShareManager.getShareID(code);
                idx.supported = true;
                //=========================
                off++;
            }
            
            //  ^UPCOM
            /*
            idx = mContext.mPriceboard.getPriceboardIndexOfMarket(3);
            idx.code = "^UPCOM";
            idx.id = 752;
            idx.supported = true;
            */
        }

        int getMarketID(int shareID, int marketID)
        {
            if (mCompanyInfos.ContainsKey(shareID)){
                stCompanyInfo inf = mCompanyInfos[shareID];
                return inf.floor;
            }

            return marketID;
        }

        public void processShareIDs(xDataInput di)
        {
            int floor_cnt = di.readInt();  //  floor count

            //byte[] data = di.getBytes();
            //int off = di.getCurrentOffset();

            byte[] p = new byte[10000 * ShareManager.SHARE_ID_SIZE];
            int k = 0;
            int k_old;

            int kkk = 0;

            Priceboard priceboard = mContext.mPriceboard;

            try
            {
                for (int i = 0; i < floor_cnt; i++)
                {
                    byte marketID = di.readByte();
                    //  make sure the floor is created
                    mContext.mPriceboard.getPriceboardIndexOfMarket(marketID);
                    //  share count
                    int share_cnt = di.readInt();
                    mShareIDCount += share_cnt;
                   
                    //  data now
                    int share_id;
                    float ce, reference, floor_price;
                    for (int j = 0; j < share_cnt; j++)
                    {
                        kkk = j;
                        if (kkk == 376)
                        {
                            int t = 0;
                        }
                        k_old = k;
                        //  floor
                        p[k++] = marketID;
                        //  id
                        share_id = di.readShort();
                        p[k++] = (byte)((share_id>>8)&0xff);    //  share id
                        p[k++] = (byte)(share_id&0xff);
                        //Console.WriteLine("ShareID: " + share_id);
                        if (share_id == 690)
                        {
                            share_id = share_id;
                        }
                        stPriceboardState pb = priceboard.createNewPriceboardState(share_id);// stPriceboardState.seekPriceboardByID(marketID, share_id);
                        pb.setID(share_id);

                        if (share_id == 1439)
                        {
                            share_id = 1439;
                        }
                        marketID = (byte)getMarketID(share_id, (int)marketID);
                        pb.setMarketID(marketID);
                        //  code
                        int codeOffset = k;
                        while (true)
                        {
                            byte c = di.readByte();
                            p[k++] = c;
                            if (c == 0)
                                break;
                        }

                        if ('A' == (char)p[codeOffset + 0]
                            && 'R' == (char)p[codeOffset+1]
                            && 'T' == (char)p[codeOffset+2])
                        {
                            int kk = 0;
                        }

                        pb.setCode(p, codeOffset);

                        ce = di.readInt()/1000.0f;
                        floor_price = di.readInt()/1000.0f;
                        reference = di.readInt()/1000.0f;

                        pb.setCe(ce);
                        pb.setRef(reference);
                        pb.setFloor(floor_price);

                        k = k_old + ShareManager.SHARE_ID_SIZE;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.GetType() + e.ToString());
            }
            mShareIDs = new byte[k];
            Buffer.BlockCopy(p, 0, mShareIDs, 0, k);

            p = null;

            if (k > 0)
            {
                saveShareIDs();
            }

            loadShareIDs();
        }

        public int getTotalShareIDCount()
        {
            return mShareIDCount;
        }

        public int getShareIDAt(int idx, int[] marketID)
        {
            if (idx < vIDs.size())
            {
                marketID[0] = vMarketIDs.elementAt(idx);
                return vIDs.elementAt(idx);
            }

            return 0;
        }

        void saveShareIDs()
        {
            if (mShareIDs != null)
            {
                xFileManager.saveFile(mShareIDs, 0, mShareIDs.Length, SHARE_IDS_FILE);
            }
        }

        public void loadShareIDs()
        {
            if (!Context.mInvalidSavedFile)
                return;
            xDataInput di = xFileManager.readFile(SHARE_IDS_FILE, false);
            if (di != null)
            {
                mShareIDs = di.getBytes();
            }
            else
            {
                return;
            }

            vMarketIDs.removeAllElements();
            vIDs.removeAllElements();
            vCodes.removeAllElements();
            if (vIDs.size() == 0)
            {
                int itemSize = SHARE_ID_SIZE;

                byte[] p = mShareIDs;
                String s = null;

                int len = mShareIDs.Length;
                for (int i = 0; i < len; i += itemSize)
                {
                    int marketID = p[i];
                    int item = Utils.readShort(p, i + 1);

                    {
                        s = Share.readTerminatedString(p, i + 3);
                        vMarketIDs.addElement(marketID);
                        vIDs.addElement(item);
                        vCodes.addElement(s);
                    }
                }
            }
        }

        public void clearCompanyInfo()
        {
            mCompanyInfos.Clear();
        }

        public int getCompanyInfoCount()
        {
            return mCompanyInfos.Count;
        }

        String readCode(byte[] p, int offset)
        {
            Utils.sb.Length = 0;
            for (int i = 0; i < Share.SHARE_CODE_LENGTH; i++)
            {
                if (p[offset + i] == 0)
                {
                    break;
                }

                Utils.sb.Append((char)p[offset + i]);
            }

            return Utils.sb.ToString();
        }

        void correctMarketId()
        {
            //String groups[] = {""};
        }

        public void loadCompanyInfo()
        {
            xDataInput di = xFileManager.readFile(FILE_COMPANY_INFO, false);
            if (di == null || di.available() < 100)
            {
                return;
            }

            clearCompanyInfo();

            int ver = di.readInt();
            if (ver < Context.FILE_VERSION_LATEST)
                return;

            int cnt = di.readInt();

            //String s = null;

            int quy_cnt;
            int shareID;

            for (int i = 0; i < cnt; i++)
            {
                //s = readCode(di.getStream(), di.getCurrentOffset());
                //di.skip(Share.SHARE_CODE_LENGTH);

                shareID = di.readInt();

                stCompanyInfo inf = null;

                if (mCompanyInfos.ContainsKey(shareID))
                {
                    inf = mCompanyInfos[shareID];
                }
                else
                {
                    inf = new stCompanyInfo();
                }
                mCompanyInfos.Add(shareID, inf);

                //s = mContext.mShareManager.getShareCode(shareID);

                //inf.code = s;
                inf.shareID = shareID;
                inf.floor = di.readByte();
                inf.company_name = di.readUTF();		//	company name

                //di.skipUTF();       //  website
                inf.EPS = di.readInt();		//	float
                inf.Beta = di.readInt();	//	float
                inf.PE = di.readInt();		//	float
                inf.book_value = di.readInt();

                inf.volume = di.readInt();
                inf.vontt = di.readInt();

                inf.ROA = di.readInt();
                inf.ROE = di.readInt();

                inf.volumeOwnedByForeigner = di.readInt();
                inf.max52weeks = di.readInt();
                inf.min52weeks = di.readInt();

                quy_cnt = di.readByte();
                if (quy_cnt > 4) quy_cnt = 4;
                for (int j = 0; j < quy_cnt; j++)
                {
                    if (j == 0)
                    {
                        inf.Q1 = di.readInt();
                        inf.loi_nhuan1 = di.readInt();
                        inf.doanh_thu1 = di.readInt();
                    }
                    else if (j == 1)
                    {
                        inf.Q2 = di.readInt();
                        inf.loi_nhuan2 = di.readInt();
                        inf.doanh_thu2 = di.readInt();
                    }
                    if (j == 2)
                    {
                        inf.Q3 = di.readInt();
                        inf.loi_nhuan3 = di.readInt();
                        inf.doanh_thu3 = di.readInt();
                    } if (j == 3)
                    {
                        inf.Q4 = di.readInt();
                        inf.loi_nhuan4 = di.readInt();
                        inf.doanh_thu4 = di.readInt();
                    }
                }
            }
        }

        public stCompanyInfo getCompanyInfo(int shareID)
        {
            if (mCompanyInfos.Count == 0)
            {
                loadCompanyInfo();
            }

            if (mCompanyInfos.ContainsKey(shareID))
            {
                return mCompanyInfos[shareID];
            }

            return null;
        }

        //=======================shor share IDs===================
        public int getShareMarketID(int id)
        {
            if (vIDs.size() > 0)
            {
                int cnt = vIDs.size();
                for (int i = 0; i < vIDs.size(); i++)
                {
                    if (vIDs.elementAt(i) == id)
                    {
                        return vMarketIDs.elementAt(i);
                    }
                }
            }
            return 0;
        }
       
        public String getShareCode(int id)
        {
            if (vIDs.size() == 0)
            {
                return null;
            }

            int cnt = vIDs.size();
            for (int i = 0; i < cnt; i++)
            {
                if (vIDs.elementAt(i) == id)
                {
                    return (string)vCodes.elementAt(i);
                }
            }
            return "";
        }

        public int getShareID(String code)
        {
            int cnt = vIDs.size();

            code = code.ToUpper();

            for (int i = 0; i < cnt; i++)
            {
                String s = (String)vCodes.elementAt(i);
                if (s.CompareTo(code) == 0)
                {
                    return vIDs.elementAt(i);
                }
            }
            return 0;
        }
        //=============================================================
        public int getShareCount()
        {
            return vIDs.size();
        }
        public void loadAllShares()
        {
            if (vIDs.size() == 0)
            {
                return;
            }

            int id = -1;

            int market;
            int shareID;

            int cnt = vIDs.size();
            for (int i = 0; i < cnt; i++)
            {
                if (i == 275)
                {
                    Utils.trace("257");
                }
                //Utils.trace("i=" + i);
                market = vMarketIDs.elementAt(i);
                shareID = vIDs.elementAt(i);

                getShareQuick(shareID, (byte)market, 20);
            }
        }

        public Share getShareQuick(int shareID, byte marketID, int maxLastCandle)
        {
            if (shareID <= 0)
            {
                return null;
            }

            Share s;

            if (mShares.ContainsKey(shareID))
            {
                return mShares[shareID];
            }

            //  not found, create a new
            s = new Share();
            s.setID(shareID);
            s.setCode(getShareCode(shareID), marketID);

            s.allocMemoryUsingShared(true);

            mShares.Add(shareID, s);

            Context.getInstance().mShareManager.loadShareFromCommon(s, maxLastCandle, true);
            //  for sorting====quick
            s.calcTotalVolume(10);
            //======================

            return s;
        }

        public Share getShare(string code)
        {
            if (code == null || code.Length == 0)
            {
                return null;
            }
            int shareID = getShareID(code);
            if (shareID > 0)
            {
                return getShare(shareID);
            }

            return null;
        }

        public Share getShare(int shareID)
        {
            if (shareID <= 0)
            {
                return null;
            }

            Share s;

            if (mShares.ContainsKey(shareID))
            {
                return mShares[shareID];
            }
            
            //  not found, create a new
            int marketID = getShareMarketID(shareID);

            s = new Share();
            s.setID(shareID);
            s.setCode(getShareCode(shareID), marketID);

            if (shareID == 752)
            {
                s.mCode = "^VN30";
            }

            s.allocMemoryUsingShared(true);
            //s.loadShare();

            mShares.Add(shareID, s);

            return s;
        }

        public void removeAllShareFiles()
        {
            xFileManager.removeFile(FILE_COMPANY_INFO);
            xFileManager.removeFile(SHARE_IDS_FILE);
            xFileManager.removeFile(SAVED_COMMON_SHARE_FILE);
            xFileManager.removeFile(SAVED_LIST_FILE);
            //=======================saveList();

            mCommonDataLoaded = false;
        }

        //==========================================================================
        int seekToShareOffset(int share_id)
        {
            if (share_id < 0)
                return -1;
            int share_cnt = Utils.readInt(mCommonData, 0);
            share_cnt = share_cnt & 0x00ffffff; //  first byte contains version

            int item_size = 2 + 2 + 2 + CANDLE_SIZE * MAX_COMMON_CANDLES;
            int pos = 8;
            byte b0 = (byte)((share_id >> 8) & 0xff);
            byte b1 = (byte)(share_id & 0xff);
            share_cnt = 0;  //  share_cnt khong phan anh dung sinh ra loi
            if (share_cnt == 0)
            {
                share_cnt = (mCommonData.Length - 8) / item_size;
            }
            /*
            Utils.trace("=====================================");
            for (int i = 0; i < share_cnt; i++)
            {
                int shareID = (mCommonData[pos] << 8) | mCommonData[pos + 1];
                Utils.trace(" " + shareID);

                pos += item_size;
            }
             */

            for (int i = 0; i < share_cnt; i++)
            {
                if (mCommonData[pos] == 0 && mCommonData[pos + 1] == 0)
                {
                    break;  //  end of the list
                }
                if (mCommonData[pos] == b0 && mCommonData[pos + 1] == b1)
                {
                    return pos + 2;
                }

                pos += item_size;
            }
            //  not found, add to the tail
            int cursor = pos;
            if (cursor > mCommonData.Length - 2*item_size)
            {
                int newSize = 4 + 4 + (share_cnt + 20) * item_size;
                byte[] pNew = new byte[newSize];
                Buffer.BlockCopy(mCommonData, 0, pNew, 0, mCommonData.Length);

                mCommonData = pNew;

                //gc();
            }

            mCommonData[cursor + 0] = b0;
            mCommonData[cursor + 1] = b1;

            return cursor + 2;
        }
        /*
        public int getShareCount()
        {
            int pos = 8;
            int share_cnt = Utils.readInt(mCommonData, 0);
            share_cnt = share_cnt & 0x00ffffff; //  first byte contains version

            return share_cnt;
        }
         */


        public void replace1ShareDataToCommon(Share share)
        {
            int share_id = share.mID;// getShareID(s.getCode());
            int pos = seekToShareOffset(share_id);
            float open;
            float close;
            float hi;
            float lo;
            float reference;
            int vol;
            int date;
            int nnmua;
            int nnban;

            int matchedDate = 0;
            int currentCandleCount = share.getCandleCount();

            
            if (pos > 0)
            {
                int common_candle_cnt = Utils.readShort(mCommonData, pos);

                pos += 2;
                int floor = Utils.readShort(mCommonData, pos);
                pos += 2;

                //  replace data cua share boi common data
                int dateStart = 6 * 4;
                int commonFirstDate = Utils.readInt(mCommonData, pos + dateStart);
                int j = 0;
                for (j = 0; j < currentCandleCount; j++)
                {
                    if (share.mCDate[j] >= commonFirstDate)
                    {
                        break;
                    }
                }
                if (j + common_candle_cnt < share.getCandleCnt())
                {
                    //  invalid
                    return;
                }
                //==============================
                float devision = 1000.0f;

                for (int i = 0; i < common_candle_cnt; i++)
                {
                    share.mCOpen[j] = Utils.readInt(mCommonData, pos) / devision;
                    pos += 4;
                    share.mCClose[j] = Utils.readInt(mCommonData, pos) / devision; ;
                    pos += 4;
                    share.mCHighest[j] = Utils.readInt(mCommonData, pos) / devision;
                    pos += 4;
                    share.mCLowest[j] = Utils.readInt(mCommonData, pos) / devision;
                    pos += 4;
                    share.mCRef[j] = Utils.readInt(mCommonData, pos) / devision;
                    pos += 4;
                    share.mCVolume[j] = Utils.readInt(mCommonData, pos);
                    pos += 4;
                    share.mCDate[j] = Utils.readInt(mCommonData, pos);
                    pos += 4;

                    share.mNNMua[j] = Utils.readInt(mCommonData, pos);
                    pos += 4;

                    share.mNNBan[j] = Utils.readInt(mCommonData, pos);
                    pos += 4;

                    if (share.mCOpen[i] == 0 || share.mCClose[j] == 0 || share.mCLowest[j] == 0 || share.mCHighest[j] == 0)
                    {
                        float notZero = share.mCOpen[i];
                        if (notZero == 0) notZero = share.mCClose[j];
                        if (notZero == 0) notZero = share.mCLowest[j];
                        if (notZero == 0) notZero = share.mCHighest[j];
                        if (notZero == 0) notZero = share.mCRef[j];

                        if (share.mCOpen[j] == 0) share.mCOpen[j] = notZero;
                        if (share.mCClose[j] == 0) share.mCClose[j] = notZero;
                        if (share.mCLowest[j] == 0) share.mCLowest[j] = notZero;
                        if (share.mCHighest[j] == 0) share.mCHighest[j] = notZero;
                    }

                    j++;
                }

                share.mCandleCnt = j;
            }
        }

         //	36bytes for each candle
        public void replaceShareDataToCommon(int share_id, int marketID, byte[] data, int off, int candleCnt)
        {
            if (share_id <= 0)
                return;

            if (share_id == 578)
            {
                share_id++;
                share_id--;
                int k = 0;
            }

            //=============================
            if (mCommonData == null)
            {
                return;
            }

            int candleSize = CANDLE_SIZE;
            
            int p0 = seekToShareOffset(share_id);

            int p1 = p0 + 4;
            if (p0 == 0)
            {
                return;
            }

            int open;
            int close;
            int highest;
            int lowest;
            int reference;
            int ce;
            int volume;
            int date;

            int j = off;
            int tmp = 0;
            int start_off = -1;
            int old_candle_cnt = Utils.readShort(mCommonData, p0);
            //int skipCandles = candleCnt - MAX_COMMON_CANDLES;
            //if (skipCandles > 0)
            //{
                //j += skipCandles * CANDLE_SIZE;
            //}
            for (int i = 0; i < candleCnt; i++)
            {
                open = Utils.readInt(data, j);
                j += 4;
                close = Utils.readInt(data, j);
                j += 4;
                highest = Utils.readInt(data, j);
                j += 4;
                lowest = Utils.readInt(data, j);
                j += 4;
                //j += 4;//c.floor = di.readInt()/100;		j += 4;
                reference = Utils.readInt(data, j);
                j += 4;

                //ce = Utils.readInt(data, j) / 100;
                //j += 4;
                if (i == 995)
                {
                    int k = 0;
                }

                volume = Utils.readInt(data, j);
                j += 4;

                int foreignBuy = Utils.readInt(data, j);
                j += 4;
                int foreignShell = Utils.readInt(data, j);
                j += 4;

                date = Utils.readInt(data, j);
                j += 4;
                //  tim den date 
                int dateOff = 6*4;
                
                //  reset start_off
                start_off = p1;

                for (int k = 0; k < old_candle_cnt; k++)
                {
                    int old_date = Utils.readInt(mCommonData, start_off + dateOff);
                    if (old_date == date)
                    {
                        //  replace
                        //  start_off -= dateOff;
                        Utils.writeInt(mCommonData, start_off, open);
                        start_off += 4;
                        Utils.writeInt(mCommonData, start_off, close);
                        start_off += 4;
                        Utils.writeInt(mCommonData, start_off, highest);
                        start_off += 4;
                        Utils.writeInt(mCommonData, start_off, lowest);
                        start_off += 4;
                        Utils.writeInt(mCommonData, start_off, reference);
                        start_off += 4;
                        //Utils.writeInt(mCommonData, start_off, ce);   start_off += 4;
                        Utils.writeInt(mCommonData, start_off, volume);
                        start_off += 4;
                        Utils.writeInt(mCommonData, start_off, date);
                        start_off += 4;

                        Utils.writeInt(mCommonData, start_off, foreignBuy);
                        start_off += 4;

                        Utils.writeInt(mCommonData, start_off, foreignShell);
                        start_off += 4;
                        break;
                    }
                    start_off += CANDLE_SIZE;
                }
            }
        }

        //	adding data format: int:open, int:close, int highest, int lowest, int floor, int ref, int ce, int volume, int date

        //	36bytes for each candle
        public void addShareDataToCommon(int share_id, int marketID, byte[] data, int off, int candleCnt)
        {
            if (share_id <= 0)
                return;
            //if (candleCnt > 599)
            //{
                //int kkk = 0;
            //}
            if (share_id == 165)
            {
                int kk = 0;
            }
            //=============================

            //=============================

            if (mCommonData == null)
            {
                mCommonData = new byte[CANDLE_SIZE * 5000 * MAX_COMMON_CANDLES];//400K
            }

            int candleSize = CANDLE_SIZE;
            int newDataSize = candleCnt * candleSize;

            int p0 = seekToShareOffset(share_id);
            //  share_id(2) | candle_cnt(2) | marketid(2) | [CANDLEs]
            //  p0=>candle_cnt(2)
            //  p1=>[CANDLEs]
            //  date pos = 7*4
            int p1 = p0 + 4;
            if (p0 == 0)
            {
                return;
            }

            int old_candle_cnt = Utils.readShort(mCommonData, p0);

            //--du lieu moi se ghi de du lieu cu
            int firstDateOff = 8 * 4;
            int newFirstDate = Utils.readInt(data, off + firstDateOff);
            int pos = p1 + 6*4;
            //  find the newFirstDate in current data
            for (int i = 0; i < old_candle_cnt; i++)
            {
                int oldDate = Utils.readInt(mCommonData, pos);
                if (oldDate >= newFirstDate)
                {
                    old_candle_cnt = i;
                    break;
                }
                pos += CANDLE_SIZE;
            }
            //-------------------------------------------
            //	move backward memory di buff
            int buffSize = MAX_COMMON_CANDLES * candleSize;   //  size for each share di the commondata

            int totalCandle = candleCnt + old_candle_cnt;
            int oldCandleSouldReject = 0;

            if (totalCandle > MAX_COMMON_CANDLES)
            {
                oldCandleSouldReject = totalCandle - MAX_COMMON_CANDLES;
                old_candle_cnt -= oldCandleSouldReject;
            }
            if (old_candle_cnt < 0)
            {
                old_candle_cnt = 0;
            }

            if (oldCandleSouldReject > 0)
            {
                int data_off = oldCandleSouldReject * CANDLE_SIZE;
                int len = (old_candle_cnt - oldCandleSouldReject) * CANDLE_SIZE;
                if (len > 0)
                {
                    for (int i = data_off; i < data_off + len; i++)
                    {
                        mCommonData[p1 + i - data_off] = mCommonData[p1 + i];
                    }
                }
            }

            int skipCandle = candleCnt - MAX_COMMON_CANDLES;
            if (skipCandle < 0)
            {
                skipCandle = 0;
            }
            int start_off = 0;

            start_off = old_candle_cnt * CANDLE_SIZE;
            start_off += p1;

            int open;
            int close;
            int highest;
            int lowest;
            int reference;
            int ce;
            int volume;
            int date;

            int j = off;
            int tmp = 0;

            int t = (2012 << 16) | (7 << 8) | 3;

            for (int i = skipCandle; i < candleCnt; i++)
            {
                if (i == candleCnt - 10)
                {
                    open = 0;
                }
                old_candle_cnt++;

                open = Utils.readInt(data, j);
                j += 4;
                close = Utils.readInt(data, j);
                j += 4;
                highest = Utils.readInt(data, j);
                j += 4;
                lowest = Utils.readInt(data, j);
                j += 4;
                //j += 4;//c.floor = di.readInt()/100;		j += 4;
                reference = Utils.readInt(data, j);
                j += 4;

                //ce = Utils.readInt(data, j) / 100;
                //j += 4;

                volume = Utils.readInt(data, j);
                j += 4;

                int foreignBuy = Utils.readInt(data, j);
                j += 4;
                int foreignShell = Utils.readInt(data, j);
                j += 4;

                date = Utils.readInt(data, j);
                //if (date >= t)  //  DEBUG
                //{
                  //  old_candle_cnt--;
                    //break;
                //}
                j += 4;
                //	correct data:
                if (open != 0)
                {
                    tmp = open;
                }
                if (close != 0 && tmp == 0)
                {
                    tmp = close;
                }
                if (highest != 0 && tmp == 0)
                {
                    tmp = highest;
                }
                if (lowest != 0 && tmp == 0)
                {
                    tmp = lowest;
                }
                if (reference != 0 && tmp == 0)
                {
                    tmp = reference;
                }
                //if (ce != 0 && tmp == 0)
                //{
                    //tmp = ce;
                //}

                if (open == 0)
                {
                    open = tmp;
                }
                if (close == 0)
                {
                    close = tmp;
                }
                if (highest == 0)
                {
                    highest = tmp;
                }
                if (lowest == 0)
                {
                    lowest = tmp;
                }
                if (reference == 0)
                {
                    reference = close;
                }

                //if (ce == 0)
                //{
                    //ce = tmp;
                //}

                if (tmp == 0) //	reject bad candle
                {
                    old_candle_cnt--;
                    continue;
                }
                //=========saving==================
                Utils.writeInt(mCommonData, start_off, open);
                start_off += 4;
                Utils.writeInt(mCommonData, start_off, close);
                start_off += 4;
                Utils.writeInt(mCommonData, start_off, highest);
                start_off += 4;
                Utils.writeInt(mCommonData, start_off, lowest);
                start_off += 4;
                Utils.writeInt(mCommonData, start_off, reference);
                start_off += 4;
                //Utils.writeInt(mCommonData, start_off, ce);   start_off += 4;
                Utils.writeInt(mCommonData, start_off, volume);
                start_off += 4;
                Utils.writeInt(mCommonData, start_off, date);
                start_off += 4;

                Utils.writeInt(mCommonData, start_off, foreignBuy);
                start_off += 4;

                Utils.writeInt(mCommonData, start_off, foreignShell);
                start_off += 4;

                if (date > mContext.mLastDayOfShareUpdate)
                    mContext.mLastDayOfShareUpdate = date;

                if (i == candleCnt - 1 && mContext.mLastUpdateAllShare < date)
                {
                    mContext.mLastUpdateAllShare = date;
                    if (mContext.mLastUpdateAllShare > Utils.getDateAsInt()) //  invalid ???
                    {
                        mContext.mLastUpdateAllShare = Utils.getDateAsInt();
                    }
                }
            }

            Utils.writeshort(mCommonData, p0, old_candle_cnt);
            Utils.writeshort(mCommonData, p0 + 2, marketID);
        }


        public void saveCommonShareData(int share_cnt)
        {
            if (mCommonData == null || share_cnt == 0)
            {
                return;
            }

            int t = (SAVED_COMMON_SHARE_FILE_VERSION << 24) | share_cnt;

            Utils.writeInt(mCommonData, 0, t);//share_cnt);
            //mContext.mLastDayOfShareUpdate = (2012 << 16) | (7 << 8) | 3; //DEBUG
            Utils.writeInt(mCommonData, 4, mContext.mLastDayOfShareUpdate);

            int item_size = 2 + 2 + 2 + CANDLE_SIZE * MAX_COMMON_CANDLES;
            int totalSize = 4 + 4 + share_cnt * item_size;

            xFileManager.saveFile(mCommonData, 0, totalSize, SAVED_COMMON_SHARE_FILE);
        }

        public void prepareCommonShareBuffer(int shareCnt)
        {
            if (shareCnt <= 0)
                return;

            int item_size = 2 + 2 + 2 + CANDLE_SIZE * MAX_COMMON_CANDLES;
            int size = 4 + 4 + (shareCnt+50) * (item_size);

            if (mCommonData == null || mCommonData.Length < size)
            {
                mCommonData = new byte[size];
            }
        }

        public void reloadCommonData()
        {
            //mCommonData = null;
            mCommonDataLoaded = false;
            loadCommonData();
        }

        bool mCommonDataLoaded = false;
        public void loadCommonData()
        {
            if (!Context.mInvalidSavedFile)
                return;
            if (mCommonDataLoaded == false)
            {
                xDataInput di = xFileManager.readFile(SAVED_COMMON_SHARE_FILE, false);
                if (di != null)
                {
                    int ver = di.readInt() >> 24;
                    if (ver == SAVED_COMMON_SHARE_FILE_VERSION)
                    {
                        byte[] p = di.getBytes();
                        //  mCommonData 
                        if (mCommonData == null || mCommonData.Length < p.Length)
                        {
                            int item_size = 2 + 2 + 2 + CANDLE_SIZE * MAX_COMMON_CANDLES;
                            int cnt = p.Length / item_size;

                            mCommonData = new byte[p.Length + (50 * item_size)];
                        }
                        mCommonDataLoaded = true;
                        Buffer.BlockCopy(p, 0, mCommonData, 0, p.Length);
                        //mCommonData[0] = 0; //  clear version field

                        mContext.mLastDayOfShareUpdate = Utils.readInt(mCommonData, 4);
                    }
                }
            }
        }

        public void loadShareFromCommon(Share s, int maxLastCandle, bool useCommonData)
        {
            loadCommonData();
   
            if (mCommonData == null)
            {
                return;
            }

            //s.clearCalculations();

            int share_id = s.mID;// getShareID(s.getCode());
            int pos = seekToShareOffset(share_id);
            float open;
            float close;
            float hi;
            float lo;
            float reference;
            int vol;
            int date;

            if (maxLastCandle == -1)
                maxLastCandle = 800;

            if (pos > 0)
            {
                s.removeAllCandles();

                int candle_cnt = Utils.readShort(mCommonData, pos);
                pos += 2;
                int floor = Utils.readShort(mCommonData, pos);
                pos += 2;

                int skipCandle = 0;
                if (maxLastCandle > 0 && maxLastCandle < candle_cnt)
                {
                    skipCandle = candle_cnt - maxLastCandle;
                }

                float devision = 1000.0f;
                if (s.isIndex())
                    devision = 100 / 0f;

                for (int i = 0; i < candle_cnt; i++)
                {
                    if (i < skipCandle)
                    {
                        pos += CANDLE_SIZE;
                        continue;
                    }

                    open = Utils.readInt(mCommonData, pos) / devision;
                    pos += 4;
                    close = Utils.readInt(mCommonData, pos) / devision; ;
                    pos += 4;
                    hi = Utils.readInt(mCommonData, pos) / devision;
                    pos += 4;
                    lo = Utils.readInt(mCommonData, pos) / devision;
                    pos += 4;
                    reference = Utils.readInt(mCommonData, pos) / devision;
                    pos += 4;
                    vol = Utils.readInt(mCommonData, pos);
                    pos += 4;
                    date = Utils.readInt(mCommonData, pos);
                    pos += 4;

                    int nnmua = Utils.readInt(mCommonData, pos);
                    pos += 4;
                    int nnban = Utils.readInt(mCommonData, pos);
                    pos += 4;

                    s.addMoreCandle(open, close, reference, hi, lo, vol, date);
                }
            }
            //========debug only=====
            //if (s.mCandleCnt > 261)
                //s.mCandleCnt = 261;
            //=======================

            //mCommonData = null;
            //System.gc();
            s.appendTodayCandle();
        }

        public int getLastUpdateAllShare()
        {
            if (mContext.mLastUpdateAllShare == 0)
            {
                int today = Utils.getDateAsInt(40);
                mContext.mLastUpdateAllShare = today;
            }

            return mContext.mLastUpdateAllShare;
        }

        int getLastupdate()
        {
            int shareID = getShareID("VNM");
            if (mShares.ContainsKey(shareID))
            {
                Share share = mShares[shareID];
                if (share.getCandleCnt() > 0){
                    return share.getLastCandleDate();
                }
            }

            return 0;
        }

        public int getVnindexCnt()
        {
            return mContext.mPriceboard.getIndicesCount();
        }

        public Share getVnindexShareAt(int idx)
        {
            for (int i = 0; i < mContext.mPriceboard.getIndicesCount(); i++)
            {
                if (i == idx)
                {
                    stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(i);
                    Share share = getShare(pi.id);

                    return share;
                }
            }

            return null;
        }
        //======================sort=====================
        public void sortMostIncreased(xVector shares, int days)
        {
            String format = "{0:F1}";
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                if (!share.isIndex())
                {
                    if (share.getCode().CompareTo("IDJ") == 0)
                    {
                        Utils.trace("found");
                    }
                    share.mSortParam = share.getIncreasedPercentInDays(days);
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat(format, (float)(share.mSortParam));
                    share.mCompareText = Utils.sb.ToString();
                }
            }

            sort(shares);
        }

        public void sortHighestEPS(xVector shares)
        {
            String format = "{0:F1}";
            mContext.mShareManager.loadCompanyInfo();
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                stCompanyInfo inf = getCompanyInfo(share.mID);
                if (inf != null)
                {
                    share.mSortParam = inf.EPS;
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat(format, (float)(inf.EPS / 1000));
                    share.mCompareText = Utils.sb.ToString();
                }
                else
                    share.mSortParam = 0;
            }

            sort(shares);
        }

        public void sortLowestPE(xVector shares)
        {
            String format = "{0:F1}";
            mContext.mShareManager.loadCompanyInfo();
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                stCompanyInfo inf = getCompanyInfo(share.mID);
                if (inf != null)
                {
                    if (inf.PE <= 0)
                        share.mSortParam = 100000;
                    else
                        share.mSortParam = inf.PE;
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat(format, (float)(inf.PE / 100));
                    share.mCompareText = Utils.sb.ToString();
                }
                else
                    share.mSortParam = 100000;
            }

            sort(shares);
        }

        public void sortLowestROA(xVector shares)
        {
            String format = "{0:F1}%";
            mContext.mShareManager.loadCompanyInfo();
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                stCompanyInfo inf = getCompanyInfo(share.mID);
                if (inf != null)
                {
                    share.mSortParam = inf.ROA;
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat(format, (float)inf.ROA);
                    share.mCompareText = Utils.sb.ToString();
                }
                else
                    share.mSortParam = 0;
            }

            sort(shares);
        }

        public void sortLowestROE(xVector shares)
        {
            String format = "{0:F1}%";
            mContext.mShareManager.loadCompanyInfo();
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                stCompanyInfo inf = getCompanyInfo(share.mID);
                if (inf != null)
                {
                    share.mSortParam = inf.ROE;
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat(format, (float)inf.ROE);
                    share.mCompareText = Utils.sb.ToString();
                }
                else
                    share.mSortParam = 0;
            }

            sort(shares);
        }

        public void sortLowestBeta(xVector shares)
        {
            String format = "{0:F1}";
            mContext.mShareManager.loadCompanyInfo();
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                stCompanyInfo inf = getCompanyInfo(share.mID);
                if (inf != null)
                {
                    share.mSortParam = inf.Beta;
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat(format, (float)(inf.Beta / 100));
                    share.mCompareText = Utils.sb.ToString();
                }
                else
                    share.mSortParam = 0;
            }

            sort(shares);
        }

        public void sortTodayBiggestVolume(xVector shares)
        {
            String format = "{0:F1}";
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                stPriceboardState ps = mContext.mPriceboard.getPriceboard(share.mID);
                if (ps != null)
                {
                    share.mSortParam = ps.getTotalVolume();
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat(format, (float)(share.mSortParam));
                    share.mCompareText = Utils.sb.ToString();
                }
                else
                    share.mSortParam = 0;
            }

            sort(shares);
        }
        //  Khoi luong niem yet
        public void sortOnMarketVolume(xVector shares)
        {
            mContext.mShareManager.loadCompanyInfo();
            String format = "{0:F1}";

            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                stCompanyInfo inf = getCompanyInfo(share.mID);
                if (inf != null)
                {
                    share.mSortParam = inf.volume;
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat(format, (double)(inf.volume/1000));
                    share.mCompareText = Utils.sb.ToString();
                }
            }

            sort(shares);
        }

        public void sortCheapestPrice(xVector shares)
        {
            String format = "{0:F2}";
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                stPriceboardState ps = mContext.mPriceboard.getPriceboard(share.mID);
                if (ps != null)
                {
                    share.mSortParam = (ps.getCurrentPrice());
                    if (share.mSortParam == 0)
                    {
                        share.mSortParam = (ps.getRef());
                    }

                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat(format, (float)(ps.getCurrentPrice() - ps.getRef()));
                    share.mCompareText = Utils.sb.ToString();
                }
                else
                    share.mSortParam = 0;
            }

            sort(shares);
        }

        public void sortOnMarketValue(xVector shares)
        {
            mContext.mShareManager.loadCompanyInfo();
            String format = "{0}";
            int price = 0;
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                if (share.isIndex())
                    continue;
                stCompanyInfo inf = getCompanyInfo(share.mID);
                Utils.sb.Length = 0;
                if (inf != null)
                {
                    share.mSortParam = inf.vontt;

                    Utils.sb.AppendFormat(format, inf.vontt);
                }
                else
                    share.mSortParam = 0;
                share.mCompareText = Utils.sb.ToString();
            }

            sort(shares);
        }

        public void sortABC(xVector shares)
        {
            if (shares.size() > 0)
            {
                List<object> list = shares.getInternalList();
                ShareComparer cmp = new ShareComparer(ShareComparer.SORT_ABC);
                list.Sort(cmp);
            }
        }

        public void sort(xVector shares)
        {
            if (shares.size() > 0)
            {
                List<object> list = shares.getInternalList();
                ShareComparer cmp = new ShareComparer(ShareComparer.SORT_NORMAL);
                list.Sort(cmp);
            }
        }

        public void sortAlphabet(xVector shares)
        {
            String format = "{0:F1}";
            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                if (!share.isIndex())
                {
                    stPriceboardState ps = mContext.mPriceboard.getPriceboard(share.mID);
                    Utils.sb.Length = 0;
                    Utils.sb.AppendFormat(format, (float)((ps.getCurrentPrice() - ps.getRef()) / 1000));
                    share.mCompareText = Utils.sb.ToString();
                }
            }

            sortABC(shares);
        }

        public void sortHeiken(xVector shares)
        {
             xVector greenHeikens = new xVector(200);
            int maxLength = 100;
            int daybacks = 15;
            float[] heiken = new float[200];

            String format = "{0:F1}";

            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                share.mCompareText = null;
                share.loadShareFromCommonData(true);
                int max = share.calcHeiken(maxLength, heiken);

                if (max > daybacks)
                {
                    int e = max - 1;
                    int b = max - daybacks;

                    if (heiken[e] > 0)
                    {
                        for (int j = e-1; j >= b; j--)
                        {
                            if (heiken[j] < 0)
                            {
                                greenHeikens.addElement(share);
                                share.mSortParam = j;
                                break;
                            }
                        }
                    }
                }
                if (!share.isIndex())
                {
                    /*
                    if (share.mCode.CompareTo("MKV") == 0)
                    {
                    int t = 0;
                    }
                     */
                    stPriceboardState ps = mContext.mPriceboard.getPriceboard(share.mID);
                    Utils.sb.Length = 0;
                    if (ps.getCurrentPrice() > 0 && ps.getRef() > 0)
                    {
                        Utils.sb.AppendFormat(format, (float)((ps.getCurrentPrice() - ps.getRef()) / 1000));
                    }
                    else
                    {

                    }
                    share.mCompareText = Utils.sb.ToString();
                }
            }

            sort(greenHeikens);

            shares.removeAllElements();
            for (int i = 0; i < greenHeikens.size(); i++)
            {
                Share share = (Share)greenHeikens.elementAt(i);
                shares.addElement(share);
            }
        }

        public void sortVolDotbien(xVector shares)
        {
            String format = "{0:F1}";

            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                loadShareFromCommon(share, 50, true);

                int ave3 = share.getAveVolumeInDays(3);
                int ave50 = share.getAveVolumeInDays(50);
                if (ave50 > 0)
                {
                    share.mSortParam = ave3*100/ave50;
                }
                else
                {
                    share.mSortParam = 0;
                }
            }

            sort(shares);
        }

        public void sortTichluy(int dd, int days, xVector shares)
        {
            String format = "{0:F1}";
            stCandle c = new stCandle();

            xVector tichluy = new xVector();

            for (int i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                loadShareFromCommon(share, 50, true);

                float lowest = 100000;
                float highest = 0;
                int e = share.getCandleCnt()-1;
                int b = share.getCandleCnt()-days;
                if (b < 0) b = 0;

                for (int j = b; j <= e; j++)
                {
                    share.getCandle(j, c);
                    if (c.highest > highest){
                        highest = c.highest;
                    }
                    if (c.lowest < lowest){
                        lowest = c.lowest;
                    }
                }

                if (lowest > 0 && highest > 0){
                    float percent = (highest-lowest)*100/lowest;
                    if (percent < dd){
                        tichluy.addElement(share);
                        share.mSortParam = 100-percent;
                    }
                }
            }

            sort(tichluy);
            shares.removeAllElements();
            for (int i = 0; i < tichluy.size(); i++)
            {
                shares.addElement(tichluy.elementAt(i));
            }
        }

        public void sortMacdCutSignal(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                loadShareFromCommon(share, 60, true);
                if (share.isMACDBuySignal())
                {
                    v.addElement(share);
                    share.mCompareText = null;
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortMacdConvergency(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                loadShareFromCommon(share, 60, true);
                if (share.isMACDConvergency())
                {
                    v.addElement(share);
                    share.mCompareText = null;
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortRSICutSMA(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                loadShareFromCommon(share, 60, true);
                if (share.isRSICutSMA())
                {
                    v.addElement(share);
                    share.mCompareText = null;
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortMFICutSMA(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 60, true);
                if (share.isMFICutSMA())
                {
                    v.addElement(share);
                    share.mCompareText = null;
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortADX_cut_DMIs(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                loadShareFromCommon(share, 100, true);
                share.calcADX();
                //if (share.mCode == "DVP")
                //{
                    //int k = 0;
                //}
                if (share.isPDICutMDI(true, 15))
                {
                    v.addElement(share);
                    share.mCompareText = null;
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortROCCutSMA(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                loadShareFromCommon(share, 60, true);

                if (share.isROCCutSMA())
                {
                    v.addElement(share);
                    share.mCompareText = null;
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortADLCutSMA(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);
                loadShareFromCommon(share, 60, true);
                if (share.isADLCutSMA())
                {
                    v.addElement(share);
                    share.mCompareText = null;
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortBullishNVI(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            int period = (int)(mContext.mOptNVI_EMA[1] > mContext.mOptNVI_EMA[0]?mContext.mOptNVI_EMA[1]:mContext.mOptNVI_EMA[0]);
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, period + 10, true);
                share.calcNVI();
                if (share.isNVIBullish())
                {
                    v.addElement(share);
                    share.mCompareText = null;
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortStochastic(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 60, true);
                if (share.getCandleCount() > 10)
                {
                    share.calcStochastic();
                    if (share.isStochasticKCutDAbove(15))
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }
            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortPriceEnterKumo(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 70, true);
                if (share.getCandleCount() > 50)
                {
                    share.calcIchimoku();
                    if (share.priceEnterKumoUp())
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }
            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortPriceAboveKumo(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, (int)(2*mContext.mOptIchimokuTime3), true);
                if (share.getCandleCount() > 50)
                {
                    share.calcIchimoku();
                    if (share.priceUpAboveKumo())
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }
            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortPSARReverseUp(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 70, true);
                if (share.getCandleCount() > 10)
                {
                    share.calcPSAR();
                    if (share.isPSARReverseGreen())
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }
            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortTenkanCutKijunAbove(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 70, true);
                if (share.getCandleCount() > 10)
                {
                    share.calcIchimoku();
                    if (share.isTenkanCutKijunAbove(15))
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }
            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortRSIHigher(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 30, true);
                if (share.getCandleCount() > 10)
                {
                    share.calcRSI(0);
                    if (share.pRSI[share.getCandleCount() - 1] <= 31)
                    {
                        v.addElement(share);
                        share.mSortParam = share.pRSI[share.getCandleCount() - 1];
                        share.mCompareText = null;
                    }
                }
            }
            sort(v);
            shares.removeAllElements();
            for (i = v.size()-1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortMFIOversold(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 30, true);
                if (share.getCandleCount() > 10)
                {
                    share.calcMFI(0);
                    if (share.pMFI[share.getCandleCount() - 1] <= 31)
                    {
                        v.addElement(share);
                        share.mSortParam = share.pMFI[share.getCandleCount() - 1];
                        share.mCompareText = null;
                    }
                }
            }
            sort(v);
            shares.removeAllElements();
            for (i = v.size() - 1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortROCOversold(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 30, true);
                if (share.getCandleCount() > 10)
                {
                    share.calcROC(0);
                    if (share.pROC[share.getCandleCount() - 1] <= -10)
                    {
                        v.addElement(share);
                        share.mSortParam = -share.pROC[share.getCandleCount() - 1];
                        share.mCompareText = null;
                    }
                }
            }
            sort(v);
            shares.removeAllElements();
            for (i = v.size() - 1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortCandleBullishEngulfing(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 15, true);
                if (share.getCandleCount() > 10)
                {
                    if (share.isCandleBullishEngulfing())
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }

            shares.removeAllElements();
            for (i = v.size() - 1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortCandleBullishPearcing(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 15, true);
                if (share.getCandleCount() > 10)
                {
                    if (share.isCandleBullishPearcing())
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }

            shares.removeAllElements();
            for (i = v.size() - 1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortCandleMorningStar(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 15, true);
                if (share.getCandleCount() > 10)
                {
                    if (share.isCandleMorningStar())
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }

            shares.removeAllElements();
            for (i = v.size() - 1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortCandleHammer(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 10, true);
                if (share.getCandleCount() > 0)
                {
                    if (share.isCandleHammer())
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }

            shares.removeAllElements();
            for (i = v.size() - 1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortCandleHarami(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 10, true);
                if (share.getCandleCount() > 0)
                {
                    if (share.isCandleHarami())
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }

            shares.removeAllElements();
            for (i = v.size() - 1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortBullishDiverRSI(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                //if (share.mCode.CompareTo("HAG") == 0)
                //{
                    //int k = 0;
                //}

                loadShareFromCommon(share, 50, true);
                if (share.getCandleCount() > 10)
                {
                    //if (share.mCode.CompareTo("FPT") == 0)
                    //{
                      //  int k = 0;
                    //}
                    //share.mCandleCnt -= 3;
                    share.calcRSI(0);
                    if (share.isBullishDiverRSI())
                    {
                        v.addElement(share);
                        share.mSortParam = share.pMFI[share.getCandleCount() - 1];
                        share.mCompareText = null;
                    }
                }
            }
            sort(v);
            shares.removeAllElements();
            for (i = v.size() - 1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortBullishDiverROC(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 50, true);
                if (share.getCandleCount() > 10)
                {
                    //if (share.mCode.CompareTo("FPT") == 0)
                    //{
                    //  int k = 0;
                    //}
                    //share.mCandleCnt -= 3;
                    share.calcROC(0);
                    if (share.isBullishDiverROC())
                    {
                        v.addElement(share);
                        share.mSortParam = share.pMFI[share.getCandleCount() - 1];
                        share.mCompareText = null;
                    }
                }
            }
            sort(v);
            shares.removeAllElements();
            for (i = v.size() - 1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortBullishDiverMFI(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 50, true);
                if (share.getCandleCount() > 10)
                {
                    //if (share.mCode.CompareTo("DIG") == 0)
                    //{
                      //  int k = 0;
                    //}
                    share.calcMFI(0);
                    if (share.isBullishDiverMFI())
                    {
                        v.addElement(share);
                        share.mSortParam = share.pMFI[share.getCandleCount() - 1];
                        share.mCompareText = null;
                    }
                }
            }
            sort(v);
            shares.removeAllElements();
            for (i = v.size() - 1; i >= 0; i--)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortAccumulation(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 20, true);
                if (share.isAccumulation())
                {
                    v.addElement(share);
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortSMA1CutSMA2(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 50, true);
                if (share.isSMAInter())
                {
                    v.addElement(share);
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortSMACut(xVector shares, int period)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, period + 50, true);
                if (share.isSMACutPrice(period))
                {
                    v.addElement(share);
                }
            }

            shares.removeAllElements();
            for (i = 0; i < v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void sortBullishVolume(xVector shares)
        {
            xVector v = new xVector(shares.size());
            int i = 0;
            for (i = 0; i < shares.size(); i++)
            {
                Share share = (Share)shares.elementAt(i);

                loadShareFromCommon(share, 50, true);
                if (share.getCandleCount() > 10)
                {
                    //if (share.mCode.CompareTo("ASM") == 0)
                    {
                      int k = 0;
                    }
                    //share.mCandleCnt -= 3;
                    share.calcVolumeUpValue();
                    if (share.mSortParam > 0)
                    {
                        v.addElement(share);
                        share.mCompareText = null;
                    }
                }
            }
            sort(v);
            shares.removeAllElements();
            for (i = 0; i< v.size(); i++)
            {
                shares.addElement(v.elementAt(i));
            }
        }

        public void calcIndexOfGroup(stShareGroup g)
        {
            //--------------------------------------------------
            Share _index = new Share(Share.MAX_CANDLE_CHART_COUNT);
            Share _share = new Share(Share.MAX_CANDLE_CHART_COUNT);

            _index.setCode(g.getName(), 1);
            String rootCode = null;
            int startDate = 0;
            int candleCnt = 0;

            int tmp = g.getTotal() > 5 ? 5 : g.getTotal();
            ShareManager mShareManager = Context.getInstance().mShareManager;
            for (int i = 0; i < tmp; i++)
            {
                string code = g.getCodeAt(i);
                _share = mShareManager.getShare(code);
                if (_share == null)
                {
                    Utils.trace("Share is null:" + code);
                    continue;
                }
                _share.loadShareFromCommonData(true);

                if (_share.getCandleCount() > 0)
                {
                    int d = _share.getDate(0);
                    if (startDate == 0)
                    {
                        startDate = d;
                        rootCode = code;
                        candleCnt = _share.getCandleCount();
                    }
                    if (d < startDate)
                    {
                        startDate = d;
                        rootCode = code;
                        candleCnt = _share.getCandleCount();
                    }
                }
            }

            if (candleCnt == 0)
            {
                return;
            }
            //rootCode = "ACB";
            //  root code
            double marketCap = 0;
            _share = getShare(rootCode);// .setCode(rootCode, 1);
            _share.removeAllCandles();
            _share.loadShareFromCommonData(true);

            stCandle candle = new stCandle();
            stCandle c0 = new stCandle();
            stCandle c = new stCandle();
            stCandle c1 = new stCandle();

            for (int i = 0; i < _share.getCandleCount(); i++)
            {
                c = _share.getCandle(i, c);
                if (i == 0 && c.close > 0)
                {
                    c0 = _share.getCandle(i, c0);
                    stCompanyInfo inf = mShareManager.getCompanyInfo(_share.getShareID());
                    marketCap = inf.volume * c.close;
                }

                if (c0.close == 0)
                {
                    continue;
                }

                candle.close = (c.close * 100) / c0.close;
                candle.open = (c.open * 100) / c0.close;
                candle.highest = (c.highest * 100) / c0.close;
                candle.lowest = (c.lowest * 100) / c0.close;
                candle.volume = c.volume;
                candle.date = c.date;

                _index.addMoreCandle(candle.open, candle.close, candle.close, candle.highest, candle.lowest, candle.volume, candle.date);
            }
            stCandle lastIndexCandle;
            //------------------------------
            for (int i = 0; i < g.getTotal(); i++)
            {
                string code = g.getCodeAt(i);

                if (code.CompareTo(rootCode) == 0)
                {
                    continue;
                }

                _share = mShareManager.getShare(code);
                //_share.setCode(code);
                if (_share == null)
                {
                    continue;
                }
                _share.loadShareFromCommonData(true);

                double shareCap = 0;
                double r = 1.0f;
                double newMarketCap = 0;

                if (_share.getCandleCount() == 1)
                {
                    continue;
                }
                double Mo = marketCap;
                double ri = 0;
                bool isFirst = true;

                c0.close = 0;
                //==========================
                int lastCandleFound = -1;
                int candleFound = -1;
                for (int j = 0; j < _index.getCandleCnt(); j++)
                {
                    stCandle ci = _index.getCandle(j, c1);
                    newMarketCap = marketCap;
                    bool replaced = false;
                    for (int t = 0; t < _share.getCandleCnt(); t++)
                    {
                        c = _share.getCandle(t, c);
                        if (t == 0 || c0.close == 0)
                        {
                            c0 = _share.getCandle(t, c0);
                            stCompanyInfo inf = mShareManager.getCompanyInfo(_share.getShareID());
                            shareCap = inf.volume * c.close;
                            newMarketCap = marketCap + shareCap;
                            r = shareCap / newMarketCap;
                            ri = marketCap / newMarketCap;
                            replaced = true;
                            continue;
                        }

                        if (c0.close == 0)
                        {
                            replaced = true;
                            break;
                        }

                        int check = (2018 << 16) | (3 << 8) | 1;
                        if (ci.date == check)
                        {
                            check = check;
                        }

                        if (c.date == ci.date)
                        {
                            candleFound = t;
                        }
                    }

                    if (candleFound == -1)
                    {
                        if (lastCandleFound > 0){
                            candleFound = lastCandleFound;
                        }
                    }

                    if (candleFound > 0)
                    {
                        lastCandleFound = candleFound;
                        c = _share.getCandle(candleFound, c);
                        if (c.close > 0)
                        {
                            float open, close, hi, lo;
                            int vol, date;
                            close = (c.close * 100 / c0.close);
                            open = (c.open * 100 / c0.close);
                            hi = (c.highest * 100 / c0.close);
                            lo = (c.lowest * 100 / c0.close);

                            vol = c.volume;
                            date = c.date;
                            //-------------------------
                            ci.volume += vol;
                            ci.close = (float)(ci.close * ri + close * r);
                            ci.open = (float)(ci.open * ri + open * r);
                            ci.highest = (float)(ci.highest * ri + hi * r);
                            ci.lowest = (float)(ci.lowest * ri + lo * r);

                            _index.replaceCandle(j, ci);
                            replaced = true;
                        }
                    }
                }
                /*




                for (int j = 0; j < _share.getCandleCnt(); j++)
                {
                    c = _share.getCandle(j, c);
                    stCompanyInfo inf = null;
                    if (j == 0 || c0.close == 0)
                    {
                        c0 = _share.getCandle(j, c0);
                        inf = mShareManager.getCompanyInfo(_share.getShareID());
                        shareCap = inf.volume * c.close;
                        newMarketCap = marketCap + shareCap;
                        r = shareCap / newMarketCap;

                        continue;
                    }
                    if (c0.close == 0)
                    {
                        continue;
                    }


                    //------------------

                    float open, close, hi, lo;
                    int vol, date;
                    close = (c.close * 100 / c0.close);
                    open = (c.open * 100 / c0.close);
                    hi = (c.highest * 100 / c0.close);
                    lo = (c.lowest * 100 / c0.close);

                    vol = c.volume;
                    date = c.date;

                    int xxx = (2018<<16) | (1<<8) | 22;
                    if (xxx == date)
                    {
                        xxx++;
                    }

                    double r2 = marketCap / newMarketCap;

                    //  seek to
                    bool replaced = false;
                    for (int t = 0; t < _index.getCandleCnt(); t++)
                    {
                        stCandle ci = _index.getCandle(t, c1);

                        if (ci.date == c.date)
                        {
                            //  marketCap ofo index = (index*marketCap0)/100
                            //double Mi = (ci.close*Mo) + close*shareCap;
                            //float newClose = (ci.close*Mo + close*shareCap)/newMarketCap;

                            ci.volume += vol;
                            ci.close = (float)(ci.close * r2 + close * r);
                            ci.open = (float)(ci.open * r2 + open * r);
                            ci.highest = (float)(ci.highest * r2 + hi * r);
                            ci.lowest = (float)(ci.lowest * r2 + lo * r);

                            _index.replaceCandle(t, ci);
                            replaced = true;

                            break;
                        }
                    }

                    if (!replaced)
                    {
                    }
                }
                 */
                //
                marketCap = newMarketCap;
            }

            _index.saveShare();

            //return _index;

        }

        public void calcIndexOfGroup2(stShareGroup g)
        {
            //--------------------------------------------------
            Share _index = new Share(Share.MAX_CANDLE_CHART_COUNT);
            Share _share = new Share(Share.MAX_CANDLE_CHART_COUNT);

            _index.setCode(g.getName(), 1);
            String rootCode = null;
            int startDate = 0;
            int candleCnt = 0;

            int tmp = g.getTotal() > 5 ? 5 : g.getTotal();
            ShareManager mShareManager = Context.getInstance().mShareManager;
            for (int i = 0; i < tmp; i++)
            {
                string code = g.getCodeAt(i);
                _share = mShareManager.getShare(code);
                if (_share == null)
                {
                    Utils.trace("Share is null:" + code);
                    continue;
                }
                _share.loadShareFromCommonData(true);

                if (_share.getCandleCount() > 0)
                {
                    int d = _share.getDate(0);
                    if (startDate == 0)
                    {
                        startDate = d;
                        rootCode = code;
                        candleCnt = _share.getCandleCount();
                    }
                    if (d < startDate)
                    {
                        startDate = d;
                        rootCode = code;
                        candleCnt = _share.getCandleCount();
                    }
                }
            }

            if (candleCnt == 0)
            {
                return;
            }

            //  root code
            double marketCap = 0;
            _share.setCode(rootCode, 1);
            _share.removeAllCandles();
            _share.loadShareFromCommonData(true);

            stCandle candle = new stCandle();
            stCandle c0 = new stCandle();
            stCandle c = new stCandle();
            stCandle c1 = new stCandle();

            for (int i = 0; i < _share.getCandleCount(); i++)
            {
                c = _share.getCandle(i, c);
                if (i == 0 && c.close > 0)
                {
                    c0 = _share.getCandle(i, c0);
                    stCompanyInfo inf = mShareManager.getCompanyInfo(_share.getShareID());
                    marketCap = inf.volume * c.close;
                }

                if (c0.close == 0)
                {
                    continue;
                }

                candle.close = (c.close * 100) / c0.close;
                candle.open = (c.open * 100) / c0.close;
                candle.highest = (c.highest * 100) / c0.close;
                candle.lowest = (c.lowest * 100) / c0.close;
                candle.volume = c.volume;
                candle.date = c.date;

                _index.addMoreCandle(candle.open, candle.close, candle.close, candle.highest, candle.lowest, candle.volume, candle.date);
            }
            //------------------------------
            for (int i = 0; i < g.getTotal(); i++)
            {
                string code = g.getCodeAt(i);

                if (code.CompareTo(rootCode) == 0)
                {
                    continue;
                }

                _share = mShareManager.getShare(code);
                //_share.setCode(code);
                if (_share == null)
                {
                    continue;
                }
                _share.loadShareFromCommonData(true);

                double shareCap = 0;
                double r = 1.0f;
                double newMarketCap = 0;

                if (_share.getCandleCount() == 1)
                {
                    continue;
                }
                double Mo = marketCap;
                //==========================
                for (int j = 0; j < _share.getCandleCnt(); j++)
                {
                    c = _share.getCandle(j, c);
                    stCompanyInfo inf = null;
                    if (j == 0 || c0.close == 0)
                    {
                        c0 = _share.getCandle(j, c0);
                        inf = mShareManager.getCompanyInfo(_share.getShareID());
                        shareCap = inf.volume * c.close;
                        newMarketCap = marketCap + shareCap;
                        r = shareCap / newMarketCap;

                        continue;
                    }
                    if (c0.close == 0)
                    {
                        continue;
                    }
                    //------------------

                    float open, close, hi, lo;
                    int vol, date;
                    close = (c.close * 100 / c0.close);
                    open = (c.open * 100 / c0.close);
                    hi = (c.highest * 100 / c0.close);
                    lo = (c.lowest * 100 / c0.close);

                    vol = c.volume;
                    date = c.date;

                    double r2 = marketCap / newMarketCap;

                    //  seek to
                    for (int t = 0; t < _index.getCandleCnt(); t++)
                    {
                        stCandle ci = _index.getCandle(t, c1);

                        if (ci.date == c.date)
                        {
                            //  marketCap ofo index = (index*marketCap0)/100
                            //double Mi = (ci.close*Mo) + close*shareCap;
                            //float newClose = (ci.close*Mo + close*shareCap)/newMarketCap;

                            ci.volume += vol;
                            ci.close = (float)(ci.close * r2 + close * r);
                            ci.open = (float)(ci.open * r2 + open * r);
                            ci.highest = (float)(ci.highest * r2 + hi * r);
                            ci.lowest = (float)(ci.lowest * r2 + lo * r);

                            break;
                        }
                    }
                }
                //
                marketCap = newMarketCap;
            }

            _index.saveShare();

            //return _index;

        }
    }
}
