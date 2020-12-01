/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
using System;
using System.Text;
using xlib.framework;

/**
 *
 * @author ThuyPham
 */
namespace xlib.utils
{
    public class xTimer
    {
        int mExpirationMilliseconds;
        long mStartTime;

        public xTimer(int expiration)
        {
            mExpirationMilliseconds = expiration;
            mStartTime = Utils.currentTimeMillis();
        }

        public void reset()
        {
            mStartTime = Utils.currentTimeMillis();
        }

        // Test if the timer has expired.
        public bool isExpired()
        {
            long t = Utils.currentTimeMillis();
            int d = (int)(t - mStartTime);
            if (d < 0)
            {
                mStartTime = t;
            }
            return d >= mExpirationMilliseconds;
        }

        // Get the number of elapsed milliseconds.
        public int getElapsed()
        {
            long t = Utils.currentTimeMillis();
            int d = (int)(t - mStartTime);
            return d;
        }

        // Set the expiration.
        public void setExpiration(int aExpiration)
        {
            mStartTime = Utils.currentTimeMillis();
            mExpirationMilliseconds = aExpiration;
        }

        public int getExpiration()
        {
            return mExpirationMilliseconds;
        }

        public void expireTimer()
        {
            mStartTime = Utils.currentTimeMillis() - mExpirationMilliseconds;
        }
    }
}
