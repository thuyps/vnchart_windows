using System;
using System.Collections.Generic;
using System.Text;

using xlib.framework;

namespace stock123.app.data
{
    public class stAlarm
    {
        public stAlarm(string _c, int _d, int _u, int _l)
        {
            code = _c;
            date = _d;
            upperPrice = _u;
            lowerPrice = _l;
            fired = false;
            comment = "";
        }

        public stAlarm()
        {
            comment = "";
        }
        //public int 
        public string code;
        public int date;    //  setup date
        public int upperPrice;
        public int lowerPrice;
        public bool fired;
        public String comment;

        //  0: no alarm, -1: lower-price alarm; 1: upper-price alarm
        public int hasAlarm()
        {
            stPriceboardState ps = Context.getInstance().mPriceboard.getPriceboard(code);
            if (ps != null)
            {
                float price = ps.getCurrentPrice();
                if (price <= lowerPrice && lowerPrice > 0 && price > 0)
                    return -1;
                if (price >= upperPrice && upperPrice > 0 && price > 0)
                    return 1;
            }

            return 0;
        }

        public bool hasTriggerredAlarm()
        {
            if (!fired)
            {
                if (hasAlarm() != 0)
                {
                    fired = true;
                    return true;
                }
            }

            return false;
        }
    }
}
