using System;
using System.Collections.Generic;
using System.Text;

namespace xlib.framework
{
    public delegate void OnEventDelegate(object sender, int evt, int aIntParameter, object aParameter);
    public interface xIEventListener
    {
        void onEvent(object sender, int evt, int aIntParameter, object aParameter);
    }
}
