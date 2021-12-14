using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    record Pair(char Left, char Right);

    static void Main()
    {
        var lines = File.ReadAllLines("input");
        var start = lines[0];
        var rules = lines
            .Skip(2)
            .Select(l => l.Split(" -> "))
            .ToDictionary(p => new Pair(p[0][0], p[0][1]), p => p[1][0]);

        Console.WriteLine("Part1 = " + Calculate(start, rules, 10));
        Console.WriteLine("Part2 = " + Calculate(start, rules, 40));
    }

    static long Calculate(string start, Dictionary<Pair, char> rules, int steps)
    {
        var pairCounts = new Dictionary<Pair, long>();
        for (var i = 0; i < start.Length - 1; i++)
            Increase(pairCounts, new Pair(start[i], start[i + 1]), 1);

        for (var i = 0; i < steps; i++)
            foreach (var pair in pairCounts.ToArray())
            {
                pairCounts[pair.Key] -= pair.Value;
                var insertion = rules[pair.Key];
                Increase(pairCounts, new Pair(pair.Key.Left, insertion), pair.Value);
                Increase(pairCounts, new Pair(insertion, pair.Key.Right), pair.Value);
            }

        var elementCounts = new Dictionary<char, long>();

        // Count all the rights as each is included twice - once per side of a pair
        foreach (var pair in pairCounts)
            Increase(elementCounts, pair.Key.Right, pair.Value);

        // Count the first one one extra time as it has no left counterpart
        Increase(elementCounts, start[0], 1);

        var mostCommon = elementCounts.Values.Max();
        var leastCommon = elementCounts.Values.Min();

        return mostCommon - leastCommon;
    }

    static void Increase<TKey>(Dictionary<TKey, long> dictionary, TKey key, long amount)
    {
        dictionary[key] = dictionary.TryGetValue(key, out long existing) ? existing + amount : amount;
    }
}