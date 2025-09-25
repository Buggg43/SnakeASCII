using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.Models
{
    public enum Direction { Up, Down, Left, Right }
    public class GameState
    {
        public Direction LastDirection { get; set; }
        public Direction CurrentDirection { get; set; }
        public int SameDirectionCount { get; set; }
        public int PlayerPoints { get; set; }
        public int MovesForFruit { get; set; }
        public Random Rng { get; set; }
        public int FrameDelayMs { get; set; }
    }
}
