using System;
using System.Collections.Generic;
using System.Text;

namespace xlib.framework
{
    public class xDataOutput
    {
        byte[] mData;
        int mCursor;
        public xDataOutput(int capacity)
        {
            mData = new byte[capacity];
            mCursor = 0;
        }

        public xDataOutput(byte[] data, int offset, int size)
        {
            if (size > 0)
            {
                mData = new byte[size];
                Buffer.BlockCopy(data, offset, mData, 0, size);
                mCursor = size;
            }
            else
            {
                mData = new byte[128];
                mCursor = 0;
            }
        }

        public byte[] getBytes()
        {
            return mData;
        }
        public int size()
        {
            return mCursor;
        }

        void prepareAppend(int appendSize)
        {
            int size = mData.Length;
            int available = size - mCursor;
            if (available < appendSize)
            {
                int newSize = size + appendSize + 16*1024;

                byte[] p = new byte[newSize];
                if (mCursor > 0){
                    Buffer.BlockCopy(mData, 0, p, 0, mCursor);
                }
                mData = p;
            }
        }

        public void write(byte[] data)
        {
            if (data == null)
                return;
            prepareAppend(data.Length);
            Buffer.BlockCopy(data, 0, mData, mCursor, data.Length);
            mCursor += data.Length;
        }

        public void write(xDataOutput o)
        {
            if (o != null && o.size() > 0)
                write(o.getBytes(), 0, o.size());
        }

        public void write(byte[] data, int offset, int length)
        {
            if (data == null || length == 0)
                return;
            prepareAppend(length);
            Buffer.BlockCopy(data, offset, mData, mCursor, length);
            mCursor += length;
        }
        public void writeBoolean(bool aBool)
        {
            if (aBool)
            {
                writeByte(1);
            }
            else
            {
                writeByte(0);
            }
        }

        public void writeByte(byte aByte)
        {
            prepareAppend(1);
            mData[mCursor++] = aByte;
        }
        public void writeByte(int aByte)
        {
            prepareAppend(1);
            mData[mCursor++] = (byte)aByte;
        }
        public void writeShort(int aShort)
        {
            prepareAppend(2);
            // Write each of the two bytes, one at a time.
            mData[mCursor++] = (byte)((aShort & 0xFF00) >> 8);
            mData[mCursor++] = (byte)(aShort & 0xFF);
        }

        public void writeInt(int aInt)
        {
            writeLong(aInt);
        }

        public void writeInt(uint aInt)
        {
            writeLong((int)aInt);
        }

        public void writeFloat(float v)
        {
            writeLong((int)(v * 1000));
        }
        public void writeLong(int aLong)
        {
            prepareAppend(4);
            // Write each of the four bytes, one at a time.
            mData[mCursor++] = (byte)((aLong & 0xFF000000) >> 24);
            mData[mCursor++] = (byte)((aLong & 0xFF0000) >> 16);
            mData[mCursor++] = (byte)((aLong & 0xFF00) >> 8);
            mData[mCursor++] = (byte)(aLong & 0xFF);
        }

        public void writeUTF(String aString)
        {
            if (aString == null)
                writeShort(0);
            else
            {
                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                byte[] p = enc.GetBytes(aString);

                if (p == null)
                {
                    writeShort(0);
                }
                else
                {
                    writeShort(p.Length);
                    write(p, 0, p.Length);
                }
            }
        }

        public void reset()
        {
            mCursor = 0;
        }
    }
}