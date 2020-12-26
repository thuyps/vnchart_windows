using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xlib.framework;
using stock123.app.data;
using stock123.app;
using stock123.app.table;

namespace stock123.app.utils
{
    class ShareSortUtils
    {
        public const int SORT_RSI = 0;
        public const int SORT_MFI = 1;
        public const int SORT_VonHoa = 2;
        public const int SORT_GiaTriGiaoDich = 3;
        public const int SORT_EPS = 4;
        public const int SORT_PE = 5;
        public const int SORT_THAYDOI_VOL = 6;
        public const int SORT_VOLUME = 7;
        public const int SORT_TRADE_VALUE = 8;
        public const int SORT_SYMBOL = 9;
        public const int SORT_PRICE = 10;

        static public void doSort(xVector v, int type)
        {
            bool revert = false;
            if (type == SORT_RSI)
            {
                revert = true;
                evalueSortValueRSI(v);
            }
            else if (type == SORT_MFI)
            {
                revert = true;
                evalueSortValueMFI(v);
            }
            else if (type == SORT_EPS)
            {
                revert = true;
                evalueSortValueEPS(v);
            }
            else if (type == SORT_PE)
            {
                evalueSortValuePE(v);
            }
            else if (type == SORT_VonHoa)
            {
                revert = true;
                evalueSortValueVonhoa(v);
            }
            else if (type == SORT_TRADE_VALUE)
            {
                revert = true;
                evalueSortValueTrade(v);
            }
            else if (type == SORT_THAYDOI_VOL)
            {
                revert = true;
                evalueSortValueVolChanged(v);
            }
            else if (type == SORT_VOLUME)
            {
                revert = true;
                evalueSortValueVolume(v);
            }
            else if (type == SORT_SYMBOL)
            {
                evalueSortValueSymbol(v);
            }
            else if (type == SORT_PRICE)
            {
                evalueSortValuePrice(v);
            }

            sort(v);

            if (revert)
            {
                v.makeReverse();
            }
        }

        static void sort(xVector v)
        {
            int cnt = v.size();
            for (int i = 0; i < cnt - 1; i++)
            {
                Share smallest = (Share)v.elementAt(i);
                int smallestIdx = i;
                for (int j = i+1; j < cnt; j++)
                {
                    Share item = (Share)v.elementAt(j);
                    if (smallest.mSortParam > item.mSortParam)
                    {
                        smallest = item;
                        smallestIdx = j;
                    }
                }

                if (smallestIdx != i)
                {
                    v.swap(i, smallestIdx);
                }
            }
        }

        static void evalueSortValueRSI(xVector v)
        {
            int rsiPeriod = (int)Context.getInstance().mOptRSIPeriod[0];
            if (rsiPeriod > 3 && rsiPeriod < 30)
            {
            }
            else
            {
                rsiPeriod = 14;
            }

            for (int i = 0; i < v.size(); i++)
            {
                Share share = (Share)v.elementAt(i);
                share.loadShareFromCommonData(true, true);

                //if (share.getCode().CompareTo("SIP") == 0)
                //{
                    //share.setCode("SIP", 1);
                //}

                if (share.getCandleCnt() >= rsiPeriod)
                {
                    share.calcRSI(0);
                    share.mSortParam = share.pRSI[share.getCandleCnt() - 1];
                    share.mCompareText = String.Format("{0:F1}", share.mSortParam);
                }
                else
                {
                    share.mCompareText = "0";
                    share.mSortParam = 0;
                }
            }
        }

        static void evalueSortValueMFI(xVector v)
        {
            int mfiPeriod = (int)Context.getInstance().mOptMFIPeriod[0];
            if (mfiPeriod > 3 && mfiPeriod < 30)
            {
            }
            else
            {
                mfiPeriod = 14;
            }

            for (int i = 0; i < v.size(); i++)
            {
                Share share = (Share)v.elementAt(i);
                share.loadShareFromCommonData(true, true);

                if (share.getCandleCnt() >= mfiPeriod)
                {
                    share.calcMFI(0);
                    share.mSortParam = share.pMFI[share.getCandleCnt() - 1];
                    share.mCompareText = String.Format("{0:F1}", share.mSortParam);
                }
                else
                {
                    share.mCompareText = "0";
                    share.mSortParam = 0;
                }
            }
        }

        static void evalueSortValueVolume(xVector v)
        {
            for (int i = 0; i < v.size(); i++)
            {
                Share share = (Share)v.elementAt(i);

                stPriceboardState ps = Context.getInstance().mPriceboard.getPriceboard(share.getShareID());

                if (!share.isIndex() && ps != null)
                {
                    share.mSortParam = ps.current_volume_1;
                    share.mCompareText = RowNormalShare.volumeToString(ps.current_volume_1);
                }
                else
                {
                    share.mSortParam = 0;
                    share.mCompareText = "0";
                }
            }
        }

        static void evalueSortValueVonhoa(xVector v)
        {
            for (int i = 0; i < v.size(); i++)
            {
                Share share = (Share)v.elementAt(i);

                if (share.isIndex())
                {
                    share.mSortParam = 0;
                    share.mCompareText = "0";
                }
                else
                {
                    stCompanyInfo inf = Context.getInstance().mShareManager.getCompanyInfo(share.getShareID());
                    stPriceboardState ps = Context.getInstance().mPriceboard.getPriceboard(share.getShareID());
                    if (inf != null && ps != null)
                    {
                        share.mSortParam = inf.volume * ps.current_price_1;
                        double t = share.mSortParam;
                        share.mCompareText = RowNormalShare.volumeToString((int)t);
                    }
                    else
                    {
                        share.mSortParam = 0;
                        share.mCompareText = "0";
                    }
                }

            }
        }

        static void evalueSortValueTrade(xVector v)
        {
            for (int i = 0; i < v.size(); i++)
            {
                Share share = (Share)v.elementAt(i);

                if (share.isIndex())
                {
                    share.mSortParam = 0;
                    share.mCompareText = "0";
                }
                else
                {
                    stPriceboardState ps = Context.getInstance().mPriceboard.getPriceboard(share.getShareID());
                    if (ps != null)
                    {
                        share.mSortParam = ps.current_price_1 * ps.total_volume;
                        double t = share.mSortParam / 1000;
                        share.mCompareText = RowNormalShare.volumeToString((int)t);
                    }
                    else
                    {
                        share.mSortParam = 0;
                        share.mCompareText = "0";
                    }
                }

            }
        }

        static void evalueSortValueEPS(xVector v)
        {
            for (int i = 0; i < v.size(); i++)
            {
                Share share = (Share)v.elementAt(i);

                stCompanyInfo inf = Context.getInstance().mShareManager.getCompanyInfo(share.getShareID());

                if (!share.isIndex() && inf != null)
                {
                    share.mSortParam = inf.EPS;
                    share.mCompareText = String.Format("{0:F1}", (inf.EPS/1000.0));
                }
                else
                {
                    share.mSortParam = 0;
                    share.mCompareText = "0";
                }
            }
        }
        static void evalueSortValuePE(xVector v)
        {
            for (int i = 0; i < v.size(); i++)
            {
                Share share = (Share)v.elementAt(i);

                stCompanyInfo inf = Context.getInstance().mShareManager.getCompanyInfo(share.getShareID());

                if (!share.isIndex() && inf != null)
                {
                    share.mSortParam = inf.PE;
                    share.mCompareText = String.Format("{0:F1}", (inf.PE / 1000.0));
                }
                else
                {
                    share.mSortParam = 0;
                    share.mCompareText = "0";
                }
            }
        }
        static void evalueSortValueSymbol(xVector v)
        {
            for (int i = 0; i < v.size(); i++)
            {
                Share share = (Share)v.elementAt(i);
                String code = share.getCode();

                if (code == null || code.Length < 3)
                {
                    share.mSortParam = 1000;
                }
                else
                {
                    char[] cc = code.ToCharArray();
                    int value = ((int)cc[0] << 24) | ((int)cc[1] << 16) | ((int)cc[2] << 8);
                    if (cc.Length > 3)
                    {
                        share.mSortParam = value | (int)cc[3];
                    }
                    else
                    {
                        share.mSortParam = value;
                    }
                }
            }
        }

        static void evalueSortValuePrice(xVector v)
        {
            for (int i = 0; i < v.size(); i++)
            {
                Share share = (Share)v.elementAt(i);
                /*if (share.getCode().CompareTo("NAW") == 0)
                {
                    stPriceboardState ps = Context.getInstance().mPriceboard.getPriceboard(share.getShareID());
                    share.mSortParam = ps.current_price_1;
                }
                 */
                if (!share.isIndex())
                {
                    stPriceboardState ps = Context.getInstance().mPriceboard.getPriceboard(share.getShareID());
                    share.mSortParam = ps.current_price_1;
                    share.mSortParam = 0;
                    if (share.mSortParam == 0)
                    {
                        share.loadShareFromCommonData(false);
                        if (share.getCandleCnt() > 0)
                        {
                            share.mSortParam = share.getClose(share.getCandleCnt() - 1);
                        }
                    }
                }
                else
                {
                    share.mSortParam = 10000;
                }
            }
        }

        static void evalueSortValueVolChanged(xVector v)
        {
            for (int i = 0; i < v.size(); i++)
            {
                Share share = (Share)v.elementAt(i);
                /*
                if (share.getCode().CompareTo("VCP") == 0)
                {
                    stPriceboardState ps = Context.getInstance().mPriceboard.getPriceboard(share.getShareID());

                    int vol3 = share.getAveVolumeInDays(3);
                    int vol15 = share.getAveVolumeInDays(15);
                    if (vol15 > 0)
                    {
                        share.mSortParam = vol3 * 100;
                        share.mSortParam = share.mSortParam / vol15;
                        share.mCompareText = String.Format("{0:D}", (int)share.mSortParam);
                    }
                }
                 */

                share.loadShareFromCommonData(true);

                int vol3 = share.getAveVolumeInDays(3);
                int vol15 = share.getAveVolumeInDays(15);
                if (vol15 > 0)
                {
                    share.mSortParam = vol3 * 100;
                    share.mSortParam = share.mSortParam / vol15;
                    share.mCompareText = String.Format("{0:D}", (int)share.mSortParam);
                }
                else
                {
                    share.mSortParam = 0;
                    share.mCompareText = "0";
                }
            }
        }
    }
}
