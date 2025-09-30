using SnakeASCII.DTOs;
using SnakeASCII.GameLogic;
using SnakeASCII.Models;
using SnakeASCII.Services;

internal class Program
{
    private static void Main()
    {
        Console.CursorVisible = false;
        Console.Clear();

        Direction lastDirection = Direction.Right;
        int sameDirectionCount = 0, movesForFruit = 0, playerPoints = 0, snakeSpeed;
        const int fruitSpawnPeriod = 10;

        SnakeServiceDto snakeMove;
        Input input = new Input();
        Board board = new Board()
        {
            GameWindowWidth = Console.WindowWidth - 1,
            GameWindowHeight = Console.WindowHeight - 8
        };

        Console.BufferWidth = Console.WindowWidth;
        Console.BufferHeight = Console.WindowHeight;

        Random rng = new Random();
        SnakeService snakeService = new SnakeService();
        FruitService fruitService = new FruitService();
        GameResizer gameResizer = new GameResizer();
        Renderer renderer = new Renderer();
        PortalService portalService = new PortalService();
        PortalController portalController = new PortalController(); 

        Snake snake = new Snake();
        var snakeStartingPosition = (board.GameWindowWidth / 2, board.GameWindowHeight / 2);
        snake.Segments.Add(snakeStartingPosition);

        List<Fruit> fruits = new List<Fruit>();
        RestartGame restartGame = new RestartGame();

        renderer.DrawField((board.GameWindowWidth, board.GameWindowHeight));
        renderer.UpdateGamePanel((board.GameWindowWidth, board.GameWindowHeight), playerPoints, snake.Segments.Count, movesForFruit, sameDirectionCount, snake.Segments[0]);
        renderer.DrawSnakeFull(snake);

        while (true)
        {
            var requiredWidth = board.MinWidth;
            var requiredHeight = board.MinHeight;
            if (Console.WindowWidth < requiredWidth || Console.WindowHeight < requiredHeight)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Please resize the window to at least {board.MinWidth}x{board.MinHeight} (current: {Console.WindowWidth}x{Console.WindowHeight})");
                Thread.Sleep(500);
                continue;
            }
            var changed = gameResizer.ResizeBoard(board);
            if (changed)
            {
                Console.Clear();
                renderer.DrawField((board.GameWindowWidth, board.GameWindowHeight));

                bool warpNeeded = snake.Segments.Any(p =>
                    p.x < 1 || p.x > board.GameWindowWidth - 1 ||
                    p.y < 1 || p.y > board.GameWindowHeight - 1);

                if (warpNeeded)
                {
                    var plan = portalService.PlanToCenter(board, snake, lastDirection);
                    lastDirection = plan.newDirection;
                    sameDirectionCount = 1;
                    fruitService.ResetAfterResize(fruits);

                    renderer.DrawSnakeFull(snake);
                    renderer.DrawPortal(plan.portal);
                    renderer.UpdateGamePanel((board.GameWindowWidth, board.GameWindowHeight), playerPoints, snake.Segments.Count, movesForFruit, sameDirectionCount, snake.Segments[0]);

                    portalController.Activate(plan.portal);
                    continue;
                }
                else
                {
                    renderer.DrawSnakeFull(snake);
                    foreach (var f in fruits) renderer.DrawFruit(f.Positions);
                    renderer.UpdateGamePanel((board.GameWindowWidth, board.GameWindowHeight), playerPoints, snake.Segments.Count, movesForFruit, sameDirectionCount, snake.Segments[0]);
                }
            }

            var current = lastDirection;
            if (Console.KeyAvailable)
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
                renderer.UpdateGamePanel((board.GameWindowWidth, board.GameWindowHeight), playerPoints, snake.Segments.Count, movesForFruit, sameDirectionCount, snake.Segments[0]);

                portalController.Reset();
                continue;
            }
            else
            {
                if (snakeMove.AteFruitPosition.HasValue)
                {
                    fruitService.RemoveFruit(snakeMove.AteFruitPosition.Value, fruits);
                    playerPoints += 10;
                    movesForFruit = fruitSpawnPeriod;
                }
                else if (fruits.Count < 1)
                {
                    movesForFruit = Math.Max(0, movesForFruit - 1);
                }
            }
            if (movesForFruit == 0 && fruits.Count < 1)
            {
                var f = fruitService.TrySpawnFruit(snake, fruits, rng, board);
                if (f != null)
                {
                    fruits.Add(f);
                    renderer.DrawFruit(f.Positions);
                    movesForFruit = fruitSpawnPeriod;
                }
            }

            renderer.DrawSnake(snakeMove);

            bool wasActive = portalController.IsActive;
            bool removedTailThisTurn =
                !snakeMove.AteFruitPosition.HasValue &&
                snake.PendingGrowth == 0 &&
                snakeMove.OldTail.HasValue;

            portalController.Tick(removedTailThisTurn);
            if (wasActive && !portalController.IsActive)
            {
                renderer.ClearPortalMarkers(portalController.State, board);
            }

            snakeSpeed = snakeService.SnakeSpeed(sameDirectionCount);
            renderer.UpdateGamePanel((board.GameWindowWidth, board.GameWindowHeight), playerPoints, snake.Segments.Count, movesForFruit, sameDirectionCount, snake.Segments[0]);

            lastDirection = current;
            Thread.Sleep(snakeSpeed);
        }
    }
}
