using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotF2.Core.Drawing
{
    public struct IDColor
    {
        public IDColor(byte r, byte g, byte b)
            : this(255, r, g, b)
        {
        }

        public IDColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public byte A;
        public byte R;
        public byte G;
        public byte B;
    }
}
