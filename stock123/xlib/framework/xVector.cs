using System;
using System.Collections.Generic;
using System.Text;

namespace xlib.framework
{
    public class xVector
    {
        List<object> mArray;
        public xVector(int capacity)
        {
            mArray = new List<object>(capacity);
        }
        public xVector()
        {
            mArray = new List<object>();
        }

        public void addElement(object o)
        {
            mArray.Add(o);
        }

        public object elementAt(int idx)
        {
            if (idx < mArray.Count && idx >= 0)
            {
                return mArray[idx];
            }

            return null;
        }

        public int size()
        {
            return mArray.Count;
        }

        public bool contains(object o)
        {
            return mArray.Contains(o);
        }

        public void removeAllElements()
        {
            mArray.Clear();
        }

        public void removeElement(object o)
        {
            mArray.Remove(o);
        }

        public void removeElementAt(int idx)
        {
            if (idx >= 0 && idx < size())
                mArray.RemoveAt(idx);
        }

        public bool isEmpty()
        {
            return mArray.Count == 0;
        }

        public void insertElementAt(object o, int at)
        {
            if (at < mArray.Count)
                mArray.Insert(at, o);
            else
                mArray.Add(o);
        }

        public object firstElement()
        {
            if (mArray.Count > 0)
            {
                return mArray[0];
            }

            return null;
        }

        public object lastElement()
        {
            if (mArray.Count > 0)
            {
                return mArray[mArray.Count - 1];
            }

            return null;
        }

        public object pop()
        {
            object o = null;
            if (mArray.Count > 0)
            {
                int idx = mArray.Count - 1;
                o = mArray[idx];
                mArray.RemoveAt(idx);
            }

            return o;
        }

        public void swap(int idx1, int idx2)
        {
            if ((idx1 >= 0 && idx1 < mArray.Count)
                && (idx2 >= 0 && idx2 < mArray.Count))
            {
                object tmp = mArray[idx1];
                mArray[idx1] = mArray[idx2];
                mArray[idx2] = tmp;
            }
        }

        public List<object> getInternalList()
        {
            return mArray;
        }

        public void makeReverse()
        {
            if (mArray == null)
                return;
            mArray.Reverse();
        }
    }
}
