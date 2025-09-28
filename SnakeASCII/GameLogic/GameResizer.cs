using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.GameLogic
{
    public class GameResizer
    {
        public Board ResizeBoard(Board board)
        {             
            int minWidth = 20;
            int minHeight = 10;
            int maxWidth = 100;
            int maxHeight = 40;
            int newWidth = Math.Clamp(board.GameWindowWidth, minWidth, maxWidth);
            int newHeight = Math.Clamp(board.GameWindowHeight, minHeight, maxHeight);
            return new Board
            {
                GameWindowWidth = newWidth,
                GameWindowHeight = newHeight
            };
        }
    }
}
