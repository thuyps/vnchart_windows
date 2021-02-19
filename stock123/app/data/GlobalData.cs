using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xlib.utils;
using xlib.framework;

namespace stock123.app.data
{
    
    public class GlobalData
    {
        public const String kVNIndices = "vnindices";
        public const String kCRSMethodRatio = "crs_ratio";
        public const String kCRSMethodPercent = "crs_percent";
        public const String kCRSBaseSymbol = "crs_base";
        public const String kCRSBaseMa1 = "crs_ma1";
        public const String kCRSBaseMa2 = "crs_ma2";
        public const String kCRSPeriod = "crs_period";
        public const String kWilliamR_MA1 = "wr_ma1";
        public const String kWilliamR_MA2 = "wr_ma2";

        static VTDictionary data = new VTDictionary();
        static VTDictionary dataPriceboard = new VTDictionary();

        public static stPriceboardState getPriceboardStateOfIndexWithSymbol(String symbol)
        {
            xVector v = (xVector)dataPriceboard.getValue(kVNIndices);
            if (v == null){
                return null;
            }
            for (int i = 0; i < v.size(); i++)
            {
                stPriceboardState ps = (stPriceboardState)v.elementAt(i);
                if (ps.code.Equals(symbol)){
                    return ps;
                }
            }

            return null;
        }

        public static void addPriceboardStateOfIndex(stPriceboardState ps)
        {
            xVector v = (xVector)dataPriceboard.getValue(kVNIndices);
            if (v == null)
            {
                v = new xVector();
                dataPriceboard.setValue(kVNIndices, v);
            }

            bool added = false;
            for (int i = 0; i < v.size(); i++)
            {
                stPriceboardState _ps = (stPriceboardState)v.elementAt(i);
                if (_ps.code.Equals(ps.code))
                {
                    added = true;
                    _ps.copyFrom(ps);

                    break;
                }
            }

            if (!added)
            {
                v.addElement(ps);
            }
        }

        static public VTDictionary getData()
        {
            return data;
        }
        static public void saveData()
        {
            String str = data.toJson();
            xDataOutput o = new xDataOutput(10*1024);
            o.writeUTF(str);
            xFileManager.saveFile(o, "global.dat");
        }

        static public void loadData()
        {
            xDataInput di = xFileManager.readFile("global.dat", false);
            if (di != null)
            {
                String str = di.readUTF();

                if (str != null)
                {
                    data = new VTDictionary(str);
                }
            }
        }

        static public VTDictionary getCRSRatio()
        {
            if (getData().hasKey(kCRSMethodRatio))
            {
                String s = getData().getValueString(kCRSMethodRatio);
                return new VTDictionary(s);
            }

            VTDictionary d = new VTDictionary();
            d.setValueString(kCRSBaseSymbol, "^VNINDEX");
            d.setValueInt(kCRSBaseMa1, 5);
            d.setValueInt(kCRSBaseMa2, 0);

            return d;
        }

        static public VTDictionary getCRSPercent()
        {
            if (getData().hasKey(kCRSMethodPercent))
            {
                String s = getData().getValueString(kCRSMethodPercent);
                return new VTDictionary(s);
            }

            VTDictionary d = new VTDictionary();
            d.setValueString(kCRSBaseSymbol, "^VNINDEX");
            d.setValueInt(kCRSPeriod, 5);
            d.setValueInt(kCRSBaseMa1, 5);
            d.setValueInt(kCRSBaseMa2, 0);

            return d;
        }
    }
}
