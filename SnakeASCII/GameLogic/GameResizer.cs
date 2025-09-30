using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.GameLogic
{
    public class GameResizer
    {
        public bool ResizeBoard(Board board)
        {
            bool boardResized = false;
            int winW = Console.WindowWidth;
            int winH = Console.WindowHeight;

            int effectiveMaxW = Math.Max(board.MinWidth, Console.LargestWindowWidth - board.MarginX);
            int effectiveMaxH = Math.Max(board.MinHeight, Console.LargestWindowHeight - board.MarginY);
            Console.BufferWidth = winW;
            Console.BufferHeight = winH;
            int newWidth = Math.Clamp(winW - board.MarginX, board.MinWidth, effectiveMaxW);
            int newHeight = Math.Clamp(winH - board.MarginY, board.MinHeight, effectiveMaxH);

            // User can resize mutliple times, so only update if there's a change
            if (newWidth != board.GameWindowWidth || newHeight != board.GameWindowHeight)
            {
                board.GameWindowWidth = newWidth;
                board.GameWindowHeight = newHeight;
                boardResized = true;
            }
            return boardResized;
        }
    }
}
