/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Drawing;
using System.IO;
using System.IO.Compression;

using stock123.app.data;
using xlib.framework;
using xlib.ui;
using xlib.utils;

/**
 *
 * @author ThuyPham
 */
namespace stock123.app.net
{
    public delegate void OnDoneDelegate(NetProtocol sender, bool ok);
    public class NetProtocol : xIEventListener
    {
        public OnDoneDelegate onDoneDelegate;
        public OnEventDelegate onEventDelegate;

        public const uint SIGNAL = 0xaabbccdd;
        public const int SHARE_CODE_LENGTH = 8;
        public const int DEVICE_ID = 8;
        public const int NETMSG_BASE = 5000;
        public const int NETMSG_GET_1SHARE_DATA = 8001;// + NETMSG_BASE;
        public const int NETMSG_GET_SHARES_DATA = 8002;
        public const int NETMSG_GET_COMPANY_INFO_OLD = 100/*3*/ + NETMSG_BASE;
        public const int NETMSG_GET_COMPANY_INFO = 5102;


        //public const int NETMSG_GET_COMPANY_INFO2 = 100 + NETMSG_BASE;
        public const int NETMSG_LOGIN = 102;
        public const int NETMSG_TELCO_SMS = 150;
        public const int NETMSG_LOGIN_NEW = 110;

        public const int NETMSG_GET_SHARE_GROUP = 104;
        public const int NETMSG_RESET_PASSWORD = 107;
        public const int NETMSG_SET_PASSWORD = 108;
        public const int NETMSG_SURVEY_INSTANCE = 151;

        //==============profile=====================
        public const int NETMSG_SAVE_USER_DATA = 105;
        public const int NETMSG_GET_USER_DATA = 106;
        public const int NETMSG_SAVE_USER_DATA2      = 120;
        public const int NETMSG_GET_USER_DATA2       = 121;

        public const int NETMSG_SERVER_IP = 203;
        public const int NETMSG_APP_FEEDBACK = 204;
        public const int NETMSG_PRICEBOARD_REF = 300;

        public const int NETMSG_GENERAL_MESSAGE = 5900;
        //  sub message (5900)
        public const int SUBMSG_DATA_CHART_30m       = 1;
        public const int SUBMSG_DATABASE_30m_450 = 2;

        public const int NETMSG_PRICEBOARD_ZERO = 6001;//301; - deprecated
        public const int NETMSG_PRICEBOARD_ZERO2016 = 6003;
        //public const int NETMSG_PRICEBOARD_REALTIME = 6002;//302;      //   realtime priceboard
        public const int NETMSG_PRICEBOARD_OF_GROUP = 6004;
        public const int NETMSG_SHARE_TRADE_DETAIL = 303 + NETMSG_BASE;
        public const int NETMSG_ONLINE_TRADING_TRANSACTIONS = 304;//304 + NETMSG_BASE;       //  realtime chart
        public const int NETMSG_ONLINE_INDEX = 305;
        public const int NETMSG_GET_OPENs = 306;

        public const int NETMSG_GET_NEWS_SUMMARY_J2ME = 470;
        public const int NETMSG_GET_NEWS_DETAIL_J2ME = 471;
        public const int NETMSG_GET_NEWS_CATEGORY = 472;
        public const int NETMSG_GET_SHARE_NEWS = 402;
        public const int NETMSG_GET_SHARE_NEWS_DETAIL = 403;
        //==============================================
        public const int NETMSG_GET_SHARE_IDs_OLD = 5171;//71 + NETMSG_BASE;
        public const int NETMSG_GET_SHARE_IDs = 5173;//NETMSG_GET_SHARE_IDs_2020
        
        public const int NETMSG_GET_INDICES_IDs = 370;
        public const int NETMSG_GET_ALL_INDICES_DATA = 376;
        public const int NSGMSG_ONLINE_REALTIME_J2ME = 371;
        //==============================================
        public const int NETMSG_LATEST_CLIENT_VERSION = 500;
        public const int NETMSG_RESET_CLIENT_DATA = 501;
        //==============================================
        //public const int NETMSG_GET_1COMPANY_FINANCE_INFO = 600;
        public const int NETMSG_FILTER = 601;
        //==================Chat========================
        public const int NETMSG_GET_CHAT = 650;
        public const int NETMSG_POST_CHAT = 651;
        //==================Other indexes========================
        public const int NETMSG_GET_OTHER_INDEXES = 700;
        public const int NETMSG_GET_DOMESTIC_GOLD = 701;
        public const int NETMSG_GET_EXCHANGE_RATE = 702;
        //==============================================
        public const int NETMSG_SET_SHARE_PROFILE = 900;
        public const int NETMSG_GET_SHARES_PROFILE = 901;
        public const int NETMSG_GET_REMOVE_SHARE_PROFILE = 902;
        //========================================================
        public const int NETMSG_GET_EBOOK_CATEGORY_J2ME = 1000;
        public const int NETMSG_GET_EBOOK_DETAIL_J2ME = 1001;
        public const int NETMSG_GET_EBOOK_IMAGE_J2ME = 1002;

        public const int NETMSG_JSON_MESSAGE = 10000;
        //===========msg moi==================
        //==============================================
        public const int NETMSG_SERVER_NOTIFICATION = 200;
        public const int SN_SERVER_IS_MAINTAINING = 1;
        public const int SN_DATABASE_ERROR = 2;
        public const int SN_CLIENT_NOT_SUPPORT = 3;
        public const int SN_NORMAL_WARNING = 4;
        public const int SN_LOGIN_REQUIRED = 5;
        public const int SN_SESSION_TIMEOUT = 6;
        public const int SN_ACCOUNT_EXPIRED = 7;
        public const int SN_ACCOUNT_GOING_EXPIRE = 10;
        public const int SN_RESET_DATABASE = 9;
        //==============================================


        //==============================================
        public int mTotalDataSize = 0;
        xIEventListener mListener;
        //=========================================================
        Context mContext;
        xDataOutput mRequestPackage;
        public xDataOutput mRequestMessage;
        int mGzip;
        xDataOutput tmpOut = new xDataOutput(1024);
        xDataInput tmpIn = new xDataInput(0);
        xDataInput msgIn = new xDataInput(30 * 1024);
        int mLastErrorCode;
        String mLastError;
        int mServerNotifyicationType;
        String mServerNotificationMsg;
        String mDevice;
        xHttp mHttp;
        String mURL;
        public bool ignoreSessionId = false;

        public NetProtocol()
        {
            mRequestPackage = new xDataOutput(2048);
            mRequestMessage = new xDataOutput(2048);
            mContext = Context.getInstance();
        }

        public void resetServerNotification()
        {
            mServerNotificationMsg = "";
            mServerNotifyicationType = 0;
        }

        public int getServerNotificationCode()
        {
            return mServerNotifyicationType;
        }

        public String getServerNotificationMsg()
        {
            return mServerNotificationMsg;
        }

        public void resetRequest()
        {
            mRequestPackage.reset();
            mRequestMessage.reset();
            mGzip = 0;

            mLastErrorCode = 0;
            mLastError = "";
        }

        public void flushRequest()
        {
            if (mRequestMessage.size() == 0)
            {
                onEvent(this, xBaseControl.EVT_NET_DONE, 0, null);

                return;
            }
            //	package header
            mRequestPackage.reset();
            mRequestPackage.writeInt(SIGNAL);

            mRequestPackage.writeInt(DEVICE_ID);

            mRequestPackage.writeInt(mGzip);

            String s = mContext.getSession();
            mRequestPackage.writeUTF(s);

            mRequestPackage.writeUTF("vnChart-PC");    //  serialPhone
            mRequestPackage.writeUTF(".Net 20");        //  firmware
            mRequestPackage.writeInt(11);			//	protocol version of 2019
            //  old: 1: chi support 2 floors

            //mRequestPackage.writeInt(0x00000300);
            //mRequestPackage.writeUTF("test device id");

            int dataSize = mRequestMessage.size();
            mRequestPackage.writeInt(dataSize);
            mRequestPackage.write(mRequestMessage.getBytes(), 0, mRequestMessage.size());

            //===============================
            xDataInput di = new xDataInput(mRequestMessage.getBytes(), 0, mRequestMessage.size(), true);
            while (di.available() > 0)
            {
                int size = di.readInt();
                int msg = di.readInt();

                Utils.trace("MSG= " + msg);
                di.skip(size - 4);
            }

            //==========================
            /*
            xDataInput di = new xDataInput(mRequestPackage.getBytes(), 0, mRequestPackage.size(), true);
            int a = di.readInt();   //  signal
            a = di.readInt();   //  device-id
            a = di.readInt();   //  gz
            String session = di.readUTF();
            String serial = di.readUTF();
            String os = di.readUTF();
            int pro = di.readInt();
            */
            //==========================

            mHttp = new xHttp(this);
            mHttp.post(mURL, mRequestPackage);
        }

        public xDataOutput getRequestData()
        {
            return mRequestPackage;
        }

        public int getRequestSize()
        {
            return mRequestPackage.size();
        }

        string getDeviceID()
        {
            if (mContext.mDeviceID == null || mContext.mDeviceID.Length == 0)
            {
                Random r = new Random();
                long ms = Utils.currentTimeMillis();
                double rl = r.NextDouble();
                String id = String.Format("PC_{0:F10}_{1}", rl, ms);
                mContext.mDeviceID = id;
            }
            /*
            string cpuInfo = string.Empty;
            System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_Processor");
            System.Management.ManagementObjectCollection moc = mc.GetInstances();

            foreach (System.Management.ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    try
                    {
                        cpuInfo = mo.Properties["ProcessorID"].Value.ToString();
                        cpuInfo = "PC_" + cpuInfo;
                        break;
                    }
                    catch (Exception e)
                    {
                        cpuInfo = "PC_" + Utils.currentTimeMillis();
                    }
                    break;
                }
            }
             * */
            return mContext.mDeviceID;
        }

        public void requestLogin(String email, String pass, int createAcc)
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_LOGIN);

            tmpOut.writeUTF(email);
            //  3fa2987d4a42f6dcf1fa12e03a2fda52
            if (false)//email.CompareTo("thuyps@gmail.com") == 0 && pass.CompareTo("1") == 0)
            {
                tmpOut.writeUTF("3fa2987d4a42f6dcf1fa12e03a2fda52");
            }
            else
            {
                String md5 = Utils.MD5String(pass);
                tmpOut.writeUTF(md5);
            }
            tmpOut.writeByte(createAcc);

            tmpOut.writeInt(0x00000300);

            string deviceid = mContext.mDeviceID;
            if (deviceid == null || deviceid.Length == 0)
            {
                deviceid = getDeviceID();
                mContext.mDeviceID = deviceid;
                mContext.saveProfile();
            }

            if (deviceid == null || deviceid.Length == 0)
                deviceid = "unidentify_id";
            tmpOut.writeUTF(deviceid);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestLoginNew(String email, String pass, int createAcc)
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_LOGIN_NEW);

            tmpOut.writeUTF(email);
            //  3fa2987d4a42f6dcf1fa12e03a2fda52
            if (email.CompareTo("thuyps@gmail.com") == 0 && pass.CompareTo("1") == 0)
            {
                tmpOut.writeUTF("3fa2987d4a42f6dcf1fa12e03a2fda52");
            }
            else
            {
                String md5 = Utils.MD5String(pass);
                tmpOut.writeUTF(md5);
            }

            //----------------------
            string deviceid = mContext.mDeviceID;
            if (deviceid == null || deviceid.Length == 0)
            {
                deviceid = getDeviceID();
                mContext.mDeviceID = deviceid;
                mContext.saveProfile();
            }

            if (deviceid == null || deviceid.Length == 0)
                deviceid = "unidentify_id";
            //----------------------
            tmpOut.writeUTF(deviceid);
            tmpOut.writeByte(createAcc);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }
        /*
        public void requestIndices(int last_date) {
        String[] codes = {"^VNIndex", "^HASTC"};
        for (int i = 0; i < 2; i++) {
        tmpOut.reset();
        tmpOut.writeInt(NETMSG_GET_1SHARE_DATA);

        writeCode(tmpOut, codes[i]);

        tmpOut.writeInt(last_date);
        mRequestMessage.writeInt(tmpOut.size());
        mRequestMessage.write(tmpOut);
        }
        mGzip = 0;
        }
         */

        public void requestGetCompanyInfo()
        {
            mRequestMessage.writeInt(4);
            mRequestMessage.writeInt(NETMSG_GET_COMPANY_INFO);
            mGzip = 0;
        }

        public void requestFilter(int floor, int filterId, int[] options)
        {
            // write to buffer
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_FILTER);
            tmpOut.writeInt(filterId);
            tmpOut.writeInt(floor);
            for (int i = 0; i < 6; i++)
            {
                tmpOut.writeInt(options[i]);
            }

            // write to stream
            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        
        public void requestShareGroup()
        {
            mRequestMessage.writeInt(4);
            mRequestMessage.writeInt(NETMSG_GET_SHARE_GROUP);
        }

        public void requestGetAllShares2(int date_start)
        {
            mRequestMessage.writeInt(12);
            //mRequestMessage.writeInt(NETMSG_GET_ALL_SHARES_DATA2);
            mRequestMessage.writeInt(NETMSG_GET_SHARES_DATA);
            //  for test only
            //date_start = (2019 << 16) | (7 << 8) | 1;
            mRequestMessage.writeInt(date_start);
            mRequestMessage.writeInt(1);    //  option
            mGzip = 1;
        }
        
        public void requestGeneralMessage(int subId, xDataOutput data)
        {
            tmpOut.reset();
    
            tmpOut.writeInt(NETMSG_GENERAL_MESSAGE);
            tmpOut.writeInt(subId);
    
            if (data != null){
                tmpOut.writeInt(data.size());
                tmpOut.write(data.getBytes(), 0, data.size());
            }
            else{
                tmpOut.writeInt(0);
            }
    
            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut.getBytes(), 0, tmpOut.size());
        }


        public void requestAppFeedback(String nick, String content)
        {
            // write to buffer
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_APP_FEEDBACK);
            tmpOut.writeUTF(nick);
            tmpOut.writeUTF(content);

            // write to stream
            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        //  not used
        public void requestPriceboardRef(int floor)
        {
            for (int i = 1; i <= 2; i++)
            {
                if (floor == -1 || i == floor)
                {
                    tmpOut.reset();
                    tmpOut.writeInt(NETMSG_PRICEBOARD_REF);
                    tmpOut.writeInt(i);

                    mRequestMessage.writeInt(tmpOut.size());
                    mRequestMessage.write(tmpOut);
                }
            }
        }

        public void requestPriceboardInitial(int floor, String date)
        {
            if (floor == -1)
            {
                for (int i = 0; i < mContext.mPriceboard.getIndicesCount(); i++)
                {
                    stPriceboardStateIndex pi = mContext.mPriceboard.getPriceboardIndexAt(i);

                    if (pi != null)
                    {
                        tmpOut.reset();
                        tmpOut.writeInt(NETMSG_PRICEBOARD_ZERO2016);
                        tmpOut.writeInt(pi.marketID);		//	floor
                        date = pi.update_time;
                        if (date == null)
                        {
                            tmpOut.writeUTF("");
                        }
                        else
                        {
                            tmpOut.writeUTF(date);
                        }

                        mRequestMessage.writeInt(tmpOut.size());
                        mRequestMessage.write(tmpOut);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 3; i++)
                {
                    if (floor == -1 || i == floor)
                    {
                        tmpOut.reset();
                        tmpOut.writeInt(NETMSG_PRICEBOARD_ZERO2016);
                        tmpOut.writeInt(i);		//	floor
                        if (date == null)
                        {
                            tmpOut.writeUTF("");
                        }
                        else
                        {
                            tmpOut.writeUTF(date);
                        }

                        mRequestMessage.writeInt(tmpOut.size());
                        mRequestMessage.write(tmpOut);
                    }
                }
            }

            mGzip = 0;
        }

        public void requestOnlineIndex(int floor)
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_ONLINE_INDEX);
            tmpOut.writeInt(floor);		//	floor

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);

            //        mRequestMessage.writeInt(8);
            //        mRequestMessage.writeInt(NETMSG_ONLINE_INDEX);
            //        mRequestMessage.writeInt(floor);

            mGzip = 0;
        }

        void writeCode(xDataOutput o, String code)
        {
            int len = code.Length;
            int max = 8;
            if (len > max)
            {
                len = max;
            }
            for (int i = 0; i < len; i++)
            {
                o.writeByte((byte)code[i]);
            }
            int remain = max - len;
            for (int i = 0; i < remain; i++)
            {
                o.writeByte(0);
            }
        }

        public void requestTradeHistory(String code, int floor, int date, int time)
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_ONLINE_TRADING_TRANSACTIONS);
            tmpOut.writeInt(date);
            tmpOut.writeInt(time);
            tmpOut.writeByte(floor);
            writeCode(tmpOut, code);
            //tmpOut.writeShort(shareID);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
            mGzip = 0;
        }

        public void requestShareTradeDetail(int floor, int shareID)
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_SHARE_TRADE_DETAIL);
            tmpOut.writeByte(floor);
            tmpOut.writeShort(shareID);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestGet1ShareData(int codeID, int date_start)
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_GET_1SHARE_DATA);
            tmpOut.writeInt(codeID);

            tmpOut.writeInt(date_start);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);

            mGzip = 1;
        }

        public void requestGetAllShare()
        {
            int lastUpdate = mContext.mShareManager.getLastUpdateAllShare();
            requestGetAllShares2(lastUpdate);
        }

        public void requestGetShareIDs()
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_GET_SHARE_IDs);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);

            mGzip = 0;
        }

        public void requestIndicesIDs()
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_GET_INDICES_IDs);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        
        //  requestAllIndicesData: no process code for this method
        //  calling this method will return a series of #305
        //  Deprecated
        public void requestAllIndicesData()
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_GET_ALL_INDICES_DATA);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestPostChat(String content)
        {
            // build post chat message data
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_POST_CHAT);
            tmpOut.writeUTF(content);
            // write to output stream line
            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestGetChat(int lastId)
        {
            // build post chat message data
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_GET_CHAT);
            tmpOut.writeInt(lastId);
            // write to output stream line
            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestOtherIndexes()
        {
            // build other indexes message data
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_GET_OTHER_INDEXES);
            // write to output stream line
            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestDomesticGold()
        {
            // build other indexes message data
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_GET_DOMESTIC_GOLD);
            // write to output stream line
            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestExchangeRate()
        {
            // build other indexes message data
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_GET_EXCHANGE_RATE);
            // write to output stream line
            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestGetFavorShares(int type)
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_GET_SHARES_PROFILE);
            tmpOut.writeByte(type);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestRemoveFavorQuote(int type, int id)
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_GET_REMOVE_SHARE_PROFILE);
            tmpOut.writeShort(id);
            tmpOut.writeByte(type);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestPostSurvey(int surveyId, String value)
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_SURVEY_INSTANCE);
            tmpOut.writeInt(surveyId);
            tmpOut.writeUTF(value);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestSaveFavorQuoteToServer(int id, int type, int trade_price, int trade_vol, int minPrice, int maxPrice)
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_SET_SHARE_PROFILE);

            tmpOut.writeShort(id);
            tmpOut.writeByte(type);
            tmpOut.writeShort(minPrice);
            tmpOut.writeShort(maxPrice);
            tmpOut.writeShort(trade_price);
            tmpOut.writeInt(trade_vol);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestJSONMessage(string jsonData)
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_JSON_MESSAGE);
            tmpOut.writeUTF(jsonData);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
            mGzip = 1;
        }

        public bool processPackage(byte[] data, int offset, int len)
        {
            mTotalDataSize += len;
            if (len <= 16)
            {
                return false;
            }
            //	int j = 0;

            //===============bind data
            tmpIn.bind(data, offset, len);

            uint sinal = (uint)tmpIn.readInt();
            if (sinal != SIGNAL)
            {
                //	bad data
                return false;
            }

            int device_id = tmpIn.readInt();
            int gzip = tmpIn.readInt();

            String s = tmpIn.readUTFString();
            if (s != null && ignoreSessionId == false)
            {
                mContext.mSessionID = s;
            }

            int raw_data_size = tmpIn.readInt();
            int data_size = tmpIn.readInt();
            Utils.trace("raw data  size= " + data_size);
            if (gzip == 1)
            {
                if (raw_data_size > 0)
                {
                    //byte[] compressed = new byte[data_size];
                    //System.arraycopy(tmpIn.getStream(), tmpIn.getCurrentOffset(), compressed, 0, data_size);
                    byte[] uncompress_data;
                    int uncompressed_len = 0;
                    try
                    {
                        uncompress_data = unzip(tmpIn.getBytes(), tmpIn.getCurrentOffset(), data_size, raw_data_size);
                        uncompressed_len = raw_data_size;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                    tmpIn.bind(uncompress_data, 0, uncompressed_len);
                }
            }

            //======================================
            while (tmpIn.available() > 4)
            {
                int size = tmpIn.readInt();
                if (size > tmpIn.available() + 4)
                {
                    break;
                }
                int msg = tmpIn.readInt();

                //Utils.trace("\n----------msg = " + msg + "  size= " + size);

                processMessage(msg, tmpIn.getBytes(), tmpIn.getCurrentOffset(), size - 4);

                tmpIn.skip(size - 4);
            }

            return true;
        }

        public void processMessage(int msg, byte[] data, int offset, int len)
        {
            switch (msg)
            {
                case NETMSG_LATEST_CLIENT_VERSION:
                    msgIn.bind(data, offset, len);
                    msgIn.readInt();
                    mContext.mLatestClientVersion = msgIn.readInt();
                    mContext.mLatestClientVersionURL = msgIn.readUTF();
                    break;
                case NETMSG_RESET_CLIENT_DATA:
                    mContext.mShouldReloadAllData = true;
                    break;
                case NETMSG_SERVER_NOTIFICATION:
                    processServerNotification(data, offset, len);
                    break;
                /*case NETMSG_SERVER_IP:
                processServerIP(data, offset, len);
                break;*/
                case NETMSG_LOGIN:
                    processLogin(data, offset, len);
                    break;
                case NETMSG_LOGIN_NEW:
                    processNewLogin(data, offset, len);
                    break;
                case NETMSG_GET_SHARE_GROUP:
                    processShareGroup(data, offset, len);
                    break;
                case NETMSG_GET_1SHARE_DATA:
                    processGet1ShareData(data, offset, len);
                    break;
                case NETMSG_GET_SHARES_DATA:
                    //processGetShares(data, offset, len);
                    //break;
                //case NETMSG_GET_ALL_SHARES_DATA2:
                    processGetAllShares2(data, offset, len);
                    break;

                case NETMSG_GET_COMPANY_INFO:
                    processGetCompanyInfo(data, offset, len);
                    break;
                case NETMSG_PRICEBOARD_REF:
                    processPriceboardRef(data, offset, len);
                    break;
                //case NETMSG_PRICEBOARD_ZERO:
                    //processPriceboardZero(data, offset, len);
                    //break;
                case NETMSG_PRICEBOARD_ZERO2016:
                    processPriceboardZero2016(data, offset, len);
                    break;
                case NETMSG_ONLINE_INDEX:
                    processOnlineIndex(data, offset, len);
                    break;

                case NETMSG_ONLINE_TRADING_TRANSACTIONS:
                    processTradeHistory(data, offset, len);
                    break;
                case NETMSG_SHARE_TRADE_DETAIL:
                    processShareTradeDetail(data, offset, len);
                    break;
                case NETMSG_GET_SHARE_IDs:
                    processGetShareIDs(data, offset, len);
                    break;
                case NETMSG_GET_INDICES_IDs:
                    processGetIndicesIDs(data, offset, len);
                    break;
                case NETMSG_GET_ALL_INDICES_DATA:
                    // no processing for this msg, insteads, server sends a series of
                    // NETMSG_ONLINE_INDEX
                    break;
                case NETMSG_RESET_PASSWORD:
                    processResetPassword(data, offset, len);
                    break;
                case NETMSG_SET_PASSWORD:
                    processSetPassword(data, offset, len);
                    break;
                case NETMSG_GET_USER_DATA:
                    processGetUserData(data, offset, len);
                    break;
                case NETMSG_GET_USER_DATA2:
                    processGetUserData2(data, offset, len);
                    break;
                case NETMSG_GET_OPENs:
                    processRequestOpens(data, offset, len);
                    break;
                case NETMSG_PRICEBOARD_OF_GROUP:
                    processPriceboardOfGroup(data, offset, len);
                    break;
                case NETMSG_JSON_MESSAGE:
                    processJSONMessage(data, offset, len);
                    break;
                case NETMSG_GENERAL_MESSAGE:
                    processGeneralMessage(data, offset, len);
                    break;
            }
        }

        public int getLastErrorCode()
        {
            return mLastErrorCode;
        }

        public String getLastError()
        {
            return mLastError;
        }

        void setErrorMsg(String msg)
        {
            mLastError = msg;
        }

        byte[] readCode(byte[] data, int offset)
        {
            byte[] tmp = new byte[SHARE_CODE_LENGTH + 1];
            Utils.memcpy(tmp, data, offset, SHARE_CODE_LENGTH);
            tmp[SHARE_CODE_LENGTH] = 0;
            return tmp;
        }

        String readCodeAsString(byte[] data, int offset)
        {
            StringBuilder s = Utils.sb;
            s.Length = 0;

            int i = 0;
            while (data[offset + i] != 0 && i < Share.SHARE_CODE_LENGTH)
            {
                s.Append((char)data[offset + i]);
                i++;
            }

            return s.ToString();
        }

        void processGet1ShareData(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error_code = msgIn.readInt();
            if (error_code == 1)
            {
                String utf = msgIn.readUTFString();

                setErrorMsg(utf);
                return;
            }

            //byte[] code = readCode(msgIn.getStream(), msgIn.getCurrentOffset());
            //msgIn.skip(SHARE_CODE_LENGTH);


            int shareID = msgIn.readInt();
            Share share = mContext.mShareManager.getShare(shareID);
            share.loadShareFromFile(false);

            if (share != null)
            {
                int floor = msgIn.readByte();
                int candle_cnt = msgIn.readInt();
                if (candle_cnt > 0)
                {
                    Utils.trace("========share CANDLE CNT0: " + share.mCode + "=" + candle_cnt);
                    share.processNetData(candle_cnt, msgIn);
                    Utils.trace("========share CANDLE CNT1: " + share.mCode + "=" + share.getCandleCount());

                    share.saveShare();
                    share.loadShareFromFile(false);

                    share.mModifiedKey = 0;
                    share.clearCalculations();
                }
            }

        }

        void processServerNotification(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            mServerNotifyicationType = msgIn.readInt();

            mServerNotificationMsg = msgIn.readUTFString();

            if (mServerNotifyicationType == SN_RESET_DATABASE)
            {
                mContext.clearAllSavedData();
            }
        }

        void processLogin(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTFString();
                setErrorMsg(s);

                return;
            }

            String session_id = msgIn.readUTFString();
            if (session_id != null)
            {
                //mContext.mProfile.setEmail(mContext.mEmail, mContext.mPassword);
                mContext.mSessionID = session_id;
                mContext.mIsPurchaseAcc = msgIn.readInt() > 0 ? true : false;
                mContext.mDaysLeft = msgIn.readInt();

                //  for the lastest version
                mContext.mLatestClientVersion = msgIn.readInt();
                mContext.mLatestClientVersionURL = msgIn.readUTF();

                //  for test only
                //mContext.mLatestClientVersion = (7 << 8) | 16;
                //mContext.mLatestClientVersionURL = "https://vnchartpc.s3.amazonaws.com/vnchartpc7.14.zip";

                //mContext.mProfile.save();
                //mContext.checkFavorGroupForLoggedInAccount();
            }
        }

        void processNewLogin(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTFString();
                setErrorMsg(s);

                return;
            }

            String session_id = msgIn.readUTFString();
            if (session_id != null)
            {
                //mContext.mProfile.setEmail(mContext.mEmail, mContext.mPassword);
                mContext.mSessionID = session_id;
                mContext.mIsPurchaseAcc = msgIn.readInt() > 0 ? true : false;
                mContext.mDaysLeft = msgIn.readInt();
                mContext.mIsTrialAcc = msgIn.readInt() > 0 ? true : false;
                //mContext.mIsTrialAcc = true;
                int k = 0;
                //mContext.mProfile.save();
                //mContext.checkFavorGroupForLoggedInAccount();
            }
        }

        void processShareGroup(byte[] data, int offset, int len)
        {
            /*
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTF();
                setErrorMsg(s);
                return;
            }

            int cnt = msgIn.readInt();

            if (cnt > 0)
            {
                mContext.clearShareGroup();
            }
            for (int i = 0; i < cnt; i++)
            {
                String gname = msgIn.readUTF();
                int member_cnt = msgIn.readInt();

                stShareGroup g = mContext.getShareGroup(gname);
                g.setGroupType(stShareGroup.ID_GROUP_DEFAULT);

                if (i == 0)
                {
                    //g.setGroupType(stShareGroup.ID_GROUP_GLOBAL);
                }
                else
                {
                    if (gname.CompareTo("Index & Future") == 0)
                    {
                        g.setGroupType(stShareGroup.ID_GROUP_INDICES);
                    }
                }
                int off = msgIn.getCurrentOffset();

                for (int j = 0; j < member_cnt; j++)
                {
                    //xString *code = in.readUTF();
                    off = msgIn.getCurrentOffset();
                    msgIn.skip(SHARE_CODE_LENGTH);

                    String code = readCodeAsString(msgIn.getBytes(), off);

                    g.addCode(code);
                }

                g.sort();
            }

            if (cnt > 0)
            {
                mContext.saveDefinedShareGroup();
                mContext.loadDefinedShareGroup();

                //=====================
                //stShareGroup overview = new stShareGroup();
                //overview.setName("Toàn cảnh");
                //overview.setType(stShareGroup.ID_GROUP_MARKET_OVERVIEW);
                //mContext.mShareGroups.insertElementAt(overview, 0);
            }
             */
        }

        public void processGetAllShares2(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error_code = msgIn.readInt();
            if (error_code == 1)
            {
                String utf = msgIn.readUTF();

                setErrorMsg(utf);

                return;
            }

            int share_cnt = msgIn.readInt();

            //byte[] code = new byte[8];
            int share_id;
            byte[] p = msgIn.getBytes();
            int off = msgIn.getCurrentOffset();

            mContext.mShareManager.prepareCommonShareBuffer(share_cnt);
            mContext.mShareManager.reloadCommonData();

            for (int i = 0; i < share_cnt; i++)
            {
                share_id = Utils.readInt(p, off);
                off += 4;//SHARE_CODE_LENGTH;

                if (share_id == 719)
                {
                    int kk = 0;
                }

                int floor = p[off++];

                int candle_cnt = Utils.readInt(p, off);
                off += 4;

                int size = candle_cnt * 16;// ShareManager.NET_CANDLE_SIZE;	//	size of each candle

                mContext.mShareManager.addShareDataToCommon(share_id, floor, data, off, candle_cnt);
                off += size;
            }
            if (share_cnt > 0) //	has an update
            {
                //if (share_cnt < mContext.mShareManager.getShareCount())
                    //share_cnt = mContext.mShareManager.getShareCount();
                mContext.mShareManager.saveCommonShareData(share_cnt);
                mContext.saveProfile();
            }
        }

        public void processGetCompanyInfo(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error_code = msgIn.readInt();
            if (error_code == 1)
            {
                String utf = msgIn.readUTF();
                //		xString2* msg = xString2::utf8ToUnicode(utf);
                setErrorMsg(utf);
                return;
            }

            if (msgIn.available() > 1000)
            {
                mContext.mShareManager.processCompanyInfo(msgIn);
            }
        }

        public void processPriceboardRef(byte[] data, int offset, int len)
        {
            /*
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1) {
            mLastErrorCode = error;
            String s = msgIn.readUTF();
            setErrorMsg(s);
            return;
            }
            mContext.mPriceboard.setRefPrice(msgIn);
             * 
             */
        }

        /*
        void processPriceboardZero(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTF();
                setErrorMsg(s);
                return;
            }
            else if (error == 2)
            {
                //	nothing to update
                String s = msgIn.readUTF();
                s = "";
            }
            else if (error == 0)
            {
                mContext.mPriceboard.setZeroPriceboard(msgIn);
            }
        }
         */

        void processPriceboardZero2016(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTF();
                setErrorMsg(s);
                return;
            }
            else if (error == 2)
            {
                //	nothing to update
                String s = msgIn.readUTF();
                s = "";
            }
            else if (error == 0)
            {
                mContext.mPriceboard.setZeroPriceboard2016(msgIn);
            }
        }

        public void processOnlineIndex(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTF();
                setErrorMsg(s);

                return;
            }

            if (error == 0)
            {
                mContext.mPriceboard.setOnlineIndexData(msgIn);
            }
        }

        public void processTradeHistory(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTF();
                setErrorMsg(s);

                return;
            }

            int share_id = msgIn.readInt();
            if (share_id == 0)
                return;

            TradeHistory trade = mContext.getTradeHistory(share_id);

            if (trade != null)
            {
                trade.setData(msgIn);

                Share share = trade.convertToShareData(1, null);
                share.saveShare();
            }
        }

        public void processShareTradeDetail(byte[] data, int offset, int len)
        {

        }

        public void processGetShareIDs(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTF();
                setErrorMsg(s);

                return;
            }

            mContext.mShareManager.processShareIDs(msgIn);
        }

        public void processGetIndicesIDs(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTF();
                setErrorMsg(s);

                return;
            }

            mContext.mShareManager.processIndicesIDs(msgIn);
        }

        public void setListener(xIEventListener listener)
        {
            mListener = listener;
        }

        public void onTick()
        {
        }

        public void cancelNetwork()
        {
        }

        public void requestGetUserData()
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_GET_USER_DATA);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }
        public void requestGetUserData2()
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_GET_USER_DATA2);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestGetPriceboard(xVector shareIds)
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_PRICEBOARD_OF_GROUP);
            tmpOut.writeInt(shareIds.size());

            for (int i = 0; i < shareIds.size(); i++)
            {
                Int32 n = (Int32)shareIds.elementAt(i);
                tmpOut.writeInt(n);
            }

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut);
        }

        public void requestSaveUserData(xDataOutput o)
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_SAVE_USER_DATA);
            tmpOut.writeInt(o.size());
            tmpOut.write(o.getBytes(), 0, o.size());

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut.getBytes(), 0, tmpOut.size());
        }

        public void requestSaveUserData2(xDataOutput o)
        {
            tmpOut.reset();

            tmpOut.writeInt(NETMSG_SAVE_USER_DATA2);
            tmpOut.writeInt(o.size());
            tmpOut.write(o.getBytes(), 0, o.size());

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut.getBytes(), 0, tmpOut.size());
        }

        public void onEvent(object sender, int aEvent, int aIntParameter, object aParameter)
        {
            if (aEvent == xBaseControl.EVT_NET_DONE)
            {
                int len = aIntParameter;
                byte[] p = (byte[])aParameter;
                if (len > 0)
                {
                    xDataInput di = new xDataInput(p, 0, len, false);

                    processPackage(di.getBytes(), 0, di.size());
                }
                if (mListener != null)
                {
                    mListener.onEvent(this, aEvent, 0, null);
                }
                if (onEventDelegate != null)
                {
                    onEventDelegate(this, aEvent, 0, null);
                }
                if (onDoneDelegate != null)
                {
                    onDoneDelegate(this, true);
                }
            }
            else if (aEvent == xBaseControl.EVT_NET_ERROR)
            {
                Context.getInstance().deleteSavedServerUrls();

                if (mListener != null)
                {
                    mListener.onEvent(this, aEvent, aIntParameter, aParameter);
                }
                if (onEventDelegate != null)
                {
                    onEventDelegate(this, aEvent, aIntParameter, aParameter);
                }
                if (onDoneDelegate != null)
                {
                    onDoneDelegate(this, false);
                }
            }
            else
            {
                if (mListener != null)
                {
                    mListener.onEvent(this, aEvent, aIntParameter, aParameter);
                }
                if (onEventDelegate != null)
                {
                    onEventDelegate(this, aEvent, aIntParameter, aParameter);
                }
            }
        }

        public void setServerURL(String url)
        {
            mURL = url;
        }

        public void requestResetPassword(String email)
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_RESET_PASSWORD);
            tmpOut.writeUTF(email);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut.getBytes(), 0, tmpOut.size());
        }

        public void requestSetPassword(string newpass)
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_SET_PASSWORD);
            newpass = Utils.MD5String(newpass);

            tmpOut.writeUTF(newpass);

            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut.getBytes(), 0, tmpOut.size());
        }

        byte[] unzip(byte[] compressed, int offset, int len, int raw_size)
        {
            MemoryStream os = new MemoryStream(raw_size);
            MemoryStream zipped = new MemoryStream(compressed, offset, len);
            System.IO.Compression.GZipStream zip = new System.IO.Compression.GZipStream(zipped, CompressionMode.Decompress);

            int count = 0;
            byte[] buf = new byte[10480];
            do
            {
                count = zip.Read(buf, 0, buf.Length);
                os.Write(buf, 0, count);
            } while (count > 0);

            zip.Close();

            return os.ToArray();
        }

        public void processResetPassword(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTF();
                setErrorMsg(s);

                return;
            }
            //  success
            string msg = msgIn.readUTF();

            mServerNotifyicationType = SN_NORMAL_WARNING;
            mServerNotificationMsg = msg;
        }

        public void processSetPassword(byte[] data, int offset, int len)
        {
            msgIn.bind(data, offset, len);

            int error = msgIn.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = msgIn.readUTF();
                setErrorMsg(s);

                return;
            }
            //  success
            string msg = msgIn.readUTF();

            mServerNotifyicationType = SN_NORMAL_WARNING;
            mServerNotificationMsg = msg;
        }

        void processGetUserData(byte[] data, int offset, int len)
        {
            xDataInput di = msgIn;
            di.bind(data, offset, len);

            int error = di.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                string s = di.readUTF();
                setErrorMsg(s);

                return;
            }
            //  success
            //  skip size of data
            int size = di.readInt();
            if (size > 0)
            {
                Context.userDataManager().loadUserDataOldVersion(di);
                //mContext.loadOnlineUserData(di);
            }
            else
            {
                Context.userDataManager().loadUserDataOldVersion(null);
                //mContext.loadOnlineUserData(null);
            }
        }

        void processGetUserData2(byte[] data, int offset, int len)
        {
            xDataInput di = msgIn;
            di.bind(data, offset, len);

            int error = di.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                string s = di.readUTF();
                setErrorMsg(s);

                return;
            }
            //  success
            //  skip size of data
            int size = di.readInt();
            if (size > 0)
            {
                Context.userDataManager().loadUserData(di);
                //mContext.loadOnlineUserData(di);
            }
        }

        void processSaveUserData(byte[] data, int offset, int len)
        {
            xDataInput di = msgIn;
            di.bind(data, offset, len);

            int error = di.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                string s = di.readUTF();
                setErrorMsg(s);

                return;
            }

        }

        public void requestOpens()
        {
            tmpOut.reset();
            tmpOut.writeInt(NETMSG_GET_OPENs);
            mRequestMessage.writeInt(tmpOut.size());
            mRequestMessage.write(tmpOut.getBytes(), 0, tmpOut.size());
        }
        void processRequestOpens(byte[] data, int offset, int len)
        {
            xDataInput di = msgIn;
            di.bind(data, offset, len);

            int error = di.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = di.readUTF();
                setErrorMsg(s);

                return;
            }

            mContext.mPriceboard.updateOpens(di);
        }

        void processPriceboardOfGroup(byte[] data, int offset, int len)
        {
            xDataInput di = msgIn;
            di.bind(data, offset, len);

            int error = di.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = di.readUTF();
                setErrorMsg(s);

                return;
            }
            mContext.mPriceboard.setPriceboardDataOfGroup(di);
        }

        void processJSONMessage(byte[] data, int offset, int len)
        {
            xDataInput di = msgIn;
            di.bind(data, offset, len);

            int error = di.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = di.readUTF();
                setErrorMsg(s);

                return;
            }

            String jsonString = di.readUTF();

            VTDictionary d = new VTDictionary(jsonString);
            JSONHandler.processJsonMessage(d);
        }

        void processGeneralMessage(byte[] data, int offset, int len)
        {
            xDataInput di = msgIn;
            di.bind(data, offset, len);

            int error = di.readInt();
            if (error == 1)
            {
                mLastErrorCode = error;
                String s = di.readUTF();
                setErrorMsg(s);

                return;
            }
            else if (error == 0)
            {
                int subID = di.readInt();

                if (di.available() > 4)
                {
                    int dataSize = di.readInt();

                    if (subID == SUBMSG_DATA_CHART_30m)
                    {
                        String symbol = di.readUTF();

                        Share share = new Share();
                        share.setCode(symbol, 0);
                        share.setDataType(Share.DATATYPE_30m);
                        share.allocMemoryUsingShared(false);

                        int candleCnt = di.readInt();
                        
                        for (int i = 0; i < candleCnt; i++)
                        {
                            int date = di.readInt();
                            float open = di.readFloatJava();
                            float close = di.readFloatJava();
                            float lowest = di.readFloatJava();
                            float highest = di.readFloatJava();

                            int volume = di.readInt();

                            share.addMoreCandle(open, close, open, highest, lowest, volume, date);
                        }

                        share.saveShare();
                    }
                    
                }
            }

        }
    }
}

