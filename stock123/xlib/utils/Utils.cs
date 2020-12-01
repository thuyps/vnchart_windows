/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

using System;
using System.Text;
using xlib.framework;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

/**
 *
 * @author ThuyPham
 */
namespace xlib.utils
{
    public class Utils
    {

        public static uint LINE_GREY_COLOR = 0xff151515;
        public static uint LINE_GREY_COLOR2 = 0xff000000;//0xff050505;
        public static uint LINE_YELLOW_COLOR = 0xffaaaa5f;
        public static uint BG_WHITE_COLOR = 0xffffffff;

        public static StringBuilder sb = new StringBuilder();
        public static StringBuilder sb2 = new StringBuilder();

        public static int EXTRACT_YEAR(int d)
        {
            return (d >> 16);
        }

        public static int EXTRACT_MONTH(int d)
        {
            return ((d >> 8) & 0xff);
        }

        public static int EXTRACT_DAY(int d)
        {
            return (d & 0xff);
        }
        //	date:0x00hhmmss

        public static int EXTRACT_HOUR(int t)
        {
            return ((t >> 16) & 0xff);
        }

        public static int EXTRACT_MINUTE(int t)
        {
            return ((t >> 8) & 0xff);
        }

        public static int EXTRACT_SECOND(int t)
        {
            return (t & 0xff);
        }

        public static int ABS_INT(int a)
        {
            if (a > 0)
            {
                return a;
            }
            return -a;
        }

        public static float ABS_FLOAT(float a)
        {
            if (a > 0)
            {
                return a;
            }
            return -a;
        }

        public static void memcpy(byte[] dest, byte[] src, int src_off, int len)
        {
            if (len > dest.Length)
            {
                len = dest.Length;
            }
            for (int i = 0; i < len; i++)
            {
                dest[i] = src[src_off + i];
            }
        }

        public static void memcpy(byte[] dest, int dest_off, byte[] src, int src_off, int len)
        {
            if (len > dest.Length - dest_off)
            {
                len = dest.Length - dest_off;
            }
            for (int i = 0; i < len; i++)
            {
                dest[dest_off + i] = src[src_off + i];
            }
        }

        public static short readShort(byte[] p, int off)
        {
            int t = (((256 + p[off]) & 0xff) << 8) | ((256 + p[off + 1]) & 0xff);

            return (short)t;
        }

        public static int readInt(byte[] p, int off)
        {
            int t = (((256 + p[off]) & 0xff) << 24) | (((256 + p[off + 1]) & 0xff) << 16) | (((256 + p[off + 2]) & 0xff) << 8) | ((256 + p[off + 3]) & 0xff);

            off += 4;
            return t;
        }

        public static void writeInt(byte[] p, int off, int v)
        {
            p[off] = (byte)((v >> 24) & 0xff);
            p[off + 1] = (byte)((v >> 16) & 0xff);
            p[off + 2] = (byte)((v >> 8) & 0xff);
            p[off + 3] = (byte)(v & 0xff);
        }

        public static void writeInt(byte[] p, int off, uint v)
        {
            p[off] = (byte)((v >> 24) & 0xff);
            p[off + 1] = (byte)((v >> 16) & 0xff);
            p[off + 2] = (byte)((v >> 8) & 0xff);
            p[off + 3] = (byte)(v & 0xff);
        }

        public static void writeshort(byte[] p, int off, int v)
        {
            p[off] = (byte)((v >> 8) & 0xff);
            p[off + 1] = (byte)(v & 0xff);
        }

        public static void writeShort(byte[] p, int off, int v)
        {
            p[off] = (byte)((v >> 8) & 0xff);
            p[off + 1] = (byte)(v & 0xff);
        }

        public static void writeBytes(byte[] p, int off, byte[] src, int srcOff, int len)
        {
            for (int i = 0; i < len; i++)
            {
                p[off + i] = src[srcOff + i];
            }
        }

        public static void strcpy(byte[] dst, byte[] src)
        {
            int l = src.Length;
            int i = 0;
            for (i = 0; i < l; i++)
            {
                if (src[i] == 0)
                    break;

                dst[i] = src[i];
            }
            dst[i] = 0;
        }

        public static bool strcmp(byte[] s1, int off1, byte[] s2, int off2, int maxCnt)
        {
            if (s1[off1] == 0 && s2[off2] != 0)
                return false;
            if (s1[off1] != 0 && s2[off2] == 0)
                return false;

            for (int i = 0; i < maxCnt; i++)
            {
                if (s1[off1 + i] == 0 || s2[off2 + i] == 0)
                    break;
                if (s1[off1 + i] != s2[off2 + i])
                {
                    return false;
                }
            }

            return true;
        }

        public static String bytesNullTerminatedToString(byte[] p, int offset, int len)
        {
            int l = 0;

            for (int i = 0; i < len; i++)
            {
                if (offset + i >= p.Length)
                    break;

                if (p[offset + i] == 0)
                    break;
                l++;
            }

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            return enc.GetString(p, offset, l);
        }

        public static String bytesToString(byte[] p, int offset, int len)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            return enc.GetString(p, offset, len);
        }


        public static int getDaysCountFrom(int date)
        {
            if (date == 0)
                return 10000;
            DateTime now = DateTime.Today;
            DateTime dt = new DateTime((date >> 16) & 0xffff, (date >> 8) & 0xff, date & 0xff);

            TimeSpan ts = now.Subtract(dt);
            return ts.Days;
        }

        public static long dateToNumber(int date)
        {
            long y = EXTRACT_YEAR(date);
            long m = EXTRACT_MONTH(date);
            long d = EXTRACT_DAY(date);
            m = (m + 9) % 12;   /* mar=0, feb=11 */
            y = y - m / 10;       /* if Jan/Feb, year-- */
            return 365 * y + y / 4 - y / 100 + y / 400 + (m * 306 + 5) / 10 + (d - 1);
        }

        public static int dateFromNumber(long d)
        {
            long y, ddd, mm, dd, mi;

            y = (10000 * d + 14780) / 3652425;
            ddd = d - (y * 365 + y / 4 - y / 100 + y / 400);
            if (ddd < 0)
            {
                y--;
                ddd = d - (y * 365 + y / 4 - y / 100 + y / 400);
            }
            mi = (52 + 100 * ddd) / 3060;
            int Y = (int)(y + (mi + 2) / 12);
            int M = (int)((mi + 2) % 12 + 1);
            int D = (int)(ddd - (mi * 306 + 5) / 10 + 1);

            return (Y << 16) | (M << 8) | D;
        }

        public static int stepBackToWorkingDay(int date)
        {
            while (true)
            {
                int day = dayOfWeek(date);
                if (day == 0 || day == 6)
                {
                    long number = dateToNumber(date);
                    number--;
                    date = dateFromNumber(number);
                }
                else
                {
                    break;
                }
            }
            return date;
        }

        public static int getDateAsInt()
        {
            DateTime now = DateTime.Today;

            int year = now.Year;
            int month = now.Month;
            int date = now.Day;

            return (year << 16) | (month << 8) | (date);
        }

        public static String dateIntToString(int date)
        {
            return "" + EXTRACT_DAY(date) + "/" + (EXTRACT_MONTH(date)) + "/" + (EXTRACT_YEAR(date) - 2000);
        }

        public static String dateIntToString4(int date)
        {
            return "" + EXTRACT_DAY(date) + "/" + (EXTRACT_MONTH(date)) + "/" + (EXTRACT_YEAR(date));
        }

        public static String getDateAsString()
        {
            DateTime now = DateTime.Now;
            return now.ToShortDateString();
            //return "" + date + "/" + month + "/" + year;
        }

        public static int getDateAsInt(int dayBack)
        {
            DateTime now = DateTime.Now;
            TimeSpan ts = new TimeSpan(dayBack, 0, 0, 0);

            now = now.Subtract(ts);

            int year = now.Year;
            int month = now.Month;
            int date = now.Day;

            return (year << 16) | (month << 8) | (date);
        }

        public static xVector getTokens(String s, char delim)
        {
            xVector v = new xVector();

            StringBuilder tk = Utils.sb;
            tk.Length = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == delim)
                {
                    v.addElement(tk.ToString());
                    tk.Length = 0;
                }
                else
                {
                    tk.Append(c);
                }
            }
            if (tk.Length > 0)
            {
                v.addElement(tk.ToString());
            }

            return v;
        }

        public static int intToString(int v, byte[] sz, int off)
        {
            //  v = 123;
            int i = 0;
            do
            {
                tempSz[i++] = (byte)((v % 10) + '0');
                v = v / 10;
            } while (v > 0);
            for (int j = i - 1; j >= 0; j--)
            {
                sz[off++] = tempSz[j];
            }
            return i;
        }

        public static String szToString(byte[] sz)
        {
            Utils.sb.Length = 0;

            int j = 0;
            while (sz[j] != 0)
            {
                Utils.sb.Append((char)sz[j]);
                j++;
            }
            return Utils.sb.ToString();
        }
        static byte[] tempSz = new byte[50];
        static byte[] tempSz1 = new byte[50];

        public static String formatDecimalNumberWidthSignal(int val, int majorDev, int postSurfixCnt)
        {
            String s = formatDecimalNumber(val, majorDev, postSurfixCnt);
            if (val == 0)
                return s;

            if (val < 0) return "- " + s;

            return "+" + s;
        }

        //	val = 12345, decimalRound = 100, postSurfixCnt = 1 => sz = 123.4
        //	val = 12345, decimalRound = 1000, postSurfixCnt = 2 => sz = 12.34
        static String nullVal = "-";
        public static String formatDecimalNumber(int val, int majorDev, int postSurfixCnt)
        {
            if (val == 0)
                return nullVal;
            StringBuilder sb = Utils.sb2;
            sb.Length = 0;
            double t = val;
            t = t / majorDev;
            if (postSurfixCnt == 1)
                sb.AppendFormat("{0:F1}", t);
            else if (postSurfixCnt == 2)
                sb.AppendFormat("{0:F2}", t);
            else
                sb.AppendFormat("{0:F3}", t);

            return sb.ToString();
            /*
            byte[] sz = tempSz1;
            if (val == 0)
            {
                sz[0] = (byte)'-';
                sz[1] = 0;
                return szToString(sz);
            }

            int sub = (val % majorDev);
            if (sub < 0)
            {
                sub = -sub;
            }
            int t = 0;
            if (val < 0)
            {
                t += intToString(Utils.ABS_INT(val / majorDev), sz, t);
                sz[t++] = (byte)'.';
                t += intToString(sub, sz, t);
            }
            else
            {
                t += intToString(val / majorDev, sz, t);
                sz[t++] = (byte)'.';
                t += intToString(sub, sz, t);
            }
            sz[t] = 0;

            //	17.500
            int j = 0;
            while (sz[j] != '.')
            {
                j++;
            }
            if (postSurfixCnt == 0)
            {
                sz[j] = 0;
                return szToString(sz);
            }
            j++;
            int k = 0;
            while (sz[j + k] != 0 && k < postSurfixCnt)
            {
                k++;
            }
            sz[j + k] = 0;

            return szToString(sz);
             */
        }

        //	val = 12345, decimalRound = 100, postSurfixCnt = 1 => sz = 123.4
        //	val = 12345, decimalRound = 1000, postSurfixCnt = 2 => sz = 12.34
        public static void formatDecimalNumberAsNullTerminatedString(int val, int majorDev, int postSurfixCnt, byte[] o)
        {
            byte[] sz = o;
            if (val == 0)
            {
                sz[0] = (byte)'-';
                sz[1] = 0;
                return;
            }

            int sub = (val % majorDev);
            if (sub < 0)
            {
                sub = -sub;
            }
            int t = 0;
            if (val < 0)
            {
                t += intToString(Utils.ABS_INT(val / majorDev), sz, t);
                sz[t++] = (byte)'.';
                t += intToString(sub, sz, t);
            }
            else
            {
                t += intToString(val / majorDev, sz, t);
                sz[t++] = (byte)'.';
                t += intToString(sub, sz, t);
            }
            sz[t] = 0;

            //	17.500
            int j = 0;
            while (sz[j] != '.')
            {
                j++;
            }
            if (postSurfixCnt == 0)
            {
                sz[j] = 0;
                return;
            }
            j++;
            int k = 0;
            while (sz[j + k] != 0 && k < postSurfixCnt)
            {
                k++;
            }
            sz[j + k] = 0;
        }

        //	val = 12345, decimalRound = 100, postSurfixCnt = 1 => sz = 123.4
        //	val = 12345, decimalRound = 1000, postSurfixCnt = 2 => sz = 12.34
        //	val = 1112.345, decimalRound = 10000, postSurfixCnt = 4 => sz = 12.34
        public static String formatDecimalNumber2(int val, int decimalRound, int postSurfixCnt)
        {
            Utils.sb.Length = 0;
            if (val == 0)
            {
                sb.Append('-');
                return sb.ToString();
            }
            return formatDecimalNumber(val, decimalRound, postSurfixCnt);
        }

        public static String[] filterStringList(String[] strings, String pattern)
        {
            /*
            if(strings == null || strings.Length <= 0)
                return null;
        
            xVector temp = new xVector();
            int len = strings.Length;

            for(int i = 0; i < len; i++)
            {
                if(strings[i].startsWith(pattern.toUpperCase()))
                {
                    temp.addElement(strings[i]);               
                }
            }

            len = temp.size();
            String[] rtn = new String[len];
            temp.copyInto(rtn);

            return rtn;
             */
            return null;
        }

        public static String formatNumberDouble(double number)
        {
            //String s = number.ToString("G");
            //String fmt = "000,000,000,000";
            //String s = String.Format("{0:C0}", Convert.ToInt64(number));
            //String s = number.ToString(fmt);
            //s = String.Format(new ByteByByteFormatter(), "{0,10:N0}", number);

            Int64 t = Convert.ToInt64(number);
            String s = null;
            s = String.Format("{0:C0}", t);

            return s;
        }

        public static String formatNumber(int number)
        {
            if (number == 0)
            {
                return "0";
            }
            //  2345678
            byte[] tmp = tempSz;//new byte[15];
            int j = 0;
            int signal = 1;
            if (number < 0)
            {
                number = -number;
                signal = -1;
            }

            while (number > 0)
            {
                tmp[j++] = (byte)(number % 10);
                number = number / 10;
            }
            // 00013

            sb.Length = 0;
            if (signal == -1)
                sb.Append('-');
            for (int i = j - 1; i >= 0; i--)
            {
                sb.Append((char)('0' + tmp[i]));
                if ((i % 3) == 0 && i > 0)
                {
                    sb.Append(',');
                }
            }

            return sb.ToString();
        }

        public static String formatNumber(float number, int noRound)
        {
            // parse the integer part
            int iNumber = (int)number;
            String s = formatNumber(iNumber);
            // start
            sb.Length = 0;
            sb.Append(s);
            if (sb.Length == 0)
            {
                return sb.ToString();
            }
            // parse the float part
            if (noRound > 0)
            {
                sb.Append('.');
                float dNumber = number - iNumber;
                for (int i = 0; i < noRound; i++)
                {
                    dNumber *= 10;
                }
                int iDNumber = (int)dNumber;
                String sDNumber = "" + iDNumber;
                if (sDNumber.Length > noRound)
                {
                    sDNumber = sDNumber.Substring(0, noRound);
                }
                for (int i = 0; i < noRound - sDNumber.Length; i++)
                {
                    sb.Append('0');
                }
                sb.Append(sDNumber);
            }
            // return the result
            return sb.ToString();
        }

        static public long currentTimeMillis()
        {
            return (long)DateTime.Now.TimeOfDay.TotalMilliseconds;// .Milliseconds;
        }

        static public void trace(String msg)
        {
            //System.Diagnostics.Trace.WriteLine(msg);
            System.Console.WriteLine(msg);
        }

        static public String MD5String(String pass)
        {
            MD5 md5 = MD5.Create();
            UTF8Encoding enc = new UTF8Encoding();
            byte[] data = md5.ComputeHash(enc.GetBytes(pass));
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        static int[] t = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };
        static public int dayOfWeek(int date)
        {
            return dayOfWeek(EXTRACT_YEAR(date), EXTRACT_MONTH(date), EXTRACT_DAY(date));
        }
        static public int dayOfWeek(int y, int m, int d) /* 0 = Sunday */
        {
            //if (m >= 1 && m <= 12 && d >= 0 && d <= 31)
            {
                y -= (m < 3) ? 1 : 0;
                return (y + y / 4 - y / 100 + y / 400 + t[m - 1] + d) % 7;
            }
            return 0;
        }

        static Label labelUtils = null;
        static Graphics graphicUtils = null;
        static public int getStringW(String text, Font f)
        {
            if (graphicUtils == null)
            {
                labelUtils = new Label();
                labelUtils.Text = "";
                graphicUtils = labelUtils.CreateGraphics();
            }

            return (int)graphicUtils.MeasureString(text, f).Width;
        }

        static public bool isInRect(int x, int y, int left, int top, int w, int h)
        {
            if (x >= left && y >= top && x < left + w && y < top + h)
                return true;

            return false;
        }

        static public ImageList createImageList(String imgPNG, int frameW, int frameH)
        {
            Image png = Image.FromFile(imgPNG);
            ImageList imgs = new ImageList();
            imgs.ColorDepth = ColorDepth.Depth32Bit;
            imgs.TransparentColor = Color.Transparent;
            imgs.ImageSize = new Size(frameW, frameH);
            imgs.Images.AddStrip(png);

            return imgs;
        }

        static public ImageList createImageList(String imgPNG)
        {
            Image png = Image.FromFile(imgPNG);
            ImageList imgs = new ImageList();
            imgs.ColorDepth = ColorDepth.Depth32Bit;
            imgs.TransparentColor = Color.Transparent;
            imgs.ImageSize = new Size(png.Height, png.Height);
            imgs.Images.AddStrip(png);

            return imgs;
        }

        static public ImageList createImageList(Image img, int w, int h)
        {
            ImageList imgs = new ImageList();
            imgs.ColorDepth = ColorDepth.Depth32Bit;
            imgs.TransparentColor = Color.Transparent;
            if (w == -1)
            {
                w = img.Height;
                h = img.Height;
            }
            imgs.ImageSize = new Size(w, h);
            imgs.Images.AddStrip(img);

            return imgs;
        }

        static public Image createImageFromBytes(byte[] data, int off, int len)
        {
            try
            {
                Stream stream = new MemoryStream(data, off, len);
                Image png = Image.FromStream(stream);

                return png;
            }catch(Exception e)
            {
            }

            return null;
        }

        public static int convertBigIntToLittleInt(int a)
        {
            int n = ((a & 0xff) << 24) | (((a >> 8) & 0xff) << 16) | (((a >> 16) & 0xff) << 8) | ((a >> 24) & 0xff);
            return n;
        }

        static public StringBuilder getSB()
        {
            sb.Length = 0;
            return sb;
        }

        static public bool isValidEmail(string email)
        {
            if (email == null)
                return false;
            if (email.Length == 0)
                return false;
            if (!email.Contains("@"))
                return false;

            return true;
        }

        public static void captureScreenArea(int x, int y, int w, int h, string filename)
        {
            try
            {
                Bitmap bm = new Bitmap(w, h);
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(x, y, 0, 0, new Size(w, h));

                bm.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception e)
            {
            }
        }

        public static void captureViewAsImage(xlib.ui.xBaseControl c, string filename)
        {
            try
            {
                Bitmap bm = new Bitmap(c.getW(), c.getH());
                Graphics g = Graphics.FromImage(bm);

                c.getControl().DrawToBitmap(bm, new Rectangle(0, 0, c.getW(), c.getH()));

                bm.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                //bm.Save(filename + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
            }
        }

        public static void captureViewAsImage(xlib.ui.xBaseControl c, int x, int y, int w, int h, string filename)
        {
            try
            {
                Bitmap bm = new Bitmap(w, h);
                Graphics g = Graphics.FromImage(bm);

                c.getControl().DrawToBitmap(bm, new Rectangle(-x, -y, c.getW(), c.getH()));

                bm.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                //bm.Save(filename + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
            }
        }

        public static void parseJSONToMap(String s)
        {
            JsonReader reader = new JsonTextReader(new StringReader(s));
            Dictionary<string, string> dict = new Dictionary<string, string>();
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
        }

        public static float stringToFloat(String s)
        {
            try
            {
                return float.Parse(s);
            }
            catch (Exception e)
            {
            }
            return 0;
        }

        public static int stringToInt(String s)
        {
            try
            {
                return int.Parse(s);
            }
            catch (Exception e)
            {
            }
            return 0;
        }

        public static bool stringToBool(String s)
        {
            try
            {
                return bool.Parse(s);
            }
            catch (Exception e)
            {
            }
            return false;
        }
    }
}