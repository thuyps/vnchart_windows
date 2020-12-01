using System;
using System.Collections.Generic;
using System.Text;
using xlib.framework;
using xlib.ui;

namespace xlib.utils
{
    public class xResourceManager
    {
        int mItemCnt;
        byte[] mHeader;
        xDataInput mData;
        public xResourceManager(string resfile)
        {    
            xDataInput di = xFileManager.readFile(resfile, true);
            init(di);
        }

        void init(xDataInput di)
        {
            mItemCnt = 0;
            if (di == null || di.available() < 10)
                return;
            //=================================================
            //	item_cnt | header session | data
            //=================================================
            //	header session: flag:1 | offset:3 | size:4, total 8 bytes
            //=================================================
            //check signature
            //========================

            const string sig = "ezze";
            for (int i = 0; i < 4; i++)
            {
                if (di.readByte() != sig[i])
                {
                    return;
                }
            }
            int size0 = di.readInt();   //  little endien
            int size = Utils.convertBigIntToLittleInt(size0);
            if (di.available() != size + 4)    //  last 4 ezze
                return;
            //=======================================
            //	get number of item
            size0 = di.readInt();
            mItemCnt = Utils.convertBigIntToLittleInt(size0);

            if (mItemCnt > 0)
            {
                //	read header
                size = 64 * mItemCnt;
                mHeader = new byte[size];
                di.read(mHeader, 0, size);
            }

            mData = di;
        }

        public xDataInput getResourceAsStream(string filePath)
        {
            int[] len = { 0 };
            byte[] data;

            data = getResource(filePath, len);
            if (data != null)
            {
                return new xDataInput(data, 0, len[0], false);
            }

            return null;
        }

        // get a resource from the compressed chunks
        byte[] getResource(string filePath, int[] size)
        {
            if (mData == null)
                return null;
            size[0] = 0;
            //	looking for the position of the item
            int pos = -1;
            int itemSize = 16 * 4;	//	16 integers for each
            byte[] s0 = new byte[filePath.Length];
            int i = 0;
            int n = 0;
            for (i = 0; i < filePath.Length; i++)
            {
                s0[i] = (byte)filePath[i];
            }
            for (i = 0; i < mItemCnt; i++)
            {
                n = i * itemSize;
                //const char* s = (const char*)&mHeader[i*itemSize];
                if (utils.Utils.strcmp(s0, 0, mHeader, n, s0.Length))
                {
                    pos = n;
                    break;
                }
            }

            if (pos == -1)
                return null;

            pos += (14 * 4);	//	14*4 is the length of filename

            n = Utils.convertBigIntToLittleInt(Utils.readInt(mHeader, pos));
            int flag = n >> 24;
            int offset = (n & 0x00FFFFFF) + 8;
            n = Utils.convertBigIntToLittleInt(Utils.readInt(mHeader, pos + 4));
            int dataLen = n;// mHeader[pos + 1];

            //	calc offset in the real resource file
            offset = offset + mItemCnt * 64 + 4;	//	offset + header_len + 4(number_of_items)

            if (offset >= mData.size())
            {
                return null;
            }

            mData.reset();
            mData.skip(offset);

            //=====================================================
            //	if data is compressed, then data has the format:
            //	compressed_data | actual_size:4
            //=====================================================

            byte[] pData = null;
            int outSize = 0;

            //	if data is compressed
            if (flag != 0)	//	data is compressed
            {
            }
            else
            {
                pData = new byte[dataLen];
                mData.read(pData, 0, dataLen);
                //	data is in raw format, return now
                size[0] = dataLen;
                return pData;
            }

            return null;
        }

        public int getItemCount()
        {
            return mItemCnt;
        }
    }
}
