using System;
using System.Collections.Generic;
using System.Text;
using xlib.framework;
using xlib.utils;

namespace stock123.app.data
{
    public class FilterItem
    {
        public string name;
        public int type;
        public float param1;
        public float param2;
        public int param3;
        public bool hasSetting;
        public bool[] selected = {false};

        public String getTitle()
        {
            if (type == FilterManager.SIGNAL_RS)
            {
                return String.Format("Relative Strength (MA={0:D}", (int)param1);
            }
            if (type == FilterManager.SIGNAL_RS_PERFORMANCE)
            {
                return String.Format("Relative Price perform (period={0:D}, MA={1:D})", (int)param1, (int)param2);
            }
            if (type == FilterManager.SIGNAL_VOLUME_IS_UP) return "Khối lượng tăng";
            if (type == FilterManager.SIGNAL_VOLUME_IS_DOWN) return "Khối lượng giảm";

            //  MACD
            if (type == FilterManager.SIGNAL_MACD_CUT_ABOVE_SIGNALLINE) return "MACD cắt & vượt trên Signal";
            if (type == FilterManager.SIGNAL_BUY_MACD_ABOVE_SIGNALLINE) return "MACD trên Signal";
            if (type == FilterManager.SIGNAL_MACD_CUT_ABOVE_CENTERLINE) return "MACD cắt & vượt trên Center line";
            if (type == FilterManager.SIGNAL_BUY_MACD_ABOVE_CENTERLINE) return "MACD trên Center line";

            //  MACD
            if (type == FilterManager.SIGNAL_MACD_CUT_BELOW_SIGNALLINE) return "MACD nhỏ hơn Signal";
            if (type == FilterManager.SIGNAL_MACD_CUT_BELOW_CENTERLINE) return "MACD nhỏ hơn Center line";

            //  Ichimoku
            if (type == FilterManager.SIGNAL_ICHIMOKU_CUT_ABOVE_TENKAN_KIJUN) return "Tenkan-sen (Conv) cắt & vượt trên Kijun-sen (Base)";
            if (type == FilterManager.SIGNAL_BUY_ICHIMOKU_PRICE_CUT_ABOVE_KIJUN) return "Giá cắt & vượt trên Kijun-sen (Base)";
            if (type == FilterManager.SIGNAL_BUY_ICHIMOKU_GREEN_CLOUD) return "Ichimoku: Mây màu xanh";

            //  Ichimoku
            if (type == FilterManager.SIGNAL_ICHIMOKU_TENKAN_BELOW_KIJUN) return "Tenkan-sen (Conv) nằm dưới Kijun-sen (Base).";
            if (type == FilterManager.SIGNAL_ICHIMOKU_PRICE_CUT_BELOW_KIJUN) return "Giá nằm dưới Kijun-sen (Base).";

            //  PSAR & VSTOP
            if (type == FilterManager.SIGNAL_PSAR_TURNS_GREEN) return "PSAR xanh";
            if (type == FilterManager.SIGNAL_PSAR_TURNS_RED) return "PSAR đỏ";
            if (type == FilterManager.SIGNAL_VSTOP_TURN_GREEN) return "VStop xanh";
            if (type == FilterManager.SIGNAL_VSTOP_TURNS_RED) return "VSTOP đỏ";

            //  Moving average
            if (type == FilterManager.SIGNAL_MA1_CUT_ABOVE_MA2
                || type == FilterManager.SIGNAL_MA1_CUT_ABOVE_MA2_2
                || type == FilterManager.SIGNAL_MA1_CUT_ABOVE_MA2_3)
            {
                String s = String.Format("MA {0} nằm trên MA {1}", param1, param2);
                if (param3 > 0)
                {
                    s = String.Format("MA {0} nằm trên MA {1} ({2} ngày)", param1, param2, param3);
                }
                return s;
            }

            if (type == FilterManager.SIGNAL_PRICE_FLUCTUATION)
            {
                int minPercent = (int)param1;
                int maxPercent = (int)param2;
                int n = param3;
                if (n < 1)
                {
                    n = 66;

                }
                String s = String.Format("Giá dao động khoảng {0}% trong {1} ngày", /*minPercent, */maxPercent, n);
                return s;
            }
            if (type == FilterManager.SIGNAL_TOP_PRICE_DOWN)
            {
                int minPercent = (int)param1;
                int maxPercent = (int)param2;
                int n = param3;
                if (n < 1)
                {
                    n = 66;

                }
                String s = String.Format("Giảm trong khoảng [{0}%-{1}%] trong {2} ngày", minPercent, maxPercent, n);
                return s;
            }
            else if (type == FilterManager.SIGNAL_TOP_PRICE_UP)
            {
                int minPercent = (int)param1;
                int maxPercent = (int)param2;
                int n = param3;
                if (n < 1)
                {
                    n = 66;

                }
                String s = String.Format("Tăng trong khoảng [{0}%-{1}%] trong {2} ngày", minPercent, maxPercent, n);
                return s;
            }

            if (type == FilterManager.SIGNAL_MA1_CUT_BELOW_MA2
                || type == FilterManager.SIGNAL_MA3_CUT_BELOW_MA4)
            {
                String s = String.Format("MA {0} nằm dưới MA {1}", param1, param2);
                if (param3 > 0)
                {
                    s = String.Format("MA {0} nằm dưới MA {1} ({2} ngày)", param1, param2, param3);
                }
                return s;
            }

            if (type == FilterManager.SIGNAL_VOLUME_IS_OUT)
            {
                return "Khối lượng cạn kiệt";
            }

            if (type == FilterManager.SIGNAL_HEIKEN_CANDLE_GREEN) return "Heiken xanh";
            if (type == FilterManager.SIGNAL_HEIKEN_CANDLE_RED) return "Heiken đỏ";

            //  TRIX
            //arr.addElement(initWithSignal(FilterManager.SIGNAL_BUY_TRIX, "TRIX buy"));
            //arr.addElement(initWithSignal(FilterManager.SIGNAL_SELL_TRIX, "TRIX sign"));

            //  ADX
            if (type == FilterManager.SIGNAL_BUY_ADX_DIs_CROSS)
            {
                String s = String.Format("ADX: DI+ cắt & vượt trên DI- {0} ngày", param1);
                return s;
            }
            if (type == FilterManager.SIGNAL_SELL_ADX_DIs_CROSS) return "ADX: DI+ nằm dưới DI-";
            if (type == FilterManager.SIGNAL_ADX_DIPLUS_ABOVE_DIMINUS) return "ADX: DI+ nằm trên DI-";

            if (type == FilterManager.SIGNAL_BUY_ADX_HIGHER_THAN)
            {
                String s = String.Format("ADX lớn hơn {0}", param1);
                return s;
            }
            //  Stochartics
            if (type == FilterManager.SIGNAL_BUY_FULLSTO) return "Full Stochartic: %K cắt & vượt trên %D";
            //arr.addElement(initWithSignal(FilterManager.SIGNAL_SELL_FULLSTO, "Full Stochartic: %K cut below %D"));
            //arr.addElement(initWithSignal(FilterManager.SIGNAL_FULLSTO_LOWER_THAN, "Oversold  Full Stochastic: %K is below 25"));
            //arr.addElement(initWithSignal(FilterManager.SIGNAL_FULLSTO_HIGHER_THAN, "Overbought Full Stochastic: %K is above 75"));
            if (type == FilterManager.SIGNAL_FULLSTO_UP_AND_HIGHER_THAN)
            {
                String s = String.Format("Full Stochastic tăng và lớn hơn {0}", param1);
                return s;
            }

            if (type == FilterManager.SIGNAL_FULLSTO_HIGHER_THAN)
            {
                String s = String.Format("Full Stochastic lớn hơn {0}", param1);
                return s;
            }

            //  RSI
            if (type == FilterManager.SIGNAL_RSI_LOWER_THAN)
            {
                String s = String.Format("RSI nhỏ hơn {0}", param1);
                if (param3 > 0)
                {
                    s = String.Format("RSI nhỏ hơn {0} ({1} ngày)", param1, param3);
                }
                return s;
            }

            if (type == FilterManager.SIGNAL_RSI_HIGHER_THAN){

                String s = String.Format("RSI lớn hơn {0}", param1);
                if (param3 > 0)
                {
                    s = String.Format("RSI lớn hơn {0} ({1} ngày)", param1, param3);
                }
                return s;
            }

            if (type == FilterManager.SIGNAL_RSI_UP_AND_HIGHER_THAN)
            {
                String s = String.Format("RSI tăng và lớn hơn {0}", param1);
                return s;
            }

            //  MFI
            if (type == FilterManager.SIGNAL_MFI_LOWER_THAN) {
                String s = String.Format("MFI is lower than {0}", param1);
                if (param3 > 0)
                {
                    s = String.Format("MFI is lower than {0} ({1} ngày)", param1, param3);
                }
                return s;
            }
            
            if (type == FilterManager.SIGNAL_MFI_HIGHER_THAN)
            {
                String s = String.Format("MFI is higher than {0}", param1);
                if (param3 > 0)
                {
                    s = String.Format("MFI is higher than {0} ({1} ngày)", param1, param3);
                }
                return s;
            }

            if (type == FilterManager.SIGNAL_MFI_UP_AND_HIGHER_THAN)
            {
                String s = String.Format("MFI tăng và lớn hơn {0}", param1);
                return s;
            }

            //  ROC
            if (type == FilterManager.SIGNAL_ROC_LOWER_THAN){
                String s = String.Format("ROC is below {0}", param1);
                return s;
            }
            if (type == FilterManager.SIGNAL_ROC_HIGHER_THAN)
            {
                String s = String.Format("ROC is above {0}", param1);
                return s;
            }

            if (type == FilterManager.SIGNAL_ROC_UP_AND_HIGHER_THAN)
            {
                String s = String.Format("ROC is up and higher than {0}", param1);
                return s;
            }
            if (type == FilterManager.SIGNAL_ROC_MA1_CUT_ABOVE_MA2)
            {
                String s = String.Format("ROC: MA({0}) cắt trên MA({1}), số ngày: {2}", (int)param1, (int)param2, (int)param3);
                return s;
            }
            
            //  WilliamR
            if (type == FilterManager.SIGNAL_WILLIAM_HIGHER_THAN)
            {
                String s = String.Format("William %R lớn hơn {0}", param1);
                return s;
            }

            if (type == FilterManager.SIGNAL_WILLIAM_LOWER_THAN)
            {
                String s = String.Format("William %R nhỏ hơn {0}", param1);
                return s;
            }

            if (type == FilterManager.SIGNAL_WILLIAM_UP_AND_HIGHER_THAN)
            {
                String s = String.Format("William %R tăng và lớn hơn {0}", param1, param2);
                return s;
            }

            if (type == FilterManager.SIGNAL_WILLIAM_DOWN_AND_LOWER_THAN)
            {
                String s = String.Format("William %R giảm và nhỏ hơn {0}", param1);
                return s;
            }

            if (type == FilterManager.SIGNAL_WILLIAM_CUT_SMA)
            {
                String s = String.Format("William %R SMA({0}) cắt trên SMA({1}) ({2} ngày)", (int)param1, (int)param2, (int)param3);
                return s;
            }
            if (type == FilterManager.SIGNAL_STOCH_RSI1 || type == FilterManager.SIGNAL_STOCH_RSI2)
            {
                int period = (int)this.param1;
                int rsi = (period >> 16) & 0xff;
                int stoch = (period >> 8) & 0xff;
                int smoothK = period & 0xff;
                int t = (int)this.param3;
                int min = t & 0xff;
                int max = (t >> 8) & 0xff;
                t = (int)this.param2;
                int smoothD = t & 0xff;

                if (rsi == 0)
                {
                    rsi = 14;
                    stoch = 14;
                    smoothK = 3;
                    smoothD = 3;
                }

                String s = String.Format("StockRSI({0}/{1}/{2}/{3}) tăng trong khoảng [{4}-{5}]", 
                   rsi, stoch, smoothK, smoothD,
                   (int)param2, (int)param3);
                return s;
            }

            if (type == FilterManager.SIGNAL_PRICE_HIGHEST_IN)
            {
                String s = String.Format("Giá cao nhất trong {0} ngày", param1);
                return s;
            }

            if (type == FilterManager.SIGNAL_TICHLUY)
            {
                return "Cổ phiếu tích luỹ";
            }
            if (type == FilterManager.SIGNAL_VOLUME)
            {
                return String.Format("Khối lượng giao dịch trung bình > {0:D}", (int)param1);
            }
            if (type == FilterManager.SIGNAL_TRADE_VALUE)
            {
                return String.Format("Giá trị giao dịch trung bình > {0:D}(triệu)", (int)param1);
            }
            if (type == FilterManager.SIGNAL_PRICE)
            {
                float value1 = (float)param1;
                float value2 = (float)param2;

                String s;
                if (value1 == 0 && value2 == 0)
                {
                    s = "Khoảng giá: 0 -> ∞";
                }
                else if (value1 == 0)
                {
                    s = String.Format("Khoảng giá: 0k -> {0:F2}k", value2);
                }
                else if (value2 == 0)
                {
                    s = String.Format("Khoảng giá:{0:F2}k -> ∞", value1);
                }
                else
                {
                    s = String.Format("Khoảng giá: {0:F2}k -> {1:F2}k", value1, value2);
                }

                return s;
            }

            return "";
        }
    }

    public class FilterSet{
        public string name;
        xVector filterItems;

        public FilterSet()
        {
            filterItems = new xVector();
        }
        public FilterSet(string _name){
            name = _name;
            filterItems = new xVector();
        }

        public void clear()
        {
            filterItems.removeAllElements();
        }

        public void addFilterItem(FilterItem item)
        {
            filterItems.addElement(item);
        }

        public int getFilterItemCnt()
        {
            return filterItems.size();
        }

        public FilterItem getFilterItemAt(int i)
        {
            if (i >= 0 && i < filterItems.size())
            {
                return (FilterItem)filterItems.elementAt(i);
            }
            return null;
        }

        public void removeFilterItem(FilterItem item)
        {
            for (int i = 0; i < filterItems.size(); i++)
            {
                FilterItem o = (FilterItem)filterItems.elementAt(i);
                if (o.type == item.type)
                {
                    filterItems.removeElementAt(i);
                    break;
                }
            }
        }
        public void saveFilterSet(xDataOutput o)
        {
            o.writeUTF(name);
            o.writeInt(filterItems.size());
            for (int i = 0; i < filterItems.size(); i++)
            {
                FilterItem item = (FilterItem)filterItems.elementAt(i);
                o.writeInt(item.type);
                o.writeInt((int)(item.param1 * 1000));
                o.writeInt((int)(item.param2 * 1000));
                o.writeInt(item.param3);
                o.writeBoolean(item.selected[0]);
            }
        }

        public void loadFilterSet717(xDataInput di)
        {
            name = di.readUTF();
            int cnt = di.readInt();
            if (filterItems == null)
            {
                filterItems = new xVector();
            }
            for (int i = 0; i < cnt; i++)
            {
                FilterItem item = new FilterItem();
                item.type = di.readInt();
                item.param1 = di.readInt() / 100.0f;
                item.param2 = di.readInt() / 100.0f;
                item.selected[0] = di.readBoolean();

                filterItems.addElement(item);
            }
        }
        public void loadFilterSet(xDataInput di)
        {
            name = di.readUTF();
            int cnt = di.readInt();
            if (filterItems == null)
            {
                filterItems = new xVector();
            }
            for (int i = 0; i < cnt; i++)
            {
                FilterItem item = new FilterItem();
                item.type = di.readInt();
                item.param1 = di.readInt() / 100.0f;
                item.param2 = di.readInt() / 100.0f;
                item.param3 = di.readInt();
                item.selected[0] = di.readBoolean();

                filterItems.addElement(item);
            }
        }

        public void loadFilterSet729(xDataInput di)
        {
            name = di.readUTF();
            int cnt = di.readInt();
            if (filterItems == null)
            {
                filterItems = new xVector();
            }
            for (int i = 0; i < cnt; i++)
            {
                FilterItem item = new FilterItem();
                item.type = di.readInt();
                item.param1 = di.readInt() / 1000.0f;
                item.param2 = di.readInt() / 1000.0f;
                item.param3 = di.readInt();
                item.selected[0] = di.readBoolean();

                filterItems.addElement(item);
            }
        }
    }
    class FilterManager
    {
        public const int SIGNAL_MACD_CUT_ABOVE_SIGNALLINE                  = 201;
        public const int SIGNAL_MACD_CUT_ABOVE_CENTERLINE                = 202;
        public const int SIGNAL_BUY_MACD_ABOVE_SIGNALLINE = 203;
        public const int SIGNAL_BUY_MACD_ABOVE_CENTERLINE = 204;
        public const int SIGNAL_ICHIMOKU_CUT_ABOVE_TENKAN_KIJUN            = 205;
        public const int SIGNAL_BUY_ICHIMOKU_GREEN_CLOUD  = 206;
        public const int SIGNAL_MA1_CUT_ABOVE_MA2                   = 210;
        public const int SIGNAL_MA1_CUT_ABOVE_MA2_2 = 211;
        public const int SIGNAL_MA1_CUT_ABOVE_MA2_3 = 212;
        public const int SIGNAL_PSAR_TURNS_GREEN                    = 215;
        public const int SIGNAL_VSTOP_TURN_GREEN                   = 216;
        public const int SIGNAL_HEIKEN_CANDLE_GREEN                   = 220;
        public const int SIGNAL_BUY_TRIX                   = 224;
        public const int SIGNAL_BUY_ICHIMOKU_PRICE_CUT_ABOVE_KIJUN    = 230;
        public const int SIGNAL_BUY_ADX_DIs_CROSS        = 231;
        public const int SIGNAL_BUY_FULLSTO        = 232;    //  from oversold -> crosses 20
        
        public const int SIGNAL_MACD_CUT_BELOW_SIGNALLINE                  = 301;
        public const int SIGNAL_MACD_CUT_BELOW_CENTERLINE                  = 302;
        public const int SIGNAL_ICHIMOKU_TENKAN_BELOW_KIJUN            = 305;
        public const int SIGNAL_SELL_ICHIMOKU_PRICE_CUT_BELOW_KIJUN      = 306;
        
        public const int SIGNAL_MA1_CUT_BELOW_MA2                   = 310;
        public const int SIGNAL_MA3_CUT_BELOW_MA4                   = 311;
        
        public const int SIGNAL_PSAR_TURNS_RED                    = 315;
        public const int SIGNAL_VSTOP_TURNS_RED                   = 316;
        public const int SIGNAL_HEIKEN_CANDLE_RED                   = 320;
        public const int SIGNAL_SELL_TRIX                   = 324;
        public const int SIGNAL_ICHIMOKU_PRICE_CUT_BELOW_KIJUN    = 330;
        public const int SIGNAL_SELL_ADX_DIs_CROSS        = 331;
        public const int SIGNAL_SELL_FULLSTO        = 332;    //  from oversold -> crosses 20
        
        //=====================================================
        public const int SIGNAL_RSI_HIGHER_THAN                 = 400;
        public const int SIGNAL_MFI_HIGHER_THAN                 = 401;
        public const int SIGNAL_ROC_HIGHER_THAN                 = 402;
        public const int SIGNAL_FULLSTO_HIGHER_THAN                 = 403;
        
        public const int SIGNAL_RSI_LOWER_THAN                 = 450;
        public const int SIGNAL_MFI_LOWER_THAN                 = 451;
        public const int SIGNAL_ROC_LOWER_THAN                 = 452;
        public const int SIGNAL_FULLSTO_LOWER_THAN             = 453;
        
        public const int SIGNAL_RSI_UP_AND_HIGHER_THAN         = 500;
        public const int SIGNAL_MFI_UP_AND_HIGHER_THAN         = 501;
        public const int SIGNAL_ROC_UP_AND_HIGHER_THAN         = 502;
        public const int SIGNAL_FULLSTO_UP_AND_HIGHER_THAN     = 503;
        public const int SIGNAL_BUY_ADX_HIGHER_THAN            = 504;
        public const int SIGNAL_ROC_MA1_CUT_ABOVE_MA2 = 505;
        //public const int SIGNAL_FULLSTO_HIGHER_THAN = 505;
        
        public const int SIGNAL_WILLIAM_UP_AND_HIGHER_THAN     = 520;
        public const int SIGNAL_WILLIAM_DOWN_AND_LOWER_THAN     = 521;
        public const int SIGNAL_WILLIAM_HIGHER_THAN     = 522;
        public const int SIGNAL_WILLIAM_LOWER_THAN     = 523;

        public const int SIGNAL_WILLIAM_CUT_SMA = 524;

        public const int SIGNAL_STOCH_RSI1 = 525;
        public const int SIGNAL_STOCH_RSI2 = 526;
        
        //public const int SIGNAL_PRICE_CROSS_UP        = 520;
        //public const int SIGNAL_PRICE_CROSS_DOWN      = 521;
        
        public const int SIGNAL_BREAOUT_UP           = 530;
        public const int SIGNAL_BREAOUT_WEDGE           = 531;

        public const int SIGNAL_VOLUME      = 540;
        public const int SIGNAL_TRADE_VALUE      = 541;
        public const int SIGNAL_PRICE      = 542;

        public const int SIGNAL_RS = 550;
        public const int SIGNAL_RS_PERFORMANCE = 551;
        public const int SIGNAL_VOLUME_IS_OUT = 553;

        public const int SIGNAL_PRICE_FLUCTUATION = 559;    //  *
        public const int SIGNAL_TOP_PRICE_DOWN = 560;
        public const int SIGNAL_TOP_PRICE_UP = 580;
        
        public const int SIGNAL_VOLUME_IS_UP                 = 800;
        public const int SIGNAL_VOLUME_IS_DOWN               = 801;
        
        public const int SIGNAL_ADX_DIPLUS_ABOVE_DIMINUS     = 820;

        public const int SIGNAL_PRICE_HIGHEST_IN = 830;

        public const int SIGNAL_TICHLUY = 1001;

        xVector mFilterSets = new xVector();
        const string FILE_FILTER = "filter.dat";
        public const int FILTER_VERSION_717 = 3;
        public const int FILTER_VERSION_718 = 4;
        public const int FILTER_VERSION_729 = 5;

        static FilterManager instance;
        public static FilterManager getInstance()
        {
            if (instance == null)
            {
                instance = new FilterManager();
                instance.loadFilterSets(null);
            }
            return instance;
        }

        public void addFilterSet(FilterSet filterSet)
        {
            if (!mFilterSets.contains(filterSet))
            {
                mFilterSets.addElement(filterSet);
            }
        }

        public void removeFilterSet(FilterSet filterSet)
        {
            mFilterSets.removeElement(filterSet);
        }

        int filterVersion = 0;
        public bool loadFilterSets(xDataInput di)
        {
            mFilterSets.removeAllElements();

            if (di != null)
            {
                int ver = di.readInt();
                if (ver == FILTER_VERSION_717)
                {
                    int cnt = di.readInt();
                    for (int i = 0; i < cnt; i++)
                    {
                        FilterSet filter = new FilterSet();
                        filter.loadFilterSet717(di);
                        mFilterSets.addElement(filter);
                    }

                    filterVersion = di.readInt();
                }
                else if (ver == FILTER_VERSION_718)
                {
                    int cnt = di.readInt();
                    for (int i = 0; i < cnt; i++)
                    {
                        FilterSet filter = new FilterSet();
                        filter.loadFilterSet(di);
                        mFilterSets.addElement(filter);
                    }

                    filterVersion = di.readInt();
                }
                else if (ver == FILTER_VERSION_729)
                {
                    int cnt = di.readInt();
                    for (int i = 0; i < cnt; i++)
                    {
                        FilterSet filter = new FilterSet();
                        filter.loadFilterSet729(di);
                        mFilterSets.addElement(filter);
                    }

                    filterVersion = di.readInt();
                }
                else if (ver >= 0x10203040)
                {
                    return false;
                }
            }

            if (filterVersion != 0x00110011)
            {
                //  create default
                FilterSet filterSet = new FilterSet();
                FilterItem item;

                if (!isContains(SIGNAL_MACD_CUT_ABOVE_SIGNALLINE))
                {
                    filterSet.name = "MACD cắt đường tín hiệu";
                    filterSet.addFilterItem(initWithSignal(SIGNAL_MACD_CUT_ABOVE_SIGNALLINE, ""));
                    mFilterSets.addElement(filterSet);
                }

                //  set 2
                if (!isContains(SIGNAL_VOLUME_IS_UP))
                {
                    filterSet = new FilterSet("Volume tăng");
                    filterSet.addFilterItem(initWithSignal(SIGNAL_VOLUME_IS_UP, ""));
                    mFilterSets.addElement(filterSet);
                }
                
                //  SMA
                filterSet = new FilterSet("SMA 5 nằm trên SMA 22");
                item = initWithSignal(SIGNAL_MA1_CUT_ABOVE_MA2, "");
                item.param1 = 5;
                item.param2 = 22;
                item.param3 = 3;
                filterSet.addFilterItem(item);
                mFilterSets.addElement(filterSet);

                //  set 3
                if (!isContains(SIGNAL_PSAR_TURNS_GREEN))
                {
                    filterSet = new FilterSet("Parabolic PSAR xanh");
                    filterSet.addFilterItem(initWithSignal(SIGNAL_PSAR_TURNS_GREEN, ""));
                    mFilterSets.addElement(filterSet);
                }

                //  set 4
                filterSet = new FilterSet("RSI cao hơn");
                item = initWithSignal(SIGNAL_RSI_HIGHER_THAN, "");
                item.param1 = 50;
                item.param3 = 3;
                filterSet.addFilterItem(item);
                mFilterSets.addElement(filterSet);

                filterSet = new FilterSet("RSI thấp hơn");
                item = initWithSignal(SIGNAL_RSI_LOWER_THAN, "");
                item.param1 = 30;
                item.param3 = 3;
                filterSet.addFilterItem(item);
                mFilterSets.addElement(filterSet);

                //  set 5
                filterSet = new FilterSet("MFI cao hơn");
                item = initWithSignal(SIGNAL_MFI_HIGHER_THAN, "");
                item.param1 = 50;
                item.param3 = 3;
                filterSet.addFilterItem(item);
                mFilterSets.addElement(filterSet);

                filterSet = new FilterSet("MFI thấp hơn");
                item = initWithSignal(SIGNAL_MFI_LOWER_THAN, "");
                item.param1 = 30;
                item.param3 = 3;
                filterSet.addFilterItem(item);
                mFilterSets.addElement(filterSet);
            }

            return true;
        }

        bool isContains(int signal)
        {
            for (int i = 0; i < mFilterSets.size(); i++)
            {
                FilterSet filterSet = (FilterSet)mFilterSets.elementAt(i);
                for (int j = 0; j < filterSet.getFilterItemCnt(); j++)
                {
                    FilterItem item = (FilterItem)filterSet.getFilterItemAt(j);
                    if (item.type == signal)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void saveFilterSets(xDataOutput o)
        {
            o.writeInt(FILTER_VERSION_729);
            o.writeInt(mFilterSets.size());

            for (int i = 0; i < mFilterSets.size(); i++)
            {
                FilterSet filter = (FilterSet)mFilterSets.elementAt(i);
                filter.saveFilterSet(o);
            }

            //  filter version
            o.writeInt(0x00110011);

        }

        public int getFilterSetCnt()
        {
            return mFilterSets.size();
        }

        public FilterSet getFilterSetAt(int i)
        {
            if (i >= 0 && i < mFilterSets.size())
            {
                return (FilterSet)mFilterSets.elementAt(i);
            }

            return null;
        }

        //=======================================================
        static FilterItem initWithSignal(int type, string name)
        {
            FilterItem item = new FilterItem();
            item.type = type;
            item.name = name;
            item.hasSetting = false;
            return item;
        }

        static FilterItem initWithSignal(int type, string name, bool hasSetting)
        {
            FilterItem item = new FilterItem();
            item.type = type;
            item.name = name;
            item.hasSetting = hasSetting;
            return item;
        }

        static public xVector getAvailableSignalItemsAndFilterSet(FilterSet filterSet)
        {
            xVector v = getAvailableSignalItems();
            for (int i = 0; i < v.size(); i++)
            {
                FilterItem item = (FilterItem)v.elementAt(i);
                for (int j = 0; j < filterSet.getFilterItemCnt(); j++)
                {
                    FilterItem item2 = filterSet.getFilterItemAt(j);
                    if (item.type == item2.type)
                    {
                        item.param1 = item2.param1;
                        item.param2 = item2.param2;
                        item.param3 = item2.param3;
                        item.selected[0] = true;
                    }
                }
            }

            return v;
        }

        static public xVector getAvailableSignalItems()
        {
            xVector arr = new xVector();
            //  Breakout
            //if (Context::getInstance()->isThuyPS()){
                //arr.addElement(initWithSignal(SIGNAL_BREAOUT_UP, "Breakout"));
                //arr.addElement(initWithSignal(SIGNAL_BREAOUT_WEDGE, "Breakout Wedge"));
            //}

            //  volume
            FilterItem item = initWithSignal(SIGNAL_VOLUME, null, true);
            item.param1 = 5000;
            arr.addElement(item);

            //  trade value (trieu)
            item = initWithSignal(SIGNAL_TRADE_VALUE, null, true);
            item.param1 = 50;
            arr.addElement(item);

            //  khoang gia
            item = initWithSignal(SIGNAL_PRICE, null, true);
            item.param1 = 1;
            item.param2 = 0;
            arr.addElement(item);

            //  volume up/down
            arr.addElement(initWithSignal(SIGNAL_VOLUME_IS_UP, "Khối lượng tăng"));
            arr.addElement(initWithSignal(SIGNAL_VOLUME_IS_DOWN, "Khối lượng giảm"));
            arr.addElement(initWithSignal(SIGNAL_VOLUME_IS_OUT, "Khối lượng cạn kiệt"));

            //  gia tang trong khoang
            item = initWithSignal(SIGNAL_TOP_PRICE_UP, "", true);
            item.param1 = 10;
            item.param2 = 50;
            item.param3 = 30;
            arr.addElement(item);

            //  gia giảm trong khoảng
            item = initWithSignal(SIGNAL_TOP_PRICE_DOWN, "", true);
            item.param1 = 10;
            item.param2 = 30;
            item.param3 = 30;
            arr.addElement(item);

            //  dao dong
            item = initWithSignal(SIGNAL_PRICE_FLUCTUATION, "", true);
            item.param1 = 10;
            item.param2 = 15;
            item.param3 = 10;
            arr.addElement(item);

            //  MACD
            arr.addElement(initWithSignal(SIGNAL_MACD_CUT_ABOVE_SIGNALLINE, "MACD cắt + vượt trên Signal"));
            arr.addElement(initWithSignal(SIGNAL_BUY_MACD_ABOVE_SIGNALLINE, "MACD trên Signal"));
            arr.addElement(initWithSignal(SIGNAL_MACD_CUT_ABOVE_CENTERLINE, "MACD cắt + vượt trên Center line"));
            arr.addElement(initWithSignal(SIGNAL_BUY_MACD_ABOVE_CENTERLINE, "MACD trên Center line"));
            
            //  MACD
            arr.addElement(initWithSignal(SIGNAL_MACD_CUT_BELOW_SIGNALLINE, "MACD nhỏ hơn Signal"));
            arr.addElement(initWithSignal(SIGNAL_MACD_CUT_BELOW_CENTERLINE, "MACD nhỏ hơn Center line"));
            
            
            //  Ichimoku
            arr.addElement(initWithSignal(SIGNAL_ICHIMOKU_CUT_ABOVE_TENKAN_KIJUN, "Tenkan-sen (Conv) cắt + vượt trên Kijun-sen (Base)"));
            arr.addElement(initWithSignal(SIGNAL_BUY_ICHIMOKU_PRICE_CUT_ABOVE_KIJUN, "Giá cắt + vượt trên Kijun-sen (Base)"));
            arr.addElement(initWithSignal(SIGNAL_BUY_ICHIMOKU_GREEN_CLOUD, "Ichimoku: Green Cloud"));
            
            //  Ichimoku
            arr.addElement(initWithSignal(SIGNAL_ICHIMOKU_TENKAN_BELOW_KIJUN, "Tenkan-sen (Conv) nằm dưới Kijun-sen (Base)."));
            arr.addElement(initWithSignal(SIGNAL_ICHIMOKU_PRICE_CUT_BELOW_KIJUN, "Giá nằm dưới Kijun-sen (Base)."));
            
            
            //  PSAR & VSTOP
            item = initWithSignal(SIGNAL_PSAR_TURNS_GREEN, "PSAR xanh", true);
            item.param1 = 0.02f;
            item.param2 = 0.2f;
            item.param3 = 0;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_PSAR_TURNS_RED, "PSAR đỏ", true);
            item.param1 = 0.02f;
            item.param2 = 0.2f;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_VSTOP_TURN_GREEN, "VStop xanh", true);
            item.param1 = 20;
            item.param2 = 0.2f;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_VSTOP_TURNS_RED, "VSTOP đỏ", true);
            item.param1 = 20;
            item.param2 = 0.2f;
            arr.addElement(item);
            
            
            //  Moving average
            item = initWithSignal(SIGNAL_MA1_CUT_ABOVE_MA2, "MA 5 nằm trên MA 22", true);
            item.param1 = 5;
            item.param2 = 22;
            item.param3 = 3;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_MA1_CUT_ABOVE_MA2_2, "MA 20 nằm trên MA 100", true);
            item.param1 = 20;
            item.param2 = 100;
            item.param3 = 9;
            arr.addElement(item);
            item = initWithSignal(SIGNAL_MA1_CUT_ABOVE_MA2_3, "MA 20 nằm trên MA 100", true);
            item.param1 = 20;
            item.param2 = 100;
            item.param3 = 9;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_MA1_CUT_BELOW_MA2, "MA 5 nằm dưới MA 22", true);
            item.param1 = 5;
            item.param2 = 22;
            item.param3 = 3;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_MA3_CUT_BELOW_MA4, "MA 20 nằm dưới MA 100", true);
            item.param1 = 20;
            item.param2 = 100;
            item.param3 = 9;
            arr.addElement(item);

            
            arr.addElement(initWithSignal(SIGNAL_HEIKEN_CANDLE_GREEN, "Heiken xanh"));
            arr.addElement(initWithSignal(SIGNAL_HEIKEN_CANDLE_RED, "Heiken đỏ"));
            
            //  TRIX
            //arr.addElement(initWithSignal(SIGNAL_BUY_TRIX, "TRIX buy"));
            //arr.addElement(initWithSignal(SIGNAL_SELL_TRIX, "TRIX sign"));
            
            //  ADX
            item = initWithSignal(SIGNAL_BUY_ADX_DIs_CROSS, "ADX: DI+ cắt + vượt trên DI-", true);
            item.param1 = 3;
            arr.addElement(item);


            arr.addElement(initWithSignal(SIGNAL_SELL_ADX_DIs_CROSS, "ADX: DI+ nằm dưới DI-"));
            arr.addElement(initWithSignal(SIGNAL_ADX_DIPLUS_ABOVE_DIMINUS, "ADX: DI+ nằm trên DI-"));

            item = initWithSignal(SIGNAL_BUY_ADX_HIGHER_THAN, "ADX lớn hơn 20", true);
            item.param1 = 20;
            arr.addElement(item);

            //  Stochartics
            arr.addElement(initWithSignal(SIGNAL_BUY_FULLSTO, "Full Stochartic: %K cắt + vượt trên %D"));
            //arr.addElement(initWithSignal(SIGNAL_SELL_FULLSTO, "Full Stochartic: %K cut below %D"));
            //arr.addElement(initWithSignal(SIGNAL_FULLSTO_LOWER_THAN, "Oversold  Full Stochastic: %K is below 25"));
            //arr.addElement(initWithSignal(SIGNAL_FULLSTO_HIGHER_THAN, "Overbought Full Stochastic: %K is above 75"));
            item = initWithSignal(SIGNAL_FULLSTO_UP_AND_HIGHER_THAN, "Full Stochastic tăng và lớn hơn 25", true);
            item.param1 = 25;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_FULLSTO_HIGHER_THAN, "Full Stochastic lớn hơn 25", true);
            item.param1 = 25;
            arr.addElement(item);
            
            //  RSI
            item = initWithSignal(SIGNAL_RSI_LOWER_THAN, "RSI nhỏ hơn 30", true);
            item.param1 = 30;
            item.param3 = 3;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_RSI_HIGHER_THAN, "RSI lớn hơn 70", true);
            item.param1 = 50;
            item.param3 = 3;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_RSI_UP_AND_HIGHER_THAN, "RSI tăng và lớn hơn 50", true);
            item.param1 = 50;
            arr.addElement(item);
            
            //  MFI
            item = initWithSignal(SIGNAL_MFI_LOWER_THAN, "MFI is lower than", true);
            item.param1 = 30;
            item.param3 = 3;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_MFI_HIGHER_THAN, "MFI is higher than", true);
            item.param1 = 50;
            item.param3 = 3;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_MFI_UP_AND_HIGHER_THAN, "MFI tăng và lớn hơn", true);
            item.param1 = 50;
            arr.addElement(item);
            
            //  ROC
            arr.addElement(initWithSignal(SIGNAL_ROC_LOWER_THAN, "ROC is below -10"));
            arr.addElement(initWithSignal(SIGNAL_ROC_HIGHER_THAN, "ROC is above 10"));

            item = initWithSignal(SIGNAL_ROC_UP_AND_HIGHER_THAN, "ROC is up and higher than", true);
            item.param1 = 10;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_ROC_MA1_CUT_ABOVE_MA2, "ROC Ma1 x Ma2", true);
            item.param1 = 3;
            item.param2 = 14;
            item.param3 = 10;
            arr.addElement(item);

            //  RSI 1
            item = initWithSignal(SIGNAL_STOCH_RSI1, "", true);
            item.param1 = (14<<16)|(14<<16)|3;    //  period
            item.param2 = 3;
            item.param3 = (80<<8)|10;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_STOCH_RSI2, "", true);
            item.param1 = (14 << 16) | (14 << 16) | 3;    //  period
            item.param2 = 3;
            item.param3 = (80 << 8) | 10;
            arr.addElement(item);
            
            //  WilliamR
            item = initWithSignal(SIGNAL_WILLIAM_HIGHER_THAN, "William %R lớn hơn", true);
            item.param1 = -20;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_WILLIAM_LOWER_THAN, "William %R nhỏ hơn", true);
            item.param1 = -80;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_WILLIAM_UP_AND_HIGHER_THAN, "William %R tăng và lớn hơn", true);
            item.param1 = -50;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_WILLIAM_DOWN_AND_LOWER_THAN, "William %R giảm và nhỏ hơn", true);
            item.param1 = -50;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_WILLIAM_CUT_SMA, "", true);
            item.param1 = 3;
            item.param2 = 10;
            item.param3 = 3;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_PRICE_HIGHEST_IN, "", true);
            item.param1 = 6 * 22;
            arr.addElement(item);

            //  Relative strength
            item = initWithSignal(SIGNAL_RS, null, true);
            item.param1 = 3;
            item.param2 = 15;
            item.param3 = 5;
            arr.addElement(item);

            /*
            //  RS performance
            item = initWithSignal(SIGNAL_RS_PERFORMANCE, null, true);
            item.param1 = 3;
            item.param2 = 3;
            //item.param2 = 15;
            arr.addElement(item);
            */
            return arr;
        }

        public bool hasSignal(FilterItem filter, Share share)
        {
            if (share.mCode.CompareTo("ACB") == 0)
            {
                Utils.trace("---------");
                //share.clearCalculations();
            }

            share.clearCalculations();
            if (share.getCandleCnt() < 12)
            {
                return false;
            }

            int last = share.getCandleCnt() - 1;
            int type = filter.type;
            bool res = false;
            int days = filter.param3;
            switch (type)
            {
                case SIGNAL_MACD_CUT_ABOVE_SIGNALLINE:
                    share.calcMACD();
                    res = share.isMACDCutSignalAbove(9);// Share.pMACD[last] > Share.pMACDSignal9[last];
                    break;
                case SIGNAL_MACD_CUT_ABOVE_CENTERLINE:
                    share.calcMACD();
                    res = share.hasIntersesionAbove(share.pMACD, null, 9);
                    break;
                case SIGNAL_BUY_MACD_ABOVE_SIGNALLINE:
                    share.calcMACD();
                    res = share.pMACD[last] > share.pMACDSignal9[last];
                    break;
                case SIGNAL_BUY_MACD_ABOVE_CENTERLINE:
                    share.calcMACD();
                    res = share.pMACD[last] > 0;
                    break;
                case SIGNAL_ICHIMOKU_CUT_ABOVE_TENKAN_KIJUN:
                    share.calcIchimoku();
                    res = share.hasIntersesionAbove(share.pTenkansen, share.pKijunsen, 9);
                    break;
                case SIGNAL_BUY_ICHIMOKU_GREEN_CLOUD:
                    share.calcIchimoku();
                    res = share.pSpanA[last] > share.pSpanB[last];
                    break;
                case SIGNAL_MA1_CUT_ABOVE_MA2:
                case SIGNAL_MA1_CUT_ABOVE_MA2_2:
                case SIGNAL_MA1_CUT_ABOVE_MA2_3:
                    {
                        if (filter.param1 > 0 && filter.param2 > 0)
                        {
                            share.calcSMA((int)filter.param1, (int)filter.param2, 0, 0, 0);
                            if (share.pSMA1[last] > share.pSMA2[last] && days > 0)
                            {
                                int begin = share.getCandleCount() - days - 1;
                                if (begin < 0)
                                {
                                    begin = 0;
                                }
                                bool ok = false;
                                for (int i = share.getCandleCount() - 1; i >= begin; i--)
                                {
                                    if (share.pSMA1[i] <= share.pSMA2[i])
                                    {
                                        ok = true;
                                        break;
                                    }
                                }
                                return ok;
                            }

                            return share.pSMA1[last] > share.pSMA2[last];
                        }
                    }
                    return false;
                case SIGNAL_MA1_CUT_BELOW_MA2:
                case SIGNAL_MA3_CUT_BELOW_MA4:
                    {
                        if (filter.param1 > 0 && filter.param2 > 0)
                        {
                            share.calcSMA((int)filter.param1, (int)filter.param2, 0, 0, 0);
                            if (share.pSMA1[last] < share.pSMA2[last] && days > 0)
                            {
                                int begin = share.getCandleCount() - days - 1;
                                if (begin < 0)
                                {
                                    begin = 0;
                                }
                                bool ok = false;
                                for (int i = share.getCandleCount() - 1; i >= begin; i--)
                                {
                                    if (share.pSMA1[i] >= share.pSMA2[i])
                                    {
                                        ok = true;
                                        break;
                                    }
                                }
                                return ok;
                            }

                            return share.pSMA1[last] < share.pSMA2[last];
                        }
                    }
                    return false;

                case SIGNAL_PRICE_FLUCTUATION:
                    {
                        /*
                        int start = share.getCandleCnt()-1-filter.param3;
                        int end = share.getCandleCnt()-1;

                        if (start < 0) start = 0;

                        bool ok = true;
                        float hi = 0;
                        float low = 0;
                        for (int i = start; i <= end; i++)
                        {
                            float c = share.getClose(i);
                            if (hi == 0) hi = c;
                            if (low == 0) low = c;

                            if (hi < c) hi = c;
                            if (low > c) low = c;
                        }

                        if (low > 0)
                        {
                            float percents = (hi - low)*100 / low;
                            float min = filter.param1;
                            float max = filter.param2;

                            if (percents >= min && percents <= max)
                            {
                                return true;
                            }
                        }
                         */
                        float percents = filter.param2;
                        int cnt = share.getCandleCnt();
                        int begin = cnt - days;
                        if (begin < 0) { begin = 0; }

                        int totalCandles = cnt - begin;
                        if (totalCandles > 0)
                        {
                            //  trung binh gia'
                            double avgPrice = 0;
                            double totalValue = 0;
                            double totalVolume = 0;

                            for (int i = begin; i < cnt; i++)
                            {
                                totalValue += share.getClose(i) * share.getVolume(i);
                                totalVolume += share.getVolume(i);
                            }
                            if (totalVolume > 0)
                            {
                                avgPrice = totalValue / totalVolume;
                            }

                            //  check thrust point
                            float halfPercents = percents / 2.0f;
                            double thrustTop = avgPrice + avgPrice * (halfPercents + 8) / 100.0f;
                            double thrustBottom = avgPrice - avgPrice * (halfPercents + 8) / 100.0f;

                            double upper = avgPrice + avgPrice * (halfPercents + 0) / 100.0f;
                            double lower = avgPrice - avgPrice * (halfPercents + 0) / 100.0f;


                            bool ok = true;
                            int testCount = 0;
                            for (int i = begin + 1; i < cnt - 1; i++)
                            {
                                float close = share.getClose(i);

                                if (close > thrustBottom && close < thrustTop)
                                {
                                    if (close >= lower && close <= upper)
                                    {
                                        //  totally agree this point
                                    }
                                    else
                                    {
                                        float c0 = share.getClose(i - 1);
                                        float c1 = c0;
                                        if (i < cnt - 2) { c1 = share.getClose(i + 1); }
                                        if (close > upper)
                                        {
                                            if (c0 > upper || c1 > upper)
                                            {
                                                //  reject this point
                                                ok = false;
                                            }
                                            else
                                            {
                                                //  this is just a test point
                                                testCount++;
                                            }
                                        }
                                        else if (close < lower)
                                        {
                                            if (c0 < lower || c1 < lower)
                                            {
                                                //  reject this point
                                                ok = false;
                                            }
                                            else
                                            {
                                                //  this is just a test point
                                                testCount++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ok = false;
                                }

                                if (!ok)
                                {
                                    break;
                                }
                            }
                            return ok && (testCount < days / 4);
                        }
                    }
                    break;
                case SIGNAL_TOP_PRICE_UP:
                    {
                        days = filter.param3;
                        int minPercent = (int)filter.param1;
                        int maxPercent = (int)filter.param2;

                        share.mSortParam = (int)share.getChangedInPercent(1, days);

                        return share.mSortParam >= minPercent && share.mSortParam < maxPercent;
                    }
                    break;
                case SIGNAL_TOP_PRICE_DOWN:
                    {
                        days = filter.param3;
                        int minPercent = (int)filter.param1;
                        int maxPercent = (int)filter.param2;

                        share.mSortParam = (int)share.getChangedInPercent(-1, days);

                        return share.mSortParam >= minPercent && share.mSortParam < maxPercent;
                    }
                    break;

                case SIGNAL_PSAR_TURNS_GREEN:
                    {
                        days = (int)filter.param3;
                        share.calcPSAR(filter.param1, filter.param2);
                        res = share.pSAR_SignalUp[last];

                        if (res && days > 0)
                        {
                            int b = last - days;
                            if (b < 0) b = 0;
                            res = false;
                            for (int i = last - 1; i >= b; i--)
                            {
                                if (share.pSAR_SignalUp[i] == false)
                                {
                                    res = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case SIGNAL_VSTOP_TURN_GREEN:
                    {
                        days = (int)filter.param3;
                        share.calcVSTOP((int)filter.param1, filter.param2);
                        res = share.pVSTOP_SignalUp[last];

                        if (res && days > 0)
                        {
                            int b = last - days;
                            if (b < 0) b = 0;
                            res = false;
                            for (int i = last - 1; i >= b; i--)
                            {
                                if (share.pVSTOP_SignalUp[i] == false)
                                {
                                    res = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case SIGNAL_HEIKEN_CANDLE_GREEN:
                    {
                        float[] heiken = share.pTMP;
                        int max = share.getCandleCnt() > 30 ? 30 : share.getCandleCnt();
                        share.calcHeiken(max, heiken);

                        return heiken[max - 1] > 0;
                    }
                    break;
                case SIGNAL_BUY_TRIX:
                    break;
                case SIGNAL_BUY_ICHIMOKU_PRICE_CUT_ABOVE_KIJUN:
                    share.calcIchimoku();

                    res = share.hasIntersesionAbove(share.mCClose, share.pKijunsen, 9);
                    break;
                case SIGNAL_BUY_ADX_DIs_CROSS:
                    share.calcADX();

                    if (filter.param1 > 20 || filter.param1 < 0)
                    {
                        filter.param1 = 0;
                    }

                    res = share.hasIntersesionAbove(share.pPLUS_DI, share.pMINUS_DI, (int)filter.param1);
                    break;
                case SIGNAL_BUY_FULLSTO:
                    share.calcStochasticFull();
                    res = share.hasIntersesionAbove(share.pStochasticSlowK, share.pStochasticSlowD, 9);
                    break;

                case SIGNAL_MACD_CUT_BELOW_SIGNALLINE:
                    share.calcMACD();
                    res = share.pMACD[last] < share.pMACDSignal9[last];
                    break;
                case SIGNAL_MACD_CUT_BELOW_CENTERLINE:
                    share.calcMACD();
                    res = share.pMACD[last] < 0;
                    break;
                case SIGNAL_ICHIMOKU_TENKAN_BELOW_KIJUN:
                    share.calcIchimoku();
                    res = share.pTenkansen[last] < share.pKijunsen[last];
                    break;
                case SIGNAL_SELL_ICHIMOKU_PRICE_CUT_BELOW_KIJUN:
                    break;

                case SIGNAL_PSAR_TURNS_RED:
                    {
                        days = (int)filter.param3;
                        share.calcPSAR(filter.param1, filter.param2);
                        res = share.pSAR_SignalUp[last] == false;

                        if (res && days > 0)
                        {
                            int b = last - days;
                            if (b < 0) b = 0;
                            res = false;
                            for (int i = last - 1; i >= b; i--)
                            {
                                if (share.pSAR_SignalUp[i] == true)
                                {
                                    res = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case SIGNAL_VSTOP_TURNS_RED:
                    {
                        days = (int)filter.param3;
                        share.calcVSTOP((int)filter.param1, filter.param2);
                        res = share.pVSTOP_SignalUp[last] == false;

                        if (res && days > 0)
                        {
                            int b = last - days;
                            if (b < 0) b = 0;
                            res = false;
                            for (int i = last - 1; i >= b; i--)
                            {
                                if (share.pVSTOP_SignalUp[i] == true)
                                {
                                    res = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case SIGNAL_HEIKEN_CANDLE_RED:
                    {
                        float[] heiken = share.pTMP;
                        int max = share.getCandleCnt() > 30 ? 30 : share.getCandleCnt();
                        share.calcHeiken(max, heiken);

                        return heiken[max - 1] < 0;
                    }

                    break;
                case SIGNAL_SELL_TRIX:
                    break;
                case SIGNAL_ICHIMOKU_PRICE_CUT_BELOW_KIJUN:
                    share.calcIchimoku();
                    res = share.mCClose[last] < share.pKijunsen[last];
                    break;
                case SIGNAL_SELL_ADX_DIs_CROSS:
                    share.calcADX();
                    res = share.pPLUS_DI[last] < share.pMINUS_DI[last];
                    break;
                case SIGNAL_SELL_FULLSTO:
                    share.calcStochasticFull();

                    res = share.pStochasticSlowK[last] < share.pStochasticSlowD[last];
                    break;

                //=====================================================
                case SIGNAL_RSI_HIGHER_THAN:
                    {
                        share.calcRSI(0);
                        float value = filter.param1 / 1.0f;

                        if (share.pRSI[last] >= value && days > 0)
                        {
                            int begin = share.getCandleCount() - days - 1;
                            if (begin < 0)
                            {
                                begin = 0;
                            }
                            bool ok = false;
                            for (int i = share.getCandleCount() - 1; i >= begin; i--)
                            {
                                if (share.pRSI[i] < value)
                                {
                                    ok = true;
                                    break;
                                }
                            }
                            return ok;
                        }

                        return share.pRSI[last] >= value;
                    }
                case SIGNAL_RSI_LOWER_THAN:
                    {
                        share.calcRSI(0);
                        float value = filter.param1 / 1.0f;

                        if (share.pRSI[last] <= value && days > 0)
                        {
                            int begin = share.getCandleCount() - days - 1;
                            if (begin < 0)
                            {
                                begin = 0;
                            }
                            bool ok = false;
                            for (int i = share.getCandleCount() - 1; i >= begin; i--)
                            {
                                if (share.pRSI[i] > value)
                                {
                                    ok = true;
                                    break;
                                }
                            }
                            return ok;
                        }

                        return share.pRSI[last] <= value;
                    }
                case SIGNAL_MFI_HIGHER_THAN:
                    {
                        share.calcMFI(0);
                        float value = filter.param1 / 1.0f;

                        if (share.pMFI[last] >= value && days > 0)
                        {
                            int begin = share.getCandleCount() - days - 1;
                            if (begin < 0)
                            {
                                begin = 0;
                            }
                            bool ok = false;
                            for (int i = share.getCandleCount() - 1; i >= begin; i--)
                            {
                                if (share.pMFI[i] < value)
                                {
                                    ok = true;
                                    break;
                                }
                            }
                            return ok;
                        }

                        return share.pMFI[last] >= value;
                    }

                case SIGNAL_MFI_LOWER_THAN:
                    {
                        share.calcMFI(0);
                        float value = filter.param1 / 1.0f;

                        if (share.pMFI[last] <= value && days > 0)
                        {
                            int begin = share.getCandleCount() - days - 1;
                            if (begin < 0)
                            {
                                begin = 0;
                            }
                            bool ok = false;
                            for (int i = share.getCandleCount() - 1; i >= begin; i--)
                            {
                                if (share.pMFI[i] > value)
                                {
                                    ok = true;
                                    break;
                                }
                            }
                            return ok;
                        }

                        return share.pMFI[last] <= value;
                    }
                case SIGNAL_ROC_HIGHER_THAN:
                    share.calcROC(0);
                    res = share.pROC[last] >= filter.param1 / 1.0f;
                    break;
                case SIGNAL_FULLSTO_HIGHER_THAN:
                    share.calcStochasticFull();
                    res = share.pStochasticSlowK[last] >= filter.param1 / 1.0f;
                    break;

                
                case SIGNAL_ROC_LOWER_THAN:
                    share.calcROC(0);
                    res = share.pROC[last] <= filter.param1 / 1.0f;
                    break;
                case SIGNAL_FULLSTO_LOWER_THAN:
                    share.calcStochasticFull();
                    res = share.pStochasticSlowK[last] <= filter.param1 / 1.0f;
                    break;

                case SIGNAL_RSI_UP_AND_HIGHER_THAN:
                    share.calcRSI(0);
                    if (share.pRSI[last] >= filter.param1)
                    {
                        res = share.isUpTrend(share.pRSI);
                    }
                    break;
                case SIGNAL_MFI_UP_AND_HIGHER_THAN:
                    share.calcMFI(0);
                    if (share.pMFI[last] >= filter.param1)
                    {
                        res = share.isUpTrend(share.pMFI);
                    }
                    break;
                case SIGNAL_ROC_UP_AND_HIGHER_THAN:
                    share.calcROC(0);
                    if (share.pROC[last] >= filter.param1)
                    {
                        res = share.isUpTrend(share.pROC);
                    }
                    break;
                case SIGNAL_ROC_MA1_CUT_ABOVE_MA2:
                    if (filter.param1 > 0 && filter.param2 > 0)
                    {
                        share.calcROC(0);
                        //if (share.mCode.CompareTo("SSB") == 0)
                        //{/
                            //Utils.trace("a");
                        //}

                        int items = (int)Math.Max(2*filter.param1, 2*filter.param2);
                        int stepBack = (int)filter.param3;
                        last = items - 1;
                        int off = share.getCandleCnt() - items;
                        if (off < 0)
                        {
                            return false;
                        }
                        try
                        {
                            Share.EMA(share.pROC, off, items, (int)filter.param1, Share.pStaticTMP1);
                            Share.EMA(share.pROC, off, items, (int)filter.param2, Share.pStaticTMP2);
                        }
                        catch (Exception e2)
                        {
                            Utils.trace(e2.ToString());
                        }
                        if (Share.pStaticTMP1[last] > Share.pStaticTMP2[last])
                        {
                            for (int i = 1; i < stepBack; i++)
                            {
                                if (Share.pStaticTMP1[last - i] < Share.pStaticTMP2[last - i])
                                {
                                    res = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case SIGNAL_FULLSTO_UP_AND_HIGHER_THAN:
                    share.calcStochasticFull();
                    res = share.pStochasticSlowK[last] <= filter.param1 / 1.0f;
                    if (res)
                    {
                        res = share.isUpTrend(share.pStochasticSlowK);
                    }
                    break;
                case SIGNAL_BUY_ADX_HIGHER_THAN:
                    share.calcADX();
                    res = share.pADX[last] >= filter.param1;
                    break;
                //public const int SIGNAL_FULLSTO_HIGHER_THAN = 505;

                case SIGNAL_WILLIAM_UP_AND_HIGHER_THAN:
                    share.calcWilliamR();
                    res = share.pWilliamR[last] >= filter.param1;
                    if (res)
                    {
                        res = share.isUpTrend(share.pWilliamR);
                    }
                    break;
                case SIGNAL_STOCH_RSI1:
                case SIGNAL_STOCH_RSI2:
                    {
                        if (share.mCode.CompareTo("IDJ") == 0)
                        {
                            Utils.trace("aaaa");
                        }
                        int period = (int)filter.param1;
                        int rsi = (period >> 16) & 0xff;
                        int stoch = (period >> 8) & 0xff;
                        int smoothK = period & 0xff;
                        int t = (int)filter.param3;
                        int min = t & 0xff;
                        int max = (t >> 8) & 0xff;
                        t = (int)filter.param2;
                        int smoothD = t & 0xff;

                        if (min > max || min < 0 || max < 0 || min > 100 || max > 100)
                        {
                            min = 0;
                            max = 100;
                        }

                        share.calcStochRSI(rsi, stoch, smoothK, smoothD, Share.pStaticTMP1, Share.pStaticTMP2);

                        float value = Share.pStaticTMP1[last];
                        float valueD = Share.pStaticTMP2[last];

                        if (value >= valueD && value >= min && value <= max)
                        {
                            res = true;// share.isUpTrend(Share.pStaticTMP);
                        }
                    }
                    
                    break;
                case SIGNAL_WILLIAM_CUT_SMA:
                    {
                        int cnt = share.getCandleCnt();

                        days = (int)filter.param3;
                        int sma1 = (int)filter.param1;
                        int sma2 = (int)filter.param2;

                        if (sma1 < 1) sma1 = 1;
                        if (days < 0) days = 1;

                        cnt = days + sma2 + 10;
                        if (cnt > share.getCandleCnt())
                        {
                            cnt = share.getCandleCnt();
                        }

                        if (cnt > 10)
                        {
                            share.calcWilliamR();
                            int offset = share.getCandleCnt() - cnt;

                            float[] pSMA1 = Share.SMA(share.pWilliamR, offset, cnt, sma1, Share.pStaticTMP);
                            float[] pSMA2 = Share.SMA(share.pWilliamR, offset, cnt, sma2, Share.pStaticTMP1);
                        
                            return isLine1CutAboveLine2InDays(pSMA1, 0, pSMA2, 0, cnt, days);
                        }

                    }
                    break;
                case SIGNAL_WILLIAM_DOWN_AND_LOWER_THAN:
                    share.calcWilliamR();
                    res = share.pWilliamR[last] <= filter.param1;
                    break;
                case SIGNAL_WILLIAM_HIGHER_THAN:
                    share.calcWilliamR();
                    res = share.pWilliamR[last] >= filter.param1;
                    break;
                case SIGNAL_WILLIAM_LOWER_THAN:
                    share.calcWilliamR();
                    res = share.pWilliamR[last] <= filter.param1;
                    break;

                //case SIGNAL_PRICE_CROSS_UP:
                    //break;
                //case SIGNAL_PRICE_CROSS_DOWN:
                    //break;

                case SIGNAL_BREAOUT_UP:
                    break;
                case SIGNAL_BREAOUT_WEDGE:
                    break;

                case SIGNAL_VOLUME_IS_UP:
                    if (share.mCode.CompareTo("SMC") == 0)
                    {
                        res = false;
                    }
                    res = share.isUpTrend(share.mCVolume, share.getCandleCnt());
                    {
                        double t = (double)share.getAveVolumeInDays(3) / share.getAveVolumeInDays(6);
                        share.mSortParam = t;
                    }
                    break;
                case SIGNAL_VOLUME_IS_DOWN:
                    {
                        res = share.isDownTrend(share.mCVolume, share.getCandleCnt());
                        double t = share.getAveVolumeInDays(6) / share.getAveVolumeInDays(3);
                        share.mSortParam = t;
                    }
                    break;
                case SIGNAL_VOLUME_IS_OUT:
                    {
                        if (share.mCode.CompareTo("PJT") == 0)
                        {
                            Utils.trace("bcc");
                        }
                        double d1 = share.calcAvgVolume(3);
                        double d2 = share.calcAvgVolume(20, 4);

                        if (d2 > 0)
                        {
                            double r = d1 / d2;

                            if (r < 0.7)
                            {
                                share.mSortParam = (int)(10 / r);
                                return true;
                            }
                        }
                        
                    }
                    break;

                case SIGNAL_ADX_DIPLUS_ABOVE_DIMINUS:
                    share.calcADX();
                    res = share.pPLUS_DI[last] > share.pMINUS_DI[last];
                    break;
                case SIGNAL_PRICE_HIGHEST_IN:
                    {
                        int end = share.getCandleCnt() - 10;
                        int begin = end - (int)filter.param1;

                        if (begin < 0) begin = 0;
                        if (end > begin)
                        {
                            float highestPrice = share.getHighestPriceIn(begin, end);
                            float percent = (share.getClose(share.getCandleCnt()-1) - highestPrice) * 100 / highestPrice;
                            res = percent > 0 && percent < 30.0;
                        }
                    }
                    break;
                case SIGNAL_PRICE:
                    {
                        float price = share.getClose(share.getCandleCnt() - 1);

                        float value1 = filter.param1;
                        float value2 = filter.param2;
                        if (value1 == 0 && value2 == 0)
                        {
                            res = true;
                        }
                        else if (value1 == 0)
                        {
                            res = price < value2;
                        }
                        else if (value2 == 0)
                        {
                            res = price > value1;
                        }
                        else
                        {
                            res = value1 <= price && price <= value2;
                        }
                    }
                    break;
                case SIGNAL_VOLUME:
                    {
                        int avgVol = share.getAveVolumeInDays(4);
                        res = avgVol > filter.param1;
                    }
 
                    break;
                case SIGNAL_TRADE_VALUE:
                    {
                        int value = (int)filter.param1;
                        int avgVol = share.getAveVolumeInDays(4);

                        double tradeValue = avgVol * share.getClose(share.getCandleCount() - 1);
                        tradeValue /= 1000;     //  to million
                        res = tradeValue >= value;
                    }
                    break;
                case SIGNAL_RS:
                    {
                        if (share.getCode().CompareTo("STB") == 0)
                        {
                            Utils.trace("aaa");
                        }
                        Share vnindex = Context.getInstance().mShareManager.getShare("^VNINDEX");
                        days = (int)filter.param3;
                        if (vnindex != null && share.getCandleCnt() > days){
                            int ma1 = (int)filter.param1;
                            int ma2 = (int)filter.param2;
                            share.calcRSPrice(vnindex, ma1, ma2);

                            int j = share.getCandleCnt()-1;

                            if (ma1 > 0 && ma2 > 0 && j > ma1 && j > ma2)
                            {
                                res = share.pCRS_MA1[j] >= share.pCRS_MA2[j];
                                if (res)
                                {
                                    res = false;
                                    int begin = share.getCandleCnt() - days;
                                    if (begin < 0)
                                    {
                                        begin = 0;
                                    }
                                    for (; j >= begin; j--)
                                    {
                                        if (share.pCRS_MA1[j] < share.pCRS_MA2[j])
                                        {
                                            res = true;
                                            break;
                                        }
                                    }
                                }
                                
                            }
                            else
                            {
                                res = false;
                            }
                        }

                    }
                    break;
                case SIGNAL_RS_PERFORMANCE:
                    {
                        Share vnindex = Context.getInstance().mShareManager.getShare("^VNINDEX");
                        days = (int)filter.param3;
                        if (vnindex != null && share.getCandleCnt() > days)
                        {
                            int period = (int)filter.param1;
                            int ma = (int)filter.param2;
                            share.calcRSPerformance(vnindex, period, (int)ma, 0);

                            int j = share.getCandleCnt() - 1;

                            if (ma > 0)
                            {
                                res = share.pCRS_Percent[j] >= share.pCRS_MA1_Percent[j];
                            }
                            else
                            {
                                res = true;
                            }
                            if (res && days > 0)
                            {
                                res = false;
                                for (; j >= share.getCandleCnt() - days; j--)
                                {
                                    if (share.pCRS_Percent[j] < share.pCRS_MA1_Percent[j])
                                    {
                                        res = true;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    break;
                default:
                    break;
            }
            return res;
        }

        bool isLine1CutAboveLine2InDays(float[] line1, int offset1, 
            float[] line2, int offset2,
            int cnt,
            int days)
        {
            int last1 = offset1 + cnt - 1;
            int last2 = offset2 + cnt - 1;

            if (last1 > line1.Length) last1 = line1.Length - 1;
            if (last2 > line2.Length) last2 = line2.Length - 1;

            if (line1[last1] >= line2[last2])
            {
                if (days > 0)
                {
                    bool ok = false;
                    for (int i = 0; i < days; i++)
                    {
                        if (last1 - i >= 0 && last2 - i >= 0)
                        {
                            if (line1[last1 - i] < line2[last2 - i])
                            {
                                ok = true;
                                break;
                            }
                        }
                    }
                    return ok;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}
