using System;
using System.Collections.Generic;
using System.Text;

namespace xlib.utils
{
    public class xFloat
    {
        float v = 0;
        float getter() { return v; }
        void setter(float _v) { v = _v; }

        public float Value
        {
            get { return getter(); }
            set { setter(value); }
        }
    }
}
