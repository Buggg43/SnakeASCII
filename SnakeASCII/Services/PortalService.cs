using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.Services
{
    public class PortalService
    {
        // Teleportuje głowę do centrum, buduje ciało "za głową", ustawia PendingRegrow i zwraca (newDir, portal)
        public (Direction newDirection, PortalState portal) PlanToCenter(Board board, Snake snake, Direction lastDir)
        {
            var oldHead = snake.Segments[0];

            // Jeśli głowa jest już wewnątrz, nie robimy portalu.
            if (board.ContainsInterior(oldHead))
                return (lastDir, new PortalState { IsActive = false });

            // 1) Wejście portalu na ramce (EntryPos)
            var entry = board.ClampToBorder(oldHead);

            // 2) Wyjście w centrum wnętrza (ExitPos)
            var exit = (board.GameWindowWidth / 2, board.GameWindowHeight / 2); // deterministyczny "lewodół" środka

            // 3) Kierunek po teleportacji: w stronę osi z większą różnicą Entry->Exit
            int dx = exit.Item1 - entry.Item1;
            int dy = exit.Item2 - entry.Item2;
            Direction newDir = Math.Abs(dx) >= Math.Abs(dy)
                ? (dx >= 0 ? Direction.Right : Direction.Left)
                : (dy >= 0 ? Direction.Down : Direction.Up);

            // 4) Rekonstrukcja ciała: głowa = exit, reszta "za głową" w linii prostej, ile się zmieści
            int oldLen = snake.Segments.Count;
            var newBody = new List<(int x, int y)>(oldLen) { exit };

            (int sx, int sy) backStep = newDir switch
            {
                Direction.Right => (-1, 0),
                Direction.Left => (1, 0),
                Direction.Down => (0, -1),
                _ => (0, 1), // Up
            };

            var cur = exit;
            while (newBody.Count < oldLen)
            {
                var next = (cur.Item1 + backStep.sx, cur.Item2 + backStep.sy);
                if (!board.ContainsInterior(next)) break;
                newBody.Add(next);
                cur = next;
            }

            int pendingRegrow = Math.Max(0, oldLen - newBody.Count);

            snake.Segments = newBody;
            snake.PendingGrowth = pendingRegrow;

            // 5) Portal "po obu stronach" – wygaszamy gdy ogon "przepłynie" cały
            int tailFlush = (newBody.Count - 1) + pendingRegrow;

            var portal = new PortalState
            {
                IsActive = true,
                EntryPos = entry,
                ExitPos = exit,
                TailFlushRemaining = tailFlush
            };

            return (newDir, portal);
        }
    }
}
