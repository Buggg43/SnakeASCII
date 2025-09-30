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
        public int MinWidth { get; set; } = 60;
        public int MinHeight { get; set; } = 20;
        public int MarginX { get; set; } = 1;
        public int MarginY { get; set; } = 8;

        public bool ContainsInterior((int x, int y) p)
            => p.x >= 1 && p.x <= GameWindowWidth - 1
            && p.y >= 1 && p.y <= GameWindowHeight - 1;

        public (int x, int y) ClampToInterior((int x, int y) p)
            => (Math.Clamp(p.x, 1, GameWindowWidth - 1),
                Math.Clamp(p.y, 1, GameWindowHeight - 1));
        enum Side { None, Right, Left, Down, Up }
        Side side = Side.None;
        public (int x, int y) ClampToBorder((int x, int y) p)
        {
            int W = GameWindowWidth;
            int H = GameWindowHeight;

            int overRight = p.x - (W - 1);
            int overLeft = 1 - p.x;
            int overDown = p.y - (H - 1);
            int overUp = 1 - p.y;
            int maxOver = 0;


            void Consider(int over, Side s)
            {
                if (over > maxOver) { maxOver = over; side = s; }
            }

            Consider(overRight, Side.Right);
            Consider(overLeft, Side.Left);
            Consider(overDown, Side.Down);
            Consider(overUp, Side.Up);

            if (side == Side.None)
                return ClampToInterior(p);

            return side switch
            {
                Side.Right => (W, Math.Clamp(p.y, 1, H - 1)),
                Side.Left  => (0, Math.Clamp(p.y, 1, H - 1)),
                Side.Down  => (Math.Clamp(p.x, 1, W - 1), H),
                _          => (Math.Clamp(p.x, 1, W - 1), 0), // Up
            };
        }       
    }
}