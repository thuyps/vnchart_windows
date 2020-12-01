using System;
using System.Collections.Generic;
using System.Text;

namespace xlib.framework
{
    public interface xIEventListener
    {
        void onEvent(object sender, int evt, int aIntParameter, object aParameter);
    }
}
