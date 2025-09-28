using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.GameLogic
{
    public class Input
    {
        public Direction GetDirection(ConsoleKeyInfo keyInfo, Direction lastDirection)
        {
            return keyInfo.Key switch
            {
                ConsoleKey.UpArrow when lastDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when lastDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when lastDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when lastDirection != Direction.Left => Direction.Right,
                _ => lastDirection
            };
        }
    }
}
