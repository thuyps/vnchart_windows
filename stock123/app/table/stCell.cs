using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace stock123.app.table
{
    class stCell
    {
        public String text;	//	enough?
        public String text2;
        public bool bold;
        public Font f;
        public uint textColor;		//	0: white
        public uint textColor2;		//	0: white
        public uint bgColor;
        public short x;
        public short w; 
    }
}
