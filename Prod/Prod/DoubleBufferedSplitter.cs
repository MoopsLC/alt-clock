using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Forms;

namespace Deweyvm.Prod
{
    class DoubleBufferedSplitter : SplitContainer
    {
        public DoubleBufferedSplitter()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint 
                | ControlStyles.UserPaint 
                | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}