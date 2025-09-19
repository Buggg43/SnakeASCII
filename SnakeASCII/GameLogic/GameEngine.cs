using SnakeASCII.objects;
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
        public enum Direction { Up, Down, Left, Right }

        public void UpdateGamePanel((int x, int y) gameWindow, int playerPoints, Snake snake, List<Fruits> fruits, int playerMoves, bool playerMoved)
        {
            int frameStartWidth = gameWindow.x / 5;
            Console.SetCursorPosition(frameStartWidth + 1, gameWindow.y + 2);
            Console.WriteLine($"Player points: {playerPoints}, Fruits: {fruits.Count}");
            Console.SetCursorPosition(frameStartWidth + 1, gameWindow.y + 3);
            Console.WriteLine($"Player positionX: {snake.Segments[0].x}, positionY: {snake.Segments[0].y}");
            Console.SetCursorPosition(frameStartWidth + 1, gameWindow.y + 4);
            Console.WriteLine($"moves left for fruit to spawn: {playerMoves}");
            playerMoved = false;
            GameOver(snake, gameWindow);
            Console.SetCursorPosition(snake.Segments[0].x, snake.Segments[0].y);
        }
        public int Move(List<Fruits>fruits,Snake snake, Direction dir,ref int playerMoves)
        {
            var head = snake.Segments[0];
            Direction currentDirection = dir;
            (int dx, int dy) = currentDirection switch
            {
                Direction.Up => (0, -1),
                Direction.Down => (0, 1),
                Direction.Left => (-1, 0),
                Direction.Right => (1, 0),
                _ => (0, 0)
            };
            snake.Segments.Insert(0, (head.x + dx, head.y + dy));
            var ateFruit = fruits.Any(f => f.Positions == snake.Segments[0]);
            if (!ateFruit)
            {
                snake.Segments.Remove(snake.Segments.Last());
            }
            else
            {
                snake.SnakeLength = snake.Segments.Count;
                fruits.RemoveAll(s=>s.Positions ==snake.Segments[0]);
            }
            return playerMoves--;
        }
        public Snake PlayerMove(Snake snake, List<Fruits> fruits, int playerPoints,ref int playerMoves,(int x, int y) gameWindow)
        {
            Direction currentDirection = new Direction();
            if (Console.KeyAvailable)
            {
                var head = snake.Segments[0];
                bool playerMoved = false;

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    playerMoved = true;
                    currentDirection = Direction.Up;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    playerMoved = true;
                    currentDirection = Direction.Down;
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    playerMoved = true;
                    currentDirection = Direction.Left;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    playerMoved = true;
                    currentDirection = Direction.Right;
                }
                if (playerMoved)
                {
                    RemoveSnake(snake, gameWindow);
                    Move(fruits, snake, currentDirection,ref playerMoves);
                    UpdateGamePanel(gameWindow,playerPoints,snake,fruits,playerMoves,playerMoved);
                    DrawSnake(snake, gameWindow);
                }
            }
            return snake;
        }
        public void RemoveSnake(Snake snake, (int x, int y) gameWindow)
        {
            foreach (var segment in snake.Segments)
            {
                Console.SetCursorPosition(segment.x, segment.y);
                Console.Write(" ");
            }
            Console.SetCursorPosition(snake.Segments[0].x, snake.Segments[0].y);
        }
        public void DrawSnake(Snake snake, (int x, int y) gameWindow)
        {
            foreach (var segment in snake.Segments) 
            {
                Console.SetCursorPosition(segment.x, segment.y);
                Console.Write("@");
            }
            Console.SetCursorPosition(snake.Segments[0].x, snake.Segments[0].y);
        }
        public void GameOver(Snake snake, (int x, int y) gameWindow)
        {
            if (snake.Segments[0].x < 2 || snake.Segments[0].y < 2 || snake.Segments[0].x >= gameWindow.x || snake.Segments[0].y >= gameWindow.y)
            {
                Console.SetCursorPosition(gameWindow.x/2, gameWindow.y/2);
                Console.WriteLine("###### YOU LOSE ######");
                Console.ReadKey();
                Console.Clear();

                DrawField(gameWindow);
                Console.SetCursorPosition(gameWindow.x / 2, gameWindow.y / 2);
                Console.Write("@");
                snake.Segments.Clear();
                snake.Segments.Insert(0, (gameWindow.x / 2, gameWindow.y / 2));
            }
        }
        public List<Fruits> DrawFruits(Snake snake,List<Fruits> fruits, (int windowWidth, int windowHeight) gameWindow,Random rng)
        {
            bool emptySpot = false;
            (int x, int y) newFruit = (0,0);
            while (emptySpot == false)
            {
                newFruit = (rng.Next(1, gameWindow.windowWidth - 1), rng.Next(1, gameWindow.windowHeight - 1));
                if (!fruits.Any(f => f.Positions == newFruit) && !snake.Segments.Any(s => s.x == newFruit.Item1 && s.y == newFruit.Item2))
                {
                    emptySpot = true;
                }
            }
            var fruit = new Fruits()
            {
                Positions = newFruit
            };
            fruits.Add(fruit);
            Console.SetCursorPosition(fruit.Positions.x,fruit.Positions.y);
            Console.Write("*");
            return fruits;
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
