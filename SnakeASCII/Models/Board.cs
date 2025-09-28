using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.Models
{
    public class Board
    {
        public int GameWindowWidth { get; set; } 
        public int GameWindowHeight { get; set; }
        public bool IsActive { get; set; }
    }
}
