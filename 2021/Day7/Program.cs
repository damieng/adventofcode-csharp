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

    record Data(int count, long cost);

    private static long MovePart2(int[] startPositions)
    {
        var min = startPositions.Min();
        var max = startPositions.Max();

        var costs = new long[max - min + 1];

        var z = startPositions.GroupBy(g => g).ToDictionary(g => g.Key, new Data(g.Count(), 0));

        // Simulate all the moves and add up their costs
        for (var i = min; i <= max; i++)
        {
            foreach(var item in z)
            {
                long cost = 0;
                var movement = Math.Abs(i - startPositions[j]);
                for (var s = 1; s <= movement; s++)
                    cost += s;
                item.Value.Item2 = cost;
            }
        }



        return costs.Min();
    }

}