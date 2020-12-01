using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xlib.utils;
using System.Collections;
using stock123.app.data;

namespace stock123.app.net
{
    class JSONHandler
    {
        public const int JMSG_ALARM_PRICE_ADD = 1;
        public const int JMSG_ALARM_PRICE_REMOVE = 2;
        public const int JMSG_ALARM_PRICE_GET = 3;
        public const int JMSG_ALARM_PRICE_EDIT = 4;
        public const int JMSG_GET_ACCOUNT_INFO = 20;
        public const int JMSG_USE_PURCHASED_PACKAGE = 21;

        public const int JMSG_LIST_INDICES = 22;


        //  alarm type
        public const int ALARM_TYPE_GUARD_PRICE = 1;
        public const int ALARM_TYPE_GUARD_RSI = 2;

        public const String kMessageID = "msg_id";

        public const String PARAM_ALARM_ID = "id";
        public const String PARAM_ALARM_TYPE = "type";
        public const String PARAM_ALARM_DIRECTION = "dir";
        public const String PARAM_ALARM_PRICE = "price";

        public const String PARAM_1 = "param1";
        public const String PARAM_2 = "param2";
        public const String PARAM_3 = "param3";
        public const String PARAM_4 = "param4";

        public const String PARAM_SYMBOL = "symbol";
        public const String PARAM_CREATE_DATE = "cdate";
        public const String PARAM_FIRED_DATE = "fdate";
        public const String PARAM_STATUS = "status";

        public const String kSymbol = "symb";
        public const String kPoint = "pt";
        public const String kChanged = "chg";
        public const String kChangedPercent = "chgpc";
        public const String kVolume = "vol";
        public const String kHighest = "hi";
        public const String kLowest = "lo";
        public const String kRef = "ref";

        public const String kArray = "array";
        public const String kErrorCode = "error_code";

        public static void processJsonMessage(VTDictionary dict)
        {
            int msgId = dict.getValueInt(kMessageID);
            if (msgId == JMSG_LIST_INDICES)
            {
                processListIndices(dict);
            }
        }

        static void processListIndices(VTDictionary dict)
        {
            int code = dict.getValueInt(kErrorCode);
            ArrayList arr = dict.getArray(kArray);

            stShareGroup g = Context.getInstance().vnIndicesGroup;
            if (g == null)
            {
                g = new stShareGroup();
                Context.getInstance().vnIndicesGroup = g;
                g.setType(stShareGroup.ID_GROUP_INDICES);
            }
            if (code == 0 & arr != null)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    VTDictionary d = (VTDictionary)arr[i];
                    int id = d.getValueInt("id");
                    String symbol = d.getValueString(kSymbol);
                    float price = d.getValueFloat(kPoint)/1000.0f;
                    float change = d.getValueFloat(kChanged) / 1000.0f;
                    long vol = d.getValueLong(kVolume);
                    float hi = d.getValueFloat(kHighest) / 1000.0f;
                    float lo = d.getValueFloat(kLowest) / 1000.0f;
                    float reference = d.getValueFloat(kRef) / 1000.0f;

                    stPriceboardState ps = GlobalData.getPriceboardStateOfIndexWithSymbol(symbol);
                    if (ps == null)
                    {
                        ps = new stPriceboardState();
                        
                        GlobalData.addPriceboardStateOfIndex(ps);
                    }
                    ps.id = id;
                    ps.code = symbol;
                    ps.current_price_1 = price;
                    ps.total_volume = Convert.ToInt32(vol);
                    ps.max = hi;
                    ps.min = lo;
                    ps.change = change;
                    ps.reference = reference;

                    g.addCode(symbol);
                }
            }
        }
    }
}
