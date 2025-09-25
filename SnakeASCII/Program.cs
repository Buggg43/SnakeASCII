using SnakeASCII.DTOs;
using SnakeASCII.GameLogic;
using SnakeASCII.Models;
using SnakeASCII.Services;
using System.Xml.Linq;
internal class Program
{
    private static void Main()
    {
        Console.CursorVisible = false;
        Console.Clear();

        Direction lastDirection = Direction.Right;
        int sameDirectionCount = 0, movesForFruit = 0, playerPoints = 0, snakeSpeed;
        SnakeServiceDto snakeMove;
        Input input = new Input();
        Board board = new Board();
        Random rng = new Random();
        SnakeService snakeService = new SnakeService();
        FruitService fruitService = new FruitService();
        Renderer renderer = new Renderer();
        Snake snake = new Snake();
        var snakeStartingPosition = (board.GameWindowWidth / 2, board.GameWindowHeight / 2);
        snake.Segments.Add(snakeStartingPosition);
        List<Fruit> fruits = new List<Fruit>();
        RestartGame restartGame = new RestartGame();


        renderer.DrawField((board.GameWindowWidth, board.GameWindowHeight));
        renderer.UpdateGamePanel((board.GameWindowWidth, board.GameWindowHeight), playerPoints, snake.SnakeLength, movesForFruit, sameDirectionCount, snake.Segments[0]);
        renderer.DrawSnakeFull(snake);
        while (true)
        {
            var current = lastDirection;
            if(Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                current = input.GetDirection(key, lastDirection);
            }
            sameDirectionCount = (current == lastDirection) ? sameDirectionCount + 1 : 1;

            snakeMove = snakeService.Move(fruits, snake, current, board);

            if (snakeMove.Collision)
            {
                renderer.ShowGameOver((board.GameWindowWidth, board.GameWindowHeight));
                Console.Clear();
                renderer.DrawField((board.GameWindowWidth, board.GameWindowHeight));
                restartGame.Restart(ref playerPoints, ref movesForFruit, ref sameDirectionCount, ref lastDirection, ref snake, snakeStartingPosition, ref fruits);
                renderer.DrawSnakeFull(snake);
                renderer.UpdateGamePanel((board.GameWindowWidth, board.GameWindowHeight), playerPoints, snake.SnakeLength, movesForFruit, sameDirectionCount, snake.Segments[0]);
                continue;
            }
            else
            {
                if (snakeMove.AteFruitPosition.HasValue)
                {
                    fruitService.RemoveFruit(snakeMove.AteFruitPosition.Value, fruits);
                    playerPoints += 10;
                    movesForFruit = 10;
                }
                else if (fruits.Count < 1)
                    movesForFruit = Math.Max(0, movesForFruit - 1);
            }
            if (movesForFruit == 0 && fruits.Count < 1)
            {
                var f = fruitService.TrySpawnFruit(snake, fruits, rng, board);
                if (f != null)
                {
                    fruits.Add(f);
                    renderer.DrawFruit(f.Positions);
                    movesForFruit = 10;
                }
            }
            renderer.DrawSnake(snakeMove);
            snakeSpeed = snakeService.SnakeSpeed(sameDirectionCount);
            renderer.UpdateGamePanel((board.GameWindowWidth, board.GameWindowHeight), playerPoints, snake.SnakeLength, movesForFruit, sameDirectionCount, snake.Segments[0]);
            lastDirection = current;
            Thread.Sleep(snakeSpeed);
        }
    }
}