using SnakeASCII.GameLogic;
using SnakeASCII.Models;
using static SnakeASCII.GameLogic.GameEngine;
internal class Program
{
    private static void Main()
    {
        Console.CursorVisible = false;
        Console.Clear();
        var engine = new GameEngine();

        (int windowWidth, int windowHeight) gameWindow = (80,20);
        int movesForFruit = 10, playerPoints = 0;

        Snake snake = new Snake();
        snake.Segments.Insert(0,(gameWindow.windowWidth / 2, gameWindow.windowHeight / 2));
        var head = snake.Segments[0];
        var rng = new Random();
        var fruits = new List<Fruit>();
        Direction LastDirection = new Direction();
        LastDirection = Direction.Zero;
        int sameDirectionCount= 0;

        engine.DrawField(gameWindow);
        engine.DrawSnake(snake,gameWindow);
        while (true)
        {
            engine.PlayerMove(snake, fruits, playerPoints, gameWindow,ref LastDirection,ref sameDirectionCount,ref movesForFruit);
            engine.DrawFruits(snake,fruits, gameWindow,rng,ref movesForFruit);

            var collectedFruits = fruits.Where(f => f.Positions == snake.Segments[0]).ToList();
            foreach (var fruit in collectedFruits)
            {
                snake.SnakeLength++;
                fruits.Remove(fruit);
                movesForFruit = 10;
            }
            engine.SnakeSpeed(sameDirectionCount);
        }
    }
}