using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.Models
{
    public class PortalState
    {
        public bool IsActive { get; set; }
        public (int x, int y) EntryPos { get; set; }
        public (int x, int y) ExitPos { get; set; }
        public int TailFlushRemaining { get; set; }
    }
}
