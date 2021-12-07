using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var startPositions = File
            .ReadAllLines("input")[0]
            .Split(',')
            .Select(i => int.Parse(i))
            .ToArray();

        Console.WriteLine("Part1 = " + MovePart1(startPositions));
        Console.WriteLine("Part2 = " + MovePart2(startPositions));
    }

    private static long MovePart1(int[] startPositions)
    {
        var min = startPositions.Min();
        var max = startPositions.Max();

        var costs = new long[max-min+1];

        // Simulate all the moves and add up their costs
        for (var i = min; i <= max; i++)
        {
            for(var j = 0; j < startPositions.Length; j++) {
                costs[i - min] += Math.Abs(i - startPositions[j]);
            }
        }

        return costs.Min();
    }

    private static long MovePart2(int[] startPositions)
    {
        var min = startPositions.Min();
        var max = startPositions.Max();

        var costs = new long[max - min + 1];

        // Simulate all the moves and add up their costs
        for (var i = min; i <= max; i++)
        {
            for (var j = 0; j < startPositions.Length; j++)
            {
                // Slow and inefficient
                var movement = Math.Abs(i - startPositions[j]);
                for (var s = 1; s <= movement; s++)
                    costs[i - min] += s;
            }
        }

        return costs.Min();
    }

}