using SnakeASCII.GameLogic;
using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.Services
{
    public class FruitService
    {
        public Fruit TrySpawnFruit(Snake snake, List<Fruit> fruits, Random rng, Board board)
        {
            bool emptySpot = false;
            (int x, int y) newFruit = (0, 0);
            while (emptySpot == false)
            {
                newFruit = (rng.Next(1, board.GameWindowWidth - 1), rng.Next(1, board.GameWindowHeight - 1));
                if (!fruits.Any(f => f.Positions == newFruit) && !snake.Segments.Any(s => s.x == newFruit.Item1 && s.y == newFruit.Item2))
                {
                    emptySpot = true;
                }
            }
            return new Fruit { Positions = newFruit };
        }
        public void RemoveFruit((int x, int y) pos, List<Fruit> fruits)
        {
            var fruitToRemove = fruits.FirstOrDefault(f => f.Positions == pos);
            if (fruitToRemove != null)
            {
                fruits.Remove(fruitToRemove);
            }
        }
        public void ResetAfterResize(List<Fruit> fruits)
        {
            fruits.Clear();
        }
    }
}
