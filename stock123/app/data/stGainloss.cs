using System;
using System.Collections.Generic;
using System.Text;

using xlib.framework;

namespace stock123.app.data
{
    public class stGainloss
    {
        public stGainloss(string _c, int _d, float _p, int _v)
        {
            code = _c;
            date = _d;
            price = _p;
            volume = _v;
        }
        //public int 
        public string code;
        public int date;
        public float price;
        public int volume;
    }

    public class GainLossManager
    {
        const string FILE_GAINLOSS = "gain";
        const int FILE_GAINLOSS_VERSION = 0x01;

        public const uint FILE_GAINLOSS_SIGNAL = 0x000a0b0c;

        xVector mGainlosses;

        public GainLossManager()
        {
            mGainlosses = new xVector(20);
        }

        public int getTotal()
        {
            return mGainlosses.size();
        }

        public stGainloss getGainLossAt(int idx)
        {
            if (idx >= 0 && idx < mGainlosses.size())
            {
                return (stGainloss)mGainlosses.elementAt(idx);
            }

            return null;
        }
        public void addGainLoss(string _c, int _d, float _p, int _v)
        {
            stGainloss g = new stGainloss(_c, _d, _p, _v);
            mGainlosses.addElement(g);
        }
        public void removeGainLoss(string code)
        {
            for (int i = 0; i < mGainlosses.size(); i++)
            {
                stGainloss g = (stGainloss)mGainlosses.elementAt(i);
                if (g.code == code)
                {
                    mGainlosses.removeElementAt(i);
                    break;
                }
            }
        }
        public void removeGainLoss(stGainloss remove)
        {
            for (int i = 0; i < mGainlosses.size(); i++)
            {
                stGainloss g = (stGainloss)mGainlosses.elementAt(i);
                if (g == remove)
                {
                    mGainlosses.removeElementAt(i);
                    break;
                }
            }
        }

        public void load()
        {
            /*
            xDataInput di = xFileManager.readFile(FILE_GAINLOSS, false);
            if (di != null)
            {
                mGainlosses.removeAllElements();
                int ver = di.readInt();
                if (ver == FILE_GAINLOSS_VERSION)
                {
                    int cnt = di.readInt();
                    for (int i = 0; i < cnt; i++)
                    {
                        string code = di.readUTF();
                        int date = di.readInt();
                        float price = di.readFloat();
                        int vol = di.readInt();

                        stGainloss g = new stGainloss(code, date, price, vol);
                        mGainlosses.addElement(g);
                    }

                    sortList();
                }
            }
             */
        }
        public void save()
        {
            xDataOutput o = new xDataOutput(2048);
            o.writeInt(FILE_GAINLOSS_VERSION);

            o.writeInt(mGainlosses.size());
            for (int i = 0; i < mGainlosses.size(); i++)
            {
                stGainloss g = (stGainloss)mGainlosses.elementAt(i);
                o.writeUTF(g.code);
                o.writeInt(g.date);
                o.writeFloat(g.price);
                o.writeInt(g.volume);
            }

            xFileManager.saveFile(o, FILE_GAINLOSS);
        }

        public void sortList()
        {
            for (int i = 0; i < mGainlosses.size() - 1; i++)
            {
                stGainloss a = (stGainloss)mGainlosses.elementAt(i);
                stGainloss smallest = a;
                int smallestIDX = i;
                for (int j = i + 1; j < mGainlosses.size(); j++)
                {
                    stGainloss g = (stGainloss)mGainlosses.elementAt(j);
                    if (smallest.code == g.code)
                    {
                        smallest = g;
                        smallestIDX = j;
                    }
                }

                if (smallestIDX > i)
                {
                    mGainlosses.swap(i, smallestIDX);
                }
            }
        }

        public void clearAll()
        {
            mGainlosses.removeAllElements();
        }
    }
}
