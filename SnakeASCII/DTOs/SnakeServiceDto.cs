using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.DTOs
{
    public class SnakeServiceDto
    {
        public (int x, int y) NewHead { get;set; }
        public (int x, int y) PrevHead {get; set; }
        public (int x, int y)? AteFruitPosition { get; set; }
        //public bool? RemovedTail { get; set; }
        public (int x, int y)? OldTail { get; set; }
        public bool Collision { get; set; }
    }
}
