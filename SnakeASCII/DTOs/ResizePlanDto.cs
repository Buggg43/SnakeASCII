using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.DTOs
{
    public class ResizePlanDto
    {
        public Board NewBoard { get; set; }
        public (int x, int y) NewHead { get; set; }
        public Direction NewDirection { get; set; }
        public List<(int x, int y)> NewSnakeSegments { get; set; }
        public int PendingGrowth { get; set; }
        public bool ClearFruits { get; set; }
        public bool PortalState { get; set; 
    }
}
