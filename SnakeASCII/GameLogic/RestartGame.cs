using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnakeASCII.GameLogic
{
    public class RestartGame
    {
        public void restart (ref int playerPoints, ref int movesForFruit, ref int sameDirectionCount, ref Direction lastDirestion, ref Snake snake, (int x, int y) snakeStartingPosition)
        {
            playerPoints = 0;
            movesForFruit = 10;
            sameDirectionCount = 0;
            lastDirestion = Direction.Right;
            snake.Segments.Clear();
            snake.Segments.Insert(0, snakeStartingPosition);
        }
    }
}
