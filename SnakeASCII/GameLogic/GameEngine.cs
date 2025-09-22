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
        public enum Direction { Up, Down, Left, Right, Zero }

        public void UpdateGamePanel((int x, int y) gameWindow, int playerPoints, Snake snake, List<Fruits> fruits, int playerMoves, bool playerMoved, ref int sameDirectionCount)
        {
            int frameStartWidth = gameWindow.x / 5;
            int frameEndWidth = (gameWindow.x / 2) + (gameWindow.x / 5);
            Console.SetCursorPosition(frameStartWidth + 1, gameWindow.y + 2);
            Console.Write($"Player points: {playerPoints}, Fruits: {fruits.Count}");
            Console.SetCursorPosition(frameStartWidth + 1, gameWindow.y + 3);
            Console.Write($"Player positionX: {snake.Segments[0].x}, positionY: {snake.Segments[0].y}");
            Console.SetCursorPosition(frameStartWidth + 1, gameWindow.y + 4);
            Console.Write($"moves left for fruit to spawn: {playerMoves}".PadRight(frameEndWidth - frameStartWidth - 1));
            Console.WriteLine(sameDirectionCount);
            playerMoved = false;
            GameOver(snake, gameWindow, ref sameDirectionCount);
            Console.SetCursorPosition(snake.Segments[0].x, snake.Segments[0].y);
        }
        /* public int Move(List<Fruits>fruits,Snake snake, Direction dir,ref int playerMoves)
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
            bool playerMoved = false;
            Direction currentDirection = new Direction();
            if (Console.KeyAvailable)
            {
                var head = snake.Segments[0];
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
                    DrawSnake(snake, gameWindow);
                }
            }
            UpdateGamePanel(gameWindow, playerPoints, snake, fruits, playerMoves, playerMoved);
            return snake;
        } */
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
                Direction.Zero => (0, 0),
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
        public void SnakeSpeed(int sameDirectionCount)
        {
            int minDelay = 20;
            int maxDelay = 300;
            var frameDelayMs = int.Max(minDelay, maxDelay - (sameDirectionCount * 5));
            Thread.Sleep(frameDelayMs);
        }
        public Snake PlayerMove(Snake snake, List<Fruits> fruits, int playerPoints,ref int playerMoves,(int x, int y) gameWindow,ref Direction LastDirection, ref int sameDirectionCount)
        {
            bool playerMoved = false;
            Direction currentDirection = LastDirection;
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow && LastDirection != Direction.Down)
                {
                    playerMoved = true;
                    currentDirection = Direction.Up;
                }
                else if (key.Key == ConsoleKey.DownArrow && LastDirection != Direction.Up)
                {
                    playerMoved = true;
                    currentDirection = Direction.Down;
                }
                else if (key.Key == ConsoleKey.LeftArrow && LastDirection != Direction.Right)
                {
                    playerMoved = true;
                    currentDirection = Direction.Left;
                }
                else if (key.Key == ConsoleKey.RightArrow && LastDirection != Direction.Left)
                {
                    playerMoved = true;
                    currentDirection = Direction.Right;
                }

            }
            if (currentDirection == LastDirection)
            {
                sameDirectionCount++;
            }
            else
            {
                sameDirectionCount = 0;
            }
            if (playerMoved)
            {
                RemoveSnake(snake, gameWindow);
                Move(fruits, snake, currentDirection, ref playerMoves);
                DrawSnake(snake, gameWindow);
            }
            else
            {
                RemoveSnake(snake, gameWindow);
                Move(fruits, snake, LastDirection, ref playerMoves);
                DrawSnake(snake, gameWindow);
            }
            LastDirection = currentDirection;
            UpdateGamePanel(gameWindow, playerPoints, snake, fruits, playerMoves, playerMoved,ref sameDirectionCount);
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
        public void GameOver(Snake snake, (int x, int y) gameWindow, ref int sameDirectionCount)
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
            sameDirectionCount = 0;
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
