using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var octo = File
            .ReadAllLines("input")
            .Select(o => o.Select(j => j - 48).ToArray())
            .ToArray();

        var (flashCount, allFlashedAt) = CalculateOcto(octo);

        Console.WriteLine("Part1 = " + flashCount);
        Console.WriteLine("Part2 = " + allFlashedAt);
    }

    static (long, int) CalculateOcto(int[][] octo)
    {
        long flashesInFirst100 = 0;
        int allFlashedAt = -1;

        int step = 1;
        do
        {
            // Increase
            for (var y = 0; y < octo.Length; y++)
            {
                for (var x = 0; x < octo[y].Length; x++)
                {
                    octo[y][x]++;
                }
            }

            // Calculate flashes
            bool stillFlashing;
            var flashed = new bool[octo.Length, octo[0].Length];
            int stepFlashCount = 0;
            do
            {
                stillFlashing = false;
                for (var y = 0; y < octo.Length; y++)
                {
                    for (var x = 0; x < octo[y].Length; x++)
                    {
                        if (octo[y][x] > 9 && !flashed[y, x])
                        {
                            flashed[y, x] = true;
                            if (step <= 100)
                                flashesInFirst100++;
                            stepFlashCount++;
                            stillFlashing = true;
                            IncreaseSurrounding(octo, y, x);
                        }
                    }
                }
            }
            while (stillFlashing);

            if (allFlashedAt == -1 && stepFlashCount == octo.Length * octo[0].Length)
                allFlashedAt = step;

            // Reset
            for (var y = 0; y < octo.Length; y++)
            {
                for (var x = 0; x < octo[y].Length; x++)
                {
                    if (flashed[y, x]) octo[y][x] = 0;
                }
            }

            step++;
        }
        while (allFlashedAt == -1);

        return (flashesInFirst100, allFlashedAt);
    }

    static void IncreaseSurrounding(int[][] octopuses, int y, int x)
    {
        var leftEdge = x == 0;
        var topEdge = y == 0;
        var rightEdge = x == octopuses[y].Length - 1;
        var bottomEdge = y == octopuses.Length - 1;

        if (!leftEdge)
        {
            if (!topEdge) octopuses[y - 1][x - 1]++;
            octopuses[y][x - 1]++;
            if (!bottomEdge) octopuses[y + 1][x - 1]++;
        }
        if (!rightEdge)
        {
            if (!topEdge) octopuses[y - 1][x + 1]++;
            octopuses[y][x + 1]++;
            if (!bottomEdge) octopuses[y + 1][x + 1]++;
        }
        if (!topEdge) octopuses[y - 1][x]++;
        if (!bottomEdge) octopuses[y + 1][x]++;
    }
}