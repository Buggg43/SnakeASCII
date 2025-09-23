using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.GameLogic
{
    public class Renderer
    {
        public void UpdateGamePanel((int x, int y) gameWindow, int playerPoints, int snakeLength, int playerMoves, int sameDirectionCount, (int x, int y) snakePosition)
        {
            int frameStartWidth = gameWindow.x / 5;
            int frameEndWidth = (gameWindow.x / 2) + (gameWindow.x / 5);
            Console.SetCursorPosition(frameStartWidth + 1, gameWindow.y + 2);
            Console.Write($"Player points: {playerPoints}, Fruits eaten: {snakeLength -1}, Snake Length: {snakeLength}".PadRight(frameEndWidth - frameStartWidth - 1));
            Console.SetCursorPosition(frameStartWidth + 1, gameWindow.y + 3);
            Console.Write($"Player positionX: {snakePosition.x}, positionY: {snakePosition.y}".PadRight(frameEndWidth - frameStartWidth - 1));
            Console.SetCursorPosition(frameStartWidth + 1, gameWindow.y + 4);
            Console.Write($"moves left for fruit to spawn: {playerMoves}".PadRight(frameEndWidth - frameStartWidth - 1));
        }
        public void RemoveSnake((int x, int y) snakeTail,(int x, int y) snakeHead)
        {
            Console.SetCursorPosition(snakeTail.x, snakeTail.y);
            Console.Write(" ");
            Console.SetCursorPosition(snakeHead.x, snakeHead.y);
        }
        public void DrawSnake((int x, int y) snakeHead)
        {
                Console.SetCursorPosition(snakeHead.x, snakeHead.y);
                Console.Write("@");
        }
        public void DrawFruit((int x, int y) fruitPos)
        { 
            Console.SetCursorPosition(fruitPos.x, fruitPos.y);
            Console.Write("*");
        }
        public void DrawField((int windowWidth, int windowHeight) gameWindow)
        {
            for (int i = 0; i <= gameWindow.windowHeight; i++)
            {
                for (int j = 0; j <= gameWindow.windowWidth; j++)
                {
                    if (i == 0 || i == gameWindow.windowHeight || j == 0 || j == gameWindow.windowWidth)
                        Console.Write("#");
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }

            int frameStartWidth = gameWindow.windowWidth / 5;
            int frameEndWidth = (gameWindow.windowWidth / 2) + (gameWindow.windowWidth / 5);
            Console.SetCursorPosition(frameStartWidth, gameWindow.windowHeight + 1);
            var position = Console.GetCursorPosition();
            for (int i = gameWindow.windowHeight + 1; i <= gameWindow.windowHeight + 5; i++)
            {
                Console.SetCursorPosition(frameStartWidth, i);
                for (int j = frameStartWidth; j <= frameEndWidth; j++)
                {

                    if (i == gameWindow.windowHeight + 1 || j == frameStartWidth || j == frameEndWidth || i == gameWindow.windowHeight + 5)
                        Console.Write("#");
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}
