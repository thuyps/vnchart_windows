using System;
using System.Collections.Generic;
using System.Text;

namespace stock123.app.data
{
    public class ShareComparer:IComparer<object>
    {
        int mSortType;
        public const int SORT_ABC = 0;
        public const int SORT_NORMAL = 1;

        public ShareComparer(int sortType)
        {
            mSortType = sortType;
        }

        public int Compare(object o1, object o2)
        {
            Share s0 = (Share)o1;
            Share s1 = (Share)o2;
            if (s0.mCode == null || s1.mCode == null)
                return 0;
            switch (mSortType)
            {
                case SORT_ABC:
                    return s0.mCode.CompareTo(s1.mCode);
                case SORT_NORMAL:
                    if (s0.mSortParam < s1.mSortParam) return 1;
                    else if (s0.mSortParam > s1.mSortParam) return -1;
                    break;
            }
            return 0;
        }
    }
}
