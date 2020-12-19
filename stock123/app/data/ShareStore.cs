using System;
using System.Collections.Generic;
using System.Text;

using xlib.framework;
using xlib.utils;

namespace stock123.app.data
{
    public class ShareStore
    {
        public const int NET_CANDLE_SIZE = 32;
        public const int MAX_CANDLES = 800;
        public const int CANDLE_SIZE			= 20;		//	sizeof(stCandle)
        public const int HEADER_OFFSET		    = 16;
        public const int SHARE_CODE_LENGTH = 8;
        //	for new shares
	    xVector mNewShareIDs = new xVector(10);		//	String:char[SHARE_CODE_LENGTH]
        xVector mNewShares = new xVector(10);   //  Share

        //	temp variable
        xDataInput mDI = new xDataInput();

        byte[] mBuffer;
        int mBufferSize;

        int mHeaderSize;
        int mDataOffset;
        int mShareCnt;

        //==============================================
        void clearMemory()
        {
            mBuffer = null;

            mBufferSize = 0;
            mHeaderSize = 0;
            mDataOffset = 0;
            mShareCnt = 0;

            mNewShares.removeAllElements();
            mNewShares.removeAllElements();
            mNewShareIDs.removeAllElements();
        }

        void loadData()
        {
            xDataInput di = xFileManager.readFile(Context.FILE_STORE_NAME, false);
            if (di != null)
            {
                clearMemory();

                mBufferSize = di.available();
                int ver = di.readInt();
                if (ver == Context.FILE_VERSION)
                {
                    mHeaderSize = di.readInt();
                    mDataOffset = di.readInt();
                    mShareCnt = di.readInt();

                    mBuffer = di.getBytes();
                }
            }
        }

        //	adding data format: int:open, int:close, int highest, int lowest, int floor, int ref, int ce, int volume, int date
//	36bytes for each candle
        static int totalSize = 0;
        /*
        void addData2(byte[] code, int floor, byte[] data, int offset, int data_size)
        {
            if (code[0] == (byte)'^')
                return;
            //	if (strcmp(code, "BHS") == 0)
            //	{
            //		int k = 0;
            //	}
            int i, j;
            totalSize += data_size;

            int candle_cnt = data_size / NET_CANDLE_SIZE;

            int candleSize = CANDLE_SIZE;
            int size = candle_cnt * candleSize;

            //	if (strlen(code) > 3)
            //	{
            //		int k = 0;
            //	}

            int off = 0;
            int[] offsets = { 0 };
            byte[] p0 = seekTo(code, offsets);
            off = offsets[0];

            int p1 = off+8;

            if (p0 == null)
                return;

            int old_candle_cnt = Utils.readInt(p0, off);
            //	move backward memory in buff
            int buffSize = MAX_CANDLES * candleSize;
            int size2Move = candleSize * old_candle_cnt;
            int remain = buffSize - size;
            if (remain < size2Move)
                size2Move = remain;

            int total_candle_save = 0;
            if (size2Move > 0)
            {
                int end = size + size2Move;
                for (i = 0; i < size2Move; i++)
                    p0[p1 + i + size] = p0[p1 + i]; //  copy to the end
                //memmove(p1 + size, p1, size2Move);
                total_candle_save = size2Move / candleSize;
            }
            //	if network data is too large, skip some first candles
            int candleAccept = candle_cnt;
            if (candleAccept > MAX_CANDLES)
                candleAccept = MAX_CANDLES;
            int skipCandle = candle_cnt - candleAccept;

            int c0 = 0;
            int postP = (candleAccept - 1) * candleSize;

            int tmp = 0;

            j = 0;
            if (skipCandle > 0)
                j = skipCandle * NET_CANDLE_SIZE;

            stCandle c = new stCandle();

            for (i = skipCandle; i < candle_cnt; i++)
            {
                total_candle_save++;
                c0 = p1 + postP;	//	move backward
                postP -= candleSize;

                c.init(p0, c0);

                int open = Utils.readInt(data, j) / 100; j += 4;
                int close = Utils.readInt(data, j) / 100; j += 4;
                int highest = Utils.readInt(data, j) / 100; j += 4;
                int lowest = Utils.readInt(data, j) / 100; j += 4;
                //j += 4;//floor = di.readInt()/100;		j += 4;
                int reference = Utils.readInt(data, j) / 100; j += 4;

                int ce = Utils.readInt(data, j) / 100; j += 4;

                int volume = Utils.readInt(data, j); j += 4;
                int date = Utils.readInt(data, j); j += 4;
                //	correct data:
                if (open != 0) tmp = open;
                if (close != 0 && tmp == 0) tmp = close;
                if (highest != 0 && tmp == 0) tmp = highest;
                if (lowest != 0 && tmp == 0) tmp = lowest;
                if (reference != 0 && tmp == 0) tmp = reference;
                if (ce != 0 && tmp == 0) tmp = ce;

                if (open == 0) open = tmp;
                if (close == 0) close = tmp;
                if (highest == 0) highest = tmp;
                if (lowest == 0) lowest = tmp;
                if (reference == 0) reference = close;

                if (ce == 0) ce = tmp;

                c.setOpen(open);
                c.setClose(close);
                c.setCe(ce);
                c.setRef(reference);
                c.setVolume(volume);
                c.setDate(date);
                c.setHighest(highest);
                c.setLowest(lowest);

                if (tmp == 0)	//	reject bad candle
                {
                    i--;
                    candle_cnt--;
                }
            }

            Utils.writeInt(p0, off+0, total_candle_save);
            Utils.writeInt(p0, off+4, floor);
        }
         */

//	addDataToShare for INDICES only
//	adding data format: int:open, int:close, int highest, int lowest, int floor, int ref, int ce, int volume, int date
//	36bytes for each candle
//	first candle is earlies
        /*
        void addDataToShare(Share s, byte[] data, int offset, int data_size)
        {
            int candle_cnt = data_size / NET_CANDLE_SIZE;
            xDataInput di = mDI;
            di.bind(data, offset, data_size);

            int tmp = 0;
            stCandle tmpCandle;

            int lastDate = s.getLastCandleDate();

            for (int i = 0; i < candle_cnt; i++)
            {
                int open = di.readInt() / 10;
                int close = di.readInt() / 10;
                int highest = di.readInt() / 10;
                int lowest = di.readInt() / 10;
                int reference = di.readInt() / 10;

                int ce = di.readInt() / 10;

                int volume = di.readInt();
                int date = di.readInt();
                //	correct data:
                if (open != 0) tmp = open;
                if (close != 0 && tmp == 0) tmp = close;
                if (highest != 0 && tmp == 0) tmp = highest;
                if (lowest != 0 && tmp == 0) tmp = lowest;
                if (reference != 0 && tmp == 0) tmp = reference;
                if (ce != 0 && tmp == 0) tmp = ce;

                if (open == 0) open = tmp;
                if (close == 0) close = tmp;
                if (highest == 0) highest = tmp;
                if (lowest == 0) lowest = tmp;
                if (reference == 0) reference = tmp;

                if (ce == 0) ce = tmp;
                if (tmp == 0)	//	reject bad candle
                {
                    i--;
                    candle_cnt--;
                }

                if (lastDate != 0)
                {
                    if (date < lastDate)
                        continue;		//	skip old candle
                    else
                        lastDate = 0;
                }

                s.addMoreCandle(open, close, reference, highest, lowest, volume, date);
            }
        }
*/
        byte[] seekTo(byte[] code, int[] out_offset)
        {
            if (mBuffer != null)
            {
                //		int i = 0;

                int off = 0;
                int cnt = mShareCnt;	//	share count
                off = HEADER_OFFSET;	//	skip file header
                xDataInput di = mDI;
                di.bind(mBuffer, off, mShareCnt * (SHARE_CODE_LENGTH + 4));
                for (int i = 0; i < cnt; i++)
                {
                    int di_off = di.getCurrentOffset();

                    bool equal = Utils.strcmp(mBuffer, di_off, code, 0, Share.SHARE_CODE_LENGTH);

                    if (equal)
                    {
                        di.skip(Share.SHARE_CODE_LENGTH);
                        int offset = di.readInt();
                        out_offset[0] = mDataOffset + offset;
                        return mBuffer;
                    }
                    di.skip(SHARE_CODE_LENGTH + 4);
                }
            }

            out_offset[0] = 0;
            //	not found, 
            for (int i = 0; i < mNewShareIDs.size(); i++)
            {
                byte[] c = (byte[])mNewShareIDs.elementAt(i);
                if (Utils.strcmp(code, 0, c, 0, Share.SHARE_CODE_LENGTH))
                {
                    return (byte[])mNewShares.elementAt(i);
                }
            }

            byte[] newCode = new byte[Share.SHARE_CODE_LENGTH + 10];
            Utils.strcpy(newCode, code);
            newCode[Share.SHARE_CODE_LENGTH - 1] = 0;

            mNewShareIDs.addElement(newCode);

            byte[] data = new byte[4 + 4 + MAX_CANDLES * CANDLE_SIZE];		//	candle_cnt + floor + dataOfCandle

            //	candle count = 0
            Utils.writeInt(data, 0, 0);
            mNewShares.addElement(data);

            return data;
        }

        void saveData()
        {
            int totalShares = mShareCnt + mNewShareIDs.size();
            int headerSize = totalShares * (SHARE_CODE_LENGTH + 4);	//	code-length + offset
            int dataOffset0 = HEADER_OFFSET + headerSize;
            int share_size = CANDLE_SIZE * MAX_CANDLES + 4 + 4;	//	4 == number of candles, 4 == marketID
            int total_size = HEADER_OFFSET + headerSize + totalShares * share_size;
            byte[] p = new byte[total_size];

            int j = 0;
            int off = 0;
            //	16 bytes file header
            Utils.writeInt(p, off, Context.FILE_VERSION);	//	version
            Utils.writeInt(p, off + 4, headerSize);		//	header size
            Utils.writeInt(p, off + 8, dataOffset0);		//	starting of data_offset
            Utils.writeInt(p, off + 12, totalShares);	//	total shares

            //	data entry 
            j = HEADER_OFFSET;
            if (mBuffer != null)
            {
                Buffer.BlockCopy(mBuffer, HEADER_OFFSET, p, j, mHeaderSize);
                //memcpy(p+j, mBuffer+HEADER_OFFSET, mHeaderSize);
            }
            int newDataOffset = mShareCnt * share_size;
            j += mHeaderSize;
            //	adding more data entry ( for new shares )
            for (int i = 0; i < mNewShareIDs.size(); i++)
            {
                byte[] code = (byte[])mNewShareIDs.elementAt(i);
                int offset = newDataOffset;
                Utils.writeBytes(p, j, code, 0, Share.SHARE_CODE_LENGTH);
                Utils.writeInt(p, j + Share.SHARE_CODE_LENGTH, offset);

                j += 12;
                newDataOffset += share_size;
            }
            //	data area
            j = dataOffset0;
            if (mBuffer != null)
            {
                Buffer.BlockCopy(mBuffer, mDataOffset, p, j, mShareCnt * share_size);
                //memcpy(p+j, mBuffer+mDataOffset, mShareCnt*share_size);
            }
            j += mShareCnt * share_size;

            for (int i = 0; i < mNewShareIDs.size(); i++)
            {
                byte[] pn = (byte[])mNewShares.elementAt(i);
                Buffer.BlockCopy(pn, 0, p, j, share_size);
                //memcpy(p+j, pn, share_size);
                j += share_size;
            }

            xFileManager.saveFile(p, 0, total_size, Context.FILE_STORE_NAME);

            p = null;
        }
    }
}
