using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

record Display(string[] signals, string[] output);

class Program
{
    static void Main()
    {
        var map = File
            .ReadAllLines("input")
            .Select(i => i.Select(m => ((int)m - 48)).ToArray())
            .ToArray();

        Console.WriteLine("Part1 = " + Part1(map));
        Console.WriteLine("Part2 = " + Part2(map));
    }

    private static long Part1(int[][] grid)
    {
        var lowest = new List<int>();

        for (var y = 0; y < grid.Length; y++)
        {
            for (var x = 0; x < grid[y].Length; x++)
            {
                var point = grid[y][x];
                if ((y == 0 || grid[y - 1][x] > point) &&
                    (y == grid.Length - 1 || grid[y + 1][x] > point) &&
                    (x == 0 || grid[y][x - 1] > point) &&
                    (x == grid[y].Length - 1 || grid[y][x + 1] > point))
                {
                    lowest.Add(point);
                }
            }
        }

        return lowest.Select(i => i + 1).Sum();
    }

    private static long Part2(int[][] grid)
    {
        var highest = new List<int>();

        for (var y = 0; y < grid.Length; y++)
        {
            for (var x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] != 9)
                {
                    // Basically run a flood-fill
                    int size = 0;

                    var checking = new Queue<(int y, int x)>();
                    checking.Enqueue((y, x));

                    while (checking.Any())
                    {
                        var (py, px) = checking.Dequeue();
                        if (grid[py][px] != 9)
                        {
                            grid[py][px] = 9; // fill it
                            size++;
                            if (py > 0) checking.Enqueue((py - 1, px));
                            if (py < grid.Length - 1) checking.Enqueue((py + 1, px));
                            if (px > 0) checking.Enqueue((py, px - 1));
                            if (px < grid[y].Length - 1) checking.Enqueue((py, px + 1));
                        }
                    }

                    highest.Add(size);
                }
            }
        }

        var top = highest.OrderByDescending(h => h).Take(3).ToArray();
        return top[0] * top[1] * top[2];
    }
}