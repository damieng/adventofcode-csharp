using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var parsedInput = File
            .ReadAllLines("input")
            .Select(i => int.Parse(i))
            .ToArray();

        Console.WriteLine("Part 1 = " + Part1(parsedInput));
        Console.WriteLine("Part 2 = " + Part2(parsedInput));
    }

    static int Part1(int[] input)
    {
        int? previous = null;
        int increased = 0;

        foreach (var depth in input)
        {
            if (previous != null && depth > previous)
                increased++;
            previous = depth;
        }

        return increased;
    }

    static int Part2(int[] input)
    {
        int? previous = null;
        int increased = 0;

        for (var i = 2; i < input.Length; i++)
        {
            var depth = input[i - 2] + input[i - 1] + input[i];
            if (previous != null && depth > previous)
                increased++;
            previous = depth;
        }

        return increased;
    }
}
