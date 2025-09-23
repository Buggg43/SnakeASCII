using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.Models
{
    public class Snake
    {
        public List<(int x, int y)> Segments { get; set; } = new List<(int x, int y)> ();
        public int SnakeLength { get; set; } = 1;
    }
}
