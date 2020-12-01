using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

using xlib.ui;

namespace xlib.framework
{
    public class xHttp
    {
        xIEventListener mListener;
        xDataOutput mResponseData = new xDataOutput(10*1024);
        String mURL;
        xDataOutput mOutData = null;
        xVector mRequestParams = new xVector();
        xVector mRequestValues = new xVector();
        Thread mNetThread;
        string mHttpMethod = "POST";

        public xHttp(xIEventListener listener)
        {
            mListener = listener;
        }

        public bool get(String url, xDataOutput data)
        {
            mURL = url;
            mOutData = null;
            mHttpMethod = "GET";
            if (data != null)
            {
                mOutData = new xDataOutput(data.getBytes(), 0, data.size());
            }
            Thread thread = new Thread(new ThreadStart(this.run));
            mNetThread = thread;
            thread.Start();

            return true;
        }


        public bool post(String url, xDataOutput data)
        {
            mURL = url;
            mOutData = null;
            if (data != null){
                mOutData = new xDataOutput(data.getBytes(), 0, data.size());
            }
            Thread thread = new Thread(new ThreadStart(this.run));
            mNetThread = thread;
            thread.Start();

            return true;
        }

        static public Boolean pingAddress(String url)
        {
            try
            {
                Uri uri = new Uri(url);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";

                //-----------------------------------------------------
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                return res.StatusCode == HttpStatusCode.OK;
            }
            catch(Exception e)
            {
                utils.Utils.trace(e.ToString());
            }

            return false;
        }

        void run()
        {
            try
            {
                xMainApplication.getxMainApplication().postMessage(this, mListener, xBaseControl.EVT_NET_CONNECTING, 0, null);
                //mURL = "http://soft123.com.vn:8888/SmaSrv/SSTK";// "http://soft123.com.vn/web/home";
                Uri uri = new Uri(mURL);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = mHttpMethod;
                req.ContentType = "application/x-www-form-urlencoded";

                if (mRequestParams.size() > 0)
                {
                    for (int i = 0; i < mRequestParams.size(); i++)
                    {
                        string param = (string)mRequestParams.elementAt(i);
                        string value = (string)mRequestValues.elementAt(i);
                        req.Headers.Add(param, value);
                    }
                }

                if (mOutData != null)
                {
                    //==========================
                    xDataInput di = new xDataInput(mOutData.getBytes(), 0, mOutData.size(), true);
                    int a = di.readInt();   //  signal
                    a = di.readInt();   //  device-id
                    a = di.readInt();   //  gz
                    String session = di.readUTF();
                    String serial = di.readUTF();
                    String os = di.readUTF();
                    int pro = di.readInt();

                    //==========================
                    req.ContentLength = mOutData.size();

                    Stream o = req.GetRequestStream();
                    o.Write(mOutData.getBytes(), 0, mOutData.size());
                    //o.Flush();
                    o.Close();
                }

                //-----------------------------------------------------
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                Console.WriteLine(res.StatusDescription);
                Console.WriteLine("====Net: received content length: " + res.ContentLength);

                Stream dataStream = res.GetResponseStream();
                BinaryReader reader = new BinaryReader(dataStream);

                xMainApplication.getxMainApplication().postMessage(this, mListener, xBaseControl.EVT_NET_CONNECTED, 0, null);

                int c = 0;
                mResponseData.reset();

                int cnt = 0;
                long remain = res.ContentLength;
                byte[] buffer = new byte[1024];
                int total = 0;
                if (res.ContentLength > 0)
                    mResponseData = new xDataOutput((int)res.ContentLength + 1024);
                //mResponseData = new xDataOutput(10 * 1024 * 1024);
                while (remain > 0 || res.ContentLength == -1)
                {
                    if (!xMainApplication.getxMainApplication().isRunning())
                        break;
                    int read = reader.Read(buffer, 0, 1024);
                    //c = reader.ReadByte();
                    cnt += read;
                    total += read;
                    if (read > 0)
                    {
                        remain -= read;
                        mResponseData.write(buffer, 0, read);
                        //utils.Utils.trace("======write: " + read + "/" + total);
                        //mResponseData.writeByte((byte)c);
                    }
                    else
                    {
                        break;
                    }

                    if (cnt >= 1024)
                    {
                        xMainApplication.getxMainApplication().postMessage(this, mListener, xBaseControl.EVT_NET_DATA_AVAILABLE, mResponseData.size(), mResponseData.getBytes());
                        cnt = 0;
                    }

                    Thread.Sleep(2);
                }
                res.Close();

                xMainApplication.getxMainApplication().postMessage(this, mListener, xBaseControl.EVT_NET_DONE, mResponseData.size(), mResponseData.getBytes());
            }
            catch (IOException e)
            {
                xMainApplication.getxMainApplication().postMessage(this, mListener, xBaseControl.EVT_NET_ERROR, 0, e.Message);
            }
            catch (Exception e2)
            {
                xMainApplication.getxMainApplication().postMessage(this, mListener, xBaseControl.EVT_NET_ERROR, 0, e2.Message);
            }
        }

        public void addRequest(String key, String val)
        {
            mRequestParams.addElement(key);
            mRequestValues.addElement(val);
        }

        public void cancel()
        {
            if (mNetThread != null)
            {
                try
                {
                    mNetThread.Abort();
                }catch(Exception e)
                {
                }
            }
        }
    }
}
