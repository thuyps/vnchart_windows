using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Web;

using stock123.app.data;
using xlib.framework;
using xlib.ui;
using xlib.utils;

//using fastJSON;
using Newtonsoft.Json;

namespace stock123.app
{
    public class g_NetProtocol: xIEventListener
    {
        public const int NET_QUERY_NONE = 0;
        public const int NET_QUERY_DOWJONE = 1;
        public const int NET_QUERY_INDICES = 2;
        public const int NET_QUERY_SHARE_DATA = 3;
        public const int NET_QUERY_OFFLINE_DATA = 4;
        public const int NET_QUERY_CURRENCIES = 5;

        xHttp mHttp;
        xIEventListener mListener;
        int mNetJob = NET_QUERY_NONE;
        Context mContext;
        public g_NetProtocol(xIEventListener listener)
        {
            mListener = listener;
            mContext = Context.getInstance();
        }
        //=====================================================
        public void startRequest(string url, string param, string paramValue)
        {
            mHttp = new xHttp(this);
            if (param != null)
            {
                mHttp.addRequest(param, paramValue);
            }
            mHttp.get(url, null);
        }
        void processData(xDataInput di)
        {
        }
        //=====================================================
        public void onEvent(object sender, int aEvent, int aIntParameter, object aParameter)
        {
            if (aEvent == xBaseControl.EVT_NET_DONE)
            {
                int len = aIntParameter;
                byte[] p = (byte[])aParameter;
                if (len > 0)
                {
                    xDataInput di = new xDataInput(p, 0, len, false);
                    if (mNetJob == NET_QUERY_DOWJONE){
                        processDowjonePriceboard(di);
                    }else if (mNetJob == NET_QUERY_INDICES){
                        processPriceboardIndiceData(di);
                    }else if (mNetJob == NET_QUERY_SHARE_DATA){
                        //processPriceboardShareData(di);
                    }else if (mNetJob == NET_QUERY_OFFLINE_DATA){
                        processOfflineShareData(di);
                    }
                }
                if (mListener != null)
                {
                    mListener.onEvent(this, aEvent, 0, null);
                }
            }
            else if (aEvent == xBaseControl.EVT_NET_ERROR)
            {
                if (mListener != null)
                    mListener.onEvent(this, aEvent, aIntParameter, aParameter);
            }
            else
            {
                if (mListener != null)
                    mListener.onEvent(this, aEvent, aIntParameter, aParameter);
            }
        }

        public void cancelNetwork()
        {
            if (mHttp != null)
            {
                mHttp.cancel();
            }
        }

        public void getDowjonePriceboard()
        {
            cancelNetwork();

            //string szURL = "http://finance.google.com/finance/info";//?client=ig&q=INDEXDJX:.DJI";
            string szURL = "http://finance.google.com/finance/info?client=ig&q=INDEXDJX:.DJI";
            //    startNetwork(szURL, "client=ig&q=INDEXDJX:.DJI");
            startRequest(szURL, null, null);//"client", "ig&q=INDEXDJX:.DJI");

            mNetJob = NET_QUERY_DOWJONE;
        }
        public void getPriceboardIndiceData()
        {
            cancelNetwork();

            //sprintf(szURL, "http://uk.old.finance.yahoo.com/d/quotes.csv?s=%s&f=t1sl1c1p2&e=.csv", share_group);
            //	sprintf(szURL, "http://download.finance.yahoo.com/d/quotes.csv?s=%s&f=t1sl1c1p2&e=.csv", share_group);
            //string szURL = "http://download.finance.yahoo.com/d/quotes.csv";
            //String s = HttpUtility.HtmlEncode();
            string szURL = "http://download.finance.yahoo.com/d/quotes.csv?s=GOOG,^STI,^GSPC,^IXIC,^HSI,^N225,^KS11,^TWII,^FTSE,^GDAXI,^FCHI&f=t1sl1c1p2n&e=.csv";

            startRequest(szURL, null, null);

            mNetJob = NET_QUERY_INDICES;
        }
/*
        public void getPriceboardShareData(string share_group)
        {
            cancelNetwork();

            //sprintf(szURL, "http://uk.old.finance.yahoo.com/d/quotes.csv?s=%s&f=sl1c1p2v&e=.csv", share_group);
            string szURL = "http://download.finance.yahoo.com/d/quotes.csv";

            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            sb.AppendFormat("{0}&f=sl1hgc1p2vn&e=.csv", share_group);

            startRequest(szURL, "s", sb.ToString());

            mNetJob = NET_QUERY_SHARE_DATA;
        }
        */
        public void getOfflineShareData(string code, int last_update)
        {
            if (code == null || code.Length == 0)
                return;

            cancelNetwork();

            //	http://ichart.finance.yahoo.com/table.csv?s=AA&d=11&e=31&f=2010&g=d&a=0&b=1&c=2000&ignore=.csv
            string szURL = null;

            if (last_update == Utils.getDateAsInt())
                return;

            int begin_date = last_update;
            if (begin_date == 0 || (Utils.EXTRACT_YEAR(begin_date) < 2000))
                begin_date = (2000 << 16) | (01 << 8) | (01);

            int end_date = Utils.getDateAsInt();
            //int end_date = (2005<<16) | (01<<8) | (01);

            //	a: start_month, starting at 0
            //	b: start day: start at 1
            //	c: start year:

            //	d: end month, starting at 0
            //	e: end day
            //	f: end year
            StringBuilder sb = Utils.sb;
            sb.Length = 0;
            sb.AppendFormat("http://ichart.finance.yahoo.com/table.csv?s={0}&d={1}&e={2}&f={3}&g=d&a={4}&b={5}&c={6}&ignore=.csv",
                    code,
                    Utils.EXTRACT_MONTH(end_date),
                    Utils.EXTRACT_DAY(end_date),
                    Utils.EXTRACT_YEAR(end_date),
                    Utils.EXTRACT_MONTH(begin_date) - 1,
                    Utils.EXTRACT_DAY(begin_date),
                    Utils.EXTRACT_YEAR(begin_date)
                    );
            startRequest(sb.ToString(), null, null);

            mNetJob = NET_QUERY_OFFLINE_DATA;
        }

        //	code | price | change | change_percent | volume
        void processDowjonePriceboard(xDataInput data)
        {
            byte[] p = data.getBytes();
            int off = data.getCurrentOffset();
            int len = data.available();

            if (len < 10)
                return;
            //	extract json structure
            int i = off;
            int j = 0;

            while (p[i] == ' ') i++;
            while (p[i] == 10 || p[i] == 13) i++; 

            if (p[i] == '<')	//	html tag
                return;

            //	seek for []
            int start = 0;
            int end = 0;
            for (; i < len; i++)
            {
                if (p[i] == '[')
                {
                    start = i;
                    break;
                }
            }
            i++;
            for (; i < len; i++)
            {
                if (p[i] == ']')
                {
                    end = i;
                    break;
                }
            }

            start++;	//	skip []
            //start++;	//	skip []
            //start++;	//	skip []
            //end--;
            //end--;
            end--;

            if (start < end)
            {
                StringBuilder sb = Utils.sb;
                sb.Length = 0;
                for (i = start; i <= end; i++)
                {
                    sb.Append((char)p[i]);
                }
                string s = sb.ToString();
                Utils.trace("===========================");
                Utils.trace(s);
                Utils.trace("===========================");
                //================================
                //object js = JSON.Instance.ToObject(s);
                JsonTextReader reader = new JsonTextReader(new StringReader(s));
                Dictionary<string, string> dict = new Dictionary<string,string>();
                string k = null;
                string v = null;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        k = reader.Value.ToString();
                    }
                    if (reader.TokenType == JsonToken.String
                        || reader.TokenType == JsonToken.Integer
                        || reader.TokenType == JsonToken.Boolean
                        || reader.TokenType == JsonToken.Float)
                    {
                        //reader.Token, reader.Value
                        v = reader.Value.ToString();
                        if (k != null)
                        {
                            dict.Add(k, v);
                        }
                        k = null;
                    }
                }

                try
                {
                    string time = dict["lt"];
                    string t = dict["t"];
                    string l_curr = dict["l_cur"];
                    string c = dict["c"];
                    string cp = dict["cp"];

                    if (t != null && t.Length > 0)
                    {
                        double point = double.Parse(l_curr);
                        double change = double.Parse(c);
                        double changePercent = double.Parse(cp);
                        string symbol = t;

                        mContext.addGlobalQuote("^DJI"/*symbol*/, point, change, changePercent, "DowJones Industrial");
                    }
                }
                catch (Exception e)
                {
                }
                //================================
            }
        }

        int TEXT_SKIP_SPACE(byte[] p, int i)
        {
            while (p[i] == ' ') i++;
            return i;
        }
        int TEXT_SKIP_CLRF(byte[] p, int i){
            while (p[i] == 10 || p[i] == 13) i++;
            return i;
        }

        int TEXT_POP(byte[] p, int i, int len, StringBuilder sb, byte delim)
        {
            sb.Length = 0;
            while (p[i] != delim && i < len) 
            {
                if (p[i] != ' ')
                    sb.Append((char)p[i]);
                i++;
            }

            i++;
            return i;
        }

        int TEXT_SKIP(byte[] p, int i, byte c, int len)
        {
            while (p[i] != c && i < len) i++;
            return i;
        }

        //	time, date, code | price | change | change_percent | volume
        void processPriceboardIndiceData(xDataInput data)
        {
            byte[] p = data.getBytes();
            int len = data.available();

            StringBuilder sb = Utils.sb;
            sb.Length = 0;

            byte delim = (byte)',';
            int tmp = 0;

            //	remove ""

            //	detect data is valid?
            int i = 0;
            //===================================
            int cnt = data.available();
            int j = 0;
            for (i = 0; i < cnt; i++)
            {
                if (p[i] == '\"' && i < cnt - 1)
                {
                    p[j++] = p[i + 1];
                    i++;
                }
                else
                {
                    p[j++] = p[i];
                }
            }
            cnt = j;
            j = 0;
            for (i = 0; i < cnt; i++)
            {
                if (p[i] == '\"' && i < cnt - 1)
                {
                    p[j++] = p[i + 1];
                    i++;
                }
                else
                {
                    p[j++] = p[i];
                }
            }
            len = j;
            //===================================

            i = 0;

            if (len > 0)
            {
                i = TEXT_SKIP_SPACE(p, i);
                i = TEXT_SKIP_CLRF(p, i);

                if (p[i] == '<')	//	html tag
                    return;
                if (p[i] < '0' || p[i] > '9')
                    return;
            }
            else
                return;

            string symbol;
            string name;
            string time;
            double point, change, changePercent;
            try
            {
                for (; i < len; i++)
                {
                    i = TEXT_SKIP_SPACE(p, i);
                    i = TEXT_SKIP_CLRF(p, i);

                    //	time 
                    i = TEXT_POP(p, i, len, sb, delim);
                    time = sb.ToString();

                    /*
                     //	date
                     TEXT_POP(p, i, sz, delim);
                     SAFE_DELETE(item->date);
                     item->date = new xString((const char*)sz);
                     */
                    //	get code
                    i = TEXT_POP(p, i, len, sb, delim);
                    symbol = sb.ToString();

                    //	price
                    i = TEXT_POP(p, i, len, sb, delim);
                    point = double.Parse(sb.ToString());

                    //	change
                    i = TEXT_POP(p, i, len, sb, delim);
                    change = double.Parse(sb.ToString());

                    //	change percent
                    i = TEXT_POP(p, i, len, sb, delim);//delim);
                    if (sb.Length > 0)
                    {
                        if (sb[sb.Length - 1] == '%')
                            sb.Remove(sb.Length - 1, 1);  //	remove '%'
                    }
                    changePercent = double.Parse(sb.ToString());

                    //  get name
                    i = TEXT_POP(p, i, len, sb, 10);
                    if (sb.Length > 0)
                    {
                        if (sb[sb.Length - 1] == '\r')
                            sb.Remove(sb.Length - 1, 1);
                    }
                    name = sb.ToString();

                    mContext.addGlobalQuote(symbol, point, change, changePercent, name);

                    i--;		//	step back 1 unit
                }
            }
            catch (Exception e)
            {
            }
        }

        //	Date,Open,High,Low,Close,Volume,Adj Close
        //	2010-11-19,11180.77,11238.52,11096.92,11203.55,3675390000,11203.55
        void processOfflineShareData(xDataInput data)
        {
            byte[] p = data.getBytes();
            int len = data.available();

            int i = 0;
            StringBuilder sz = Utils.sb;

            byte delim = (byte)',';
            int tmp = 0;

            for (i = 0; i < len; i++)
            {
                if (p[i] == '\n')
                    break;
            }

            if (len > 0)
            {
                i = TEXT_SKIP_SPACE(p, i);
                i = TEXT_SKIP_CLRF(p, i);
                if (p[i] < '0' || p[i] > '9')
                    return;
            }
            else
                return;

            Share s = mContext.mSelectedGlobalQuote;
            if (s == null)
            {
                s = new Share();
            }
            s.loadShareFromFile(false);
            int last_date = s.getLastCandleDate();
            int date;
            double o, c, h, l;
            double volume;
            try
            {
                xVectorInt vo = new xVectorInt(20000);
                xVectorInt vc = new xVectorInt(20000);
                xVectorInt vh = new xVectorInt(20000);
                xVectorInt vl = new xVectorInt(20000);
                xVectorInt vv = new xVectorInt(20000);
                xVectorInt vd = new xVectorInt(20000);
                for (; i < len; i++)
                {
                    i = TEXT_SKIP_SPACE(p, i);
                    i = TEXT_SKIP_CLRF(p, i);
                    if (i >= len)
                        break;

                    //	date
                    i = TEXT_POP(p, i, len, sz, delim);
                    date = toDateInteger(sz);
                    if (date == 0)		//	invalid data
                        break;

                    //	Open
                    i = TEXT_POP(p, i, len, sz, delim);
                    o = double.Parse(sz.ToString());

                    //	High
                    i = TEXT_POP(p, i, len, sz, delim);
                    h = double.Parse(sz.ToString());

                    //	Low
                    i = TEXT_POP(p, i, len, sz, delim);
                    l = double.Parse(sz.ToString());

                    //	Close
                    i = TEXT_POP(p, i, len, sz, delim);
                    c = double.Parse(sz.ToString());

                    //	Volume
                    //	2000000000
                    i = TEXT_POP(p, i, len, sz, delim);
                    volume = double.Parse(sz.ToString());// / 1000000;

                    //	adj_close
                    i = TEXT_POP(p, i, len, sz, 10);
                    //c->adj_close = atof(sz);
                    //s.addMoreCandle((int)(o * 100), (int)(c * 100), (int)(o * 100), (int)(h * 100), (int)(l * 100), (int)volume, date);
                    vo.addElement((int)(o * 10));
                    vc.addElement((int)(c * 10));
                    vh.addElement((int)(h * 10));
                    vl.addElement((int)(l * 10));
                    vv.addElement((int)volume);
                    vd.addElement(date);

                    //---------------------------------
                    i--;	// step back 1 unit
                }
                for (i = vo.size()-1; i >= 0; i--)
                {
                    s.addMoreCandle(vo.elementAt(i), 
                                    vc.elementAt(i),
                                    vo.elementAt(i),
                                    vh.elementAt(i),
                                    vl.elementAt(i),
                                    vv.elementAt(i),
                                    vd.elementAt(i));
                }

                s.saveShare();
            }
            catch (Exception e)
            {
                Utils.trace(e.ToString());
            }

            //============now move to share=========    
            s.setCursorScope(Context.getInstance().mOptHistoryChartTimeFrame);
            s.resetCursor();
        }

        int toDateInteger(StringBuilder sDate)
        {
            int res = 0;
            try
            {
                int i = 0;
                byte[] date = new byte[sDate.Length + 1];
                for (i = 0; i < sDate.Length; i++)
                {
                    date[i] = (byte)sDate[i];
                }
                byte delim = (byte)'-';
                date[sDate.Length] = delim;
                //	2010-11-19

                i = 0;

                StringBuilder sz = Utils.sb;
                //	year
                if (date[0] < '0' || date[0] > '9')
                    return 0;
                i = TEXT_POP(date, i, 20, sz, delim);
                res = int.Parse(sz.ToString()) << 16;

                if (res == 0)
                    return 0;

                //	month
                i = TEXT_POP(date, i, 20, sz, delim);
                res |= (int.Parse(sz.ToString()) << 8);

                //	date
                TEXT_POP(date, i, 20, sz, delim);
                res |= int.Parse(sz.ToString());
            }
            catch (Exception e)
            {
            }

            return res;
        }
    }
}
