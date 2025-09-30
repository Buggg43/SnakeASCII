using SnakeASCII.DTOs;
using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.GameLogic
{
    public class Renderer
    {
        public void UpdateGamePanel((int x, int y) gameWindow, int playerPoints, int snakeLength, int playerMoves, int sameDirectionCount, (int x, int y) snakePosition)
        {
            int frameStartWidth = gameWindow.x / 5;
            int frameEndWidth = (gameWindow.x / 2) + (gameWindow.x / 5);
            var width = frameEndWidth - frameStartWidth - 1;

            ClearSpan(gameWindow.y + 2, frameStartWidth + 1, width);
            if (TrySetCursorPosition(frameStartWidth + 1, gameWindow.y + 2))
                Console.Write($"Player points: {playerPoints}, Fruits eaten: {snakeLength - 1}");
            ClearSpan(gameWindow.y + 3, frameStartWidth + 1, width);
            if (TrySetCursorPosition(frameStartWidth + 1, gameWindow.y + 3))
                Console.Write($"Player positionX: {snakePosition.x}, positionY: {snakePosition.y}");
            ClearSpan(gameWindow.y + 4, frameStartWidth + 1, width);
            if (TrySetCursorPosition(frameStartWidth + 1, gameWindow.y + 4))
                Console.Write($"Moves left for fruit to spawn: {playerMoves}");
            ClearSpan(gameWindow.y + 5, frameStartWidth + 1, width);
            if (TrySetCursorPosition(frameStartWidth + 1, gameWindow.y + 5))
                Console.Write($"Snake Length: {snakeLength}, Speed increase: {sameDirectionCount}");
        }
        public void DrawSnake(SnakeServiceDto dto)
        {
            Console.SetCursorPosition(dto.NewHead.x, dto.NewHead.y);
            Console.Write("@");
            Console.SetCursorPosition(dto.PrevHead.x, dto.PrevHead.y);
            Console.Write("O");
            if (dto.OldTail.HasValue)
            {
                Console.SetCursorPosition(dto.OldTail.Value.x, dto.OldTail.Value.y);
                Console.Write(" ");
            }
        }
        public void DrawSnakeFull(Snake snake)
        {
            Console.SetCursorPosition(snake.Segments[0].x, snake.Segments[0].y);
            Console.Write("@");
            for (int i = 1; i < snake.Segments.Count; i++)
            {
                Console.SetCursorPosition(snake.Segments[i].x, snake.Segments[i].y);
                Console.Write("O");
            }
        }
        public void DrawFruit((int x, int y) fruitPos)
        {
            if (TrySetCursorPosition(fruitPos.x, fruitPos.y)) ;
            Console.Write("*");
        }
        public void DrawField((int windowWidth, int windowHeight) gameWindow)
        {
            var maxW = Math.Min(Console.BufferWidth, gameWindow.windowWidth);
            var maxH = Math.Min(Console.BufferHeight, gameWindow.windowHeight);
            var safeW = Math.Min(gameWindow.windowWidth, Console.BufferWidth - 1);
            var safeH = Math.Min(gameWindow.windowHeight, Console.BufferHeight - 1);
            var length = Math.Clamp(gameWindow.windowWidth, 0, maxW - 1);
            if (TrySetCursorPosition(0, 0))
                Console.Write(new string('#', safeW + 1));
            for (int i = 1; i < safeH; i++)
            {
                if (TrySetCursorPosition(0, i))
                    Console.Write("#");

                ClearSpan(i, 1, safeW - 1);

                if (TrySetCursorPosition(safeW, i))
                    Console.Write("#");
            }
            if (TrySetCursorPosition(0, safeH))
                Console.Write(new string('#', safeW + 1));

            int panelTop = Math.Min(safeH + 1, Console.BufferHeight - 1);
            int panelBottom = Math.Min(panelTop + 5, Console.BufferHeight - 1);
            int panelLeft = Math.Clamp(gameWindow.windowWidth / 5, 0, safeW - 1);
            int panelRight = Math.Clamp((gameWindow.windowWidth / 2) + (gameWindow.windowWidth / 5), panelLeft + 1, safeW);

            if (TrySetCursorPosition(panelLeft, panelTop)) //top
                Console.Write(new string('#', panelRight - panelLeft + 1));
            for (int y = panelTop + 1; y < panelBottom; y++)
            {
                if (TrySetCursorPosition(panelLeft, y)) //left
                    Console.Write("#");
                ClearSpan(y, panelLeft + 1, panelRight - panelLeft - 1);
                if (TrySetCursorPosition(panelRight, y)) //right
                    Console.Write("#");
            }
            if (TrySetCursorPosition(panelLeft, panelBottom)) //bottom
                Console.Write(new string('#', panelRight - panelLeft + 1));
        }
        public void ShowGameOver((int x, int y) gameWindow)
        {
            Console.SetCursorPosition(gameWindow.x / 2 - 9, gameWindow.y / 2);
            Console.Write("###### YOU LOSE ######");
            Console.ReadKey();
        }
        public void ClearSpan(int y, int startX, int desiredLen)
        {
            var maxW = Math.Min(Console.BufferWidth, Console.WindowWidth);
            int availableWidth = maxW - startX;
            if (desiredLen <= 0 || availableWidth <= 0)
                return;

            var len = Math.Clamp(desiredLen, 0, maxW - startX);
            if (!TrySetCursorPosition(startX, y))
                return;

            Console.Write(new string(' ', len));
        }
        public bool TrySetCursorPosition(int x, int y)
        {
            if (x >= 0 && x < Console.BufferWidth && y >= 0 && y < Console.BufferHeight)
            {
                Console.SetCursorPosition(x, y);
                return true;
            }
            return false;
        }
        public void DrawPortal(PortalState portal)
        {
            if (!portal.IsActive) return;
            Console.SetCursorPosition(portal.EntryPos.x, portal.EntryPos.y);
            Console.Write('P'); 
            Console.SetCursorPosition(portal.ExitPos.x, portal.ExitPos.y);
            Console.Write('P');
        }
        public void ClearPortalMarkers(PortalState portal, Board board)
        {
            Console.SetCursorPosition(portal.EntryPos.x, portal.EntryPos.y);
            bool onVerticalWall = portal.EntryPos.x == 0 || portal.EntryPos.x == board.GameWindowWidth;
            bool onHorizontalWall = portal.EntryPos.y == 0 || portal.EntryPos.y == board.GameWindowHeight;
            if (onVerticalWall || onHorizontalWall) Console.Write('#'); else Console.Write(' ');

            Console.SetCursorPosition(portal.ExitPos.x, portal.ExitPos.y);
            Console.Write(' ');
        }

}
}
