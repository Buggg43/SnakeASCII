using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.objects
{
    public class Fruits
    {
        public static int FruitId { get; set; }
        public string Name { get; set; } = $"Fruit {FruitId}";
        public int Score { get; set; } = FruitId / 2;
        public (int x, int y) Positions { get; set; }
        public bool ShowFruit { get; set; } = false;
    }
}
