/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
using System;
using System.Collections.Generic;
using System.Text;
using xlib.framework;
/**
 *
 * @author ThuyPham
 */
namespace stock123.app.data
{
    public class stShareGroup
    {

        public static int MAX_SHARES_IN_GROUP = 35;
        /*
        public static  int GROUP_TYPE_DEFAULT = 0;
        public static  int GROUP_TYPE_CUSTOME = 1;
        public static  int GROUP_TYPE_TOP_VOLUME = 2;
        public static  int GROUP_TYPE_TOP_INCREASED = 3;
        public static  int GROUP_TYPE_TOP_DECREASED = 4;
         * 
         */
        //=======================================
        // GROUP INDEXES
        public static int ID_GROUP_DEFAULT = -1;
        public static int ID_GROUP_FAVOR = 1;
        public static int ID_GROUP_GLOBAL = 2;
        public static int ID_GROUP_MOST_INC = 3;
        public static int ID_GROUP_MOST_DEC = 4;
        public static int ID_GROUP_MOST_VOL = 5;
        public static int ID_GROUP_MOST_VOL_INC_PERCENT = 6;
        public static int ID_GROUP_GAINLOSS = 9;
        public static int ID_GROUP_MARKET_OVERVIEW = 10;
        public static int ID_GROUP_INDICES = 11;

        //=======================================
        String name;
        xVector mShares = new xVector(10);
        int type;
        int groupId;
        byte[] next_file = { 0, 0, 0, 0, 0, 0, 0, 0 };    //  8 bytes
        bool data_updated = false;
        //========================================

        public stShareGroup()
        {
            name = null;

            type = ID_GROUP_DEFAULT;
        }

        public void setDataUploaded()
        {
            data_updated = true;
        }

        public bool isDataUpdated()
        {
            return data_updated;
        }

        public void setGroupType(int aId)
        {
            this.type = aId;
        }

        public int getGroupType()
        {
            return this.type;
        }
/*
        public void setGroupID(int aId)
        {
            this.groupId = aId;
        }

        public int getGroupID()
        {
            return this.groupId;
        }
*/
        public void clear()
        {
            mShares.removeAllElements();
        }

        public int getTotal()
        {
            return mShares.size();
        }

        public void setName(String _name)
        {
            name = _name;
        }

        public String getName()
        {
            return name;
        }

        public void removeCode(String code)
        {
            int cnt = mShares.size();
            for (int i = 0; i < cnt; i++)
            {
                String c = (String)mShares.elementAt(i);
                if (c.CompareTo(code) == 0)
                {
                    mShares.removeElementAt(i);
                    Share.deleteSavedFile(code);
                    Context.getInstance().mIsFavorGroupChanged = true;

                    break;
                }
            }
        }

        public void clearNextfile()
        {
            for (int i = 0; i < 8; i++)
                next_file[i] = 0;
        }

        public void sort()
        {
            for (int i = 0; i < mShares.size() - 1; i++)
            {
                String smallestCode = (String)mShares.elementAt(i);
                int smallestIdx = i;
                for (int j = i + 1; j < mShares.size(); j++)
                {
                    String code = (String)mShares.elementAt(j);
                    if (smallestCode.CompareTo(code) > 0)
                    {
                        smallestCode = code;
                        smallestIdx = j;
                    }
                }

                if (smallestIdx != i)
                {
                    mShares.swap(i, smallestIdx);
                }
            }
        }

        public bool addCode(String code)
        {
            if (code == null || code.Length == 0)
                return false;
            for (int i = 0; i < mShares.size(); i++)
            {
                String c = (String)mShares.elementAt(i);
                if (c.CompareTo(code) == 0)
                    return false;
            }
            mShares.addElement(code);

            return true;
        }

        public int getIDAt(int idx)
        {
            if (idx < mShares.size())
            {
                String code = (String)mShares.elementAt(idx);
                return Context.getInstance().mShareManager.getShareID(code);
            }
            return 0;
        }

        public String getCodeAt(int idx)
        {
            if (idx < mShares.size())
            {
                return (String)mShares.elementAt(idx);
            }

            return null;
        }

        public void setType(int _t)
        {
            type = _t;
        }

        public int getType()
        {
            return type;
        }

        public bool isName(String name)
        {
            if (this.name == null || name == null)
                return false;

            return this.name.Equals(name);
        }

        public void setNextfile(xDataInput di)
        {
            for (int i = 0; i < 8; i++)
                next_file[i] = di.readByte();
        }

        public byte[] getNextfile()
        {
            return next_file;
        }

        public bool containShare(int id)
        {
            String code = Context.getInstance().mShareManager.getShareCode(id);
            if (code == null)
                return false;
            for (int i = 0; i < mShares.size(); i++)
            {
                String c = (String)mShares.elementAt(i);
                if (c.CompareTo(code) == 0)
                {
                    return true;
                }
            }

            return false;
        }
        public void load(xDataInput di)
        {
            this.setName(di.readUTF());	//	already
            this.setType(di.readInt());

            int members = di.readInt();
            for (int j = 0; j < members; j++)
            {
                String code = di.readUTF();
                this.addCode(code);
            }
        }
        public void save(xDataOutput o)
        {
            o.writeUTF(this.getName());
            o.writeInt(this.getType());

            int members = this.getTotal();
            o.writeInt(members);
            for (int j = 0; j < members; j++)
            {
                o.writeUTF(this.getCodeAt(j));
            }
        }

        public bool isFavorGroup()
        {
            return type == ID_GROUP_FAVOR;
        }
    }
}