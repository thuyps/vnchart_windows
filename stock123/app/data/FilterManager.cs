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
        public bool hasSetting;
        public bool[] selected = {false};

        public String getTitle()
        {
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
                || type == FilterManager.SIGNAL_MA3_CUT_ABOVE_MA4)
            {
                String s = String.Format("MA {0} nằm trên MA {1}", param1, param2);
                return s;
            }

            if (type == FilterManager.SIGNAL_HEIKEN_CANDLE_GREEN) return "Heiken xanh";
            if (type == FilterManager.SIGNAL_HEIKEN_CANDLE_RED) return "Heiken đỏ";

            //  TRIX
            //arr.addElement(initWithSignal(FilterManager.SIGNAL_BUY_TRIX, "TRIX buy"));
            //arr.addElement(initWithSignal(FilterManager.SIGNAL_SELL_TRIX, "TRIX sign"));

            //  ADX
            if (type == FilterManager.SIGNAL_BUY_ADX_DIs_CROSS) return "ADX: DI+ cắt & vượt trên DI-";
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
                return s;
            }

            if (type == FilterManager.SIGNAL_RSI_HIGHER_THAN){
                String s = String.Format("RSI lớn hơn {0}", param1);
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
                return s;
            }
            
            if (type == FilterManager.SIGNAL_MFI_HIGHER_THAN)
            {
                String s = String.Format("MFI is higher than {0}", param1);
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
                String s = String.Format("William %R tăng và lớn hơn {0}", param1);
                return s;
            }

            if (type == FilterManager.SIGNAL_WILLIAM_DOWN_AND_LOWER_THAN)
            {
                String s = String.Format("William %R giảm và nhỏ hơn {0}", param1);
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
                o.writeInt((int)(item.param1 * 100));
                o.writeInt((int)(item.param2 * 100));
                o.writeBoolean(item.selected[0]);
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
        public const int SIGNAL_MA3_CUT_ABOVE_MA4                   = 211;
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
        //public const int SIGNAL_FULLSTO_HIGHER_THAN = 505;
        
        public const int SIGNAL_WILLIAM_UP_AND_HIGHER_THAN     = 520;
        public const int SIGNAL_WILLIAM_DOWN_AND_LOWER_THAN     = 521;
        public const int SIGNAL_WILLIAM_HIGHER_THAN     = 522;
        public const int SIGNAL_WILLIAM_LOWER_THAN     = 523;
        
        //public const int SIGNAL_PRICE_CROSS_UP        = 520;
        //public const int SIGNAL_PRICE_CROSS_DOWN      = 521;
        
        public const int SIGNAL_BREAOUT_UP           = 540;
        public const int SIGNAL_BREAOUT_WEDGE           = 541;
        
        public const int SIGNAL_VOLUME_IS_UP                 = 800;
        public const int SIGNAL_VOLUME_IS_DOWN               = 801;
        
        public const int SIGNAL_ADX_DIPLUS_ABOVE_DIMINUS     = 820;

        public const int SIGNAL_PRICE_HIGHEST_IN = 830;

        public const int SIGNAL_TICHLUY = 1001;

        xVector mFilterSets = new xVector();
        const string FILE_FILTER = "filter.dat";
        public const int FILTER_VERSION = 3;

        static FilterManager instance;
        public static FilterManager getInstance()
        {
            if (instance == null)
            {
                instance = new FilterManager();
                instance.loadFilterSets();
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
        public void loadFilterSets()
        {
            mFilterSets.removeAllElements();

            xDataInput di = xFileManager.readFile(FILE_FILTER, false);
            if (di != null)
            {
                int ver = di.readInt();
                if (ver == FILTER_VERSION)
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
                filterSet = new FilterSet("SMA 9 nằm trên SMA 22");
                item = initWithSignal(SIGNAL_MA1_CUT_ABOVE_MA2, "");
                item.param1 = 9;
                item.param2 = 22;
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
                filterSet.addFilterItem(item);
                mFilterSets.addElement(filterSet);

                filterSet = new FilterSet("RSI thấp hơn");
                item = initWithSignal(SIGNAL_RSI_LOWER_THAN, "");
                item.param1 = 30;
                filterSet.addFilterItem(item);
                mFilterSets.addElement(filterSet);

                //  set 5
                filterSet = new FilterSet("MFI cao hơn");
                item = initWithSignal(SIGNAL_MFI_HIGHER_THAN, "");
                item.param1 = 50;
                filterSet.addFilterItem(item);
                mFilterSets.addElement(filterSet);

                filterSet = new FilterSet("MFI thấp hơn");
                item = initWithSignal(SIGNAL_MFI_LOWER_THAN, "");
                item.param1 = 30;
                filterSet.addFilterItem(item);
                mFilterSets.addElement(filterSet);
            }
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

        public void saveFilterSets()
        {
            xDataOutput o = new xDataOutput(2048);
            o.writeInt(FILTER_VERSION);
            o.writeInt(mFilterSets.size());

            for (int i = 0; i < mFilterSets.size(); i++)
            {
                FilterSet filter = (FilterSet)mFilterSets.elementAt(i);
                filter.saveFilterSet(o);
            }

            //  filter version
            o.writeInt(0x00110011);

            xFileManager.saveFile(o, FILE_FILTER);
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
    
            arr.addElement(initWithSignal(SIGNAL_VOLUME_IS_UP, "Khối lượng tăng"));
            arr.addElement(initWithSignal(SIGNAL_VOLUME_IS_DOWN, "Khối lượng giảm"));
            
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
            arr.addElement(initWithSignal(SIGNAL_PSAR_TURNS_GREEN, "PSAR xanh"));
            arr.addElement(initWithSignal(SIGNAL_PSAR_TURNS_RED, "PSAR đỏ"));
            arr.addElement(initWithSignal(SIGNAL_VSTOP_TURN_GREEN, "VStop xanh"));
            arr.addElement(initWithSignal(SIGNAL_VSTOP_TURNS_RED, "VSTOP đỏ"));
            
            
            //  Moving average
            FilterItem item = initWithSignal(SIGNAL_MA1_CUT_ABOVE_MA2, "MA 5 nằm trên MA 22", true);
            item.param1 = 5;
            item.param2 = 22;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_MA3_CUT_ABOVE_MA4, "MA 50 nằm trên MA 100", true);
            item.param1 = 50;
            item.param2 = 100;
            arr.addElement(item);
            /*
            item = [[FilterItem alloc]initWithSignal:SIGNAL_MA1_CUT_BELOW_MA2, "Moving Average 9 is below Moving Average 15"];
            [item setParam:0 value:9];
            [item setParam:1 value:15];
            [arr addObject:item];
            
            item = [[FilterItem alloc]initWithSignal:SIGNAL_MA3_CUT_BELOW_MA4, "Moving Average 50 is below Moving Average 100"];
            [item setParam:0 value:50];
            [item setParam:1 value:100];
            [arr addObject:item];
            */
            
            arr.addElement(initWithSignal(SIGNAL_HEIKEN_CANDLE_GREEN, "Heiken xanh"));
            arr.addElement(initWithSignal(SIGNAL_HEIKEN_CANDLE_RED, "Heiken đỏ"));
            
            //  TRIX
            //arr.addElement(initWithSignal(SIGNAL_BUY_TRIX, "TRIX buy"));
            //arr.addElement(initWithSignal(SIGNAL_SELL_TRIX, "TRIX sign"));
            
            //  ADX
            arr.addElement(initWithSignal(SIGNAL_BUY_ADX_DIs_CROSS, "ADX: DI+ cắt + vượt trên DI-"));
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
            arr.addElement(item);

            item = initWithSignal(SIGNAL_RSI_HIGHER_THAN, "RSI lớn hơn 70", true);
            item.param1 = 70;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_RSI_UP_AND_HIGHER_THAN, "RSI tăng và lớn hơn 50", true);
            item.param1 = 50;
            arr.addElement(item);
            
            //  MFI
            item = initWithSignal(SIGNAL_MFI_LOWER_THAN, "MFI is lower than", true);
            item.param1 = 30;
            arr.addElement(item);

            item = initWithSignal(SIGNAL_MFI_HIGHER_THAN, "MFI is higher than", true);
            item.param1 = 70;
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

            item = initWithSignal(SIGNAL_PRICE_HIGHEST_IN, "", true);
            item.param1 = 6 * 22;
            arr.addElement(item);

            return arr;
        }

        public bool hasSignal(FilterItem filter, Share share)
        {
            if (share.mCode.CompareTo("VND") == 0)
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
            switch (type)
            {
                case SIGNAL_MACD_CUT_ABOVE_SIGNALLINE:
                    share.calcMACD();
                    res = share.isMACDCutSignalAbove(9);// Share.pMACD[last] > Share.pMACDSignal9[last];
                    break;
                case SIGNAL_MACD_CUT_ABOVE_CENTERLINE:
                    share.calcMACD();
                    res = share.hasIntersesionAbove(Share.pMACD, null, 9);
                    break;
                case SIGNAL_BUY_MACD_ABOVE_SIGNALLINE:
                    share.calcMACD();
                    res = Share.pMACD[last] > Share.pMACDSignal9[last];
                    break;
                case SIGNAL_BUY_MACD_ABOVE_CENTERLINE:
                    share.calcMACD();
                    res = Share.pMACD[last] > 0;
                    break;
                case SIGNAL_ICHIMOKU_CUT_ABOVE_TENKAN_KIJUN:
                    share.calcIchimoku();
                    res = share.hasIntersesionAbove(Share.pTenkansen, Share.pKijunsen, 9);
                    break;
                case SIGNAL_BUY_ICHIMOKU_GREEN_CLOUD:
                    share.calcIchimoku();
                    res = Share.pSpanA[last] > Share.pSpanB[last];
                    break;
                case SIGNAL_MA1_CUT_ABOVE_MA2:
                case SIGNAL_MA3_CUT_ABOVE_MA4:
                    {
                        if (filter.param1 > 0 && filter.param2 > 0)
                        {
                            share.calcSMA((int)filter.param1, (int)filter.param2, 0, 0, 0);
                            res = Share.pSMA1[last] > Share.pSMA2[last];
                        }
                    }
                    break;
                    break;
                case SIGNAL_PSAR_TURNS_GREEN:
                    share.calcPSAR();
                    res = Share.pSAR_SignalUp[last];
                    break;
                case SIGNAL_VSTOP_TURN_GREEN:
                    share.calcVSTOP();
                    res = Share.pVSTOP_SignalUp[last];
                    break;
                case SIGNAL_HEIKEN_CANDLE_GREEN:
                    {
                        float[] heiken = Share.pTMP;
                        int max = share.getCandleCnt() > 30 ? 30 : share.getCandleCnt();
                        share.calcHeiken(max, heiken);

                        return heiken[max - 1] > 0;
                    }
                    break;
                case SIGNAL_BUY_TRIX:
                    break;
                case SIGNAL_BUY_ICHIMOKU_PRICE_CUT_ABOVE_KIJUN:
                    share.calcIchimoku();

                    res = share.hasIntersesionAbove(share.mCClose, Share.pKijunsen, 9);
                    break;
                case SIGNAL_BUY_ADX_DIs_CROSS:
                    share.calcADX();

                    res = share.hasIntersesionAbove(Share.pPLUS_DI, Share.pMINUS_DI, 9);
                    break;
                case SIGNAL_BUY_FULLSTO:
                    share.calcStochasticFull();
                    res = share.hasIntersesionAbove(Share.pStochasticSlowK, Share.pStochasticSlowD, 9);
                    break;

                case SIGNAL_MACD_CUT_BELOW_SIGNALLINE:
                    share.calcMACD();
                    res = Share.pMACD[last] < Share.pMACDSignal9[last];
                    break;
                case SIGNAL_MACD_CUT_BELOW_CENTERLINE:
                    share.calcMACD();
                    res = Share.pMACD[last] < 0;
                    break;
                case SIGNAL_ICHIMOKU_TENKAN_BELOW_KIJUN:
                    share.calcIchimoku();
                    res = Share.pTenkansen[last] < Share.pKijunsen[last];
                    break;
                case SIGNAL_SELL_ICHIMOKU_PRICE_CUT_BELOW_KIJUN:
                    break;

                case SIGNAL_MA1_CUT_BELOW_MA2:
                    //share.calcSMA(
                    break;
                case SIGNAL_MA3_CUT_BELOW_MA4:
                    break;

                case SIGNAL_PSAR_TURNS_RED:
                    share.calcPSAR();
                    res = Share.pSAR_SignalUp[last] == false;
                    break;
                case SIGNAL_VSTOP_TURNS_RED:
                    share.calcVSTOP();
                    res = Share.pVSTOP_SignalUp[last] == false;
                    break;
                case SIGNAL_HEIKEN_CANDLE_RED:
                    {
                        float[] heiken = Share.pTMP;
                        int max = share.getCandleCnt() > 30 ? 30 : share.getCandleCnt();
                        share.calcHeiken(max, heiken);

                        return heiken[max - 1] < 0;
                    }

                    break;
                case SIGNAL_SELL_TRIX:
                    break;
                case SIGNAL_ICHIMOKU_PRICE_CUT_BELOW_KIJUN:
                    share.calcIchimoku();
                    res = share.mCClose[last] < Share.pKijunsen[last];
                    break;
                case SIGNAL_SELL_ADX_DIs_CROSS:
                    share.calcADX();
                    res = Share.pPLUS_DI[last] < Share.pMINUS_DI[last];
                    break;
                case SIGNAL_SELL_FULLSTO:
                    share.calcStochasticFull();

                    res = Share.pStochasticSlowK[last] < Share.pStochasticSlowD[last];
                    break;

                //=====================================================
                case SIGNAL_RSI_HIGHER_THAN:
                    share.calcRSI(0);
                    res = Share.pRSI[last] >= filter.param1 / 1.0f;
                    break;
                case SIGNAL_MFI_HIGHER_THAN:
                    share.calcMFI(0);
                    res = Share.pMFI[last] >= filter.param1 / 1.0f;
                    break;
                case SIGNAL_ROC_HIGHER_THAN:
                    share.calcROC(0);
                    res = Share.pROC[last] >= filter.param1 / 1.0f;
                    break;
                case SIGNAL_FULLSTO_HIGHER_THAN:
                    share.calcStochasticFull();
                    res = Share.pStochasticSlowK[last] >= filter.param1 / 1.0f;
                    break;

                case SIGNAL_RSI_LOWER_THAN:
                    share.calcRSI(0);
                    res = Share.pRSI[last] <= filter.param1 / 1.0f;
                    break;
                case SIGNAL_MFI_LOWER_THAN:
                    share.calcMFI(0);
                    res = Share.pMFI[last] <= filter.param1 / 1.0f;
                    break;
                case SIGNAL_ROC_LOWER_THAN:
                    share.calcROC(0);
                    res = Share.pROC[last] <= filter.param1 / 1.0f;
                    break;
                case SIGNAL_FULLSTO_LOWER_THAN:
                    share.calcStochasticFull();
                    res = Share.pStochasticSlowK[last] <= filter.param1 / 1.0f;
                    break;

                case SIGNAL_RSI_UP_AND_HIGHER_THAN:
                    share.calcRSI(0);
                    if (Share.pRSI[last] >= filter.param1)
                    {
                        res = share.isUpTrend(Share.pRSI);
                    }
                    break;
                case SIGNAL_MFI_UP_AND_HIGHER_THAN:
                    share.calcMFI(0);
                    if (Share.pMFI[last] >= filter.param1)
                    {
                        res = share.isUpTrend(Share.pMFI);
                    }
                    break;
                case SIGNAL_ROC_UP_AND_HIGHER_THAN:
                    share.calcROC(0);
                    if (Share.pROC[last] >= filter.param1)
                    {
                        res = share.isUpTrend(Share.pROC);
                    }
                    break;
                case SIGNAL_FULLSTO_UP_AND_HIGHER_THAN:
                    share.calcStochasticFull();
                    res = Share.pStochasticSlowK[last] <= filter.param1 / 1.0f;
                    if (res)
                    {
                        res = share.isUpTrend(Share.pStochasticSlowK);
                    }
                    break;
                case SIGNAL_BUY_ADX_HIGHER_THAN:
                    share.calcADX();
                    res = Share.pADX[last] >= filter.param1;
                    break;
                //public const int SIGNAL_FULLSTO_HIGHER_THAN = 505;

                case SIGNAL_WILLIAM_UP_AND_HIGHER_THAN:
                    share.calcWilliamR();
                    res = Share.pWilliamR[last] >= filter.param1;
                    if (res)
                    {
                        res = share.isUpTrend(Share.pWilliamR);
                    }
                    break;
                case SIGNAL_WILLIAM_DOWN_AND_LOWER_THAN:
                    share.calcWilliamR();
                    res = Share.pWilliamR[last] <= filter.param1;
                    break;
                case SIGNAL_WILLIAM_HIGHER_THAN:
                    share.calcWilliamR();
                    res = Share.pWilliamR[last] >= filter.param1;
                    break;
                case SIGNAL_WILLIAM_LOWER_THAN:
                    share.calcWilliamR();
                    res = Share.pWilliamR[last] <= filter.param1;
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

                case SIGNAL_ADX_DIPLUS_ABOVE_DIMINUS:
                    share.calcADX();
                    res = Share.pPLUS_DI[last] > Share.pMINUS_DI[last];
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
                default:
                    break;
            }
            return res;
        }
    }
}
