using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.GameLogic
{
    public class WIndowSync
    {
        public bool DetectResize(out(int winW, int winH) size)
        {
            size = (Console.WindowWidth, Console.WindowHeight);
            if (Console.WindowHeight != Console.BufferHeight || Console.WindowWidth != Console.BufferWidth)
                return true;
            return false;
        }
        public void SyncBufferToWindow((int winW, int winH) size)
        {
            Console.Clear();
            Console.BufferWidth = size.winW;
            Console.BufferHeight = size.winH;
        }
    }
}
