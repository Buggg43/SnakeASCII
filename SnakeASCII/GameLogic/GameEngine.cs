using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnakeASCII.GameLogic
{
    public class GameEngine
    {
     
        public void GameOver(Snake snake, (int x, int y) gameWindow, ref int sameDirectionCount)
        {
            if (snake.Segments[0].x < 2 || snake.Segments[0].y < 2 || snake.Segments[0].x >= gameWindow.x || snake.Segments[0].y >= gameWindow.y)
            {
                Console.SetCursorPosition(gameWindow.x/2, gameWindow.y/2);
                Console.WriteLine("###### YOU LOSE ######");
                Console.ReadKey();
                Console.Clear();

                //DrawField(gameWindow);
                Console.SetCursorPosition(gameWindow.x / 2, gameWindow.y / 2);
                Console.Write("@");
                snake.Segments.Clear();
                snake.Segments.Insert(0, (gameWindow.x / 2, gameWindow.y / 2));
            }
            sameDirectionCount = 0;
        }
    }
}
