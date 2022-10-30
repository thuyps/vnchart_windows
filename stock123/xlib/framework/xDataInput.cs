using System;
using System.Collections.Generic;
using System.Text;

namespace xlib.framework
{
    public class xDataInput
    {
        byte[] mData;
        int mOffset;
        int mSize;
        int mNumBytesRead;

        public xDataInput()
        {
        }

        public xDataInput(int capacity)
        {
            if (capacity > 0)
            {
                mData = new byte[capacity];
                mSize = 0;
                mOffset = 0;
                mNumBytesRead = 0;
            }
        }

        public xDataInput(byte[] data, int offset, int size, bool makeCopy)
        {
            // Make our own copy of the data.
            mSize = size;
            mNumBytesRead = 0;

            if (makeCopy)
            {
                mData = new byte[size];
                Buffer.BlockCopy(data, offset, mData, 0, size);
                mOffset = 0;
            }
            else
            {
                mData = data;
                mOffset = offset;
            }

            mNumBytesRead = 0;
        }

        public int available()
        {
            return mSize - mNumBytesRead;
        }

        public int read(byte[] aByteArray, int aOffset, int aLength)
        {
            int length = aLength;
            if (length > available())
            {
                length = available();
            }

            Buffer.BlockCopy(mData, mOffset + mNumBytesRead, aByteArray, aOffset, length);

            mNumBytesRead += length;

            return length;
        }

        public bool readBoolean()
        {
            byte b = readByte();

            if (b > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte readByte()
        {
            if (available() < 1)
            {
                return 0;
            }

            byte b = mData[mOffset + mNumBytesRead];
            mNumBytesRead++;

            return b;
        }

        public String readUTFString()
        {
            return readUTF();
        }

        public String readUTF()
        {
            if (available() < 2)
            {
                return null;
            }

            // Find the length of the string.
            int length = readShort();

            if (available() < length)
            {
                return null;
            }

            // Get the characters of the string.
            byte[] tBuf = new byte[length];
            Buffer.BlockCopy(mData, mOffset + mNumBytesRead, tBuf, 0, length);
            System.Text.UTF8Encoding utf8 = new UTF8Encoding();
            String s = utf8.GetString(tBuf);

            // Update the number of bytes read.
            mNumBytesRead += length;

            // Return the string.
            return s;
        }

        public String toUTF8String()
        {
            try
            {
                return Encoding.UTF8.GetString(getBytes(), 0, size());
                //return BitConverter.ToString(getBytes(), 0, size());
            }
            catch (Exception e)
            {
                utils.Utils.trace(e.Message);
            }

            return null;
        }

        public short readShort()
        {
            if (available() < 2)
            {
                return 0;
            }

            int s = (readByte() << 8) | readByte();

            return (short)s;
        }

        public int readInt()
        {
            return readLong();
        }
        public float readFloat()
        {
            return (float)readLong() / 1000;
        }

        //-------------------------------
        public static unsafe float Int32BitsToSingle(int value)
        {
            return *(float*)(&value);
        }

        public float readFloatJava()
        {
            int intValue = readInt();

            float f = Int32BitsToSingle(intValue);

            return f;
        }
        //-------------------------------

        public int readLong()
        {
            if (available() < 4)
            {
                return 0;
            }

            int l = readByte() << 24;
            l |= readByte() << 16;
            l |= readByte() << 8;
            l |= readByte();

            return l;
        }

        public void skipUTF()
        {
            readUTF();
        }
        public void skip(int skipLen)
        {
            if (skipLen == -1)
            {
                skipLen = available();
            }
            if (available() < skipLen)
            {
                skipLen = available();
            }

            mNumBytesRead += skipLen;
        }

        public void seek(int position)
        {
            reset();
            skip(position);
        }
        public byte[] getBytes()
        {
            return mData;
        }
        public int getCurrentOffset()
        {
            return mOffset + mNumBytesRead;
        }
        public int size()
        {
            return mSize;
        }
        public void reset()
        {
            mNumBytesRead = 0;
        }
        public void bind(byte[] data, int offset, int len)
        {
            mData = data;
            mNumBytesRead = 0;
            mOffset = offset;
            mSize = len;
        }
    }
}
