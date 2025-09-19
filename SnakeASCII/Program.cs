using SnakeASCII.GameLogic;
using SnakeASCII.objects;
internal class Program
{
    private static void Main()
    {
        Console.CursorVisible = false;
        Console.Clear();
        var engine = new GameEngine();

        (int windowWidth, int windowHeight) gameWindow = (80,20);
        int playerMoves = 10, playerPoints = 0;

        Snake snake = new Snake();
        snake.Segments.Insert(0,(gameWindow.windowWidth / 2, gameWindow.windowHeight / 2));
        var head = snake.Segments[0];
        var rng = new Random();
        var fruits = new List<Fruits>();

        engine.DrawField(gameWindow);
        engine.DrawSnake(snake,gameWindow);
        while (true)
        {
            engine.PlayerMove(snake, fruits, playerPoints,ref playerMoves, gameWindow);
            if (playerMoves <= 0)
            {
                engine.DrawFruits(snake,fruits, gameWindow,rng);
                playerMoves = 10;
            }
            var collectedFruits = fruits.Where(f => f.Positions == snake.Segments[0]).ToList();
            foreach (var fruit in collectedFruits)
            {
                snake.SnakeLength++;
                fruits.Remove(fruit);
            }
            Thread.Sleep(20);
        }
    }
}