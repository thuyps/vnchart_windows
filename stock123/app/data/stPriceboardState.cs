/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
using System;
using System.Collections.Generic;
using System.Text;
using xlib.framework;
using xlib.utils;
/**
 *
 * @author ThuyPham
 */
namespace stock123.app.data
{
    public class stPriceboardState
    {
        public String code;     //  10
        public int floorId;
        public int state;
        public int[] remain_sell_volume = { 0, 0, 0 };
        public int[] remain_buy_volume = { 0, 0, 0 };
        public int current_volume_1;
        public int total_volume;
        public float ce;
        public float reference;
        public float floor;
        public float max;
        public float min;
        public float[] remain_buy_price = { 0, 0, 0 };
        public float[] remain_sell_price = { 0, 0, 0 };
        public float current_price_1;
        public float change;
        public int changePercent;		//	1000	==	1%
        public int id; 
        
        static xVector mPriceboards = new xVector(3000);
        //=====================================================

        public stPriceboardState()
        {
        }
        public static stPriceboardState priceboard_state = new stPriceboardState();
        //=====================================================
        public static void flush(xDataOutput o, int floor)
        {
            
        }

        public static void load(xDataInput di)
        {
            
        }
        static public stPriceboardState seekPriceboard(int marketID, byte[] code, int offset)
        {
            String scode = Utils.bytesNullTerminatedToString(code, offset, 20);

            return stPriceboardState.seekPriceboard(marketID, scode);
        }


        public static stPriceboardState getPriceboardItem(int floor, String code)
        {
            for (int i = 0; i < mPriceboards.size(); i++)
            {
                stPriceboardState item = (stPriceboardState)mPriceboards.elementAt(i);
                if (item != null && item.code != null && item.code.Equals(code))
                {
                    return item;
                }
            }
            stPriceboardState newitem = new stPriceboardState();
            mPriceboards.addElement(newitem);
            newitem.code = code;
            return newitem;
        }

        static public int getCount(int floor)
        {
            return mPriceboards.size();
        }
        
        public String getCode()
        {
            return code;
        }

        public int getState()
        {
            return 0;
        }

        //------------------------------
        public int getRemainSellVolume0()
        {
            return remain_sell_volume[0];
        }

        public int getRemainBuyVolume0()
        {
            return remain_buy_volume[0];
        }
        public int getRemainSellVolume1()
        {
            return remain_sell_volume[1];
        }

        public int getRemainBuyVolume1()
        {
            return remain_buy_volume[1];
        }
        public int getRemainSellVolume2()
        {
            return remain_sell_volume[2];
        }

        public int getRemainBuyVolume2()
        {
            return remain_buy_volume[2];
        }
        //------------------------------

        public int getCurrentVolume()
        {
            return current_volume_1;
        }

        public int getTotalVolume()
        {
            return total_volume;
        }

        public float getCe()
        {
            return ce;
        }

        public float getRef()
        {
            return reference;
        }

        public float getFloor()
        {
            return floor;
        }

        public float getMax()
        {
            return max;
        }

        public float getMin()
        {
            return min;
        }
        //-------------------------------------
        public float getRemainBuyPrice0()
        {
            return remain_buy_price[0];
        }

        public float getRemainSellPrice0()
        {
            return remain_sell_price[0];
        }
        public float getRemainBuyPrice1()
        {
            return remain_buy_price[1];
        }

        public float getRemainSellPrice1()
        {
            return remain_sell_price[1];
        }
        public float getRemainBuyPrice2()
        {
            return remain_buy_price[2];
        }

        public float getRemainSellPrice2()
        {
            return remain_sell_price[2];
        }
        //-------------------------------------

        public float getCurrentPrice()
        {
            if (id == 76)
            {
                id = 76;
            }
            return current_price_1;
        }

        public float convertPriceToReal(int price)
        {
            return (float)(price / 1000);
        }

        public float getChange()
        {
            return change;
        }
        public int getAveValue()
        {
            return 0;
        }
        public int getChangePercent()
        {
            return changePercent;
        }
        public int getID()
        {
            return id;
        }
        //========================================================
       
        public void setZero(xDataInput di)
        {
            if (code.Equals("ACM"))
            {
                max = 0;
            }
            max = di.readInt()/1000.0f;
            min = di.readInt()/1000.0f;

            //  remain buy
            int i = 0;
            for (i = 0; i < 3; i++)
            {
                remain_buy_price[i] = di.readInt()/1000.0f;
                remain_buy_volume[i] = di.readInt();
            }
            
            for (i = 0; i < 3; i++)
            {
                remain_sell_price[i] = di.readInt() / 1000.0f;
                remain_sell_volume[i] = di.readInt();
            }

            current_price_1 = di.readInt() / 1000.0f;
            if (current_price_1 < 100)
            {
                id = id;
            }
            current_volume_1 = di.readInt();

            change = di.readInt()/1000.0f;
            total_volume = di.readInt();

            //  update status
            //Utils.writeInt(pData, offset + POS_STATE, 0xffffffff);
        }


        public static stPriceboardState seekPriceboard(int floor, String code)
        {
            return getPriceboardItem(floor, code);
        }

        public static stPriceboardState seekPriceboardByID(int floor, int id)
        {
            for (int i = 0; i < mPriceboards.size(); i++)
            {
                stPriceboardState item = (stPriceboardState)mPriceboards.elementAt(i);
                if (item.id == id)
                {
                    return item;
                }
            }
            stPriceboardState newitem = new stPriceboardState();
            mPriceboards.addElement(newitem);
            newitem.id = (short)id;

            return newitem;
        }

        public void setMax(int v)
        {
            max = v;
        }
        public void setMin(int v)
        {
            min = v;
        }
        //---------------------------------------
        public void setRemainBuyPrice0(float v)
        {
            remain_buy_price[0] = v;
        }
        public void setRemainBuyVolume0(int v)
        {
            remain_buy_volume[0] = v;
        }
        public void setRemainBuyPrice1(float v)
        {
            remain_buy_price[1] = v;
        }
        public void setRemainBuyVolume1(int v)
        {
            remain_buy_volume[1] = v;
        }
        public void setRemainBuyPrice2(float v)
        {
            remain_buy_price[2] = v;
        }
        public void setRemainBuyVolume2(int v)
        {
            remain_buy_volume[2] = v;
        }
        //---------------------------------------
        public void setCurrentPrice(float v)
        {
            if (current_price_1 < 100)
            {
                id = id;
            }
            current_price_1 = v;
        }
        public void setCurrentVolume(int v)
        {
            current_volume_1 = v;
        }
        public void setAveValue(int v)
        {
            
        }
        public void setChange(float v)
        {
            change = v;
        }
        public void setChangePercent(int v)
        {
            changePercent = v;
        }
        //========================================
        public void setRemainSellPrice0(float v)
        {
            remain_sell_price[0] = v;
        }
        public void setRemainSellVolume0(int v)
        {
            remain_sell_volume[0] = v;
        }
        public void setRemainSellPrice1(float v)
        {
            remain_sell_price[1] = v;
        }
        public void setRemainSellVolume1(int v)
        {
            remain_sell_volume[1] = v;
        }
        public void setRemainSellPrice2(float v)
        {
            remain_sell_price[2] = v;
        }
        public void setRemainSellVolume2(int v)
        {
            remain_sell_volume[2] = v;
        }
        //========================================
        public void setTotalVolume(int v)
        {
            total_volume = v;
        }

        public void setCe(float v)
        {
            ce = v;
        }

        public void setFloor(float v)
        {
            floor = v;
        }

        public void setRef(float v)
        {
            reference = v;
        }

        public void setState(int s)
        {
            
        }
        public void setID(int s)
        {
            id = (short)s;
        }

        public void setMarketID(int s)
        {
            floorId = (short)s;
        }

        public short getMarketID()
        {
            return (short)floorId;
        }

        public void setCode(byte[] code, int off)
        {
            String s = Utils.bytesNullTerminatedToString(code, off, 20);
            setCode(s);
        }

        public void setCode(string code)
        {
            this.code = code;
        }

        public void copyFrom(stPriceboardState ps)
        {
            this.code = ps.code;     //  10
            this.floorId = ps.floorId;
            this.state = ps.state;
            for (int i = 0; i < 3; i++)
            {
                this.remain_sell_volume[i] = ps.remain_sell_volume[i];
                this.remain_buy_volume[i] = ps.remain_buy_volume[i];
            }
            this.current_volume_1 = ps.current_volume_1;
            this.total_volume = ps.total_volume;
            this.ce = ps.ce;
            this.reference = ps.reference;
            this.floor = ps.floor;
            this.max = ps.max;
            this.min = ps.min;
            for (int i = 0; i < 3; i++)
            {
                this.remain_buy_price[i] = ps.remain_buy_price[i];
                this.remain_buy_volume[i] = ps.remain_buy_volume[i];
            }
            this.current_price_1 = ps.current_price_1;
            this.change = ps.change;
            this.changePercent = ps.changePercent;		//	1000	==	1%
            this.id = ps.id; 
        }
    }
}