using SnakeASCII.DTOs;
using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.Services
{
    public class SnakeService
    {
        public SnakeServiceDto Move(List<Fruit> fruits, Snake snake, Direction dir,Board board)
        {
            SnakeServiceDto snakeServiceDto = new SnakeServiceDto();
            snakeServiceDto.PrevHead = snake.Segments[0];
            Direction currentDirection = dir;
            (int dx, int dy) = currentDirection switch
            {
                Direction.Up => (0, -1),
                Direction.Down => (0, 1),
                Direction.Left => (-1, 0),
                Direction.Right => (1, 0),
                _ => (0, 0)
            };
            snakeServiceDto.NewHead = (snakeServiceDto.PrevHead.x + dx, snakeServiceDto.PrevHead.y + dy);
            if (snakeServiceDto.NewHead.x <= 0 || snakeServiceDto.NewHead.x >= board.GameWindowWidth ||
                snakeServiceDto.NewHead.y <= 0 || snakeServiceDto.NewHead.y >= board.GameWindowHeight)
            {
                snakeServiceDto.Collision = true;
                return snakeServiceDto;
            }
            var ateFruit = fruits.Skip(1).Any(f => f.Positions == snakeServiceDto.NewHead);
            var bodyToCheck = ateFruit ? snake.Segments : snake.Segments.Take(snake.Segments.Count - 1);
            if (bodyToCheck.Skip(1).Any(s => s == snakeServiceDto.NewHead))
            {
                snakeServiceDto.Collision = true;
                return snakeServiceDto;
            }
            if (!ateFruit)
            {
                snakeServiceDto.OldTail = snake.Segments.Last();
                snake.Segments.Remove(snake.Segments.Last());
            }
            else
            {
                snake.SnakeLength = snake.Segments.Count;
                snakeServiceDto.AteFruitPosition = snakeServiceDto.NewHead;
            }
            snake.Segments.Insert(0, snakeServiceDto.NewHead);
            return snakeServiceDto;
        }
        public int SnakeSpeed(int sameDirectionCount)
        {
            int minDelay = 20;
            int maxDelay = 300;
            var frameDelayMs = Math.Max(minDelay, maxDelay - (sameDirectionCount * 5));
            return frameDelayMs;
        }
    }
}
