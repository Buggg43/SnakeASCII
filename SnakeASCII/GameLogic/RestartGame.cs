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
        public void Restart (ref int playerPoints, ref int movesForFruit, ref int sameDirectionCount, ref Direction lastDirestion, ref Snake snake, (int x, int y) snakeStartingPosition, ref List<Fruit> fruits)
        {
            playerPoints = 0;
            movesForFruit = 0;
            sameDirectionCount = 1;
            lastDirestion = Direction.Right;
            snake.Segments.Clear();
            snake.Segments.Insert(0, snakeStartingPosition);
            fruits.Clear();
        }
    }
}
